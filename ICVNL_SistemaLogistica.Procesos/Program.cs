using ICVNL_SistemaLogistica.Procesos.Helpers;
using ICVNL_SistemaLogistica.Procesos.Service;
using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Procesos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                MainAsync(args).GetAwaiter().GetResult();
            }
            else
            {
                if (args[0].ToString() == "NotificacionPlacasNoInventariadas")
                {
                    var idEntidad = 0;
                    if (GetEntidadGobiernoParametrizacion().ExecutionOK)
                    {
                        idEntidad = GetEntidadGobiernoParametrizacion().Data;
                        var responseEnviaNotificacion = EnviaNotificacion_PlacaNoInventariada(idEntidad);
                        if (responseEnviaNotificacion.ExecutionOK)
                        {
                            System.Console.WriteLine("Proceso realizado con exito");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                    }
                }
                else
                {
                    MainAsync(args).GetAwaiter().GetResult();
                }
            }
        }
        
        public static DBResponse<string> EnviaNotificacion_PlacaNoInventariada(int Entidad)
        {
            var dbResponse = new DBResponse<string>();
            try
            {
                System.Console.WriteLine("Obteniendo las placas vendidas/entregadas no inventariadas");

                var getPlacasNoInv = new InventarioPlacas_BL().GetPlacasNoInventariadas_EnviaNotificacion(Entidad);
                if (getPlacasNoInv.ExecutionOK)
                {
                    var parametrizacion_ = new Parametrizacion_BL().GetParametrizacion();

                    foreach (var item in getPlacasNoInv.Data)
                    {
                        System.Console.WriteLine("Generando notificación para enviar a sus destinatarios");

                        StringBuilder body = new StringBuilder();
                        body.Append("Placas Vendidas que no han sido recibidas en este Sistema de Logística de Control de Placas <br/>");
                        body.Append("Número de Placa: <b>" + item.NumeroPlaca + "</b><br/>");
                        body.Append("Delegación en la que se vendió:  <b>" + item.DelegacionesBancos.NombreDelegacionBanco + "</b><br/>"); 
                        body.Append("Fecha y hora de pago: <b>" + item.FechaOperacionVenta.ToString("dd/MM/yyyy hh:mm:ss tt") + "</b><br/>");
                        body.Append("</b><br/>");

                        var infoEmailServer = InformationAccountEmail.GetInfoAccountEmail();
                        var envioMail = new EnvioEmail()
                        {
                            SubjectEmail = "Placas Vendidas que no han sido recibidas",
                            BodyEmail = body.ToString(),
                            DeliveryRecipient = false,
                            ReadRecipient = false,
                            EmailEnvia = parametrizacion_.Data.EmailDestinatariosNotificaPlacasVendidas,
                            ArchivosAdjuntos = new List<ArchivosAdjuntos>()
                    
                        };
                        var responseCorreo = Correo.EnviaCorreo(envioMail, infoEmailServer);
                        if (responseCorreo.ExecutionOK)
                        {
                            var updateMarcaNotEnviada = new InventarioPlacas_BL().UpdatePlacaSinInventariar_MarcaNotifEnviada(item);
                            System.Console.WriteLine("Notificación de la placa " + item.NumeroPlaca + " no inventariada enviada correctamente");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return dbResponse;
        }
        public static async Task MainAsync(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    System.Console.WriteLine("No hay parametros de entrada, Verifique.");
                    Console.Read();
                }
                else
                {
                    if (args[0].ToString() == "CargaOrdenesCompra")
                    {
                        System.Console.WriteLine("Iniciando con la Carga de las Ordenes de Compra.");

                        await CargaOrdenesCompra();
                    }
                    else if (args[0].ToString() == "CargaNotasEntrada")
                    {
                        await CargaNotasEntrada();
                    }
                    else if (args[0].ToString() == "PolizaPlacasVendidas")
                    {
                        await GeneraInfoPolizaPlacasVendidas();
                    }
                    else if (args[0].ToString() == "PolizaPlacasNoContabilizadas")
                    {
                        await GeneraInfoPolizaPlacasNoContabilizadas();
                    }
                }               
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ocurrio un error, Detalle: " + ex.Message);
                throw ex;
            }
        }

        public static async Task GeneraInfoPolizaPlacasVendidas()
        {
            try
            {
                var idEntidad = 0;
                if (GetEntidadGobiernoParametrizacion().ExecutionOK)
                {
                    idEntidad = GetEntidadGobiernoParametrizacion().Data;
                }
                else
                {
                    System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                    return;
                }

                var URL_BaseApi = ConfigurationManager.AppSettings["URL_BaseApi"].ToString();
                var usuarioApi = new Usuarios_API_BL().GetUserAPI();
                if (usuarioApi.ExecutionOK)
                {
                    var getTokenApi = await Task.Run(() => GetToken(usuarioApi.Data.Usuario, usuarioApi.Data.Password, URL_BaseApi));
                    if (!getTokenApi.ExecutionOK)
                    {
                        System.Console.WriteLine("No se pudo obtener el token. Detalle :" + getTokenApi.Message);

                        return;
                    }

                    var _postPoliza = new PostPolizas()
                    {
                        AccessToken = getTokenApi.Data.infofin_token,
                        Poliza = new Web.Entities.Services.Envio.Poliza()
                        {
                            descripcion = "Poliza Placas Vendidas",
                            empresa = idEntidad,
                            tipo_poliza = 3,
                            origen = "PLA",
                            usuario_poliza = int.Parse(getTokenApi.Data.infofin_usuario),
                            asientos = new List<Web.Entities.Services.Envio.Asiento>()
                        }
                    };

                    var infoPlacasVendidas = new Polizas_BL().GetPoliza_PlacasVendidasNoContabilizadas(idEntidad);
                    if (infoPlacasVendidas.ExecutionOK)
                    {
                        System.Console.WriteLine("Obteniendo información de las Placas Vendidas no contabilizadas");
                        var parametrizacion = new Parametrizacion_BL().GetParametrizacion();
                        if (parametrizacion.ExecutionOK)
                        {
                            if (parametrizacion.Data.CuentaCostosVentaPlacaVendida.Length > 0 && 
                                parametrizacion.Data.CentroCostosEntidadGobiernoPlacaVendida.Length > 0)
                            {
                                var newListInfo = infoPlacasVendidas.Data.GroupBy(x => new { x.CuentaContableCargo, x.CentroCostosAlmacen })
                                                                         .Select(
                                                                                    w => new Placas_Polizas_Sum
                                                                                    {
                                                                                        CentroCostosAlmacen = w.FirstOrDefault().CentroCostosAlmacen,
                                                                                        CuentaContableCargo = w.FirstOrDefault().CentroCostosAlmacen,
                                                                                        ImportePlacas = w.Sum(s => s.ImportePlacas)
                                                                                    }
                                                                                )
                                                                         .ToList();

                                foreach (var item in newListInfo)
                                {
                                    //agregamos el asiento del Cargo
                                    _postPoliza.Poliza.asientos.Add(new Web.Entities.Services.Envio.Asiento()
                                    {
                                        centro_costos = parametrizacion.Data.CentroCostosEntidadGobiernoPlacaVendida,
                                        cuenta = parametrizacion.Data.CuentaCostosVentaPlacaVendida,
                                        descripcion = "Asiento contable de cargo de las placas Vendidas",
                                        monto = item.ImportePlacas,
                                        tipo_cambio = 1.00M,
                                        tipo = -1
                                    });
                                    //Agrgamos el asiento de la acreditacion
                                    _postPoliza.Poliza.asientos.Add(new Web.Entities.Services.Envio.Asiento()
                                    {
                                        centro_costos = item.CentroCostosAlmacen,
                                        cuenta = item.CuentaContableCargo,
                                        descripcion = "Acreditación de placas vendidas",
                                        monto = item.ImportePlacas,
                                        tipo_cambio = 1.00M,
                                        tipo = 1
                                    });
                                }
                            }
  

                            if (_postPoliza.Poliza.asientos.Count > 0)
                            {
                                var getOrdenesCompraApi = await Task.Run(() => ApiService.ApiPostPolizasRequest<NotasEntradas>(URL_BaseApi,
                                                                                                                            "Api/Poliza/",
                                                                                                                            "AgregaPoliza",
                                                                                                                            getTokenApi.Data.access_token,
                                                                                                                            _postPoliza));
                                if (getOrdenesCompraApi.ExecutionOK)
                                {
                                    foreach (var item in infoPlacasVendidas.Data)
                                    {
                                        var actualizaPlacaContabilizada = new InventarioPlacas_BL().InventariosPlacas_ActualizaPlacaContabilizada(item.NumeroPlaca, idEntidad);
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ocurrio un error al generar la información de las polizas de placas vendidas. Detalle :" + ex.Message);
                throw ex;
            }
        }

        public static async Task GeneraInfoPolizaPlacasNoContabilizadas()
        {
            try
            {
                var idEntidad = 0;
                if (GetEntidadGobiernoParametrizacion().ExecutionOK)
                {
                    idEntidad = GetEntidadGobiernoParametrizacion().Data;
                }
                else
                {
                    System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                    return;
                }

                var URL_BaseApi = ConfigurationManager.AppSettings["URL_BaseApi"].ToString();
                var usuarioApi = new Usuarios_API_BL().GetUserAPI();
                if (usuarioApi.ExecutionOK)
                {
                    var getTokenApi = await Task.Run(() => GetToken(usuarioApi.Data.Usuario, usuarioApi.Data.Password, URL_BaseApi));
                    if (!getTokenApi.ExecutionOK)
                    {
                        System.Console.WriteLine("No se pudo obtener el token. Detalle :" + getTokenApi.Message);

                        return;
                    }

                    var _postPoliza = new PostPolizas()
                    {
                        AccessToken = getTokenApi.Data.infofin_token,
                        Poliza = new Web.Entities.Services.Envio.Poliza()
                        {
                            descripcion = "Poliza Placas Dañadas, Perdidas, Obsoletas y Destruidas",
                            empresa = idEntidad,
                            tipo_poliza = 3,
                            origen = "PLA",
                            usuario_poliza = int.Parse(getTokenApi.Data.infofin_usuario),
                            asientos = new List<Web.Entities.Services.Envio.Asiento>()
                        }
                    };

                    var infoPlacasVendidas = new Polizas_BL().GetPoliza_PlacasNoContabilizadas(idEntidad);
                    if (infoPlacasVendidas.ExecutionOK)
                    {
                        System.Console.WriteLine("Obteniendo información de las Placas Dañadas, Perdidas, Obsoletas y Destruida");
                        var parametrizacion = new Parametrizacion_BL().GetParametrizacion();
                        if (parametrizacion.ExecutionOK)
                        {
                            if (parametrizacion.Data.CuentaCostosVentaPlacaVendida.Length > 0 &&
                                parametrizacion.Data.CentroCostosEntidadGobiernoPlacaVendida.Length > 0)
                            {
                                var newListInfo = infoPlacasVendidas.Data.GroupBy(x => new { x.CuentaContableCargo, x.CentroCostosAlmacen })
                                                                         .Select(
                                                                                    w => new Placas_Polizas_Sum
                                                                                    {
                                                                                        CentroCostosAlmacen = w.FirstOrDefault().CentroCostosAlmacen,
                                                                                        CuentaContableCargo = w.FirstOrDefault().CentroCostosAlmacen,
                                                                                        ImportePlacas = w.Sum(s => s.ImportePlacas)
                                                                                    }
                                                                                )
                                                                         .ToList();

                                foreach (var item in newListInfo)
                                {
                                    //agregamos el asiento del Cargo
                                    _postPoliza.Poliza.asientos.Add(new Web.Entities.Services.Envio.Asiento()
                                    {
                                        centro_costos = parametrizacion.Data.CentroCostosEntidadGobiernoPlacaVendida,
                                        cuenta = parametrizacion.Data.CuentaCostosVentaPlacaVendida,
                                        descripcion = "Asiento contable de cargo de las placas Dañadas, Perdidas, Obsoletas y Destruida",
                                        monto = item.ImportePlacas,
                                        tipo_cambio = 1.00M,
                                        tipo = -1
                                    });
                                    //Agrgamos el asiento de la acreditacion
                                    _postPoliza.Poliza.asientos.Add(new Web.Entities.Services.Envio.Asiento()
                                    {
                                        centro_costos = item.CentroCostosAlmacen,
                                        cuenta = item.CuentaContableCargo,
                                        descripcion = "Acreditación de placas Dañadas, Perdidas, Obsoletas y Destruida",
                                        monto = item.ImportePlacas,
                                        tipo_cambio = 1.00M,
                                        tipo = 1
                                    });
                                }
                            }


                            if (_postPoliza.Poliza.asientos.Count > 0)
                            {
                                var getOrdenesCompraApi = await Task.Run(() => ApiService.ApiPostPolizasRequest<NotasEntradas>(URL_BaseApi,
                                                                                                                            "Api/Poliza/",
                                                                                                                            "AgregaPoliza",
                                                                                                                            getTokenApi.Data.access_token,
                                                                                                                            _postPoliza));
                                if (getOrdenesCompraApi.ExecutionOK)
                                {
                                    foreach (var item in infoPlacasVendidas.Data)
                                    {
                                        var actualizaPlacaContabilizada = new InventarioPlacas_BL().InventariosPlacas_ActualizaPlacaContabilizada(item.NumeroPlaca, idEntidad);
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ocurrio un error al generar la información de las polizas de placas vendidas. Detalle :" + ex.Message);
                throw ex;
            }
        }

        public static async Task CargaOrdenesCompra()
        {
            try
            {
                var idEntidad = 0;
                if (GetEntidadGobiernoParametrizacion().ExecutionOK)
                {
                    idEntidad = GetEntidadGobiernoParametrizacion().Data;
                }
                else
                {
                    System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                    return;
                }

                var URL_InventariosApi = ConfigurationManager.AppSettings["URL_InventariosApi"].ToString();
                var usuarioApi = new Usuarios_API_BL().GetUserAPI();
                if (usuarioApi.ExecutionOK)
                {
                    var getTokenApi = await Task.Run(() => GetToken(usuarioApi.Data.Usuario, usuarioApi.Data.Password, URL_InventariosApi));
                    if (!getTokenApi.ExecutionOK)
                    {
                        System.Console.WriteLine("No se pudo obtener el token. Detalle :" + getTokenApi.Message);

                        return;
                    }

                    var getInfoOrdenesCompra = await Task.Run(() => GetOrdenesCompra(getTokenApi.Data, URL_InventariosApi));
                    if (getInfoOrdenesCompra.ExecutionOK)
                    {
                        foreach (var InsertOC in getInfoOrdenesCompra.Data)
                        {
                            System.Console.WriteLine("Insertando la Orden de Compra: " + InsertOC.NumeroOrdenCompra);
                            var dbInsert = new OrdenesCompra_BL().InsertOrdenesCompra(InsertOC, new Usuarios()
                                                                                                {
                                                                                                    Usuario = usuarioApi.Data.Usuario,
                                                                                                    Entidad = idEntidad
                                                                                                 });

                            System.Console.WriteLine("Insertando la Orden de Compra: " + dbInsert.Message);

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ocurrio un error al obtener la información de las Ordenes de Compra. Detalle :" + ex.Message);
                throw ex;
            }
        }
        public static async Task CargaNotasEntrada()
        {
            try
            {
                System.Console.WriteLine("Obteniendo el valor de la Entidad Gobierno Parametrizada");
                var idEntidad = 0;
                if (GetEntidadGobiernoParametrizacion().ExecutionOK)
                {
                    idEntidad = GetEntidadGobiernoParametrizacion().Data;
                }
                else
                {
                    System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                    return;
                }
                var URL_InventariosApi = ConfigurationManager.AppSettings["URL_InventariosApi"].ToString();
                var usuarioApi = new Usuarios_API_BL().GetUserAPI();
                if (usuarioApi.ExecutionOK)
                {
                    var getTokenApi = await Task.Run(() => GetToken(usuarioApi.Data.Usuario, usuarioApi.Data.Password, URL_InventariosApi));
                    if (!getTokenApi.ExecutionOK)
                    {
                        System.Console.WriteLine("No se pudo obtener el token. Detalle :" + getTokenApi.Message);

                        return;
                    }

                    var getInfoNotaentrada = await Task.Run(() => GetNotasEntradasPlacas(getTokenApi.Data, URL_InventariosApi));
                    if (getInfoNotaentrada.ExecutionOK)
                    {
                        foreach (var InsertNE in getInfoNotaentrada.Data)
                        {
                            System.Console.WriteLine("Insertando la nota de entrada: " + InsertNE.NumeroNotaEntrada);
                            var dbInsert = new NotasEntradasPlacas_BL().UpsertNotasEntradasPlaca(InsertNE, idEntidad, new Usuarios()
                                                                                                                        {
                                                                                                                            Usuario = usuarioApi.Data.Usuario,
                                                                                                                            Entidad = idEntidad
                                                                                                                        });
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                System.Console.WriteLine("Ocurrio un error al obtener la información de las notas de entrada. Detalle :" + ex.Message);
                throw ex;
            }
        }
        public static async Task<DBResponse<Token>> GetToken(string usuario, string password, string UrlBase)
        {
            var tokenResponse = new DBResponse<Token>();
            ApiBaseLogin apiBaseLoginInventario = new ApiBaseLogin()
            {
                Controlador = "",
                UrlBase = UrlBase,
                Prefijo = "Token",
                PasswordApi = password,
                UsuarioApi = usuario
            };

            var tokenInventarios = await Task.Run(() => ApiService.PostLoginObj<Token>(apiBaseLoginInventario));
            if (!tokenInventarios.ExecutionOK)
            {
                tokenResponse.ExecutionOK = false;
                tokenResponse.Message = "Error en comunicaciónes hacía ICVNL. Detalles: " + tokenInventarios.Message;
                return tokenResponse;
            }

            tokenResponse.Data = tokenInventarios.Data;
            tokenResponse.ExecutionOK = true;

            return tokenResponse;
        }

        public static async Task<DBResponse<List<Almacenes>>> GetAlmacenes(Token token, string UrlBase)
        {
            var almacneResponse = new DBResponse<List<Almacenes>>();
            var _postAlmacen = new PostAlmacenesListado()
            {
                AccessToken = token.infofin_token,
                Almacen = new AlmacenesListado
                {
                    empresa = 1
                }
            };

            var objAlmacen = new Almacenes();

            var getAlmacenesApi = await Task.Run(() => ApiService.ApiPostAlmacenesListadoRequest<ResponseListadoAlmacenes>(UrlBase,
                                                                                  "Api/Almacenes/",
                                                                                  "ListaAlmacenes",
                                                                                  token.access_token,
                                                                                  _postAlmacen));
            if (getAlmacenesApi.ExecutionOK)
            {
                almacneResponse.Data = ((ResponseListadoAlmacenes)getAlmacenesApi.Data).Almacenes;
                almacneResponse.ExecutionOK = true;
            }
            return almacneResponse;
        }
        public static async Task<DBResponse<List<NotasEntradasPlacas>>> GetNotasEntradasPlacas(Token token, string URL_InventariosApi)
        {
            var idEntidad = 0;
            if (GetEntidadGobiernoParametrizacion().ExecutionOK)
            {
                idEntidad = GetEntidadGobiernoParametrizacion().Data;
            }
            else
            {
                System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                return new DBResponse<List<NotasEntradasPlacas>>();
            }
            var delegacionesBanco = new DelegacionesBancos_BL().GetDelegacionesBancos(1, idEntidad);

            System.Console.WriteLine("Obteniendo información de los almacenes");

            var getAlmacenes = await Task.Run(() => GetAlmacenes(token, URL_InventariosApi));
            if (!getAlmacenes.ExecutionOK)
            {
                return new DBResponse<List<NotasEntradasPlacas>>();
            } 

            var notasEntradasPlacas = new DBResponse<List<NotasEntradasPlacas>>();
            notasEntradasPlacas.Data = new List<NotasEntradasPlacas>();
            var _postNotasEntrada = new PostNotaEntrada()
            {
                AccessToken = token.infofin_token,
                NotaEntrada = new NotaEntrada()
                {
                    empresa = 1,
                    fecha_entrada_inicial = DateTime.Now.AddYears(-1),
                    fecha_entrada_final = DateTime.Now
                }
            };

            System.Console.WriteLine("Obteniendo información de las Notas de Entrada");

            var getNotasentradasApi = await Task.Run(() => ApiService.ApiPostNotasEntradasRequest<NotasEntradas>(URL_InventariosApi,
                                                                                                                  "Api/NotaEntrada/",
                                                                                                                  "ObtieneNotasEntrada",
                                                                                                                  token.access_token,
                                                                                                                  _postNotasEntrada));

            if (getNotasentradasApi.ExecutionOK)
            {
                var objListApi = ((ResponseNotasEntradas)getNotasentradasApi.Data).NotasEntradas;
                if (objListApi != null)
                {
                    foreach (var itemNE in objListApi)
                    {
                        var objEntrada = new NotasEntradasPlacas();
                        objEntrada += itemNE;
                        if (getAlmacenes.Data.Any(x => x.almacen == itemNE.almacen))
                        {
                            var centroCostos_ = getAlmacenes.Data.Where(x => x.almacen == itemNE.almacen).FirstOrDefault().centro_costo;
                            var dbresponseDelegacion = new DelegacionesBancos_BL().ExisteDelegacionBancoByCentroCostos(centroCostos_, idEntidad);
                            if (dbresponseDelegacion.ExecutionOK)
                            {
                                objEntrada.IdDelegacionBanco = dbresponseDelegacion.Data.IdDelegacionBanco;
                            }
                        }
                        notasEntradasPlacas.Data.Add(objEntrada);
                    }
                }
            }

            return notasEntradasPlacas;
        }

        public static async Task<DBResponse<List<Web.Entities.OrdenesCompra>>> GetOrdenesCompra(Token token, string URL_InventariosApi)
        {
            var idEntidad = 0;
            if (GetEntidadGobiernoParametrizacion().ExecutionOK)
            {
                idEntidad = GetEntidadGobiernoParametrizacion().Data;
            }
            else
            {
                System.Console.WriteLine("Error al obtener el valor de la Entidad Gobierno, Detalle: " + GetEntidadGobiernoParametrizacion().Message);
                return new DBResponse<List<Web.Entities.OrdenesCompra>>();
            } 

            var dBResponseOC = new DBResponse<List<Web.Entities.OrdenesCompra>>();
            dBResponseOC.Data = new List<Web.Entities.OrdenesCompra>();
            var _postOrdenesCompra = new PostOrdenCompra()
            {
                AccessToken = token.infofin_token,
                OrdenCompra = new OrdenCompra()
                {
                    empresa = 1, 
                    fecha = DateTime.Now.AddDays(-365)
                }
            };

            System.Console.WriteLine("Obteniendo información de las Ordenes de Compra");

            var getOrdenesCompraApi = await Task.Run(() => ApiService.ApiPostOrdenesCompraRequest<NotasEntradas>(URL_InventariosApi,
                                                                                                                  "Api/OrdenCompra/",
                                                                                                                  "ObtieneOrdenesCompra",
                                                                                                                  token.access_token,
                                                                                                                  _postOrdenesCompra));

            if (getOrdenesCompraApi.ExecutionOK)
            {
                var objListApi = ((ResponseOrdenesCompra)getOrdenesCompraApi.Data).OrdenesCompra;
                foreach (var item in objListApi)
                {
                    var objProveedor = new Proveedores_BL().GetProveedorByNumero(item.proveedor, idEntidad);
                    if (objProveedor.ExecutionOK)
                    {
                        var objOrdenes = new Web.Entities.OrdenesCompra();
                        objOrdenes += item;
                        objOrdenes.IdProveedor = objProveedor.Data.Id;
                        dBResponseOC.Data.Add(objOrdenes);
                    }
                }

                if (dBResponseOC.Data.Count > 0)
                {
                    dBResponseOC.ExecutionOK = true;
                }
            }

            return dBResponseOC;
        }

        public static DBResponse<int> GetEntidadGobiernoParametrizacion()
        {
            var dBResponse = new DBResponse<int>();
            int EntidadGobierno = 0;
            var parametrizacion_ = new Parametrizacion_BL().GetParametrizacion();
            if (parametrizacion_.ExecutionOK)
            {
                if (String.IsNullOrEmpty(parametrizacion_.Data.ClaveEntidadGobierno))
                {
                    dBResponse.ExecutionOK = false;
                    dBResponse.Data = 0;
                    dBResponse.Message = "No se pudo obtener la clave entidad gobierno parametrizada del Sistema Lógistica, Contacte al administrador";
                    dBResponse.NumRows = 0;

                    return dBResponse;
                }
                bool esNumerico = Int32.TryParse(parametrizacion_.Data.ClaveEntidadGobierno, out EntidadGobierno);
                if (!esNumerico)
                {
                    dBResponse.ExecutionOK = false;
                    dBResponse.Data = 0;
                    dBResponse.Message = "la clave entidad gobierno no es un númerico válido, Contacte al administrador";
                    dBResponse.NumRows = 0;
                    return dBResponse;

                }

                dBResponse.Data = EntidadGobierno;
                dBResponse.ExecutionOK = true;
            }
            return dBResponse;
        }
    }
}

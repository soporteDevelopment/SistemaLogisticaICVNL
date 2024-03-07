using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.RolesPermisos;
using ICVNL_SistemaLogistica.Web.Entities.Services;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using ICVNL_SistemaLogistica.Web.Services;
using ICVNL_SistemaLogistica.Web.ViewModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Usuario = ICVNL_SistemaLogistica.Web.Entities.Services.Envio.Usuario;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            Session["UserSC"] = null;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(InicioSesionVM user)
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (ModelState.IsValid)
            {
                ApiBaseLogin apiBaseLogin = new ApiBaseLogin()
                {
                    Controlador = "",
                    UrlBase = Helper.InfoApi.GetURLApi(),
                    Prefijo = "Token",
                    PasswordApi = user.Contrasena,
                    UsuarioApi = user.Usuario
                };

                //Realizamos el Login para obtener el token del usuario y posteriormente buscar la información
                //de ese usuario

                var login = await Task.Run(() => ApiService.PostLoginObj<Token>(apiBaseLogin));
                if (login.ExecutionOK)
                {
                    ApiBaseLogin apiBaseLoginInventario = new ApiBaseLogin()
                    {
                        Controlador = "",
                        UrlBase = Helper.InfoApi.GetURL_InventariosApi(),
                        Prefijo = "Token",
                        PasswordApi = user.Contrasena,
                        UsuarioApi = user.Usuario
                    };
                    var tokenInventarios = await Task.Run(() => ApiService.PostLoginObj<Token>(apiBaseLoginInventario));
                    if (!tokenInventarios.ExecutionOK)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    var usuario = new Usuarios();
                    usuario.Token = new Token();
                    usuario.Token = (Token)login.Data;
                    usuario.TokenInventario = (Token)tokenInventarios.Data;

                    var _postUsuario = new PostUsuario()
                    {
                        AccessToken = usuario.Token.infofin_token,
                        Usuario = new Usuario()
                        {
                            clave = int.Parse(usuario.Token.infofin_usuario)
                        }
                    };

                    var getUsuarioApi = await Task.Run(() => ApiService.ApiPostUsuarioRequest<ResponseUsuario>(Helper.InfoApi.GetURLApi(),
                                                                                          "Api/Usuario/",
                                                                                          "ObtieneUsuario",
                                                                                          usuario.Token.access_token,
                                                                                          _postUsuario));
                    if (getUsuarioApi.ExecutionOK)
                    {
                        string ip = GetIPAddress(Request);
                        var objUsuarioApi = ((ResponseUsuario)getUsuarioApi.Data).usuario;
                        usuario.ClaveUsuario = int.Parse(objUsuarioApi.clave.ToString());
                        usuario.Usuario = objUsuarioApi.nombre_usuario;
                        usuario.Nombre = objUsuarioApi.nombre;
                        usuario.IP_Usuario = ip;
                        usuario.NumeroEmpleado = objUsuarioApi.numero_empleado;
                        usuario.Entidad = 1;
                        usuario.UsuariosPermisos = new UsuariosPermisos();
                       //Inicia permisos de la aplicación
                        var taskPermisos = await Task.Run(() => GetUsuariosPermisos(usuario) );
                        if (taskPermisos.ExecutionOK)
                        {
                            usuario.UsuariosPermisos = taskPermisos.Data;
                        }

                        var taskDelegaciones = await Task.Run(() => GetDelegacionesBancosRelUsuario(usuario));
                        if (taskPermisos.ExecutionOK)
                        {
                            usuario.ListadoDelegacionesBancos = taskDelegaciones.Data;
                        }


                        //Guarda en Bitacora el Login
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = "Select",
                            FechaEvento = DateTime.Now,
                            Evento = "Ingresar",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Login",
                            JsonObject = JsonConvert.SerializeObject(user),
                            Entidad = usuario.Entidad
                        });

                        Session["UserSC"] = usuario;

                        if (!usuario.UsuariosPermisos.Pantallas_Principal.Acceso)
                        {
                            this.ShowNotificacion("error", "Error", "No tienes acceso a la aplicación, contacte con su administrador", "6", "0");
                            return View(user);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this.ShowNotificacion("error", "Error", getUsuarioApi.Message, "6", "0");
                    }
                }
                else
                {
                    this.ShowNotificacion("error", "Error", "Credenciales incorrectas, ¡Verifique con el Administrador!", "6", "0");

                }
            }

            return View(user);
        }

        public async Task<DBResponse<UsuariosPermisos>> GetUsuariosPermisos(Usuarios usuario)
        {
            var permisosResponse = new DBResponse<UsuariosPermisos>();
            var permiso = new UsuariosPermisos();

            var _postAccesos = new PostAcceso()
            {
                Acceso = new Entities.Services.Envio.Acceso
                {
                    empresa = usuario.Entidad,
                    id = usuario.ClaveUsuario,
                    modulo = 36
                },
                AccessToken = usuario.Token.infofin_token
            };

            var getPermisosUsuarioApi = await Task.Run(() => ApiService.ApiPostPermisosUsuarioRequest<Entities.Services.Respuesta.Acceso>(Helper.InfoApi.GetURLApi(),
                                                                                         "Api/Accesos/",
                                                                                         "ObtienePermisosUsuarioPorModulo",
                                                                                         usuario.Token.access_token,
                                                                                         _postAccesos));
            if (getPermisosUsuarioApi.ExecutionOK)
            {
                var PermisosUsuario = (Entities.Services.Respuesta.Acceso)getPermisosUsuarioApi.Data;
                //Pantalla Inicio
                permiso.Pantallas_Principal.Acceso = PermisosUsuario.accesos.Any(x => x == "PPWACCES"); //Pantalla Principal Web Acceso (Consulta). 
                //Parametrización
                permiso.Pantallas_Parametrizacion.Acceso = PermisosUsuario.accesos.Any(x => x == "PACCESS"); //Parametrización Acceso
                permiso.Pantallas_Parametrizacion.Actualizar = PermisosUsuario.accesos.Any(x => x == "PUPDATE"); //Parametrización Actualizar
                //Administrador de Delegaciones y Bancos Acceso
                permiso.Pantallas_DelegacionesBancos.Acceso = PermisosUsuario.accesos.Any(x => x == "ADBACCESS"); //Administrador de Delegaciones y Bancos Acceso (Consulta). 
                permiso.Pantallas_DelegacionesBancos.Registrar = PermisosUsuario.accesos.Any(x => x == "ADBINSERT"); //Administrador de Delegaciones y Bancos Registrar
                permiso.Pantallas_DelegacionesBancos.Actualizar = PermisosUsuario.accesos.Any(x => x == "ABDUPDATE"); //Administrador de Delegaciones y Bancos Actualizar
                permiso.Pantallas_DelegacionesBancos.Eliminar = PermisosUsuario.accesos.Any(x => x == "ABDDELETE"); //Administrador de Delegaciones y Bancos Eliminar (activar/reactivar)
                //Administrador de Tipos de Placas
                permiso.Pantallas_TiposPlacas.Acceso = PermisosUsuario.accesos.Any(x => x == "ATPACCESS"); //Administrador de Tipos de Placas Acceso (Consulta). 
                permiso.Pantallas_TiposPlacas.Registrar = PermisosUsuario.accesos.Any(x => x == "ATPINSERT"); //Administrador de Tipos de Placas Registrar
                permiso.Pantallas_TiposPlacas.Actualizar = PermisosUsuario.accesos.Any(x => x == "ATPUPDATE"); //Administrador de Tipos de Placas Actualizar
                permiso.Pantallas_TiposPlacas.Eliminar = PermisosUsuario.accesos.Any(x => x == "ATPDELETE"); //Administrador de Tipos de Placas Eliminar (activar/reactivar)
                //Administrador de Tipos de Ids
                permiso.Pantallas_TiposIDS.Acceso = PermisosUsuario.accesos.Any(x => x == "ATIACCESS"); //Administrador de Tipos de Ids Acceso (Consulta). 
                permiso.Pantallas_TiposIDS.Registrar = PermisosUsuario.accesos.Any(x => x == "ATIINSERT"); //Administrador de Tipos de Ids Registro
                permiso.Pantallas_TiposIDS.Actualizar = PermisosUsuario.accesos.Any(x => x == "ATIUPDATE"); //Administrador de Tipos de Ids Actualizar
                permiso.Pantallas_TiposIDS.Eliminar = PermisosUsuario.accesos.Any(x => x == "ATIDELETE"); //Administrador de Tipos de Ids Eliminar (activar/reactivar)
                //Administrador de Estados
                permiso.Pantallas_Estados.Acceso = PermisosUsuario.accesos.Any(x => x == "AEACCESS"); //Administrador de Estados Acceso (Consulta). 
                permiso.Pantallas_Estados.Registrar = PermisosUsuario.accesos.Any(x => x == "AEINSERT"); //Administrador de Estados Registro
                permiso.Pantallas_Estados.Actualizar = PermisosUsuario.accesos.Any(x => x == "AEUPDATE"); //Administrador de Estados Actualizar
                permiso.Pantallas_Estados.Eliminar = PermisosUsuario.accesos.Any(x => x == "AEDELETE"); //Administrador de Estados Eliminar (activar/reactivar)
                //Administrador de Proveedores
                permiso.Pantallas_Proveedores.Acceso = PermisosUsuario.accesos.Any(x => x == "APACCESS"); //Administrador de Proveedores Acceso (Consulta). 
                permiso.Pantallas_Proveedores.Registrar = PermisosUsuario.accesos.Any(x => x == "APINSERT"); //Administrador de Proveedores Registro
                permiso.Pantallas_Proveedores.Actualizar = PermisosUsuario.accesos.Any(x => x == "APUPDATE"); //Administrador de Proveedores Actualizar
                permiso.Pantallas_Proveedores.Eliminar = PermisosUsuario.accesos.Any(x => x == "APDELETE"); //Administrador de Proveedores Eliminar (activar/reactivar)
                //Administrador de Contratos
                permiso.Pantallas_Contratos.Acceso = PermisosUsuario.accesos.Any(x => x == "ACACCESS"); //Administrador de Contratos Acceso (Consulta). 
                permiso.Pantallas_Contratos.Registrar = PermisosUsuario.accesos.Any(x => x == "ACINSERT"); //Administrador de Contratos Registro
                permiso.Pantallas_Contratos.Actualizar = PermisosUsuario.accesos.Any(x => x == "ACUPDATE"); //Administrador de Contratos Actualizar
                permiso.Pantallas_Contratos.Eliminar = PermisosUsuario.accesos.Any(x => x == "ACDELETE"); //Administrador de Contratos Eliminar (activar/reactivar)
                permiso.Pantallas_Contratos.RangoTipoPlacaValidarPlacas = PermisosUsuario.accesos.Any(x => x == "ACVALCPRTP"); //Administrador de Contratos Validar Cantidad de Placas en el Rango por Tipo de Placa
                permiso.Pantallas_Contratos.RangoTipoPlaca = PermisosUsuario.accesos.Any(x => x == "ACRNGPSTPS"); //Administrador de Contratos Rangos de Placas por Serie del Tipo de Placa seleccionado
                permiso.Pantallas_Contratos.RangoTipoPlacaAcceso = PermisosUsuario.accesos.Any(x => x == "RNGACCESS"); //Rango de Placas por Serie del Tipo de Placa Seleccionado Acceso (Consulta). 
                permiso.Pantallas_Contratos.RangoTipoPlacaRegistro = PermisosUsuario.accesos.Any(x => x == "RNGINSERT"); //Rango de Placas por Serie del Tipo de Placa Seleccionado Registro
                permiso.Pantallas_Contratos.RangoTipoPlacaActualizar = PermisosUsuario.accesos.Any(x => x == "RNGUPDATE"); //Rango de Placas por Serie del Tipo de Placa Seleccionado Actualizar
                permiso.Pantallas_Contratos.RangoTipoPlacaEliminar = PermisosUsuario.accesos.Any(x => x == "RNGDELETE"); //Rango de Placas por Serie del Tipo de Placa Seleccionado Eliminar 
                //Administrador de Solicitudes de Placas a Proveedor
                permiso.Pantallas_SolicitudesPlacasProveedor.Acceso = PermisosUsuario.accesos.Any(x => x == "ASPACCESS"); //Administrador de Solicitudes de Placas a Proveedor Acceso (Consulta). 
                permiso.Pantallas_SolicitudesPlacasProveedor.Registrar = PermisosUsuario.accesos.Any(x => x == "ASPINSERT"); //Administrador de Solicitudes de Placas a Proveedor Registrar
                permiso.Pantallas_SolicitudesPlacasProveedor.Actualizar = PermisosUsuario.accesos.Any(x => x == "ASPUPDATE"); //Administrador de Solicitudes de Placas a Proveedor Actualizar
                permiso.Pantallas_SolicitudesPlacasProveedor.Eliminar = PermisosUsuario.accesos.Any(x => x == "ASPDELETE"); //Administrador de Solicitudes de Placas a Proveedor Eliminar (cancelar solicitud)
                permiso.Pantallas_SolicitudesPlacasProveedor.GeneraPDF_SolicitudPlaca = PermisosUsuario.accesos.Any(x => x == "ASPPDFGSP"); //Administrador de Solicitudes de Placas a Proveedor Generar PDF de la Solicitud de Placas al Proveedor
                permiso.Pantallas_SolicitudesPlacasProveedor.GeneraPDF_EnvioEmailProveedor = PermisosUsuario.accesos.Any(x => x == "ASPPDFGEP"); //Administrador de Solicitudes de Placas a Proveedor Generar PDF y Enviárselo al Proveedor por Email
                //Administrador de Recepción de las Solicitudes de Placas a Proveedores
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.Acceso = PermisosUsuario.accesos.Any(x => x == "RSPACCESS"); //Administrador de Recepción de las Solicitudes de Placas a Proveedores Acceso (Consulta). 
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.Registrar = PermisosUsuario.accesos.Any(x => x == "RSPINSERT"); //Administrador de Recepción de las Solicitudes de Placas a Proveedores Registrar
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.Actualizar = PermisosUsuario.accesos.Any(x => x == "RSPUPDATE"); //Administrador de Recepción de las Solicitudes de Placas a Proveedores Actualizar
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.Eliminar = PermisosUsuario.accesos.Any(x => x == "RSPDELETE"); //Administrador de Recepción de las Solicitudes de Placas a Proveedores Eliminar (cancelar recepcion)
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlaca = PermisosUsuario.accesos.Any(x => x == "RSPRECPL"); //Administrador de Recepción de las Solicitudes de Placas a Proveedores Recibir Placas
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaAcceso = PermisosUsuario.accesos.Any(x => x == "RPACCESS"); //Recibir Placas Acceso
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaLeerQR = PermisosUsuario.accesos.Any(x => x == "RPREADQR"); //Recibir Placas Leer QR Codes
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaRegistrar = PermisosUsuario.accesos.Any(x => x == "RPINSERT"); //Recibir Placas Insertar
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaActualizar = PermisosUsuario.accesos.Any(x => x == "RPUPDATE"); //Recibir Placas Eliminar
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaConsultaEvento = PermisosUsuario.accesos.Any(x => x == "CONERPACC"); //Consulta de Eventos Registrados en la Recepción de Placas Acceso
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaConsultaEventoDetalle = PermisosUsuario.accesos.Any(x => x == "CONERPDET"); //Consulta de Eventos Registrados en la Recepción de Placas Detalle del Evento
                permiso.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaConsultaEventoDetalleAcceso = PermisosUsuario.accesos.Any(x => x == "DETEACCESS"); //Detalle del Evento Acceso
                //Administrador de Transferencias de Placas entre Delegaciones/Bancos
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Acceso = PermisosUsuario.accesos.Any(x => x == "TPDBACCESS"); //Administrador de Transferencias de Placas entre Delegaciones/Bancos Acceso (Consulta). 
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Registrar = PermisosUsuario.accesos.Any(x => x == "TPDBINSERT"); //Administrador de Transferencias de Placas entre Delegaciones/Bancos Registrar Transferencia
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Actualizar = PermisosUsuario.accesos.Any(x => x == "TPDBUPDATE"); //Administrador de Transferencias de Placas entre Delegaciones/Bancos Actualizar Transferencia
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Cancelar = PermisosUsuario.accesos.Any(x => x == "TPDBCANCEL"); //Administrador de Transferencias de Placas entre Delegaciones/Bancos Cancelar Transferencia
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.GenerarPackList = PermisosUsuario.accesos.Any(x => x == "TPDBPCKLST"); //Administrador de Transferencias de Placas entre Delegaciones/Bancos Generar Packing List
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.TransferirPlacas = PermisosUsuario.accesos.Any(x => x == "TPDBTRANSF"); //Administrador de Transferencias de Placas entre Delegaciones/Bancos Transferir Placas 
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.TransferirPlacasAcceso = PermisosUsuario.accesos.Any(x => x == "TRPLACCESS"); //Transferir Placas Acceso (Consulta).
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.TransferirPlacasConfirmarTransferencia = PermisosUsuario.accesos.Any(x => x == "TRPLCNFIRM"); //Transferir Placas Confirmar Transferencia
                permiso.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.TransferirPlacasGeneraPDF = PermisosUsuario.accesos.Any(x => x == "TRPLPDFGEN"); //PDF del listado de placas de una Transferencia de placas ya realizada Acceso (Consulta).
                //Administrador de Recepción de Placas entre Delegaciones/Bancos
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.Acceso = PermisosUsuario.accesos.Any(x => x == "RPDACCESS"); //Administrador de Recepción de Placas entre Delegaciones/Bancos Acceso (Consulta). 
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirTransferencia = PermisosUsuario.accesos.Any(x => x == "RPDRECIBIR"); //Administrador de Recepción de Placas entre Delegaciones/Bancos Recibir Transferencia Completa de Placas
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RechazarTransferencia = PermisosUsuario.accesos.Any(x => x == "RPDRECHAZA"); //Administrador de Recepción de Placas entre Delegaciones/Bancos Rechazar Transferencia Completa de Placas
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.CerrarTransferencia = PermisosUsuario.accesos.Any(x => x == "RPDCERRAR"); //Administrador de Recepción de Placas entre Delegaciones/Bancos Cerrar Transferencia de Placas
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividuales = PermisosUsuario.accesos.Any(x => x == "RPDRECIND"); //Administrador de Recepción de Placas entre Delegaciones/Bancos Recibir Placas Individuales del Tipo de Placa seleccionado
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesAcceso = PermisosUsuario.accesos.Any(x => x == "RECACCESS"); //Recibir Placas Individuales del Tipo de Placa seleccionado Acceso (Consulta). 
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesPlacasDisponibles = PermisosUsuario.accesos.Any(x => x == "RECDISPON"); //Recibir Placas Individuales del Tipo de Placa seleccionado Colocar Placas como Disponibles
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesPlacasObsoleta = PermisosUsuario.accesos.Any(x => x == "RECOBSOL"); //Recibir Placas Individuales del Tipo de Placa seleccionado Colocar la placa como Obsoleta
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesColocarPlacaRechazada = PermisosUsuario.accesos.Any(x => x == "RECRECHA"); //Recibir Placas Individuales del Tipo de Placa seleccionado Colocar la placa como Rechazada
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesPlacasPerdida = PermisosUsuario.accesos.Any(x => x == "RECPERDIDA"); //Recibir Placas Individuales del Tipo de Placa seleccionado Colocar la placa como Perdida
                permiso.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesRegistrar = PermisosUsuario.accesos.Any(x => x == "RECGRDIRP"); //Recibir Placas Individuales del Tipo de Placa seleccionado Guardar Información de Recepción de Placas
                //Consulta de Información de Placas
                permiso.Pantallas_ConsultaInformacionPlacas.Acceso = PermisosUsuario.accesos.Any(x => x == "CIPACCESS"); //Consulta de Información de Placas Acceso (Consulta). 
                permiso.Pantallas_ConsultaInformacionPlacas.ConsultaInformacionPlaca = PermisosUsuario.accesos.Any(x => x == "CIPINFOPL"); //Consulta de Información de Placas Consulta de Información detallada de una Placa
                permiso.Pantallas_ConsultaInformacionPlacas.ColocarPlacasDisponibles = PermisosUsuario.accesos.Any(x => x == "CIPPLDISPO"); //Consulta de Información de Placas Colocar la placa como Disponible
                permiso.Pantallas_ConsultaInformacionPlacas.ColocarPlacasObsoleta = PermisosUsuario.accesos.Any(x => x == "CIPPLOBSO"); //Consulta de Información de Placas Colocar la placa como Obsoleta
                permiso.Pantallas_ConsultaInformacionPlacas.ColocarPlacaRechazada = PermisosUsuario.accesos.Any(x => x == "CIPPLRECH"); //Consulta de Información de Placas Colocar la placa como Rechazada
                permiso.Pantallas_ConsultaInformacionPlacas.ColocarPlacasPerdida = PermisosUsuario.accesos.Any(x => x == "CIPPLPERD"); //Consulta de Información de Placas Colocar la placa como Perdida
                permiso.Pantallas_ConsultaInformacionPlacas.ConsultaInformacionDetalladaPlaca = PermisosUsuario.accesos.Any(x => x == "CONDETPL"); //Consulta de Información detallada de una Placa
                //Inventario de Placas
                permiso.Pantallas_InventarioPlacas.Acceso = PermisosUsuario.accesos.Any(x => x == "INVPLACC"); //Inventario de Placas Acceso (Consulta)
                //Consulta de Bitácora
                permiso.Pantallas_ConsultaBitacora.Acceso = PermisosUsuario.accesos.Any(x => x == "CONBACCESS"); //Consulta de Bitácora Acceso (Consulta)
                //Reportes
                permiso.Pantallas_Reportes1.Acceso = PermisosUsuario.accesos.Any(x => x == "RPT1ACCESS"); //Reporte 1 Acceso (Consulta) 
                permiso.Pantallas_Reportes2.Acceso = PermisosUsuario.accesos.Any(x => x == "RPT2ACCESS"); //Reporte 2 Acceso (Consulta)
                permiso.Pantallas_Reportes3.Acceso = PermisosUsuario.accesos.Any(x => x == "RPT3ACCESS"); //Reporte 3 Acceso (Consulta)
                permiso.Pantallas_Reportes4.Acceso = PermisosUsuario.accesos.Any(x => x == "RPT4ACCESS"); //Reporte 4 Acceso (Consulta)
                permiso.Pantallas_Reportes5.Acceso = PermisosUsuario.accesos.Any(x => x == "RPT5ACCESS"); //Reporte 5 Acceso (Consulta)

                permisosResponse.Data = permiso;
                permisosResponse.ExecutionOK = true;
            }

            return permisosResponse;
        }

        public async Task<DBResponse<List<DelegacionesBancos>>> GetDelegacionesBancosRelUsuario(Usuarios usuario)
        {
            var delegacionesBancos = new DBResponse<List<DelegacionesBancos>>();
            delegacionesBancos.Data = new List<DelegacionesBancos>();

            var _PostAlmacen = new PostAlmacen()
            {
                AlmacenUsuario = new Entities.Services.Envio.Almacen
                {
                    empresa = usuario.Entidad, 
                    Usuario = usuario.ClaveUsuario
                },
                AccessToken = usuario.Token.infofin_token
            };

            var getAlmacenesListado = await Task.Run(() => GetAlmacenes(usuario.TokenInventario));


            var getAlmacenesUsuario = await Task.Run(() => ApiService.ApiPostAlmacenRequest<Entities.Services.Respuesta.AlmacenesUsuario>(Helper.InfoApi.GetURL_InventariosApi(),
                                                                                         "Api/Almacenes/",
                                                                                         "ListaAlmacenesUsuario",
                                                                                         usuario.TokenInventario.access_token,
                                                                                         _PostAlmacen));
            if (getAlmacenesUsuario.ExecutionOK)
            {
                var almacenesUsuario = (Entities.Services.Respuesta.ResponseAlmacen)getAlmacenesUsuario.Data;
                foreach (var item in almacenesUsuario.AlmacenesUsuario)
                {
                    var almacenCentroCostos = getAlmacenesListado.Data.Where(x => x.almacen == item.almacen).FirstOrDefault();
                    var getDelegacionBanco = new DelegacionesBancos_BL().ExisteDelegacionBancoByCentroCostos(almacenCentroCostos.centro_costo, usuario.Entidad);
                    if (getDelegacionBanco.ExecutionOK)
                    {
                        if (delegacionesBancos.Data.Count == 0)
                        {
                            delegacionesBancos.Data.Add(getDelegacionBanco.Data);
                        }
                        else
                        {
                            if (!delegacionesBancos.Data.Any(x => x.IdDelegacionBanco == getDelegacionBanco.Data.IdDelegacionBanco))
                            {
                                delegacionesBancos.Data.Add(getDelegacionBanco.Data);
                            }
                        }

                    }
                }
                 
                delegacionesBancos.ExecutionOK = true;
            }

            return delegacionesBancos;
        }

        public async Task<DBResponse<List<Almacenes>>> GetAlmacenes(Token token)
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

            var getAlmacenesApi = await Task.Run(() => ApiService.ApiPostAlmacenesListadoRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURL_InventariosApi(),
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

        public static string GetIPAddress(HttpRequestBase request)
        {
            String ip =
                System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
    }
}

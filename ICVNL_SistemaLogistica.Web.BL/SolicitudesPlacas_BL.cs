using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class SolicitudesPlacas_BL
    {
        public DBResponse<List<SolicitudesPlacas>> GetSolicitudesPlacas(string FolioSolicitud,
                                                                        DateTime FechaIniSol,
                                                                        DateTime FechaFinSol,
                                                                        DateTime FechaEntregaIniSol,
                                                                        DateTime FechaEntregaFinSol,
                                                                        int? IdProveedor,
                                                                        string NumeroContrato,
                                                                        int? NumeroOC,
                                                                        int Entidad)
        {
            var dbResponse = new DBResponse<List<SolicitudesPlacas>>();
            try
            {
                var responseData = new SolicitudesPlacas_DA().GetSolicitudesPlacas_List(FolioSolicitud, FechaIniSol, FechaFinSol, FechaEntregaIniSol, FechaEntregaFinSol, IdProveedor, NumeroContrato, NumeroOC, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = dbResponse.Data.Count;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new List<SolicitudesPlacas>();
                    dbResponse.NumRows = responseData.Data.Count;
                    dbResponse.Message = "No se encontro la información de la Solicitudes de Placas";
                    dbResponse.ExecutionOK = false;
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<SolicitudesPlacas>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<SolicitudesPlacas> GetSolicitudPlacas(int id, int Entidad)
        {
            var dbResponse = new DBResponse<SolicitudesPlacas>();
            try
            {
                var responseData = new SolicitudesPlacas_DA().GetSolicitudesPlacasEnc(id, Entidad);
                if (responseData.ExecutionOK)
                {
                    var objResponse = responseData.Data;
                    var solicitudPlacas = new SolicitudesPlacas() 
                    {
                        IdSolicitud = objResponse.IdSolicitud,
                        Entidad = objResponse.Entidad,
                        FechaEntrega = objResponse.FechaEntrega,
                        FechaRegistro = objResponse.FechaRegistro,
                        FechaSolicitud = objResponse.FechaSolicitud,
                        FolioSolicitud = objResponse.FolioSolicitud,
                        IdContrato = objResponse.IdContrato,
                        Contratos = new Contratos(),
                        IdOrdenCompra = objResponse.IdOrdenCompra,
                        OrdenesCompra = new OrdenesCompra(),
                        IdProveedor = objResponse.IdProveedor,
                        Proveedores = new Proveedores(),
                        SolicitudesPlacas_Detalle = new List<SolicitudesPlacas_Detalle>(),
                        UsuarioRegistra = objResponse.UsuarioRegistra
                    };
                    solicitudPlacas.OrdenesCompra.OrdenesCompra_Detalle = new List<OrdenesCompra_Detalle>();

                    var ordenCompra = new OrdenesCompra_BL().GetOrdenesCompraEnc(solicitudPlacas.IdOrdenCompra, Entidad);
                    if (ordenCompra.ExecutionOK)
                    {
                        solicitudPlacas.OrdenesCompra = ordenCompra.Data;
                    }

                    //asignamos el objeto contratos
                    var dbResponseContratos = new Contratos_BL().GetContrato(solicitudPlacas.IdContrato, Entidad);
                    if (dbResponseContratos.ExecutionOK)
                    {
                        solicitudPlacas.Contratos = dbResponseContratos.Data;
                    } 
                    //asignamos el objeto Proveedores
                    var dbResponseProveedores = new Proveedores_BL().GetProveedor(solicitudPlacas.IdProveedor, Entidad);
                    if (dbResponseProveedores.ExecutionOK)
                    {
                        solicitudPlacas.Proveedores = dbResponseProveedores.Data;
                    }
                    //asignamos el objeto detalle de la solicitud
                    var dbResponseDetalleSolicitud = new SolicitudesPlacas_DA().GetSolicitudesPlacasDet(id, Entidad);
                    if (dbResponseDetalleSolicitud.ExecutionOK)
                    {
                        foreach (var item in dbResponseDetalleSolicitud.Data)
                        {
                            item.TiposPlacas = new TiposPlacas();
                            item.DelegacionesBancos = new DelegacionesBancos(); 
                            //asignamos el objeto Tipo Placa
                            var dbResponseTipoPlaca = new TiposPlacas_DA().GetTiposPlacas_ById(Entidad, item.IdTipoPlaca);
                            if (dbResponseTipoPlaca.ExecutionOK)
                            {
                                item.TiposPlacas = dbResponseTipoPlaca.Data;
                            }
                            //asignamos el objeto Delegaciones Bancos
                            var dbResponseDelegaciones = new DelegacionesBancos_BL().GetDelegacionBanco(Entidad, item.IdDelegacionBanco);
                            if (dbResponseDelegaciones.ExecutionOK)
                            {
                                item.DelegacionesBancos = dbResponseDelegaciones.Data;
                            }

                            var objCantidadOrdenCompra = new OrdenesCompra_BL().GetCantidadOrdenCompraDetalleRenglon(item.IdDetalleOrdenCompra, Entidad);
                            if (objCantidadOrdenCompra.ExecutionOK)
                            {
                                item.CantidadPlacasOrdenCompra = objCantidadOrdenCompra.Data;
                            }

                            solicitudPlacas.SolicitudesPlacas_Detalle.Add(item);
                        }
                    }

                    dbResponse.Data = solicitudPlacas;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new SolicitudesPlacas();
                    dbResponse.Message = "No se encontro la información de la Solicitud de Placas";
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = false;
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new SolicitudesPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
        public DBResponse<SolicitudesPlacas> GetSolicitudesPlacasSiguienteFolio()
        {
            return new SolicitudesPlacas_DA().GetSolicitudesPlacasSiguienteFolio();
        }
        public DBResponse<SolicitudesPlacas> UpsertSolicitudPlacas(SolicitudesPlacas solicitudesPlacas, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<SolicitudesPlacas>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    //Eliminamos el detalle de la solicitud
                    if (!nRow)
                    {
                        //Eliminamos la informacion detallada del contrato
                        var infoSolicitud = new SolicitudesPlacas_BL().GetSolicitudPlacas(solicitudesPlacas.IdSolicitud, solicitudesPlacas.Entidad).Data;
                        var dbResponseEliminaDet = new SolicitudesPlacas_DA().DeleteInfoSolicitudPlaca(infoSolicitud);
                    }
                    //Insertamos/Actualizamos la solicitud
                    var dbResponseUpsert = new SolicitudesPlacas_DA().UpsertSolicitudPlaca(solicitudesPlacas, nRow);
                    if (dbResponseUpsert.ExecutionOK)
                    {
                        dbResponse.Data = dbResponseUpsert.Data;
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Solicitud de Placas",
                            JsonObject = JsonConvert.SerializeObject(solicitudesPlacas),
                            Entidad = usuario.Entidad
                        });
                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Data = new SolicitudesPlacas();
                        dbResponse.NumRows = 0;
                        dbResponse.ExecutionOK = false;
                    }

                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new SolicitudesPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "Error",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Solicitud de Placas",
                    JsonObject = JsonConvert.SerializeObject(solicitudesPlacas),
                    Entidad = usuario.Entidad
                });
            }
            return dbResponse;
        }
        public DBResponse<DBNull> DeleteSolicitudesPlacas(int IdSolicitud, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {                
                var inactivaSolicitud = new SolicitudesPlacas_DA().DeleteSolicitudPlaca(IdSolicitud);
                if (inactivaSolicitud.ExecutionOK)
                {
                    dbResponse.Data = inactivaSolicitud.Data;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Actualiza Estatus",
                        FechaEvento = DateTime.Now,
                        Evento = "Update",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Solicitud de Placas",
                        JsonObject = JsonConvert.SerializeObject(IdSolicitud),
                        Entidad = usuario.Entidad
                    });
                }
                else{
                    dbResponse.Data = inactivaSolicitud.Data;
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = inactivaSolicitud.Message;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "Error",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Solicitud de Placas",
                    JsonObject = JsonConvert.SerializeObject(IdSolicitud),
                    Entidad = usuario.Entidad
                });
            }
            return dbResponse;
        }
    }
}
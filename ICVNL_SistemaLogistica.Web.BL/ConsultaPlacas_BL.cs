using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class ConsultaPlacas_BL
    {
        public DBResponse<List<ConsultaInformacionPlacas>> GetConsultaInformacionPlacas_List(string NumeroEntrada,
                                                                                                     DateTime FechaIniNE,
                                                                                                     DateTime FechaFinNE,
                                                                                                     string NumeroOrden,
                                                                                                     DateTime FechaEntregaIniOC,
                                                                                                     DateTime FechaEntregaFinOC,
                                                                                                     int IdDelegacionBanco,
                                                                                                     int Entidad,
                                                                                                     string NumeroPlaca)
        {

            return new ConsultaPlacas_DA().GetConsultaInformacionPlacas_List(NumeroEntrada, FechaIniNE, FechaFinNE, NumeroOrden, FechaEntregaIniOC, FechaEntregaFinOC, IdDelegacionBanco, Entidad, NumeroPlaca);
        }

        public DBResponse<ConsultaInformacionPlacas_Detalle> GetConsultaInformacionPlaca(string NumeroPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<ConsultaInformacionPlacas_Detalle>();
            try
            {
                dbResponse = new ConsultaPlacas_DA().GetConsultaInformacionPlaca(NumeroPlaca, Entidad);
                if (dbResponse.ExecutionOK)
                {
                    dbResponse.Data.CambiosEstatusPlacas = new List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas>();
                    dbResponse.Data.TransferenciasPlacas = new List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas>();

                    var dbCambioEstatus = new ConsultaPlacas_BL().GetConsultaInformacionPlaca_CambiosEstatus(NumeroPlaca, Entidad);
                    if (dbCambioEstatus.ExecutionOK)
                    {
                        dbResponse.Data.CambiosEstatusPlacas = dbCambioEstatus.Data;
                    }

                    var dbTransferenciaDB = new ConsultaPlacas_BL().GetConsultaInformacionPlaca_Transferencias(NumeroPlaca, Entidad);
                    if (dbTransferenciaDB.ExecutionOK)
                    {
                        dbResponse.Data.TransferenciasPlacas = dbTransferenciaDB.Data;
                    }
                }
            }
            catch (Exception ex)
            {
                dbResponse.Data = new ConsultaInformacionPlacas_Detalle();
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> ConsultaCambiaEstatusPlaca(Placas_CambioEstatus _CambioEstatus, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var dbInfoPlaca = new ConsultaPlacas_BL().GetConsultaInformacionPlaca(_CambioEstatus.NumeroPlaca, _CambioEstatus.Entidad);
                    if (dbInfoPlaca.ExecutionOK)
                    {
                        if (dbInfoPlaca.Data.IdTipoEstatusPlaca == _CambioEstatus.Estatus)
                        {
                            dbResponse.Message = "El estatus no puede ser cambiado por el mismo estatus";
                            dbResponse.NumRows = 1;
                            dbResponse.ExecutionOK = false;
                            return dbResponse;
                        }

                        var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(_CambioEstatus.NumeroPlaca, dbInfoPlaca.Data.IdDelegacionBanco, _CambioEstatus.Entidad).Data;

                        _CambioEstatus.EstatusAnterior = dbInfoPlaca.Data.IdTipoEstatusPlaca;

                        var objHistPlaca = new InventarioPlacas_Existencia_HistorialCambios()
                        {
                            IdInventarioExistencia = infoPlacaHist.IdInventarioDetalle,
                            IdInventarioExistenciaHistorial = 0,
                            IdEstatusAnterior = _CambioEstatus.EstatusAnterior,
                            IdEstatusNuevo = _CambioEstatus.Estatus,
                            FechaOperacion = DateTime.Now
                        };

                        var dbUpdateEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistPlaca, _CambioEstatus.NumeroPlaca, _CambioEstatus.Entidad);
                        if (dbUpdateEstatusPlaca.ExecutionOK)
                        {
                            dbResponse.Message = "Placa Colocada como " + _CambioEstatus.DescEstatus;
                            dbResponse.NumRows = 1;
                            dbResponse.ExecutionOK = true;

                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Actualiza estatus de la placa",
                                FechaEvento = DateTime.Now,
                                Evento = "Update",
                                IP_Usuario = usuario.IP_Usuario,
                                Usuario = usuario.Usuario,
                                LugarEvento = "Consulta de Placas",
                                JsonObject = JsonConvert.SerializeObject(_CambioEstatus),
                                Entidad = usuario.Entidad
                            });

                            transaction.Complete();
                        }
                        else
                        {
                            dbResponse.Message = "Ocurrio un error al actualizar el estatus de la placa ";
                            dbResponse.NumRows = 1;
                            dbResponse.ExecutionOK = false;
                        }
                    }
                    else
                    {
                        dbResponse.Message = "Ocurrio un error al obtener los datos de la placa a actualizar ";
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = false;
                    }                  
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }


        public DBResponse<List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas>> GetConsultaInformacionPlaca_CambiosEstatus(string NumeroPlaca, int Entidad)
        {
            return new ConsultaPlacas_DA().GetConsultaInformacionPlaca_CambiosEstatus(NumeroPlaca, Entidad);
        }

        public DBResponse<List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas>> GetConsultaInformacionPlaca_Transferencias(string NumeroPlaca, int Entidad)
        {
            return new ConsultaPlacas_DA().GetConsultaInformacionPlaca_Transferencias(NumeroPlaca, Entidad); ;
        }
    }
}

using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class ConsultaPlacas_DA
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
            var responseDB = new DBResponse<List<ConsultaInformacionPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<ConsultaInformacionPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroEntrada),
                    Db.CreateParameter("p_NEF_FECHA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaIniNE),
                    Db.CreateParameter("p_NEF_FECHA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaFinNE),
                    Db.CreateParameter("p_OCN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroOrden),
                    Db.CreateParameter("p_OCF_FECHA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaEntregaIniOC),
                    Db.CreateParameter("p_OCF_FECHA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaEntregaFinOC),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_RNO_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca), 
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_consultaplacas.consulta_placas_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var consultaInformacionPlacas = new ConsultaInformacionPlacas()
                        {
                            NumeroPlaca = Datos.Str(row, "NumeroPlaca"),
                            DelegacionPlaca = Datos.Str(row, "DelegacionPlaca"),
                            FechaNotaEntrada = Convert.ToDateTime(Datos.Str(row, "FechaEntrada")),
                            FechaOrdenCompra = Convert.ToDateTime(Datos.Str(row, "FechaOrdenCompra")),
                            NumeroNotaEntrada = Datos.Str(row, "NotaEntrada"),
                            NumeroOrdenCompra = Datos.Str(row, "NumeroOrdenCompra"),
                            IdTipoEstatusPlaca = Datos.Int(row, "IdTipoEstatusPlaca"),
                            PlacaContabilizada = Datos.Int(row, "PlacaContabilizada")
                        };
                        responseDB.Data.Add(consultaInformacionPlacas);
                    }

                    if (responseDB.Data.Count > 0)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = responseDB.Data.Count;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<ConsultaInformacionPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<ConsultaInformacionPlacas_Detalle> GetConsultaInformacionPlaca(string NumeroPlaca, int Entidad)
        {
            var responseDB = new DBResponse<ConsultaInformacionPlacas_Detalle>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new ConsultaInformacionPlacas_Detalle();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RNO_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_consultaplacas.consulta_placa_infogral", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }
                else
                {
                    responseDB.Data = new ConsultaInformacionPlacas_Detalle()
                    {
                        IdInventario = 0,
                        NumeroPlaca = Datos.Str(row, "NumeroPlaca"), 
                        TipoPlaca = Datos.Str(row, "TipoPlaca"),
                        CostoPlaca = Datos.Str(row, "CostoPlaca"),
                        FechaNotaEntrada = Convert.ToDateTime(Datos.Str(row, "FechaEntrada")),
                        NumeroNotaEntrada = Datos.Str(row, "NotaEntrada"),
                        NumeroOrdenCompra = Datos.Str(row, "NumeroOrdenCompra"),
                        FechaOrdenCompra = Convert.ToDateTime(Datos.Str(row, "FechaOrdenCompra")),
                        ProveedorOrdenCompra = Datos.Str(row, "NombreProveedor"),
                        RenglonNotaEntrada = Datos.Str(row, "RenglonNE"),
                        RenglonOrdenCompra = Datos.Str(row, "RenglonOC"),
                        Entidad = Datos.Str(row, "Entidad"), 
                        IdDelegacionBanco = Datos.Int(row, "IdDelegacionBanco"),
                        AlmacenRecibioPlaca = Datos.Str(row, "NombreDelegacion"),
                        CuentaContableRecepcionPlaca = Datos.Str(row, "CuentaContableRecPlacas"),
                        CentroCostosAlmacenOrdenCompra = Datos.Str(row, "CentroCostosAlmOC"),
                        IdTipoEstatusPlaca = Datos.Int(row, "IdTipoEstatusPlaca"),
                        EstatusActualPlaca = Datos.Str(row, "EstatusPlaca"),
                        PlacaContabilizada = Datos.Int(row, "PlacaContabilizada") == 1 ? "SI" : "NO",
                        NumeroPolizaGRP_Infofin = Datos.Str(row, "NumeroPolizaInfofin")
                    }; 

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "";
                    responseDB.NumRows = 1; 
                }
            }
            catch (Exception ex)
            {
                responseDB.Data = new ConsultaInformacionPlacas_Detalle();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas>> GetConsultaInformacionPlaca_CambiosEstatus(string NumeroPlaca, int Entidad)
        {
            var responseDB = new DBResponse<List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVDN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_consultaplacas.consulta_placa_cambioestatus", CommandType.StoredProcedure, list);

                if (dt == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        responseDB.Data.Add(new ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas()
                        {
                            NumeroPlaca = Datos.Str(row, "NumeroPlaca"),
                            FechaHoraCambioEstatus = Convert.ToDateTime(Datos.Str(row, "FechaOperacion")),
                            OperacionRealizada = Datos.Str(row, "OperacionRealizada"),
                            EstatusAnterior = Datos.Str(row, "EstatusAnterior"),
                            EstatusNuevo = Datos.Str(row, "EstatusNuevo")
                        });
                    }

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "";
                    responseDB.NumRows = 1;
                }
            }
            catch (Exception ex)
            {
                responseDB.Data = new List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas>> GetConsultaInformacionPlaca_Transferencias(string NumeroPlaca, int Entidad)
        {
            var responseDB = new DBResponse<List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVDN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_consultaplacas.consulta_placa_transferencias", CommandType.StoredProcedure, list);

                if (dt == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        responseDB.Data.Add(new ConsultaInformacionPlacas_Detalle_TransferenciasPlacas()
                        {
                            FolioTransferencia = Datos.Str(row, "FolioTransferencia"),
                            FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "FechaTransferencia")),
                            TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                            {
                                Nombre = Datos.Str(row, "Nombre"),
                                Apellido = Datos.Str(row, "Apellido"),
                                NumeroID = Datos.Str(row, "NumeroID"),
                                TiposID = new TiposIDs()
                                {
                                    TipoID = Datos.Str(row, "TipoID")
                                },                                
                            },
                            TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                            {
                                MarcaVehiculo = Datos.Str(row, "MarcaVehiculo"),
                                ModeloVehiculo = Datos.Str(row, "ModeloVehiculo"),
                                NumeroEconomico = Datos.Str(row, "NumeroEconomico"),
                                PlacasVehiculo = Datos.Str(row, "PlacaVehiculo")
                            },
                            DelegacionesBancosOrigen = new DelegacionesBancos()
                            {
                                NombreDelegacionBanco = Datos.Str(row, "DelegacionOrigen")
                            },
                            DelegacionesBancosDestino = new DelegacionesBancos() 
                            {
                                NombreDelegacionBanco = Datos.Str(row, "DelegacionDestino")
                            },
                            TiposEstatusTransferencias = new TiposEstatusTransferencias()
                            {
                                EstatusTransferencia = Datos.Str(row, "EstatusTransferencia")
                            },
                            
                        });
                    }

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "";
                    responseDB.NumRows = 1;
                }
            }
            catch (Exception ex)
            {
                responseDB.Data = new List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
    }
}

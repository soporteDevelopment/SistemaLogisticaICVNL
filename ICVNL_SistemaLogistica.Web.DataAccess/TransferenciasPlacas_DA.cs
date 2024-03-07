using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.DataAccess.ILists;
using ICVNL_SistemaLogistica.Web.Entities; 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class TransferenciasPlacas_DA
    {
        public DBResponse<List<TransferenciaPlacas>> GetTransferenciasPlacas_List(string Folio,
                                                                                  DateTime FechaInicio,
                                                                                  DateTime FechaFin,
                                                                                  string NombreDP,
                                                                                  string ApellidoDP,
                                                                                  int NumeroIdDP,
                                                                                  int tipoIdDP,
                                                                                  int IdDelegacionOrigen,
                                                                                  int IdDelegacionDestino,
                                                                                  int IdEstatusTransferencia,
                                                                                  int IdEsatusPlacas,
                                                                                  int Entidad,
                                                                                  string StoreProcedured)
        {
            var responseDB = new DBResponse<List<TransferenciaPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<TransferenciaPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_FOLIO", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Folio),
                    Db.CreateParameter("p_TRNF_FECHA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaInicio),
                    Db.CreateParameter("p_TRNF_FECHA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaFin),
                    Db.CreateParameter("p_TDPC_NOMBRE", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, NombreDP),
                    Db.CreateParameter("p_TDPC_APELLIDO", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, ApellidoDP),
                    Db.CreateParameter("p_TDPN_NUMEROID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroIdDP),
                    Db.CreateParameter("p_TIN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, tipoIdDP),
                    Db.CreateParameter("p_DBN_ID_ORIGEN", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionOrigen),
                    Db.CreateParameter("p_DBN_ID_DESTINO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionDestino),
                    Db.CreateParameter("p_ETN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEstatusTransferencia),
                    Db.CreateParameter("p_TENP_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEsatusPlacas),
                    Db.CreateParameter("p_TRNN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable(StoreProcedured, CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var transferencia = new TransferenciaPlacas()
                        {
                            IdTransferencia = Datos.Int(row, "TRNN_ID"),
                            FolioTransferencia = Datos.Str(row, "TRNN_FOLIO"),
                            FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "TRNF_FECHA")),
                            TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                            {
                                Nombre = Datos.Str(row, "TDPC_NOMBRE"),
                                Apellido = Datos.Str(row, "TDPC_APELLIDO"),
                                TiposID = new TiposIDs()
                                {
                                    TipoID = Datos.Str(row, "TIC_TIPOID")
                                },
                                NumeroID = Datos.Str(row, "TDPN_NUMEROID")
                            },
                            TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                            {
                                MarcaVehiculo = Datos.Str(row, "TTC_MARCAVEHICULO"),
                                ModeloVehiculo = Datos.Str(row, "TTC_MODELOVEHICULO"),
                                PlacasVehiculo = Datos.Str(row, "TTC_PLACASVEHICULO"),
                                NumeroEconomico = Datos.Str(row, "TTC_NUMEROECONOMICO")
                            },
                            DelegacionesBancosOrigen = new DelegacionesBancos()
                            {
                                NombreDelegacionBanco = Datos.Str(row, "DEL_ORIGEN")
                            },
                            DelegacionesBancosDestino = new DelegacionesBancos()
                            {
                                NombreDelegacionBanco = Datos.Str(row, "DEL_DESTINO")
                            },
                            IdEstatusTransferencia = Datos.Int(row, "ETN_ID"),
                            TiposEstatusTransferencias = new TiposEstatusTransferencias()
                            {
                                IdEstatusTransferencia = Datos.Int(row, "ETN_ID"),
                                EstatusTransferencia = Datos.Str(row, "ETC_ESTATUS_TRANS")
                            }
                        };
                        responseDB.Data.Add(transferencia);
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
                responseDB.Data = new List<TransferenciaPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<TransferenciaPlacas> GetTransferenciaPlacasEnc(int IdTransferencia,
                                                                         int Entidad)
        {
            var responseDB = new DBResponse<TransferenciaPlacas>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new TransferenciaPlacas();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTransferencia), 
                    Db.CreateParameter("p_TRNN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_transf_placas_pl.consulta_transf_placas_enc", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var transferencia = new TransferenciaPlacas()
                    {
                        IdTransferencia = Datos.Int(row, "TRNN_ID"),
                        UsuarioRegistro = Datos.Str(row, "TRNC_USR_REG"),
                        FolioTransferencia = Datos.Str(row, "TRNN_FOLIO"),
                        FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "TRNF_FECHA")),
                        TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                        {
                            IdTransferenciaDatosPersona = Datos.Int(row, "TDPN_ID"),
                            Nombre = Datos.Str(row, "TDPC_NOMBRE"),
                            Apellido = Datos.Str(row, "TDPC_APELLIDO"),
                            IdTipoIDs = Datos.Int(row, "TIN_ID"),
                            TiposID = new TiposIDs()
                            {
                                Id = Datos.Int(row, "TIN_ID"),
                                TipoID = Datos.Str(row, "TIC_TIPOID")
                            },
                            NumeroID = Datos.Str(row, "TDPN_NUMEROID")
                        },
                        TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                        {                            
                            IdTransferenciaTransporte = Datos.Int(row, "TTN_ID"),
                            MarcaVehiculo = Datos.Str(row, "TTC_MARCAVEHICULO"),
                            ModeloVehiculo = Datos.Str(row, "TTC_MODELOVEHICULO"),
                            PlacasVehiculo = Datos.Str(row, "TTC_PLACASVEHICULO"),
                            NumeroEconomico = Datos.Str(row, "TTC_NUMEROECONOMICO")
                        },
                        IdDelegacionBancoOrigen = Datos.Int(row, "DBN_ID_ORIGEN"),
                        IdDelegacionBancoDestino = Datos.Int(row, "DBN_ID_DESTINO"),
                        IdEstatusTransferencia = Datos.Int(row, "ETN_ID"),
                        TiposEstatusTransferencias = new TiposEstatusTransferencias()
                        {
                            IdEstatusTransferencia = Datos.Int(row, "ETN_ID"),
                            EstatusTransferencia = Datos.Str(row, "ETC_ESTATUS_TRANS")
                        },
                        DelegacionesBancosDestino = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID_DESTINO")
                        },
                        DelegacionesBancosOrigen = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID_ORIGEN")
                        },
                        Entidad = Datos.Int(row, "TRNN_ENTIDAD")
                    };

                    responseDB.Data = transferencia;

                    if (responseDB.Data != null)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = 1;
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
                responseDB.Data = new TransferenciaPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<TransferenciaPlacas> GetRecepcionTransferenciaPlacasEnc(int IdTransferencia,
                                                                         int Entidad)
        {
            var responseDB = new DBResponse<TransferenciaPlacas>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new TransferenciaPlacas();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTransferencia),
                    Db.CreateParameter("p_TRNN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_transf_placas_pl.consulta_r_transf_placas_enc", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var transferencia = new TransferenciaPlacas()
                    {
                        IdTransferencia = Datos.Int(row, "TRNN_ID"),
                        UsuarioRegistro = Datos.Str(row, "TRNC_USR_REG"),
                        FolioTransferencia = Datos.Str(row, "TRNN_FOLIO"),
                        FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "TRNF_FECHA")),
                        TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                        {
                            IdTransferenciaDatosPersona = Datos.Int(row, "TDPN_ID"),
                            Nombre = Datos.Str(row, "TDPC_NOMBRE"),
                            Apellido = Datos.Str(row, "TDPC_APELLIDO"),
                            IdTipoIDs = Datos.Int(row, "TIN_ID"),
                            TiposID = new TiposIDs()
                            {
                                Id = Datos.Int(row, "TIN_ID"),
                                TipoID = Datos.Str(row, "TIC_TIPOID")
                            },
                            NumeroID = Datos.Str(row, "TDPN_NUMEROID")
                        },
                        TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                        {
                            IdTransferenciaTransporte = Datos.Int(row, "TTN_ID"),
                            MarcaVehiculo = Datos.Str(row, "TTC_MARCAVEHICULO"),
                            ModeloVehiculo = Datos.Str(row, "TTC_MODELOVEHICULO"),
                            PlacasVehiculo = Datos.Str(row, "TTC_PLACASVEHICULO"),
                            NumeroEconomico = Datos.Str(row, "TTC_NUMEROECONOMICO")
                        },
                        IdDelegacionBancoOrigen = Datos.Int(row, "DBN_ID_ORIGEN"),
                        IdDelegacionBancoDestino = Datos.Int(row, "DBN_ID_DESTINO"),
                        IdEstatusTransferencia = Datos.Int(row, "ETN_ID"),
                        TiposEstatusTransferencias = new TiposEstatusTransferencias()
                        {
                            IdEstatusTransferencia = Datos.Int(row, "ETN_ID"),
                            EstatusTransferencia = Datos.Str(row, "ETC_ESTATUS_TRANS")
                        },
                        DelegacionesBancosDestino = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID_DESTINO")
                        },
                        DelegacionesBancosOrigen = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID_ORIGEN")
                        },
                        Entidad = Datos.Int(row, "TRNN_ENTIDAD")
                    };

                    responseDB.Data = transferencia;

                    if (responseDB.Data != null)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = 1;
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
                responseDB.Data = new TransferenciaPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<TransferenciaPlacas_Detalle>> GetTransferenciaPlacasDet(int IdTransferencia,
                                                                                        int Entidad)
        {
            var responseDB = new DBResponse<List<TransferenciaPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<TransferenciaPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTransferencia),
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_transf_placas_pl.consulta_transf_placas_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var transferenciaPlacas = new TransferenciaPlacas_Detalle()
                        {
                            IdTransferenciaDet = Datos.Int(row, "DPN_ID"),
                            IdTransferencia = Datos.Int(row, "TRNN_ID"),  
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TiposPlacas = new TiposPlacas()
                            {
                                IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                                TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                            }, 
                            IdTipoEstatusPlacas = Datos.Int(row, "TPN_ID"),
                            TiposEstatusPlacas = new TiposEstatusPlacas()
                            {
                                IdTipoEstatusPlacas = Datos.Int(row, "TPN_ID"),
                                TipoEstatusPlacas = Datos.Str(row, "TEPC_ESTATUS")
                            },
                            Automatico = Datos.Int(row, "DPN_AUTOMATICO") == 1,
                            TransferirPlaca = Datos.Int(row, "DPN_TRANSFERIRPLACA") == 1,
                            Entidad = Datos.Int(row, "DPN_ENTIDAD"),
                            FechaEstatusPlaca = DateTime.Parse(Datos.Str(row, "DPF_FECHAESTATUSPLACA")),
                            NumeroPlaca = Datos.Str(row, "DPC_NUMEROPLACA"),
                            TransferirPlacaIndividual = Datos.Int(row, "DPN_TRANSFERENCIA_IND")
                        };
                        responseDB.Data.Add(transferenciaPlacas);
                    }
                     

                    if (responseDB.Data != null)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = 1;
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
                responseDB.Data = new List<TransferenciaPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<TransferenciaPlacas> UpsertTransferenciaPlacas(TransferenciaPlacas TransferenciaPlacas_, Boolean nRow)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            try
            {
                IList<Parameter> listTransferenciaPlacasEnc = new IListTransferenciaPlacas().ParametersEncabezadoTP(TransferenciaPlacas_);
                if (nRow)
                {
                    //Insertamos encabezado y obtenemos el ID de retorno
                    Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas", CommandType.StoredProcedure, listTransferenciaPlacasEnc);
                    var idTransferenciaPlacasInserted = Convert.ToInt32(listTransferenciaPlacasEnc[0].Value);
                    TransferenciaPlacas_.IdTransferencia = idTransferenciaPlacasInserted;
                    foreach (var detalle in TransferenciaPlacas_.TransferenciaPlacas_Detalle)
                    {
                        if (detalle.TransferirPlaca)
                        {
                            //Inicia insercion del detalle
                            detalle.IdTransferencia = idTransferenciaPlacasInserted;
                            IList<Parameter> listTransferenciaPlacasDet = new IListTransferenciaPlacas().ParametersDetalleTP(detalle);
                            Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_det", CommandType.StoredProcedure, listTransferenciaPlacasDet);
                        }               
                    }
                    TransferenciaPlacas_.TransferenciaPlacas_DatosPersona.IdTransferencia = idTransferenciaPlacasInserted;
                    IList<Parameter> parameterDatosPersona = new IListTransferenciaPlacas().ParametersDatosPersonaTP(TransferenciaPlacas_.TransferenciaPlacas_DatosPersona);
                    Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_dp", CommandType.StoredProcedure, parameterDatosPersona);

                    TransferenciaPlacas_.TransferenciaPlacas_Transporte.IdTransferencia = idTransferenciaPlacasInserted;
                    IList<Parameter> parameterDatosTransporte= new IListTransferenciaPlacas().ParametersTransportreTP(TransferenciaPlacas_.TransferenciaPlacas_Transporte);
                    Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_tr", CommandType.StoredProcedure, parameterDatosTransporte);

                }
                else
                {
                    Db.Insert("spcpl_transf_placas_pl_op.modifica_transf_placas", CommandType.StoredProcedure, listTransferenciaPlacasEnc);

                    foreach (var detalle in TransferenciaPlacas_.TransferenciaPlacas_Detalle)
                    {
                        if (detalle.TransferirPlaca)
                        {
                            //Inicia insercion del detalle
                            detalle.IdTransferencia = TransferenciaPlacas_.IdTransferencia;
                            IList<Parameter> listTransferenciaPlacasDet = new IListTransferenciaPlacas().ParametersDetalleTP(detalle);
                            Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_det", CommandType.StoredProcedure, listTransferenciaPlacasDet);
                        }
                    }
                    //Eliminamos datos de la persona
                    var parameterPersonaElimina = new List<Parameter>
                    {
                        Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TransferenciaPlacas_.IdTransferencia),
                        Db.CreateParameter("p_TDPC_TIPO", DbType.String, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, "Envia"),
                    };
                    Db.Execute("spcpl_transf_placas_pl_op.elimina_transf_placas_dp", CommandType.StoredProcedure, parameterPersonaElimina);

                    //Eliminamos datos del transporte
                    var parameterTransporteElimina = new List<Parameter>
                    {
                        Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TransferenciaPlacas_.IdTransferencia),
                        Db.CreateParameter("p_TTC_TIPO", DbType.String, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, "Envia"),
                    };
                    Db.Execute("spcpl_transf_placas_pl_op.elimina_transf_placas_tr", CommandType.StoredProcedure, parameterTransporteElimina);

                    TransferenciaPlacas_.TransferenciaPlacas_DatosPersona.IdTransferencia = TransferenciaPlacas_.IdTransferencia;
                    IList<Parameter> parameterDatosPersona = new IListTransferenciaPlacas().ParametersDatosPersonaTP(TransferenciaPlacas_.TransferenciaPlacas_DatosPersona);
                    Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_dp", CommandType.StoredProcedure, parameterDatosPersona);

                    TransferenciaPlacas_.TransferenciaPlacas_Transporte.IdTransferencia = TransferenciaPlacas_.IdTransferencia;
                    IList<Parameter> parameterDatosTransporte = new IListTransferenciaPlacas().ParametersTransportreTP(TransferenciaPlacas_.TransferenciaPlacas_Transporte);
                    Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_tr", CommandType.StoredProcedure, parameterDatosTransporte);
                }

                dbResponse.Data = TransferenciaPlacas_;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente el la transferencia de placas";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas_DatosPersona> UpsertTransferencia_DatosPersona(TransferenciaPlacas_DatosPersona _DatosPersona)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas_DatosPersona>();
            try
            { 
                var parameterPersonaElimina = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _DatosPersona.IdTransferencia),
                    Db.CreateParameter("p_TDPC_TIPO", DbType.String, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, "Recibe"),
                };
                Db.Execute("spcpl_transf_placas_pl_op.elimina_transf_placas_dp", CommandType.StoredProcedure, parameterPersonaElimina); 

                IList<Parameter> parameterDatosPersona = new IListTransferenciaPlacas().ParametersDatosPersonaTP(_DatosPersona);
                Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_dp", CommandType.StoredProcedure, parameterDatosPersona);

                dbResponse.Data = _DatosPersona;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Datos de la Persona que recibe las placas registrado coorrectamente";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<TransferenciaPlacas_Transporte> UpsertTransferencia_Transporte(TransferenciaPlacas_Transporte _Transporte)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas_Transporte>();
            try
            {
                var parameterTransporteElimina = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Transporte.IdTransferencia),
                    Db.CreateParameter("p_TTC_TIPO", DbType.String, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, "Recibe"),
                };
                Db.Execute("spcpl_transf_placas_pl_op.elimina_transf_placas_tr", CommandType.StoredProcedure, parameterTransporteElimina);

                IList<Parameter> parameterDatosTransporte = new IListTransferenciaPlacas().ParametersTransportreTP(_Transporte);
                Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_tr", CommandType.StoredProcedure, parameterDatosTransporte);

                dbResponse.Data = _Transporte;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Transporte en el que llegan las placas registrado correctamente";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas> DeleteTransferenciaPlacasDetalle(int IdTransferencia, int Entidad)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            try
            {
                //Eliminamos datos del detalle
                var parameterDetalleElimina = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTransferencia),
                    Db.CreateParameter("p_DPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                };
                Db.Execute("spcpl_transf_placas_pl_op.delete_transf_placas_det", CommandType.StoredProcedure, parameterDetalleElimina);
                dbResponse.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas> PackingListTransferenciaPlacas(TransferenciaPlacas TransferenciaPlacas_)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            try
            {
                //Eliminamos datos del detalle
                var parameterDetalleElimina = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TransferenciaPlacas_.IdTransferencia),
                    Db.CreateParameter("p_DPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, TransferenciaPlacas_.Entidad),
                };
                Db.Execute("spcpl_transf_placas_pl_op.delete_transf_placas_det", CommandType.StoredProcedure, parameterDetalleElimina);

                foreach (var detalle in TransferenciaPlacas_.TransferenciaPlacas_Detalle)
                {
                    if (detalle.TransferirPlaca)
                    {
                        //Inicia insercion del detalle
                        detalle.IdTransferencia = TransferenciaPlacas_.IdTransferencia;
                        IList<Parameter> listTransferenciaPlacasDet = new IListTransferenciaPlacas().ParametersDetalleTP(detalle);
                        Db.Insert("spcpl_transf_placas_pl_op.agregar_transf_placas_det", CommandType.StoredProcedure, listTransferenciaPlacasDet);
                    }
                }
                dbResponse.Data = TransferenciaPlacas_;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se asignaron las placas al packing list correctamente";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<SolicitudesPlacas> GetTransferenciaSiguienteFolio()
        {
            var responseDB = new DBResponse<SolicitudesPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_transf_placas_pl.consulta_tran_placas_sigfolio", CommandType.StoredProcedure, list); 
                responseDB.Data = new SolicitudesPlacas()
                {
                    FolioSolicitud = Datos.Str(row, "SiguienteFolio").PadLeft(8, '0')
                };
            }
            catch (Exception ex)
            {
                responseDB.Data = new SolicitudesPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DBNull> CambioEstatusTransferencia(int IdTransferencia, int EstatusTransferencia)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTransferencia),
                    Db.CreateParameter("p_ETN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, EstatusTransferencia)
                };

                Db.Execute("spcpl_transf_placas_pl_op.cambia_estatus_transf_placas", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> UpdateInventariosPlacasDet(TransferenciaPlacas_Detalle transferenciaPlacas, int Entidad)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_TRNN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.IdTransferencia),
                    Db.CreateParameter("p_DPC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.NumeroPlaca),
                    Db.CreateParameter("p_DPC_USUARIORECIBIO", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.UsuarioRecibio),
                    Db.CreateParameter("p_DPN_TRANSFERENCIA_IND", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.TransferirPlacaIndividual)
                };

                Db.Execute("spcpl_transf_placas_pl_op.modifica_transf_placas_det", CommandType.StoredProcedure, list);

                responseDB.Data = null;
                responseDB.ExecutionOK = true;

            }
            catch (Exception ex)
            {
                responseDB.Data = null;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

    }
}

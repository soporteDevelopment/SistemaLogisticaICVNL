using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.ApiServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class InventarioPlacas_DA
    {
        public DBResponse<List<InventarioPlacas>> GetInventarioPlacas_List(int IdDelegacionBanco, int IdTipoPlaca, int Entidad)
        {
            var responseDB = new DBResponse<List<InventarioPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<InventarioPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoPlaca),
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_inventarioplacas.consulta_inventario", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var InventarioPlacas = new InventarioPlacas()
                        {
                            IdInventario = Datos.Int(row, "INVN_ID"),
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            DelegacionesBancos = new DelegacionesBancos()
                            {
                                IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                                NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                            },
                            FechaInventario = DateTime.Parse(Datos.Str(row, "INVF_FECHA"))
                        };
                        responseDB.Data.Add(InventarioPlacas);
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
                responseDB.Data = new List<InventarioPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<InventarioPlacas> GetInventarioPlacas_Enc(int IdDelegacionBanco, int Entidad)
        {
            var responseDB = new DBResponse<InventarioPlacas>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new InventarioPlacas();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_inventarioplacas.consulta_inventario_enc", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    
                    var InventarioPlacas = new InventarioPlacas()
                    { 
                        IdInventario = Datos.Int(row, "INVN_ID"),
                        IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                        Entidad = Datos.Int(row, "INVN_ENTIDAD"),
                        FechaInventario = Convert.ToDateTime(Datos.Str(row, "INVF_FECHA"))
                    };

                    responseDB.Data = InventarioPlacas;
                    responseDB.ExecutionOK = true;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new InventarioPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<List<InventarioPlacas_Detalle>> GetInventarioPlacas_Detalle(int IdDelegacionBanco, int Entidad)
        {
            var responseDB = new DBResponse<List<InventarioPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<InventarioPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_inventarioplacas.consulta_inventario_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var InventarioPlacas = new InventarioPlacas_Detalle()
                        {
                           IdInventarioDetalle = Datos.Int(row, "INVDN_ID"),
                           IdInventario = Datos.Int(row, "INVN_ID"),
                           IdEstatusPlacas = Datos.Int(row, "EPN_ID"),
                           IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                           EstatusPlacas = new TiposEstatusPlacas()
                           {
                               IdTipoEstatusPlacas = Datos.Int(row, "TPN_ID"),
                               TipoEstatusPlacas =  Datos.Str(row, "TEPC_ESTATUS"),
                           },
                           TiposPlacas = new TiposPlacas()
                           {
                               IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                               TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                           },
                           Existencia = Datos.Int(row, "INVDN_EXISTENCIA"),
                           NumeroPlaca = Datos.Str(row, "INVDC_NUMEROPLACA"),
                           CostoPlaca = Datos.Dec(row, "INVDN_COSTOPLACA"),
                           NumeroPolizaInfofin = Datos.Str(row, "INVDC_NUM_POL_INFOFIN"),
                           PlacaContabilizada = Datos.Bool(row, "INVDN_PLACA_CONT"),
                           FechaEntrada = Convert.ToDateTime(Datos.Str(row, "NEF_FECHA"))
                        };

                        responseDB.Data.Add(InventarioPlacas);
                    }
                    responseDB.ExecutionOK = true; 
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<InventarioPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<InventarioPlacas_Detalle>> GetInventarioPlacas_Transferencia(int IdDelegacionBanco, int Entidad)
        {
            var responseDB = new DBResponse<List<InventarioPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<InventarioPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_inventarioplacas.consulta_inventario_det_tr", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var InventarioPlacas = new InventarioPlacas_Detalle()
                        {
                            IdInventarioDetalle = Datos.Int(row, "INVDN_ID"),
                            IdInventario = Datos.Int(row, "INVN_ID"),
                            IdEstatusPlacas = Datos.Int(row, "EPN_ID"),
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            EstatusPlacas = new TiposEstatusPlacas()
                            {
                                IdTipoEstatusPlacas = Datos.Int(row, "TPN_ID"),
                                TipoEstatusPlacas = Datos.Str(row, "TEPC_ESTATUS"),
                            },
                            TiposPlacas = new TiposPlacas()
                            {
                                IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                                TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                            },
                            Existencia = Datos.Int(row, "INVDN_EXISTENCIA"),
                            NumeroPlaca = Datos.Str(row, "INVDC_NUMEROPLACA"),
                            CostoPlaca = Datos.Dec(row, "INVDN_COSTOPLACA"),
                            NumeroPolizaInfofin = Datos.Str(row, "INVDC_NUM_POL_INFOFIN"),
                            PlacaContabilizada = Datos.Bool(row, "INVDN_PLACA_CONT"),
                        };

                        responseDB.Data.Add(InventarioPlacas);
                    }
                    responseDB.ExecutionOK = true;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<InventarioPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DBNull> InventariosPlacas_CambiaEstatus(InventarioPlacas_Existencia_HistorialCambios historialCambios, string NumeroPlaca, int Entidad)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false; 
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVDN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, historialCambios.IdInventarioExistencia),
                    Db.CreateParameter("p_IDHN_ID", DbType.Int32, 5, ParameterDirection.Output, false, null, DataRowVersion.Default, historialCambios.IdInventarioExistenciaHistorial),
                    Db.CreateParameter("p_EPN_IDNUEVO", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, historialCambios.IdEstatusNuevo), 
                    Db.CreateParameter("p_IDHN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_EPN_IDANTERIOR", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, historialCambios.IdEstatusAnterior),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca)
                };

                Db.Execute("spcpl_inventarioplacas_op.cambia_estatus_placa", CommandType.StoredProcedure, list);

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
                
        public DBResponse<DBNull> InventariosPlacas_CambiaDelegacionPlaca(int IdInventario, string NumeroPlaca, int Entidad)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad), 
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                    Db.CreateParameter("p_INVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdInventario)
                };

                Db.Execute("spcpl_inventarioplacas_op.cambia_delegacion_placa", CommandType.StoredProcedure, list);

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

        public DBResponse<DBNull> InventariosPlacas_ActualizaPlacaContabilizada(string NumeroPlaca, int Entidad)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                };

                Db.Execute("spcpl_inventarioplacas_op.actualiza_placa_a_contab", CommandType.StoredProcedure, list);

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

        public DBResponse<InventarioPlacas_Detalle> GetInventarioPlacas_InfoPlaca(string NumeroPlaca,int IdDelegacionBanco, int Entidad)
        {
            var responseDB = new DBResponse<InventarioPlacas_Detalle>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new InventarioPlacas_Detalle();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_inventarioplacas.consulta_inventario_info_pl", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                { 
                    var InventarioPlacas = new InventarioPlacas_Detalle()
                    {
                        IdInventarioDetalle = Datos.Int(row, "INVDN_ID"),
                        IdInventario = Datos.Int(row, "INVN_ID"),
                        IdEstatusPlacas = Datos.Int(row, "EPN_ID"),
                        IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                        EstatusPlacas = new TiposEstatusPlacas()
                        {
                            IdTipoEstatusPlacas = Datos.Int(row, "TPN_ID"),
                            TipoEstatusPlacas = Datos.Str(row, "TEPC_ESTATUS"),
                        },
                        TiposPlacas = new TiposPlacas()
                        {
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                        },
                        Existencia = Datos.Int(row, "INVDN_EXISTENCIA"),
                        NumeroPlaca = Datos.Str(row, "INVDC_NUMEROPLACA"),
                        CostoPlaca = Datos.Dec(row, "INVDN_COSTOPLACA"),
                        NumeroPolizaInfofin = Datos.Str(row, "INVDC_NUM_POL_INFOFIN"),
                        PlacaContabilizada = Datos.Bool(row, "INVDN_PLACA_CONT"),

                    };
                    responseDB.Data = InventarioPlacas;
                    responseDB.ExecutionOK = true;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new InventarioPlacas_Detalle();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<InventarioPlacas_Existencia_HistorialCambios>> GetInventarioPlacas_InfoHistoricoCambiosPlaca(string NumeroPlaca, int IdDelegacionBanco, int Entidad)
        {
            var responseDB = new DBResponse<List<InventarioPlacas_Existencia_HistorialCambios>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<InventarioPlacas_Existencia_HistorialCambios>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroPlaca),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_INVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_inventarioplacas.consulta_inventario_infoh_pl", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var invHistorico = new InventarioPlacas_Existencia_HistorialCambios()
                        {
                            IdInventarioExistenciaHistorial = Datos.Int(row, "IDHN_ID"),
                            IdInventarioExistencia = Datos.Int(row, "INVDN_ID"),
                            IdEstatusAnterior = Datos.Int(row, "EPN_IDANTERIOR"),
                            EstatusPlacasAnterior = new TiposEstatusPlacas()
                            {
                                IdTipoEstatusPlacas = Datos.Int(row, "EPN_IDANTERIOR"),
                                TipoEstatusPlacas = Datos.Str(row, "EPC_ANTERIOR")
                            },
                            IdEstatusNuevo = Datos.Int(row, "EPN_IDNUEVO"),
                            EstatusPlacasNuevo = new TiposEstatusPlacas()
                            {
                                IdTipoEstatusPlacas = Datos.Int(row, "EPN_IDNUEVO"),
                                TipoEstatusPlacas = Datos.Str(row, "EPC_NUEVO")
                            },
                            FechaOperacion = Convert.ToDateTime(Datos.Str(row, "IDHF_FECHA_OP")),
                            Operacion = ""
                        };
                        responseDB.Data.Add(invHistorico);
                    } 
                     
                    responseDB.ExecutionOK = true;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<InventarioPlacas_Existencia_HistorialCambios>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<PlacasNoInvResponse>> GetPlacasNoInventariadas_EnviaNotificacion(int Entidad)
        {
            var responseDB = new DBResponse<List<PlacasNoInvResponse>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<PlacasNoInvResponse>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                { 
                    Db.CreateParameter("p_PVNIN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_inventarioplacas.consulta_placas_noinv_notif", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var invHistorico = new PlacasNoInvResponse()
                        {
                            IdPlacaNoInv = Datos.Int(row, "PVNIN_ID"),
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            DelegacionesBancos = new DelegacionesBancos()
                            {
                                IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                                NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                            },
                            DelegacionBanco = Datos.Str(row, "PVNIC_DELEGACIONVENTA"),
                            FechaOperacionVenta = Convert.ToDateTime(Datos.Str(row, "PVNIF_FECHAPAGO")),
                            NumeroPlaca = Datos.Str(row, "PVNIC_NUMEROPLACA"),
                            Entidad = Datos.Int(row, "PVNIN_ENTIDAD"),
                        };
                        responseDB.Data.Add(invHistorico);
                    }

                    responseDB.ExecutionOK = true;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<PlacasNoInvResponse>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DBNull> InsertInventariosPlacas_Entrega_Venta(PlacasResponse placasResponse)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_IPEVN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, placasResponse.IdPlacaEntradaVenta),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.IdDelegacionBanco),
                    Db.CreateParameter("p_INVDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.IdInventarioDetalle),
                    Db.CreateParameter("p_IPEVC_USUARIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.UsuarioRegistro),
                    Db.CreateParameter("p_IPEVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.Entidad),
                    Db.CreateParameter("p_IPEVF_VENDIDO", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.FechaOperacionVenta),
                    Db.CreateParameter("p_IPEVC_REF_INFOFIN", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.ReferenciaInfofin),
                    Db.CreateParameter("p_IPEVN_PRECIOVENTA", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.PrecioVenta),
                    Db.CreateParameter("p_IPEVC_TIPO", DbType.String, 7, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.TipoOperacion),
                    Db.CreateParameter("p_IPEVF_ENTREGADO", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.FechaOperacionEntrega)
                };

                Db.Execute("spcpl_inventarioplacas_op.agrega_inv_placa_ent_ven", CommandType.StoredProcedure, list);

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

        public DBResponse<DBNull> InsertPlacaSinInventariar(PlacasNoInvResponse placasResponse)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PVNIN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, placasResponse.IdPlacaNoInv),
                    Db.CreateParameter("p_PVNIN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.Entidad),
                    Db.CreateParameter("p_PVNIN_NOTIFICACION_ENV", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_PVNIC_DELEGACIONVENTA", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.DelegacionBanco),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.IdDelegacionBanco),
                    Db.CreateParameter("p_PVNIC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.NumeroPlaca),
                    Db.CreateParameter("p_PVNIF_FECHAPAGO", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.FechaOperacionVenta)
                };

                Db.Insert("spcpl_inventarioplacas_op.agregar_placa_noinventario", CommandType.StoredProcedure, list);

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

        public DBResponse<DBNull> InsertPlacaInventario(InventarioPlacas_Detalle placaDetalle)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> listInvDet = new List<Parameter>
                {
                    Db.CreateParameter("p_INVDN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, placaDetalle.IdInventarioDetalle),
                    Db.CreateParameter("p_INVDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.Entidad),
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.IdNotaEntrada),
                    Db.CreateParameter("p_INVDN_PLACA_CONT", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.PlacaContabilizada ? 1 : 0),
                    Db.CreateParameter("p_INVDN_EXISTENCIA", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.Existencia),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.IdTipoPlaca),
                    Db.CreateParameter("p_INVDN_BORRADO", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_INVDC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.NumeroPlaca),
                    Db.CreateParameter("p_INVDC_NUM_POL_INFOFIN", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.NumeroPolizaInfofin),
                    Db.CreateParameter("p_INVDC_SERIE", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.Serie),
                    Db.CreateParameter("p_INVDN_COSTOPLACA", DbType.Decimal, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.CostoPlaca),
                    Db.CreateParameter("p_INVN_ID", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.IdInventario),
                    Db.CreateParameter("p_EPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.IdEstatusPlacas)
                };

                Db.Insert("spcpl_inventarioplacas_op.agregar_inventario_det", CommandType.StoredProcedure, listInvDet); 
                var IdInventarioDetalle_ = Convert.ToInt32(listInvDet[0].Value);
                placaDetalle.InventarioPlacas_Relacion_NE_OC.IdInventarioDetalle = IdInventarioDetalle_;


                IList<Parameter> listInvDetRel = new List<Parameter>
                {
                    Db.CreateParameter("p_RNO_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.idRelacionInventario),
                    Db.CreateParameter("p_RNO_RENGLON_NE", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.RenglonNE),
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.IdOrdenCompra),
                    Db.CreateParameter("p_RNO_RENGLON_OC", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.RenglonOC),
                    Db.CreateParameter("p_RNO_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.Entidad),
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.IdNotaEntrada),
                    Db.CreateParameter("p_INVDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placaDetalle.InventarioPlacas_Relacion_NE_OC.IdInventarioDetalle),
                };

                Db.Insert("spcpl_inventarioplacas_op.agregar_inventario_rel_ne_oc", CommandType.StoredProcedure, listInvDetRel);

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

        public DBResponse<DBNull> UpdatePlacaSinInventariar_MarcaNotifEnviada(PlacasNoInvResponse placasResponse)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_IPEVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.IdPlacaNoInv),
                    Db.CreateParameter("p_PVNIC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.NumeroPlaca),
                    Db.CreateParameter("p_PVNIN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, placasResponse.Entidad)
                };

                Db.Update("spcpl_inventarioplacas_op.actualiza_pl_noinv_notifenvio", CommandType.StoredProcedure, list);

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

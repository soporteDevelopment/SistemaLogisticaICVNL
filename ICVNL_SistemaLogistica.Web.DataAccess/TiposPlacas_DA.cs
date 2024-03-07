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
    public class TiposPlacas_DA
    {
        public DBResponse<List<TiposPlacas>> GetTiposPlacas_List(int? Estatus, int Entidad)
        {
            var responseDB = new DBResponse<List<TiposPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<TiposPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_tpn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_TPN_ACTIVO", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Estatus),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_tipos_placas.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var TiposPlacas = new TiposPlacas()
                        {
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA"),
                            MascaraPlaca = Datos.Str(row, "TPC_MASCARA_PLACA"),
                            CodigoInfofin = Datos.Str(row, "TPC_COD_INFOFIN"),
                            CantidadPlacas = Datos.Int(row, "TPN_CANT_LAM_JGO_PL"),
                            CantidadPlacasCaja = Datos.Int(row, "TPN_CANT_PLACAS_CAJA"),
                            Entidad = Datos.Int(row, "TPN_ENTIDAD"),
                            OrdenPlaca = Datos.Str(row, "TPC_ORDEN_PLACA"),
                            OrdenSeriePlaca = Datos.Str(row, "TPN_ORDEN_SERIE"),
                            Estatus = Datos.Int(row, "TPN_ACTIVO")
                        };
                        responseDB.Data.Add(TiposPlacas);
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
                responseDB.Data = new List<TiposPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<TiposPlacas> GetTiposPlacas_ById(int Entidad, int IdTipoPlaca)
        {
            var responseDB = new DBResponse<TiposPlacas>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new TiposPlacas();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_tpn_id", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoPlaca),
                    Db.CreateParameter("p_tpn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_tipos_placas.consulta_tiposplacas", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var TiposPlacas = new TiposPlacas()
                    {
                        IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                        TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA"),
                        MascaraPlaca = Datos.Str(row, "TPC_MASCARA_PLACA"),
                        CodigoInfofin = Datos.Str(row, "TPC_COD_INFOFIN"),
                        CantidadPlacas = Datos.Int(row, "TPN_CANT_LAM_JGO_PL"),
                        CantidadPlacasCaja = Datos.Int(row, "TPN_CANT_PLACAS_CAJA"),
                        Entidad = Datos.Int(row, "TPN_ENTIDAD"),
                        OrdenPlaca = Datos.Str(row, "TPC_ORDEN_PLACA"),
                        OrdenSeriePlaca = Datos.Str(row, "TPN_ORDEN_SERIE"),
                        Estatus = Datos.Int(row, "TPN_ACTIVO")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Data = TiposPlacas;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new TiposPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<TiposPlacas> ExisteTipoPlaca(int Entidad, string TipoPlaca)
        {
            var responseDB = new DBResponse<TiposPlacas>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TPC_TIPOPLACA", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, TipoPlaca),
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_tipos_placas.existe_tipoplaca", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = new TiposPlacas();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new TiposPlacas()
                    {
                        IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                        TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA"),
                    };
                    responseDB.Message = "el Tipo de Placa ya existe en la compañía";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new TiposPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<TiposPlacas> ExisteTipoPlaca_Infofin(int Entidad, string ClaveInfofin)
        {
            var responseDB = new DBResponse<TiposPlacas>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TPC_COD_INFOFIN", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, ClaveInfofin),
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_tipos_placas.existe_tipoplaca_infofin", CommandType.StoredProcedure, list);
                if (row == null)
                {
                    responseDB.Data = new TiposPlacas();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new TiposPlacas()
                    {
                        IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                        TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA"),
                    };
                    responseDB.Message = "el Código de Artículo en Infofin ingresado al Tipo de Placa ya fue agregado";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new TiposPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<TiposPlacas> UpsertTipoPlaca(TiposPlacas TiposPlacas, Boolean nRow)
        {
            var dbResponse = new DBResponse<TiposPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TPC_TIPOPLACA", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.TipoPlaca),
                    Db.CreateParameter("p_TPC_MASCARA_PLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.MascaraPlaca),
                    Db.CreateParameter("p_TPN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_TPC_COD_INFOFIN", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.CodigoInfofin),
                    Db.CreateParameter("p_TPN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.IdTipoPlaca),
                    Db.CreateParameter("p_TPC_ORDEN_PLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.OrdenPlaca),
                    Db.CreateParameter("p_TPN_ORDEN_SERIE", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.OrdenSeriePlaca),
                    Db.CreateParameter("p_TPN_CANT_PLACAS_CAJA", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.CantidadPlacasCaja),
                    Db.CreateParameter("p_TPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.Entidad),
                    Db.CreateParameter("p_TPN_CANT_LAM_JGO_PL", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposPlacas.CantidadPlacas)
                };
                if (nRow)
                {
                    Db.Insert("spcpl_tipos_placas_op.agregar", CommandType.StoredProcedure, list);
                }
                else
                {
                    Db.Update("spcpl_tipos_placas_op.modificar", CommandType.StoredProcedure, list);
                }
                dbResponse.Data = TiposPlacas;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente el Tipo de Placa";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<DBNull> CambiaEstatusTipoPlaca(int IdTipoPlaca)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoPlaca)
                };

                Db.Execute("spcpl_tipos_placas_op.cambia_estatus", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el Tipo de Placa";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
    }
}

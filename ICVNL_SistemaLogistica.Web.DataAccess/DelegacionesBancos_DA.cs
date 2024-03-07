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
    public class DelegacionesBancos_DA
    {

        public DBResponse<List<DelegacionesBancos>> GetDelegacionesBancos_List(int? Estatus, int Entidad)
        {
            var responseDB = new DBResponse<List<DelegacionesBancos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<DelegacionesBancos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_DBN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, Estatus),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_delegaciones_bancos.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var delegacionesBancos = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            AlmacenCentroCostos = Datos.Str(row, "DBC_ALMACEN_CC"),
                            NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL"),
                            CentroCostosInfoFin = Datos.Str(row, "DBC_CC_INFOFIN"),
                            Estatus = Datos.Int(row, "DBN_ACTIVO")
                        };
                        responseDB.Data.Add(delegacionesBancos);
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
                responseDB.Data = new List<DelegacionesBancos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<DelegacionesBancos> GetDelegacionesBancos_ById(int Entidad, int IdDelegacionBanco)
        {
            var responseDB = new DBResponse<DelegacionesBancos>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new DelegacionesBancos();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_dbn_id", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_delegaciones_bancos.consulta_delegacionbanco", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var delegacionesBancos = new DelegacionesBancos()
                    {
                        IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                        AlmacenCentroCostos = Datos.Str(row, "DBC_ALMACEN_CC"),
                        NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL"),
                        CentroCostosInfoFin = Datos.Str(row, "DBC_CC_INFOFIN"),
                        Entidad = Datos.Int(row, "DBN_ENTIDAD"),
                        Estatus = Datos.Int(row, "DBN_ACTIVO")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Data = delegacionesBancos;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new DelegacionesBancos();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DelegacionesBancos> ExisteDelegacionBanco(int Entidad, string NombreDelegacion)
        {
            var responseDB = new DBResponse<DelegacionesBancos>();
            responseDB.ExecutionOK = false;  
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBC_NOMBRE_DEL", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, NombreDelegacion),
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_delegaciones_bancos.existe_delegacionbanco", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = new DelegacionesBancos();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new DelegacionesBancos()
                    {
                        IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                        NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL"),
                    };
                    responseDB.Message = "La Delegación/Banco ya existe en la compañía";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new DelegacionesBancos();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DelegacionesBancos> ExisteDelegacionBancoInfofin(int Entidad, string ClaveInfofin)
        {
            var responseDB = new DBResponse<DelegacionesBancos>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBC_CC_INFOFIN", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, ClaveInfofin),
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_delegaciones_bancos.existe_delegacionbco_infofin", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = new DelegacionesBancos();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new DelegacionesBancos()
                    {
                        IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                        NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL"),
                    };
                    responseDB.Message = "el Centro de Costos en Infofin ingresado a la Delegación ya fue agregado";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new DelegacionesBancos();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }


        public DBResponse<DelegacionesBancos> UpsertDelegacionBanco(DelegacionesBancos delegacionesBancos, Boolean nRow)
        {
            var dbResponse = new DBResponse<DelegacionesBancos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBC_ALMACEN_CC", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, delegacionesBancos.AlmacenCentroCostos),
                    Db.CreateParameter("p_DBC_CC_INFOFIN", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, delegacionesBancos.CentroCostosInfoFin), 
                    Db.CreateParameter("p_DBC_NOMBRE_DEL", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, delegacionesBancos.NombreDelegacionBanco),
                    Db.CreateParameter("p_DBN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, delegacionesBancos.Entidad),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, delegacionesBancos.IdDelegacionBanco),
                    Db.CreateParameter("p_DBN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_DBN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0)
                };
                if (nRow)
                {
                    Db.Insert("spcpl_delegaciones_bancos_op.agregar", CommandType.StoredProcedure, list);
                }
                else
                {
                    Db.Update("spcpl_delegaciones_bancos_op.modificar", CommandType.StoredProcedure, list);
                }
                dbResponse.Data = delegacionesBancos;
                dbResponse.ExecutionOK = true; 
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente la Delegación/Banco";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<DBNull> CambiaEstatusDelegacionBanco(int IdDelegacionBanco)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                { 
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco) 
                };

                Db.Execute("spcpl_delegaciones_bancos_op.cambia_estatus", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente la Delegación/Banco";
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

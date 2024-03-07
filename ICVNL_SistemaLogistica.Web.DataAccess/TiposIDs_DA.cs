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
    public class TiposIDs_DA
    {
        public DBResponse<List<TiposIDs>> GetTiposIDs_List(int? Estatus, int Entidad)
        {
            var responseDB = new DBResponse<List<TiposIDs>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<TiposIDs>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_tin_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_TIN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, Estatus),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_tipos_ids.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var TiposIDs = new TiposIDs()
                        {
                            Id = Datos.Int(row, "TIN_ID"),
                            TipoID = Datos.Str(row, "TIC_TIPOID"),                            
                            Estatus = Datos.Int(row, "TIN_ACTIVO")
                        };
                        responseDB.Data.Add(TiposIDs);
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
                responseDB.Data = new List<TiposIDs>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<TiposIDs> GetTiposIDs_ById(int Entidad, int IdTipoID)
        {
            var responseDB = new DBResponse<TiposIDs>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new TiposIDs();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_tin_id", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoID),
                    Db.CreateParameter("p_tin_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_tipos_ids.consulta_tipoid", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var TiposIDs = new TiposIDs()
                    {
                        Id = Datos.Int(row, "TIN_ID"),
                        TipoID = Datos.Str(row, "TIC_TIPOID"),
                        Estatus = Datos.Int(row, "TIN_ACTIVO")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Data = TiposIDs;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new TiposIDs();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<TiposIDs> ExisteTipoID(int Entidad, string TipoID)
        {
            var responseDB = new DBResponse<TiposIDs>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TIC_TIPOID", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, TipoID),
                    Db.CreateParameter("p_tin_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_tipos_ids.existe_tipoid", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = new TiposIDs();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new TiposIDs()
                    {
                        Id = Datos.Int(row, "TIN_ID"),
                        TipoID = Datos.Str(row, "TIC_TIPOID")
                    };
                    responseDB.Message = "el Tipo de ID ya existe en la compañía";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new TiposIDs();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }


        public DBResponse<TiposIDs> UpsertTipoID(TiposIDs TiposIDs, Boolean nRow)
        {
            var dbResponse = new DBResponse<TiposIDs>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                { 
                    Db.CreateParameter("p_TIN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_TIN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposIDs.Id),
                    Db.CreateParameter("p_TIC_TIPOID", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposIDs.TipoID),
                    Db.CreateParameter("p_TIN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_TPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, TiposIDs.Entidad)
                };
                if (nRow)
                {
                    Db.Insert("spcpl_tipos_ids_op.agregar", CommandType.StoredProcedure, list);
                }
                else
                {
                    Db.Update("spcpl_tipos_ids_op.modificar", CommandType.StoredProcedure, list);
                }
                dbResponse.Data = TiposIDs;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente el Tipo de ID";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<DBNull> CambioEstatusTipoID(int IdTipoID)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TIN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoID)
                };

                Db.Execute("spcpl_tipos_ids_op.cambia_estatus", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el Tipo de ID";
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
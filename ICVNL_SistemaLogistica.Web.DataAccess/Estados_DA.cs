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
    public class Estados_DA
    {
        public DBResponse<List<Estados>> GetEstados_List(int? Estatus, int Entidad)
        {
            var responseDB = new DBResponse<List<Estados>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Estados>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_esn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_ESN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, Estatus),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_estados.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var Estados = new Estados()
                        {
                            Id = Datos.Int(row, "ESN_ID"),
                            Estado = Datos.Str(row, "ESC_NOMBRE"),
                            Entidad = Datos.Int(row, "ESN_ENTIDAD"),
                            Estatus = Datos.Int(row, "ESN_ACTIVO")
                        };
                        responseDB.Data.Add(Estados);
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
                responseDB.Data = new List<Estados>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<Estados> GetEstados_ById(int Entidad, int IdEstado)
        {
            var responseDB = new DBResponse<Estados>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Estados();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ESN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEstado),
                    Db.CreateParameter("p_esn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_estados.consulta_estado", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var Estados = new Estados()
                    {
                        Id = Datos.Int(row, "ESN_ID"),
                        Estado = Datos.Str(row, "ESC_NOMBRE"),
                        Entidad = Datos.Int(row, "ESN_ENTIDAD"),
                        Estatus = Datos.Int(row, "ESN_ACTIVO")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Data = Estados;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Estados();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Estados> ExisteEstado(int Entidad, string Estado)
        {
            var responseDB = new DBResponse<Estados>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ESC_NOMBRE", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, Estado),
                    Db.CreateParameter("p_esn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_estados.existe_estado", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = new Estados();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new Estados()
                    {
                        Id = Datos.Int(row, "ESN_ID"),
                        Estado = Datos.Str(row, "ESC_NOMBRE"),
                    };
                    responseDB.Message = "el Estado ya existe en la compañía";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Estados();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }


        public DBResponse<Estados> UpsertEstado(Estados Estados, Boolean nRow)
        {
            var dbResponse = new DBResponse<Estados>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ESN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_ESC_NOMBRE", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, Estados.Estado),
                    Db.CreateParameter("p_ESN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_ESN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Estados.Entidad),
                    Db.CreateParameter("p_ESN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, Estados.Id)
                };
                if (nRow)
                {
                    Db.Insert("spcpl_estados_op.agregar", CommandType.StoredProcedure, list);
                }
                else
                {
                    Db.Update("spcpl_estados_op.modificar", CommandType.StoredProcedure, list);
                }
                dbResponse.Data = Estados;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente el Estado";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<DBNull> CambiaEstatusEstado(int IdEstado)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ESN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEstado)
                };

                Db.Execute("spcpl_estados_op.cambia_estatus", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el Estado";
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
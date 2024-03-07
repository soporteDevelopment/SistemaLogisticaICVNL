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
    public class Archivos_DA
    {
        public DBResponse<List<Archivos>> GetArchivos_List(int IdOrigen, string TablaOrigen, int Entidad)
        {
            var responseDB = new DBResponse<List<Archivos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Archivos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ARN_IDORIGEN", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrigen),
                    Db.CreateParameter("p_ARC_TABLAORIGEN", DbType.String, 30, ParameterDirection.Input, false, null, DataRowVersion.Default, TablaOrigen),
                    Db.CreateParameter("p_ARN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_archivos.consulta_archivo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var archivos = new Archivos()
                        {
                            IdArchivo = Datos.Int(row, "ARN_ID"),
                            IdOrigen = Datos.Int(row, "ARN_IDORIGEN"),
                            TablaOrigen = Datos.Str(row, "ARC_TABLAORIGEN"),
                            Entidad = Datos.Int(row, "ARN_ENTIDAD"),
                            Borrado = Datos.Int(row, "ARN_BORRADO"),
                            NombreArchivo = Datos.Str(row, "ARC_ARCHIVO"),
                            Archivo = Datos.ArrBytes(row, "ARB_ARCHIVO")
                        };
                        responseDB.Data.Add(archivos);
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
                responseDB.Data = new List<Archivos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Archivos> GetArchivo(int IdArchivo, int Entidad)
        {
            var responseDB = new DBResponse<Archivos>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Archivos();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ARN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdArchivo),
                    Db.CreateParameter("p_ARN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_archivos.consulta_archivo_i", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }

                else
                {
                    responseDB.Data = new Archivos()
                    {
                        IdOrigen = Datos.Int(row, "ARN_IDORIGEN"),
                        TablaOrigen = Datos.Str(row, "ARC_TABLAORIGEN"),
                        Entidad = Datos.Int(row, "ARN_ENTIDAD"),
                        Borrado = Datos.Int(row, "ARN_BORRADO"),
                        NombreArchivo = Datos.Str(row, "ARC_ARCHIVO"),
                        Archivo = Datos.ArrBytes(row, "ARB_ARCHIVO")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = 1;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Archivos();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DBNull> EliminaArchivo(int IdArchivo)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false; 
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ARN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdArchivo), 
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                Db.Execute("spcpl_archivos_op.eliminar_archivos_i", CommandType.StoredProcedure, list);
                responseDB.ExecutionOK = true;

            }
            catch (Exception ex)
            { 
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<DBNull> InsertaArchivo(Archivos archivos)
        {
            var responseDB = new DBResponse<DBNull>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> listArchivos = new IListArchivos().ParametersAgregaArchivos(archivos);
                Db.Insert("spcpl_archivos_op.agregar_archivo", CommandType.StoredProcedure, listArchivos);

                responseDB.ExecutionOK = true;

            }
            catch (Exception ex)
            {
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
    }
}

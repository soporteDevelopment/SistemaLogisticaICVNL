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
    public class Proveedores_DA
    {

        public DBResponse<List<Proveedores>> GetProveedores_List(string NumeroProv, string Proveedor, int? Estatus, int Entidad)
        {
            var responseDB = new DBResponse<List<Proveedores>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Proveedores>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroProv),
                    Db.CreateParameter("p_PROVC_NOMBRE", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, Proveedor),
                    Db.CreateParameter("p_PROVN_ACTIVO", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Estatus),
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_proveedores.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var Proveedores = new Proveedores()
                        {
                            Id = Datos.Int(row, "PROVN_ID"),
                            NumeroProveedor = Datos.Str(row, "PROVC_NUMERO"),
                            NombreProveedor = Datos.Str(row, "PROVC_NOMBRE"),
                            EmailProveedor = Datos.Str(row, "PROVC_EMAIL"),
                            Estatus = Datos.Int(row, "PROVN_ACTIVO")
                        };
                        responseDB.Data.Add(Proveedores);
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
                responseDB.Data = new List<Proveedores>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<Proveedores> GetProveedores_ById( int IdProveedor, int Entidad)
        {
            var responseDB = new DBResponse<Proveedores>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Proveedores();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_proveedores.consulta_proveedor", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    var Proveedores = new Proveedores()
                    {
                        Id = Datos.Int(row, "PROVN_ID"),
                        NumeroProveedor = Datos.Str(row, "PROVC_NUMERO"),
                        NombreProveedor = Datos.Str(row, "PROVC_NOMBRE"),
                        EmailProveedor = Datos.Str(row, "PROVC_EMAIL"),
                        Estatus = Datos.Int(row, "PROVN_ACTIVO")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Data = Proveedores;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Proveedores();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Proveedores> ExisteProveedor(int Entidad, string NumeroProveedor)
        {
            var responseDB = new DBResponse<Proveedores>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVC_NUMERO", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroProveedor),
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_proveedores.existe_proveedor", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = new Proveedores();
                }
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new Proveedores()
                    {
                        Id = Datos.Int(row, "PROVN_ID"),
                        NombreProveedor = Datos.Str(row, "PROVC_NOMBRE"),
                    };
                    responseDB.Message = "el Proveedor ya existe en la compañía";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Proveedores();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Proveedores> UpsertProveedor(Proveedores Proveedores, Boolean nRow)
        {
            var dbResponse = new DBResponse<Proveedores>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_PROVN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Proveedores.Entidad),
                    Db.CreateParameter("p_PROVC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, Proveedores.NumeroProveedor), 
                    Db.CreateParameter("p_PROVC_EMAIL", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, Proveedores.EmailProveedor),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, Proveedores.Id),
                    Db.CreateParameter("p_PROVC_NOMBRE", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, Proveedores.NombreProveedor),
                };
                if (nRow)
                {
                    Db.Insert("spcpl_proveedores_op.agregar", CommandType.StoredProcedure, list);
                }
                else
                {
                    Db.Update("spcpl_proveedores_op.modificar", CommandType.StoredProcedure, list);
                }
                dbResponse.Data = Proveedores;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente el Proveedor";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<DBNull> CambiaEstatusProveedor(int IdProveedor)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor)
                };

                Db.Execute("spcpl_proveedores_op.cambia_estatus", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el Proveedor";
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

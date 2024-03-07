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
    public class Usuarios_API_DA
    {
        public DBResponse<Api_UsuarioICVNL> GetUserAPI_Dev(string usuario, string password)
        {
            var responseDB = new DBResponse<Api_UsuarioICVNL>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Api_UsuarioICVNL();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_USRC_USUARIO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, usuario),
                    Db.CreateParameter("p_USRC_PASSWORD", DbType.String, 64, ParameterDirection.Input, false, null, DataRowVersion.Default, password),
                    Db.CreateParameter("p_USRN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_usuario_api.consulta_usuario_api_dev", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "Usuario inválido";
                    responseDB.NumRows = 0;
                    return responseDB;
                }

                else
                {
                    responseDB.Data = new Api_UsuarioICVNL()
                    {
                        Usuario = Datos.Str(row, "USRC_USUARIO"),
                        Password = Datos.Str(row, "USRC_PASSWORD")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = 1;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Api_UsuarioICVNL();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Api_UsuarioICVNL> GetUserAPI()
        {
            var responseDB = new DBResponse<Api_UsuarioICVNL>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Api_UsuarioICVNL();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_usuario_api.consulta_usuario_api", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No existen usuarios";
                    responseDB.NumRows = 0;
                    return responseDB;
                }

                else
                {
                    responseDB.Data = new Api_UsuarioICVNL()
                    {
                        Usuario = Datos.Str(row, "USRC_USUARIO"),
                        Password = Datos.Str(row, "USRC_PASSWORD")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = 1;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Api_UsuarioICVNL();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
    }
}

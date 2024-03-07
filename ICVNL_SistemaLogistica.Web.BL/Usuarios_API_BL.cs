using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class Usuarios_API_BL
    {
        public DBResponse<Api_UsuarioICVNL> GetUserAPI_Dev(string usuario, string password)
        {
            return new Usuarios_API_DA().GetUserAPI_Dev(usuario, password);
        }
        public DBResponse<Api_UsuarioICVNL> GetUserAPI()
        {
            return new Usuarios_API_DA().GetUserAPI();
        }
    }
}

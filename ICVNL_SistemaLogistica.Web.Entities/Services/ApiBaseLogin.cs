using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services
{
    public class ApiBaseLogin
    {
        public string UrlBase { get; set; }
        public string Prefijo { get; set; }
        public string Controlador { get; set; }
        public string UsuarioApi { get; set; }
        public string PasswordApi { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class Usuario
    {
        public int clave { get; set; }
    }
    public class PostUsuario
    {
        public Usuario Usuario { get; set; }
        public string AccessToken { get; set; }
    }

}

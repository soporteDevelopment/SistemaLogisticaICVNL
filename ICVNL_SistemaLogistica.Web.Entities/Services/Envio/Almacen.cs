using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{

    public class Almacen
    {
        public int empresa { get; set; } 
        public int Usuario { get; set; }
    }

    public class PostAlmacen
    {
        public Almacen AlmacenUsuario { get; set; }
        public string AccessToken { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class Articulo
    {
        public int empresa { get; set; }
        public string articulo { get; set; }
    }

    public class PostArticulo
    {
        public Articulo Articulo { get; set; }
        public string AccessToken { get; set; }
    }
}

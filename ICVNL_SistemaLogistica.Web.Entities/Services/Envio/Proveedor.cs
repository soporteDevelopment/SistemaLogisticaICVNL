using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class Proveedor
    {
        public int empresa { get; set; }
        public string proveedor { get; set; }
    }

    public class PostProveedor
    {
        public Proveedor Proveedor { get; set; }
        public string AccessToken { get; set; }
    }

}

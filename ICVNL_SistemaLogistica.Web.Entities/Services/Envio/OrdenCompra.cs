using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class OrdenCompra
    {
        public int empresa { get; set; }
        public DateTime fecha { get; set; }
    }

    public class PostOrdenCompra
    {
        public OrdenCompra OrdenCompra { get; set; }
        public string AccessToken { get; set; }
    }
}

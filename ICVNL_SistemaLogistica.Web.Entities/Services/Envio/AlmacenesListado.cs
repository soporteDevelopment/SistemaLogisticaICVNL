using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class AlmacenesListado
    {
        public int empresa { get; set; }
    }

    public class PostAlmacenesListado
    {
        public AlmacenesListado Almacen { get; set; }
        public string AccessToken { get; set; }
    }
}

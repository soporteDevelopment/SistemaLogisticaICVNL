using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class Cuenta
    {
        public int empresa { get; set; }
        public string cuenta_sin_formato { get; set; }
        public int tipos_cuentas { get; set; }
        public int niveles { get; set; }
    }

    public class PostCuenta
    {
        public Cuenta Cuenta { get; set; }
        public string AccessToken { get; set; }
    }
}

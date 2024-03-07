using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class Asiento
    {
        public string cuenta { get; set; }
        public string centro_costos { get; set; }
        public string descripcion { get; set; }
        public int tipo { get; set; }
        public decimal monto { get; set; }
        public decimal tipo_cambio { get; set; }
    }

    public class Poliza
    {
        public int empresa { get; set; }
        public int tipo_poliza { get; set; }
        public string descripcion { get; set; }
        public int usuario_poliza { get; set; }
        public string origen { get; set; }
        public List<Asiento> asientos { get; set; }
    }

    public class PostPolizas
    {
        public Poliza Poliza { get; set; }
        public string AccessToken { get; set; }
    }
}

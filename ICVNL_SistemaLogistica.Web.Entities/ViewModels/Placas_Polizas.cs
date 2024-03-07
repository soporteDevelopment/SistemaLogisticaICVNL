using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Placas_Polizas
    {
        public string NumeroPlaca { get; set; }
        public string CuentaContableCargo { get; set; }
        public string CentroCostosAlmacen { get; set; }
        public decimal ImportePlacas { get; set; }
    }

    public class Placas_Polizas_Sum
    { 
        public string CuentaContableCargo { get; set; }
        public string CentroCostosAlmacen { get; set; }
        public decimal ImportePlacas { get; set; }
    }
}

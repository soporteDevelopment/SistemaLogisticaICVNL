using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class InventarioPlacas
    {
        public int IdInventario { get; set; }
        public int IdDelegacionBanco { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public DateTime FechaInventario { get; set; }
        public int Entidad { get; set; }

        public List<InventarioPlacas_Existencia> InventarioPlacas_Existencia { get; set; }
        public List<InventarioPlacas_TotalesExistencia> InventarioPlacas_TotalesExistencia { get; set; }
        public List<InventarioPlacas_Detalle> InventarioPlacas_Detalle { get; set; }
    }
}

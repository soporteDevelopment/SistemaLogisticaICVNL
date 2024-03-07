using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class InventarioPlacas_Existencia
    {
        public int IdInventarioExistencia { get; set; }
        public int IdInventario { get; set; }
        public string NumeroPlaca { get; set; }
        public int IdEstatusPlaca { get; set; }
        public TiposEstatusPlacas TiposEstatusPlacas { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int Existencia { get; set; }
        public List<InventarioPlacas_Existencia_HistorialCambios> HistorialCambios { get; set; }
        public Boolean PlacaContabilizada { get; set; }
        public string NumeroPolizaGRP_Infofin { get; set; }
        public decimal CostoPlaca { get; set; }
    }
}

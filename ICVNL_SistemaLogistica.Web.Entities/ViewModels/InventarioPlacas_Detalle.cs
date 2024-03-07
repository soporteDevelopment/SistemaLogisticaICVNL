using ICVNL_SistemaLogistica.Web.Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class InventarioPlacas_Detalle
    {
        public int IdInventarioDetalle { get; set; }
        public int IdInventario { get; set; }
        public string NumeroPlaca { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int Existencia { get; set; }
        public int IdEstatusPlacas { get; set; }
        public TiposEstatusPlacas EstatusPlacas { get; set; }
        public string Serie { get; set; }
        public string ExistenciaDesde { get; set; }
        public string ExistenciaHasta { get; set; }
        public string ExistenciaTotalRango { get; set; }
        public decimal CostoPlaca { get; set; }
        public string NumeroPolizaInfofin { get; set; }
        public Boolean PlacaContabilizada { get; set; }
        public int Bloque { get; set; }
        public int Entidad { get; set; }
        public DateTime FechaEntrada { get; set; }
        public InventarioPlacas_Relacion_NE_OC InventarioPlacas_Relacion_NE_OC { get; set; }
    }
}

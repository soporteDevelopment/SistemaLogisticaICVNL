using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class InventarioPlacas_Existencia_HistorialCambios
    {
        public int IdInventarioExistenciaHistorial { get; set; }
        public int IdInventarioExistencia { get; set; }
        public DateTime FechaOperacion { get; set; }
        public int IdEstatusAnterior { get; set; }
        public TiposEstatusPlacas EstatusPlacasAnterior { get; set; }
        public int IdEstatusNuevo { get; set; }
        public TiposEstatusPlacas EstatusPlacasNuevo { get; set; }
        public string Operacion { get; set; }

    }
}

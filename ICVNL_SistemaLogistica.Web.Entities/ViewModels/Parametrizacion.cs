using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Parametrizacion
    {
        public int IdParametrizacion { get; set; }
        public string EmailDestinatariosSolicitudPlacas { get; set; }
        public string EmailDestinatariosNotificaPlacasVendidas { get; set; }
        public string ClaveEntidadGobierno { get; set; }
        public string CuentaCostosVentaPlacaVendida { get; set; }
        public string CentroCostosEntidadGobiernoPlacaVendida { get; set; }
        public string CuentaCostosVentaPlacaReporte { get; set; }
        public string CentroCostosEntidadGobiernoPlacaReporte { get; set; } 
        public DateTime? FechaUltimaEjecucionNE { get; set; }
        public int TipoPolizaPlacaVendida { get; set; }
        public int TipoPolizaPlacaReporte { get; set; }
        public int Entidad { get; set; }

    }
}

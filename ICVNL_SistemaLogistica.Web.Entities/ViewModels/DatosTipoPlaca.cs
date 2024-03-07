using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class DatosTipoPlaca
    {
        public int IdDetalleOrdenCompra { get; set; }
        public int CantidadPlacasCaja { get; set; }
        public int CantidadPlacas { get; set; }
        public string MascaraPlaca { get; set; }
        public string OrdenPlaca { get; set; }
        public string RangoFinal { get; set; }
        public string RangoInicial { get; set; }
    }
}

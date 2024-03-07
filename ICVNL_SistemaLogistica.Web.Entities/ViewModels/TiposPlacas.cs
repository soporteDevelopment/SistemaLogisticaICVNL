using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TiposPlacas
    {
        public int IdTipoPlaca { get; set; }
        public string TipoPlaca { get; set; }
        public string CodigoInfofin { get; set; }
        public int CantidadPlacas { get; set; }
        public int CantidadPlacasCaja { get; set; }
        public string MascaraPlaca { get; set; }
        public string OrdenPlaca { get; set; }
        public string OrdenSeriePlaca { get; set; }
        public int Entidad { get; set; }
        public int Estatus { get; set; } 
    }
}

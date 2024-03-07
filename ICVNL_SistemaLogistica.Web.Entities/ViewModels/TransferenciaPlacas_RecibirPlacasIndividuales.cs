using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas_RecibirPlacasIndividuales
    {
        public int IdTransferencia { get; set; }
        public int IdTransferenciaIndividual { get; set; }
        public int IdTransferenciaListado2 { get; set; } 
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int IdTipoEstatusPlacas { get; set; }
        public TiposEstatusPlacas TiposEstatusPlacas { get; set; }
        public string NumeroPlaca { get; set; }
        public Boolean Disponible { get; set; }
        public Boolean Obsoleta { get; set; }
        public Boolean Rechazada { get; set; }
        public Boolean Perdida { get; set; }
    }
}

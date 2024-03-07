using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas_Detalle
    {
        public int IdTransferenciaDet { get; set; }
        public int IdTransferencia { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int IdTipoEstatusPlacas { get; set; }
        public TiposEstatusPlacas TiposEstatusPlacas { get; set; } 
        public Boolean Automatico { get; set; }
        public int TransferirPlacaIndividual { get; set; }
        public Boolean TransferirPlaca { get; set; }
        public Boolean PlacaSeleccionadaManual { get; set; }
        public Boolean PlacaSeleccionadaAutomatica { get; set; }
        public string NumeroPlaca { get; set; }
        public DateTime FechaEstatusPlaca { get; set; }
        public string UsuarioRecibio { get; set; }
        public int Entidad { get; set; }
    }
}

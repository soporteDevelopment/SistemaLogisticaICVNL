using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas_Listado2
    {
        public int IdTransferenciaListado2 { get; set; }
        public int IdTransferencia { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int IdTipoEstatusPlacas { get; set; }
        public TiposEstatusPlacas TiposEstatusPlacas { get; set; }
        public Boolean Automatico { get; set; }
        public Boolean TransferirPlaca { get; set; }
        public string NumeroPlaca { get; set; }
    }
}

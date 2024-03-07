using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class ConsultaInformacionPlacas
    { 
        public string NumeroPlaca { get; set; }
        public DateTime FechaNotaEntrada { get; set; }
        public string NumeroNotaEntrada { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public string DelegacionPlaca { get; set; }
        public int IdTipoEstatusPlaca { get; set; }
        public int PlacaContabilizada { get; set; }
    }
}

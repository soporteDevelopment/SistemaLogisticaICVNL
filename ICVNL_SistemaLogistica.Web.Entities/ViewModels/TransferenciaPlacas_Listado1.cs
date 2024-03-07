using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas_Listado1
    {
        public int IdTransferenciaListado1 { get; set; }
        public int IdTransferencia { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int CantidadDisponiblesDelegacion { get; set; }
        public int CantidadDisponiblesSerTransferidas { get; set; }

    }
}

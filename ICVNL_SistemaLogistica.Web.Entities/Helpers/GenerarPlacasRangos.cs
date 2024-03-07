using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Helpers
{
    public class GenerarPlacasRangos
    {
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TipoPlaca { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public int Cantidad { get; set; }
        public int CantidadSolicitadaNotaEntrada { get; set; }
    }
}

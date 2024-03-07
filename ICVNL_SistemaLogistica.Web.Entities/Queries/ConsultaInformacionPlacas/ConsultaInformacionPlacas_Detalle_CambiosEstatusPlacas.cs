using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas
    {
        public string NumeroPlaca { get; set; }
        public DateTime FechaHoraCambioEstatus { get; set; }
        public string OperacionRealizada { get; set; }
        public string EstatusAnterior { get; set; }
        public string EstatusNuevo { get; set; }
    }
}

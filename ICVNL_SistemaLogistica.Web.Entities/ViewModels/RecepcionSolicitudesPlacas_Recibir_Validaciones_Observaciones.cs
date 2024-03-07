using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones
    {
        public int IdObservacion { get; set; } 
        public int IdEventoRecepcion { get; set; }
        public int IdRecepcionValidaciones { get; set; }
        public string Observacion { get; set; }
        public int Entidad { get; set; }

 
    }
}

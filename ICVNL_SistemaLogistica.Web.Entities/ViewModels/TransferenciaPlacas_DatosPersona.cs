using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas_DatosPersona
    {
        public int IdTransferenciaDatosPersona { get; set; }
        public int IdTransferencia { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int IdTipoIDs { get; set; }
        public TiposIDs TiposID { get; set; }
        public string NumeroID { get; set; }
        public int Entidad { get; set; }
        public string Tipo { get; set; }
    }
}

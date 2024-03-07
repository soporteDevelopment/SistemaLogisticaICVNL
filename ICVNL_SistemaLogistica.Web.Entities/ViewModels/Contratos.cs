using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Contratos
    {
        public int IdContrato { get; set; }
        public string NumeroContrato { get; set; }
        public DateTime FechaContrato { get; set; }
        public List<Contratos_Detalle> Contratos_Detalle { get; set; } = new List<Contratos_Detalle>();
        public List<Contratos_Archivos> Contratos_Archivos { get; set; } = new List<Contratos_Archivos>();
        public int Entidad { get; set; }
        public string Usuario { get; set; }
         
    }
}

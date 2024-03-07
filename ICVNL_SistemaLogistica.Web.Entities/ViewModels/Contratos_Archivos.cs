using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Contratos_Archivos
    {
        public int IdContrato { get; set; }
        public int Consecutivo { get; set; }
        public Byte[] Archivo { get; set; }
        public string NombreArchivo { get; set; }
    }
}

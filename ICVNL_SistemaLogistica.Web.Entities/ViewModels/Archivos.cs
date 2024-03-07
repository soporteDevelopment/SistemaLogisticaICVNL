using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Archivos
    {
        public int IdArchivo { get; set; }
        public string NombreArchivo { get; set; }
        public Byte[] Archivo { get; set; }
        public int IdOrigen { get; set; }
        public string TablaOrigen { get; set; }
        public int Entidad { get; set; }
        public int Borrado { get; set; }
    }
}

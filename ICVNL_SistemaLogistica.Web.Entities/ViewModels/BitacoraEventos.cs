using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class BitacoraEventos
    {
        public int IdBitacora { get; set; }
        public DateTime FechaEvento { get; set; }
        public string LugarEvento { get; set; }
        public string InstruccionRealizada { get; set; }
        public string Usuario { get; set; }
        public string Evento { get; set; }
        public string IP_Usuario { get; set; }
        public string JsonObject { get; set; }
        public int Entidad { get; set; }
    }
}

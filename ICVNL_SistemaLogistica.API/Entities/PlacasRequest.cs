using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.API.Entities
{
    public class PlacasRequest
    {
        public string NumeroPlaca { get; set; }
        public string AlmacenCentroCostos { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string ReferenciaInfofin { get; set; }
        public string TipoOperacion { get; set; }
        public decimal PrecioVenta { get; set; }
        public string UsuarioRegistro { get; set; }
        public string IP_Usuario { get; set; }
    }
}

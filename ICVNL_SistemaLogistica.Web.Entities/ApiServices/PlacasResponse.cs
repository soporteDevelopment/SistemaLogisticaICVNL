using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.ApiServices
{
    public class PlacasResponse
    {
        public int IdPlacaEntradaVenta { get; set; }
        public int IdInventarioDetalle { get; set; }
        public int IdDelegacionBanco { get; set; }
        public DateTime FechaOperacionEntrega { get; set; }
        public DateTime FechaOperacionVenta { get; set; }
        public string ReferenciaInfofin { get; set; }
        public string TipoOperacion { get; set; }
        public decimal PrecioVenta { get; set; }
        public string UsuarioRegistro { get; set; } 
        public string IP_Usuario { get; set; }
        public int Entidad { get; set; }

    }
}

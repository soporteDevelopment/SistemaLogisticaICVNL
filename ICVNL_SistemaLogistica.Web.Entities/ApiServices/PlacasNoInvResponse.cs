using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.ApiServices
{
    public class PlacasNoInvResponse
    {
        public int IdPlacaNoInv { get; set; }
        public string NumeroPlaca { get; set; }
        public int IdDelegacionBanco { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public string DelegacionBanco { get; set; } 
        public DateTime FechaOperacionVenta { get; set; }
        public int Entidad { get; set; }
    }
}

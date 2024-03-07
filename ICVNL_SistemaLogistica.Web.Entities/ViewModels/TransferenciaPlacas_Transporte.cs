using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas_Transporte
    {
        public int IdTransferenciaTransporte { get; set; }
        public int IdTransferencia { get; set; }
        public string MarcaVehiculo { get; set; }
        public string ModeloVehiculo { get; set; }
        public string PlacasVehiculo { get; set; }
        public string NumeroEconomico { get; set; }

        public int Entidad { get; set; }
        public string Tipo { get; set; }
    }
}

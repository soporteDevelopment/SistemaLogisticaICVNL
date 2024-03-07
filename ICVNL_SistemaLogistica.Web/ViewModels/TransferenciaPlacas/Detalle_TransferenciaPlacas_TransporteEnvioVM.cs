using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_TransferenciaPlacas_TransporteEnvioVM
    {
        public int IdTransferenciaTransporteEnvio { get; set; }
        public int IdTransferencia { get; set; }

        [Display(Name = "Marca Vehículo")]
        public string MarcaVehiculo { get; set; }

        [Display(Name = "Modelo Vehículo")]
        public string ModeloVehiculo { get; set; }

        [Display(Name = "Placas Vehículo")]
        public string PlacasVehiculo { get; set; }

        [Display(Name = "Número Economico")]
        public string NumeroEconomico { get; set; }

        public static Detalle_TransferenciaPlacas_TransporteEnvioVM operator +(Detalle_TransferenciaPlacas_TransporteEnvioVM placas_TransporteEnvioVM, TransferenciaPlacas_Transporte _TransporteEnvio)
        {
            placas_TransporteEnvioVM.IdTransferenciaTransporteEnvio = _TransporteEnvio.IdTransferenciaTransporte;
            placas_TransporteEnvioVM.IdTransferencia = _TransporteEnvio.IdTransferencia;
            placas_TransporteEnvioVM.MarcaVehiculo = _TransporteEnvio.MarcaVehiculo;
            placas_TransporteEnvioVM.ModeloVehiculo = _TransporteEnvio.ModeloVehiculo;
            placas_TransporteEnvioVM.PlacasVehiculo = _TransporteEnvio.PlacasVehiculo;
            placas_TransporteEnvioVM.NumeroEconomico = _TransporteEnvio.NumeroEconomico;
            return placas_TransporteEnvioVM;
        }
    }
}
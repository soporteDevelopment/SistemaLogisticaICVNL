using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ContratosDetailsRangosVM
    {
        public int Id { get; set; }
        public int Consecutivo { get; set; }
        public int IdDetalle { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Rango Inicial Placas")]
        public string RangoInicial { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Rango Final Placas")]
        public string RangoFinal { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad por Serie")]
        public string CantidadSerie { get; set; }

        public static Detalle_ContratosDetailsRangosVM operator +(Detalle_ContratosDetailsRangosVM detalle_ContratosDetailsRangosVM, Contratos_Detalles_Rangos contratos_Detalles_Rangos)
        {
            detalle_ContratosDetailsRangosVM.Id = contratos_Detalles_Rangos.IdContratoDetalle;
            detalle_ContratosDetailsRangosVM.Consecutivo = contratos_Detalles_Rangos.Consecutivo;
            detalle_ContratosDetailsRangosVM.RangoInicial = contratos_Detalles_Rangos.RangoInicial;
            detalle_ContratosDetailsRangosVM.RangoFinal = contratos_Detalles_Rangos.RangoFinal;
            detalle_ContratosDetailsRangosVM.CantidadSerie = contratos_Detalles_Rangos.CantidadSerie;


            return detalle_ContratosDetailsRangosVM;
        }
    }
}
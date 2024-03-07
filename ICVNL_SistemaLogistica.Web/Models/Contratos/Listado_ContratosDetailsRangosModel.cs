using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ContratosDetailsRangosModel
    {
        public int IdContratoDetalleRangos { get; set; }
        public int Consecutivo { get; set; }
        public int IdContratoDetalle { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public string CantidadSerie { get; set; }

        public static Listado_ContratosDetailsRangosModel operator +(Listado_ContratosDetailsRangosModel detalle_ContratosDetailsRangosVM, Contratos_Detalles_Rangos contratos_Detalles_Rangos)
        {
            detalle_ContratosDetailsRangosVM.IdContratoDetalleRangos = contratos_Detalles_Rangos.IdContratoDetalleRangos;
            detalle_ContratosDetailsRangosVM.IdContratoDetalle = contratos_Detalles_Rangos.IdContratoDetalle;
            detalle_ContratosDetailsRangosVM.Consecutivo = contratos_Detalles_Rangos.Consecutivo;
            detalle_ContratosDetailsRangosVM.RangoInicial = contratos_Detalles_Rangos.RangoInicial;
            detalle_ContratosDetailsRangosVM.RangoFinal = contratos_Detalles_Rangos.RangoFinal;
            detalle_ContratosDetailsRangosVM.CantidadSerie = contratos_Detalles_Rangos.CantidadSerie;
            return detalle_ContratosDetailsRangosVM;
        }
    }
}
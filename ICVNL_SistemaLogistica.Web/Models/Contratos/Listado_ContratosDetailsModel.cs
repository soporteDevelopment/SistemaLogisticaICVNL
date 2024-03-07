using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ContratosDetailsModel
    {
        public int IdContratoDetalle { get; set; }
        public int IdContrato { get; set; }
        public int Consecutivo { get; set; }
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TipoPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int CantidadPlacas { get; set; }
        public int CantidadPlacasCaja { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public string OficioSICT { get; set; }

        public List<Listado_ContratosDetailsRangosModel> Detalle_ContratosDetailsRangosVM { get; set; }
        public Detalle_ContratosDetailsRangosVM DetalleRangos { get; set; }

        public static Listado_ContratosDetailsModel operator +(Listado_ContratosDetailsModel detalle_ContratosDetailsVM, Contratos_Detalle contratos_Detalle)
        {
            detalle_ContratosDetailsVM.IdContratoDetalle = contratos_Detalle.IdContratoDetalle;
            detalle_ContratosDetailsVM.IdContrato = contratos_Detalle.IdContrato;
            detalle_ContratosDetailsVM.Consecutivo = contratos_Detalle.Consecutivo;
            detalle_ContratosDetailsVM.IdProveedor = contratos_Detalle.IdProveedor;
            detalle_ContratosDetailsVM.Proveedores += contratos_Detalle.Proveedores;
            detalle_ContratosDetailsVM.IdTipoPlaca = contratos_Detalle.IdTipoPlaca;
            detalle_ContratosDetailsVM.TipoPlacas += contratos_Detalle.TipoPlacas;
            detalle_ContratosDetailsVM.CantidadPlacas = contratos_Detalle.CantidadPlacas;
            detalle_ContratosDetailsVM.CantidadPlacasCaja = contratos_Detalle.CantidadPlacasCaja;
            detalle_ContratosDetailsVM.RangoInicial = contratos_Detalle.RangoInicial;
            detalle_ContratosDetailsVM.RangoFinal = contratos_Detalle.RangoFinal;
            detalle_ContratosDetailsVM.OficioSICT = contratos_Detalle.OficioSICT;

            detalle_ContratosDetailsVM.Detalle_ContratosDetailsRangosVM = new List<Listado_ContratosDetailsRangosModel>();
            foreach (var item in contratos_Detalle.Contratos_Detalles_Rangos)
            {
                detalle_ContratosDetailsVM.Detalle_ContratosDetailsRangosVM.Add(new Listado_ContratosDetailsRangosModel() + item);
            }
            return detalle_ContratosDetailsVM;
        }
    }
}
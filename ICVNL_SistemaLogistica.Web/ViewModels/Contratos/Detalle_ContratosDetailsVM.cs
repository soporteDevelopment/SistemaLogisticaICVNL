using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ContratosDetailsVM
    {
        public int Id { get; set; }
        public int IdContrato { get; set; }
        public int Consecutivo { get; set; }
        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Tipo de Placa")]
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TipoPlacas { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad de Placas")]
        public int CantidadPlacas { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad de Placas por Caja ")]
        public int CantidadPlacasCaja { get; set; }

        [Display(Name = "Estructura de la Placa ")]
        public string EstructuraPlaca { get; set; }

        [Display(Name = "Orden de la Placa ")]
        public string OrdenPlaca { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Rango Inicial Placas")]
        public string RangoInicial { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Rango Final Placas")]
        public string RangoFinal { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Oficio SICT")]
        public string OficioSICT { get; set; }

        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        public IEnumerable<dynamic> ListadoTiposPlacasDDL { get; set; }
        public List<Listado_ContratosDetailsRangosModel> Detalle_ContratosDetailsRangosVM { get; set; }
        public Detalle_ContratosDetailsRangosVM DetalleRangos { get; set; }

        public static Detalle_ContratosDetailsVM operator +(Detalle_ContratosDetailsVM detalle_ContratosDetailsVM, Contratos_Detalle contratos_Detalle)
        {
            detalle_ContratosDetailsVM.Id = contratos_Detalle.IdContratoDetalle;
            detalle_ContratosDetailsVM.IdContrato = contratos_Detalle.IdContrato;
            detalle_ContratosDetailsVM.Consecutivo = contratos_Detalle.Consecutivo;
            detalle_ContratosDetailsVM.IdProveedor = contratos_Detalle.IdProveedor;
            detalle_ContratosDetailsVM.Proveedores += contratos_Detalle.Proveedores;
            detalle_ContratosDetailsVM.TipoPlacas += contratos_Detalle.TipoPlacas;
            detalle_ContratosDetailsVM.CantidadPlacas = contratos_Detalle.CantidadPlacas;
            detalle_ContratosDetailsVM.CantidadPlacasCaja = contratos_Detalle.CantidadPlacasCaja;
            detalle_ContratosDetailsVM.EstructuraPlaca = contratos_Detalle.MascaraPlaca;
            detalle_ContratosDetailsVM.OrdenPlaca = contratos_Detalle.OrdenPlaca;
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
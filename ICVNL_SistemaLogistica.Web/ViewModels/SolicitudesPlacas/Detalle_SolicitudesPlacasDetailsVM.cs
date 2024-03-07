using ICVNL_SistemaLogistica.Web.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_SolicitudesPlacasDetailsVM
    {
        public int IdSolicitudDetalle { get; set; }
        public int IdSolicitud { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Delegación")]
        public int IdDelegacionBanco { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Tipo Placa")]
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Rango de Placas Inicial")]
        public string RangoPlacaInicial { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Rango de Placas Final")]
        public string RangoPlacaFinal { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad de Placas")]
        public int CantidadPlacas { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesBancosDDL { get; set; }
        public IEnumerable<dynamic> ListadoTiposPlacasDDL { get; set; }

        public static Detalle_SolicitudesPlacasDetailsVM operator +(Detalle_SolicitudesPlacasDetailsVM detalle_SolicitudesPlacasDetailsVM, SolicitudesPlacas_Detalle solicitudesPlacas_Detalle)
        {
            detalle_SolicitudesPlacasDetailsVM.IdSolicitudDetalle = solicitudesPlacas_Detalle.IdSolicitudDetalle;
            detalle_SolicitudesPlacasDetailsVM.IdSolicitud = solicitudesPlacas_Detalle.IdSolicitud;
            detalle_SolicitudesPlacasDetailsVM.IdDelegacionBanco = solicitudesPlacas_Detalle.IdDelegacionBanco;
            detalle_SolicitudesPlacasDetailsVM.DelegacionesBancos += solicitudesPlacas_Detalle.DelegacionesBancos;
            detalle_SolicitudesPlacasDetailsVM.IdTipoPlaca = solicitudesPlacas_Detalle.IdTipoPlaca;
            detalle_SolicitudesPlacasDetailsVM.TiposPlacas += solicitudesPlacas_Detalle.TiposPlacas;
            detalle_SolicitudesPlacasDetailsVM.RangoPlacaInicial = solicitudesPlacas_Detalle.RangoPlacaInicial;
            detalle_SolicitudesPlacasDetailsVM.RangoPlacaFinal = solicitudesPlacas_Detalle.RangoPlacaFinal;
            detalle_SolicitudesPlacasDetailsVM.CantidadPlacas = solicitudesPlacas_Detalle.CantidadPlacas;
            return detalle_SolicitudesPlacasDetailsVM;
        }
    }
}
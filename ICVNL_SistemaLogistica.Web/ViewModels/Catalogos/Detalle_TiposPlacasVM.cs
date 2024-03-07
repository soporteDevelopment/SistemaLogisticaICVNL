using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_TiposPlacasVM
    {
        [Key]
        [Display(Name = "Id del Tipo de Placa")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Tipo de Placa")]
        public string TipoPlaca { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Código de Artículo en Infofin")]
        public string CodigoInfofin { get; set; }
        [Display(Name = "Descripción del Artículo en Infofin")]
        public string DescripcionInfofin { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad de Láminas en un Juego de Placas")]
        public int CantidadPlacas { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad de Placas en una Caja")]
        public int CantidadPlacasCaja { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Mascara de la Placa ")]
        public string MascaraPlaca { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Orden de la Placa ")]
        public string OrdenPlaca { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Orden de la Serie de la Placa ")]
        public string OrdenSeriePlaca { get; set; }

        public static Detalle_TiposPlacasVM operator +(Detalle_TiposPlacasVM detalle_TiposPlacasVM, TiposPlacas tiposPlacas)
        {
            detalle_TiposPlacasVM = new Detalle_TiposPlacasVM();
            detalle_TiposPlacasVM.Id = tiposPlacas.IdTipoPlaca;
            detalle_TiposPlacasVM.TipoPlaca = tiposPlacas.TipoPlaca;
            detalle_TiposPlacasVM.CodigoInfofin = tiposPlacas.CodigoInfofin;
            detalle_TiposPlacasVM.CantidadPlacas = tiposPlacas.CantidadPlacas;
            detalle_TiposPlacasVM.CantidadPlacasCaja = tiposPlacas.CantidadPlacasCaja;
            detalle_TiposPlacasVM.MascaraPlaca = tiposPlacas.MascaraPlaca;
            detalle_TiposPlacasVM.OrdenPlaca = tiposPlacas.OrdenPlaca;
            detalle_TiposPlacasVM.OrdenSeriePlaca = tiposPlacas.OrdenSeriePlaca;
            return detalle_TiposPlacasVM;
        }
    }
}
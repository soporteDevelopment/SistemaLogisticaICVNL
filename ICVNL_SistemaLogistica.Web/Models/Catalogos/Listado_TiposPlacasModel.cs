using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_TiposPlacasModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Placa")]
        public string TipoPlaca { get; set; }

        [Display(Name = "Código Infofin")]
        public string CodigoInfofin { get; set; }

        [Display(Name = "Cantidad Láminas")]
        public int CantidadPlacas { get; set; }

        [Display(Name = "Cantidad Placas Caja")]
        public int CantidadPlacasCaja { get; set; }

        [Display(Name = "Mascara de la Placa")]
        public string MascaraPlaca { get; set; }

        [Display(Name = "Orden")]
        public string OrdenPlaca { get; set; }
        public int Estatus { get; set; }

        public static Listado_TiposPlacasModel operator +(Listado_TiposPlacasModel detalle_TiposPlacasVM, TiposPlacas tiposPlacas)
        {
            detalle_TiposPlacasVM.Id = tiposPlacas.IdTipoPlaca;
            detalle_TiposPlacasVM.TipoPlaca = tiposPlacas.TipoPlaca;
            detalle_TiposPlacasVM.CodigoInfofin = tiposPlacas.CodigoInfofin;
            detalle_TiposPlacasVM.CantidadPlacas = tiposPlacas.CantidadPlacas;
            detalle_TiposPlacasVM.CantidadPlacasCaja = tiposPlacas.CantidadPlacasCaja;
            detalle_TiposPlacasVM.MascaraPlaca = tiposPlacas.MascaraPlaca;
            detalle_TiposPlacasVM.OrdenPlaca = tiposPlacas.OrdenPlaca;
            detalle_TiposPlacasVM.Estatus = tiposPlacas.Estatus;
            return detalle_TiposPlacasVM;
        }
    }
}
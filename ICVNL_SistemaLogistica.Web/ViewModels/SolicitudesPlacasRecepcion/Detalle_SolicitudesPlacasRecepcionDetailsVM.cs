using ICVNL_SistemaLogistica.Web.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_SolicitudesPlacasRecepcionDetailsVM
    {
        public int IdRecepcion { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Tipo de Placa")]
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cant. Solicitada Orden de Compra")]
        public int CantidadSolicitadaOrdenCompra { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cant. de las Notas de Entrada Aut.")]
        public int CantidadNotasEntradaAutorizada { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cantidad Recibida")]
        public int CantidadRecibida { get; set; }
        public IEnumerable<dynamic> ListadoTiposPlacasDDL { get; set; }

        public static Detalle_SolicitudesPlacasRecepcionDetailsVM operator +(Detalle_SolicitudesPlacasRecepcionDetailsVM detalle_SolicitudesPlacasRecepcionDetailsVM, RecepcionSolicitudesPlacas_Detalle recepcionSolicitudesPlacas_Detalle)
        {
            detalle_SolicitudesPlacasRecepcionDetailsVM.IdRecepcion = recepcionSolicitudesPlacas_Detalle.IdRecepcion;
            detalle_SolicitudesPlacasRecepcionDetailsVM.IdTipoPlaca = recepcionSolicitudesPlacas_Detalle.IdTipoPlaca;
            detalle_SolicitudesPlacasRecepcionDetailsVM.TiposPlacas += recepcionSolicitudesPlacas_Detalle.TiposPlacas;
            detalle_SolicitudesPlacasRecepcionDetailsVM.CantidadSolicitadaOrdenCompra = recepcionSolicitudesPlacas_Detalle.CantidadSolicitadaOrdenCompra;
            detalle_SolicitudesPlacasRecepcionDetailsVM.CantidadNotasEntradaAutorizada = recepcionSolicitudesPlacas_Detalle.CantidadNotasEntradaAutorizada;
            detalle_SolicitudesPlacasRecepcionDetailsVM.CantidadRecibida = recepcionSolicitudesPlacas_Detalle.CantidadRecibida;
            return detalle_SolicitudesPlacasRecepcionDetailsVM;
        }

    }
}
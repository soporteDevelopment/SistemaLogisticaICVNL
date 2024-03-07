using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_SolicitudesPlacasRecepcionDetailsModel
    {
        public int Consecutivo { get; set; }
        public int IdRecepcion { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; }
        public int CantidadSolicitadaOrdenCompra { get; set; }
        public int CantidadNotasEntradaAutorizada { get; set; }
        public int CantidadRecibida { get; set; }

        public static Listado_SolicitudesPlacasRecepcionDetailsModel operator +(Listado_SolicitudesPlacasRecepcionDetailsModel listado_SolicitudesPlacasRecepcionDetails, RecepcionSolicitudesPlacas_Detalle recepcion)
        {
            listado_SolicitudesPlacasRecepcionDetails.IdRecepcion = recepcion.IdRecepcion;
            listado_SolicitudesPlacasRecepcionDetails.IdTipoPlaca = recepcion.IdTipoPlaca;
            listado_SolicitudesPlacasRecepcionDetails.TiposPlacas += recepcion.TiposPlacas;
            listado_SolicitudesPlacasRecepcionDetails.CantidadSolicitadaOrdenCompra = recepcion.CantidadSolicitadaOrdenCompra;
            listado_SolicitudesPlacasRecepcionDetails.CantidadNotasEntradaAutorizada = recepcion.CantidadNotasEntradaAutorizada;
            listado_SolicitudesPlacasRecepcionDetails.CantidadRecibida = recepcion.CantidadRecibida;
            return listado_SolicitudesPlacasRecepcionDetails;
        }
    }
}
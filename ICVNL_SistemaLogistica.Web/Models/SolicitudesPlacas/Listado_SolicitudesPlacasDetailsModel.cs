using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_SolicitudesPlacasDetailsModel
    {
        public int Consecutivo { get; set; }
        public int IdSolicitudDetalle { get; set; }
        public int IdSolicitud { get; set; }
        public int IdDelegacionBanco { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public string RangoPlacaInicial { get; set; }
        public string RangoPlacaFinal { get; set; }
        public int CantidadPlacas { get; set; }
        public int IdDetalleOrdenCompra { get; set; }
        public int CantidadPlacasOrdenCompra { get; set; }
        public int CantidadPlacasNotaEntrada { get; set; }
        public int Entidad { get; set; }

        public static Listado_SolicitudesPlacasDetailsModel operator +(Listado_SolicitudesPlacasDetailsModel listado_SolicitudesPlacasDetailsModel, SolicitudesPlacas_Detalle solicitudes)
        {
            listado_SolicitudesPlacasDetailsModel.IdSolicitudDetalle = solicitudes.IdSolicitudDetalle;
            listado_SolicitudesPlacasDetailsModel.IdSolicitud = solicitudes.IdSolicitud;
            listado_SolicitudesPlacasDetailsModel.IdDelegacionBanco = solicitudes.IdDelegacionBanco;
            listado_SolicitudesPlacasDetailsModel.DelegacionesBancos += solicitudes.DelegacionesBancos;
            listado_SolicitudesPlacasDetailsModel.IdTipoPlaca = solicitudes.IdTipoPlaca;
            listado_SolicitudesPlacasDetailsModel.TiposPlacas += solicitudes.TiposPlacas;
            listado_SolicitudesPlacasDetailsModel.RangoPlacaInicial = solicitudes.RangoPlacaInicial;
            listado_SolicitudesPlacasDetailsModel.RangoPlacaFinal = solicitudes.RangoPlacaFinal;
            listado_SolicitudesPlacasDetailsModel.CantidadPlacas = solicitudes.CantidadPlacas;
            listado_SolicitudesPlacasDetailsModel.IdDetalleOrdenCompra = solicitudes.IdDetalleOrdenCompra;
            listado_SolicitudesPlacasDetailsModel.CantidadPlacasOrdenCompra = solicitudes.CantidadPlacasOrdenCompra;
            listado_SolicitudesPlacasDetailsModel.CantidadPlacasNotaEntrada = solicitudes.CantidadPlacasNotasEntrada;
            listado_SolicitudesPlacasDetailsModel.Entidad = solicitudes.Entidad;
            return listado_SolicitudesPlacasDetailsModel;
        }
    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_SolicitudesPlacasRecepcionModel
    {
        public int IdRecepcion { get; set; }
        public int IdSolicitud { get; set; }
        public Detalle_SolicitudesPlacasVM SolicitudPlacas { get; set; } = new Detalle_SolicitudesPlacasVM();
        public string FolioRecepcion { get; set; }
        public string DelegacionBanco { get; set; }
        public string NumeroContraro { get; set; }
        public string OrdenCompra { get; set; }
        public string NotaEntrada { get; set; }
        public static Listado_SolicitudesPlacasRecepcionModel operator +(Listado_SolicitudesPlacasRecepcionModel listado_SolicitudesPlacasRecepcion, RecepcionSolicitudesPlacas recepcion)
        {
            listado_SolicitudesPlacasRecepcion.IdRecepcion = recepcion.IdRecepcion;
            listado_SolicitudesPlacasRecepcion.IdSolicitud = recepcion.IdSolicitud;
            listado_SolicitudesPlacasRecepcion.SolicitudPlacas += recepcion.SolicitudesPlacas;
            listado_SolicitudesPlacasRecepcion.FolioRecepcion = recepcion.FolioRecepcion;
            listado_SolicitudesPlacasRecepcion.DelegacionBanco = recepcion.DelegacionesBancos.NombreDelegacionBanco;
            listado_SolicitudesPlacasRecepcion.NumeroContraro = recepcion.SolicitudesPlacas.Contratos.NumeroContrato;
            listado_SolicitudesPlacasRecepcion.OrdenCompra = recepcion.SolicitudesPlacas.OrdenesCompra.NumeroOrdenCompra;
            listado_SolicitudesPlacasRecepcion.NotaEntrada = recepcion.NotaEntradaAutorizada;

            return listado_SolicitudesPlacasRecepcion;
        }


    }
}
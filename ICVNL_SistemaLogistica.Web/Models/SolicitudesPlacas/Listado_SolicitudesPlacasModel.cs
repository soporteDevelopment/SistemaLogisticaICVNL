using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_SolicitudesPlacasModel
    {
        public int IdSolicitud { get; set; }
        public string NombreProveedor { get; set; }
        public string NumeroContrato { get; set; }

        public static Listado_SolicitudesPlacasModel operator +(Listado_SolicitudesPlacasModel listado_SolicitudesPlacasModel, SolicitudesPlacas contratos)
        {
            listado_SolicitudesPlacasModel.IdSolicitud = contratos.IdSolicitud;
            listado_SolicitudesPlacasModel.NombreProveedor = contratos.Proveedores.NombreProveedor;
            listado_SolicitudesPlacasModel.NumeroContrato = contratos.Contratos.NumeroContrato;

            return listado_SolicitudesPlacasModel;
        }
    }
}
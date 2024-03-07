using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM
    {
        public int IdSolicitud { get; set; }
        public int IdRecepcion { get; set; }

        public Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM { get; set; }
        public List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel> Listado { get; set; }
        public List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel> ListadoCajasRecibidas { get; set; }

        public IEnumerable<dynamic> ListadoDelegacionesDDL { get; set; }
        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        public IEnumerable<dynamic> ListadoTiposPlacasDDL { get; set; }
    }
}
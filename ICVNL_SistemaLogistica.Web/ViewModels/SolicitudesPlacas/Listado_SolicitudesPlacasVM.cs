using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_SolicitudesPlacasVM
    {
        #region Filtros especiales de la página
        public string FiltroFolioSolicitud { get; set; }
        public string FiltroFechaSolicitud { get; set; }
        public string FiltroFechaEntrega { get; set; }
        public string FiltroIdProveedor { get; set; }
        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        public string FiltroNumeroContrato { get; set; }
        public string FiltroOrdenCompra { get; set; }

        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        #endregion
        public List<Listado_SolicitudesPlacasModel> Listado { get; set; }
    }
}
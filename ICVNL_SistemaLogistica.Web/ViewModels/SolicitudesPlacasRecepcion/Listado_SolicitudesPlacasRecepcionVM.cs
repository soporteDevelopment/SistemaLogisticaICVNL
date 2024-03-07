using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_SolicitudesPlacasRecepcionVM
    {
        #region Filtros especiales de la página
        public string FiltroFolioRecepcion { get; set; }
        public string FiltroFolioSolicitud { get; set; }
        public string FiltroIdDelegacionBanco { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesBancosDDL { get; set; }
        public string FiltroNumeroContrato { get; set; }
        public string FiltroIdProveedor { get; set; }
        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        public string FiltroOrdenCompra { get; set; }
        public string FiltroNotaEntrada { get; set; }

        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        #endregion
        public List<Listado_SolicitudesPlacasRecepcionModel> Listado { get; set; }
    }
}
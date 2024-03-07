using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_ContratosVM
    {
        #region Filtros especiales de la página
        public string FiltroNumeroContrato { get; set; }
        public string FiltroIdContrato { get; set; }
        public string FiltroIdProveedor { get; set; }
        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        #endregion
        public List<Listado_ContratosModel> Listado { get; set; }
    }
}
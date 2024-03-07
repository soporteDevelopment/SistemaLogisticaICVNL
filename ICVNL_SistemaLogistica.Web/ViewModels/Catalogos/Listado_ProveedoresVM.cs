using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_ProveedoresVM
    {
        #region Filtros especiales de la página
        public string FiltroNumeroProveedor { get; set; }
        public string FiltroNombreProveedor { get; set; }

        public int? FiltroEstatus { get; set; }
        public IEnumerable<dynamic> ListadoEstatusDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        #endregion
        public List<Listado_ProveedoresModel> Listado { get; set; }
    }
}
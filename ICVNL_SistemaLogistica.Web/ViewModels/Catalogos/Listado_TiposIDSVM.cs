using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_TiposIDSVM
    {
        #region Filtros especiales de la página
        public int? FiltroEstatus { get; set; }
        public IEnumerable<dynamic> ListadoEstatusDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales 
        #endregion
        public List<Listado_TiposIDSModel> Listado { get; set; }
    }
}
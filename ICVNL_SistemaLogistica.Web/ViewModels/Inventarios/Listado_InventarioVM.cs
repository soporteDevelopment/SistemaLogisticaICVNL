using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_InventarioVM
    {
        #region Filtros especiales de la página 
        public string FiltroIdDelegacion { get; set; }
        public string FiltroIdTipoPlaca { get; set; }
        public IEnumerable<dynamic> ListadoDelegaciones { get; set; }
        public IEnumerable<dynamic> ListadoTipoPlaca { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales 
        #endregion
        public List<Listado_InventarioPlacasModel> Listado { get; set; }
    }
}
using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_ConsultaInformacionPlacasVM
    {
        #region Filtros especiales de la página
        public string FiltroNumeroNotaEntrada { get; set; }
        public string FiltroFechaNotaEntrada { get; set; }
        public string FiltroNumeroOrdenCompra { get; set; }
        public string FiltroFechaOrdenCompra { get; set; }
        public string FiltroIdDelegacion { get; set; }
        public string FiltroNumeroPlaca { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        public int TotalFilas { get; set; } = 50;
        public int NumeroPagina { get; set; } = 50;
        #endregion
        public List<Listado_ConsultaPlacasModel> Listado { get; set; }
    }
}
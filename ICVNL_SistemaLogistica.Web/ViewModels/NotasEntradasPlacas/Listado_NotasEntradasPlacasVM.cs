using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_NotasEntradasPlacasVM
    {
        #region Filtros especiales de la página
        public string FiltroNumeroNotaEntrada { get; set; }
        public string FiltroFechaNotaEntrada { get; set; }
        public string FiltroNumeroOrden { get; set; }
        public string FiltroFechaOrdenCompra { get; set; }
        public string FiltroIdEstatus { get; set; }
        public IEnumerable<dynamic> ListadoEstatusNotasEntradasDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        public int TotalFilas { get; set; } = 50;
        public int NumeroPagina { get; set; } = 1;
        #endregion
        public List<Listado_NotasEntradasPlacasModel> Listado { get; set; }
    }
}
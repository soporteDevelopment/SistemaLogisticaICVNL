using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_RecepcionPlacasVM
    {
        #region Filtros especiales de la página
        public string FiltroFolioTransferencia { get; set; }
        public string FiltroFechaTransferencia { get; set; }
        public string FiltroNombrePersona { get; set; }
        public string FiltroApellidoPersona { get; set; }
        public string FiltroIdTipoIDs { get; set; }
        public string FiltroNumeroID { get; set; }
        public string FiltroIdDelegacionOrigen { get; set; }
        public string FiltroIdDelegacionDestino { get; set; }
        public string FiltroIdEstatusTransferencia { get; set; }
        public string FiltroIdEstatusPlacas { get; set; }
        public IEnumerable<dynamic> ListadoTiposIDsDDL { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesDDL { get; set; }
        public IEnumerable<dynamic> ListadoEstatusTransferenciaDDL { get; set; }
        public IEnumerable<dynamic> ListadoEstatusPlacasDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        public int TotalFilas { get; set; } = 50;
        public int NumeroPagina { get; set; } = 50;
        #endregion
        public List<Listado_RecepcionPlacasModel> Listado { get; set; }
    }
}
using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_RecepcionSolicitudesPlacas_EventosVM
    {

        #region Filtros especiales de la página
        public string FiltroFechaQR { get; set; }
        public string FiltroIdProveedor { get; set; }
        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        public string FiltroIdDelegacionBanco { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesBancosDDL { get; set; }
        public string FiltroIdTipoPlaca { get; set; }
        public IEnumerable<dynamic> ListadoTipoEstructuraDDL { get; set; }
        public string FiltroIdTiposEventosRecepcionPlacas { get; set; }
        public IEnumerable<dynamic> ListadoTiposEventosRecepcionPlacasDDL { get; set; }
        #endregion

        #region Datos Obligatorios para la paginación con sus valores iniciales
        #endregion
        public List<Listado_RecepcionSolicitudesPlacas_EventosModel> Listado { get; set; }


    }
}
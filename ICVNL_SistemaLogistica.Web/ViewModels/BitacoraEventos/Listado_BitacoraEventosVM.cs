using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_BitacoraEventosVM
    {
        public string FiltroFechaEvento { get; set; }
        public string FiltroLugarEvento { get; set; }
        public string FiltroEvento { get; set; }
        public string FiltroUsuario { get; set; }
        public string FiltroInstruccionRealizada { get; set; }
        public string FiltroIP { get; set; }
        public IEnumerable<dynamic> ListadoInstruccionesBitacoraDDL { get; set; }
        public IEnumerable<dynamic> ListadoLugarEventoDDL { get; set; }

        #region Datos Obligatorios para la paginación con sus valores iniciales
        #endregion
        public List<Listado_BitacoraEventosModel> Listado { get; set; }
    }
}
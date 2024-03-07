using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM
    {
        public int IdSolicitud { get; set; }
        public int IdRecepcion { get; set; }
        public int IdValidacionEvento { get; set; }
        public List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesModel> Listado { get; set; }
        public Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM DetalleVM { get; set; } = new Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM();
    }
}
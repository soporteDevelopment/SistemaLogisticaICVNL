using System;

namespace ICVNL_SistemaLogistica.Web.Entities.RolesPermisos
{
    public class Pantallas_RecepcionSolicitudesPlacasProveedor
    {
        public Boolean Acceso { get; set; }
        public Boolean Registrar { get; set; }
        public Boolean Actualizar { get; set; }
        public Boolean Eliminar { get; set; }
        public Boolean RecibirPlaca { get; set; } 
        public Boolean RecibirPlacaAcceso { get; set; }
        public Boolean RecibirPlacaLeerQR { get; set; } 
        public Boolean RecibirPlacaRegistrar { get; set; } 
        public Boolean RecibirPlacaActualizar { get; set; } 
        public Boolean RecibirPlacaEliminar { get; set; }
        public Boolean RecibirPlacaConsultaEvento { get; set; }
        public Boolean RecibirPlacaConsultaEventoDetalle { get; set; }
        public Boolean RecibirPlacaConsultaEventoDetalleAcceso { get; set; }
    }
}

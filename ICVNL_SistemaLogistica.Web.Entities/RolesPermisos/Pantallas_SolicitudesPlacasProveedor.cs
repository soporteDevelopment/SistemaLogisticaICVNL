using System;

namespace ICVNL_SistemaLogistica.Web.Entities.RolesPermisos
{
    public class Pantallas_SolicitudesPlacasProveedor
    {
        public Boolean Acceso { get; set; }
        public Boolean Registrar { get; set; }
        public Boolean Actualizar { get; set; }
        public Boolean Eliminar { get; set; }
        public Boolean GeneraPDF_SolicitudPlaca { get; set; }
        public Boolean GeneraPDF_EnvioEmailProveedor { get; set; }
    }
}

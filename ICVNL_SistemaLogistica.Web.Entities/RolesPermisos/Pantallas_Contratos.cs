using System;

namespace ICVNL_SistemaLogistica.Web.Entities.RolesPermisos
{
    public class Pantallas_Contratos
    {
        public Boolean Acceso { get; set; }
        public Boolean Registrar { get; set; }
        public Boolean Actualizar { get; set; }
        public Boolean Eliminar { get; set; }
        public Boolean RangoTipoPlacaValidarPlacas { get; set; }
        public Boolean RangoTipoPlaca  { get; set; }
        public Boolean RangoTipoPlacaAcceso { get; set; }
        public Boolean RangoTipoPlacaRegistro { get; set; }
        public Boolean RangoTipoPlacaActualizar { get; set; }
        public Boolean RangoTipoPlacaEliminar { get; set; }
    }
}

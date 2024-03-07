using System;

namespace ICVNL_SistemaLogistica.Web.Entities.RolesPermisos
{
    public class Pantallas_TransferenciasPlacasEntreDelegacionesBancos
    {
        public Boolean Acceso { get; set; }
        public Boolean Registrar { get; set; }
        public Boolean Actualizar { get; set; }
        public Boolean Cancelar { get; set; }
        public Boolean GenerarPackList { get; set; }
        public Boolean TransferirPlacas { get; set; }
        public Boolean TransferirPlacasAcceso { get; set; }
        public Boolean TransferirPlacasConfirmarTransferencia { get; set; }
        public Boolean TransferirPlacasGeneraPDF { get; set; }
    }
}

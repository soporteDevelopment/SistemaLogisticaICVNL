using System;

namespace ICVNL_SistemaLogistica.Web.Entities.RolesPermisos
{
    public class Pantallas_RecepcionPlacasEntreDelegacionesBancos
    {
        public Boolean Acceso { get; set; }
        public Boolean RecibirTransferencia { get; set; }
        public Boolean RechazarTransferencia { get; set; }
        public Boolean CerrarTransferencia { get; set; }
        public Boolean RecibirPlacasIndividuales { get; set; }

        public Boolean RecibirPlacasIndividualesAcceso { get; set; }
        public Boolean RecibirPlacasIndividualesPlacasDisponibles { get; set; }
        public Boolean RecibirPlacasIndividualesPlacasObsoleta { get; set; }
        public Boolean RecibirPlacasIndividualesColocarPlacaRechazada { get; set; }
        public Boolean RecibirPlacasIndividualesPlacasPerdida { get; set; }
        public Boolean RecibirPlacasIndividualesRegistrar { get; set; }
    }
}

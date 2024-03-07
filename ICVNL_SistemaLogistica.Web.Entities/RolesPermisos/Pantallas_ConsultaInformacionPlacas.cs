using System;

namespace ICVNL_SistemaLogistica.Web.Entities.RolesPermisos
{
    public class Pantallas_ConsultaInformacionPlacas
    {
        public Boolean Acceso { get; set; }
        public Boolean ConsultaInformacionPlaca { get; set; }
        public Boolean ConsultaInformacionDetalladaPlaca { get; set; }
        public Boolean ColocarPlacasDisponibles { get; set; }
        public Boolean ColocarPlacasObsoleta { get; set; }
        public Boolean ColocarPlacaRechazada { get; set; }
        public Boolean ColocarPlacasPerdida { get; set; } 
    }
}

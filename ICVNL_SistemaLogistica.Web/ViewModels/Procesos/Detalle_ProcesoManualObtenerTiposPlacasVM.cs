using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ProcesoManualObtenerTiposPlacasVM
    {
        [Display(Name = "Nota de Entrada")]
        public string FolioNotaEntrada { get; set; }
    }
}
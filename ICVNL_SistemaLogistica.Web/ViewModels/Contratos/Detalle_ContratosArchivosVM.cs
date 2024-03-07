using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ContratosArchivosVM
    {
        public int IdContrato { get; set; }
        public int Consecutivo { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Proveedor")]
        public string URL_Archivo { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Proveedor")]
        public string NombreArchivo { get; set; }

        public static Detalle_ContratosArchivosVM operator +(Detalle_ContratosArchivosVM detalle_ContratosArchivosVM, Contratos_Archivos contratos_Archivos)
        {
            detalle_ContratosArchivosVM.IdContrato = contratos_Archivos.IdContrato;
            detalle_ContratosArchivosVM.Consecutivo = contratos_Archivos.Consecutivo;
            detalle_ContratosArchivosVM.URL_Archivo = "";
            detalle_ContratosArchivosVM.NombreArchivo += contratos_Archivos.NombreArchivo;
            return detalle_ContratosArchivosVM;
        }
    }
}
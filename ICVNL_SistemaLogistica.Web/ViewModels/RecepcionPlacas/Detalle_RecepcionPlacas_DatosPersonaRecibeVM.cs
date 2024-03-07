using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_RecepcionPlacas_DatosPersonaRecibeVM
    {
        public int IdTransferenciaDatosPersona { get; set; }
        public int IdTransferencia { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Tipo de ID")]
        public int IdTipoIDs { get; set; }
        public Detalle_TiposIDSVM TiposID { get; set; } = new Detalle_TiposIDSVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Número de ID")]
        public string NumeroID { get; set; }

        public static Detalle_RecepcionPlacas_DatosPersonaRecibeVM operator +(Detalle_RecepcionPlacas_DatosPersonaRecibeVM placas_DatosPersonaEnvioVM, TransferenciaPlacas_DatosPersona _DatosPersonaEnvio)
        {
            placas_DatosPersonaEnvioVM.IdTransferenciaDatosPersona = _DatosPersonaEnvio.IdTransferenciaDatosPersona;
            placas_DatosPersonaEnvioVM.IdTransferencia = _DatosPersonaEnvio.IdTransferencia;
            placas_DatosPersonaEnvioVM.Nombre = _DatosPersonaEnvio.Nombre;
            placas_DatosPersonaEnvioVM.Apellido = _DatosPersonaEnvio.Apellido;
            placas_DatosPersonaEnvioVM.IdTipoIDs = _DatosPersonaEnvio.IdTipoIDs;
            placas_DatosPersonaEnvioVM.TiposID += _DatosPersonaEnvio.TiposID;
            placas_DatosPersonaEnvioVM.NumeroID = _DatosPersonaEnvio.NumeroID;
            return placas_DatosPersonaEnvioVM;
        }
    }
}
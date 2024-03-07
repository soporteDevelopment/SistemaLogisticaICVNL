using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_TiposIDSVM
    {
        [Key]
        [Display(Name = "Id del Tipo de ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "ID")]
        public string TipoID { get; set; }

        public static Detalle_TiposIDSVM operator +(Detalle_TiposIDSVM detalle_TiposIDsVM, TiposIDs tiposIDs)
        {
            detalle_TiposIDsVM.Id = tiposIDs.Id;
            detalle_TiposIDsVM.TipoID = tiposIDs.TipoID;

            return detalle_TiposIDsVM;
        }
    }
}
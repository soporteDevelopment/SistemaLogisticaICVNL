using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_EstadosVM
    {
        [Key]
        [Display(Name = "Id del Estado")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
        public int Estatus { get; set; }

        public static Detalle_EstadosVM operator +(Detalle_EstadosVM detalle_EstadosVM, Estados estados)
        {
            detalle_EstadosVM.Id = estados.Id;
            detalle_EstadosVM.Estado = estados.Estado;
            detalle_EstadosVM.Estatus = estados.Estatus;

            return detalle_EstadosVM;
        }
    }
}
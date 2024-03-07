using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_DelegacionesBancosVM
    {
        [Key]
        [Display(Name = "Id de la Delegación/Banco")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Nombre de la Delegación/Banco")]
        public string NombreDelegacionBanco { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Centro de Costos en Infofin")]
        public string CentroCostosInfoFin { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Almacén del Centro de Costos")]
        public string AlmacenCentroCostos { get; set; }
        public int Entidad { get; set; }

        public static Detalle_DelegacionesBancosVM operator +(Detalle_DelegacionesBancosVM delegacionesBancosVM, DelegacionesBancos delegacionesBancos)
        {
            delegacionesBancosVM.Id = delegacionesBancos.IdDelegacionBanco;
            delegacionesBancosVM.NombreDelegacionBanco = delegacionesBancos.NombreDelegacionBanco;
            delegacionesBancosVM.CentroCostosInfoFin = delegacionesBancos.CentroCostosInfoFin;
            delegacionesBancosVM.AlmacenCentroCostos = delegacionesBancos.AlmacenCentroCostos;
            delegacionesBancosVM.Entidad = delegacionesBancos.Entidad;

            return delegacionesBancosVM;
        }
    }
}
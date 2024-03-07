using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ProveedoresVM
    {
        [Key]
        [Display(Name = "Id de la Delegación/Banco")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Número del Proveedor")]
        public string NumeroProveedor { get; set; }

        [Display(Name = "Nombre del Proveedor")]
        public string NombreProveedor { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Email del Proveedor")]
        public string EmailProveedor { get; set; }


        public static Detalle_ProveedoresVM operator +(Detalle_ProveedoresVM detalle_ProveedoresVM, Proveedores proveedor)
        {
            detalle_ProveedoresVM = new Detalle_ProveedoresVM();
            detalle_ProveedoresVM.Id = proveedor.Id;
            detalle_ProveedoresVM.NumeroProveedor = proveedor.NumeroProveedor;
            detalle_ProveedoresVM.NombreProveedor = proveedor.NombreProveedor;
            detalle_ProveedoresVM.EmailProveedor = proveedor.EmailProveedor;

            return detalle_ProveedoresVM;
        }
    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ProveedoresModel
    {
        public int Id { get; set; }

        [Display(Name = "Número del Proveedor")]
        public string NumeroProveedor { get; set; }

        [Display(Name = "Nombre del Proveedor")]
        public string NombreProveedor { get; set; }

        [Display(Name = "Email del Proveedor")]
        public string EmailProveedor { get; set; }
        public int Estatus { get; set; }

        public static Listado_ProveedoresModel operator +(Listado_ProveedoresModel detalle_ProveedoresVM, Proveedores proveedor)
        {
            detalle_ProveedoresVM.Id = proveedor.Id;
            detalle_ProveedoresVM.NumeroProveedor = proveedor.NumeroProveedor;
            detalle_ProveedoresVM.NombreProveedor = proveedor.NombreProveedor;
            detalle_ProveedoresVM.EmailProveedor = proveedor.EmailProveedor;
            detalle_ProveedoresVM.Estatus = proveedor.Estatus;

            return detalle_ProveedoresVM;
        }
    }
}
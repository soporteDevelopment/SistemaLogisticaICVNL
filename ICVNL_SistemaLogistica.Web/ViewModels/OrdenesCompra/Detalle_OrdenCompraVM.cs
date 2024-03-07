using ICVNL_SistemaLogistica.Web.Entities;
using System;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_OrdenCompraVM
    {
        public int IdOrdenCompra { get; set; }
        public int? IdDelegacionRecibe { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public DateTime FechaOrdenCompraAutorizada { get; set; }
        public string EntidadInfofin { get; set; }
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedor { get; set; }
        public Detalle_OrdenCompra_DetailsVM Detalle_OrdenCompra { get; set; } = new Detalle_OrdenCompra_DetailsVM();

        public static Detalle_OrdenCompraVM operator +(Detalle_OrdenCompraVM ordenCompraVM, OrdenesCompra ordenesCompra)
        {
            ordenCompraVM.IdOrdenCompra = ordenesCompra.IdOrdenCompra;
            ordenCompraVM.IdDelegacionRecibe = ordenesCompra.IdDelegacionRecibe;
            ordenCompraVM.DelegacionesBancos += ordenesCompra.DelegacionesBancos;
            ordenCompraVM.NumeroOrdenCompra = ordenesCompra.NumeroOrdenCompra;
            ordenCompraVM.FechaOrdenCompra = ordenesCompra.FechaOrdenCompra;
            ordenCompraVM.FechaOrdenCompraAutorizada = ordenesCompra.FechaOrdenCompraAutorizada;
            ordenCompraVM.EntidadInfofin = ordenesCompra.EntidadInfofin;
            ordenCompraVM.IdProveedor = ordenesCompra.IdProveedor;
            ordenCompraVM.Proveedor += ordenesCompra.Proveedor;
            return ordenCompraVM;
        }

        public static Detalle_OrdenCompraVM operator /(Detalle_OrdenCompraVM ordenCompraVM, OrdenesCompra_Detalle compra_Detalle)
        {
            ordenCompraVM.Detalle_OrdenCompra += compra_Detalle;
            return ordenCompraVM;
        }
    }
}
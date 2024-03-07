using ICVNL_SistemaLogistica.Web.Entities;
using System;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_OrdenCompra_DetailsVM
    {
        public int IdOrdenCompraDetalle { get; set; }
        public int IdOrdenCompra { get; set; }
        public string CodigoArticulo_TipoPlaca { get; set; }
        public string CuentaContable { get; set; }
        public string CentroCostosAlmacen { get; set; }
        public int CantidadPiezas { get; set; }
        public decimal CostoPlaca { get; set; }
        public decimal CostoTotal { get; set; }
        public int RenglonOrdenCompra { get; set; }
        public int? RenglonNotaEntrada { get; set; }
        public DateTime? FechaRecepcion_TipoPlaca { get; set; }

        public static Detalle_OrdenCompra_DetailsVM operator +(Detalle_OrdenCompra_DetailsVM _DetailsVM, OrdenesCompra_Detalle compra_Detalle)
        {
            _DetailsVM.IdOrdenCompraDetalle = compra_Detalle.IdOrdenCompraDetalle;
            _DetailsVM.IdOrdenCompra = compra_Detalle.IdOrdenCompra;
            _DetailsVM.CodigoArticulo_TipoPlaca = compra_Detalle.CodigoArticulo_TipoPlaca;
            _DetailsVM.CuentaContable = compra_Detalle.CuentaContable;
            _DetailsVM.CentroCostosAlmacen = compra_Detalle.CentroCostosAlmacen;
            _DetailsVM.CantidadPiezas = compra_Detalle.CantidadPiezas;
            _DetailsVM.CostoPlaca = compra_Detalle.CostoPlaca;
            _DetailsVM.CostoTotal = compra_Detalle.CostoTotal;
            _DetailsVM.RenglonOrdenCompra = compra_Detalle.RenglonOrdenCompra;
            _DetailsVM.RenglonNotaEntrada = compra_Detalle.RenglonNotaEntrada;
            _DetailsVM.FechaRecepcion_TipoPlaca = compra_Detalle.FechaRecepcion_TipoPlaca;
            return _DetailsVM;
        }
    }
}
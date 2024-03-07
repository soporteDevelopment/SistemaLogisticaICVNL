using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class OrdenesCompra_Detalle
    {
        public int IdOrdenCompraDetalle { get; set; }
        public int IdOrdenCompra { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TipoPlaca { get; set; }
        public string CodigoArticulo_TipoPlaca { get; set; }
        public string CuentaContable { get; set; }
        public string CentroCostosAlmacen { get; set; }
        public int CantidadPiezas { get; set; }
        public decimal CostoPlaca { get; set; }
        public decimal CostoTotal { get; set; }
        public int RenglonOrdenCompra { get; set; }
        public int? RenglonNotaEntrada { get; set; }
        public DateTime? FechaRecepcion_TipoPlaca { get; set; }

        public static OrdenesCompra_Detalle operator +(OrdenesCompra_Detalle _Detalle, Services.Respuesta.RenglonesOC renglonesOC)
        {
            _Detalle.IdOrdenCompra = 0;
            _Detalle.IdOrdenCompraDetalle = 0; 
            _Detalle.CodigoArticulo_TipoPlaca = renglonesOC.articulo;
            _Detalle.CuentaContable = renglonesOC.cuenta_contable;
            _Detalle.CentroCostosAlmacen = "";
            _Detalle.CantidadPiezas = (int)renglonesOC.cantidad;
            _Detalle.CostoPlaca = renglonesOC.precio;
            _Detalle.CostoTotal = (renglonesOC.precio * (int)renglonesOC.cantidad);
            _Detalle.RenglonOrdenCompra = 0;
            _Detalle.RenglonNotaEntrada = 0;
            _Detalle.FechaRecepcion_TipoPlaca = null; 

            return _Detalle;
        }
    }
}

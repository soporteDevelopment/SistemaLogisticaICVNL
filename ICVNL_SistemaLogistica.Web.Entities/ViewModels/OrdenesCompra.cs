using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class OrdenesCompra
    {
        public int IdOrdenCompra { get; set; }
        public int? IdDelegacionRecibe { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public DateTime FechaOrdenCompraAutorizada { get; set; }
        public string EntidadInfofin { get; set; } 
        public int IdProveedor { get; set; }
        public Proveedores Proveedor { get; set; }
        public List<OrdenesCompra_Detalle> OrdenesCompra_Detalle { get; set; } = new List<OrdenesCompra_Detalle>();

        public static OrdenesCompra operator +(OrdenesCompra ordenesCompra, Services.Respuesta.OrdenesCompra ordenCompraAPI)
        {
            ordenesCompra.IdOrdenCompra = 0;
            ordenesCompra.NumeroOrdenCompra = ordenCompraAPI.orden.ToString();
            ordenesCompra.FechaOrdenCompra = ordenCompraAPI.fecha;
            ordenesCompra.FechaOrdenCompraAutorizada = ordenCompraAPI.fecha; 
            ordenesCompra.EntidadInfofin = ordenCompraAPI.empresa.ToString();
            foreach (var item in ordenCompraAPI.renglones)
            {
                ordenesCompra.OrdenesCompra_Detalle.Add(new Entities.OrdenesCompra_Detalle() + item);
            }
            return ordenesCompra;
        }
    }
}

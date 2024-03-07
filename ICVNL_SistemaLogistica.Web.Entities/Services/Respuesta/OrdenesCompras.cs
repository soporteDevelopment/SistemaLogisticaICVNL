using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    public class OrdenesCompra
    {
        public double empresa { get; set; }
        public string proveedor { get; set; }
        public double orden { get; set; }
        public DateTime fecha { get; set; }
        public object almacen { get; set; }
        public List<RenglonesOC> renglones { get; set; }
    }

    public class RenglonesOC
    {
        public string articulo { get; set; }
        public double cantidad { get; set; }
        public string cuenta_contable { get; set; }
        public decimal precio { get; set; }
        public decimal importe { get; set; }
    }

    public class ResponseOrdenesCompra
    {
        public List<OrdenesCompra> OrdenesCompra { get; set; }
        public int Acknowledge { get; set; }
        public object CorrelationId { get; set; }
        public object MessageId { get; set; }
        public object MessageParameters { get; set; }
        public string MessageText { get; set; }
        public double IdEvento { get; set; }
        public int TotalRows { get; set; }
        public int Language { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int Idalmacen { get; set; }
        public object Idsucursal { get; set; }
        public object IdProveedor { get; set; }
    }
}

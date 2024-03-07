using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    public class NotasEntradas
    {
        public int almacen { get; set; }
        public int orden { get; set; }
        public DateTime fecha_orden { get; set; }
        public int entrada { get; set; }
        public DateTime fecha_entrada { get; set; }
        public int empresa { get; set; }
        public string proveedor { get; set; }
        public List<Renglones> renglones { get; set; }
    }

    public class Renglones
    {
        public string articulo { get; set; }
        public string cuenta_contable { get; set; }
        public string centro_costos { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal total { get; set; }
        public int roc_renglon { get; set; }
        public int ren_renglon { get; set; }
        public DateTime fecha_recepcion { get; set; }
    }

    public class ResponseNotasEntradas
    {
        public List<NotasEntradas> NotasEntradas { get; set; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Articulo
    {
        public string empresa { get; set; }
        public string articulo { get; set; }
        public string descripcion { get; set; }
        public object articulo_corp { get; set; }
        public string familia { get; set; }
        public string codigo_barras { get; set; }
        public object equivalente { get; set; }
        public object sustituto { get; set; }
        public string descripcion_larga { get; set; }
        public double precio_venta { get; set; }
        public double estatus { get; set; }
        public double clave_existencia { get; set; }
        public string unidad_medida_compra { get; set; }
        public string unidad_medida_uso { get; set; }
        public double factor_uso_compra { get; set; }
        public string impuesto1 { get; set; }
        public string impuesto2 { get; set; }
        public object impuesto3 { get; set; }
        public object retencion_venta { get; set; }
    }

    public class ResponseArticulo
    {
        public Articulo Articulo { get; set; }
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

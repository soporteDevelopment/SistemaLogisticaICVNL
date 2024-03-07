using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    public class Almacenes
    {
        public double empresa { get; set; }
        public double almacen { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public double responsable { get; set; }
        public object colonia { get; set; }
        public object ciudad { get; set; }
        public object estado { get; set; }
        public object pais { get; set; }
        public object telefono { get; set; }
        public object fax { get; set; }
        public object url { get; set; }
        public string centro_costo { get; set; }
        public object email { get; set; }
        public double entransito_sn { get; set; }
        public string zona { get; set; }
        public object almacen_separado { get; set; }
        public int es_cedis { get; set; }
        public int existencia_por_localizacion_sn { get; set; }
        public int tipo_reposicion { get; set; }
        public string colonia_sat { get; set; }
        public string localidad_sat { get; set; }
        public string municipio_sat { get; set; }
        public string codigo_postal { get; set; }
        public string clave_pais { get; set; }
        public string clave_estado { get; set; }
    }
    

    public class ResponseListadoAlmacenes
    {
        public List<Almacenes> Almacenes { get; set; }
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

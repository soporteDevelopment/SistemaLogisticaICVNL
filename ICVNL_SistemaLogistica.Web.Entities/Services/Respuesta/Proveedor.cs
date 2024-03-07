using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Proveedor
    {
        public double empresa { get; set; }
        public object proveedor_pad { get; set; }
        public double usuario { get; set; }
        public string proveedor { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public object telefono { get; set; }
        public string rfc { get; set; }
        public double global { get; set; }
        public string cveref { get; set; }
        public string ctaanticipo { get; set; }
        public string bantransf { get; set; }
        public double numsuctransf { get; set; }
        public string tipoprov { get; set; }
        public double esfilial { get; set; }
        public double beneficiario { get; set; }
        public object fax { get; set; }
        public string email { get; set; }
        public object url { get; set; }
        public double modificable { get; set; }
        public double nopagosianticipo { get; set; }
        public double enviodefault { get; set; }
        public object contactodefault { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }
        public double cp { get; set; }
        public double ivasn { get; set; }
        public object curp { get; set; }
        public string plaza { get; set; }
        public string giro { get; set; }
        public double estatus { get; set; }
        public double pagacomision { get; set; }
        public string clavedpp { get; set; }
        public double comprassn { get; set; }
        public string condpagodef { get; set; }
        public double esciadefacaje { get; set; }
        public double usafacotraje { get; set; }
        public double porcppagafactaje { get; set; }
        public double porcomision { get; set; }
        public string tipodetercero { get; set; }
        public double extranjerosn { get; set; }
        public object ciber { get; set; }
        public object nombreabreviado { get; set; }
        public object ctaanticipomonalt { get; set; }
        public double formatoemailpagos { get; set; }
        public double porcdifcant { get; set; }
        public double tiposurtido { get; set; }
        public int retencion_resico_sn { get; set; }
        public object clave_retencion_resico { get; set; }
        public double almacen { get; set; }
        public object ipAddress { get; set; }
        public int idIdioma { get; set; }
        public object idUsuario { get; set; }
    }

    public class ResponseProveedor
    {
        public Proveedor proveedor { get; set; }
        public int acknowledge { get; set; }
        public object correlationId { get; set; }
        public object messageId { get; set; }
        public object messageParameters { get; set; }
        public string messageText { get; set; }
        public double idEvento { get; set; }
        public int totalRows { get; set; }
        public int language { get; set; }
        public int userId { get; set; }
        public int companyId { get; set; }
        public int idalmacen { get; set; }
        public object idsucursal { get; set; }
        public object idProveedor { get; set; }
    }


}

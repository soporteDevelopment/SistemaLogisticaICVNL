using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    public class Asiento
    {
        public string cuenta { get; set; }
        public string centro_costos { get; set; }
        public object clave_referencia { get; set; }
        public object numero_referencia { get; set; }
        public string descripcion { get; set; }
        public double tipo { get; set; }
        public double monto { get; set; }
        public double tipo_cambio { get; set; }
        public object ipAddress { get; set; }
        public int idIdioma { get; set; }
        public object idUsuario { get; set; }
    }

    public class Poliza
    {
        public double empresa { get; set; }
        public object indice { get; set; }
        public double tipo_poliza { get; set; }
        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public double usuario_poliza { get; set; }
        public string origen { get; set; }
        public List<Asiento> asientos { get; set; }
        public object ipAddress { get; set; }
        public int idIdioma { get; set; }
        public object idUsuario { get; set; }
    }

    public class ResponsePolizas
    {
        public Poliza poliza { get; set; }
        public int Acknowledge { get; set; }
        public object CorrelationId { get; set; }
        public string MessageId { get; set; }
        public string MessageParameters { get; set; }
        public string MessageText { get; set; }
        public double IdEvento { get; set; }
        public int TotalRows { get; set; }
        public int Language { get; set; }
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public int Idalmacen { get; set; }
        public int Idsucursal { get; set; }
        public int IdProveedor { get; set; }
    }
}

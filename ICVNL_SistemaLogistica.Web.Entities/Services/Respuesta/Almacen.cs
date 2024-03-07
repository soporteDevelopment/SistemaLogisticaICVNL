using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    public class AlmacenesUsuario
    {
        public double empresa { get; set; }
        public double almacen { get; set; }
        public string nombre { get; set; }
        public object zona { get; set; }
    }

    public class ResponseAlmacen
    {
        public List<AlmacenesUsuario> AlmacenesUsuario { get; set; }
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

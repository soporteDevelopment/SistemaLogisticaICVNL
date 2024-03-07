using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    public class Acceso
    {
        public List<string> accesos { get; set; }
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

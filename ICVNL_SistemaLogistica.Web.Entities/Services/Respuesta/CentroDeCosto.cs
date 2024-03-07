using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CentroDeCosto
    {
        public double empresa { get; set; }
        public string centrocostos { get; set; }
        public string descripcion { get; set; }
        public string reporta_a_centrocosto { get; set; }
        public string responsable_centrocosto { get; set; }
        public double folio_poliza_activo_fijo { get; set; }
        public object cuenta_poliza_activo_fijo { get; set; }
        public object cencto_costos_poliza_activo_fijo { get; set; }
        public object ipAddress { get; set; }
        public int idIdioma { get; set; }
        public object idUsuario { get; set; }
    }

    public class ResponseCentroDeCosto
    {
        public CentroDeCosto centroDeCosto { get; set; }
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

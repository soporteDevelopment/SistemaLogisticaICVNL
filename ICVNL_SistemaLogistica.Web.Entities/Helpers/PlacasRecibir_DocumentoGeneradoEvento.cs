using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Helpers
{
    public class PlacasRecibir_DocumentoGeneradoEvento
    {
        public string Hora { get; set; }
        public string Fecha { get; set; }
        public string NumEmpleado { get; set; }
        public string DelegacionBanco { get; set; }
        public string FolioNotaEntrada { get; set; }
        public string NombreProveedor { get; set; }
        public string FolioContrato { get; set; }
        public string PartidaContrato { get; set; }
        public string TipoProblema { get; set; }
        public string NumeroCaja { get; set; }
        public string TipoPlaca { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Helpers
{
    public class ErroresGeneradosPlacas
    {
        public int ConsecutivoError { get; set; }
        public string Error { get; set; }
        public int EsEvento { get; set; }
        public int Prioridad { get; set; }
    }
}

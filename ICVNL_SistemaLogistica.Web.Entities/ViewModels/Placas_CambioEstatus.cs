using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.ViewModels
{
    public class Placas_CambioEstatus
    {
        public string NumeroPlaca { get; set; }
        public int Entidad { get; set; }
        public int Estatus { get; set; }
        public int EstatusAnterior { get; set; }
        public string DescEstatus { get; set; }
    }
}

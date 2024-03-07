using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TiposEstatusNotaEntrada
    {
        public int IdEstatusNotaentrada { get; set; }
        public string EstatusNotaEntrada { get; set; }

        public TiposEstatusNotaEntrada(int IdEstatusNotaentrada_, string EstatusNotaEntrada_)
        {
            this.IdEstatusNotaentrada = IdEstatusNotaentrada_;
            this.EstatusNotaEntrada = EstatusNotaEntrada_;
        }
        public TiposEstatusNotaEntrada()
        {

        }
    }
}

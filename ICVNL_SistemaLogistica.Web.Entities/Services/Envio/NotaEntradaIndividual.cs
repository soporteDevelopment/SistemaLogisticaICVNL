using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class NotaEntradaInd
    {
        public int empresa { get; set; } 
        public int entrada { get; set; }
    }
    public class NotaEntradaAlmacenInd
    {
        public int empresa { get; set; }
        public int entrada { get; set; }
        public int almacen { get; set; }
    }
    public class PostNotaEntradaInd
    {
        public NotaEntradaInd NotaEntrada { get; set; }
        public string AccessToken { get; set; }
    }
    public class PostNotaEntradaAlmacenInd
    {
        public NotaEntradaAlmacenInd NotaEntrada { get; set; }
        public string AccessToken { get; set; }
    }
}

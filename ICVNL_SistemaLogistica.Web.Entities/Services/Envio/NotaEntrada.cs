using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    public class NotaEntrada
    {
        public int empresa { get; set; }
        public DateTime fecha_entrada_inicial { get; set; }
        public DateTime fecha_entrada_final { get; set; }
    }

    public class PostNotaEntrada
    {
        public NotaEntrada NotaEntrada { get; set; }
        public string AccessToken { get; set; }
    }
}

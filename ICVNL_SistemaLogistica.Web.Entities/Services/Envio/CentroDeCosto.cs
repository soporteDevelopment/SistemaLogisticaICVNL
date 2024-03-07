using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Envio
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class CentroDeCosto
    {
        public int empresa { get; set; }
        public string centrocostos { get; set; }
    }

    public class PostCentroDeCosto
    {
        public CentroDeCosto CentroDeCosto { get; set; }
        public string AccessToken { get; set; }
    }


}

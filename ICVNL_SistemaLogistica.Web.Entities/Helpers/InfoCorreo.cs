using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class InfoCorreo
    {
        public string MailerName { get; set; }
        public string InformadorEmail { get; set; }
        public string InformadorPassword { get; set; }
        public string ServidorSMTP { get; set; }
        public int PuertoSMTP { get; set; }
        public bool UsarSSL { get; set; }
        public int SmtpTimeout { get; set; }

    }
}

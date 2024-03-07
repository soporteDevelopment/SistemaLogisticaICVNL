using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class EnvioEmail
    {
        public string EmailEnvia { get; set; }
        public string EmailConCopiaEnvia { get; set; }
        public string SubjectEmail { get; set; }
        public string BodyEmail { get; set; }
        public Boolean ReadRecipient { get; set; }
        public Boolean DeliveryRecipient { get; set; }
        public List<ArchivosAdjuntos> ArchivosAdjuntos { get; set; }
    }

    public class ArchivosAdjuntos
    {
        public string RutaArchivo { get; set; }
    }
}

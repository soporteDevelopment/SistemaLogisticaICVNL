using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TiposIDs
    {
        public int Id { get; set; }
        public string TipoID { get; set; }
        public int Entidad { get; set; }
        public int Estatus { get; set; }

        public TiposIDs(int Id_, string TipoID_)
        {
            this.Id = Id_;
            this.TipoID = TipoID_;
        }

        public TiposIDs()
        {
        }
    }
}

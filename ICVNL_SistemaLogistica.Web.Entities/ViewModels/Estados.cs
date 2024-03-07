using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Estados
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public int Entidad { get; set; }
        public int Estatus { get; set; }
        public Estados(int Id_, string Estado_)
        {
            this.Id = Id_;
            this.Estado = Estado_;
        }
        public Estados()
        {

        }
    }
}

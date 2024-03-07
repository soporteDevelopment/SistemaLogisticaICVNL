using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Proveedores
    {
        public int Id { get; set; }
        public string NumeroProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public string EmailProveedor { get; set; } 
        public int Entidad { get; set; }
        public int Estatus { get; set; }

        public Proveedores(int id_,
                                  string NumeroProveedor_,
                                  string NombreProveedor_,
                                  string EmailProveedor_)
        {
            this.Id = id_;
            this.NumeroProveedor = NumeroProveedor_;
            this.NombreProveedor = NombreProveedor_;
            this.EmailProveedor = EmailProveedor_;
        }

        public Proveedores()
        {

        }
    }
}

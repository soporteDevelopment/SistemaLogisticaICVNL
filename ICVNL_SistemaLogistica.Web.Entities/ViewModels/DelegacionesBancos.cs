using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class DelegacionesBancos
    {
        public int IdDelegacionBanco { get; set; }
        public string NombreDelegacionBanco { get; set; }
        public string CentroCostosInfoFin { get; set; }
        public string AlmacenCentroCostos { get; set; }
        public int Entidad { get; set; }
        public int Estatus { get; set; }

        public DelegacionesBancos(int idDelegacionBanco_,
                                  string NombreDelegacionBanco_,
                                  string CentroCostosInfoFin_,
                                  string AlmacenCentroCostos_)
        {
            this.IdDelegacionBanco = idDelegacionBanco_;
            this.NombreDelegacionBanco = NombreDelegacionBanco_;
            this.CentroCostosInfoFin = CentroCostosInfoFin_;
            this.AlmacenCentroCostos = AlmacenCentroCostos_;
        }

        public DelegacionesBancos()
        {
            
        }
    }
}

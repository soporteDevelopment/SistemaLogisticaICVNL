using ICVNL_SistemaLogistica.Web.Entities.ViewModels;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICVNL_SistemaLogistica.Web.DataAccess;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class Polizas_BL
    {
        public DBResponse<List<Placas_Polizas>> GetPoliza_PlacasVendidasNoContabilizadas(int Entidad)
        {
            return new Polizas_DA().GetPoliza_PlacasVendidasNoContabilizadas(Entidad);
        }
        public DBResponse<List<Placas_Polizas>> GetPoliza_PlacasNoContabilizadas(int Entidad)
        {
            return new Polizas_DA().GetPoliza_PlacasNoContabilizadas(Entidad);
        }
    }
}

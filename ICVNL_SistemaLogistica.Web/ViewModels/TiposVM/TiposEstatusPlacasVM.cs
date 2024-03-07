using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class TiposEstatusPlacasVM
    {
        public int IdTipoEstatusPlacas { get; set; }
        public string TipoEstatusPlacas { get; set; }

        public static TiposEstatusPlacasVM operator +(TiposEstatusPlacasVM tiposEstatus, TiposEstatusPlacas estatusPlacas)
        {
            tiposEstatus.IdTipoEstatusPlacas = estatusPlacas.IdTipoEstatusPlacas;
            tiposEstatus.TipoEstatusPlacas = estatusPlacas.TipoEstatusPlacas;
            return tiposEstatus;
        }
    }
}
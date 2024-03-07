using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class TiposEventosRecepcionPlacasVM
    {
        public int IdTiposEventosRecepcionPlacas { get; set; }
        public string TiposEventosRecepcionPlacas { get; set; }

        public static TiposEventosRecepcionPlacasVM operator +(TiposEventosRecepcionPlacasVM tiposProblemasPresentados, TiposEventosRecepcionPlacas tiposEventos)
        {
            tiposProblemasPresentados.IdTiposEventosRecepcionPlacas = tiposEventos.IdTiposEventosRecepcionPlacas;
            tiposProblemasPresentados.TiposEventosRecepcionPlacas = tiposEventos.TipoEventosRecepcionPlacas;
            return tiposProblemasPresentados;
        }
    }
}
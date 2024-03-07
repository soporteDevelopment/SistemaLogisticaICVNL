using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_InventarioPlacas_TotalesExistenciaModel
    {
        public int IdInventario { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int Existencia { get; set; }

        public static Listado_InventarioPlacas_TotalesExistenciaModel operator +(Listado_InventarioPlacas_TotalesExistenciaModel _TotalesExistenciaM, InventarioPlacas_TotalesExistencia totalesExistencia)
        {
            _TotalesExistenciaM.IdInventario = totalesExistencia.IdInventario;
            _TotalesExistenciaM.IdTipoPlaca = totalesExistencia.IdTipoPlaca;
            _TotalesExistenciaM.TiposPlacas += totalesExistencia.TiposPlacas;
            _TotalesExistenciaM.Existencia = totalesExistencia.Existencia;

            return _TotalesExistenciaM;
        }
    }
}
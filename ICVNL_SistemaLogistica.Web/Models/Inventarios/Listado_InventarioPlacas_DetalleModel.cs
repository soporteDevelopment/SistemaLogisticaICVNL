using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_InventarioPlacas_DetalleModel
    {
        public int IdInventarioDetalle { get; set; }
        public int IdInventario { get; set; }
        public string NumeroPlaca { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int Existencia { get; set; }
        public int IdEstatusPlacas { get; set; }
        public TiposEstatusPlacasVM EstatusPlacas { get; set; } = new TiposEstatusPlacasVM();
        public string Serie { get; set; }
        public string ExistenciaDesde { get; set; }
        public string ExistenciaHasta { get; set; }
        public string ExistenciaTotalRango { get; set; }

        public static Listado_InventarioPlacas_DetalleModel operator +(Listado_InventarioPlacas_DetalleModel _DetalleModel, InventarioPlacas_Detalle inventarioPlacasDet)
        {
            _DetalleModel.IdInventarioDetalle = inventarioPlacasDet.IdInventarioDetalle;
            _DetalleModel.IdInventario = inventarioPlacasDet.IdInventario;
            _DetalleModel.NumeroPlaca = inventarioPlacasDet.NumeroPlaca;
            _DetalleModel.IdTipoPlaca = inventarioPlacasDet.IdTipoPlaca;
            _DetalleModel.TiposPlacas += inventarioPlacasDet.TiposPlacas;
            _DetalleModel.Existencia = inventarioPlacasDet.Existencia;
            _DetalleModel.IdEstatusPlacas = inventarioPlacasDet.IdEstatusPlacas;
            _DetalleModel.EstatusPlacas += inventarioPlacasDet.EstatusPlacas;
            _DetalleModel.Serie = inventarioPlacasDet.Serie;
            _DetalleModel.ExistenciaDesde = inventarioPlacasDet.ExistenciaDesde;
            _DetalleModel.ExistenciaHasta = inventarioPlacasDet.ExistenciaHasta;
            _DetalleModel.ExistenciaTotalRango = inventarioPlacasDet.ExistenciaTotalRango;
            return _DetalleModel;
        }

    }
}
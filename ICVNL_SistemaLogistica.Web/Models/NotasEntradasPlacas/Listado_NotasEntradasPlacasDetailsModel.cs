using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_NotasEntradasPlacasDetailsModel
    {
        public int IdNotaEntradaDetalle { get; set; }
        public int IdNotaEntrada { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int CantidadPlacas { get; set; }

        public decimal CostoPlaca { get; set; }
        public decimal CostoTotal { get; set; }
        public int CantidadNumerosPlacaIdentificada { get; set; }
        public int CantidadNumerosPlacaPorIdentificarse { get; set; }
        public int IdEstatusNotaEntrada { get; set; }
        public TiposEstatusNotaEntradaVM TiposEstatus { get; set; } = new TiposEstatusNotaEntradaVM();
        public static Listado_NotasEntradasPlacasDetailsModel operator +(Listado_NotasEntradasPlacasDetailsModel placasDetailsVM, NotasEntradasPlacas_Detalle _Detalle)
        {
            placasDetailsVM.IdNotaEntradaDetalle = _Detalle.IdNotaEntradaDetalle;
            placasDetailsVM.IdNotaEntrada = _Detalle.IdNotaEntrada;
            placasDetailsVM.IdTipoPlaca = _Detalle.IdTipoPlaca;
            placasDetailsVM.TiposPlacas += _Detalle.TiposPlacas;
            placasDetailsVM.CantidadPlacas = _Detalle.CantidadPlacas;
            placasDetailsVM.CostoPlaca = _Detalle.CostoPlaca;
            placasDetailsVM.CostoTotal = _Detalle.CostoTotal;
            placasDetailsVM.CantidadNumerosPlacaIdentificada = _Detalle.CantidadNumerosPlacaIdentificada;
            placasDetailsVM.CantidadNumerosPlacaPorIdentificarse = _Detalle.CantidadNumerosPlacaPorIdentificarse;
            placasDetailsVM.IdEstatusNotaEntrada = _Detalle.IdEstatusNotaEntrada;
            placasDetailsVM.TiposEstatus += _Detalle.TiposEstatus;
            return placasDetailsVM;
        }
    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_NotasEntradasPlacasDetailsVM
    {
        [Key]
        [Display(Name = "Id")]
        public int IdNotaEntradaDetalle { get; set; }
        public int NumeroRenglon { get; set; }
        public int IdNotaEntrada { get; set; }

        [Display(Name = "Tipo de Placa")]
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();

        [Display(Name = "Cantidad de Placas")]
        public int CantidadPlacas { get; set; }

        [Display(Name = "Costo por Placa")]
        public decimal CostoPlaca { get; set; }

        [Display(Name = "Costo Total")]
        public decimal CostoTotal { get; set; }

        [Display(Name = "Cant. de Núm. de Placa Identificados Nota Entrada")]
        public int CantidadNumerosPlacaIdentificada { get; set; }

        [Display(Name = "Cant. de Núm. de Placa por Identificarse Nota Entrada")]
        public int CantidadNumerosPlacaPorIdentificarse { get; set; }

        [Display(Name = "Estatus 1 de la Nota de Entrada")]
        public int IdEstatusNotaEntrada { get; set; }
        public TiposEstatusNotaEntradaVM TiposEstatusNotaEntrada { get; set; } = new TiposEstatusNotaEntradaVM();
        public static Detalle_NotasEntradasPlacasDetailsVM operator +(Detalle_NotasEntradasPlacasDetailsVM placasDetailsVM, NotasEntradasPlacas_Detalle _Detalle)
        {
            placasDetailsVM.IdNotaEntradaDetalle = _Detalle.IdNotaEntradaDetalle;
            placasDetailsVM.NumeroRenglon = _Detalle.NumeroRenglon;
            placasDetailsVM.IdNotaEntrada = _Detalle.IdNotaEntrada;
            placasDetailsVM.IdTipoPlaca = _Detalle.IdTipoPlaca;
            placasDetailsVM.TiposPlacas += _Detalle.TiposPlacas;
            placasDetailsVM.CantidadPlacas = _Detalle.CantidadPlacas;
            placasDetailsVM.CostoPlaca = _Detalle.CostoPlaca;
            placasDetailsVM.CostoTotal = _Detalle.CostoTotal;
            placasDetailsVM.CantidadNumerosPlacaIdentificada = _Detalle.CantidadNumerosPlacaIdentificada;
            placasDetailsVM.CantidadNumerosPlacaPorIdentificarse = _Detalle.CantidadNumerosPlacaPorIdentificarse;
            placasDetailsVM.IdEstatusNotaEntrada = _Detalle.IdEstatusNotaEntrada;
            placasDetailsVM.TiposEstatusNotaEntrada += _Detalle.TiposEstatus;
            return placasDetailsVM;
        }

    }
}
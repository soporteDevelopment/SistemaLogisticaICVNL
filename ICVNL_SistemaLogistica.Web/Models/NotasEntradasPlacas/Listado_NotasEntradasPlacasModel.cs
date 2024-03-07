using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_NotasEntradasPlacasModel
    {
        public int IdNotaEntrada { get; set; }
        public int NumeroNotaEntrada { get; set; }
        public DateTime FechaNotaEntrada { get; set; }
        public int IdOrdenCompra { get; set; }
        public int NumeroOrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public int CantidadPlacasNotaEntrada { get; set; }

        public int CantidadNumerosPlacaIdentificada { get; set; }
        public int CantidadNumerosPlacaPorIdentificarse { get; set; }
        public int IdEstatusNotaEntrada { get; set; }
        public string ColorEstatusNotaEntrada { get; set; }
        public TiposEstatusNotaEntradaVM TiposEstatusNotaEntrada { get; set; } = new TiposEstatusNotaEntradaVM();

        public static Listado_NotasEntradasPlacasModel operator +(Listado_NotasEntradasPlacasModel _NotasEntradasPlacasVM, NotasEntradasPlacas notasEntradasPlacas)
        {
            _NotasEntradasPlacasVM.IdNotaEntrada = notasEntradasPlacas.IdNotaEntrada;
            _NotasEntradasPlacasVM.NumeroNotaEntrada = notasEntradasPlacas.NumeroNotaEntrada;
            _NotasEntradasPlacasVM.FechaNotaEntrada = notasEntradasPlacas.FechaNotaEntrada;
            _NotasEntradasPlacasVM.FechaOrdenCompra = notasEntradasPlacas.OrdenCompra.FechaOrdenCompra;
            _NotasEntradasPlacasVM.IdOrdenCompra = notasEntradasPlacas.IdOrdenCompra;
            _NotasEntradasPlacasVM.NumeroOrdenCompra = int.Parse(notasEntradasPlacas.OrdenCompra.NumeroOrdenCompra);
            _NotasEntradasPlacasVM.CantidadPlacasNotaEntrada = notasEntradasPlacas.CantidadPlacasNotaEntrada;
            _NotasEntradasPlacasVM.CantidadNumerosPlacaIdentificada = notasEntradasPlacas.CantidadNumerosPlacaIdentificada;
            _NotasEntradasPlacasVM.CantidadNumerosPlacaPorIdentificarse = notasEntradasPlacas.CantidadNumerosPlacaPorIdentificarse;
            _NotasEntradasPlacasVM.IdEstatusNotaEntrada = notasEntradasPlacas.IdEstatusNotaEntrada;
            _NotasEntradasPlacasVM.TiposEstatusNotaEntrada += notasEntradasPlacas.TiposEstatusNotaEntrada;
            _NotasEntradasPlacasVM.ColorEstatusNotaEntrada = notasEntradasPlacas.IdEstatusNotaEntrada == 2 ? "red" : "";
            return _NotasEntradasPlacasVM;
        }
    }
}
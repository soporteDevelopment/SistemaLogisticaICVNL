using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_NotasEntradasPlacasVM
    {
        [Key]
        [Display(Name = "Id Nota Entrada")]
        public int IdNotaEntrada { get; set; }

        [Display(Name = "Numero de la Nota de Entrada")]
        public int NumeroNotaEntrada { get; set; }

        [Display(Name = "Fecha de la Nota de Entrada")]
        public DateTime FechaNotaEntrada { get; set; }

        [Display(Name = "Numero de Orden de Compra")]
        public int IdOrdenCompra { get; set; }
        public int NumeroOrdenCompra { get; set; }

        [Display(Name = "Fecha de la Orden de Compra")]
        public DateTime FechaOrdenCompra { get; set; }

        [Display(Name = "Cant. de Placas de la Nota Entrada")]
        public int CantidadPlacasNotaEntrada { get; set; }

        [Display(Name = "Cant. de Núm. de Placa Identificados Nota Entrada")]
        public int CantidadNumerosPlacaIdentificada { get; set; }

        [Display(Name = "Cant. de Núm. de Placa por Identificarse Nota Entrada")]
        public int CantidadNumerosPlacaPorIdentificarse { get; set; }

        [Display(Name = "Estatus 1 de la Nota de Entrada")]
        public int IdEstatusNotaEntrada { get; set; }

        [Display(Name = "Estatus")]
        public string FiltroIdEstatus { get; set; }
        public IEnumerable<dynamic> ListadoEstatusNotasEntradasDDL { get; set; }

        public TiposEstatusNotaEntradaVM TiposEstatusNotaEntrada { get; set; } = new TiposEstatusNotaEntradaVM();
        public List<Listado_NotasEntradasPlacasDetailsModel> NotasEntradasPlacas_Detalle { get; set; } = new List<Listado_NotasEntradasPlacasDetailsModel>();
        public Detalle_NotasEntradasPlacasDetailsVM DetalleNotasEntradas { get; set; } = new Detalle_NotasEntradasPlacasDetailsVM();

        public static Detalle_NotasEntradasPlacasVM operator +(Detalle_NotasEntradasPlacasVM _NotasEntradasPlacasVM, NotasEntradasPlacas notasEntradasPlacas)
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
            foreach (var item in notasEntradasPlacas.NotasEntradasPlacas_Detalle)
            {
                _NotasEntradasPlacasVM.NotasEntradasPlacas_Detalle.Add(new Listado_NotasEntradasPlacasDetailsModel() + item);
            }

            return _NotasEntradasPlacasVM;
        }



    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ConsultaInformacionPlacasVM
    {
        public int IdInventario { get; set; }

        [Display(Name = "Número de Placa")]
        public string NumeroPlaca { get; set; }

        [Display(Name = "Tipo de Placa")]
        public string TipoPlaca { get; set; }

        [Display(Name = "Costo Placa")]
        public string CostoPlaca { get; set; }

        [Display(Name = "Fecha Ingreso Partida")]
        public DateTime FechaIngreso { get; set; }

        [Display(Name = "Número Nota de Entrada")]
        public string NumeroNotaEntrada { get; set; }

        [Display(Name = "Fecha Ingreso Nota de Entrada en el Sistema")]
        public DateTime FechaIngresaNotaEntradaSistema { get; set; }

        [Display(Name = "Fecha Nota de Entrada")]
        public DateTime FechaNotaEntrada { get; set; }

        [Display(Name = "Renglón Nota de Entrada")]
        public string RenglonNotaEntrada { get; set; }

        [Display(Name = "Número de Orden de Compra")]
        public string NumeroOrdenCompra { get; set; }

        [Display(Name = "Fecha Orden de Compra")]
        public DateTime FechaOrdenCompra { get; set; }

        [Display(Name = "Renglón Orden de Compra")]
        public string RenglonOrdenCompra { get; set; }

        [Display(Name = "Proveedor Orden de Compra")]
        public string ProveedorOrdenCompra { get; set; }

        [Display(Name = "Entidad")]
        public string Entidad { get; set; }

        [Display(Name = "Almacén recibió la Placa")]
        public string AlmacenRecibioPlaca { get; set; }

        [Display(Name = "Cuenta Contable recepción de las Placas")]
        public string CuentaContableRecepcionPlaca { get; set; }

        [Display(Name = "Centro de Costos Almacén Orden de Compra ")]
        public string CentroCostosAlmacenOrdenCompra { get; set; }

        [Display(Name = "Estatus Actual Placa")]
        public string EstatusActualPlaca { get; set; }

        [Display(Name = "Placa Contabilizada")]
        public string PlacaContabilizada { get; set; }

        [Display(Name = "Número de Póliza GRP Infofin")]

        public string NumeroPolizaGRP_Infofin { get; set; }
        public List<Listado_ConsultaInformacionPlacas_Detalle_TransferenciasPlacasModel> TransferenciasPlacas { get; set; } = new List<Listado_ConsultaInformacionPlacas_Detalle_TransferenciasPlacasModel>();
        public List<Listado_ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacasModel> CambiosEstatusPlacas { get; set; } = new List<Listado_ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacasModel>();

        public static Detalle_ConsultaInformacionPlacasVM operator +(Detalle_ConsultaInformacionPlacasVM consultaPlacasVM, ConsultaInformacionPlacas_Detalle informacionPlacas)
        {
            consultaPlacasVM.IdInventario = informacionPlacas.IdInventario;
            consultaPlacasVM.NumeroPlaca = informacionPlacas.NumeroPlaca;
            consultaPlacasVM.TipoPlaca = informacionPlacas.TipoPlaca;
            consultaPlacasVM.CostoPlaca = informacionPlacas.CostoPlaca;
            consultaPlacasVM.FechaIngreso = informacionPlacas.FechaIngreso;
            consultaPlacasVM.NumeroNotaEntrada = informacionPlacas.NumeroNotaEntrada;
            consultaPlacasVM.FechaNotaEntrada = informacionPlacas.FechaNotaEntrada;
            consultaPlacasVM.FechaIngresaNotaEntradaSistema = informacionPlacas.FechaIngresoNotaEntradaSistema;
            consultaPlacasVM.RenglonNotaEntrada = informacionPlacas.RenglonNotaEntrada;
            consultaPlacasVM.NumeroOrdenCompra = informacionPlacas.NumeroOrdenCompra;
            consultaPlacasVM.FechaOrdenCompra = informacionPlacas.FechaOrdenCompra;
            consultaPlacasVM.RenglonOrdenCompra = informacionPlacas.RenglonOrdenCompra;
            consultaPlacasVM.ProveedorOrdenCompra = informacionPlacas.ProveedorOrdenCompra;
            consultaPlacasVM.Entidad = informacionPlacas.Entidad;
            consultaPlacasVM.AlmacenRecibioPlaca = informacionPlacas.AlmacenRecibioPlaca;
            consultaPlacasVM.CuentaContableRecepcionPlaca = informacionPlacas.CuentaContableRecepcionPlaca;
            consultaPlacasVM.CentroCostosAlmacenOrdenCompra = informacionPlacas.CentroCostosAlmacenOrdenCompra;
            consultaPlacasVM.EstatusActualPlaca = informacionPlacas.EstatusActualPlaca;
            consultaPlacasVM.PlacaContabilizada = informacionPlacas.PlacaContabilizada;
            consultaPlacasVM.NumeroPolizaGRP_Infofin = informacionPlacas.NumeroPolizaGRP_Infofin;
            foreach (var item in informacionPlacas.TransferenciasPlacas)
            {
                consultaPlacasVM.TransferenciasPlacas.Add(new Listado_ConsultaInformacionPlacas_Detalle_TransferenciasPlacasModel() + item);
            }
            foreach (var item in informacionPlacas.CambiosEstatusPlacas)
            {
                consultaPlacasVM.CambiosEstatusPlacas.Add(new Listado_ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacasModel() + item);
            }
            return consultaPlacasVM;
        }

    }
}
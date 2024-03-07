using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_InventarioPlacas_ExistenciaModel
    {
        public int IdInventarioExistencia { get; set; }
        public int IdInventario { get; set; }

        [Display(Name = "Número de Placa")]
        public string NumeroPlaca { get; set; }
        public int IdEstatusPlaca { get; set; }
        public TiposEstatusPlacasVM TiposEstatusPlacas { get; set; } = new TiposEstatusPlacasVM();

        [Display(Name = "Tipo de Placa")]
        public string TipoPlaca { get; set; }
        public int Existencia { get; set; }
        public List<Listado_InventarioPlacas_Existencia_HistorialCambiosModel> HistorialCambios { get; set; } = new List<Listado_InventarioPlacas_Existencia_HistorialCambiosModel>();
        public Boolean PlacaContabilizada { get; set; }
        public string NumeroPolizaGRP_Infofin { get; set; }

        [Display(Name = "Costo de la Placa")]
        public decimal CostoPlaca { get; set; }
        public static Listado_InventarioPlacas_ExistenciaModel operator +(Listado_InventarioPlacas_ExistenciaModel _ExistenciaVM, InventarioPlacas_Existencia _Existencia)
        {
            _ExistenciaVM.IdInventarioExistencia = _Existencia.IdInventarioExistencia;
            _ExistenciaVM.IdInventario = _Existencia.IdInventario;
            _ExistenciaVM.NumeroPlaca = _Existencia.NumeroPlaca;
            _ExistenciaVM.IdEstatusPlaca = _Existencia.IdEstatusPlaca;
            _ExistenciaVM.TiposEstatusPlacas += _Existencia.TiposEstatusPlacas;
            _ExistenciaVM.TipoPlaca = _Existencia.TiposPlacas.TipoPlaca;
            foreach (var item in _Existencia.HistorialCambios)
            {
                _ExistenciaVM.HistorialCambios.Add(new Listado_InventarioPlacas_Existencia_HistorialCambiosModel() + item);
            }
            _ExistenciaVM.PlacaContabilizada = _Existencia.PlacaContabilizada;
            _ExistenciaVM.NumeroPolizaGRP_Infofin = _Existencia.NumeroPolizaGRP_Infofin;
            _ExistenciaVM.CostoPlaca = _Existencia.CostoPlaca;
            return _ExistenciaVM;
        }
    }
}
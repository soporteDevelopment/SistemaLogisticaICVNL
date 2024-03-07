using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_InventarioPlacas_Existencia_HistorialCambiosModel
    {
        public int IdInventarioExistenciaHistorial { get; set; }
        public int IdInventarioExistencia { get; set; }
        public DateTime FechaOperacion { get; set; }
        public int IdEstatusAnterior { get; set; }
        public TiposEstatusPlacasVM EstatusPlacasAnterior { get; set; } = new TiposEstatusPlacasVM();
        public int IdEstatusNuevo { get; set; }
        public TiposEstatusPlacasVM EstatusPlacasNuevo { get; set; } = new TiposEstatusPlacasVM();
        public string Operacion { get; set; }

        public static Listado_InventarioPlacas_Existencia_HistorialCambiosModel operator +(Listado_InventarioPlacas_Existencia_HistorialCambiosModel _HistorialCambiosModel, InventarioPlacas_Existencia_HistorialCambios historialCambios)
        {
            _HistorialCambiosModel.IdInventarioExistenciaHistorial = historialCambios.IdInventarioExistenciaHistorial;
            _HistorialCambiosModel.IdInventarioExistencia = historialCambios.IdInventarioExistencia;
            _HistorialCambiosModel.FechaOperacion = historialCambios.FechaOperacion;
            _HistorialCambiosModel.IdEstatusAnterior = historialCambios.IdEstatusAnterior;
            _HistorialCambiosModel.EstatusPlacasAnterior += historialCambios.EstatusPlacasAnterior;
            _HistorialCambiosModel.IdEstatusNuevo = historialCambios.IdEstatusNuevo;
            _HistorialCambiosModel.EstatusPlacasNuevo += historialCambios.EstatusPlacasNuevo;
            _HistorialCambiosModel.Operacion = historialCambios.Operacion;

            return _HistorialCambiosModel;
        }
    }
}
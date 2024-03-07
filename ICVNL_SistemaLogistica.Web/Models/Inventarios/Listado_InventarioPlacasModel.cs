using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_InventarioPlacasModel
    {
        public int IdInventario { get; set; }
        public int IdDelegacionBanco { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();
        public DateTime FechaInventario { get; set; }
        public List<Listado_InventarioPlacas_TotalesExistenciaModel> InventarioPlacas_TotalesExistencia { get; set; } = new List<Listado_InventarioPlacas_TotalesExistenciaModel>();
        public List<Listado_InventarioPlacas_ExistenciaModel> InventarioPlacas_Existencia { get; set; } = new List<Listado_InventarioPlacas_ExistenciaModel>();
        public List<Listado_InventarioPlacas_DetalleModel> InventarioPlacas_Detalle { get; set; } = new List<Listado_InventarioPlacas_DetalleModel>();

        public static Listado_InventarioPlacasModel operator +(Listado_InventarioPlacasModel detalle_, InventarioPlacas inventarioPlacas)
        {
            detalle_.IdInventario = inventarioPlacas.IdInventario;
            detalle_.IdDelegacionBanco = inventarioPlacas.IdDelegacionBanco;
            detalle_.DelegacionesBancos += inventarioPlacas.DelegacionesBancos;
            detalle_.FechaInventario = inventarioPlacas.FechaInventario;
            foreach (var item in inventarioPlacas.InventarioPlacas_TotalesExistencia)
            {
                detalle_.InventarioPlacas_TotalesExistencia.Add(new Listado_InventarioPlacas_TotalesExistenciaModel() + item);

            }
            foreach (var item in inventarioPlacas.InventarioPlacas_Existencia)
            {
                detalle_.InventarioPlacas_Existencia.Add(new Listado_InventarioPlacas_ExistenciaModel() + item);

            }
            foreach (var item in inventarioPlacas.InventarioPlacas_Detalle)
            {
                detalle_.InventarioPlacas_Detalle.Add(new Listado_InventarioPlacas_DetalleModel() + item);
            }
            return detalle_;
        }

    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel
    {
        public int IdEventoRecepcion { get; set; }
        public int IdValidacionEvento { get; set; }        
        public int IdRecepcion { get; set; }
        public int Consecutivo { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public Detalle_SolicitudesPlacasRecepcionVM RecepcionSolicitudesPlacas { get; set; } = new Detalle_SolicitudesPlacasRecepcionVM();
        public int IdDelegacionBanco { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();
        public string NumeroCaja { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public string Rangos { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public int CantidadLaminas { get; set; } 

        public static Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel operator +(Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel listado_SolicitudesPlacas, RecepcionSolicitudesPlacas_Recibir placas_Recibir)
        {
            listado_SolicitudesPlacas.IdEventoRecepcion = placas_Recibir.IdEventoRecepcion;
            listado_SolicitudesPlacas.IdValidacionEvento = placas_Recibir.IdValidacionEvento;
            listado_SolicitudesPlacas.FechaHoraRegistro = placas_Recibir.FechaHoraRegistro;
            listado_SolicitudesPlacas.IdRecepcion = placas_Recibir.IdRecepcion;
            listado_SolicitudesPlacas.RecepcionSolicitudesPlacas += placas_Recibir.RecepcionSolicitudesPlacas;
            listado_SolicitudesPlacas.IdProveedor = placas_Recibir.IdProveedor;
            listado_SolicitudesPlacas.Proveedores += placas_Recibir.Proveedores;
            listado_SolicitudesPlacas.NumeroCaja = placas_Recibir.NumeroCaja;
            listado_SolicitudesPlacas.IdTipoPlaca = placas_Recibir.IdTipoPlaca;
            listado_SolicitudesPlacas.TiposPlacas += placas_Recibir.TiposPlacas;
            listado_SolicitudesPlacas.Rangos = placas_Recibir.Rangos;
            listado_SolicitudesPlacas.IdDelegacionBanco = placas_Recibir.IdDelegacionBanco;
            listado_SolicitudesPlacas.DelegacionesBancos += placas_Recibir.DelegacionesBancos;
            listado_SolicitudesPlacas.CantidadLaminas = placas_Recibir.CantidadLaminas;
            return listado_SolicitudesPlacas;
        }
    }
}
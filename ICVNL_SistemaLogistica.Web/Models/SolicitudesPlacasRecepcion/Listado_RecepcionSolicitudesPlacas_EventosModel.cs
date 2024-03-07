using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_RecepcionSolicitudesPlacas_EventosModel
    {
        public int IdEventoRecepcion { get; set; }
        public int IdRecepcion { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public Detalle_SolicitudesPlacasRecepcionVM RecepcionSolicitudesPlacas { get; set; } = new Detalle_SolicitudesPlacasRecepcionVM();
        public string Delegacion { get; set; }
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();
        public string NumeroCaja { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public string Rangos { get; set; }
        public int CantidadLaminas { get; set; }
        public int IdTiposEventosRecepcionPlacas { get; set; }
        public string TipoEventosRecepcionPlacas { get; set; }
        public string URL_ArchivoOficio { get; set; }

        public static Listado_RecepcionSolicitudesPlacas_EventosModel operator +(Listado_RecepcionSolicitudesPlacas_EventosModel listado_SolicitudesPlacas, RecepcionSolicitudesPlacas_Eventos placas_Eventos)
        {
            listado_SolicitudesPlacas.IdEventoRecepcion = placas_Eventos.IdEventoRecepcion;
            listado_SolicitudesPlacas.FechaHoraRegistro = placas_Eventos.FechaHoraRegistro;
            listado_SolicitudesPlacas.IdRecepcion = placas_Eventos.IdRecepcion;
            if (placas_Eventos.RecepcionSolicitudesPlacas != null)
            {
                listado_SolicitudesPlacas.RecepcionSolicitudesPlacas += placas_Eventos.RecepcionSolicitudesPlacas;
            }
            listado_SolicitudesPlacas.IdProveedor = placas_Eventos.IdProveedor;
            listado_SolicitudesPlacas.Proveedores += placas_Eventos.Proveedores;
            listado_SolicitudesPlacas.NumeroCaja = placas_Eventos.NumeroCaja;
            listado_SolicitudesPlacas.IdTipoPlaca = placas_Eventos.IdTipoPlaca;
            listado_SolicitudesPlacas.TiposPlacas += placas_Eventos.TiposPlacas;
            listado_SolicitudesPlacas.Rangos = placas_Eventos.Rangos;
            listado_SolicitudesPlacas.Delegacion = placas_Eventos.DelegacionesBancos.NombreDelegacionBanco;
            listado_SolicitudesPlacas.CantidadLaminas = placas_Eventos.CantidadLaminas;
            listado_SolicitudesPlacas.IdTiposEventosRecepcionPlacas = placas_Eventos.IdTiposEventosRecepcionPlacas;
            listado_SolicitudesPlacas.TipoEventosRecepcionPlacas = placas_Eventos.TipoEventosRecepcionPlacas;
            listado_SolicitudesPlacas.URL_ArchivoOficio = placas_Eventos.URL_ArchivoOficio;
            return listado_SolicitudesPlacas;
        }
    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_RecepcionSolicitudesPlacas_EventosVM
    {
        public int IdArchivo { get; set; }
        public int IdEventoRecepcion { get; set; }

        [Display(Name = "Fecha Hora Lectura QR")]
        public DateTime FechaHoraRegistro { get; set; }

        [Display(Name = "Nombre del Proveedor")]
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();

        [Display(Name = "Número de la Caja")]
        public int NumeroCaja { get; set; }

        [Display(Name = "Tipo de Estructura (Tipo de Placa)")]
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();

        [Display(Name = "Rango de Placas")]
        public string Rangos { get; set; }

        [Display(Name = "Cantidad de Láminas")]
        public int CantidadLaminas { get; set; }

        [Display(Name = "Destino (Nombre de la Delegación)")]
        public string Delegacion { get; set; }

        [Display(Name = "Evento")]
        public int IdTiposEventosRecepcionPlacas { get; set; }
        public string TipoEventosRecepcionPlacas { get; set; }

        [Display(Name = "Link a Oficio/Acta Generado")]
        public string URL_ArchivoOficio { get; set; }
        public string URL_ArchivoPDF { get; set; }

        public List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel> ListadoArchivos { get; set; } = new List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel>();
        public List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel> ListadoObservaciones { get; set; } = new List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel>();

        public static Detalle_RecepcionSolicitudesPlacas_EventosVM operator +(Detalle_RecepcionSolicitudesPlacas_EventosVM validacionesModel, RecepcionSolicitudesPlacas_Eventos recepcionSolicitudesPlacas)
        {
            validacionesModel.IdArchivo = recepcionSolicitudesPlacas.IdArchivo;
            validacionesModel.IdEventoRecepcion = recepcionSolicitudesPlacas.IdEventoRecepcion;
            validacionesModel.FechaHoraRegistro = recepcionSolicitudesPlacas.FechaHoraRegistro;
            validacionesModel.Delegacion = recepcionSolicitudesPlacas.DelegacionesBancos.NombreDelegacionBanco;
            validacionesModel.IdProveedor = recepcionSolicitudesPlacas.IdProveedor;
            validacionesModel.Proveedores += recepcionSolicitudesPlacas.Proveedores;
            validacionesModel.IdTiposEventosRecepcionPlacas = recepcionSolicitudesPlacas.IdTiposEventosRecepcionPlacas;
            validacionesModel.TipoEventosRecepcionPlacas += recepcionSolicitudesPlacas.TipoEventosRecepcionPlacas;
            validacionesModel.IdTipoPlaca = recepcionSolicitudesPlacas.IdTipoPlaca;
            validacionesModel.TiposPlacas += recepcionSolicitudesPlacas.TiposPlacas;
            validacionesModel.URL_ArchivoPDF = recepcionSolicitudesPlacas.URL_ArchivoOficio;
            validacionesModel.Rangos = recepcionSolicitudesPlacas.Rangos;


            foreach (var item in recepcionSolicitudesPlacas.Observaciones)
            {
                validacionesModel.ListadoObservaciones.Add(new Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel() + item);
            }
            foreach (var item in recepcionSolicitudesPlacas.Archivos)
            {
                validacionesModel.ListadoArchivos.Add(new Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel() + item);
            }
            return validacionesModel;
        }
    }
}
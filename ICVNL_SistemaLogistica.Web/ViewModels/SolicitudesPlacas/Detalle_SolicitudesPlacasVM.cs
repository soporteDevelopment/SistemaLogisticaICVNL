using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_SolicitudesPlacasVM
    {
        public string NumeroFila { get; set; }

        [Key]
        [Display(Name = "Id Solicitud de Placas")]
        public int IdSolicitud { get; set; }

        [Display(Name = "Folio de Solicitud")]
        public string FolioSolicitud { get; set; }

        [Display(Name = "Fecha Solicitud")]
        public DateTime FechaSolicitud { get; set; }
        public string FechaSolicitudStr { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Fecha Entrega Solicitada")]
        public DateTime FechaEntrega { get; set; }
        public string FechaEntregaStr { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Nombre Proveedor")]
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Número de Contrato")]
        public int IdContrato { get; set; }
        public Detalle_ContratosVM Contratos { get; set; } = new Detalle_ContratosVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Orden de Compra")]
        public int IdOrdenCompra { get; set; }
        public string OrdenCompra { get; set; }
        public Detalle_SolicitudesPlacasDetailsVM DetalleSolicitud { get; set; } = new Detalle_SolicitudesPlacasDetailsVM();
        public List<Listado_SolicitudesPlacasDetailsModel> Detalle_SolicitudesPlacasDetailsVM { get; set; } = new List<Listado_SolicitudesPlacasDetailsModel>();

        public IEnumerable<dynamic> ListadoContratosDDL { get; set; }
        public IEnumerable<dynamic> ListadoProveedoresDDL { get; set; }
        public IEnumerable<dynamic> ListadoOrdenesCompraDDL { get; set; }

        public static Detalle_SolicitudesPlacasVM operator +(Detalle_SolicitudesPlacasVM detalle_SolicitudesPlacasVM, SolicitudesPlacas solicitudesPlacas)
        {
            detalle_SolicitudesPlacasVM.IdSolicitud = solicitudesPlacas.IdSolicitud;
            detalle_SolicitudesPlacasVM.FolioSolicitud = solicitudesPlacas.FolioSolicitud;
            detalle_SolicitudesPlacasVM.FechaSolicitud = solicitudesPlacas.FechaSolicitud;
            detalle_SolicitudesPlacasVM.FechaSolicitudStr = solicitudesPlacas.FechaSolicitud.ToString("dd/MM/yyyy");
            detalle_SolicitudesPlacasVM.FechaEntrega = solicitudesPlacas.FechaEntrega;
            detalle_SolicitudesPlacasVM.FechaEntregaStr = solicitudesPlacas.FechaEntrega.ToString("dd/MM/yyyy");
            detalle_SolicitudesPlacasVM.IdProveedor = solicitudesPlacas.IdProveedor;
            detalle_SolicitudesPlacasVM.Proveedores += solicitudesPlacas.Proveedores;
            detalle_SolicitudesPlacasVM.IdContrato = solicitudesPlacas.IdContrato;
            detalle_SolicitudesPlacasVM.Contratos += solicitudesPlacas.Contratos;
            detalle_SolicitudesPlacasVM.IdOrdenCompra = solicitudesPlacas.IdOrdenCompra;
            detalle_SolicitudesPlacasVM.OrdenCompra = solicitudesPlacas.OrdenesCompra.NumeroOrdenCompra;
            foreach (var item in solicitudesPlacas.SolicitudesPlacas_Detalle)
            {
                detalle_SolicitudesPlacasVM.Detalle_SolicitudesPlacasDetailsVM.Add(new Listado_SolicitudesPlacasDetailsModel() + item);
            }
            return detalle_SolicitudesPlacasVM;
        }

    }
}
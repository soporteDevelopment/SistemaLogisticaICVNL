using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_SolicitudesPlacasRecepcionVM
    {
        public string NumeroFila { get; set; }

        [Key]
        [Display(Name = "Id Solicitud Placas")]
        public int IdRecepcion { get; set; }

        [Display(Name = "Folio Recepción Solicitud Placas")]
        public string FolioRecepcion { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Solicitud de Placas a Proveedores")]
        public int IdSolicitud { get; set; }
        public Detalle_SolicitudesPlacasVM SolicitudesPlacas { get; set; } = new Detalle_SolicitudesPlacasVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Delegación")]
        public int IdDelegacionBanco { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Nota de Entrada Autorizada ")]
        public string NotaEntradaAutorizada { get; set; }
        public Boolean Recibida { get; set; }
        public List<Listado_SolicitudesPlacasRecepcionDetailsModel> RecepcionSolicitudesPlacas_Detalle { get; set; } = new List<Listado_SolicitudesPlacasRecepcionDetailsModel>();

        public Detalle_SolicitudesPlacasRecepcionDetailsVM DetalleRecepcion { get; set; } = new Detalle_SolicitudesPlacasRecepcionDetailsVM();

        public IEnumerable<dynamic> ListadoDelegacionesDDL { get; set; }
        public IEnumerable<dynamic> ListadoSolicitudesPlacasDDL { get; set; }

        public static Detalle_SolicitudesPlacasRecepcionVM operator +(Detalle_SolicitudesPlacasRecepcionVM detalle_SolicitudesPlacasRecepcionVM, RecepcionSolicitudesPlacas recepcionSolicitudesPlacas)
        {
            detalle_SolicitudesPlacasRecepcionVM.IdRecepcion = recepcionSolicitudesPlacas.IdRecepcion;
            detalle_SolicitudesPlacasRecepcionVM.FolioRecepcion = recepcionSolicitudesPlacas.FolioRecepcion;
            detalle_SolicitudesPlacasRecepcionVM.IdSolicitud = recepcionSolicitudesPlacas.IdSolicitud;
            detalle_SolicitudesPlacasRecepcionVM.SolicitudesPlacas += recepcionSolicitudesPlacas.SolicitudesPlacas;
            detalle_SolicitudesPlacasRecepcionVM.IdDelegacionBanco = recepcionSolicitudesPlacas.IdDelegacionBanco;
            detalle_SolicitudesPlacasRecepcionVM.DelegacionesBancos += recepcionSolicitudesPlacas.DelegacionesBancos;
            detalle_SolicitudesPlacasRecepcionVM.NotaEntradaAutorizada = recepcionSolicitudesPlacas.NotaEntradaAutorizada;
            detalle_SolicitudesPlacasRecepcionVM.Recibida = recepcionSolicitudesPlacas.Recibida;

            foreach (var item in recepcionSolicitudesPlacas.RecepcionSolicitudesPlacas_Detalle)
            {
                detalle_SolicitudesPlacasRecepcionVM.RecepcionSolicitudesPlacas_Detalle.Add(new Listado_SolicitudesPlacasRecepcionDetailsModel() + item);
            }
            return detalle_SolicitudesPlacasRecepcionVM;
        }
    }
}
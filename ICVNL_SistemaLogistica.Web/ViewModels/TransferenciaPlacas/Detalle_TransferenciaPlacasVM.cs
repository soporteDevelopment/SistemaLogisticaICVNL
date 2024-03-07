using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_TransferenciaPlacasVM
    {
        [Key]
        [Display(Name = "Id de la Transferencia")]
        public int IdTransferencia { get; set; }

        [Display(Name = "Folio de la Transferencia")]
        public string FolioTransferencia { get; set; }

        [Display(Name = "Fecha hora registro Transferencia")]
        public DateTime FechaHoraRegistro { get; set; }
        public string FechaHoraRegistroStr { get; set; }

        [Display(Name = "Datos Persona que se va a llevar las placas")]
        public int IdTransferenciaDatosPersonaEnvio { get; set; }
        public Detalle_TransferenciaPlacas_DatosPersonaEnvioVM TransferenciaPlacas_DatosPersonaEnvio { get; set; } = new Detalle_TransferenciaPlacas_DatosPersonaEnvioVM();

        [Display(Name = "Datos del Transporte en el que se van a llevar las Placas")]
        public int IdTransferenciaTransporteEnvio { get; set; }
        public Detalle_TransferenciaPlacas_TransporteEnvioVM TransferenciaPlacas_TransporteEnvio { get; set; } = new Detalle_TransferenciaPlacas_TransporteEnvioVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Delegación Origen")]
        public int IdDelegacionBancoOrigen { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancosOrigen { get; set; } = new Detalle_DelegacionesBancosVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Delegación Destino")]
        public int IdDelegacionBancoDestino { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancosDestino { get; set; } = new Detalle_DelegacionesBancosVM();

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Estatus Transferencia")]
        public int IdEstatusTransferencia { get; set; }
        public TiposEstatusTransferenciasVM TiposEstatusTransferencias { get; set; } = new TiposEstatusTransferenciasVM();

        public List<Listado_TransferenciaPlacas_Listado1_Model> TransferenciaPlacas_Listado1 { get; set; } = new List<Listado_TransferenciaPlacas_Listado1_Model>();
        public List<Listado_TransferenciaPlacas_Listado2_Model> TransferenciaPlacas_Listado2 { get; set; } = new List<Listado_TransferenciaPlacas_Listado2_Model>();

        public IEnumerable<dynamic> ListadoTiposIDsDDL { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesDDL { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesUsuarioDDL { get; set; }
        public IEnumerable<dynamic> ListadoEstatusTransferenciaDDL { get; set; }
        public IEnumerable<dynamic> ListadoEstatusPlacasDDL { get; set; }

        public static Detalle_TransferenciaPlacasVM operator +(Detalle_TransferenciaPlacasVM transferenciaPlacasVM, TransferenciaPlacas transferenciaPlacas)
        {

            transferenciaPlacasVM.IdTransferencia = transferenciaPlacas.IdTransferencia;
            transferenciaPlacasVM.FolioTransferencia = transferenciaPlacas.FolioTransferencia;
            transferenciaPlacasVM.FechaHoraRegistro = transferenciaPlacas.FechaHoraRegistro;
            transferenciaPlacasVM.FechaHoraRegistroStr = transferenciaPlacas.FechaHoraRegistro.ToString("dd/MM/yyyy hh:mm:ss tt");
            transferenciaPlacasVM.IdTransferenciaDatosPersonaEnvio = transferenciaPlacas.IdTransferenciaDatosPersona;
            transferenciaPlacasVM.TransferenciaPlacas_DatosPersonaEnvio += transferenciaPlacas.TransferenciaPlacas_DatosPersona;
            transferenciaPlacasVM.IdTransferenciaTransporteEnvio = transferenciaPlacas.IdTransferenciaTransporte;
            transferenciaPlacasVM.TransferenciaPlacas_TransporteEnvio += transferenciaPlacas.TransferenciaPlacas_Transporte;

            transferenciaPlacasVM.IdDelegacionBancoOrigen = transferenciaPlacas.IdDelegacionBancoOrigen;
            transferenciaPlacasVM.DelegacionesBancosOrigen += transferenciaPlacas.DelegacionesBancosOrigen;
            transferenciaPlacasVM.IdDelegacionBancoDestino = transferenciaPlacas.IdDelegacionBancoDestino;
            transferenciaPlacasVM.DelegacionesBancosDestino += transferenciaPlacas.DelegacionesBancosDestino;

            transferenciaPlacasVM.IdEstatusTransferencia = transferenciaPlacas.IdEstatusTransferencia;
            transferenciaPlacasVM.TiposEstatusTransferencias += transferenciaPlacas.TiposEstatusTransferencias;

            foreach (var item in transferenciaPlacas.TransferenciaPlacas_Listado1)
            {
                transferenciaPlacasVM.TransferenciaPlacas_Listado1.Add(new Listado_TransferenciaPlacas_Listado1_Model() + item);
            }

            foreach (var item in transferenciaPlacas.TransferenciaPlacas_Listado2)
            {
                transferenciaPlacasVM.TransferenciaPlacas_Listado2.Add(new Listado_TransferenciaPlacas_Listado2_Model() + item);
            }

            return transferenciaPlacasVM;
        }

    }
}
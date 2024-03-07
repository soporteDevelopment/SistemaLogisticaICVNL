using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_RecepcionPlacasVM
    {
        [Key]
        [Display(Name = "Id de la Transferencia")]
        public int IdTransferencia { get; set; }

        [Display(Name = "Folio de la Transferencia")]
        public string FolioTransferencia { get; set; }

        [Display(Name = "Fecha hora registro Transferencia")]
        public DateTime FechaHoraRegistro { get; set; }

        [Display(Name = "Datos Persona que va a recibir las placas")]
        public int IdTransferenciaDatosPersonaRecibe { get; set; }
        public Detalle_RecepcionPlacas_DatosPersonaRecibeVM RecepcionPlacas_DatosPersonaRecibe { get; set; } = new Detalle_RecepcionPlacas_DatosPersonaRecibeVM();

        [Display(Name = "Datos del Transporte en el que llegaron las Placas ")]
        public int IdTransferenciaTransporteRecibe { get; set; }
        public Detalle_RecepcionPlacas_TransporteRecibeVM RecepcionPlacas_TransporteRecibe { get; set; } = new Detalle_RecepcionPlacas_TransporteRecibeVM();

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

        public List<Listado_RecepcionPlacas_Listado1_Model> RecepcionPlacas_Listado1 { get; set; } = new List<Listado_RecepcionPlacas_Listado1_Model>();
        public List<Listado_RecepcionPlacas_Listado2_Model> RecepcionPlacas_Listado2 { get; set; } = new List<Listado_RecepcionPlacas_Listado2_Model>();

        public IEnumerable<dynamic> ListadoTiposIDsDDL { get; set; }
        public IEnumerable<dynamic> ListadoDelegacionesDDL { get; set; }
        public IEnumerable<dynamic> ListadoEstatusTransferenciaDDL { get; set; }
        public IEnumerable<dynamic> ListadoEstatusPlacasDDL { get; set; }

        public static Detalle_RecepcionPlacasVM operator +(Detalle_RecepcionPlacasVM RecepcionPlacasVM, TransferenciaPlacas RecepcionPlacas)
        {

            RecepcionPlacasVM.IdTransferencia = RecepcionPlacas.IdTransferencia;
            RecepcionPlacasVM.FolioTransferencia = RecepcionPlacas.FolioTransferencia;
            RecepcionPlacasVM.FechaHoraRegistro = RecepcionPlacas.FechaHoraRegistro;
            RecepcionPlacasVM.IdTransferenciaDatosPersonaRecibe = RecepcionPlacas.IdTransferenciaDatosPersona;
            RecepcionPlacasVM.RecepcionPlacas_DatosPersonaRecibe += RecepcionPlacas.TransferenciaPlacas_DatosPersona;
            RecepcionPlacasVM.IdTransferenciaTransporteRecibe = RecepcionPlacas.IdTransferenciaTransporte;
            RecepcionPlacasVM.RecepcionPlacas_TransporteRecibe += RecepcionPlacas.TransferenciaPlacas_Transporte;

            RecepcionPlacasVM.IdDelegacionBancoOrigen = RecepcionPlacas.IdDelegacionBancoOrigen;
            RecepcionPlacasVM.DelegacionesBancosOrigen += RecepcionPlacas.DelegacionesBancosOrigen;
            RecepcionPlacasVM.IdDelegacionBancoDestino = RecepcionPlacas.IdDelegacionBancoDestino;
            RecepcionPlacasVM.DelegacionesBancosDestino += RecepcionPlacas.DelegacionesBancosDestino;

            RecepcionPlacasVM.IdEstatusTransferencia = RecepcionPlacas.IdEstatusTransferencia;
            RecepcionPlacasVM.TiposEstatusTransferencias += RecepcionPlacas.TiposEstatusTransferencias;

            foreach (var item in RecepcionPlacas.TransferenciaPlacas_Listado1)
            {
                RecepcionPlacasVM.RecepcionPlacas_Listado1.Add(new Listado_RecepcionPlacas_Listado1_Model() + item);
            }

            foreach (var item in RecepcionPlacas.TransferenciaPlacas_Listado2)
            {
                RecepcionPlacasVM.RecepcionPlacas_Listado2.Add(new Listado_RecepcionPlacas_Listado2_Model() + item);
            }

            return RecepcionPlacasVM;
        }

    }
}
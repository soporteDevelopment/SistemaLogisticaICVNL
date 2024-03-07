using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_TransferenciaPlacasModel
    {
        public int IdTransferencia { get; set; }
        public string FolioTransferencia { get; set; }
        public DateTime FechaHoraRegistro { get; set; }

        public int IdTransferenciaDatosPersonaEnvio { get; set; }
        public Detalle_TransferenciaPlacas_DatosPersonaEnvioVM TransferenciaPlacas_DatosPersonaEnvio { get; set; } = new Detalle_TransferenciaPlacas_DatosPersonaEnvioVM();
        public int IdTransferenciaTransporteEnvio { get; set; }
        public Detalle_TransferenciaPlacas_TransporteEnvioVM TransferenciaPlacas_TransporteEnvio { get; set; } = new Detalle_TransferenciaPlacas_TransporteEnvioVM();

        public int IdDelegacionBancoOrigen { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancosOrigen { get; set; } = new Detalle_DelegacionesBancosVM();

        public int IdDelegacionBancoDestino { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancosDestino { get; set; } = new Detalle_DelegacionesBancosVM();
        public int IdEstatusTransferencia { get; set; }
        public TiposEstatusTransferenciasVM TiposEstatusTransferencias { get; set; } = new TiposEstatusTransferenciasVM();

        public static Listado_TransferenciaPlacasModel operator +(Listado_TransferenciaPlacasModel transferenciaPlacasM, TransferenciaPlacas transferenciaPlacas)
        {
            transferenciaPlacasM.IdTransferencia = transferenciaPlacas.IdTransferencia;
            transferenciaPlacasM.FolioTransferencia = transferenciaPlacas.FolioTransferencia;
            transferenciaPlacasM.FechaHoraRegistro = transferenciaPlacas.FechaHoraRegistro;
            transferenciaPlacasM.IdTransferenciaDatosPersonaEnvio = transferenciaPlacas.IdTransferenciaDatosPersona;
            transferenciaPlacasM.TransferenciaPlacas_DatosPersonaEnvio += transferenciaPlacas.TransferenciaPlacas_DatosPersona;
            transferenciaPlacasM.IdTransferenciaTransporteEnvio = transferenciaPlacas.IdTransferenciaTransporte;
            transferenciaPlacasM.TransferenciaPlacas_TransporteEnvio += transferenciaPlacas.TransferenciaPlacas_Transporte;

            transferenciaPlacasM.IdDelegacionBancoOrigen = transferenciaPlacas.IdDelegacionBancoOrigen;
            transferenciaPlacasM.DelegacionesBancosOrigen += transferenciaPlacas.DelegacionesBancosOrigen;
            transferenciaPlacasM.IdDelegacionBancoDestino = transferenciaPlacas.IdDelegacionBancoDestino;
            transferenciaPlacasM.DelegacionesBancosDestino += transferenciaPlacas.DelegacionesBancosDestino;

            transferenciaPlacasM.IdEstatusTransferencia = transferenciaPlacas.IdEstatusTransferencia;
            transferenciaPlacasM.TiposEstatusTransferencias += transferenciaPlacas.TiposEstatusTransferencias;

            return transferenciaPlacasM;
        }
    }
}
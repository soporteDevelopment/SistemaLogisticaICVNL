using ICVNL_SistemaLogistica.Web.Entities;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ConsultaInformacionPlacas_Detalle_TransferenciasPlacasModel
    {
        public string FolioTransferencia { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string TipoIDs { get; set; }
        public string NumeroID { get; set; }
        public string MarcaVehiculo { get; set; }
        public string ModeloVehiculo { get; set; }
        public string PlacasVehiculo { get; set; }
        public string NumeroEconomico { get; set; }
        public string DelegacionOrigen { get; set; }
        public string DelegacionDestino { get; set; }
        public string EstatusTransferencia { get; set; }

        public static Listado_ConsultaInformacionPlacas_Detalle_TransferenciasPlacasModel operator +(Listado_ConsultaInformacionPlacas_Detalle_TransferenciasPlacasModel Detalle_TransferenciasPlacas, ConsultaInformacionPlacas_Detalle_TransferenciasPlacas transferenciasPlacas)
        {
            Detalle_TransferenciasPlacas.FolioTransferencia = transferenciasPlacas.FolioTransferencia;
            Detalle_TransferenciasPlacas.FechaHoraRegistro = transferenciasPlacas.FechaHoraRegistro;
            Detalle_TransferenciasPlacas.Nombre = transferenciasPlacas.TransferenciaPlacas_DatosPersona.Nombre;
            Detalle_TransferenciasPlacas.Apellido = transferenciasPlacas.TransferenciaPlacas_DatosPersona.Apellido;
            Detalle_TransferenciasPlacas.TipoIDs = transferenciasPlacas.TransferenciaPlacas_DatosPersona.TiposID.TipoID;
            Detalle_TransferenciasPlacas.NumeroID = transferenciasPlacas.TransferenciaPlacas_DatosPersona.NumeroID;
            Detalle_TransferenciasPlacas.MarcaVehiculo = transferenciasPlacas.TransferenciaPlacas_Transporte.MarcaVehiculo;
            Detalle_TransferenciasPlacas.ModeloVehiculo = transferenciasPlacas.TransferenciaPlacas_Transporte.ModeloVehiculo;
            Detalle_TransferenciasPlacas.PlacasVehiculo = transferenciasPlacas.TransferenciaPlacas_Transporte.PlacasVehiculo;
            Detalle_TransferenciasPlacas.NumeroEconomico = transferenciasPlacas.TransferenciaPlacas_Transporte.NumeroEconomico;
            Detalle_TransferenciasPlacas.DelegacionOrigen = transferenciasPlacas.DelegacionesBancosOrigen.NombreDelegacionBanco;
            Detalle_TransferenciasPlacas.DelegacionDestino = transferenciasPlacas.DelegacionesBancosDestino.NombreDelegacionBanco;
            Detalle_TransferenciasPlacas.EstatusTransferencia = transferenciasPlacas.TiposEstatusTransferencias.EstatusTransferencia;
            return Detalle_TransferenciasPlacas;
        }

    }
}
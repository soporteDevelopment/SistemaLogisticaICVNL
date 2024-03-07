using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class ConsultaInformacionPlacas_Detalle_TransferenciasPlacas
    {
        public string FolioTransferencia { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public int IdTransferenciaDatosPersona { get; set; }
        public TransferenciaPlacas_DatosPersona TransferenciaPlacas_DatosPersona { get; set; }
        public int IdTransferenciaTransporte { get; set; }
        public TransferenciaPlacas_Transporte TransferenciaPlacas_Transporte { get; set; }
        public int IdDelegacionBancoOrigen { get; set; }
        public DelegacionesBancos DelegacionesBancosOrigen { get; set; }
        public int IdDelegacionBancoDestino { get; set; }
        public DelegacionesBancos DelegacionesBancosDestino { get; set; }
        public int IdEstatusTransferencia { get; set; }
        public TiposEstatusTransferencias TiposEstatusTransferencias { get; set; }
    }
}

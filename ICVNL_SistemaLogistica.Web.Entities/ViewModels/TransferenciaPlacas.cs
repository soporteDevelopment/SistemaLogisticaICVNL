using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class TransferenciaPlacas
    {
        public int IdTransferencia { get; set; }
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
        public List<TransferenciaPlacas_Detalle> TransferenciaPlacas_Detalle { get; set; }
        public List<TransferenciaPlacas_Listado1> TransferenciaPlacas_Listado1 { get; set; }
        public List<TransferenciaPlacas_Listado2> TransferenciaPlacas_Listado2 { get; set; }
        public string UsuarioRegistro { get; set; }
        public int Entidad { get; set; }

    }
}

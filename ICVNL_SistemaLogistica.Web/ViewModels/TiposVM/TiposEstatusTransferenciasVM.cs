using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class TiposEstatusTransferenciasVM
    {
        public int IdEstatusTransferencia { get; set; }
        public string EstatusTransferencia { get; set; }

        public static TiposEstatusTransferenciasVM operator +(TiposEstatusTransferenciasVM tiposEstatusTransferenciasVM, TiposEstatusTransferencias tiposEstatus)
        {
            tiposEstatusTransferenciasVM.IdEstatusTransferencia = tiposEstatus.IdEstatusTransferencia;
            tiposEstatusTransferenciasVM.EstatusTransferencia = tiposEstatus.EstatusTransferencia;
            return tiposEstatusTransferenciasVM;
        }
    }
}
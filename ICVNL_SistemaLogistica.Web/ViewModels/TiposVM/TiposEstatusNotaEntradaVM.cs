using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class TiposEstatusNotaEntradaVM
    {
        public int IdEstatusNotaentrada { get; set; }
        public string EstatusNotaEntrada { get; set; }
        public static TiposEstatusNotaEntradaVM operator +(TiposEstatusNotaEntradaVM tiposEstatusNota, TiposEstatusNotaEntrada tiposEstatusNotaEntrada)
        {
            tiposEstatusNota.IdEstatusNotaentrada = tiposEstatusNotaEntrada.IdEstatusNotaentrada;
            tiposEstatusNota.EstatusNotaEntrada = tiposEstatusNotaEntrada.EstatusNotaEntrada;
            return tiposEstatusNota;
        }
    }
}
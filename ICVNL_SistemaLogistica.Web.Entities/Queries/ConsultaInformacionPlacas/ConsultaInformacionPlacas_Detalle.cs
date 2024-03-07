using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class ConsultaInformacionPlacas_Detalle
    { 
        public int IdInventario { get; set; }
        public string NumeroPlaca { get; set; }
        public string TipoPlaca { get; set; }
        public string CostoPlaca { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string NumeroNotaEntrada { get; set; }
        public DateTime FechaNotaEntrada { get; set; }
        public DateTime FechaIngresoNotaEntradaSistema { get; set; }
        public string RenglonNotaEntrada { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public string RenglonOrdenCompra { get; set; }
        public string ProveedorOrdenCompra { get; set; }
        public string Entidad { get; set; }
        public int IdDelegacionBanco { get; set; }
        public string AlmacenRecibioPlaca { get; set; }
        public string CuentaContableRecepcionPlaca { get; set; }
        public string CentroCostosAlmacenOrdenCompra { get; set; }
        public int IdTipoEstatusPlaca { get; set; }
        public string EstatusActualPlaca { get; set; }
        public string PlacaContabilizada { get; set; }
        public string NumeroPolizaGRP_Infofin { get; set; }
        public List<ConsultaInformacionPlacas_Detalle_TransferenciasPlacas> TransferenciasPlacas { get; set; }
        public List<ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas> CambiosEstatusPlacas { get; set; }

    }
}

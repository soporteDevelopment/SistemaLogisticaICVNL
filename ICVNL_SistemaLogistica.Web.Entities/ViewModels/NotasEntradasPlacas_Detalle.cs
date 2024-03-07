using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class NotasEntradasPlacas_Detalle
    {
        public int IdNotaEntradaDetalle { get; set; }
        public int IdNotaEntrada { get; set; }
        public int NumeroRenglon { get; set; }
        public string CuentaContable { get; set; }
        public string CentroCostosAlmacen { get; set; }
        
        public string CodigoArticuloInfofin { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; } 
        public int CantidadPlacas { get; set; }
        public decimal CostoPlaca { get; set; }
        public decimal CostoTotal { get; set; }
        public int CantidadNumerosPlacaIdentificada { get; set; }
        public int CantidadNumerosPlacaPorIdentificarse { get; set; }
        public int IdEstatusNotaEntrada { get; set; }
        public TiposEstatusNotaEntrada TiposEstatus { get; set; }
        public int Entidad { get; set; }
        public DateTime FechaRecepcionTP { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM
    {
        public int IdEventoRecepcion { get; set; }
        public int IdValidacionEvento { get; set; }
        public int Consecutivo { get; set; }
        public int IdRecepcion { get; set; }
        [Display(Name = "Fecha Hora de Registro")]
        public DateTime FechaHoraRegistro { get; set; } 
        [Display(Name = "Destino (Delegación Banco)")]
        public int IdDelegacionBanco { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();
        [Display(Name = "Proveedor")]
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();
        [Display(Name = "Número de Caja")]
        public string NumeroCaja { get; set; }
        [Display(Name = "Tipo Estructura (Tipo de Placa)")]
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        [Display(Name = "Rangos de las Placas")]
        public string RangosPlacas { get; set; }

        [Display(Name = "Rangos Inicial")]
        public string RangoInicial { get; set; }

        [Display(Name = "Rangos Final")]
        public string RangoFinal { get; set; }
        [Display(Name = "Cantidad Laminas")]
        public int CantidadLaminas { get; set; }

    }
}

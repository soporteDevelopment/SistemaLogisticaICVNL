using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class SolicitudesPlacas_Detalle
    {
        public int IdSolicitudDetalle { get; set; }
        public int IdSolicitud { get; set; }
        public int IdDelegacionBanco { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public string RangoPlacaInicial { get; set; }
        public string RangoPlacaFinal { get; set; }
        public int CantidadPlacas { get; set; }
        public int CantidadPlacasOrdenCompra { get; set; }
        public int CantidadPlacasNotasEntrada { get; set; }
        public int Entidad { get; set; }
        public int IdDetalleOrdenCompra { get; set; }

    }
}

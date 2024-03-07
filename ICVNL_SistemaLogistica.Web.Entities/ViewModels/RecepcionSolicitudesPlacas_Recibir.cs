using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class RecepcionSolicitudesPlacas_Recibir
    {
        public int IdEventoRecepcion { get; set; }
        public int IdValidacionEvento { get; set; }
        public int IdSolicitud { get; set; }
        public DateTime FechaHoraRegistro { get; set; }
        public int IdRecepcion { get; set; }
        public RecepcionSolicitudesPlacas RecepcionSolicitudesPlacas { get; set; }
        public int IdProveedor { get; set; }
        public Proveedores Proveedores { get; set; }
        public string NumeroCaja { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public string Rangos { get; set; }
        public int CantidadLaminas { get; set; }
        public int IdDelegacionBanco { get; set; }
        public int Entidad { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public RecepcionSolicitudesPlacas_Recibir_Validaciones RecepcionSolicitudesPlacas_Recibir_Validaciones { get; set; }

    }
}

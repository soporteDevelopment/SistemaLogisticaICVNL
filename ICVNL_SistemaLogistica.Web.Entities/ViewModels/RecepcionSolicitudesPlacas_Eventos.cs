using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class RecepcionSolicitudesPlacas_Eventos
    {
        public int IdEventoRecepcion { get; set; }
        public int IdArchivo { get; set; }
        
        public DateTime FechaHoraRegistro { get; set; }
        public int IdSolicitud { get; set; }
        public int IdRecepcion { get; set; }
        public RecepcionSolicitudesPlacas RecepcionSolicitudesPlacas { get; set; }
        public int IdProveedor { get; set; }
        public Proveedores Proveedores { get; set; }
        public int IdDelegacionBanco { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public string NumeroCaja { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public string Rangos { get; set; }
        public int CantidadLaminas { get; set; }
        public string Delegacion { get; set; }
        public int IdTiposEventosRecepcionPlacas { get; set; }
        public string TipoEventosRecepcionPlacas { get; set; }
        public string URL_ArchivoOficio { get; set; }
        public List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos> Archivos { get; set; }
        public List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones> Observaciones { get; set; }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class RecepcionSolicitudesPlacas_Recibir_Validaciones
    {
        public int IdRecepcionValidaciones { get; set; }
        public int IdArchivo { get; set; }
        public int IdRecepcion { get; set; }
        public int IdEventoRecepcion { get; set; }
        public string Horas { get; set; }
        public string Fecha { get; set; }
        public string UsuarioNombre { get; set; }
        public Usuarios Usuario { get; set; }
        public int IdNotaEntrada { get; set; }
        public int IdDelegacionesBancos { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; }
        public string NotaEntrada { get; set; }
        public int IdProveedor { get; set; }
        public Proveedores Proveedores { get; set; }
        public int IdContrato { get; set; }
        public Contratos Contrato { get; set; }
        public string PartidaContrato { get; set; }
        public int IdTipoProblema { get; set; }
        public TiposEventosRecepcionPlacas TiposEventosRecepcionPlacas { get; set; }
        public string CajaNdeM { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TipoPlaca { get; set; } 

        public List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos> Archivos { get; set; }
        public List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones> Observaciones { get; set; }

    }
}

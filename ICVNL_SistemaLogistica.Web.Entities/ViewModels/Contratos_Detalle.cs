using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Contratos_Detalle
    {
        public int IdContratoDetalle { get; set; }
        public int IdContrato { get; set; }

        public int Consecutivo { get; set; }
        public int IdProveedor { get; set; }
        public Proveedores Proveedores { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TipoPlacas { get; set; }
        public int CantidadPlacas { get; set; }
        public int CantidadPlacasCaja { get; set; }
        public string MascaraPlaca { get; set; }
        public string OrdenPlaca { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public string OficioSICT { get; set; }
        public int Entidad { get; set; }

        public List<Contratos_Detalles_Rangos> Contratos_Detalles_Rangos { get; set; } = new List<Contratos_Detalles_Rangos>(); 
        
    }
}

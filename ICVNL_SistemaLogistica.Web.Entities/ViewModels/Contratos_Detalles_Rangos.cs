using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Contratos_Detalles_Rangos
    {
        public int IdContratoDetalleRangos { get; set; }
        public int Consecutivo { get; set; }
        public int IdContratoDetalle { get; set; }
        public string RangoInicial { get; set; }
        public string RangoFinal { get; set; }
        public string CantidadSerie { get; set; }
        public int Entidad { get; set; }

        public Contratos_Detalles_Rangos(int IdContratoDetalleRangos_,
                                         int Consecutivo_,
                                         int IdContratoDetalle_,
                                         string RangoInicial_,
                                         string RangoFinal_,
                                         string CantidadSerie_)
        {
            this.IdContratoDetalleRangos = IdContratoDetalleRangos_;
            this.Consecutivo = Consecutivo_;
            this.IdContratoDetalle = IdContratoDetalle_;
            this.RangoInicial = RangoInicial_;
            this.RangoFinal = RangoFinal_;
            this.CantidadSerie = CantidadSerie_;
        }

        public Contratos_Detalles_Rangos()
        {

        }
    }
}

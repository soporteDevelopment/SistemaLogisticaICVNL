using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class RecepcionSolicitudesPlacas_Detalle
    {
        public int IdRecepcionDetalle { get; set; }
        public int IdRecepcion { get; set; }
        public int IdTipoPlaca { get; set; }
        public TiposPlacas TiposPlacas { get; set; }
        public int CantidadSolicitadaOrdenCompra { get; set; }
        public int CantidadNotasEntradaAutorizada { get; set; }
        public int CantidadRecibida { get; set; }
        public int Entidad { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class InventarioPlacas_Relacion_NE_OC
    {
        public int idRelacionInventario { get; set; }
        public int IdInventarioDetalle { get; set; }
        public int IdOrdenCompra { get; set; }
        public int IdNotaEntrada { get; set; }
        public int RenglonNE { get; set; }
        public int RenglonOC { get; set; }
        public int Entidad { get; set; }
    }
}

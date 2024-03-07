using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_EstadosModel
    {
        public int Id { get; set; }
        public string Estado { get; set; }
        public int Estatus { get; set; }

        public static Listado_EstadosModel operator +(Listado_EstadosModel detalle_EstadosVM, Estados estados)
        {
            detalle_EstadosVM.Id = estados.Id;
            detalle_EstadosVM.Estado = estados.Estado;
            detalle_EstadosVM.Estatus = estados.Estatus;

            return detalle_EstadosVM;
        }
    }
}
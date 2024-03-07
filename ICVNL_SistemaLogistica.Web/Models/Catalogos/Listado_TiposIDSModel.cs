using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_TiposIDSModel
    {
        public int Id { get; set; }
        public string TipoID { get; set; }
        public int Estatus { get; set; }

        public static Listado_TiposIDSModel operator +(Listado_TiposIDSModel detalle_TiposIDsVM, TiposIDs tiposIDs)
        {
            detalle_TiposIDsVM.Id = tiposIDs.Id;
            detalle_TiposIDsVM.TipoID = tiposIDs.TipoID;
            detalle_TiposIDsVM.Estatus = tiposIDs.Estatus;

            return detalle_TiposIDsVM;
        }
    }
}
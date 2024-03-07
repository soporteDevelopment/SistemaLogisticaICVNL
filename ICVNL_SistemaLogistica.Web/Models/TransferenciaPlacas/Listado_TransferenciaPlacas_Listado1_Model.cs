using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_TransferenciaPlacas_Listado1_Model
    {
        public int IdTransferenciaListado1 { get; set; }
        public int IdTransferencia { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int CantidadDisponiblesDelegacion { get; set; }
        public int CantidadDisponiblesSerTransferidas { get; set; }

        public static Listado_TransferenciaPlacas_Listado1_Model operator +(Listado_TransferenciaPlacas_Listado1_Model _Listado1_Model, TransferenciaPlacas_Listado1 placas_Listado1)
        {
            _Listado1_Model.IdTransferenciaListado1 = placas_Listado1.IdTransferenciaListado1;
            _Listado1_Model.IdTransferencia = placas_Listado1.IdTransferencia;
            _Listado1_Model.IdTipoPlaca = placas_Listado1.IdTipoPlaca;
            _Listado1_Model.TiposPlacas += placas_Listado1.TiposPlacas;
            _Listado1_Model.CantidadDisponiblesDelegacion = placas_Listado1.CantidadDisponiblesDelegacion;
            _Listado1_Model.CantidadDisponiblesSerTransferidas = placas_Listado1.CantidadDisponiblesSerTransferidas;

            return _Listado1_Model;
        }

    }
}
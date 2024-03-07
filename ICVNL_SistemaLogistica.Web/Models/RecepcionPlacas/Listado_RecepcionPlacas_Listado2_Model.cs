using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_RecepcionPlacas_Listado2_Model
    {
        public int IdTransferenciaListado2 { get; set; }
        public int IdTransferencia { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int IdTipoEstatusPlacas { get; set; }
        public TiposEstatusPlacasVM TiposEstatusPlacas { get; set; } = new TiposEstatusPlacasVM();
        public Boolean Automatico { get; set; }
        public Boolean TransferirPlaca { get; set; }
        public string NumeroPlaca { get; set; }

        public static Listado_RecepcionPlacas_Listado2_Model operator +(Listado_RecepcionPlacas_Listado2_Model _Listado2_Model, TransferenciaPlacas_Listado2 placas_Listado2)
        {
            _Listado2_Model.IdTransferenciaListado2 = placas_Listado2.IdTransferenciaListado2;
            _Listado2_Model.IdTransferencia = placas_Listado2.IdTransferencia;
            _Listado2_Model.IdTipoPlaca = placas_Listado2.IdTipoPlaca;
            _Listado2_Model.TiposPlacas += placas_Listado2.TiposPlacas;
            _Listado2_Model.IdTipoEstatusPlacas = placas_Listado2.IdTipoEstatusPlacas;
            _Listado2_Model.TiposEstatusPlacas += placas_Listado2.TiposEstatusPlacas;
            _Listado2_Model.Automatico = placas_Listado2.Automatico;
            _Listado2_Model.TransferirPlaca = placas_Listado2.TransferirPlaca;
            _Listado2_Model.NumeroPlaca = placas_Listado2.NumeroPlaca;

            return _Listado2_Model;
        }

    }
}
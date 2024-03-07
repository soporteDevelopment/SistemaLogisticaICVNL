using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_RecepcionPlacas_IndividualesModel
    {
        public int IdTransferenciaIndividual { get; set; }
        public int IdTransferenciaListado2 { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TiposPlacas { get; set; } = new Detalle_TiposPlacasVM();
        public int IdTipoEstatusPlacas { get; set; }
        public TiposEstatusPlacasVM TiposEstatusPlacas { get; set; } = new TiposEstatusPlacasVM();
        public string NumeroPlaca { get; set; }
        public Boolean Disponible { get; set; }
        public Boolean Obsoleta { get; set; }
        public Boolean Rechazada { get; set; }
        public Boolean Perdida { get; set; }
        public Boolean Seleccionado { get; set; }

        public static Listado_RecepcionPlacas_IndividualesModel operator +(Listado_RecepcionPlacas_IndividualesModel _Listado2_Model, TransferenciaPlacas_RecibirPlacasIndividuales _RecibirPlacasIndividuales)
        {
            _Listado2_Model.IdTransferenciaIndividual = _RecibirPlacasIndividuales.IdTransferenciaListado2;
            _Listado2_Model.IdTransferenciaListado2 = _RecibirPlacasIndividuales.IdTransferenciaListado2;
            _Listado2_Model.NumeroPlaca = _RecibirPlacasIndividuales.NumeroPlaca;
            _Listado2_Model.IdTipoPlaca = _RecibirPlacasIndividuales.IdTipoPlaca;
            _Listado2_Model.TiposPlacas += _RecibirPlacasIndividuales.TiposPlacas;
            _Listado2_Model.IdTipoEstatusPlacas = _RecibirPlacasIndividuales.IdTipoEstatusPlacas;
            _Listado2_Model.TiposEstatusPlacas += _RecibirPlacasIndividuales.TiposEstatusPlacas;
            _Listado2_Model.Disponible = _RecibirPlacasIndividuales.Disponible;
            _Listado2_Model.Obsoleta = _RecibirPlacasIndividuales.Obsoleta;
            _Listado2_Model.Rechazada = _RecibirPlacasIndividuales.Rechazada;
            _Listado2_Model.Obsoleta = _RecibirPlacasIndividuales.Obsoleta;
            _Listado2_Model.Perdida = _RecibirPlacasIndividuales.Perdida;

            return _Listado2_Model;
        }
    }
}
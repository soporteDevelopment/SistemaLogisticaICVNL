using ICVNL_SistemaLogistica.Web.Entities;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacasModel
    {
        public string NumeroPlaca { get; set; }
        public DateTime FechaHoraCambioEstatus { get; set; }
        public string OperacionRealizada { get; set; }
        public string EstatusAnterior { get; set; }
        public string EstatusNuevo { get; set; }

        public static Listado_ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacasModel operator +(Listado_ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacasModel _CambiosEstatusPlacasModel, ConsultaInformacionPlacas_Detalle_CambiosEstatusPlacas estatusPlacas)
        {
            _CambiosEstatusPlacasModel.NumeroPlaca = estatusPlacas.NumeroPlaca;
            _CambiosEstatusPlacasModel.FechaHoraCambioEstatus = estatusPlacas.FechaHoraCambioEstatus;
            _CambiosEstatusPlacasModel.OperacionRealizada = estatusPlacas.OperacionRealizada;
            _CambiosEstatusPlacasModel.EstatusAnterior = estatusPlacas.EstatusAnterior;
            _CambiosEstatusPlacasModel.EstatusNuevo = estatusPlacas.EstatusNuevo;
            return _CambiosEstatusPlacasModel;
        }
    }
}
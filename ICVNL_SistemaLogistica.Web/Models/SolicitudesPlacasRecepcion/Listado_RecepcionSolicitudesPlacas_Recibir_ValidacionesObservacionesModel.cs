using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel
    {
        public int IdObservacion { get; set; }
        public string Observacion { get; set; }

        public static Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel operator +(Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel _ValidacionesArchivosModel, RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones _Observaciones)
        {
            _ValidacionesArchivosModel.IdObservacion = _Observaciones.IdObservacion;
            _ValidacionesArchivosModel.Observacion = _Observaciones.Observacion;
            return _ValidacionesArchivosModel;
        }
    }
}
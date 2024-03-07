using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel
    {
        public int IdArchivo { get; set; } 
        public int Consecutivo { get; set; }
        public Byte[] Archivo { get; set; }
        public string NombreArchivo { get; set; }
        public string ArchivoBase64 { get; set; }

        public static Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel operator +(Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel _ValidacionesArchivosModel, RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos _Archivos)
        {
            _ValidacionesArchivosModel.IdArchivo = _Archivos.IdArchivo; 
            _ValidacionesArchivosModel.NombreArchivo = _Archivos.NombreArchivo;
            _ValidacionesArchivosModel.Consecutivo = _Archivos.Consecutivo;
            if (_Archivos.Archivo != null)
            {
                _ValidacionesArchivosModel.ArchivoBase64 = Convert.ToBase64String(_Archivos.Archivo);
            }
            return _ValidacionesArchivosModel;
        }
    }
}
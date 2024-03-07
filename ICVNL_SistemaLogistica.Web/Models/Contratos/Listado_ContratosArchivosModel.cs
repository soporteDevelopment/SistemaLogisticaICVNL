using ICVNL_SistemaLogistica.Web.Entities;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ContratosArchivosModel
    {
        public int IdContrato { get; set; }
        public int Consecutivo { get; set; }
        public string ArchivoBase64 { get; set; }
        public Byte[] ArchivoBytes { get; set; }
        public string NombreArchivo { get; set; }

        public static Listado_ContratosArchivosModel operator +(Listado_ContratosArchivosModel detalle_ContratosArchivosVM, Contratos_Archivos contratos_Archivos)
        {
            detalle_ContratosArchivosVM.IdContrato = contratos_Archivos.IdContrato;
            detalle_ContratosArchivosVM.Consecutivo = contratos_Archivos.Consecutivo;
            detalle_ContratosArchivosVM.ArchivoBytes = null;
            if (contratos_Archivos.Archivo != null)
            {
                detalle_ContratosArchivosVM.ArchivoBase64 = Convert.ToBase64String(contratos_Archivos.Archivo);
            }
            detalle_ContratosArchivosVM.NombreArchivo += contratos_Archivos.NombreArchivo;
            return detalle_ContratosArchivosVM;
        }
    }
}
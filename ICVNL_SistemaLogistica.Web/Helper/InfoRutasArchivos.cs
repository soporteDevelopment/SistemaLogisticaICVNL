using ICVNL_SistemaLogistica.Web.Entities;
using System.Configuration;

namespace ICVNL_SistemaLogistica.Web.Helper
{
    public class InfoRutasArchivos
    {
        public static RutasArchivos GetInfoFilePath()
        {
            return new RutasArchivos()
            {
                DirectorioArchivos = ConfigurationManager.AppSettings["DirectorioArchivos"].ToString(),
                DirectorioArchivosPDF = ConfigurationManager.AppSettings["DirectorioArchivos"].ToString() + "PDF",
                DirectorioPlantillas = ConfigurationManager.AppSettings["DirectorioArchivos"].ToString() + "Plantillas",
            };
        }
    }
}
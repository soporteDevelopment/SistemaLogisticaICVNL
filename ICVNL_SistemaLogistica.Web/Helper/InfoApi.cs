using System;
using System.Configuration;

namespace ICVNL_SistemaLogistica.Web.Helper
{
    public static class InfoApi
    {
        public static String GetURLApi()
        {
            return ConfigurationManager.AppSettings["URL_BaseApi"].ToString();
        }
        public static String GetURL_InventariosApi()
        {
            return ConfigurationManager.AppSettings["URL_InventariosApi"].ToString();
        }
    }
}
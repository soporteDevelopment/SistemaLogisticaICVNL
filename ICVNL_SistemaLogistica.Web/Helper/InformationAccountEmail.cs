using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Configuration;

namespace ICVNL_SistemaLogistica.Web.Helper
{
    public static class InformationAccountEmail
    {
        public static InfoCorreo GetInfoAccountEmail()
        {
            return new InfoCorreo()
            {
                MailerName = ConfigurationManager.AppSettings["MailerName"].ToString(),
                ServidorSMTP = ConfigurationManager.AppSettings["ServidorSMTP"].ToString(),
                PuertoSMTP = Convert.ToInt32(ConfigurationManager.AppSettings["PuertoSMTP"].ToString()),
                UsarSSL = ConfigurationManager.AppSettings["UsaSSL"].ToString().ToUpper() == "TRUE",
                SmtpTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpTimeout"].ToString()),
                InformadorEmail = ConfigurationManager.AppSettings["InformadorEmail"].ToString(),
                InformadorPassword = ConfigurationManager.AppSettings["InformadorPassword"].ToString()
            };
        }
    }
}
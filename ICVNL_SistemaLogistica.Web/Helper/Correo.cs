using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ICVNL_SistemaLogistica.Web.Helper
{
    using System;
    using System.Net.Mail;
    using System.Collections.Generic;
    using System.Text.RegularExpressions; 
    using System.Net;
    using ICVNL_SistemaLogistica.Web.Entities;
    using ICVNL_SistemaLogistica.Web.Helper;

    /// <summary>
    /// Funciones para el envío de correos electrónicos
    /// </summary>
    public static class Correo
    {
        public static DBResponse<Boolean> EnviaCorreo(EnvioEmail envioEmail, InfoCorreo infoCorreo)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string mailerNombre = infoCorreo.MailerName;
                string mailerEmail = infoCorreo.InformadorEmail;
                string mailerPassword = infoCorreo.InformadorPassword;

                string servidorSMTP = infoCorreo.ServidorSMTP;
                int puertoSMTP = infoCorreo.PuertoSMTP;
                int timeOut = infoCorreo.SmtpTimeout;
                bool usarSSL = infoCorreo.UsarSSL;

                //create the mail message
                MailMessage mail = new MailMessage();

                //Destinatarios
                List<MailAddress> destinatarios = new List<MailAddress>();
                var emails = envioEmail.EmailEnvia.Split(';');
                foreach (string em in emails)
                {
                    destinatarios.Add(new MailAddress(em));
                }
                List<MailAddress> destinatariosConCopia = new List<MailAddress>();
                if (envioEmail.EmailConCopiaEnvia != null)
                {
                    if (envioEmail.EmailConCopiaEnvia.Length > 0)
                    {
                        var emailsConCopia = envioEmail.EmailConCopiaEnvia.Split(';');
                        foreach (string em in emailsConCopia)
                        {
                            destinatariosConCopia.Add(new MailAddress(em));
                        }
                    }
                }
                //set the addresses
                try
                {
                    mail.From = new MailAddress(mailerEmail, mailerNombre);
                }
                catch (Exception exc)
                {
                    dbResponse.Message = "Función EnviaCorreo: Sección mail.From | " + exc.Message; ;
                    dbResponse.ExecutionOK = false;
                    return dbResponse;
                }

                try
                {
                    foreach (MailAddress direccion in destinatarios)
                    {
                        mail.To.Add(direccion);
                    }
                }
                catch (Exception exc)
                {
                    dbResponse.Message = "Función EnviaCorreo: Sección mail.To.Add | " + exc.Message; ;
                    dbResponse.Data = false;
                    dbResponse.ExecutionOK = false;
                    return dbResponse;
                }

                if (envioEmail.ArchivosAdjuntos != null)
                {
                    foreach (var fileLocation in envioEmail.ArchivosAdjuntos)
                    {
                        Attachment att = new Attachment(fileLocation.RutaArchivo)
                        {
                            Name = System.IO.Path.GetFileName(fileLocation.RutaArchivo)
                        };
                        mail.Attachments.Add(att);
                    }
                }

                //set the content
                mail.Subject = envioEmail.SubjectEmail;
                mail.IsBodyHtml = true;
                mail.Body = envioEmail.BodyEmail;
                if (envioEmail.DeliveryRecipient)
                {
                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    mail.Headers.Add("Return-Receipt-To", infoCorreo.MailerName);
                }
                if (envioEmail.ReadRecipient)
                {
                    mail.Headers.Add("Disposition-Notification-To", infoCorreo.MailerName);
                }

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                if (destinatariosConCopia.Count > 0)
                {
                    foreach (MailAddress direccion in destinatariosConCopia)
                    {
                        mail.CC.Add(direccion);
                    }
                }
                try
                {
                    var smptClient = new SmtpClient(servidorSMTP, puertoSMTP)
                    {
                        Credentials = new System.Net.NetworkCredential(mailerEmail, mailerPassword),
                        EnableSsl = usarSSL,
                        Timeout = timeOut
                    };

                    smptClient.Send(mail);

                    dbResponse.Message = "Email enviado";
                    dbResponse.ExecutionOK = true;
                    dbResponse.Data = true;
                }
                catch (Exception exc)
                {
                    dbResponse.Data = false;
                    dbResponse.Message = "Función EnviaCorreo: Sección smtp.Send | " + exc.Message + "<br /> " + (exc.InnerException != null ? exc.InnerException.Message : "" + "  "); ;
                    dbResponse.ExecutionOK = false;
                    return dbResponse;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = ex.ToString();
                dbResponse.Data = false;
                dbResponse.ExecutionOK = false;
            }
            return dbResponse;
        }

        
        /// <summary>
        /// Indica si una cadena representa una dirección de correo electrónico válida
        /// </summary>
        /// <param name="cadena">La cadena a investigar</param>
        /// <returns>Verdadero si el email tiene un formato correcto.</returns>
        public static bool EsEmailValido(string cadena)
        {
            bool esValido;
            string patron = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            Regex expressionEmail = new Regex(patron);
            esValido = expressionEmail.IsMatch(cadena);
            return esValido;
        }
    }
}
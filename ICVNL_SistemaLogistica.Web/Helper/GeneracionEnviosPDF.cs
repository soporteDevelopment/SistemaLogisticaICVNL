using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Helpers;
using ICVNL_SistemaLogistica.Web.Helper;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ICVNL_SistemaLogistica.Web.Helper
{
    public static class GeneracionEnviosPDF
    {
        public static DBResponse<byte[]> SolicitudPlacasGeneraPDF(SolicitudesPlacas solicitudesPlacas, string PathDirectory, Boolean enviaMailProveedor)
        {
            var dbResponse = new DBResponse<byte[]>();

            string rutaArchivoXLS = PathDirectory + "\\Plantillas\\SolicitudPlacas.xls";
            string rutaGenerada = PathDirectory + "Generados\\";

            if (!Directory.Exists(rutaGenerada))
                Directory.CreateDirectory(rutaGenerada);

            Workbook workbook = new Workbook();
            workbook.LoadFromFile(rutaArchivoXLS, true);
            Worksheet sheet = workbook.Worksheets[0];

            sheet.Range["C2"].Text = solicitudesPlacas.FolioSolicitud;
            sheet.Range["C3"].Text = solicitudesPlacas.Proveedores.NombreProveedor;
            sheet.Range["C4"].Text = solicitudesPlacas.Contratos.NumeroContrato;
            sheet.Range["C5"].Text = solicitudesPlacas.OrdenesCompra.NumeroOrdenCompra;
            sheet.Range["C6"].Text = solicitudesPlacas.FechaSolicitud.ToString("dd/MM/yyyy");
            sheet.Range["C7"].Text = solicitudesPlacas.FechaEntrega.ToString("dd/MM/yyyy");

            int rowInicial = 11;

            foreach (var item in solicitudesPlacas.SolicitudesPlacas_Detalle)
            {
                sheet.Range["B" + rowInicial.ToString()].Text = item.DelegacionesBancos.NombreDelegacionBanco.ToString();
                sheet.Range["C" + rowInicial.ToString()].Text = item.TiposPlacas.TipoPlaca.ToString();
                sheet.Range["E" + rowInicial.ToString()].Text = item.RangoPlacaInicial.ToString();
                sheet.Range["G" + rowInicial.ToString()].Text = item.RangoPlacaFinal.ToString();
                sheet.Range["I" + rowInicial.ToString()].Text = item.CantidadPlacas.ToString();
                rowInicial++;
            }

            var nuevoArchivo = rutaGenerada + String.Format("SolicitudPlacas_{0}.pdf", solicitudesPlacas.FolioSolicitud.Trim());
            workbook.SaveToFile(nuevoArchivo, FileFormat.PDF);
            workbook.Dispose();
            sheet.Dispose();

            if (enviaMailProveedor)
            {
                var infoEmailServer = InformationAccountEmail.GetInfoAccountEmail();
                var envioMail = new EnvioEmail()
                {
                    SubjectEmail = "Solicitud de Placas de Proveedor",
                    BodyEmail = "",
                    DeliveryRecipient = true,
                    ReadRecipient = true,
                    EmailEnvia = solicitudesPlacas.Proveedores.EmailProveedor,
                    ArchivosAdjuntos = new List<ArchivosAdjuntos>()
                    {
                        new ArchivosAdjuntos()
                        {
                            RutaArchivo = nuevoArchivo
                        }
                    }
                };

                var enviaMail = Correo.EnviaCorreo(envioMail, infoEmailServer);
            }

            using (var exportData = new MemoryStream())
            {
                using (FileStream FreadStream = new FileStream(nuevoArchivo, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[FreadStream.Length];
                    FreadStream.Read(bytes, 0, (int)FreadStream.Length);

                    FreadStream.Seek(0, SeekOrigin.Begin);
                    exportData.Write(bytes, 0, (int)FreadStream.Length);
                }
                dbResponse.Data = exportData.GetBuffer();
            }

            //Eliminamos el archivo de apoyo
            try
            {
                File.Delete(nuevoArchivo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            return dbResponse; 
        }

        public static DBResponse<byte[]> TransferenciaPlacasPDF(TransferenciaPlacas transferenciaPlacas, string PathDirectory)
        {
            var dbResponse = new DBResponse<byte[]>();
            List<string> documentosCreados = new List<string>();
            var nuevoArchivo = "";
            string rutaArchivoXLS = PathDirectory + "\\Plantillas\\TransferenciaPlacas.xls";
            string rutaGenerada = PathDirectory + "Generados\\";
            try
            {
                if (!Directory.Exists(rutaGenerada))
                    Directory.CreateDirectory(rutaGenerada);

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(rutaArchivoXLS, true);
                Worksheet sheet = workbook.Worksheets[0];

                sheet.Range["H4"].Text = transferenciaPlacas.FolioTransferencia;
                sheet.Range["AA4"].Text = transferenciaPlacas.FechaHoraRegistro.ToString("dd/MM/yyyy hh:mm tt");

                sheet.Range["E7"].Text = transferenciaPlacas.TransferenciaPlacas_DatosPersona.Nombre;
                sheet.Range["E8"].Text = transferenciaPlacas.TransferenciaPlacas_DatosPersona.Apellido;
                sheet.Range["E9"].Text = transferenciaPlacas.TransferenciaPlacas_DatosPersona.TiposID.TipoID;
                sheet.Range["E10"].Text = transferenciaPlacas.TransferenciaPlacas_DatosPersona.NumeroID;

                sheet.Range["Z7"].Text = transferenciaPlacas.TransferenciaPlacas_Transporte.MarcaVehiculo;
                sheet.Range["Z8"].Text = transferenciaPlacas.TransferenciaPlacas_Transporte.ModeloVehiculo;
                sheet.Range["Z9"].Text = transferenciaPlacas.TransferenciaPlacas_Transporte.PlacasVehiculo;
                sheet.Range["Z10"].Text = transferenciaPlacas.TransferenciaPlacas_Transporte.NumeroEconomico;

                sheet.Range["E12"].Text = transferenciaPlacas.DelegacionesBancosOrigen.NombreDelegacionBanco;
                sheet.Range["Y12"].Text = transferenciaPlacas.DelegacionesBancosDestino.NombreDelegacionBanco;
                int rowInicial = 16;
                int rowList1 = 5;
                int rowList2 = 5;  

                foreach (var item in transferenciaPlacas.TransferenciaPlacas_Listado1.Where(x => x.CantidadDisponiblesSerTransferidas > 0))
                {
                    sheet.Range["B" + rowInicial.ToString()].Text = item.TiposPlacas.TipoPlaca.ToString();
                    sheet.Range["F" + rowInicial.ToString()].Text = item.CantidadDisponiblesSerTransferidas.ToString();
                    rowInicial++;
                    sheet.InsertRow(rowInicial); 
                    sheet.Range["B" + rowInicial.ToString() + ":E" + rowInicial.ToString()].Merge();
                    sheet.Range["F" + rowInicial.ToString() + ":Q" + rowInicial.ToString()].Merge();
                    
                }

                rowInicial += rowList1;
                foreach (var item in transferenciaPlacas.TransferenciaPlacas_Listado2.Where(x=>x.Automatico == true))
                {
                    sheet.Range["B" + rowInicial.ToString()].Text = item.TiposPlacas.TipoPlaca.ToString();
                    sheet.Range["F" + rowInicial.ToString()].Text = item.NumeroPlaca;
                    sheet.Range["R" + rowInicial.ToString()].Text = item.TiposEstatusPlacas.TipoEstatusPlacas;
                    rowInicial++;
                    sheet.InsertRow(rowInicial); 
                    sheet.Range["B" + rowInicial.ToString() + ":E" + rowInicial.ToString()].Merge();
                    sheet.Range["F" + rowInicial.ToString() + ":Q" + rowInicial.ToString()].Merge();
                    sheet.Range["R" + rowInicial.ToString() + ":AB" + rowInicial.ToString()].Merge();
                    
                }

                rowInicial += rowList2;
                foreach (var item in transferenciaPlacas.TransferenciaPlacas_Listado2.Where(x => x.Automatico == false))
                {
                    sheet.Range["B" + rowInicial.ToString()].Text = item.TiposPlacas.TipoPlaca.ToString();
                    sheet.Range["F" + rowInicial.ToString()].Text = item.NumeroPlaca;
                    sheet.Range["R" + rowInicial.ToString()].Text = item.TiposEstatusPlacas.TipoEstatusPlacas;
                    rowInicial++;
                    sheet.InsertRow(rowInicial); 
                        sheet.Range["B" + rowInicial.ToString() + ":E" + rowInicial.ToString()].Merge();
                        sheet.Range["F" + rowInicial.ToString() + ":Q" + rowInicial.ToString()].Merge();
                        sheet.Range["R" + rowInicial.ToString() + ":AB" + rowInicial.ToString()].Merge();                 
                }

                nuevoArchivo = rutaGenerada + String.Format("SolicitudPlacas_{0}.pdf", transferenciaPlacas.FolioTransferencia.Trim());
                workbook.SaveToFile(nuevoArchivo, FileFormat.PDF);
                workbook.Dispose();
                sheet.Dispose();


                using (var exportData = new MemoryStream())
                {
                    using (FileStream FreadStream = new FileStream(nuevoArchivo, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[FreadStream.Length];
                        FreadStream.Read(bytes, 0, (int)FreadStream.Length);

                        FreadStream.Seek(0, SeekOrigin.Begin);
                        exportData.Write(bytes, 0, (int)FreadStream.Length);
                    }
                    dbResponse.Data = exportData.GetBuffer();
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "Ocurrio un error al generar el PDF de la transferencia de placas " + ex.Message;
            }

            //Eliminamos el archivo de apoyo
            try
            {
                File.Delete(nuevoArchivo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            return dbResponse;
        }

        public static DBResponse<byte[]> RecepcionPlacasEventos(PlacasRecibir_DocumentoGeneradoEvento eventosPlacas, string PathDirectory)
        {
            var dbResponse = new DBResponse<byte[]>();
            List<string> documentosCreados = new List<string>();
            var NombreFinalArchivo = "";
            var nuevoArchivo = "";
            string rutaArchivoXLS = PathDirectory + "\\Plantillas\\RecepcionPlacasEventos.xls";
            string rutaGenerada = PathDirectory + "Generados\\";
            try
            {
                if (!Directory.Exists(rutaGenerada))
                    Directory.CreateDirectory(rutaGenerada);

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(rutaArchivoXLS, true);
                Worksheet sheet = workbook.Worksheets[0];

                var contentCell = sheet.Range["B14"].Text;

                var newContent = contentCell.Replace("[Hora]", eventosPlacas.Hora)
                                            .Replace("[Fecha]", eventosPlacas.Fecha)
                                            .Replace("[NumEmpleado]", eventosPlacas.NumEmpleado)
                                            .Replace("[DelegacionBanco]", eventosPlacas.DelegacionBanco)
                                            .Replace("[FolioNotaEntrada]", eventosPlacas.FolioNotaEntrada)
                                            .Replace("[NombreProveedor]", eventosPlacas.NombreProveedor)
                                            .Replace("[FolioContrato]", eventosPlacas.FolioContrato)
                                            .Replace("[PartidaContrato]", eventosPlacas.PartidaContrato)
                                            .Replace("[TipoProblema]", eventosPlacas.TipoProblema)
                                            .Replace("[NumeroCaja]", eventosPlacas.NumeroCaja)
                                            .Replace("[TipoPlaca]", eventosPlacas.TipoPlaca);

                sheet.Range["B14"].Text = newContent;
                sheet.Range["B14"].Style.HorizontalAlignment = HorizontalAlignType.Justify;
                sheet.Range["B14"].Style.VerticalAlignment = VerticalAlignType.Justify; 
                sheet.Range["B14"].Style.WrapText = true;
                sheet.Range["B14"].Style.Font.FontName = "Arial";
                sheet.Range["B14"].Style.Font.Size = 14;


                NombreFinalArchivo = String.Format("RecepcionPlacasEventos_{0}.pdf", DateTime.Now.ToString("ddMMyyyyhhmmss"));
                nuevoArchivo = rutaGenerada + NombreFinalArchivo;
                workbook.SaveToFile(nuevoArchivo, FileFormat.PDF);
                workbook.Dispose();
                sheet.Dispose();


                using (var exportData = new MemoryStream())
                {
                    using (FileStream FreadStream = new FileStream(nuevoArchivo, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[FreadStream.Length];
                        FreadStream.Read(bytes, 0, (int)FreadStream.Length);

                        FreadStream.Seek(0, SeekOrigin.Begin);
                        exportData.Write(bytes, 0, (int)FreadStream.Length);
                    }
                    dbResponse.Data = exportData.GetBuffer();
                    dbResponse.Message = NombreFinalArchivo;
                    dbResponse.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "Ocurrio un error al generar el PDF de los eventos surgidos en recepción de placas" + ex.Message;
            }

            //Eliminamos el archivo de apoyo
            try
            {
                File.Delete(nuevoArchivo);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }


            return dbResponse;
        }
    }
}
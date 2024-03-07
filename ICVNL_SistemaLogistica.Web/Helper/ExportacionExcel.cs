using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace ICVNL_SistemaLogistica.Web.Helper
{
    public static class ExportacionExcel
    {
        public static DBResponse<byte[]> ExportarInventarioExcel(List<Listado_InventarioPlacasModel> _InventarioPlacasModels, string PathArchivo)
        {
            var dbResponse = new DBResponse<byte[]>();
            var memoryStream = new MemoryStream();

            using (var fs = new FileStream(PathArchivo, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                IFont boldFont = workbook.CreateFont();
                boldFont.IsBold = true;
                ICellStyle boldStyle = workbook.CreateCellStyle();
                boldStyle.SetFont(boldFont);

                boldStyle.BorderBottom = BorderStyle.Medium;
                boldStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                boldStyle.FillPattern = FillPattern.SolidForeground;

                foreach (var itemInventario in _InventarioPlacasModels)
                {
                    ISheet excelSheet = workbook.CreateSheet(itemInventario.DelegacionesBancos.NombreDelegacionBanco);
                    IRow row = excelSheet.CreateRow(0);

                    List<String> columns = new List<string>();
                    int columnIndex = 0;

                    var dtEncabezado = GetInfoInventarioEncabezado(itemInventario);
                    if (dtEncabezado.ExecutionOK)
                    {
                        foreach (System.Data.DataColumn column in dtEncabezado.Data.Columns)
                        {
                            columns.Add(column.ColumnName);
                            row.CreateCell(columnIndex).SetCellValue(column.ColumnName); 
                            row.GetCell(columnIndex).CellStyle = boldStyle;
                            columnIndex++;
                            excelSheet.AutoSizeColumn(column.Ordinal);
                        }

                        int rowIndex = 1;
                        foreach (DataRow dsrow in dtEncabezado.Data.Rows)
                        {
                            row = excelSheet.CreateRow(rowIndex);
                            int cellIndex = 0;
                            foreach (String col in columns)
                            {
                                row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                                cellIndex++;
                            }

                            rowIndex++;
                        }
                        rowIndex += 1;
                        row = excelSheet.CreateRow(rowIndex);
                        rowIndex += 1;
                        row = excelSheet.CreateRow(rowIndex);
                        rowIndex += 1;
                        row = excelSheet.CreateRow(rowIndex);

                        var dtTotalesExistencia = GetInfoInventarioTipoPlacaExistencias(itemInventario.InventarioPlacas_TotalesExistencia);
                        if (dtTotalesExistencia.ExecutionOK)
                        {
                            columns.Clear();
                            columnIndex = 0;
                            foreach (System.Data.DataColumn column in dtTotalesExistencia.Data.Columns)
                            {
                                columns.Add(column.ColumnName);
                                row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                                row.GetCell(columnIndex).CellStyle = boldStyle;

                                columnIndex++;
                                excelSheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex += 1;
                            foreach (DataRow dsrow in dtTotalesExistencia.Data.Rows)
                            {
                                row = excelSheet.CreateRow(rowIndex);
                                int cellIndex = 0;
                                foreach (String col in columns)
                                {
                                    row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                                    cellIndex++;
                                }

                                rowIndex++;
                            }

                        }
                        rowIndex += 1;
                        row = excelSheet.CreateRow(rowIndex);
                        rowIndex += 1;
                        row = excelSheet.CreateRow(rowIndex); 

                        var dtTotalesDetalle = GetInfoInventarioTipoPlacaExistencias_Detalle(itemInventario.InventarioPlacas_Detalle);
                        if (dtTotalesDetalle.ExecutionOK)
                        {
                            columns.Clear();
                            columnIndex = 0;
                            foreach (System.Data.DataColumn column in dtTotalesDetalle.Data.Columns)
                            {
                                columns.Add(column.ColumnName);
                                row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                                row.GetCell(columnIndex).CellStyle = boldStyle;

                                columnIndex++;
                                excelSheet.AutoSizeColumn(column.Ordinal);
                            }

                            rowIndex += 1;
                            foreach (DataRow dsrow in dtTotalesDetalle.Data.Rows)
                            {
                                row = excelSheet.CreateRow(rowIndex);
                                int cellIndex = 0;
                                foreach (String col in columns)
                                {
                                    row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                                    cellIndex++;
                                }

                                rowIndex++;
                            }

                        }
                    }
                }

                workbook.Write(fs);
            }


            //write excel into memory stream
            using (var exportData = new MemoryStream())
            {
                using (FileStream FreadStream = new FileStream(PathArchivo, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[FreadStream.Length];
                    FreadStream.Read(bytes, 0, (int)FreadStream.Length);

                    FreadStream.Seek(0, SeekOrigin.Begin);
                    exportData.Write(bytes, 0, (int)FreadStream.Length);
                }

                //Eliminamos el archivo de apoyo
                try
                {
                    File.Delete(PathArchivo);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }

                dbResponse.Data = exportData.GetBuffer();
            }

            return dbResponse;
        }


        public static DBResponse<DataTable> GetInfoInventarioEncabezado(Listado_InventarioPlacasModel _InventarioPlacasModel)
        {
            var dbResponse = new DBResponse<DataTable>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Almacén General/Delegaciones/Bancos");
            dt.Columns.Add("Fecha del Inventario");
            dt.Columns.Add("Total de Placas");

            DataRow dr;
            dr = dt.NewRow();

            dr[0] = _InventarioPlacasModel.DelegacionesBancos.NombreDelegacionBanco;
            dr[1] = _InventarioPlacasModel.FechaInventario.ToString("dd/MM/yyyy");
            dr[2] = _InventarioPlacasModel.InventarioPlacas_TotalesExistencia.Sum(x => x.Existencia);
            dt.Rows.Add(dr);


            dbResponse.Data = dt;
            dbResponse.ExecutionOK = true;
            return dbResponse;
        }

        public static DBResponse<DataTable> GetInfoInventarioTipoPlacaExistencias(List<Listado_InventarioPlacas_TotalesExistenciaModel> _TotalesExistencia)
        {
            var dbResponse = new DBResponse<DataTable>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Tipo de Placa");
            dt.Columns.Add("Existencias");

            DataRow dr;
            foreach (var item in _TotalesExistencia)
            {
                dr = dt.NewRow();

                dr[0] = item.TiposPlacas.TipoPlaca;
                dr[1] = item.Existencia;
                dt.Rows.Add(dr);
            }

            dbResponse.Data = dt;
            dbResponse.ExecutionOK = true;
            return dbResponse;
        }

        public static DBResponse<DataTable> GetInfoInventarioTipoPlacaExistencias_Detalle(List<Listado_InventarioPlacas_DetalleModel> inventarioPlacasDetalle)
        {
            var dbResponse = new DBResponse<DataTable>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Tipo de Placa");
            dt.Columns.Add("Estatus");
            dt.Columns.Add("Serie");
            dt.Columns.Add("Existencia desde");
            dt.Columns.Add("Existencias hasta");
            dt.Columns.Add("Total");

            DataRow dr;
            foreach (var item in inventarioPlacasDetalle)
            {
                dr = dt.NewRow();

                dr[0] = item.TiposPlacas.TipoPlaca;
                dr[1] = item.EstatusPlacas.TipoEstatusPlacas;
                dr[2] = item.Serie;
                dr[3] = item.ExistenciaDesde;
                dr[4] = item.ExistenciaHasta;
                dr[5] = item.ExistenciaTotalRango;
                dt.Rows.Add(dr);
            }

            dbResponse.Data = dt;
            dbResponse.ExecutionOK = true;
            return dbResponse;
        }
    }
}
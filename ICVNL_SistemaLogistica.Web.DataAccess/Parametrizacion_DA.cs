using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Net.Transactions;
using Framework.Net.Data;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class Parametrizacion_DA
    {
        public DBResponse<Parametrizacion> GetParametrizacion()
        {
            var responseDB = new DBResponse<Parametrizacion>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Parametrizacion();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_parametrizacion.consulta_parametrizacion", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    DateTime? dateNull = null;
                    var objParametrizacion = new Parametrizacion()
                    {
                        IdParametrizacion = Datos.Int(row, "PARN_ID"),
                        EmailDestinatariosSolicitudPlacas = Datos.Str(row, "PARC_EMAIL_ENV_SP"),
                        EmailDestinatariosNotificaPlacasVendidas = Datos.Str(row, "PARC_EMAIL_NOT_PL_V"),
                        ClaveEntidadGobierno = Datos.Str(row, "PARC_CVE_ENT_GOB"),
                        CentroCostosEntidadGobiernoPlacaReporte = Datos.Str(row, "PARC_CTACOST_VTA_PL_RPT"),
                        CuentaCostosVentaPlacaReporte = Datos.Str(row, "PARC_ENT_VTA_PL_RPT"),
                        CentroCostosEntidadGobiernoPlacaVendida = Datos.Str(row, "PARC_CTACOST_ENT_GOB"),
                        CuentaCostosVentaPlacaVendida = Datos.Str(row, "PARC_CVE_ENT_GOB_VEN"),
                        FechaUltimaEjecucionNE = Datos.Str(row, "PARF_ULT_EJEC_NE") == "" ? dateNull : DateTime.Parse(Datos.Str(row, "PARF_ULT_EJEC_NE")),
                        TipoPolizaPlacaReporte = Datos.Int(row, "PARN_TP_PL_RPT"),
                        TipoPolizaPlacaVendida = Datos.Int(row, "PARN_TP_PL_VEN"),
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Data = objParametrizacion;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Parametrizacion();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<DBNull> UpsertParametrizacion(Parametrizacion parametrizacion, Boolean nRow)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                if (nRow)
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        Db.CreateParameter("p_PARN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.IdParametrizacion),
                        Db.CreateParameter("p_PARC_EMAIL_NOT_PL_V", DbType.String, 1000, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.EmailDestinatariosNotificaPlacasVendidas),
                        Db.CreateParameter("p_PARC_CTACOST_ENT_GOB", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.CentroCostosEntidadGobiernoPlacaVendida),
                        Db.CreateParameter("p_PARC_ENT_VTA_PL_RPT", DbType.String, 100, ParameterDirection.InputOutput, false, null, DataRowVersion.Default, parametrizacion.CuentaCostosVentaPlacaReporte),
                        Db.CreateParameter("p_PARC_CTACOST_VTA_PL_RPT", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.CentroCostosEntidadGobiernoPlacaReporte),
                        Db.CreateParameter("p_PARC_CVE_ENT_GOB", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.ClaveEntidadGobierno),
                        Db.CreateParameter("p_PARC_EMAIL_ENV_SP", DbType.String, 1000, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.EmailDestinatariosSolicitudPlacas),
                        Db.CreateParameter("p_PARC_CVE_ENT_GOB_VEN", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.CuentaCostosVentaPlacaVendida),
                        Db.CreateParameter("p_PARN_TP_PL_VEN", DbType.Int32, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.TipoPolizaPlacaVendida),
                        Db.CreateParameter("p_PARN_TP_PL_RPT", DbType.Int32, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.TipoPolizaPlacaReporte),
                        Db.CreateParameter("p_PARN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                        Db.CreateParameter("p_PARN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.Entidad),
                        Db.CreateParameter("p_PARF_ULT_EJEC_NE", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.FechaUltimaEjecucionNE),
                        
                    };
                    Db.Insert("spcpl_parametrizacion_op.agrega_param", CommandType.StoredProcedure, list);
                }
                else
                {
                    IList<Parameter> list = new List<Parameter>
                    {
                        Db.CreateParameter("p_PARN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.IdParametrizacion),
                        Db.CreateParameter("p_PARC_EMAIL_NOT_PL_V", DbType.String, 1000, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.EmailDestinatariosNotificaPlacasVendidas),
                        Db.CreateParameter("p_PARC_CTACOST_ENT_GOB", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.CentroCostosEntidadGobiernoPlacaVendida),
                        Db.CreateParameter("p_PARC_ENT_VTA_PL_RPT", DbType.String, 100, ParameterDirection.InputOutput, false, null, DataRowVersion.Default, parametrizacion.CuentaCostosVentaPlacaReporte),
                        Db.CreateParameter("p_PARC_CTACOST_VTA_PL_RPT", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.CentroCostosEntidadGobiernoPlacaReporte),
                        Db.CreateParameter("p_PARC_CVE_ENT_GOB", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.ClaveEntidadGobierno),
                        Db.CreateParameter("p_PARC_EMAIL_ENV_SP", DbType.String, 1000, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.EmailDestinatariosSolicitudPlacas),
                        Db.CreateParameter("p_PARC_CVE_ENT_GOB_VEN", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.CuentaCostosVentaPlacaVendida),
                        Db.CreateParameter("p_PARN_TP_PL_VEN", DbType.Int32, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.TipoPolizaPlacaVendida),
                        Db.CreateParameter("p_PARN_TP_PL_RPT", DbType.Int32, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.TipoPolizaPlacaReporte),
                        Db.CreateParameter("p_PARN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                        Db.CreateParameter("p_PARN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, parametrizacion.Entidad)
                    };
                    Db.Insert("spcpl_parametrizacion_op.modifica_param", CommandType.StoredProcedure, list);
                }

                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se " + (nRow ? "Agrego" : "modificó") + " correctamente la Parametrización";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
    }
}

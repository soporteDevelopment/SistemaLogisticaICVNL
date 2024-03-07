using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class Polizas_DA
    {
        public DBResponse<List<Placas_Polizas>> GetPoliza_PlacasVendidasNoContabilizadas(int Entidad)
        {
            var responseDB = new DBResponse<List<Placas_Polizas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Placas_Polizas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RNO_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_polizas.con_placas_ven_no_cont", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var placasPoliza = new Placas_Polizas()
                        {
                            CentroCostosAlmacen = Datos.Str(row, "OCDC_CEN_COSTOS_ALM"),
                            ImportePlacas = Datos.Dec(row, "S_INVDN_COSTOPLACA"), 
                            NumeroPlaca = Datos.Str(row, "INVDC_NUMEROPLACA"),
                            CuentaContableCargo = Datos.Str(row, "NEDN_CTA_CON_CARGO")
                        };
                        responseDB.Data.Add(placasPoliza);
                    }

                    if (responseDB.Data.Count > 0)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = responseDB.Data.Count;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<Placas_Polizas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<Placas_Polizas>> GetPoliza_PlacasNoContabilizadas(int Entidad)
        {
            var responseDB = new DBResponse<List<Placas_Polizas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Placas_Polizas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RNO_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_polizas.consulta_placas_no_cont", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var placasPoliza = new Placas_Polizas()
                        {
                            ImportePlacas = Datos.Dec(row, "S_INVDN_COSTOPLACA"),
                            CentroCostosAlmacen = Datos.Str(row, "OCDC_CEN_COSTOS_ALM"),
                            NumeroPlaca = Datos.Str(row, "INVDC_NUMEROPLACA"),
                            CuentaContableCargo = Datos.Str(row, "NEDN_CTA_CON_CARGO")
                        };
                        responseDB.Data.Add(placasPoliza);
                    }

                    if (responseDB.Data.Count > 0)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = responseDB.Data.Count;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<Placas_Polizas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

    }
}

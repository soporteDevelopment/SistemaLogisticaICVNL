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
    public class Bitacora_DA
    {
        public DBResponse<List<BitacoraEventos>> GetBitacoraEventos_List (DateTime FechaIni,
                                                                          DateTime FechaFin,
                                                                          string LugarEvento,
                                                                          string Evento,
                                                                          string Usuario, 
                                                                          string InstruccionRealizada,
                                                                          string IP_User,  
                                                                          int Entidad)
        {
            var responseDB = new DBResponse<List<BitacoraEventos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<BitacoraEventos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                { 
                    Db.CreateParameter("p_BITF_EVENTO_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaIni),
                    Db.CreateParameter("p_BITF_EVENTO_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaFin), 
                    Db.CreateParameter("p_BITC_LUGAREVENTO", DbType.String, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, LugarEvento),
                    Db.CreateParameter("p_BITC_EVENTO", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, Evento),  
                    Db.CreateParameter("p_BITC_USR", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, Usuario),
                    Db.CreateParameter("p_BITC_INSTR_REAL", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, InstruccionRealizada),
                    Db.CreateParameter("p_BITC_IP_USR", DbType.String, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, IP_User),
                    Db.CreateParameter("p_BITN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_bitacora.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var bitacoraEventos = new BitacoraEventos()
                        {
                            IdBitacora = Datos.Int(row, "BITN_ID"),
                            FechaEvento = Convert.ToDateTime(Datos.Str(row, "BITF_EVENTO")), 
                            LugarEvento = Datos.Str(row, "BITC_LUGAREVENTO"),
                            Evento = Datos.Str(row, "BITC_EVENTO"),
                            Usuario = Datos.Str(row, "BITC_USR"),
                            InstruccionRealizada = Datos.Str(row, "BITC_INSTR_REAL"),
                            IP_Usuario = Datos.Str(row, "BITC_IP_USR"),
                            JsonObject = Datos.Str(row, "BITC_JSONOBJECT"),
                            Entidad = Datos.Int(row, "BITN_ENTIDAD")
                        };
                        responseDB.Data.Add(bitacoraEventos);
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
                responseDB.Data = new List<BitacoraEventos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<DBNull> InsertBitacora(BitacoraEventos bitacora)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            { 
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_BITC_USR", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.Usuario),
                    Db.CreateParameter("p_BITF_EVENTO", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.FechaEvento),
                    Db.CreateParameter("p_BITC_IP_USR", DbType.String, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.IP_Usuario),
                    Db.CreateParameter("p_BITN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_BITC_LUGAREVENTO", DbType.String, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.LugarEvento),
                    Db.CreateParameter("p_BITN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.Entidad),
                    Db.CreateParameter("p_BITC_JSONOBJECT", DbType.String, 4000, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.JsonObject),
                    Db.CreateParameter("p_BITC_INSTR_REAL", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.InstruccionRealizada),
                    Db.CreateParameter("p_BITC_EVENTO", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, bitacora.Evento)
                };

                Db.Insert("spcpl_bitacora_op.agrega_bit", CommandType.StoredProcedure, list);

                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se registro movimiento en la bitacora";

            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Data = null;
                dbResponse.Message = "Ocurrio un error al insertar en la bitacora: " + ex.Message;
            }
            return dbResponse;
        }
    }
}

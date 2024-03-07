using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class BitacoraEventos_BL
    {
        public DBResponse<List<BitacoraEventos>> GetBitacoraEventos(DateTime FiltroEventoIni_,
                                                                    DateTime FiltroEventoFin_, 
                                                                    string FiltroLugarEvento_,
                                                                    string FiltroEvento_,
                                                                    string FiltroUsuario_,
                                                                    string FiltroInstruccionRealizada_,
                                                                    string FiltroIP_,
                                                                    int Entidad)
        {
            var dbResponse = new DBResponse<List<BitacoraEventos>>();
            try
            {
                var responseData = new Bitacora_DA().GetBitacoraEventos_List(FiltroEventoIni_, FiltroEventoFin_, FiltroLugarEvento_, FiltroEvento_, FiltroUsuario_, FiltroInstruccionRealizada_, FiltroIP_, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = responseData.Data.Count;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new List<BitacoraEventos>();
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No se encontro información en la bitacora";
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<BitacoraEventos>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> InsertBitacora(BitacoraEventos bitacoraEventos)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    try
                    {
                        dbResponse = new Bitacora_DA().InsertBitacora(bitacoraEventos);
                        if (dbResponse.ExecutionOK)
                        {  
                            transaction.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                        dbResponse.Data = null;
                        dbResponse.ExecutionOK = false;
                        dbResponse.NumRows = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
    }
}

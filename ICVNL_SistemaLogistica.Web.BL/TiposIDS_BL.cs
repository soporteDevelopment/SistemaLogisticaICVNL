using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class TiposIDS_BL
    {
        public DBResponse<List<TiposIDs>> GetTiposIDs(int? Estatus, int Entidad)
        {
            var dbResponse = new DBResponse<List<TiposIDs>>();
            try
            {
                var responseData = new TiposIDs_DA().GetTiposIDs_List(Estatus ,Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<TiposIDs>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<TiposIDs> GetTipoID(int Entidad, int id)
        {
            var dbResponse = new DBResponse<TiposIDs>();
            try
            {
                var responseData = new TiposIDs_DA().GetTiposIDs_ById(Entidad, id);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = responseData.ExecutionOK ? 1 : 0;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new TiposIDs();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Boolean> ExisteTipoID(TiposIDs TiposIDs, int Entidad)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                var response = new TiposIDs_DA().ExisteTipoID(Entidad, TiposIDs.TipoID);
                if (response.ExecutionOK)
                {
                    if (TiposIDs.Id != response.Data.Id)
                    {
                        dbResponse.Data = true;
                        dbResponse.ExecutionOK = response.ExecutionOK;
                        dbResponse.Message = response.Message;
                    }
                    else 
                    { 
                        dbResponse.Data = false;
                    }
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = false;
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<string> ValidacionesTipoID(TiposIDs TipoID)
        {
            var dbResponse = new DBResponse<string>();
            try
            {
                var mensaje = "";
                if (TipoID.TipoID.Length == 0)
                {
                    mensaje += "El tipo de ID es obligatorio <br />";
                }
                if (TipoID.TipoID.Length > 200)
                {
                    mensaje += "El tipo de ID no debe tener más de 200 carácteres <br />";
                }

                dbResponse.Data = mensaje;
                dbResponse.ExecutionOK = mensaje.Length > 0;
                dbResponse.Message = "";
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = "";
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<TiposIDs> UpsertTipoID(TiposIDs TiposIDs, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<TiposIDs>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new TiposIDs_DA().UpsertTipoID(TiposIDs, nRow);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            FechaEvento = DateTime.Now,
                            Evento = nRow ? "Inserta" : "Actualiza",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Tipos de IDs",
                            JsonObject = JsonConvert.SerializeObject(TiposIDs),
                            Entidad = usuario.Entidad
                        });
                        dbResponse.Message = response.Message;
                        dbResponse.Data = response.Data;
                        transaction.Complete();
                    }
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                    dbResponse.Data = new TiposIDs();
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Tipos de IDs",
                        JsonObject = JsonConvert.SerializeObject(TiposIDs),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }

        public DBResponse<DBNull> CambioEstatusTipoID(int IdTipoID, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new TiposIDs_DA().CambioEstatusTipoID(IdTipoID);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = "Update",
                            FechaEvento = DateTime.Now,
                            Evento = "Actualiza Estatus",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Tipos de IDs",
                            JsonObject = JsonConvert.SerializeObject(IdTipoID),
                            Entidad = usuario.Entidad
                        });
                        transaction.Complete();
                    }
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Tipos de IDs",
                        JsonObject = JsonConvert.SerializeObject(IdTipoID),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }
    }
}

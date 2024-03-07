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
    public class Estados_BL
    {
        public DBResponse<List<Estados>> GetEstados(int? Estatus, int Entidad)
        {
            var dbResponse = new DBResponse<List<Estados>>();
            try
            {
                var responseData = new Estados_DA().GetEstados_List(Estatus, Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<Estados>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Estados> GetEstado(int Entidad, int id)
        {
            var dbResponse = new DBResponse<Estados>();
            try
            {
                var responseData = new Estados_DA().GetEstados_ById(Entidad, id);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = responseData.ExecutionOK ? 1 : 0;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new Estados();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Boolean> ExisteEstado(Estados Estados, int Entidad)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                var response = new Estados_DA().ExisteEstado(Entidad, Estados.Estado);
                if (response.ExecutionOK)
                {
                    if (Estados.Id != response.Data.Id)
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

        public DBResponse<string> ValidacionesEstado(Estados Estado)
        {
            var dbResponse = new DBResponse<string>();
            try
            {
                var mensaje = "";
                if (Estado.Estado.Length == 0)
                {
                    mensaje += "El Estado es obligatorio <br />";
                }
                if (Estado.Estado.Length > 100)
                {
                    mensaje += "El Estado no debe tener más de 100 carácteres <br />";
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

        public DBResponse<Estados> UpsertEstado(Estados Estados, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<Estados>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {

                    var response = new Estados_DA().UpsertEstado(Estados, nRow);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Estados",
                            JsonObject = JsonConvert.SerializeObject(Estados),
                            Entidad = usuario.Entidad
                        });
                        dbResponse.Message = response.Message;
                        dbResponse.Data = response.Data;
                        transaction.Complete();
                    }
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new Estados();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "Error",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Estados",
                    JsonObject = JsonConvert.SerializeObject(Estados),
                    Entidad = usuario.Entidad
                });
            }

            return dbResponse;
        }


        public DBResponse<DBNull> CambiaEstatusEstado(int IdEstado, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new Estados_DA().CambiaEstatusEstado(IdEstado);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = "Delete",
                            FechaEvento = DateTime.Now,
                            Evento = "Actualiza Estatus",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Estados",
                            JsonObject = JsonConvert.SerializeObject(IdEstado),
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
                        LugarEvento = "Estados",
                        JsonObject = JsonConvert.SerializeObject(IdEstado),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }
    }
}

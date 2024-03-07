using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class DelegacionesBancos_BL
    {
        public DBResponse<List<DelegacionesBancos>> GetDelegacionesBancos(int? Estatus,int Entidad)
        {
            var dbResponse = new DBResponse<List<DelegacionesBancos>>();
            try
            {
                var responseData = new DelegacionesBancos_DA().GetDelegacionesBancos_List(Estatus, Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<DelegacionesBancos>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<DelegacionesBancos> GetDelegacionBanco(int Entidad, int id)
        {
            var dbResponse = new DBResponse<DelegacionesBancos>();
            try
            {
                var responseData = new DelegacionesBancos_DA().GetDelegacionesBancos_ById(Entidad, id);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = responseData.ExecutionOK ? 1 : 0;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new DelegacionesBancos();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Boolean> ExisteDelegacionBanco(DelegacionesBancos delegacionesBancos, int Entidad)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                var response = new DelegacionesBancos_DA().ExisteDelegacionBanco(Entidad, delegacionesBancos.NombreDelegacionBanco);
                if (response.ExecutionOK)
                {
                    if (delegacionesBancos.IdDelegacionBanco != response.Data.IdDelegacionBanco)
                    {
                        dbResponse.Data = true;
                        dbResponse.ExecutionOK = response.ExecutionOK;
                        dbResponse.Message = response.Message;
                        return dbResponse;
                    }
                    else
                    {
                        dbResponse.Data = false;
                    }
                }
                var responseInfofin = new DelegacionesBancos_DA().ExisteDelegacionBancoInfofin(Entidad, delegacionesBancos.CentroCostosInfoFin);
                if (responseInfofin.ExecutionOK)
                {
                    if (delegacionesBancos.IdDelegacionBanco != responseInfofin.Data.IdDelegacionBanco)
                    {
                        dbResponse.Data = true;
                        dbResponse.ExecutionOK = responseInfofin.ExecutionOK;
                        dbResponse.Message += responseInfofin.Message;
                        return dbResponse;
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

        public DBResponse<DelegacionesBancos> ExisteDelegacionBancoByCentroCostos(string CentroCostosInfoFin, int Entidad)
        {
            var dbResponse = new DBResponse<DelegacionesBancos>();
            try
            {
                 var responseInfofin = new DelegacionesBancos_DA().ExisteDelegacionBancoInfofin(Entidad, CentroCostosInfoFin);
                if (responseInfofin.ExecutionOK)
                {
                    dbResponse.Data = responseInfofin.Data;
                    dbResponse.ExecutionOK = responseInfofin.ExecutionOK;
                    dbResponse.Message += responseInfofin.Message;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new DelegacionesBancos();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<string> ValidacionesDelegacionBanco(DelegacionesBancos delegaciones)
        {
            var dbResponse = new DBResponse<string>();
            try
            {
                var mensaje = "";
                if (delegaciones.NombreDelegacionBanco.Length == 0)
                {
                    mensaje += "El nombre de la delegación/banco es obligatorio <br />";
                }
                if (delegaciones.NombreDelegacionBanco.Length > 200)
                {
                    mensaje += "El nombre de la delegación/banco no debe tener más de 200 carácteres <br />";
                }
                if (delegaciones.CentroCostosInfoFin.Length == 0)
                {
                    mensaje += "El código del centro de costos de Infofin es obligatorio <br />";
                }
                if (delegaciones.CentroCostosInfoFin.Length > 100)
                {
                    mensaje += "El código del centro de costos de Infofin no debe tener más de 100 carácteres <br />";
                }

                dbResponse.Message = mensaje;
                dbResponse.ExecutionOK = mensaje.Length > 0;
                dbResponse.Data = "";
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

        public DBResponse<DelegacionesBancos> UpsertDelegacionBanco(DelegacionesBancos delegacionesBancos, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<DelegacionesBancos>();

            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    var response = new DelegacionesBancos_DA().UpsertDelegacionBanco(delegacionesBancos, nRow);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Delegaciones Bancos",
                            JsonObject = JsonConvert.SerializeObject(delegacionesBancos),
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
                dbResponse.Data = new DelegacionesBancos();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "UpsertDelegacionBanco",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Delegación Banco",
                    JsonObject = JsonConvert.SerializeObject(delegacionesBancos),
                    Entidad = usuario.Entidad
                });
            }
            
            return dbResponse;
        }

        public DBResponse<DBNull> CambiaEstatusDelegacionBanco(int IdDelegacionBanco, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new DelegacionesBancos_DA().CambiaEstatusDelegacionBanco(IdDelegacionBanco);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = "Update",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = "Actualiza Estatus",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Delegación Banco",
                            JsonObject = JsonConvert.SerializeObject(IdDelegacionBanco),
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
                        Evento = "Error",
                        FechaEvento = DateTime.Now,
                        InstruccionRealizada = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Delegación Banco",
                        JsonObject = JsonConvert.SerializeObject(IdDelegacionBanco),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }
    }
}

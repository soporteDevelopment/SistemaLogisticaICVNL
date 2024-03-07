using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class TiposPlacas_BL
    {
        public DBResponse<List<TiposPlacas>> GetTiposPlacas(int? Estatus, int Entidad)
        {
            var dbResponse = new DBResponse<List<TiposPlacas>>();
            try
            {
                var responseData = new TiposPlacas_DA().GetTiposPlacas_List(Estatus, Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<TiposPlacas>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<TiposPlacas> GetTipoPlaca(int Entidad, int id)
        {
            var dbResponse = new DBResponse<TiposPlacas>();
            try
            {
                var responseData = new TiposPlacas_DA().GetTiposPlacas_ById(Entidad, id);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = responseData.ExecutionOK ? 1 : 0;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new TiposPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Boolean> ExisteTipoPlaca(TiposPlacas TiposPlacas, int Entidad)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                var response = new TiposPlacas_DA().ExisteTipoPlaca(Entidad, TiposPlacas.TipoPlaca);
                if (response.ExecutionOK)
                {
                    if (TiposPlacas.IdTipoPlaca != response.Data.IdTipoPlaca)
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
                var responseInfofin = new TiposPlacas_DA().ExisteTipoPlaca_Infofin(Entidad, TiposPlacas.CodigoInfofin);
                if (responseInfofin.ExecutionOK)
                {
                    if (TiposPlacas.IdTipoPlaca != responseInfofin.Data.IdTipoPlaca)
                    {
                        dbResponse.Data = true;
                        dbResponse.ExecutionOK = responseInfofin.ExecutionOK;
                        dbResponse.Message = responseInfofin.Message;
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

        public DBResponse<TiposPlacas> GetTipoPlacaCodigoInfofin(string CodigoInfofin, int Entidad)
        {
            var dbResponse = new DBResponse<TiposPlacas>();
            try
            {
                var responseInfofin = new TiposPlacas_DA().ExisteTipoPlaca_Infofin(Entidad, CodigoInfofin);
                if (responseInfofin.ExecutionOK)
                {
                    dbResponse.Data = responseInfofin.Data;
                    dbResponse.ExecutionOK = true;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new TiposPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
        public DBResponse<string> ValidacionesTipoPlaca(TiposPlacas TipoPlaca)
        {
            var dbResponse = new DBResponse<string>();
            try
            {
                var mensaje = "";
                if (TipoPlaca.TipoPlaca.Length == 0)
                {
                    mensaje += "El tipo de placa es obligatorio <br />";
                }
                if (TipoPlaca.TipoPlaca.Length > 200)
                {
                    mensaje += "El tipo de placa no debe tener más de 200 carácteres <br />";
                }
                if (TipoPlaca.CodigoInfofin.Length == 0)
                {
                    mensaje += "El código de Infofin es obligatorio <br />";
                }
                if (TipoPlaca.CodigoInfofin.Length > 100)
                {
                    mensaje += "El código infofin no debe tener más de 100 carácteres <br />";
                }
                if (TipoPlaca.MascaraPlaca.Length == 0)
                {
                    mensaje += "La mascara de la placa es obligatorio <br />";
                }
                if (TipoPlaca.MascaraPlaca.Length > 12)
                {
                    mensaje += "La mascara de la placa no debe tener más de 12 carácteres <br />";
                }
                //char[] charsMascara = TipoPlaca.MascaraPlaca.ToCharArray();
                //var mascaraContainsCharsInvalid = charsMascara.Where(chrInv => new char[] { 'Ñ', 'I','O','Q' }.Contains(chrInv)).ToList();
                //if (mascaraContainsCharsInvalid.Count > 0)
                //{
                //    mensaje += "La mascara de la placa no debe contener los siguientes caracteres: 'Ñ', 'I','O','Q' <br />";
                //}
                char[] charsMascara = TipoPlaca.MascaraPlaca.ToCharArray();
                var mascaraContainsCharsValid = charsMascara.Where(chrInv => new char[] { 'a','A','n','N' }.Contains(chrInv)).ToList();
                if (mascaraContainsCharsValid.Count() != charsMascara.Count())
                {
                    mensaje += "La mascara únicamente debe tener 'N' o 'A' <br />";
                }
                if (TipoPlaca.OrdenPlaca.Length == 0)
                {
                    mensaje += "El orden de la placa es obligatorio <br />";
                }
                if (TipoPlaca.OrdenPlaca.Length > 12)
                {
                    mensaje += "El orden de la placa no debe tener más de 12 carácteres <br />";
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

        public DBResponse<TiposPlacas> UpsertTipoPlaca(TiposPlacas TiposPlacas, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<TiposPlacas>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new TiposPlacas_DA().UpsertTipoPlaca(TiposPlacas, nRow);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            FechaEvento = DateTime.Now,
                            Evento = nRow ? "Inserta" : "Actualiza",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Tipos de Placa",
                            JsonObject = JsonConvert.SerializeObject(TiposPlacas),
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
                    dbResponse.Data = new TiposPlacas();
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "UpsertTipoPlaca",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Tipos de Placa",
                        JsonObject = JsonConvert.SerializeObject(TiposPlacas),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }

        public DBResponse<DBNull> CambiaEstatusTipoPlaca(int IdTipoPlaca, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new TiposPlacas_DA().CambiaEstatusTipoPlaca(IdTipoPlaca);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = "Update",
                            FechaEvento = DateTime.Now,
                            Evento = "Actualiza Estatus",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Tipos de Placa",
                            JsonObject = JsonConvert.SerializeObject(IdTipoPlaca),
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
                        LugarEvento = "Tipos de Placa",
                        JsonObject = JsonConvert.SerializeObject(IdTipoPlaca),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }
    }
}

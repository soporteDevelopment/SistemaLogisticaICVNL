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
    public class Parametrizacion_BL
    {
        public DBResponse<Parametrizacion> GetParametrizacion()
        {
            var dbResponse = new DBResponse<Parametrizacion>();
            try
            {
                var accesoDatos = new Parametrizacion_DA().GetParametrizacion();
                if (accesoDatos.ExecutionOK)
                {
                    dbResponse.Data = accesoDatos.Data;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new Parametrizacion();
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new Parametrizacion();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
        public DBResponse<DBNull> UpsertParametrizacion(Parametrizacion parametrizacion, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<DBNull>();

            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    var response = new Parametrizacion_DA().UpsertParametrizacion(parametrizacion, false);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Parametrización",
                            JsonObject = JsonConvert.SerializeObject(parametrizacion),
                            Entidad = usuario.Entidad
                        });
                        dbResponse.Message = response.Message;
                        transaction.Complete();
                    }
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
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
                    LugarEvento = "Parametrización",
                    JsonObject = JsonConvert.SerializeObject(parametrizacion),
                    Entidad = usuario.Entidad
                });
            }
            
            return dbResponse;
        }

    }
}

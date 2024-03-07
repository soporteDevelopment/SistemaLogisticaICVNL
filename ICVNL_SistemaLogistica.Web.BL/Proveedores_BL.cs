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
    public class Proveedores_BL
    {
        public DBResponse<List<Proveedores>> GetProveedores(string NumeroProv, string Proveedor, int? Estatus, int Entidad)
        {
            var dbResponse = new DBResponse<List<Proveedores>>();
            try
            {
                var responseData = new Proveedores_DA().GetProveedores_List(NumeroProv, Proveedor, Estatus, Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<Proveedores>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Proveedores> GetProveedor(int id, int Entidad)
        {
            var dbResponse = new DBResponse<Proveedores>();
            try
            {
                var responseData = new Proveedores_DA().GetProveedores_ById(id, Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = responseData.ExecutionOK ? 1 : 0;
                dbResponse.ExecutionOK = responseData.ExecutionOK;
                dbResponse.Message = responseData.Message;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new Proveedores();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Boolean> ExisteProveedor(Proveedores Proveedores, int Entidad)
        {
            var dbResponse = new DBResponse<Boolean>();
            try
            {
                var response = new Proveedores_DA().ExisteProveedor(Entidad, Proveedores.NumeroProveedor);
                if (response.ExecutionOK)
                {
                    if (Proveedores.Id != response.Data.Id)
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

        public DBResponse<Proveedores> GetProveedorByNumero(string NumeroProveedor, int Entidad)
        {
            var dbResponse = new DBResponse<Proveedores>();
            try
            {
                var response = new Proveedores_DA().ExisteProveedor(Entidad, NumeroProveedor);
                if (response.ExecutionOK)
                {
                    dbResponse.Data = response.Data;
                    dbResponse.ExecutionOK = response.ExecutionOK;
                    dbResponse.Message = response.Message;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new Proveedores();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<string> ValidacionesProveedor(Proveedores Proveedor)
        {
            var dbResponse = new DBResponse<string>();
            try
            {
                var mensaje = "";
                if (Proveedor.NumeroProveedor.Length == 0)
                {
                    mensaje += "El Proveedor es obligatorio <br />";
                }
                if (Proveedor.NumeroProveedor.Length > 200)
                {
                    mensaje += "El Número de Proveedor no debe tener más de 20 carácteres <br />";
                }
                if (Proveedor.EmailProveedor.Length == 0)
                {
                    mensaje += "El Email de Proveedor es obligatorio <br />";
                }
                if (Proveedor.EmailProveedor.Length > 200)
                {
                    mensaje += "El Email de Proveedor no debe tener más de 200 carácteres <br />";
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

        public DBResponse<Proveedores> UpsertProveedor(Proveedores Proveedores, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<Proveedores>();

            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    var response = new Proveedores_DA().UpsertProveedor(Proveedores, nRow);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Proveedores",
                            JsonObject = JsonConvert.SerializeObject(Proveedores),
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
                dbResponse.Data = new Proveedores();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "Error",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Proveedores",
                    JsonObject = JsonConvert.SerializeObject(Proveedores),
                    Entidad = usuario.Entidad
                });
            }
            return dbResponse;
        }

        public DBResponse<DBNull> CambiaEstatusProveedor(int IdProveedor, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new Proveedores_DA().CambiaEstatusProveedor(IdProveedor);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = "Update",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = "Cambia Estatus",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Proveedores",
                            JsonObject = JsonConvert.SerializeObject(IdProveedor),
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
                        LugarEvento = "Proveedores",
                        JsonObject = JsonConvert.SerializeObject(IdProveedor),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }
    }
}

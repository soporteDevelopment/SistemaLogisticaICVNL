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
    public class Contratos_BL
    {
        public DBResponse<List<Contratos>> GetContratos(int? FiltroIdContrato_,
                                                        string FiltroNumeroContrato_,
                                                        int? FiltroIdProveedor_,
                                                        int Entidad)
        {
            var dbResponse = new DBResponse<List<Contratos>>();
            try
            {
                var responseData = new Contratos_DA().GetContratos_List(FiltroIdContrato_, FiltroNumeroContrato_, FiltroIdProveedor_, Entidad);
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<Contratos>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Contratos> GetContrato(int idContrato, int Entidad)
        {
            var dbResponse = new DBResponse<Contratos>();
            try
            {
                var contrato_ = new Contratos();
                var responseData = new Contratos_DA().GetContratosEnc(idContrato, Entidad);
                if (responseData.ExecutionOK)
                {
                    var dataResponse = responseData.Data;
                    contrato_.IdContrato = dataResponse.IdContrato;
                    contrato_.NumeroContrato = dataResponse.NumeroContrato;

                    var responseDataDetails = new Contratos_DA().GetContratosDet(idContrato, Entidad);
                    if (responseDataDetails.ExecutionOK)
                    {
                        var consecutivoDet = 1;
                        contrato_.Contratos_Detalle = new List<Contratos_Detalle>();
                        foreach (var detail in responseDataDetails.Data)
                        {
                            var consecutivoRng = 1;
                            List<Contratos_Detalles_Rangos> _rangosDetails = new List<Contratos_Detalles_Rangos>();
                            var responseDataDetailsRng = new Contratos_DA().GetContratosDet_Rangos(detail.IdContratoDetalle, Entidad);
                            foreach (var rango in responseDataDetailsRng.Data)
                            {
                                rango.Consecutivo = consecutivoRng;
                                _rangosDetails.Add(rango);
                                consecutivoRng++;
                            }
                            detail.Contratos_Detalles_Rangos = _rangosDetails;
                            detail.Consecutivo = consecutivoDet;
                            detail.MascaraPlaca = detail.MascaraPlaca;
                            detail.OrdenPlaca = detail.OrdenPlaca;
                            detail.Proveedores = new Proveedores();
                            var dbResponseProveedores = new Proveedores_DA().GetProveedores_ById(detail.IdProveedor, Entidad);
                            if (dbResponseProveedores.ExecutionOK)
                            {
                                detail.Proveedores = dbResponseProveedores.Data;
                            }
                            detail.TipoPlacas = new TiposPlacas();
                            var dbResponseTipoPlaca = new TiposPlacas_DA().GetTiposPlacas_ById(Entidad, detail.IdTipoPlaca);
                            if (dbResponseTipoPlaca.ExecutionOK)
                            {
                                detail.TipoPlacas = dbResponseTipoPlaca.Data;
                            }
                            contrato_.Contratos_Detalle.Add(detail);
                            consecutivoDet++;
                        } 
                    }
                    var responseArchivos = new Archivos_DA().GetArchivos_List(contrato_.IdContrato, "CPL_CONTRATOS", Entidad);
                    if (responseArchivos.ExecutionOK)
                    {
                        var consecutivoArchivo = 1;
                        contrato_.Contratos_Archivos = new List<Contratos_Archivos>();
                        foreach (var detail in responseArchivos.Data)
                        {
                            contrato_.Contratos_Archivos.Add(new Contratos_Archivos()
                            {
                                Archivo = detail.Archivo,
                                Consecutivo = consecutivoArchivo,
                                IdContrato = contrato_.IdContrato,
                                NombreArchivo = detail.NombreArchivo
                            });
                            consecutivoArchivo++;
                        }
                    }

                    dbResponse.Data = contrato_;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new Contratos();
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new Contratos();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<Contratos_Detalle> GetContratosTipoPlacaDet(int IdContrato, int IdTipoPlaca, int Entidad)
        {
            return new Contratos_DA().GetContratosTipoPlacaDet(IdContrato, IdTipoPlaca, Entidad);
        }

        public DBResponse<Contratos> UpsertContrato(Contratos objContratos, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<Contratos>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    if (!nRow)
                    {
                        //Eliminamos la informacion detallada del contrato
                        var infoContrato = new Contratos_BL().GetContrato(objContratos.IdContrato, objContratos.Entidad).Data;
                        var deleteData = new Contratos_DA().DeleteInfoContrato(infoContrato);
                    }
                    var responseData = new Contratos_DA().UpsertContrato(objContratos, nRow);
                    if (responseData.ExecutionOK)
                    {
                        dbResponse.Data = responseData.Data;
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Contratos",
                            JsonObject = JsonConvert.SerializeObject(objContratos),
                            Entidad = usuario.Entidad
                        });

                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Data = new Contratos();
                        dbResponse.NumRows = 0;
                        dbResponse.ExecutionOK = false;
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                    dbResponse.Data = new Contratos();
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Contratos",
                        JsonObject = JsonConvert.SerializeObject(objContratos),
                        Entidad = usuario.Entidad
                    });
                }
            }

            return dbResponse;
        }

        public DBResponse<DBNull> DeleteContrato(int IdContrato, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var response = new Contratos_DA().DeleteContrato(IdContrato);
                    if (response.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = "Update",
                            FechaEvento = DateTime.Now,
                            Evento = "Actualiza Estatus",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Contratos",
                            JsonObject = JsonConvert.SerializeObject(IdContrato),
                            Entidad = usuario.Entidad
                        });
                        transaction.Complete();
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;
                    }
                    else
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = response.Message;
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
                        LugarEvento = "Contratos",
                        JsonObject = JsonConvert.SerializeObject(IdContrato),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }
    }
}

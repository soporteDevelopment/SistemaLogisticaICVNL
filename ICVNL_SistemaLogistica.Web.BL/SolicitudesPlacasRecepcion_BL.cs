using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Helpers;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class SolicitudesPlacasRecepcion_BL
    {
        public DBResponse<List<RecepcionSolicitudesPlacas>> GetRecepcionSolicitudesPlacas(string FolioRecepcion,
                                                                                        int IdDelegacionBanco,
                                                                                        string FolioSolicitud,
                                                                                        int IdProveedor,
                                                                                        string NumeroContrato,
                                                                                        int IdOrdenCompra,
                                                                                        int Entidad)
        {
            var dbResponse = new DBResponse<List<RecepcionSolicitudesPlacas>>();
            try
            {
                var responseData = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_List(FolioRecepcion, IdDelegacionBanco, FolioSolicitud, IdProveedor, NumeroContrato, IdOrdenCompra, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = dbResponse.Data.Count;
                    dbResponse.ExecutionOK = dbResponse.Data.Count != 0;
                }
                else
                {
                    dbResponse.Data = new List<RecepcionSolicitudesPlacas>();
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<RecepcionSolicitudesPlacas>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<RecepcionSolicitudesPlacas> GetRecepcionSolicitudPlacas(int IdRecepcionSolicitudPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas>();
            try
            {
                var responseData = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacasEnc(IdRecepcionSolicitudPlaca, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = new RecepcionSolicitudesPlacas()
                    {
                        IdRecepcion = responseData.Data.IdRecepcion,
                        IdDelegacionBanco = responseData.Data.IdDelegacionBanco,
                        IdSolicitud = responseData.Data.IdSolicitud,
                        Fecha = responseData.Data.Fecha,
                        Entidad = responseData.Data.Entidad,
                        Estatus = responseData.Data.Estatus,
                        FolioRecepcion = responseData.Data.FolioRecepcion,
                        NotaEntradaAutorizada = responseData.Data.NotaEntradaAutorizada,
                        Recibida = responseData.Data.Recibida,
                        SolicitudesPlacas = new SolicitudesPlacas(),
                        DelegacionesBancos = new DelegacionesBancos(),
                        RecepcionSolicitudesPlacas_Detalle = new List<RecepcionSolicitudesPlacas_Detalle>(),
                        UsuarioRegistro = responseData.Data.UsuarioRegistro
                    };

                    var solicitudPlacas = new SolicitudesPlacas_BL().GetSolicitudPlacas(dbResponse.Data.IdSolicitud, Entidad);
                    if (solicitudPlacas.ExecutionOK)
                    {
                        dbResponse.Data.SolicitudesPlacas = solicitudPlacas.Data;
                    }

                    var delegeacionesBancos = new DelegacionesBancos_BL().GetDelegacionBanco(Entidad, dbResponse.Data.IdDelegacionBanco);
                    if (delegeacionesBancos.ExecutionOK)
                    {
                        dbResponse.Data.DelegacionesBancos = delegeacionesBancos.Data;
                    }

                    var detalle = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacasDet(IdRecepcionSolicitudPlaca, Entidad);
                    if (detalle.ExecutionOK)
                    {
                        foreach (var item in detalle.Data)
                        {
                            item.TiposPlacas = new TiposPlacas(); 
                            //asignamos el objeto Tipo Placa
                            var dbResponseTipoPlaca = new TiposPlacas_DA().GetTiposPlacas_ById(Entidad, item.IdTipoPlaca);
                            if (dbResponseTipoPlaca.ExecutionOK)
                            {
                                item.TiposPlacas = dbResponseTipoPlaca.Data;
                            }
                            dbResponse.Data.RecepcionSolicitudesPlacas_Detalle.Add(item);
                        }
                    }
                     
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new RecepcionSolicitudesPlacas();
                    dbResponse.Message = "No se encontro la información de la Recepción de la Solicitud de Placas";
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }         

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new RecepcionSolicitudesPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
        public DBResponse<RecepcionSolicitudesPlacas> GetRecepcionesSolicitudesPlacasSiguienteFolio()
        {
            return new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacasSiguienteFolio();
        }
        public DBResponse<DBNull> InsertaRecepcionSolicitudPlaca_Validaciones_Observacion(RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones observaciones)
        {
            return new RecepcionSolicitudesPlacas_DA().InsertaRecepcionSolicitudPlaca_Validaciones_Observacion(observaciones);
        }
        public DBResponse<DBNull> DeleteRecepcionSolicitudPlaca_Validaciones_Observacion(int IdObservacion)
        {
            return new RecepcionSolicitudesPlacas_DA().DeleteRecepcionSolicitudPlaca_Validaciones_Observacion(IdObservacion);
        }
        public DBResponse<RecepcionSolicitudesPlacas_Recibir> InsertaRecepcionSolicitudPlaca_Recibir(RecepcionSolicitudesPlacas_Recibir recibir, Usuarios usuario)
        { 
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas_Recibir>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {  
                    var dbResponseUpsert = new RecepcionSolicitudesPlacas_DA().InsertaRecepcionSolicitudPlaca_Recibir(recibir);
                    if (dbResponseUpsert.ExecutionOK)
                    {
                        dbResponse.Data = dbResponseUpsert.Data;
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = "Insert",
                            FechaEvento = DateTime.Now,
                            Evento = "Insertar",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Recepción de Solicitud de Placas",
                            JsonObject = JsonConvert.SerializeObject(recibir),
                            Entidad = usuario.Entidad
                        });
                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Data = new RecepcionSolicitudesPlacas_Recibir();
                        dbResponse.NumRows = 0;
                        dbResponse.ExecutionOK = false;
                    }

                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new RecepcionSolicitudesPlacas_Recibir();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "Error",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Recepción de Solicitud de Placas",
                    JsonObject = JsonConvert.SerializeObject(recibir),
                    Entidad = usuario.Entidad
                });
            }
            return dbResponse;
        }
        public DBResponse<DBNull> DeleteRecepcionSolicitudPlaca(int IdRecepcion)
        {
            return new RecepcionSolicitudesPlacas_DA().DeleteRecepcionSolicitudPlaca(IdRecepcion);
        }
        public DBResponse<RecepcionSolicitudesPlacas> UpsertRecepcionSolicitudPlacas(RecepcionSolicitudesPlacas recepcionSolicitudesPlacas, Usuarios usuario, Boolean nRow)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    //Eliminamos el detalle de la solicitud
                    if (!nRow)
                    {
                        //Eliminamos la informacion detallada del contrato
                        var infoRecepcionSolicitud = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudPlacas(recepcionSolicitudesPlacas.IdRecepcion, recepcionSolicitudesPlacas.Entidad).Data;
                        var dbResponseEliminaDet = new RecepcionSolicitudesPlacas_DA().DeleteInfoReceepcionSolicitudPlaca(infoRecepcionSolicitud);
                    }

                    //Insertamos/Actualizamos la recepcion de la solicitud
                    var dbResponseUpsert = new RecepcionSolicitudesPlacas_DA().UpsertRecepcionSolicitudPlaca(recepcionSolicitudesPlacas, nRow);
                    if (dbResponseUpsert.ExecutionOK)
                    {
                        dbResponse.Data = dbResponseUpsert.Data;
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            FechaEvento = DateTime.Now,
                            Evento = nRow ? "Insertar" : "Actualizar",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Recepción de Solicitud de Placas",
                            JsonObject = JsonConvert.SerializeObject(recepcionSolicitudesPlacas),
                            Entidad = usuario.Entidad
                        });
                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Data = new RecepcionSolicitudesPlacas();
                        dbResponse.NumRows = 0;
                        dbResponse.ExecutionOK = false;
                    }

                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new RecepcionSolicitudesPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;

                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                {
                    InstruccionRealizada = "Error",
                    FechaEvento = DateTime.Now,
                    Evento = "UpsertRecepcionSolicitudPlacas",
                    IP_Usuario = usuario.IP_Usuario,
                    Usuario = usuario.Usuario,
                    LugarEvento = "Recepción de Solicitud de Placas",
                    JsonObject = JsonConvert.SerializeObject(recepcionSolicitudesPlacas),
                    Entidad = usuario.Entidad
                });
            }
            return dbResponse;
        }

        public DBResponse<List<RecepcionSolicitudesPlacas_Recibir>> GetListadoPlacasRecibidas(int IdSolicitud, int Entidad)
        {
            return new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir(IdSolicitud, Entidad);
        }

        public DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones>> GetRecepcionesSolicitudesPlacas_Recibir_ValidacionesList(int IdEventoRecepcion, int Entidad)
        {
            var dbResponse = new DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones>>();
            try
            {
                var dbResponseRecepcion = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_ValidacionesList(IdEventoRecepcion, Entidad);
                if (dbResponseRecepcion.ExecutionOK)
                {
                    foreach (var item in dbResponseRecepcion.Data)
                    {
                        var archivoGen = new Archivos_BL().GetArchivos_List(item.IdEventoRecepcion, "CPL_REC_SOL_PLACAS_EV", Entidad).Data.FirstOrDefault();
                        item.IdArchivo = archivoGen == null ? 0 : archivoGen.IdArchivo;
                        item.Archivos = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>();
                        item.Observaciones = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>();

                        var observacionesResponse = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Observaciones(IdEventoRecepcion, Entidad);
                        if (observacionesResponse.ExecutionOK)
                        {
                            foreach (var itemObs in observacionesResponse.Data)
                            {
                                item.Observaciones.Add(new RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones()
                                {
                                    IdObservacion = itemObs.IdObservacion,
                                    Observacion = itemObs.Observacion
                                });
                            }
                        }
                        var archivosResponse = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Archivos(IdEventoRecepcion, Entidad);
                        if (archivosResponse.ExecutionOK)
                        {
                            var consecutivoArchivo = 0;
                            foreach (var itemArchivo in archivosResponse.Data)
                            {
                                item.Archivos.Add(new RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos()
                                {
                                    IdArchivo = itemArchivo.IdArchivo,
                                    NombreArchivo = itemArchivo.NombreArchivo,
                                    Archivo = itemArchivo.Archivo,
                                    Consecutivo = consecutivoArchivo
                                });

                            }
                        }
                    } 
                    dbResponse = dbResponseRecepcion;
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No se pudo obtener la información";
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<RecepcionSolicitudesPlacas_Recibir_Validaciones> GetRecepcionesSolicitudesPlacas_Recibir_Validaciones(int IdValidacionEvento, int Entidad)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas_Recibir_Validaciones>();
            try
            {
                var dbResponseRecepcion = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validaciones(IdValidacionEvento, Entidad);
                if (dbResponseRecepcion.ExecutionOK)
                {

                    dbResponseRecepcion.Data.Archivos = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>();
                    dbResponseRecepcion.Data.Observaciones = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>();

                    var observacionesResponse = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Observaciones(IdValidacionEvento, Entidad);
                    if (observacionesResponse.ExecutionOK)
                    {
                        foreach (var itemObs in observacionesResponse.Data)
                        {
                            dbResponseRecepcion.Data.Observaciones.Add(new RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones()
                            {
                                IdObservacion = itemObs.IdObservacion,
                                Observacion = itemObs.Observacion
                            });
                        }
                    }
                    var archivosResponse = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Archivos(IdValidacionEvento, Entidad);
                    if (archivosResponse.ExecutionOK)
                    {
                        var consecutivoArchivo = 0;
                        foreach (var itemArchivo in archivosResponse.Data)
                        {
                            dbResponseRecepcion.Data.Archivos.Add(new RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos()
                            {
                                IdArchivo = itemArchivo.IdArchivo,
                                NombreArchivo = itemArchivo.NombreArchivo,
                                Archivo = itemArchivo.Archivo,
                                Consecutivo = consecutivoArchivo
                            });

                        }
                    }
                    dbResponse = dbResponseRecepcion;
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No se pudo obtener la información";
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new RecepcionSolicitudesPlacas_Recibir_Validaciones();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<List<RecepcionSolicitudesPlacas_Eventos>> GetRecepcionesSolicitudesPlacas_ListEventos(DateTime FechaIniQR,
                                                                                                                 DateTime FechaFinQR,
                                                                                                                 int IdProveedor,
                                                                                                                 int IdDelegacionBanco,
                                                                                                                 int IdTipoPlaca,
                                                                                                                 int IdTipoEventoRPN,
                                                                                                                 int Entidad)
        {
            var dbResponse = new DBResponse<List<RecepcionSolicitudesPlacas_Eventos>>();
            try
            {
                var responseData = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_ListEventos(FechaIniQR, FechaFinQR, IdProveedor, IdDelegacionBanco, IdTipoPlaca, IdTipoEventoRPN, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = responseData.Data.Count;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<RecepcionSolicitudesPlacas_Eventos>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<RecepcionSolicitudesPlacas_Eventos> GetRecepcionesSolicitudesPlacas_Evento(int IdEventoRecepcion, int Entidad)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas_Eventos>();
            try
            {
                var responseData = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Evento(IdEventoRecepcion, Entidad);
                if (responseData.ExecutionOK)
                {
                    var archivoGen = new Archivos_BL().GetArchivos_List(IdEventoRecepcion, "CPL_REC_SOL_PLACAS_EV", Entidad).Data.FirstOrDefault();
                    responseData.Data.IdArchivo = archivoGen == null ? 0 : archivoGen.IdArchivo;
                    responseData.Data.Archivos = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>();
                    responseData.Data.Observaciones = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>();
                    var observacionesResponse = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Observaciones(IdEventoRecepcion, Entidad);
                    if (observacionesResponse.ExecutionOK)
                    {
                        foreach (var itemObs in observacionesResponse.Data)
                        {
                            responseData.Data.Observaciones.Add(new RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones()
                            {
                                IdObservacion = itemObs.IdObservacion,
                                Observacion = itemObs.Observacion
                            });
                        }
                    }
                    var archivosResponse = new RecepcionSolicitudesPlacas_DA().GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Archivos(IdEventoRecepcion, Entidad);
                    if (archivosResponse.ExecutionOK)
                    {
                        var consecutivoArchivo = 0;
                        foreach (var itemArchivo in archivosResponse.Data)
                        {
                            responseData.Data.Archivos.Add(new RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos()
                            {
                                IdArchivo = itemArchivo.IdArchivo,
                                NombreArchivo = itemArchivo.NombreArchivo,
                                Archivo = itemArchivo.Archivo,
                                Consecutivo = consecutivoArchivo
                            });

                        }
                    }


                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new RecepcionSolicitudesPlacas_Eventos();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<List<ErroresGeneradosPlacas>> ValidaRecepcionRangosPlacas(GenerarPlacasRangos GenerarPlacasRangos)
        {
            var dbResponse = new DBResponse<List<ErroresGeneradosPlacas>>();
            dbResponse.Data = new List<ErroresGeneradosPlacas>();
            try
            {
                var algoritmo = new AlgoritmoGeneracionPlacas_BL().Validaciones(GenerarPlacasRangos);
                if (algoritmo.ExecutionOK)
                {
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Data = algoritmo.Data;
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK =false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
    }
}

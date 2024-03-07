using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class TransferenciasPlacas_BL
    {
        public DBResponse<List<TransferenciaPlacas>> GetTransferenciaPlacas(string Folio,
                                                                            DateTime FechaInicio,
                                                                            DateTime FechaFin,
                                                                            string NombreDP,
                                                                            string ApellidoDP,
                                                                            int NumeroIdDP,
                                                                            int tipoIdDP,
                                                                            int IdDelegacionOrigen,
                                                                            int IdDelegacionDestino,
                                                                            int IdEstatusTransferencia,
                                                                            int IdEsatusPlacas,
                                                                            int Entidad,
                                                                            string StoreProcedured)
        {
            var dbResponse = new DBResponse<List<TransferenciaPlacas>>();
            try
            {
                var responseData = new TransferenciasPlacas_DA().GetTransferenciasPlacas_List(Folio,
                                                                                              FechaInicio,
                                                                                              FechaFin,
                                                                                              NombreDP,
                                                                                              ApellidoDP,
                                                                                              NumeroIdDP,
                                                                                              tipoIdDP,
                                                                                              IdDelegacionOrigen,
                                                                                              IdDelegacionDestino,
                                                                                              IdEstatusTransferencia,
                                                                                              IdEsatusPlacas,
                                                                                              Entidad,
                                                                                              StoreProcedured);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = dbResponse.Data.Count;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new List<TransferenciaPlacas>();
                    dbResponse.NumRows = dbResponse.Data.Count;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<TransferenciaPlacas>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas> GetRecepcionTransferenciaPlacasEnc(int IdTransferencia,
                                                                     int Entidad)
        {
            return new TransferenciasPlacas_DA().GetRecepcionTransferenciaPlacasEnc(IdTransferencia, Entidad);
        }
        public DBResponse<TransferenciaPlacas> GetTransferenciaPlacasEnc(int IdTransferencia,
                                                             int Entidad)
        {
            return new TransferenciasPlacas_DA().GetTransferenciaPlacasEnc(IdTransferencia, Entidad);
        }

        public DBResponse<TransferenciaPlacas> GetTransferenciaPlaca_Details(int IdTransferencia, int Entidad)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            try
            {
                var responseData = new TransferenciasPlacas_BL().GetTransferenciaPlacasEnc(IdTransferencia, Entidad);
                if (responseData.ExecutionOK)
                {
                    var objResponse = responseData.Data;

                    var objTransferenciaPlacas = new TransferenciaPlacas()
                    {
                        IdTransferencia = objResponse.IdTransferencia,
                        IdDelegacionBancoDestino = objResponse.IdDelegacionBancoDestino,
                        IdDelegacionBancoOrigen = objResponse.IdDelegacionBancoOrigen,
                        IdEstatusTransferencia = objResponse.IdEstatusTransferencia,
                        IdTransferenciaDatosPersona = objResponse.IdTransferenciaDatosPersona,
                        IdTransferenciaTransporte = objResponse.IdTransferenciaTransporte,
                        Entidad = objResponse.Entidad,
                        FechaHoraRegistro = objResponse.FechaHoraRegistro,
                        FolioTransferencia = objResponse.FolioTransferencia,
                        DelegacionesBancosDestino = objResponse.DelegacionesBancosDestino,
                        DelegacionesBancosOrigen = objResponse.DelegacionesBancosOrigen,
                        TiposEstatusTransferencias = objResponse.TiposEstatusTransferencias,
                        TransferenciaPlacas_Listado1 = new List<TransferenciaPlacas_Listado1>(),
                        TransferenciaPlacas_Listado2 = new List<TransferenciaPlacas_Listado2>(),
                        UsuarioRegistro = objResponse.UsuarioRegistro,
                        TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                        {
                            IdTipoIDs = objResponse.TransferenciaPlacas_DatosPersona.IdTipoIDs,
                            Apellido = objResponse.TransferenciaPlacas_DatosPersona.Apellido,
                            Entidad = objResponse.TransferenciaPlacas_DatosPersona.Entidad,
                            IdTransferencia = objResponse.TransferenciaPlacas_DatosPersona.IdTransferencia,
                            IdTransferenciaDatosPersona = objResponse.TransferenciaPlacas_DatosPersona.IdTransferenciaDatosPersona,
                            Nombre = objResponse.TransferenciaPlacas_DatosPersona.Nombre,
                            NumeroID = objResponse.TransferenciaPlacas_DatosPersona.NumeroID,
                            Tipo = objResponse.TransferenciaPlacas_DatosPersona.Tipo,
                            TiposID = objResponse.TransferenciaPlacas_DatosPersona.TiposID
                        },
                        TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                        {
                            IdTransferencia = objResponse.TransferenciaPlacas_Transporte.IdTransferencia,
                            Entidad = objResponse.TransferenciaPlacas_Transporte.Entidad,
                            IdTransferenciaTransporte = objResponse.TransferenciaPlacas_Transporte.IdTransferenciaTransporte,
                            MarcaVehiculo = objResponse.TransferenciaPlacas_Transporte.MarcaVehiculo,
                            ModeloVehiculo = objResponse.TransferenciaPlacas_Transporte.ModeloVehiculo,
                            Tipo = objResponse.TransferenciaPlacas_Transporte.Tipo,
                            NumeroEconomico = objResponse.TransferenciaPlacas_Transporte.NumeroEconomico,
                            PlacasVehiculo = objResponse.TransferenciaPlacas_Transporte.PlacasVehiculo
                        }
                    };

                    var responseDataDetail = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, Entidad);
                    if (responseDataDetail.ExecutionOK)
                    {
                        var objDetailAutomatico = responseDataDetail.Data.Where(x => x.Automatico == true).ToList();
                        var getPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Detalle(objTransferenciaPlacas.IdDelegacionBancoOrigen, Entidad);
                        var listPlacas = getPlacas.Data;
                        var getTiposPlacas = new TiposPlacas_BL().GetTiposPlacas(1, Entidad).Data;

                        objTransferenciaPlacas.TransferenciaPlacas_Listado1 = getTiposPlacas.GroupBy(x => new { x.IdTipoPlaca })
                                            .Select(w => new TransferenciaPlacas_Listado1
                                            {
                                                IdTransferenciaListado1 = 0,
                                                IdTransferencia = 0,
                                                IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                TiposPlacas = new TiposPlacas()
                                                {
                                                    IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                    TipoPlaca = w.FirstOrDefault().TipoPlaca
                                                },
                                                CantidadDisponiblesDelegacion = listPlacas.Where(x => x.IdEstatusPlacas == 1 && x.IdTipoPlaca == w.FirstOrDefault().IdTipoPlaca).Sum(s => s.Existencia),
                                                CantidadDisponiblesSerTransferidas = objDetailAutomatico.Where(y => y.IdTipoPlaca == w.FirstOrDefault().IdTipoPlaca).Count()
                                            }).OrderBy(p => p.IdTipoPlaca).ToList();

                        if (objTransferenciaPlacas.IdEstatusTransferencia != 1)
                        {
                            objTransferenciaPlacas.TransferenciaPlacas_Listado1 = new List<TransferenciaPlacas_Listado1>();
                            foreach (var item in getTiposPlacas)
                            {
                                if (responseDataDetail.Data.Any(x => x.IdTipoPlaca == item.IdTipoPlaca))
                                {
                                    objTransferenciaPlacas.TransferenciaPlacas_Listado1.Add(new TransferenciaPlacas_Listado1()
                                    {
                                        IdTransferenciaListado1 = 0,
                                        IdTransferencia = 0,
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        TiposPlacas = new TiposPlacas()
                                        {
                                            IdTipoPlaca = item.IdTipoPlaca,
                                            TipoPlaca = item.TipoPlaca
                                        },
                                        CantidadDisponiblesDelegacion = 0,
                                        CantidadDisponiblesSerTransferidas = responseDataDetail.Data.Where(y => y.IdTipoPlaca == item.IdTipoPlaca).Count()
                                    });
                                }
                            }

                            foreach (var item in responseDataDetail.Data)
                            {
                                objTransferenciaPlacas.TransferenciaPlacas_Listado2.Add(new TransferenciaPlacas_Listado2()
                                {
                                    Automatico = item.Automatico,
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    TiposPlacas = new TiposPlacas()
                                    {
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        TipoPlaca = item.TiposPlacas.TipoPlaca
                                    },
                                    IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                    TiposEstatusPlacas = new TiposEstatusPlacas()
                                    {
                                        IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                        TipoEstatusPlacas = item.TiposEstatusPlacas.TipoEstatusPlacas
                                    },
                                    IdTransferencia = item.IdTransferencia,
                                    IdTransferenciaListado2 = 0,
                                    NumeroPlaca = item.NumeroPlaca,
                                    TransferirPlaca = item.TransferirPlaca
                                });
                            }
                        }
                        else
                        {
                            foreach (var item in listPlacas)
                            {
                                var objPlacaTransferencia = (responseDataDetail.Data.Where(x => x.NumeroPlaca == item.NumeroPlaca).FirstOrDefault());
                                objTransferenciaPlacas.TransferenciaPlacas_Listado2.Add(new TransferenciaPlacas_Listado2()
                                {
                                    Automatico = responseDataDetail.Data.Any(x => x.NumeroPlaca == item.NumeroPlaca) ? objPlacaTransferencia.Automatico : false,
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    TiposPlacas = new TiposPlacas()
                                    {
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        TipoPlaca = item.TiposPlacas.TipoPlaca
                                    },
                                    IdTipoEstatusPlacas = item.IdEstatusPlacas,
                                    TiposEstatusPlacas = new TiposEstatusPlacas()
                                    {
                                        IdTipoEstatusPlacas = item.IdEstatusPlacas,
                                        TipoEstatusPlacas = item.EstatusPlacas.TipoEstatusPlacas
                                    },
                                    IdTransferencia = 0,
                                    IdTransferenciaListado2 = 0,
                                    NumeroPlaca = item.NumeroPlaca,
                                    TransferirPlaca = responseDataDetail.Data.Any(x => x.NumeroPlaca == item.NumeroPlaca) ? objPlacaTransferencia.TransferirPlaca : false
                                });
                            }
                        }
                    }

                    dbResponse.Data = objTransferenciaPlacas;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new TransferenciaPlacas();
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new TransferenciaPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas> GetRecepcionTransferenciaPlaca_Details(int IdTransferencia, int Entidad)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            try
            {
                var responseData = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlacasEnc(IdTransferencia, Entidad);
                if (responseData.ExecutionOK)
                {
                    var objResponse = responseData.Data;

                    var objTransferenciaPlacas = new TransferenciaPlacas()
                    {
                        IdTransferencia = objResponse.IdTransferencia,
                        IdDelegacionBancoDestino = objResponse.IdDelegacionBancoDestino,
                        IdDelegacionBancoOrigen = objResponse.IdDelegacionBancoOrigen,
                        IdEstatusTransferencia = objResponse.IdEstatusTransferencia,
                        IdTransferenciaDatosPersona = objResponse.IdTransferenciaDatosPersona,
                        IdTransferenciaTransporte = objResponse.IdTransferenciaTransporte,
                        Entidad = objResponse.Entidad,
                        FechaHoraRegistro = objResponse.FechaHoraRegistro,
                        FolioTransferencia = objResponse.FolioTransferencia,
                        DelegacionesBancosDestino = objResponse.DelegacionesBancosDestino,
                        DelegacionesBancosOrigen = objResponse.DelegacionesBancosOrigen,
                        TiposEstatusTransferencias = objResponse.TiposEstatusTransferencias,
                        TransferenciaPlacas_Listado1 = new List<TransferenciaPlacas_Listado1>(),
                        TransferenciaPlacas_Listado2 = new List<TransferenciaPlacas_Listado2>(),
                        UsuarioRegistro = objResponse.UsuarioRegistro,
                        TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                        {
                            IdTipoIDs = objResponse.TransferenciaPlacas_DatosPersona.IdTipoIDs,
                            Apellido = objResponse.TransferenciaPlacas_DatosPersona.Apellido,
                            Entidad = objResponse.TransferenciaPlacas_DatosPersona.Entidad,
                            IdTransferencia = objResponse.TransferenciaPlacas_DatosPersona.IdTransferencia,
                            IdTransferenciaDatosPersona = objResponse.TransferenciaPlacas_DatosPersona.IdTransferenciaDatosPersona,
                            Nombre = objResponse.TransferenciaPlacas_DatosPersona.Nombre,
                            NumeroID = objResponse.TransferenciaPlacas_DatosPersona.NumeroID,
                            Tipo = objResponse.TransferenciaPlacas_DatosPersona.Tipo,
                            TiposID = objResponse.TransferenciaPlacas_DatosPersona.TiposID
                        },
                        TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                        {
                            IdTransferencia = objResponse.TransferenciaPlacas_Transporte.IdTransferencia,
                            Entidad = objResponse.TransferenciaPlacas_Transporte.Entidad,
                            IdTransferenciaTransporte = objResponse.TransferenciaPlacas_Transporte.IdTransferenciaTransporte,
                            MarcaVehiculo = objResponse.TransferenciaPlacas_Transporte.MarcaVehiculo,
                            ModeloVehiculo = objResponse.TransferenciaPlacas_Transporte.ModeloVehiculo,
                            Tipo = objResponse.TransferenciaPlacas_Transporte.Tipo,
                            NumeroEconomico = objResponse.TransferenciaPlacas_Transporte.NumeroEconomico,
                            PlacasVehiculo = objResponse.TransferenciaPlacas_Transporte.PlacasVehiculo
                        }
                    };

                    var responseDataDetail = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, Entidad);
                    if (responseDataDetail.ExecutionOK)
                    {
                        var getTiposPlacas = new TiposPlacas_BL().GetTiposPlacas(1, Entidad).Data;

                        objTransferenciaPlacas.TransferenciaPlacas_Listado1 = new List<TransferenciaPlacas_Listado1>();
                        foreach (var item in getTiposPlacas)
                        {
                            if (responseDataDetail.Data.Any(x => x.IdTipoPlaca == item.IdTipoPlaca))
                            {
                                objTransferenciaPlacas.TransferenciaPlacas_Listado1.Add(new TransferenciaPlacas_Listado1()
                                {
                                    IdTransferenciaListado1 = 0,
                                    IdTransferencia = 0,
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    TiposPlacas = new TiposPlacas()
                                    {
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        TipoPlaca = item.TipoPlaca
                                    },
                                    CantidadDisponiblesDelegacion = 0,
                                    CantidadDisponiblesSerTransferidas = responseDataDetail.Data.Where(y => y.IdTipoPlaca == item.IdTipoPlaca).Count()
                                });
                            }
                        }

                        foreach (var item in responseDataDetail.Data)
                        {
                            objTransferenciaPlacas.TransferenciaPlacas_Listado2.Add(new TransferenciaPlacas_Listado2()
                            {
                                Automatico = item.Automatico,
                                IdTipoPlaca = item.IdTipoPlaca,
                                TiposPlacas = new TiposPlacas()
                                {
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    TipoPlaca = item.TiposPlacas.TipoPlaca
                                },
                                IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                TiposEstatusPlacas = new TiposEstatusPlacas()
                                {
                                    IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                    TipoEstatusPlacas = item.TiposEstatusPlacas.TipoEstatusPlacas
                                },
                                IdTransferencia = item.IdTransferencia,
                                IdTransferenciaListado2 = 0,
                                NumeroPlaca = item.NumeroPlaca,
                                TransferirPlaca = item.TransferirPlaca
                            });
                        }
                    }
                    dbResponse.Data = objTransferenciaPlacas;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new TransferenciaPlacas();
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new TransferenciaPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<List<TransferenciaPlacas_Detalle>> GetTransferenciaPlacasDet(int IdTransferencia, int Entidad)
        {
            return new TransferenciasPlacas_DA().GetTransferenciaPlacasDet(IdTransferencia, Entidad);
        }

        public DBResponse<TransferenciaPlacas> GetTransferenciaPlaca_PackingList(int IdTransferencia, int Entidad)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            try
            {
                var responseData = new TransferenciasPlacas_BL().GetTransferenciaPlacasEnc(IdTransferencia, Entidad);
                if (responseData.ExecutionOK)
                {
                    var objTransferenciaPlacas = responseData.Data;

                    var objDelegacionOrigen = new DelegacionesBancos_BL().GetDelegacionBanco(Entidad, objTransferenciaPlacas.IdDelegacionBancoOrigen).Data;
                    var objDelegacionDestino = new DelegacionesBancos_BL().GetDelegacionBanco(Entidad, objTransferenciaPlacas.IdDelegacionBancoDestino).Data;
                    objTransferenciaPlacas.DelegacionesBancosOrigen = new DelegacionesBancos();
                    objTransferenciaPlacas.DelegacionesBancosOrigen = objDelegacionOrigen;
                    objTransferenciaPlacas.DelegacionesBancosDestino = new DelegacionesBancos();
                    objTransferenciaPlacas.DelegacionesBancosDestino = objDelegacionDestino;
                    objTransferenciaPlacas.TransferenciaPlacas_Listado2 = new List<TransferenciaPlacas_Listado2>();
                    objTransferenciaPlacas.TransferenciaPlacas_Listado1 = new List<TransferenciaPlacas_Listado1>();

                    var responseDataDetail = new TransferenciasPlacas_DA().GetTransferenciaPlacasDet(IdTransferencia, Entidad);
                    if (responseDataDetail.ExecutionOK)
                    {
                        var objDetailAutomatico = responseDataDetail.Data.ToList();

                        var getPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Detalle(objTransferenciaPlacas.IdDelegacionBancoOrigen, Entidad);
                        var listPlacas = getPlacas.Data;

                        var getTiposPlacas = new TiposPlacas_BL().GetTiposPlacas(1, Entidad).Data;

                        objTransferenciaPlacas.TransferenciaPlacas_Listado1 = getTiposPlacas.GroupBy(x => new { x.IdTipoPlaca })
                                            .Select(w => new TransferenciaPlacas_Listado1
                                            {
                                                IdTransferenciaListado1 = 0,
                                                IdTransferencia = 0,
                                                IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                TiposPlacas = new TiposPlacas()
                                                {
                                                    IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                    TipoPlaca = w.FirstOrDefault().TipoPlaca
                                                },
                                                CantidadDisponiblesDelegacion = listPlacas.Where(x => x.IdTipoPlaca == w.FirstOrDefault().IdTipoPlaca).Sum(s => s.Existencia),
                                                CantidadDisponiblesSerTransferidas = objDetailAutomatico.Where(y => y.IdTipoPlaca == w.FirstOrDefault().IdTipoPlaca).Count()
                                            }).OrderBy(p => p.IdTipoPlaca).ToList();

                        foreach (var item in responseDataDetail.Data)
                        {
                            objTransferenciaPlacas.TransferenciaPlacas_Listado2.Add(new TransferenciaPlacas_Listado2()
                            {
                                Automatico = item.Automatico,
                                IdTipoPlaca = item.IdTipoPlaca,
                                TiposPlacas = new TiposPlacas()
                                {
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    TipoPlaca = item.TiposPlacas.TipoPlaca
                                },
                                IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                TiposEstatusPlacas = new TiposEstatusPlacas()
                                {
                                    IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                    TipoEstatusPlacas = item.TiposEstatusPlacas.TipoEstatusPlacas
                                },
                                IdTransferencia = 0,
                                IdTransferenciaListado2 = 0,
                                NumeroPlaca = item.NumeroPlaca,
                                TransferirPlaca = true
                            });
                        }
                    }

                    dbResponse.Data = objTransferenciaPlacas;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new TransferenciaPlacas();
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new TransferenciaPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<List<TransferenciaPlacas_RecibirPlacasIndividuales>> GetRecibirPlacasIndividuales(int IdTransferencia, int IdTipoPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<List<TransferenciaPlacas_RecibirPlacasIndividuales>>();
            try
            {
                var responseData = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = new List<TransferenciaPlacas_RecibirPlacasIndividuales>();

                    foreach (var item in responseData.Data.Where(x => x.IdTipoPlaca == IdTipoPlaca))
                    {
                        dbResponse.Data.Add(new TransferenciaPlacas_RecibirPlacasIndividuales()
                        {
                            IdTipoPlaca = item.IdTipoPlaca,
                            NumeroPlaca = item.NumeroPlaca,
                            IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                            IdTransferencia = item.IdTransferencia,
                            TiposEstatusPlacas = new TiposEstatusPlacas()
                            {
                                IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                                TipoEstatusPlacas = item.TiposEstatusPlacas.TipoEstatusPlacas
                            },
                            TiposPlacas = new TiposPlacas()
                            {
                                IdTipoPlaca = item.IdTipoPlaca,
                                TipoPlaca = item.TiposPlacas.TipoPlaca
                            },
                            IdTransferenciaIndividual = item.TransferirPlacaIndividual,
                            IdTransferenciaListado2 = 0
                        });
                    }

                    dbResponse.NumRows = dbResponse.Data.Count;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<TransferenciaPlacas_RecibirPlacasIndividuales>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas> UpsertTransferenciaPlaca(TransferenciaPlacas transferencia, Boolean nRow, Usuarios usuario)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var dbResponseInventario = new InventarioPlacas_BL().GetInventarioPlacas_Detalle(transferencia.IdDelegacionBancoOrigen, transferencia.Entidad);

                    if (!nRow)
                    {
                        var objResponseTransferenciaBD = new TransferenciasPlacas_BL().GetTransferenciaPlaca_Details(transferencia.IdTransferencia, transferencia.Entidad);
                        if (objResponseTransferenciaBD.ExecutionOK)
                        {
                            objResponseTransferenciaBD.Data.TransferenciaPlacas_Detalle = new List<TransferenciaPlacas_Detalle>();
                            var objResponseDetalle = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(transferencia.IdTransferencia, transferencia.Entidad).Data;
                            objResponseTransferenciaBD.Data.TransferenciaPlacas_Detalle = objResponseDetalle;

                            if (objResponseTransferenciaBD.Data.IdDelegacionBancoOrigen != transferencia.IdDelegacionBancoOrigen)
                            {
                                foreach (var item in objResponseTransferenciaBD.Data.TransferenciaPlacas_Detalle)
                                {
                                    var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoHistoricoCambiosPlaca(item.NumeroPlaca, objResponseTransferenciaBD.Data.IdDelegacionBancoOrigen, objResponseTransferenciaBD.Data.Entidad);
                                    if (infoPlacaHist.ExecutionOK)
                                    {
                                        if (infoPlacaHist.Data.Count > 0)
                                        {
                                            var objPlacaHist = infoPlacaHist.Data.FirstOrDefault();
                                            var infoPlaca = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(item.NumeroPlaca, objResponseTransferenciaBD.Data.IdDelegacionBancoOrigen, objResponseTransferenciaBD.Data.Entidad);

                                            var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                            {
                                                IdEstatusAnterior = infoPlaca.Data.IdEstatusPlacas,
                                                IdEstatusNuevo = objPlacaHist.IdEstatusAnterior,
                                                IdInventarioExistencia = infoPlaca.Data.IdInventarioDetalle,
                                                IdInventarioExistenciaHistorial = 0
                                            };

                                            var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, objResponseTransferenciaBD.Data.Entidad);
                                        }
                                    }
                                }

                                foreach (var item in transferencia.TransferenciaPlacas_Detalle)
                                {
                                    if (item.NumeroPlaca != "" && item.PlacaSeleccionadaManual && item.TransferirPlaca)
                                    {
                                        var infoPlaca = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(item.NumeroPlaca, transferencia.IdDelegacionBancoOrigen, transferencia.Entidad);

                                        var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                        {
                                            IdEstatusAnterior = infoPlaca.Data.IdEstatusPlacas,
                                            IdEstatusNuevo = 6,
                                            IdInventarioExistencia = infoPlaca.Data.IdInventarioDetalle,
                                            IdInventarioExistenciaHistorial = 0
                                        };

                                        var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, item.Entidad);
                                    }
                                }
                            }
                            else
                            {

                                if (dbResponseInventario.ExecutionOK)
                                {
                                    var inventarioDetalle = dbResponseInventario.Data;

                                    foreach (var item in inventarioDetalle)
                                    {
                                        if (transferencia.TransferenciaPlacas_Detalle.Any(x => x.NumeroPlaca == item.NumeroPlaca && x.TransferirPlaca))
                                        {
                                            var itemPlaca = transferencia.TransferenciaPlacas_Detalle.Where(x => x.NumeroPlaca == item.NumeroPlaca && x.TransferirPlaca).FirstOrDefault();
                                            if (itemPlaca.IdTipoEstatusPlacas != 6)
                                            {
                                                var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                                {
                                                    IdEstatusAnterior = item.IdEstatusPlacas,
                                                    IdEstatusNuevo = 6,
                                                    IdInventarioExistencia = item.IdInventarioDetalle,
                                                    IdInventarioExistenciaHistorial = 0
                                                };

                                                var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, transferencia.Entidad);
                                            }
                                        }
                                        else
                                        {
                                            var itemPlaca = transferencia.TransferenciaPlacas_Detalle.Where(x => x.NumeroPlaca == item.NumeroPlaca && x.TransferirPlaca == false).FirstOrDefault();
                                            if (itemPlaca.IdTipoEstatusPlacas == 6)
                                            {
                                                var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoHistoricoCambiosPlaca(itemPlaca.NumeroPlaca, transferencia.IdDelegacionBancoOrigen, transferencia.Entidad);
                                                if (infoPlacaHist.ExecutionOK)
                                                {
                                                    if (infoPlacaHist.Data.Count > 0)
                                                    {
                                                        var objPlacaHist = infoPlacaHist.Data.FirstOrDefault();
                                                        var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                                        {
                                                            IdEstatusAnterior = item.IdEstatusPlacas,
                                                            IdEstatusNuevo = objPlacaHist.IdEstatusAnterior,
                                                            IdInventarioExistencia = item.IdInventarioDetalle,
                                                            IdInventarioExistenciaHistorial = 0
                                                        };

                                                        var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, transferencia.Entidad);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        var deleteDetalle = new TransferenciasPlacas_DA().DeleteTransferenciaPlacasDetalle(transferencia.IdTransferencia, transferencia.Entidad);
                    }

                    var responseData = new TransferenciasPlacas_DA().UpsertTransferenciaPlacas(transferencia, nRow);
                    if (responseData.ExecutionOK)
                    {
                        dbResponse.Data = responseData.Data;
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;

                        if (nRow)
                        {
                            foreach (var item in transferencia.TransferenciaPlacas_Detalle)
                            {
                                if (item.NumeroPlaca != "" && item.PlacaSeleccionadaManual && item.TransferirPlaca)
                                {
                                    var infoPlaca = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(item.NumeroPlaca, transferencia.IdDelegacionBancoOrigen, transferencia.Entidad);

                                    var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                    {
                                        IdEstatusAnterior = infoPlaca.Data.IdEstatusPlacas,
                                        IdEstatusNuevo = 6,
                                        IdInventarioExistencia = infoPlaca.Data.IdInventarioDetalle,
                                        IdInventarioExistenciaHistorial = 0
                                    };

                                    var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, item.Entidad);
                                }
                            }
                        }

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = nRow ? "Inserta" : "Actualiza",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = nRow ? "Insert" : "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Transferencia de Placas entre Delegaciones",
                            JsonObject = JsonConvert.SerializeObject(transferencia),
                            Entidad = usuario.Entidad
                        });

                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Data = new TransferenciaPlacas();
                        dbResponse.NumRows = 0;
                        dbResponse.ExecutionOK = false;
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                    dbResponse.Data = new TransferenciaPlacas();
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Transferencia de Placas entre Delegaciones",
                        JsonObject = JsonConvert.SerializeObject(transferencia),
                        Entidad = usuario.Entidad
                    });
                }
            }

            return dbResponse;
        }
        public DBResponse<TransferenciaPlacas> InsertPackingList_TransferenciaPlaca(TransferenciaPlacas transferencia, Usuarios usuario)
        {
            var dbResponse = new DBResponse<TransferenciaPlacas>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var responseData = new TransferenciasPlacas_DA().PackingListTransferenciaPlacas(transferencia);
                    if (responseData.ExecutionOK)
                    {
                        foreach (var item in transferencia.TransferenciaPlacas_Detalle.Where(x => x.PlacaSeleccionadaAutomatica))
                        {
                            var infoPlaca = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(item.NumeroPlaca, transferencia.IdDelegacionBancoOrigen, transferencia.Entidad);

                            var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                            {
                                IdEstatusAnterior = infoPlaca.Data.IdEstatusPlacas,
                                IdEstatusNuevo = 6,
                                IdInventarioExistencia = infoPlaca.Data.IdInventarioDetalle,
                                IdInventarioExistenciaHistorial = 0
                            };

                            var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, transferencia.Entidad);
                        }

                        dbResponse.Data = responseData.Data;
                        dbResponse.NumRows = 1;
                        dbResponse.ExecutionOK = true;

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = "Inserta Packing List",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = "Insert",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Transferencia de Placas entre Delegaciones",
                            JsonObject = JsonConvert.SerializeObject(transferencia),
                            Entidad = usuario.Entidad
                        });

                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Data = new TransferenciaPlacas();
                        dbResponse.NumRows = 0;
                        dbResponse.ExecutionOK = false;
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                    dbResponse.Data = new TransferenciaPlacas();
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Transferencia de Placas entre Delegaciones",
                        JsonObject = JsonConvert.SerializeObject(transferencia),
                        Entidad = usuario.Entidad
                    });
                }
            }

            return dbResponse;
        }

        public DBResponse<TransferenciaPlacas_DatosPersona> UpsertTransferencia_DatosPersona(TransferenciaPlacas_DatosPersona _DatosPersona)
        {
            return new TransferenciasPlacas_DA().UpsertTransferencia_DatosPersona(_DatosPersona);
        }
        public DBResponse<TransferenciaPlacas_Transporte> UpsertTransferencia_Transporte(TransferenciaPlacas_Transporte _Transporte)
        {
            return new TransferenciasPlacas_DA().UpsertTransferencia_Transporte(_Transporte);
        }

        public DBResponse<SolicitudesPlacas> GetTransferenciaSiguienteFolio()
        {
            return new TransferenciasPlacas_DA().GetTransferenciaSiguienteFolio();
        }
        public DBResponse<DBNull> UpdateInventariosPlacasDet(TransferenciaPlacas_Detalle transferenciaPlacas, int Entidad)
        {
            return new TransferenciasPlacas_DA().UpdateInventariosPlacasDet(transferenciaPlacas, Entidad);
        }
        public DBResponse<DBNull> CambioEstatusTransferencia(int IdTransferencia, int EstatusTransferencia, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    //cambiar el estatus de la transferencia
                    var dbResponseCambia = new TransferenciasPlacas_DA().CambioEstatusTransferencia(IdTransferencia, EstatusTransferencia);
                    if (dbResponseCambia.ExecutionOK)
                    {
                        if (EstatusTransferencia == 2) //Cancelada
                        {
                            var dbCancelarTransf = new TransferenciasPlacas_BL().CambiaDelegacionPlacasTransferencia(IdTransferencia, usuario);
                        }
                        else if (EstatusTransferencia == 3) //Realizada
                        {
                            var dbCancelarTransf = new TransferenciasPlacas_BL().CambiaDelegacionPlacasTransferencia(IdTransferencia, usuario);
                        }
                        else if (EstatusTransferencia == 4) //Rechazada
                        {
                            var dbRechazarTransf = new TransferenciasPlacas_BL().RechazarTotalidadPlacasTransferencias(IdTransferencia, usuario);
                        }
                        else if (EstatusTransferencia == 5) //Recibida
                        {
                            var dbRecibirTransferencia = new TransferenciasPlacas_BL().RecibirTotalidadPlacasTransferencias(IdTransferencia, usuario);
                        }

                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            InstruccionRealizada = getTipoEstatus(EstatusTransferencia) + " Transferencia",
                            FechaEvento = DateTime.Now,
                            Evento = "Update",
                            IP_Usuario = usuario.IP_Usuario,
                            Usuario = usuario.Usuario,
                            LugarEvento = "Transferencia de Placas entre Delegaciones",
                            JsonObject = JsonConvert.SerializeObject(IdTransferencia),
                            Entidad = usuario.Entidad
                        });
                        dbResponse.ExecutionOK = true;

                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.Message = "Ocurrio un error al cambiar el estatus de la transferencia";
                        dbResponse.ExecutionOK = false;
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al cambiar el estatus de la transferencia " + ex.Message;
                    dbResponse.ExecutionOK = false;

                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        InstruccionRealizada = "Error",
                        FechaEvento = DateTime.Now,
                        Evento = "Error",
                        IP_Usuario = usuario.IP_Usuario,
                        Usuario = usuario.Usuario,
                        LugarEvento = "Transferencia de Placas entre Delegaciones",
                        JsonObject = JsonConvert.SerializeObject(IdTransferencia),
                        Entidad = usuario.Entidad
                    });
                }
            }
            return dbResponse;
        }

        public DBResponse<DBNull> RecibirTotalidadPlacasTransferencias(int IdTransferencia, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var transferenciaDB_ = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlacasEnc(IdTransferencia, usuario.Entidad).Data;
                    var objResponseTransferenciaBD = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, usuario.Entidad);
                    if (objResponseTransferenciaBD.ExecutionOK)
                    {

                        foreach (var item in objResponseTransferenciaBD.Data)
                        {
                            var inventarioPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Enc(transferenciaDB_.IdDelegacionBancoDestino, transferenciaDB_.Entidad).Data;
                            var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoHistoricoCambiosPlaca(item.NumeroPlaca, transferenciaDB_.IdDelegacionBancoOrigen, transferenciaDB_.Entidad);
                            if (infoPlacaHist.Data.Count > 0)
                            {
                                var objPlacaHist = infoPlacaHist.Data.FirstOrDefault();
                                int IdEstatusAnterior = objPlacaHist.IdEstatusAnterior;
                                if (objPlacaHist.IdEstatusAnterior == 3)
                                {
                                    item.IdTipoEstatusPlacas = 1;
                                }
                                var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                {
                                    IdEstatusAnterior = IdEstatusAnterior,
                                    IdEstatusNuevo = item.IdTipoEstatusPlacas,
                                    IdInventarioExistencia = objPlacaHist.IdInventarioExistencia,
                                    IdInventarioExistenciaHistorial = 0
                                };
                                var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, inventarioPlacas.Entidad);
                                if (!cambiaEstatusPlaca.ExecutionOK)
                                {
                                    throw new Exception(cambiaEstatusPlaca.Message);
                                }
                            }


                            item.TransferirPlacaIndividual = 2;
                            item.UsuarioRecibio = usuario.Usuario;
                            var actualizaDetalleTransf = new TransferenciasPlacas_BL().UpdateInventariosPlacasDet(item, usuario.Entidad);
                            if (!actualizaDetalleTransf.ExecutionOK)
                            {
                                throw new Exception(actualizaDetalleTransf.Message);
                            }
                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Actualiza a los Números de Placas",
                                FechaEvento = DateTime.Now,
                                Evento = "Update",
                                IP_Usuario = usuario.IP_Usuario,
                                Usuario = usuario.Usuario,
                                LugarEvento = "Transferencia de Placas entre Delegaciones",
                                JsonObject = JsonConvert.SerializeObject(item),
                                Entidad = usuario.Entidad
                            });

                        }
                        dbResponse.ExecutionOK = true;
                        dbResponse.Message = "Transferencia Recibida correctamente";
                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "Ocurrio un error al recibir la transferencia";
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = ex.Message;
                }
            }

            return dbResponse;
        }
        public DBResponse<DBNull> RechazarTotalidadPlacasTransferencias(int IdTransferencia, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var transferenciaDB_ = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlacasEnc(IdTransferencia, usuario.Entidad).Data;
                    var objResponseTransferenciaBD = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, usuario.Entidad);
                    if (objResponseTransferenciaBD.ExecutionOK)
                    {

                        foreach (var item in objResponseTransferenciaBD.Data)
                        {
                            var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoHistoricoCambiosPlaca(item.NumeroPlaca, transferenciaDB_.IdDelegacionBancoOrigen, transferenciaDB_.Entidad);
                            if (infoPlacaHist.Data.Count > 0)
                            {
                                var objPlacaHist = infoPlacaHist.Data.FirstOrDefault();
                                int IdEstatusAnterior = objPlacaHist.IdEstatusAnterior;
                                if (objPlacaHist.IdEstatusAnterior == 1)
                                {
                                    item.IdTipoEstatusPlacas = 3;
                                }

                                var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                {
                                    IdEstatusAnterior = IdEstatusAnterior,
                                    IdEstatusNuevo = item.IdTipoEstatusPlacas,
                                    IdInventarioExistencia = objPlacaHist.IdInventarioExistencia,
                                    IdInventarioExistenciaHistorial = 0
                                };

                                var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, transferenciaDB_.Entidad);
                                if (!cambiaEstatusPlaca.ExecutionOK)
                                {
                                    throw new Exception(cambiaEstatusPlaca.Message);
                                }
                                var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                                {
                                    InstruccionRealizada = "Actualiza a los Números de Placas",
                                    FechaEvento = DateTime.Now,
                                    Evento = "Update",
                                    IP_Usuario = usuario.IP_Usuario,
                                    Usuario = usuario.Usuario,
                                    LugarEvento = "Transferencia de Placas entre Delegaciones",
                                    JsonObject = JsonConvert.SerializeObject(item),
                                    Entidad = usuario.Entidad
                                });
                            }

                            item.TransferirPlacaIndividual = 2;
                            item.UsuarioRecibio = usuario.Usuario;
                            var actualizaDetalleTransf = new TransferenciasPlacas_BL().UpdateInventariosPlacasDet(item, usuario.Entidad);
                            if (!actualizaDetalleTransf.ExecutionOK)
                            {
                                throw new Exception(actualizaDetalleTransf.Message);
                            }

                        }
                        dbResponse.ExecutionOK = true;
                        dbResponse.Message = "Transferencia Rechazada correctamente";
                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "Ocurrio un error al recibir la transferencia";
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = ex.Message;
                }
            }

            return dbResponse;
        }
        public DBResponse<DBNull> CambiaDelegacionPlacasTransferencia(int IdTransferencia, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var transferenciaDB_ = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlaca_Details(IdTransferencia, usuario.Entidad).Data;
                    foreach (var item in transferenciaDB_.TransferenciaPlacas_Detalle)
                    {
                        var inventarioPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Enc(transferenciaDB_.IdDelegacionBancoDestino, transferenciaDB_.Entidad).Data;
                        var actualizaDelegacion = new InventarioPlacas_BL().InventariosPlacas_CambiaDelegacionPlaca(inventarioPlacas.IdInventario, item.NumeroPlaca, transferenciaDB_.Entidad);
                        if (!actualizaDelegacion.ExecutionOK)
                        {
                            throw new Exception(actualizaDelegacion.Message);
                        }
                    }

                    dbResponse.ExecutionOK = true;
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = ex.Message;
                }
            }

            return dbResponse;
        } 
        public DBResponse<DBNull> CancelarTotalidadPlacasTransferencias(int IdTransferencia, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var transferenciaDB_ = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlacasEnc(IdTransferencia, usuario.Entidad).Data;
                    var objResponseTransferenciaBD = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, usuario.Entidad);
                    if (objResponseTransferenciaBD.ExecutionOK)
                    {

                        foreach (var item in objResponseTransferenciaBD.Data)
                        { 
                            var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoHistoricoCambiosPlaca(item.NumeroPlaca, transferenciaDB_.IdDelegacionBancoOrigen, transferenciaDB_.Entidad);
                            if (infoPlacaHist.Data.Count > 0)
                            {
                                var objPlacaHist = infoPlacaHist.Data.FirstOrDefault();
                      
                                var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                                {
                                    IdEstatusAnterior = objPlacaHist.IdEstatusNuevo,
                                    IdEstatusNuevo = objPlacaHist.IdEstatusAnterior,
                                    IdInventarioExistencia = objPlacaHist.IdInventarioExistencia,
                                    IdInventarioExistenciaHistorial = 0
                                };

                                var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, transferenciaDB_.Entidad);
                                if (!cambiaEstatusPlaca.ExecutionOK)
                                {
                                    throw new Exception(cambiaEstatusPlaca.Message);
                                }
                            }
                        }
                        dbResponse.ExecutionOK = true;
                        dbResponse.Message = "Transferencia Cancelada correctamente";
                        transaction.Complete();
                    }
                    else
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "Ocurrio un error al cancelar la transferencia";
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = ex.Message;
                }
            }

            return dbResponse;
        }

        public DBResponse<DBNull> RecibirPlacasIndividuales(int IdTransferencia, List<TransferenciaPlacas_Detalle> Listado, Usuarios usuario)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    var transferenciaDB_ = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlacasEnc(IdTransferencia, usuario.Entidad).Data;
                    var objResponseTransferenciaBD = new TransferenciasPlacas_BL().GetTransferenciaPlacasDet(IdTransferencia, usuario.Entidad);

                    var inventarioPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Enc(transferenciaDB_.IdDelegacionBancoDestino, transferenciaDB_.Entidad).Data;

                    foreach (var item in Listado)
                    {
                        var infoPlacaHist = new InventarioPlacas_BL().GetInventarioPlacas_InfoHistoricoCambiosPlaca(item.NumeroPlaca, transferenciaDB_.IdDelegacionBancoOrigen, transferenciaDB_.Entidad);
                        if (infoPlacaHist.Data.Count > 0)
                        {
                            var objPlacaHist = infoPlacaHist.Data.FirstOrDefault();

                            var objHistInsert = new InventarioPlacas_Existencia_HistorialCambios()
                            {
                                IdEstatusAnterior = objPlacaHist.IdEstatusNuevo,
                                IdEstatusNuevo = item.IdTipoEstatusPlacas,
                                IdInventarioExistencia = objPlacaHist.IdInventarioExistencia,
                                IdInventarioExistenciaHistorial = 0
                            };

                            var cambiaEstatusPlaca = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(objHistInsert, item.NumeroPlaca, transferenciaDB_.Entidad);
                            if (!cambiaEstatusPlaca.ExecutionOK)
                            {
                                throw new Exception(cambiaEstatusPlaca.Message);
                            }

                            item.TransferirPlacaIndividual = 1;
                            item.UsuarioRecibio = usuario.Usuario;
                            var actualizaDetalleTransf = new TransferenciasPlacas_BL().UpdateInventariosPlacasDet(item, usuario.Entidad);
                            if (!actualizaDetalleTransf.ExecutionOK)
                            {
                                throw new Exception(actualizaDetalleTransf.Message);
                            }

                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Actualiza a los Números de Placas",
                                FechaEvento = DateTime.Now,
                                Evento = "Update",
                                IP_Usuario = usuario.IP_Usuario,
                                Usuario = usuario.Usuario,
                                LugarEvento = "Transferencia de Placas entre Delegaciones",
                                JsonObject = JsonConvert.SerializeObject(item),
                                Entidad = usuario.Entidad
                            });

                        }
                    }

                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Transferencia Individual realizada correctamente";
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = ex.Message;
                }
            }

            return dbResponse;
        }


        public string getTipoEstatus(int IdEstatusTransferencia)
        {
            string Estatus = "";
            if (IdEstatusTransferencia == 1)
                Estatus = "Registrada";
            else if (IdEstatusTransferencia == 2)
                Estatus = "Cancelada";
            else if (IdEstatusTransferencia == 3)
                Estatus = "Realizada";
            else if (IdEstatusTransferencia == 4)
                Estatus = "Rechazada";
            else if (IdEstatusTransferencia == 5)
                Estatus = "Recibida";
            else if (IdEstatusTransferencia == 6)
                Estatus = "Cerrada";

            return Estatus;
        }
    }
}

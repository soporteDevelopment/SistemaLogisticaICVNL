using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.ApiServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class InventarioPlacas_BL
    {
        public DBResponse<List<InventarioPlacas>> GetData(int FiltroIdDelegacion_, int FiltroIdTipoPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<List<InventarioPlacas>>();
            try
            {
                var responseData = new InventarioPlacas_DA().GetInventarioPlacas_List(FiltroIdDelegacion_, FiltroIdTipoPlaca, Entidad);
                if (responseData.ExecutionOK)
                {
                    for (int i = 0; i < responseData.Data.Count; i++)
                    {
                        responseData.Data[i].InventarioPlacas_Detalle = new List<InventarioPlacas_Detalle>();
                        responseData.Data[i].InventarioPlacas_Existencia = new List<InventarioPlacas_Existencia>();
                        var objDetalle = new InventarioPlacas_BL().GetInventarioPlacas_Detalle(responseData.Data[i].IdDelegacionBanco, Entidad);
                        if (objDetalle.ExecutionOK)
                        {
                            var newListTiposPlacas = new List<InventarioPlacas_Detalle>();
                            int Bloque = 1;
                            for (int x = 0; x < objDetalle.Data.Count; x++)
                            {

                                if (newListTiposPlacas.Count > 0)
                                {
                                    if ((x + 1) <= (objDetalle.Data.Count - 1))
                                    {
                                        if (objDetalle.Data[x].IdTipoPlaca == objDetalle.Data[(x + 1)].IdTipoPlaca && objDetalle.Data[x].IdEstatusPlacas != objDetalle.Data[(x + 1)].IdEstatusPlacas)
                                        {
                                            Bloque++;
                                        }
                                    }

                                }
                                newListTiposPlacas.Add(new InventarioPlacas_Detalle()
                                {
                                    IdTipoPlaca = objDetalle.Data[x].IdTipoPlaca,
                                    TiposPlacas = new TiposPlacas()
                                    {
                                        IdTipoPlaca = objDetalle.Data[x].IdTipoPlaca,
                                        TipoPlaca = objDetalle.Data[x].TiposPlacas.TipoPlaca,
                                    },
                                    IdEstatusPlacas = objDetalle.Data[x].IdEstatusPlacas,
                                    EstatusPlacas = new TiposEstatusPlacas()
                                    {
                                        IdTipoEstatusPlacas = objDetalle.Data[x].IdEstatusPlacas,
                                        TipoEstatusPlacas = objDetalle.Data[x].EstatusPlacas.TipoEstatusPlacas,
                                    },
                                    NumeroPlaca = objDetalle.Data[x].NumeroPlaca,
                                    Existencia = objDetalle.Data[x].Existencia,
                                    Bloque = Bloque
                                });
                            }

                            var inventarioPlacas = newListTiposPlacas.GroupBy(x => new { x.IdTipoPlaca, x.IdEstatusPlacas, x.Bloque })
                                                        .Select(w => new InventarioPlacas_Detalle
                                                        {
                                                            Bloque = w.FirstOrDefault().Bloque,
                                                            IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                            TiposPlacas = new TiposPlacas()
                                                            {
                                                                IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                                TipoPlaca = w.FirstOrDefault().TiposPlacas.TipoPlaca,
                                                            },
                                                            ExistenciaHasta = w.Where(x => x.IdTipoPlaca == w.FirstOrDefault().IdTipoPlaca && x.IdEstatusPlacas == w.FirstOrDefault().IdEstatusPlacas && x.Bloque == w.FirstOrDefault().Bloque).FirstOrDefault().NumeroPlaca,
                                                            ExistenciaDesde = w.Where(x => x.IdTipoPlaca == w.LastOrDefault().IdTipoPlaca && x.IdEstatusPlacas == w.LastOrDefault().IdEstatusPlacas && x.Bloque == w.FirstOrDefault().Bloque).LastOrDefault().NumeroPlaca,
                                                            Existencia = w.Where(x => x.IdTipoPlaca == w.LastOrDefault().IdTipoPlaca && x.IdEstatusPlacas == w.LastOrDefault().IdEstatusPlacas && x.Bloque == w.FirstOrDefault().Bloque).Sum(s => s.Existencia),
                                                            ExistenciaTotalRango = w.Where(x => x.Bloque == w.FirstOrDefault().Bloque).Sum(s => s.Existencia).ToString(),
                                                            IdEstatusPlacas = w.FirstOrDefault().IdEstatusPlacas,
                                                            EstatusPlacas = new TiposEstatusPlacas()
                                                            {
                                                                IdTipoEstatusPlacas = w.FirstOrDefault().IdEstatusPlacas,
                                                                TipoEstatusPlacas = w.FirstOrDefault().EstatusPlacas.TipoEstatusPlacas,
                                                            }
                                                        }).OrderBy(p => p.Bloque).ToList();


                            responseData.Data[i].InventarioPlacas_Detalle = inventarioPlacas;

                            foreach (var item in objDetalle.Data)
                            {
                                responseData.Data[i].InventarioPlacas_Existencia.Add(new InventarioPlacas_Existencia()
                                {
                                    CostoPlaca = item.CostoPlaca,
                                    Existencia = item.Existencia,
                                    HistorialCambios = new List<InventarioPlacas_Existencia_HistorialCambios>(),
                                    IdEstatusPlaca = item.IdEstatusPlacas,
                                    IdInventario = item.IdInventario,
                                    IdInventarioExistencia = item.IdInventarioDetalle,
                                    NumeroPlaca = item.NumeroPlaca,
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    PlacaContabilizada = item.PlacaContabilizada,
                                    NumeroPolizaGRP_Infofin = item.NumeroPolizaInfofin,
                                    TiposEstatusPlacas = new TiposEstatusPlacas()
                                    {
                                        IdTipoEstatusPlacas = item.EstatusPlacas.IdTipoEstatusPlacas,
                                        TipoEstatusPlacas = item.EstatusPlacas.TipoEstatusPlacas
                                    },
                                    TiposPlacas = new TiposPlacas()
                                    {
                                        IdTipoPlaca = item.TiposPlacas.IdTipoPlaca,
                                        TipoPlaca = item.TiposPlacas.TipoPlaca
                                    }
                                });
                            }

                            var tiposdePlacas = objDetalle.Data.GroupBy(x => new { x.IdTipoPlaca })
                                                        .Select(w => new TiposPlacas
                                                        {
                                                            IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                            TipoPlaca = w.FirstOrDefault().TiposPlacas.TipoPlaca,
                                                        }).OrderBy(p => p.IdTipoPlaca).ToList();

                            responseData.Data[i].InventarioPlacas_TotalesExistencia = new List<InventarioPlacas_TotalesExistencia>();

                            foreach (var itemTP in tiposdePlacas)
                            {
                                responseData.Data[i].InventarioPlacas_TotalesExistencia.Add(
                                    new InventarioPlacas_TotalesExistencia()
                                    {
                                        Existencia = inventarioPlacas.Where(w => w.IdTipoPlaca == itemTP.IdTipoPlaca).Sum(x => x.Existencia),
                                        IdInventario = responseData.Data[i].IdInventario,
                                        IdTipoPlaca = itemTP.IdTipoPlaca,
                                        TiposPlacas = new TiposPlacas()
                                        {
                                            IdTipoPlaca = itemTP.IdTipoPlaca,
                                            TipoPlaca = itemTP.TipoPlaca
                                        }
                                    });
                            }

                        }
                    }
                }
                dbResponse.Data = responseData.Data;
                dbResponse.NumRows = dbResponse.Data.Count;
                dbResponse.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<InventarioPlacas>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
        public DBResponse<InventarioPlacas> GetInventarioPlacas_Enc(int IdDelegacionBanco, int Entidad)
        {
            return new InventarioPlacas_DA().GetInventarioPlacas_Enc(IdDelegacionBanco, Entidad);
        }
        public DBResponse<List<InventarioPlacas_Detalle>> GetInventarioPlacas_Detalle(int IdDelegacionBanco, int Entidad)
        {
            return new InventarioPlacas_DA().GetInventarioPlacas_Detalle(IdDelegacionBanco, Entidad);
        }
        public DBResponse<List<InventarioPlacas_Detalle>> GetInventarioPlacas_Transferencia(int IdDelegacionBanco, int Entidad)
        {
            return new InventarioPlacas_DA().GetInventarioPlacas_Transferencia(IdDelegacionBanco, Entidad);
        }
        public DBResponse<DBNull> InventariosPlacas_CambiaEstatus(InventarioPlacas_Existencia_HistorialCambios historialCambios, string NumeroPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().InventariosPlacas_CambiaEstatus(historialCambios, NumeroPlaca, Entidad);
                    if (dbResponse.ExecutionOK)
                    {
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }
        public DBResponse<DBNull> InventariosPlacas_CambiaDelegacionPlaca(int IdInventario, string NumeroPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().InventariosPlacas_CambiaDelegacionPlaca(IdInventario, NumeroPlaca, Entidad);
                    if (dbResponse.ExecutionOK)
                    {
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }

        public DBResponse<DBNull> InventariosPlacas_ActualizaPlacaContabilizada(string NumeroPlaca, int Entidad)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().InventariosPlacas_ActualizaPlacaContabilizada(NumeroPlaca, Entidad);
                    if (dbResponse.ExecutionOK)
                    {
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }
        public DBResponse<InventarioPlacas_Detalle> GetInventarioPlacas_InfoPlaca(string NumeroPlaca, int IdDelegacionBanco, int Entidad)
        {
            return new InventarioPlacas_DA().GetInventarioPlacas_InfoPlaca(NumeroPlaca, IdDelegacionBanco, Entidad);
        }
        public DBResponse<List<InventarioPlacas_Existencia_HistorialCambios>> GetInventarioPlacas_InfoHistoricoCambiosPlaca(string NumeroPlaca, int IdDelegacionBanco, int Entidad)
        {
            return new InventarioPlacas_DA().GetInventarioPlacas_InfoHistoricoCambiosPlaca(NumeroPlaca, IdDelegacionBanco, Entidad);
        }
        public DBResponse<List<PlacasNoInvResponse>> GetPlacasNoInventariadas_EnviaNotificacion(int Entidad)
        {
            return new InventarioPlacas_DA().GetPlacasNoInventariadas_EnviaNotificacion(Entidad);
        }
        public DBResponse<DBNull> UpdatePlacaSinInventariar_MarcaNotifEnviada(PlacasNoInvResponse placasResponse)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().UpdatePlacaSinInventariar_MarcaNotifEnviada(placasResponse);
                    if (dbResponse.ExecutionOK)
                    {
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }
        public DBResponse<DBNull> InsertPlacaSinInventariar(PlacasNoInvResponse placasResponse)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().InsertPlacaSinInventariar(placasResponse);
                    if (dbResponse.ExecutionOK)
                    {
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }
        public DBResponse<DBNull> InsertPlacaInventario(InventarioPlacas_Detalle placaDetalle)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().InsertPlacaInventario(placaDetalle);
                    if (dbResponse.ExecutionOK)
                    {
                        transaction.Complete();
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }
        public DBResponse<DBNull> InsertInventariosPlacas_Entrega_Venta(PlacasResponse placasResponse, InventarioPlacas_Detalle infoPlaca)
        {
            var dbResponse = new DBResponse<DBNull>();
            using (var transaction = new TransactionDecorator())
            {
                try
                {
                    dbResponse = new InventarioPlacas_DA().InsertInventariosPlacas_Entrega_Venta(placasResponse);
                    if (dbResponse.ExecutionOK)
                    {
                        var estatusHistorial = new InventarioPlacas_Existencia_HistorialCambios()
                        {
                            IdInventarioExistencia = placasResponse.IdInventarioDetalle,
                            IdEstatusAnterior = infoPlaca.IdEstatusPlacas,
                            IdEstatusNuevo = placasResponse.TipoOperacion == "Entrega" ? 7 : 5,
                            FechaOperacion = DateTime.Now,
                            Operacion = ""
                        };

                        var cambiaEstatus = new InventarioPlacas_BL().InventariosPlacas_CambiaEstatus(estatusHistorial, infoPlaca.NumeroPlaca, placasResponse.Entidad);
                        if (cambiaEstatus.ExecutionOK)
                        {
                            transaction.Complete();
                        }
                        else
                        {
                            dbResponse.Message = "Ocurrio un error al guardar la información del estatus de la placa";
                            dbResponse.Data = null;
                            dbResponse.ExecutionOK = false;
                            dbResponse.NumRows = 0;
                        }
                    }
                    else
                    {
                        dbResponse.Message = "Ocurrio un error al guardar la información de la " + placasResponse.TipoOperacion + " de la placa";
                        dbResponse.Data = null;
                        dbResponse.ExecutionOK = false;
                        dbResponse.NumRows = 0;
                    }
                }
                catch (Exception ex)
                {
                    dbResponse.Message = "Ocurrio un error al guardar la información: " + ex.Message;
                    dbResponse.Data = null;
                    dbResponse.ExecutionOK = false;
                    dbResponse.NumRows = 0;
                }
            }
            return dbResponse;
        }
    }
}

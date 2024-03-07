using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class NotasEntradasPlacas_BL
    {
        public DBResponse<List<NotasEntradasPlacas>> GetNotasEntradasPlacas(string NumeroEntrada,
                                                                            DateTime FechaIniNE,
                                                                            DateTime FechaFinNE,
                                                                            string NumeroOrden,
                                                                            DateTime FechaEntregaIniOC,
                                                                            DateTime FechaEntregaFinOC,
                                                                            int IdEstatus1,
                                                                            int Entidad)
        {
            var dbResponse = new DBResponse<List<NotasEntradasPlacas>>();
            try
            {
                var responseData = new NotasEntrada_DA().GetNotasEntradasPlacas_List(NumeroEntrada, FechaIniNE, FechaFinNE, NumeroOrden, FechaEntregaIniOC, FechaEntregaFinOC, IdEstatus1, Entidad);
                if (responseData.ExecutionOK)
                {
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = dbResponse.Data.Count;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new List<NotasEntradasPlacas>();
                    dbResponse.NumRows = 0;
                    dbResponse.ExecutionOK = false;
                }
                
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new List<NotasEntradasPlacas>();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<NotasEntradasPlacas> GetNotasEntradasPlaca(int IdNotaEntrada, int Entidad)
        {
            var dbResponse = new DBResponse<NotasEntradasPlacas>();
            try
            {
                var responseData = new NotasEntrada_DA().GetNotasEntradasPlacasEnc(IdNotaEntrada, Entidad);
                if (responseData.ExecutionOK)
                {
                    var objNotaEntrada = responseData.Data;
                    dbResponse.Data = new NotasEntradasPlacas();
                    dbResponse.Data = objNotaEntrada;
                    dbResponse.Data.OrdenCompra = new Entities.OrdenesCompra();
                    var ordenCompra = new OrdenesCompra_BL().GetOrdenesCompraEnc(objNotaEntrada.IdOrdenCompra, Entidad);
                    if (ordenCompra.ExecutionOK)
                    {
                        dbResponse.Data.OrdenCompra = ordenCompra.Data;
                    }

                    dbResponse.Data.NotasEntradasPlacas_Detalle = new List<NotasEntradasPlacas_Detalle>();

                    var responseDetails = new NotasEntrada_DA().GetNotasEntradasPlacasDet(IdNotaEntrada, Entidad);
                    if (responseDetails.ExecutionOK)
                    {
                        foreach (var item in responseDetails.Data)
                        {
                            item.TiposPlacas = new TiposPlacas();
                            var tipoPlaca = new TiposPlacas_BL().GetTipoPlaca(Entidad, item.IdTipoPlaca);
                            if (tipoPlaca.ExecutionOK)
                            {
                                item.TiposPlacas = tipoPlaca.Data;
                            }
                            item.TiposEstatus = new TiposEstatusNotaEntrada()
                            {
                                IdEstatusNotaentrada = item.IdEstatusNotaEntrada,
                                EstatusNotaEntrada = item.IdEstatusNotaEntrada == 1 ? "Notas de Entrada con todas sus placas identificadas" : "Notas de Entrada con placas pendientes de ser identificadas"
                            };

                            dbResponse.Data.NotasEntradasPlacas_Detalle.Add(item);
                        }
                    }
                    dbResponse.Data = responseData.Data;
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new NotasEntradasPlacas();
                    dbResponse.NumRows = 1;
                    dbResponse.ExecutionOK = false;
                }

            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new NotasEntradasPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
        public DBResponse<NotasEntradasPlacas> GetNotasEntradasPlacasEnc_Folio(string FolioNE, int Entidad)
        {
            return new NotasEntrada_DA().GetNotasEntradasPlacasEnc_Folio(FolioNE, Entidad);
        }
        public DBResponse<List<NotasEntradasPlacas_Detalle>> GetNotasEntradasPlacasDet_Folio(string FolioNE, int Entidad)
        {
            return new NotasEntrada_DA().GetNotasEntradasPlacasDet_Folio(FolioNE, Entidad);
        }
        public DBResponse<NotasEntradasPlacas> UpsertNotasEntradasPlaca(NotasEntradasPlacas notasEntradasPlacas, int Entidad, Usuarios usuarios)
        {
            var dbResponse = new DBResponse<NotasEntradasPlacas>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {

                    dbResponse.Data = new NotasEntradasPlacas();

                    var responseExisteNE = new NotasEntradasPlacas_BL().GetNotasEntradasPlacasEnc_Folio(notasEntradasPlacas.NumeroNotaEntrada.ToString(), Entidad);
                    if (!responseExisteNE.ExecutionOK)
                    {

                        var dbresponseOC = new OrdenesCompra_BL().GetOrdenesCompraNumeroOC_Enc(notasEntradasPlacas.NumeroOrdenCompra, Entidad);
                        if (!dbresponseOC.ExecutionOK)
                        {
                            dbResponse.NumRows = 0;
                            dbResponse.ExecutionOK = false;
                            dbResponse.Message = "Orden de Compra no encontrada";
                            return dbResponse;
                        }
                        else
                        {
                            if (notasEntradasPlacas.IdDelegacionBanco != null)
                            {
                                var updateOC = new OrdenesCompra_BL().UpdateOrdenesCompraEnc(dbresponseOC.Data.IdOrdenCompra, (int)notasEntradasPlacas.IdDelegacionBanco);
                                if (!updateOC.ExecutionOK)
                                {
                                    dbResponse.NumRows = 0;
                                    dbResponse.ExecutionOK = false;
                                    dbResponse.Message = "Ocurrio un error al actualizar la delegación/banco de la Orden de Compra relacionada a la Nota de Entrada";
                                    return dbResponse;
                                }
                            }
                        }

                        notasEntradasPlacas.IdOrdenCompra = dbresponseOC.Data.IdOrdenCompra;
                        notasEntradasPlacas.CantidadNumerosPlacaPorIdentificarse = notasEntradasPlacas.NotasEntradasPlacas_Detalle.Sum(x => x.CantidadPlacas);
                        notasEntradasPlacas.CantidadNumerosPlacaIdentificada = 0;
                        for (int i = 0; i < notasEntradasPlacas.NotasEntradasPlacas_Detalle.Count; i++)
                        {
                            var responseTipoPlaca = new TiposPlacas_BL().GetTipoPlacaCodigoInfofin(notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].CodigoArticuloInfofin, Entidad);
                            if (!responseTipoPlaca.ExecutionOK)
                            {
                                dbResponse.NumRows = 0;
                                dbResponse.ExecutionOK = false;
                                dbResponse.Message = "Tipo de Placa (Código de artículo) no encontrado";
                                return dbResponse;
                            }
                            notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].IdTipoPlaca = responseTipoPlaca.Data.IdTipoPlaca;
                        }

                        var dbUpsert = new NotasEntrada_DA().InsertNotaEntradaPlaca(notasEntradasPlacas);
                        if (dbUpsert.ExecutionOK)
                        {
                            dbResponse.NumRows = 1;
                            dbResponse.ExecutionOK = true;
                            dbResponse.Message = "Se registro correctamente la nota de entrada";
                        }
                        else
                        {
                            dbResponse.NumRows = 0;
                            dbResponse.ExecutionOK = false;
                            dbResponse.Message = "Ocurrio un error al instertar la nota de entrada. Detalles: " + dbUpsert.Message;
                        }
                    }
                    else
                    {

                        var objEntrada = responseExisteNE.Data;
                        var responseDetails = new NotasEntrada_DA().GetNotasEntradasPlacasDet(objEntrada.IdNotaEntrada, Entidad);
                        for (int i = 0; i < notasEntradasPlacas.NotasEntradasPlacas_Detalle.Count; i++)
                        {
                            var responseTipoPlaca = new TiposPlacas_BL().GetTipoPlacaCodigoInfofin(notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].CodigoArticuloInfofin, Entidad);
                            if (!responseTipoPlaca.ExecutionOK)
                            {
                                dbResponse.NumRows = 0;
                                dbResponse.ExecutionOK = false;
                                dbResponse.Message = "Tipo de Placa (Código de artículo) no encontrado";
                                return dbResponse;
                            }
                            notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].IdTipoPlaca = responseTipoPlaca.Data.IdTipoPlaca;
                            if (!responseDetails.Data.Any(x => x.IdTipoPlaca == notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].IdTipoPlaca))
                            {
                                var insertDetalle = new NotasEntrada_DA().InsertNotaEntradaPlacaDetalle(notasEntradasPlacas.NotasEntradasPlacas_Detalle[i]);
                                if (!insertDetalle.ExecutionOK)
                                {
                                    dbResponse.NumRows = 0;
                                    dbResponse.ExecutionOK = false;
                                    dbResponse.Message = "Ocurrio un error al instertar la nota de entrada. Detalles: " + insertDetalle.Message;
                                }
                            }
                        }


                        var dbresponseOC = new OrdenesCompra_BL().GetOrdenesCompraNumeroOC_Enc(notasEntradasPlacas.NumeroOrdenCompra, Entidad);
                        if (dbresponseOC.ExecutionOK)
                        {
                            if (notasEntradasPlacas.IdDelegacionBanco != null)
                            {
                                var updateOC = new OrdenesCompra_BL().UpdateOrdenesCompraEnc(dbresponseOC.Data.IdOrdenCompra, (int)notasEntradasPlacas.IdDelegacionBanco);
                                if (!updateOC.ExecutionOK)
                                {
                                    dbResponse.NumRows = 0;
                                    dbResponse.ExecutionOK = false;
                                    dbResponse.Message = "Ocurrio un error al actualizar la delegación/banco de la Orden de Compra relacionada a la Nota de Entrada";
                                    return dbResponse;
                                }
                            }
                            var dbResponseDetOC = new OrdenesCompra_BL().GetOrdenesCompraDet(dbresponseOC.Data.IdOrdenCompra, Entidad);
                            for (int i = 0; i < notasEntradasPlacas.NotasEntradasPlacas_Detalle.Count; i++)
                            {
                                if (dbResponseDetOC.Data.Any(x=>x.IdTipoPlaca == notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].IdTipoPlaca))
                                {
                                    var objDetNE = notasEntradasPlacas.NotasEntradasPlacas_Detalle[i];
                                    var idCompraDetalle = dbResponseDetOC.Data.Where(x => x.IdTipoPlaca == notasEntradasPlacas.NotasEntradasPlacas_Detalle[i].IdTipoPlaca).FirstOrDefault().IdOrdenCompraDetalle;
                                    var updateDetOC = new OrdenesCompra_BL().UpdateOrdenesCompraDet(idCompraDetalle,
                                                                                                    objDetNE.CuentaContable,
                                                                                                    objDetNE.NumeroRenglon,
                                                                                                    objDetNE.CentroCostosAlmacen,
                                                                                                    objDetNE.FechaRecepcionTP);
                                    if (!updateDetOC.ExecutionOK)
                                    {
                                        dbResponse.NumRows = 0;
                                        dbResponse.ExecutionOK = false;
                                        dbResponse.Message = "Ocurrio un error al actualizar la informacion del articulo de la Orden de Compra en la Nota de Entrada relacionada";
                                        return dbResponse;
                                    }
                                }
                            }
                        }
                    }
                    var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                    {
                        Evento = "Inserta",
                        FechaEvento = DateTime.Now,
                        InstruccionRealizada = "Insert",
                        IP_Usuario = " ",
                        Usuario = usuarios.Usuario,
                        LugarEvento = "Proceso. Notas de Entrada de Placas del Sistema GRP Infofin",
                        JsonObject = JsonConvert.SerializeObject(notasEntradasPlacas),
                        Entidad = usuarios.Entidad
                    });


                    transaction.Complete();
                }
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = new NotasEntradasPlacas();
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> DeleteNotasEntradasPlaca(int IdNotaEntrada)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                dbResponse.Data = null;
                dbResponse.NumRows = 1;
                dbResponse.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                dbResponse.Message = "Ocurrio un error al obtener la informacón solicitada " + ex.Message;
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.NumRows = 0;
            }
            return dbResponse;
        }
    }
}

using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class NotasEntrada_DA
    {
        public DBResponse<List<NotasEntradasPlacas>> GetNotasEntradasPlacas_List(string NumeroEntrada,
                                                                                     DateTime FechaIniNE,
                                                                                     DateTime FechaFinNE,
                                                                                     string NumeroOrden,
                                                                                     DateTime FechaEntregaIniOC,
                                                                                     DateTime FechaEntregaFinOC,
                                                                                     int IdEstatus1, 
                                                                                     int Entidad)
        {
            var responseDB = new DBResponse<List<NotasEntradasPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<NotasEntradasPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroEntrada),
                    Db.CreateParameter("p_NEF_FECHA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaIniNE),
                    Db.CreateParameter("p_NEF_FECHA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaFinNE),
                    Db.CreateParameter("p_OCN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroOrden),
                    Db.CreateParameter("p_OCF_FECHA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaEntregaIniOC),
                    Db.CreateParameter("p_OCF_FECHA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaEntregaFinOC),
                    Db.CreateParameter("p_ENEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEstatus1), 
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_notasentrada_pl.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var notasEntradasPlacas = new NotasEntradasPlacas()
                        {
                            NumeroNotaEntrada = Datos.Int(row, "NEN_NUMERO"),
                            CantidadNumerosPlacaIdentificada = Datos.Int(row, "NEDN_CANT_PL_IDENT"),
                            CantidadNumerosPlacaPorIdentificarse = Datos.Int(row, "NEDN_CANT_PL_X_IDENT"),
                            CantidadPlacasNotaEntrada = Datos.Int(row, "NEDN_CANTIDADPLACAS"),
                            FechaNotaEntrada = Convert.ToDateTime(Datos.Str(row, "NEF_FECHA")),
                            OrdenCompra = new OrdenesCompra()
                            {
                                NumeroOrdenCompra = Datos.Str(row, "OCN_NUMERO"),
                                FechaOrdenCompra = Convert.ToDateTime(Datos.Str(row, "OCF_FECHA")),
                            },                         
                            IdNotaEntrada = Datos.Int(row, "NEN_ID"),
                            IdEstatusNotaEntrada = Datos.Int(row, "ENEN_ID"),
                            TiposEstatusNotaEntrada = new TiposEstatusNotaEntrada()
                            {
                                EstatusNotaEntrada = Datos.Str(row, "ESTATUS1")
                            }

                        };
                        responseDB.Data.Add(notasEntradasPlacas);
                    }

                    if (responseDB.Data.Count > 0)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = responseDB.Data.Count;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<NotasEntradasPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<NotasEntradasPlacas> GetNotasEntradasPlacasEnc(int IdNotaEntradaPlaca, int Entidad)
        {
            var responseDB = new DBResponse<NotasEntradasPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdNotaEntradaPlaca),
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_notasentrada_pl.consulta_notasentrada_enc", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    DataRow row = dt.Rows[0];
                    responseDB.Data = new NotasEntradasPlacas()
                    {
                        NumeroNotaEntrada = Datos.Int(row, "NEN_NUMERO"),                         
                        CantidadNumerosPlacaIdentificada = Datos.Int(row, "NEDN_CANT_PL_IDENT"),
                        CantidadNumerosPlacaPorIdentificarse = Datos.Int(row, "NEDN_CANT_PL_X_IDENT"),
                        CantidadPlacasNotaEntrada = Datos.Int(row, "NEDN_CANTIDADPLACAS"),
                        FechaNotaEntrada = Convert.ToDateTime(Datos.Str(row, "NEF_FECHA")), 
                        IdNotaEntrada = Datos.Int(row, "NEN_ID"),
                        IdEstatusNotaEntrada = Datos.Int(row, "ENEN_ID"),
                        OrdenCompra = new OrdenesCompra()
                        {
                            NumeroOrdenCompra = Datos.Str(row, "OCN_NUMERO"),
                            FechaOrdenCompra = Convert.ToDateTime(Datos.Str(row, "OCF_FECHA")),
                        },
                        TiposEstatusNotaEntrada = new TiposEstatusNotaEntrada()
                        {
                            EstatusNotaEntrada = Datos.Str(row, "ESTATUS1")
                        },
                        IdOrdenCompra = Datos.Int(row, "OCN_ID")
                    };

                    if (responseDB.Data != null)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = 1;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new NotasEntradasPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        } 
        public DBResponse<List<NotasEntradasPlacas_Detalle>> GetNotasEntradasPlacasDet(int IdNotaEntradaPlaca, int Entidad)
        {
            var responseDB = new DBResponse<List<NotasEntradasPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<NotasEntradasPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdNotaEntradaPlaca),
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_notasentrada_pl.consulta_notasentrada_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new NotasEntradasPlacas_Detalle()
                        {
                            IdEstatusNotaEntrada = Datos.Int(row, "ENEN_ID"), 
                            CostoPlaca = Datos.Dec(row, "NEDN_COSTOPLACA"),
                            CostoTotal = Datos.Dec(row, "NEDN_COSTOTOTAL"), 
                            CantidadNumerosPlacaIdentificada = Datos.Int(row, "NEDN_CANT_PL_IDENT"),
                            IdNotaEntrada = Datos.Int(row, "NEN_ID"),
                            CantidadPlacas = Datos.Int(row, "NEDN_CANTIDADPLACAS"),
                            CantidadNumerosPlacaPorIdentificarse = Datos.Int(row, "NEDN_CANT_PL_X_IDENT"),
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            IdNotaEntradaDetalle = Datos.Int(row, "NEDN_ID"),
                            NumeroRenglon = Datos.Int(row, "NEDN_NUM_RENGLON") 
                        };
                        responseDB.Data.Add(detalle_);
                    }

                    if (responseDB.Data.Count > 0)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = responseDB.Data.Count;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<NotasEntradasPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<NotasEntradasPlacas> GetNotasEntradasPlacasEnc_Folio(string FolioNE, int Entidad)
        {
            var responseDB = new DBResponse<NotasEntradasPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_NUMERO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, FolioNE),
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_notasentrada_pl.consulta_notasentrada_fol_enc", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    DataRow row = dt.Rows[0];
                    responseDB.Data = new NotasEntradasPlacas()
                    {
                        NumeroNotaEntrada = Datos.Int(row, "NEN_NUMERO"),
                        CantidadNumerosPlacaIdentificada = Datos.Int(row, "NEDN_CANT_PL_IDENT"),
                        CantidadNumerosPlacaPorIdentificarse = Datos.Int(row, "NEDN_CANT_PL_X_IDENT"),
                        CantidadPlacasNotaEntrada = Datos.Int(row, "NEDN_CANTIDADPLACAS"),
                        FechaNotaEntrada = Convert.ToDateTime(Datos.Str(row, "NEF_FECHA")),
                        IdNotaEntrada = Datos.Int(row, "NEN_ID"),
                        IdEstatusNotaEntrada = Datos.Int(row, "ENEN_ID"),
                        OrdenCompra = new OrdenesCompra()
                        {
                            NumeroOrdenCompra = Datos.Str(row, "OCN_NUMERO"),
                            FechaOrdenCompra = Convert.ToDateTime(Datos.Str(row, "OCF_FECHA")),
                        },
                        TiposEstatusNotaEntrada = new TiposEstatusNotaEntrada()
                        {
                            EstatusNotaEntrada = Datos.Str(row, "ESTATUS1")
                        },
                        IdOrdenCompra = Datos.Int(row, "OCN_ID")
                    };

                    if (responseDB.Data != null)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = 1;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new NotasEntradasPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<List<NotasEntradasPlacas_Detalle>> GetNotasEntradasPlacasDet_Folio(string FolioNE, int Entidad)
        {
            var responseDB = new DBResponse<List<NotasEntradasPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<NotasEntradasPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, FolioNE),
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_notasentrada_pl.consulta_notasentrada_fol_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new NotasEntradasPlacas_Detalle()
                        {
                            IdEstatusNotaEntrada = Datos.Int(row, "ENEN_ID"),
                            CostoPlaca = Datos.Dec(row, "NEDN_COSTOPLACA"),
                            CostoTotal = Datos.Dec(row, "NEDN_COSTOTOTAL"),
                            CantidadNumerosPlacaIdentificada = Datos.Int(row, "NEDN_CANT_PL_IDENT"),
                            IdNotaEntrada = Datos.Int(row, "NEN_ID"),
                            CantidadPlacas = Datos.Int(row, "NEDN_CANTIDADPLACAS"),
                            CantidadNumerosPlacaPorIdentificarse = Datos.Int(row, "NEDN_CANT_PL_X_IDENT"),
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TiposPlacas = new TiposPlacas()
                            {
                                IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                                TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA"),
                            },
                            IdNotaEntradaDetalle = Datos.Int(row, "NEDN_ID"),
                            NumeroRenglon = Datos.Int(row, "NEDN_NUM_RENGLON")
                        };
                        responseDB.Data.Add(detalle_);
                    }

                    if (responseDB.Data.Count > 0)
                    {
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = responseDB.Data.Count;
                    }
                    else
                    {
                        responseDB.ExecutionOK = false;
                        responseDB.Message = "No se encontró información";
                        responseDB.NumRows = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<NotasEntradasPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<NotasEntradasPlacas> InsertNotaEntradaPlaca(NotasEntradasPlacas notasEntradas)
        {
            var dbResponse = new DBResponse<NotasEntradasPlacas>();
            try
            {
                IList<Parameter> listNotaEntradaPlacaEnc = new List<Parameter>
                {
                    Db.CreateParameter("p_NEC_USR", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, notasEntradas.UsuarioRegistro),
                    Db.CreateParameter("p_NEN_BORRADO", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_ENEN_ID", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 2),
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, notasEntradas.IdOrdenCompra),
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, notasEntradas.IdEstatusNotaEntrada),
                    Db.CreateParameter("p_NEF_FECHA", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, notasEntradas.FechaNotaEntrada),
                    Db.CreateParameter("p_NEN_ENTIDAD", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, notasEntradas.Entidad),
                    Db.CreateParameter("p_NEN_NUMERO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, notasEntradas.NumeroNotaEntrada)
                };
                //Insertamos encabezado y obtenemos el ID de retorno
                Db.Insert("spcpl_notasentrada_pl_op.agregar_notaentrada", CommandType.StoredProcedure, listNotaEntradaPlacaEnc);
                var idNotaEntradaPlacaInserted = Convert.ToInt32(listNotaEntradaPlacaEnc[4].Value);
                notasEntradas.IdNotaEntrada = idNotaEntradaPlacaInserted;
                foreach (var detalle in notasEntradas.NotasEntradasPlacas_Detalle)
                {
                    //Inicia insercion del detalle
                    detalle.IdNotaEntrada = idNotaEntradaPlacaInserted;
                    IList<Parameter> listNotaEntradaPlacaDet = new List<Parameter>
                    {
                        Db.CreateParameter("p_ENEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdNotaEntradaDetalle),
                        Db.CreateParameter("p_NEDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, notasEntradas.Entidad),
                        Db.CreateParameter("p_NEDN_COSTOPLACA", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CostoPlaca),
                        Db.CreateParameter("p_NEDN_COSTOTOTAL", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CostoTotal),
                        Db.CreateParameter("p_NEDN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                        Db.CreateParameter("p_NEDN_CANT_PL_IDENT", DbType.Int32, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadNumerosPlacaIdentificada),
                        Db.CreateParameter("p_NEN_ID", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdNotaEntrada),
                        Db.CreateParameter("p_NEDN_CANTIDADPLACAS", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadPlacas),
                        Db.CreateParameter("p_NEDN_CANT_PL_X_IDENT", DbType.Int32, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadNumerosPlacaPorIdentificarse),
                        Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdTipoPlaca),
                        Db.CreateParameter("p_NEDN_ID", DbType.Int32, 12, ParameterDirection.Output, false, null, DataRowVersion.Default, detalle.IdNotaEntradaDetalle),
                        Db.CreateParameter("p_NEDN_NUM_RENGLON", DbType.Int32, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.NumeroRenglon)
                    };
                    Db.Insert("spcpl_notasentrada_pl_op.agregar_notaentrada_det", CommandType.StoredProcedure, listNotaEntradaPlacaDet);
                }

                dbResponse.Data = notasEntradas;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se agregó correctamente la Nota de Entrada";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> InsertNotaEntradaPlacaDetalle(NotasEntradasPlacas_Detalle detalle  )
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                  IList<Parameter> listNotaEntradaPlacaDet = new List<Parameter>
                {
                    Db.CreateParameter("p_ENEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdNotaEntradaDetalle),
                    Db.CreateParameter("p_NEDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.Entidad),
                    Db.CreateParameter("p_NEDN_COSTOPLACA", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CostoPlaca),
                    Db.CreateParameter("p_NEDN_COSTOTOTAL", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CostoTotal),
                    Db.CreateParameter("p_NEDN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_NEDN_CANT_PL_IDENT", DbType.Int32, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadNumerosPlacaIdentificada),
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdNotaEntrada),
                    Db.CreateParameter("p_NEDN_CANTIDADPLACAS", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadPlacas),
                    Db.CreateParameter("p_NEDN_CANT_PL_X_IDENT", DbType.Int32, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadNumerosPlacaPorIdentificarse),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdTipoPlaca),
                    Db.CreateParameter("p_NEDN_ID", DbType.Int32, 12, ParameterDirection.Output, false, null, DataRowVersion.Default, detalle.IdNotaEntradaDetalle),
                    Db.CreateParameter("p_NEDN_NUM_RENGLON", DbType.Int32, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.NumeroRenglon)
                };
                Db.Insert("spcpl_notasentrada_pl_op.agregar_notaentrada_det", CommandType.StoredProcedure, listNotaEntradaPlacaDet);
             
                dbResponse.Data = null;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se agregó la partida a la nota de entrada";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> DeleteNotaEntradaPlaca(int IdNotaEntradaPlaca)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdNotaEntradaPlaca)
                };

                Db.Execute("spcpl_notasentrada_pl_op.inactiva_notaentrada", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el NotaEntradaPlaca";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

    }
}

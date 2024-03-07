using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class Contratos_DA
    {
        public DBResponse<List<Contratos>> GetContratos_List(int? IdContrato, string NumeroContrato, int? IdProveedor,int Entidad)
        {
            var responseDB = new DBResponse<List<Contratos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Contratos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContrato),
                    Db.CreateParameter("p_CONC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroContrato),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_CONN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_contratos.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var Contrato = new Contratos()
                        {
                            IdContrato = Datos.Int(row, "CONN_ID"),
                            NumeroContrato = Datos.Str(row, "CONC_NUMERO")
                        };
                        responseDB.Data.Add(Contrato);
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
                responseDB.Data = new List<Contratos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<Contratos> GetContratosEnc(int IdContrato, int Entidad)
        {
            var responseDB = new DBResponse<Contratos>();
            responseDB.ExecutionOK = false;
            
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContrato),
                    Db.CreateParameter("p_CONN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_contratos.consulta_contrato_enc", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    DataRow row = dt.Rows[0];
                    responseDB.Data = new Contratos()
                    {
                        IdContrato = Datos.Int(row, "CONN_ID"),
                        NumeroContrato = Datos.Str(row, "CONC_NUMERO")
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
                responseDB.Data = new Contratos();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<List<Contratos_Detalle>> GetContratosDet(int IdContrato, int Entidad)
        {
            var responseDB = new DBResponse<List<Contratos_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Contratos_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContrato), 
                    Db.CreateParameter("p_CONN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_contratos.consulta_contrato_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new Contratos_Detalle()
                        {
                            IdContrato = Datos.Int(row, "CONN_ID"),
                            IdContratoDetalle = Datos.Int(row, "CONDN_ID"),
                            IdProveedor = Datos.Int(row, "PROVN_ID"),
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            CantidadPlacasCaja = Datos.Int(row, "CONDN_CANTIDADPLACASCAJA"),
                            CantidadPlacas = Datos.Int(row, "CONDN_CantidadPlacas"),
                            Consecutivo = index,
                            MascaraPlaca = Datos.Str(row, "CONDC_MASCARAPLACA"),
                            OficioSICT = Datos.Str(row, "CONDC_OFICIOSICT"),
                            OrdenPlaca = Datos.Str(row, "CONDC_ORDENPLACA"),
                            RangoFinal = Datos.Str(row, "CONDC_RANGOFINAL"),
                            RangoInicial = Datos.Str(row, "CONDC_RANGOINICIAL")
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
                responseDB.Data = new List<Contratos_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Contratos_Detalle> GetContratosTipoPlacaDet(int IdContrato, int IdTipoPlaca, int Entidad)
        {
            var responseDB = new DBResponse<Contratos_Detalle>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new Contratos_Detalle();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContrato),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoPlaca),
                    Db.CreateParameter("p_CONDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_contratos.consulta_contdet_tipoplaca", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    responseDB.Data = new Contratos_Detalle()
                    {                        
                        CantidadPlacasCaja = Datos.Int(row, "CONDN_CANTIDADPLACASCAJA"),
                        CantidadPlacas = Datos.Int(row, "CONDN_CANTIDADPLACAS"),  
                        RangoFinal = Datos.Str(row, "CONDC_RANGOFINAL"),
                        RangoInicial = Datos.Str(row, "CONDC_RANGOINICIAL")
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = 1;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new Contratos_Detalle();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<Contratos_Detalles_Rangos>> GetContratosDet_Rangos(int IdContratoDetalle, int Entidad)
        {
            var responseDB = new DBResponse<List<Contratos_Detalles_Rangos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<Contratos_Detalles_Rangos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContratoDetalle),
                    Db.CreateParameter("p_CONN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_contratos.consulta_contrato_det_rng", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new Contratos_Detalles_Rangos()
                        {
                            IdContratoDetalleRangos = Datos.Int(row, "CDRN_ID"), 
                            IdContratoDetalle = Datos.Int(row, "CONDN_ID"),
                            CantidadSerie = Datos.Str(row, "CDRN_CANTIDADSERIE"),
                            Consecutivo = index,
                            RangoInicial = Datos.Str(row, "CDRC_RANGOINICIAL"),
                            RangoFinal = Datos.Str(row, "CDRC_RANGOFINAL")
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
                responseDB.Data = new List<Contratos_Detalles_Rangos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Boolean> ExisteContrato(int Entidad, string Contrato)
        {
            var responseDB = new DBResponse<Boolean>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, Contrato),
                    Db.CreateParameter("p_CONN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_contratos.existe_contrato", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.Data = false;
                }
                else
                {
                    responseDB.Data = true;
                    responseDB.Message = "el Contrato ya existe en la compañía";
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = false;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<Contratos> UpsertContrato(Contratos contrato_, Boolean nRow)
        {
            var dbResponse = new DBResponse<Contratos>();
            try
            {
                IList<Parameter> listContratoEnc = new IListContratos().ParametersAgregaContratosEncabezado(contrato_);
                if (nRow)
                {
                    //Insertamos encabezado y obtenemos el ID de retorno
                   Db.Insert("spcpl_contratos_op.agregar_contrato", CommandType.StoredProcedure, listContratoEnc);
                   var idContratoInserted = Convert.ToInt32(listContratoEnc[6].Value);
                   contrato_.IdContrato = idContratoInserted;
                    foreach (var detalle in contrato_.Contratos_Detalle)
                    {
                        //Inicia insercion del detalle
                        detalle.IdContrato = idContratoInserted;
                        IList<Parameter> listContratoDet = new IListContratos().ParametersAgregaContratosDetalle(detalle);
                        Db.Insert("spcpl_contratos_op.agregar_contrato_det", CommandType.StoredProcedure, listContratoDet);
                        var IdContratoDetalleInserted = Convert.ToInt32(listContratoDet[10].Value);
                        //Si tiene rangos se agregan a la inserción
                        if (detalle.Contratos_Detalles_Rangos.Count > 0)
                        {
                            foreach (var rango in detalle.Contratos_Detalles_Rangos)
                            {
                                rango.IdContratoDetalle = IdContratoDetalleInserted;
                                IList<Parameter> listContratoDetRng = new IListContratos().ParametersAgregaContratosDetalleRangos(rango);
                                Db.Insert("spcpl_contratos_op.agregar_contrato_det_rng", CommandType.StoredProcedure, listContratoDetRng);
                            }
                        }
                    }                    

                    //Insertamos los archivos
                    foreach (var item in contrato_.Contratos_Archivos)
                    {
                        var archivos = new Archivos()
                        {
                            IdOrigen = idContratoInserted,
                            TablaOrigen = "CPL_CONTRATOS",
                            Archivo = item.Archivo,
                            Borrado = 0,
                            Entidad = contrato_.Entidad,
                            NombreArchivo = item.NombreArchivo
                        };
                        IList<Parameter> listArchivos = new IListArchivos().ParametersAgregaArchivos(archivos);
                        Db.Insert("spcpl_archivos_op.agregar_archivo", CommandType.StoredProcedure, listArchivos);
                    }
                }
                else
                {
                    Db.Update("spcpl_contratos_op.modificar_contrato", CommandType.StoredProcedure, listContratoEnc);
                    foreach (var detalle in contrato_.Contratos_Detalle)
                    {
                        //Inicia insercion del detalle
                        detalle.IdContrato = contrato_.IdContrato;
                        detalle.IdContratoDetalle = 0;
                        IList<Parameter> listContratoDet = new IListContratos().ParametersAgregaContratosDetalle(detalle);
                        Db.Insert("spcpl_contratos_op.agregar_contrato_det", CommandType.StoredProcedure, listContratoDet);
                        var IdContratoDetalleInserted = Convert.ToInt32(listContratoDet[10].Value);
                        //Si tiene rangos se agregan a la inserción
                        if (detalle.Contratos_Detalles_Rangos.Count > 0)
                        {
                            foreach (var rango in detalle.Contratos_Detalles_Rangos)
                            {
                                rango.IdContratoDetalleRangos = 0;
                                rango.IdContratoDetalle = IdContratoDetalleInserted;
                                IList<Parameter> listContratoDetRng = new IListContratos().ParametersAgregaContratosDetalleRangos(rango);
                                Db.Insert("spcpl_contratos_op.agregar_contrato_det_rng", CommandType.StoredProcedure, listContratoDetRng);
                            }
                        }
                    }
                    //Insertamos los archivos 
                    foreach (var item in contrato_.Contratos_Archivos)
                    {
                        var archivos = new Archivos()
                        {
                            IdOrigen = contrato_.IdContrato,
                            TablaOrigen = "CPL_CONTRATOS",
                            Archivo = item.Archivo,
                            Borrado = 0,
                            Entidad = contrato_.Entidad,
                            NombreArchivo = item.NombreArchivo
                        };
                        IList<Parameter> listArchivos = new IListArchivos().ParametersAgregaArchivos(archivos);
                        Db.Insert("spcpl_archivos_op.agregar_archivo", CommandType.StoredProcedure, listArchivos);
                    }
                }
                dbResponse.Data = contrato_;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente el Contrato";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<Contratos> DeleteInfoContrato(Contratos contrato_)
        {
            var dbResponse = new DBResponse<Contratos>();
            try
            {

                foreach (var detalle in contrato_.Contratos_Detalle)
                {
                    if (detalle.Contratos_Detalles_Rangos.Count > 0)
                    {
                        foreach (var rango in detalle.Contratos_Detalles_Rangos)
                        {
                            //Eliminamos el rango anterior
                            IList<Parameter> pEliminaRango = new List<Parameter>
                        {
                            Db.CreateParameter("p_CONDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, rango.IdContratoDetalle),
                        };
                            Db.Execute("spcpl_contratos_op.eliminar_contrato_det_rng", CommandType.StoredProcedure, pEliminaRango);
                        }
                    }
                }

                foreach (var detalle in contrato_.Contratos_Detalle)
                {
                    //Eliminamos el detalle anterior
                    IList<Parameter> pEliminaDetalle = new List<Parameter>
                    {
                        Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdContrato),
                    };
                    Db.Execute("spcpl_contratos_op.eliminar_contrato_det", CommandType.StoredProcedure, pEliminaDetalle);
                }

                //Insertamos los archivos
                IList<Parameter> pEliminaArchivos = new List<Parameter>
                {
                    Db.CreateParameter("p_ARN_IDORIGEN", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, contrato_.IdContrato),
                    Db.CreateParameter("p_ARC_TABLAORIGEN", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, "CPL_CONTRATOS"),
                };
                Db.Execute("spcpl_archivos_op.eliminar_archivos", CommandType.StoredProcedure, pEliminaArchivos);

                dbResponse.Data = contrato_;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> DeleteContrato(int IdContrato)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContrato)
                };

                Db.Execute("spcpl_contratos_op.inactiva_contrato", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el Contrato";
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

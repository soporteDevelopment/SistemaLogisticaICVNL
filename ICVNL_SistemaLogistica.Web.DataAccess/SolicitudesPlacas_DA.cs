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
    public class SolicitudesPlacas_DA
    {
        public DBResponse<List<SolicitudesPlacas>> GetSolicitudesPlacas_List(string FolioSolicitud, 
                                                                             DateTime FechaIniSol,
                                                                             DateTime FechaFinSol,
                                                                             DateTime FechaEntregaIniSol,
                                                                             DateTime FechaEntregaFinSol,
                                                                             int? IdProveedor,
                                                                             string NumeroContrato,
                                                                             int? NumeroOC,
                                                                             int Entidad)
        {
            var responseDB = new DBResponse<List<SolicitudesPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<SolicitudesPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_SOLC_FOLIO", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, FolioSolicitud),
                    Db.CreateParameter("p_SOLF_FECHA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaIniSol),
                    Db.CreateParameter("p_SOLF_FECHA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaFinSol),
                    Db.CreateParameter("p_SOLF_FECHAENTREGA_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaEntregaIniSol),
                    Db.CreateParameter("p_SOLF_FECHAENTREGA_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaEntregaFinSol),
                    Db.CreateParameter("P_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_CONC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroContrato),
                    Db.CreateParameter("p_OCN_NUMERO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroOC),
                    Db.CreateParameter("p_SOLN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_solicitud_pl.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var solicitudes = new SolicitudesPlacas()
                        {
                            IdSolicitud = Datos.Int(row, "SOLN_ID"),
                            FolioSolicitud = Datos.Str(row, "SOLC_FOLIO"),
                            IdProveedor = Datos.Int(row, "PROVN_ID"),
                            Proveedores = new Proveedores()
                            {
                                NombreProveedor = Datos.Str(row, "PROVC_NOMBRE"),
                            },
                            Contratos = new Contratos()
                            {
                                NumeroContrato = Datos.Str(row, "CONC_NUMERO")
                            }
                        };
                        responseDB.Data.Add(solicitudes);
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
                responseDB.Data = new List<SolicitudesPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<SolicitudesPlacas> GetSolicitudesPlacasEnc(int IdSolicitudPlaca, int Entidad)
        {
            var responseDB = new DBResponse<SolicitudesPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdSolicitudPlaca),
                    Db.CreateParameter("p_SOLN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_solicitud_pl.consulta_solicitud_enc", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    DataRow row = dt.Rows[0];
                    responseDB.Data = new SolicitudesPlacas()
                    {
                        IdSolicitud = Datos.Int(row, "SOLN_ID"),
                        FechaEntrega = DateTime.Parse(Datos.Str(row, "SOLF_FECHAENTREGA")),
                        FechaSolicitud = DateTime.Parse(Datos.Str(row, "SOLF_FECHA")), 
                        FolioSolicitud = Datos.Str(row, "SOLC_FOLIO"),
                        IdOrdenCompra = Datos.Int(row, "OCN_ID"),
                        IdContrato = Datos.Int(row, "CONTN_ID"),
                        IdProveedor = Datos.Int(row, "PROVN_ID") 
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
                responseDB.Data = new SolicitudesPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<SolicitudesPlacas> GetSolicitudesPlacasSiguienteFolio()
        {
            var responseDB = new DBResponse<SolicitudesPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_solicitud_pl.consulta_solicitud_sigfolio", CommandType.StoredProcedure, list);

                DataRow row = dt.Rows[0];
                responseDB.Data = new SolicitudesPlacas()
                {
                    FolioSolicitud = Datos.Str(row, "SiguienteFolio").PadLeft(8, '0')
                };
            }
            catch (Exception ex)
            {
                responseDB.Data = new SolicitudesPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<List<SolicitudesPlacas_Detalle>> GetSolicitudesPlacasDet(int IdSolicitudPlaca, int Entidad)
        {
            var responseDB = new DBResponse<List<SolicitudesPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<SolicitudesPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdSolicitudPlaca),
                    Db.CreateParameter("p_SOLDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_solicitud_pl.consulta_solicitud_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new SolicitudesPlacas_Detalle()
                        {
                           IdSolicitudDetalle = Datos.Int(row, "SOLDN_ID"),
                           IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                           IdSolicitud = Datos.Int(row, "SOLN_ID"),
                           IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                           CantidadPlacas = Datos.Int(row, "SOLDN_CANT_PLACAS"),
                           RangoPlacaFinal = Datos.Str(row, "SOLDC_RNG_PL_FIN"),
                           RangoPlacaInicial = Datos.Str(row, "SOLDC_RNG_PL_INI"),
                           IdDetalleOrdenCompra = Datos.Int(row, "OCDN_ID")
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
                responseDB.Data = new List<SolicitudesPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<SolicitudesPlacas> UpsertSolicitudPlaca(SolicitudesPlacas SolicitudPlaca_, Boolean nRow)
        {
            var dbResponse = new DBResponse<SolicitudesPlacas>();
            try
            {
                IList<Parameter> listSolicitudPlacaEnc = new IListSolicitudesPlacas().ParametersAgregaSolicitudPlacasEncabezado(SolicitudPlaca_);
                if (nRow)
                {
                    //Insertamos encabezado y obtenemos el ID de retorno
                    Db.Insert("spcpl_solicitud_pl_op.agregar_solicitud", CommandType.StoredProcedure, listSolicitudPlacaEnc);
                    var idSolicitudPlacaInserted = Convert.ToInt32(listSolicitudPlacaEnc[9].Value);
                    SolicitudPlaca_.IdSolicitud = idSolicitudPlacaInserted;
                    foreach (var detalle in SolicitudPlaca_.SolicitudesPlacas_Detalle)
                    {
                        //Inicia insercion del detalle
                        detalle.IdSolicitud = idSolicitudPlacaInserted;
                        IList<Parameter> listSolicitudPlacaDet = new IListSolicitudesPlacas().ParametersAgregaSolicitudPlacasDetalle(detalle);
                        Db.Insert("spcpl_solicitud_pl_op.agregar_solicitud_det", CommandType.StoredProcedure, listSolicitudPlacaDet);
                    }
                }
                else
                {
                    Db.Update("spcpl_solicitud_pl_op.agregar_solicitud", CommandType.StoredProcedure, listSolicitudPlacaEnc);
                    foreach (var detalle in SolicitudPlaca_.SolicitudesPlacas_Detalle)
                    {
                        //Inicia insercion del detalle
                        detalle.IdSolicitud = SolicitudPlaca_.IdSolicitud;
                        detalle.IdSolicitudDetalle = 0;
                        IList<Parameter> listSolicitudPlacaDet = new IListSolicitudesPlacas().ParametersAgregaSolicitudPlacasDetalle(detalle);
                        Db.Insert("spcpl_solicitud_pl_op.agregar_solicitud_det", CommandType.StoredProcedure, listSolicitudPlacaDet); 
                    } 
                }
                dbResponse.Data = SolicitudPlaca_;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente la Solicitud de Placas";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<SolicitudesPlacas> DeleteInfoSolicitudPlaca(SolicitudesPlacas SolicitudPlaca_)
        {
            var dbResponse = new DBResponse<SolicitudesPlacas>();
            try
            {

                foreach (var detalle in SolicitudPlaca_.SolicitudesPlacas_Detalle)
                {
                    //Eliminamos el detalle anterior
                    IList<Parameter> pEliminaDetalle = new List<Parameter>
                    {
                        Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdSolicitud),
                    };
                    Db.Execute("spcpl_solicitud_pl_op.eliminar_solicitud_det", CommandType.StoredProcedure, pEliminaDetalle);
                }

                dbResponse.Data = SolicitudPlaca_;
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

        public DBResponse<DBNull> DeleteSolicitudPlaca(int IdSolicitudPlaca)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdSolicitudPlaca)
                };

                Db.Execute("spcpl_solicitud_pl_op.inactiva_solicitud", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente el SolicitudPlaca";
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

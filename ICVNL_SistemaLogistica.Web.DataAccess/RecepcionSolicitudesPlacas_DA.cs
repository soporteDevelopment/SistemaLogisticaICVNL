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
    public class RecepcionSolicitudesPlacas_DA
    {
        public DBResponse<List<RecepcionSolicitudesPlacas>> GetRecepcionesSolicitudesPlacas_List(string FolioRecepcion,
                                                                                        int IdDelegacionBanco,
                                                                                        string FolioSolicitud,
                                                                                        int IdProveedor,
                                                                                        string NumeroContrato,
                                                                                        int IdOrdenCompra,
                                                                                        int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RECC_FOLIO", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, FolioRecepcion), 
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_SOLC_FOLIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, FolioSolicitud),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_CONC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumeroContrato),
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompra),
                    Db.CreateParameter("p_RECN_ENTIDAD", DbType.Int32, 2, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_listado", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        var recepcionSolicitudes = new RecepcionSolicitudesPlacas()
                        { 
                            IdRecepcion = Datos.Int(row, "RECN_ID"),
                            IdSolicitud = Datos.Int(row, "SOLN_ID"),
                            FolioRecepcion = Datos.Str(row, "RECC_FOLIO"),
                            NotaEntradaAutorizada = Datos.Str(row, "RECC_NE_AUTORIZADA"),
                            DelegacionesBancos = new DelegacionesBancos()
                            {
                                NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL"),
                            },
                            SolicitudesPlacas = new SolicitudesPlacas()
                            {
                                FolioSolicitud = Datos.Str(row, "SOLC_FOLIO"),
                                Contratos = new Contratos()
                                {
                                    NumeroContrato = Datos.Str(row, "CONC_NUMERO"),
                                    Contratos_Archivos = new List<Contratos_Archivos>(),
                                    Contratos_Detalle = new List<Contratos_Detalle>()
                                },
                                OrdenesCompra = new OrdenesCompra()
                                {
                                    NumeroOrdenCompra = Datos.Str(row, "OCN_NUMERO"),
                                    DelegacionesBancos = new DelegacionesBancos(),
                                    OrdenesCompra_Detalle = new List<OrdenesCompra_Detalle>()
                                },
                                Proveedores = new Proveedores()
                                {
                                    NombreProveedor = Datos.Str(row, "PROVC_NOMBRE")
                                },
                                SolicitudesPlacas_Detalle = new List<SolicitudesPlacas_Detalle>(),
                            }

                        };
                        responseDB.Data.Add(recepcionSolicitudes);
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
                responseDB.Data = new List<RecepcionSolicitudesPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<RecepcionSolicitudesPlacas> GetRecepcionesSolicitudesPlacasEnc(int IdRecepcionSolicitudPlaca, int Entidad)
        {
            var responseDB = new DBResponse<RecepcionSolicitudesPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdRecepcionSolicitudPlaca),
                    Db.CreateParameter("p_SOLN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_recsolicitud_pl.consulta_recsolicitud_enc", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                { 
                    responseDB.Data = new RecepcionSolicitudesPlacas()
                    {
                        NotaEntradaAutorizada = Datos.Str(row, "RECC_NE_AUTORIZADA"),
                        FolioRecepcion = Datos.Str(row, "RECC_FOLIO"),
                        IdDelegacionBanco = Datos.Int(row, "DBN_ID"), 
                        IdSolicitud = Datos.Int(row, "SOLN_ID"),
                        UsuarioRegistro = Datos.Str(row, "RECC_USR_REG"),
                        Recibida = Datos.Int(row, "RECN_RECIBIDO") == 1,
                        IdRecepcion = Datos.Int(row, "RECN_ID"),
                        Entidad = Datos.Int(row, "RECN_ENTIDAD") ,
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
                responseDB.Data = new RecepcionSolicitudesPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<RecepcionSolicitudesPlacas> GetRecepcionesSolicitudesPlacasSiguienteFolio()
        {
            var responseDB = new DBResponse<RecepcionSolicitudesPlacas>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.con_recsolicitud_sigfolio", CommandType.StoredProcedure, list);

                DataRow row = dt.Rows[0];
                responseDB.Data = new RecepcionSolicitudesPlacas()
                {
                    FolioRecepcion = Datos.Str(row, "SiguienteFolio").PadLeft(8, '0')
                };
            }
            catch (Exception ex)
            {
                responseDB.Data = new RecepcionSolicitudesPlacas();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<List<RecepcionSolicitudesPlacas_Detalle>> GetRecepcionesSolicitudesPlacasDet(int IdRecepcionSolicitudPlaca, int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas_Detalle>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas_Detalle>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdRecepcionSolicitudPlaca),
                    Db.CreateParameter("p_RECDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new RecepcionSolicitudesPlacas_Detalle()
                        {
                            IdRecepcionDetalle = Datos.Int(row, "RECDN_ID"), 
                            CantidadRecibida = Datos.Int(row, "RECDN_CANT_REC"), 
                            CantidadNotasEntradaAutorizada = Datos.Int(row, "RECDN_CANT_NE_AUT"), 
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"), 
                            CantidadSolicitadaOrdenCompra = Datos.Int(row, "RECDN_CANT_SOL_OC"),
                            IdRecepcion = Datos.Int(row, "RECN_ID"),
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
                responseDB.Data = new List<RecepcionSolicitudesPlacas_Detalle>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }      

        public DBResponse<List<RecepcionSolicitudesPlacas_Eventos>> GetRecepcionesSolicitudesPlacas_ListEventos(DateTime FechaIniQR,
                                                                                                                 DateTime FechaFinQR, 
                                                                                                                 int IdProveedor, 
                                                                                                                 int IdDelegacionBanco, 
                                                                                                                 int IdTipoPlaca, 
                                                                                                                 int IdTipoEventoRPN, 
                                                                                                                 int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas_Eventos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas_Eventos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RPEVF_FECHA_QR_INI", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaIniQR),
                    Db.CreateParameter("p_RPEVF_FECHA_QR_FIN", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, FechaFinQR),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoPlaca),
                    Db.CreateParameter("p_TERPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoEventoRPN),
                    Db.CreateParameter("p_RPEVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_lst_ev", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new RecepcionSolicitudesPlacas_Eventos()
                        {
                            IdRecepcion = Datos.Int(row, "RECN_ID"),
                            IdEventoRecepcion = Datos.Int(row, "RVALN_ID"),
                            IdSolicitud = Datos.Int(row, "SOLN_ID"),
                            FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "RPEVF_FECHA_QR")),
                            IdProveedor = Datos.Int(row, "PROVN_ID"),
                            Proveedores = new Proveedores()
                            {
                                Id = Datos.Int(row, "PROVN_ID"),
                                NombreProveedor = Datos.Str(row, "PROVC_NOMBRE")
                            },
                            NumeroCaja = Datos.Str(row, "RPEVC_NUM_CAJA"),
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            DelegacionesBancos = new DelegacionesBancos()
                            {
                                IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                                NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                            },
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TiposPlacas = new TiposPlacas()
                            {
                                IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                                TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                            },
                            CantidadLaminas = Datos.Int(row, "RPEVN_CANTIDADLAMINAS"),
                            Rangos = Datos.Str(row, "RPEVC_RANGOS"),
                            IdTiposEventosRecepcionPlacas = Datos.Int(row, "TERPN_ID"),
                            TipoEventosRecepcionPlacas = Datos.Str(row, "TERPC_EVENTOS_PL"),

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
                responseDB.Data = new List<RecepcionSolicitudesPlacas_Eventos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<RecepcionSolicitudesPlacas_Recibir>> GetRecepcionesSolicitudesPlacas_Recibir(int IdSolicitud, int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas_Recibir>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdSolicitud),
                    Db.CreateParameter("p_RPEVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_ev", CommandType.StoredProcedure, list);

                if (dt == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }
                else
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        responseDB.Data.Add(new RecepcionSolicitudesPlacas_Recibir()
                        {
                            IdRecepcion = Datos.Int(row, "RECN_ID"),
                            IdEventoRecepcion = Datos.Int(row, "RPEVN_ID"), 
                            IdSolicitud = Datos.Int(row, "SOLN_ID"),
                            FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "RPEVF_FECHA_QR")),
                            IdProveedor = Datos.Int(row, "PROVN_ID"),
                            Proveedores = new Proveedores()
                            {
                                Id = Datos.Int(row, "PROVN_ID"),
                                NombreProveedor = Datos.Str(row, "PROVC_NOMBRE")
                            },
                            NumeroCaja = Datos.Str(row, "RPEVC_NUM_CAJA"),
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            DelegacionesBancos = new DelegacionesBancos()
                            {
                                IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                                NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                            },
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TiposPlacas = new TiposPlacas()
                            {
                                IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                                TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                            },
                            CantidadLaminas = Datos.Int(row, "RPEVN_CANTIDADLAMINAS"),
                            Rangos = Datos.Str(row, "RPEVC_RANGOS")
                        });
                    }
                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = responseDB.Data.Count;
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<RecepcionSolicitudesPlacas_Eventos> GetRecepcionesSolicitudesPlacas_Evento(int IdEventoRecepcion, int Entidad)
        {
            var responseDB = new DBResponse<RecepcionSolicitudesPlacas_Eventos>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new RecepcionSolicitudesPlacas_Eventos();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RVALN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEventoRecepcion),
                    Db.CreateParameter("p_RPEVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_recsolicitud_pl.consulta_recsolicitud_evento", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }
                else
                {
                    responseDB.Data = new RecepcionSolicitudesPlacas_Eventos()
                    {
                        IdRecepcion = Datos.Int(row, "RECN_ID"),
                        IdEventoRecepcion = Datos.Int(row, "RPEVN_ID"),
                        IdSolicitud = Datos.Int(row, "SOLN_ID"),
                        FechaHoraRegistro = Convert.ToDateTime(Datos.Str(row, "RPEVF_FECHA_QR")),
                        IdProveedor = Datos.Int(row, "PROVN_ID"),
                        Proveedores = new Proveedores()
                        {
                            Id = Datos.Int(row, "PROVN_ID"),
                            NombreProveedor = Datos.Str(row, "PROVC_NOMBRE")
                        },
                        NumeroCaja = Datos.Str(row, "RPEVC_NUM_CAJA"),
                        IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                        DelegacionesBancos = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                        },
                        IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                        TiposPlacas = new TiposPlacas()
                        {
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                        },
                        CantidadLaminas = Datos.Int(row, "RPEVN_CANTIDADLAMINAS"),
                        Rangos = Datos.Str(row, "RPEVC_RANGOS"),
                        IdTiposEventosRecepcionPlacas = Datos.Int(row, "TERPN_ID"),
                        TipoEventosRecepcionPlacas = Datos.Str(row, "TERPC_EVENTOS_PL"),
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = 1;
                }
            }
            catch (Exception ex)
            {
                responseDB.Data = new RecepcionSolicitudesPlacas_Eventos();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones>> GetRecepcionesSolicitudesPlacas_Recibir_ValidacionesList(int IdEventoRecepcion, int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RPEVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEventoRecepcion),
                    Db.CreateParameter("p_RVALN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_ev_val", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new RecepcionSolicitudesPlacas_Recibir_Validaciones()
                        {
                            IdRecepcionValidaciones = Datos.Int(row, "RVALN_ID"),
                            IdRecepcion = Datos.Int(row, "RECN_ID"),
                            IdEventoRecepcion = Datos.Int(row, "RPEVN_ID"),
                            Fecha = Convert.ToDateTime(Datos.Str(row, "RVALF_FECHA_VAL")).ToString("dd/MM/YYYY"),
                            Horas = Convert.ToDateTime(Datos.Str(row, "RVALF_FECHA_VAL")).ToString("hh:mm"),
                            Usuario = new Usuarios()
                            {
                                Usuario = Datos.Str(row, "RVALC_NOMBREUSUARIO"),
                                Puesto = Datos.Str(row, "RVALC_PUESTOUSUARIO"),
                                NumeroEmpleado = Datos.Str(row, "RVALC_NUMEMPLEADO")
                            },
                            IdDelegacionesBancos = Datos.Int(row, "DBN_ID"),
                            DelegacionesBancos = new DelegacionesBancos()
                            {
                                IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                                NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                            },
                            IdProveedor = Datos.Int(row, "PROVN_ID"),
                            Proveedores = new Proveedores()
                            {
                                Id = Datos.Int(row, "PROVN_ID"),
                                NombreProveedor = Datos.Str(row, "PROVC_NOMBRE"),
                            },
                            IdContrato = Datos.Int(row, "CONN_ID"),
                            Contrato = new Contratos()
                            {
                                IdContrato = Datos.Int(row, "CONN_ID"),
                                NumeroContrato = Datos.Str(row, "CONC_NUMERO"),
                            },
                            PartidaContrato = Datos.Str(row, "RVALN_PARTIDA_CON"),
                            IdTipoProblema = Datos.Int(row, "TERPN_ID"),
                            TiposEventosRecepcionPlacas = new TiposEventosRecepcionPlacas()
                            {
                                IdTiposEventosRecepcionPlacas = Datos.Int(row, "TERPN_ID"),
                                TipoEventosRecepcionPlacas = Datos.Str(row, "TERPC_EVENTOS_PL"),
                            },
                            CajaNdeM = Datos.Str(row, "RVALC_CAJANDEM"),
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TipoPlaca = new TiposPlacas()
                            {
                                IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                                TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                            },
                            NotaEntrada = "",                           
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
                responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<RecepcionSolicitudesPlacas_Recibir_Validaciones> GetRecepcionesSolicitudesPlacas_Recibir_Validaciones(int IdValidacionEvento, int Entidad)
        {
            var responseDB = new DBResponse<RecepcionSolicitudesPlacas_Recibir_Validaciones>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new RecepcionSolicitudesPlacas_Recibir_Validaciones();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RVALN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdValidacionEvento),
                    Db.CreateParameter("p_RVALN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var row = Db.GetDataRow("spcpl_recsolicitud_pl.consulta_recsolicitud_ev_val_i", CommandType.StoredProcedure, list);

                if (row == null)
                {
                    responseDB.ExecutionOK = false;
                    responseDB.Message = "No se encontró información";
                    responseDB.NumRows = 0;
                    return responseDB;
                }
                else
                {
                    responseDB.Data = new RecepcionSolicitudesPlacas_Recibir_Validaciones()
                    {
                        IdRecepcionValidaciones = Datos.Int(row, "RVALN_ID"),
                        IdRecepcion = Datos.Int(row, "RECN_ID"),
                        IdEventoRecepcion = Datos.Int(row, "RPEVN_ID"),
                        Fecha = Convert.ToDateTime(Datos.Str(row, "RVALF_FECHA_VAL")).ToString("dd/MM/YYYY"),
                        Horas = Convert.ToDateTime(Datos.Str(row, "RVALF_FECHA_VAL")).ToString("hh:mm"),
                        Usuario = new Usuarios()
                        {
                            Usuario = Datos.Str(row, "RVALC_NOMBREUSUARIO"),
                            Puesto = Datos.Str(row, "RVALC_PUESTOUSUARIO"),
                            NumeroEmpleado = Datos.Str(row, "RVALC_NUMEMPLEADO")
                        },
                        IdDelegacionesBancos = Datos.Int(row, "DBN_ID"),
                        DelegacionesBancos = new DelegacionesBancos()
                        {
                            IdDelegacionBanco = Datos.Int(row, "DBN_ID"),
                            NombreDelegacionBanco = Datos.Str(row, "DBC_NOMBRE_DEL")
                        },
                        IdProveedor = Datos.Int(row, "PROVN_ID"),
                        Proveedores = new Proveedores()
                        {
                            Id = Datos.Int(row, "PROVN_ID"),
                            NombreProveedor = Datos.Str(row, "PROVC_NOMBRE"),
                        },
                        IdContrato = Datos.Int(row, "CONN_ID"),
                        Contrato = new Contratos()
                        {
                            IdContrato = Datos.Int(row, "CONN_ID"),
                            NumeroContrato = Datos.Str(row, "CONC_NUMERO"),
                        },
                        PartidaContrato = Datos.Str(row, "RVALN_PARTIDA_CON"),
                        IdTipoProblema = Datos.Int(row, "TERPN_ID"),
                        TiposEventosRecepcionPlacas = new TiposEventosRecepcionPlacas()
                        {
                            IdTiposEventosRecepcionPlacas = Datos.Int(row, "TERPN_ID"),
                            TipoEventosRecepcionPlacas = Datos.Str(row, "TERPC_EVENTOS_PL"),
                        },
                        CajaNdeM = Datos.Str(row, "RVALC_CAJANDEM"),
                        IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                        TipoPlaca = new TiposPlacas()
                        {
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            TipoPlaca = Datos.Str(row, "TPC_TIPOPLACA")
                        },
                        NotaEntrada = "",
                    };

                    responseDB.ExecutionOK = true;
                    responseDB.Message = "OK";
                    responseDB.NumRows = 1;
                }
            }
            catch (Exception ex)
            {
                responseDB.Data = new RecepcionSolicitudesPlacas_Recibir_Validaciones();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }


        public DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>> GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Observaciones(int IdEventoRecepcion, int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RVALN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEventoRecepcion),
                    Db.CreateParameter("p_OBSN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_ev_val_o", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones()
                        {
                            IdObservacion = Datos.Int(row, "OBSN_ID"),
                            IdRecepcionValidaciones = Datos.Int(row, "RVALN_ID"), 
                            IdEventoRecepcion = Datos.Int(row, "RPEVN_ID"),
                            Observacion = Datos.Str(row, "OBSC_OBSERVACION")
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
                responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>> GetRecepcionesSolicitudesPlacas_Recibir_Validacion_Archivos(int IdEventoRecepcion, int Entidad)
        {
            var responseDB = new DBResponse<List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>>();
            responseDB.ExecutionOK = false;
            responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RVALN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdEventoRecepcion),
                    Db.CreateParameter("p_OBSN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_ev_val_a", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    var index = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        index++;
                        var detalle_ = new RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos()
                        {
                            IdArchivo = Datos.Int(row, "ARN_ID"),
                            Archivo = Datos.ArrBytes(row, "ARB_ARCHIVO"),
                            NombreArchivo = Datos.Str(row, "ARC_ARCHIVO"),
                            Consecutivo = index
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
                responseDB.Data = new List<RecepcionSolicitudesPlacas_Recibir_Validaciones_Archivos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<RecepcionSolicitudesPlacas> UpsertRecepcionSolicitudPlaca(RecepcionSolicitudesPlacas RecepcionSolicitudPlaca_, Boolean nRow)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas>();
            try
            {
                IList<Parameter> listSolicitudPlacaEnc = new IListRecepcionSolicitudesPlacas().ParametersAgregaRecepcioSolicitudPlacasEncabezado(RecepcionSolicitudPlaca_);
                if (nRow)
                {
                    //Insertamos encabezado y obtenemos el ID de retorno
                    Db.Insert("spcpl_recsolicitud_pl_op.agregar_recepcionsol", CommandType.StoredProcedure, listSolicitudPlacaEnc);
                    var idRecepcionSolicitudPlacaInserted = Convert.ToInt32(listSolicitudPlacaEnc[8].Value);
                    RecepcionSolicitudPlaca_.IdRecepcion = idRecepcionSolicitudPlacaInserted;
                    foreach (var detalle in RecepcionSolicitudPlaca_.RecepcionSolicitudesPlacas_Detalle)
                    {
                        //Inicia insercion del detalle
                        detalle.IdRecepcion = idRecepcionSolicitudPlacaInserted;
                        detalle.IdRecepcionDetalle = 0;
                        IList<Parameter> listSolicitudPlacaDet = new IListRecepcionSolicitudesPlacas().ParametersAgregaRecepcioSolicitudPlacasDetalle(detalle);
                        Db.Insert("spcpl_recsolicitud_pl_op.agregar_recepcionsol_det", CommandType.StoredProcedure, listSolicitudPlacaDet);
                    }
                }
                else
                {
                    IList<Parameter> listSolicitudPlacaEncMod = new IListRecepcionSolicitudesPlacas().ParametersAgregaRecepcioSolicitudPlacasEncabezadoMod(RecepcionSolicitudPlaca_);
                    Db.Update("spcpl_recsolicitud_pl_op.modificar_recepcionsol", CommandType.StoredProcedure, listSolicitudPlacaEncMod);
                    foreach (var detalle in RecepcionSolicitudPlaca_.RecepcionSolicitudesPlacas_Detalle)
                    {
                        //Inicia insercion del detalle
                        detalle.IdRecepcion = RecepcionSolicitudPlaca_.IdRecepcion;
                        detalle.IdRecepcionDetalle = 0;
                        IList<Parameter> listSolicitudPlacaDet = new IListRecepcionSolicitudesPlacas().ParametersAgregaRecepcioSolicitudPlacasDetalle(detalle);
                        Db.Insert("spcpl_recsolicitud_pl_op.agregar_recepcionsol_det", CommandType.StoredProcedure, listSolicitudPlacaDet); 
                    } 
                }
                dbResponse.Data = RecepcionSolicitudPlaca_;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se " + (nRow ? "Agregó" : "modificó") + " correctamente la Recepción de la Solicitud de Placas";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<RecepcionSolicitudesPlacas> DeleteInfoReceepcionSolicitudPlaca(RecepcionSolicitudesPlacas SolicitudPlaca_)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas>();
            try
            {

                foreach (var detalle in SolicitudPlaca_.RecepcionSolicitudesPlacas_Detalle)
                {
                    //Eliminamos el detalle anterior
                    IList<Parameter> pEliminaDetalle = new List<Parameter>
                    {
                        Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdRecepcionDetalle),
                    };
                    Db.Execute("spcpl_recsolicitud_pl_op.eliminar_recsolicitud_det", CommandType.StoredProcedure, pEliminaDetalle);
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

        public DBResponse<DBNull> DeleteRecepcionSolicitudPlaca(int IdRecepcion)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdRecepcion)
                };

                Db.Execute("spcpl_recsolicitud_pl_op.inactiva_recepcionsol", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente la Recepción de la Solicitud de Placas";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> InsertaRecepcionSolicitudPlaca_Validaciones_Observacion(RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones observaciones)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OBSN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, observaciones.IdObservacion),
                    Db.CreateParameter("p_OBSC_OBSERVACION", DbType.String, 500, ParameterDirection.Input, false, null, DataRowVersion.Default, observaciones.Observacion),
                    Db.CreateParameter("p_OBSN_BORRADO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_RVALN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, observaciones.IdRecepcionValidaciones),
                    Db.CreateParameter("p_OBSN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, observaciones.Entidad),
                };

                Db.Insert("spcpl_recsolicitud_pl_op.agregar_recepcionsol_obsv", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se agregó correctamente la observación de la validación en la Recepción de la Solicitud de Placas";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> DeleteRecepcionSolicitudPlaca_Validaciones_Observacion(int IdObservacion)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OBSN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdObservacion)
                };

                Db.Execute("spcpl_recsolicitud_pl_op.eliminar_recsolicitud_obs", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
                dbResponse.Message = "Se eliminó correctamente la observación de la validación en la Recepción de la Solicitud de Placas";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<RecepcionSolicitudesPlacas_Recibir> InsertaRecepcionSolicitudPlaca_Recibir(RecepcionSolicitudesPlacas_Recibir recibir)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas_Recibir>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_RPEVN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, recibir.IdValidacionEvento),
                    Db.CreateParameter("p_RPEVN_BORRADO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_RPEVN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.Entidad),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.IdProveedor),
                    Db.CreateParameter("p_RPEVC_NUM_CAJA", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.NumeroCaja),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.IdDelegacionBanco),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.IdTipoPlaca),
                    Db.CreateParameter("p_RPEVC_RANGOS", DbType.String, 30, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.Rangos),
                    Db.CreateParameter("p_TRPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdTipoProblema),
                    Db.CreateParameter("p_RPEVF_FECHA_QR", DbType.DateTime, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.FechaHoraRegistro),
                    Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.IdRecepcion),
                    Db.CreateParameter("p_RPEVN_CANTIDADLAMINAS", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.CantidadLaminas),
                };

                Db.Insert("spcpl_recsolicitud_pl_op.agregar_recepcionsol_ev", CommandType.StoredProcedure, list);
                var idReturnInserted = Convert.ToInt32(list[0].Value);
                recibir.IdEventoRecepcion = idReturnInserted;

                IList<Parameter> list2 = new List<Parameter>
                {
                    Db.CreateParameter("p_RVALN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdRecepcionValidaciones),
                    Db.CreateParameter("p_CONTN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdContrato),
                    Db.CreateParameter("p_RVALC_PUESTOUSUARIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.Usuario.Puesto),
                    Db.CreateParameter("p_NEN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdNotaEntrada),
                    Db.CreateParameter("p_RPEVN_ID", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.IdEventoRecepcion),
                    Db.CreateParameter("p_RVALC_NOMBREUSUARIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.UsuarioNombre),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdTipoPlaca),
                    Db.CreateParameter("p_RVALC_NOTAENTRADA", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.NotaEntrada),
                    Db.CreateParameter("p_RVALC_CAJANDEM", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.CajaNdeM),
                    Db.CreateParameter("p_RVALF_FECHA_VAL", DbType.DateTime, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.Fecha),
                    Db.CreateParameter("p_TERPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdTipoProblema),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdProveedor),
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.IdDelegacionesBancos),
                    Db.CreateParameter("p_RVALN_PARTIDA_CON", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.PartidaContrato),
                    Db.CreateParameter("p_RVALC_NUMEMPLEADO", DbType.String, 10, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.RecepcionSolicitudesPlacas_Recibir_Validaciones.Usuario.NumeroEmpleado),
                    Db.CreateParameter("p_RVALN_ENTIDAD", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recibir.Entidad),
                };

                Db.Insert("spcpl_recsolicitud_pl_op.agregar_recepcionsol_ev_val", CommandType.StoredProcedure, list2);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = recibir;
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

    }
}

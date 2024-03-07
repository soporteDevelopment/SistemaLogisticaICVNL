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
    public class Controles_DDL_DA
    {
        public DBResponse<IEnumerable<dynamic>> GetDelegacionesBancos_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false; 

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_dbn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_delegaciones_bancos.consulta_delegacionbanco_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var delegacionesBancos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"), 
                            Texto = Datos.Str(row, "Texto"),  
                        };
                        _list.Add(delegacionesBancos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x=>x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<DelegacionesBancos>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetTiposPlacas_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_tpn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_tipos_placas.consulta_tiposplacas_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<TiposPlacas> _list = new List<TiposPlacas>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tiposPlacas = new TiposPlacas()
                        {
                            IdTipoPlaca = Datos.Int(row, "Valor"),
                            TipoPlaca = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tiposPlacas);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.IdTipoPlaca,
                                         Texto = nlst.TipoPlaca
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<TiposPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetTiposIDS_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_tpn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_tipos_ids.consulta_tiposids_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<TiposIDs> _list = new List<TiposIDs>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tipos = new TiposIDs()
                        {
                            Id = Datos.Int(row, "Valor"),
                            TipoID = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tipos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Id,
                                         Texto = nlst.TipoID
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<TiposIDs>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetEstados_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_esn_entidad", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_estados.consulta_estados_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<Estados> _list = new List<Estados>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tipos = new Estados()
                        {
                            Id = Datos.Int(row, "Valor"),
                            Estado = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tipos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Id,
                                         Texto = nlst.Estado
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<Estados>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetProveedores_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_proveedores.consulta_proveedor_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<Proveedores> _list = new List<Proveedores>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tipos = new Proveedores()
                        {
                            Id = Datos.Int(row, "Valor"),
                            NombreProveedor = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tipos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Id,
                                         Texto = nlst.NombreProveedor
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<Proveedores>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetContratos_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_contratos.consulta_contratos_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<Contratos> _list = new List<Contratos>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tipos = new Contratos()
                        {
                            IdContrato = Datos.Int(row, "Valor"),
                            NumeroContrato = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tipos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.IdContrato,
                                         Texto = nlst.NumeroContrato
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<Proveedores>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetContratosPorveedorSeleccionado_DDL(int IdProveedor,int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_PROVN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_contratos.consulta_contratos_prov_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<Contratos> _list = new List<Contratos>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tipos = new Contratos()
                        {
                            IdContrato = Datos.Int(row, "Valor"),
                            NumeroContrato = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tipos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.IdContrato,
                                         Texto = nlst.NumeroContrato
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<Proveedores>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetOrdenesCompraPorveedorSeleccionado_DDL(int IdProveedor, int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, IdProveedor),
                    Db.CreateParameter("p_OCN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_ordenescompra.consulta_oc_prov_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<OrdenesCompra> _list = new List<OrdenesCompra>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tipos = new OrdenesCompra()
                        {
                            IdOrdenCompra = Datos.Int(row, "Valor"),
                            NumeroOrdenCompra = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tipos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.IdOrdenCompra,
                                         Texto = nlst.NumeroOrdenCompra
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<Proveedores>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetTiposPlacasOrdenCompraSeleccionada_DDL(int IdOrdenCompra, int IdContrato, int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompra),
                    Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdContrato),
                    Db.CreateParameter("p_OCN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_ordenescompra.consulta_oc_tipoplaca_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<TiposPlacas> _list = new List<TiposPlacas>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var tiposPlacas = new TiposPlacas()
                        {
                            IdTipoPlaca = Datos.Int(row, "Valor"),
                            TipoPlaca = Datos.Str(row, "Texto"),
                        };
                        _list.Add(tiposPlacas);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.IdTipoPlaca,
                                         Texto = nlst.TipoPlaca
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = 0, Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<TiposPlacas>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        
        public DBResponse<IEnumerable<dynamic>> GetBitacoraEventos_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_BITC_EVENTO", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_bitacora.consulta_bit_instreal_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"),
                            Texto = Datos.Str(row, "Texto"),
                        };
                        _list.Add(eventos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<DynamicDDL>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetOrdenesCompra_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                { 
                    Db.CreateParameter("p_OCN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_ordenescompra.consulta_ordencompra_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"),
                            Texto = Datos.Str(row, "Texto"),
                        };
                        _list.Add(eventos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<DynamicDDL>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetDelegaciones_RecepcionSolicitudes_DDL(int IdDelegacionBanco, int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdDelegacionBanco),
                    Db.CreateParameter("p_BITC_EVENTO", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_solicitud_pl.consulta_solicitud_delbanco", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"),
                            Texto = Datos.Str(row, "Texto"),
                        };
                        _list.Add(eventos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<Proveedores>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetTiposEstatusTransferencias_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_ETN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_transf_placas_pl.consulta_tipo_transfer_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"),
                            Texto = Datos.Str(row, "Texto"),
                        };
                        _list.Add(eventos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<DynamicDDL>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposEstatusPlacas_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TEPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_transf_placas_pl.consulta_tipo_est_pl_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"),
                            Texto = Datos.Str(row, "Texto"),
                        };
                        _list.Add(eventos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<DynamicDDL>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<IEnumerable<dynamic>> GetTiposEventos_DDL(int Entidad, string TextoInicial)
        {
            var responseDB = new DBResponse<IEnumerable<dynamic>>();
            responseDB.ExecutionOK = false;

            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_TERPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_recsolicitud_pl.consulta_recsolicitud_ev_combo", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    List<DynamicDDL> _list = new List<DynamicDDL>();
                    foreach (DataRow row in dt.Rows)
                    {
                        var eventos = new DynamicDDL()
                        {
                            Valor = Datos.Str(row, "Valor"),
                            Texto = Datos.Str(row, "Texto"),
                        };
                        _list.Add(eventos);
                    }

                    if (_list.Count > 0)
                    {
                        var query = (from nlst in _list
                                     select new
                                     {
                                         Valor = nlst.Valor,
                                         Texto = nlst.Texto
                                     }).Distinct().OrderBy(x => x.Valor).ToList();

                        query.Insert(0, new { Valor = "0", Texto = TextoInicial });


                        responseDB.Data = query;
                        responseDB.ExecutionOK = true;
                        responseDB.Message = "OK";
                        responseDB.NumRows = _list.Count;
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
                responseDB.Data = new List<DynamicDDL>();
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }
    }
}

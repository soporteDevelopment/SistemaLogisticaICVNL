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
    public class OrdenesCompra_DA
    {
        public DBResponse<OrdenesCompra> GetOrdenesCompraEnc(int IdOrdenCompra, int Entidad)
        {
            var responseDB = new DBResponse<OrdenesCompra>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompra), 
                    Db.CreateParameter("p_OCN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_ordenescompra.consulta_ordencompra_enc", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new OrdenesCompra()
                    {
                        IdOrdenCompra = Datos.Int(row, "OCN_ID"),
                        IdProveedor = Datos.Int(row, "PROVN_ID"),
                        IdDelegacionRecibe = Datos.Int(row, "DBN_IDRECIBE"),
                        EntidadInfofin = Datos.Str(row, "OCC_ENTIDADINFOFIN"),
                        FechaOrdenCompra = DateTime.Parse(Datos.Str(row, "OCF_FECHA")),
                        FechaOrdenCompraAutorizada = DateTime.Parse(Datos.Str(row, "OCF_FECHA_AUT")),
                        NumeroOrdenCompra = Datos.Str(row, "OCN_NUMERO")
                    };
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = null;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<List<OrdenesCompra_Detalle>> GetOrdenesCompraDet(int IdOrdenCompra, int Entidad)
        {
            var responseDB = new DBResponse<List<OrdenesCompra_Detalle>>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompra),
                    Db.CreateParameter("p_OCDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                var dt = Db.GetDataTable("spcpl_ordenescompra.consulta_ordencompra_det", CommandType.StoredProcedure, list);

                if (dt == null)
                    return responseDB;
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new List<OrdenesCompra_Detalle>();

                    foreach (DataRow row in dt.Rows)
                    {
                        responseDB.Data.Add(new OrdenesCompra_Detalle()
                        {
                            IdOrdenCompra = Datos.Int(row, "OCN_ID"),
                            CuentaContable = Datos.Str(row, "OCDC_CUENTACONTABLE"),
                            CostoTotal = Datos.Dec(row, "OCDN_COSTOTOTAL"),
                            CostoPlaca = Datos.Dec(row, "OCDN_COSTOPLACA"), 
                            IdTipoPlaca = Datos.Int(row, "TPN_ID"),
                            RenglonNotaEntrada = Datos.Int(row, "OCDN_RENGLON_NE"), 
                            CodigoArticulo_TipoPlaca = Datos.Str(row, "OCDC_COD_ART_TP"),
                            CentroCostosAlmacen = Datos.Str(row, "OCDC_CEN_COSTOS_ALM"),
                            CantidadPiezas = Datos.Int(row, "OCDN_CANTIDADPIEZAS"),
                            IdOrdenCompraDetalle = Datos.Int(row, "OCDN_ID"),
                            FechaRecepcion_TipoPlaca = DateTime.Parse(Datos.Str(row, "OCDF_FECHA_REC_TP")),
                            RenglonOrdenCompra = Datos.Int(row, "OCDN_RENGLON")
                        });
                    }                    
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = null;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<OrdenesCompra> GetOrdenesCompraNumeroOC_Enc(string NumerOrdenCompra, int Entidad)
        {
            var responseDB = new DBResponse<OrdenesCompra>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, NumerOrdenCompra),
                    Db.CreateParameter("p_OCN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_ordenescompra.consulta_ordencompra_enc_folio", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = new OrdenesCompra()
                    {
                        IdOrdenCompra = Datos.Int(row, "OCN_ID"),
                        IdProveedor = Datos.Int(row, "PROVN_ID"),
                        IdDelegacionRecibe = Datos.Int(row, "DBN_IDRECIBE"),
                        EntidadInfofin = Datos.Str(row, "OCC_ENTIDADINFOFIN"),
                        FechaOrdenCompra = DateTime.Parse(Datos.Str(row, "OCF_FECHA")),
                        FechaOrdenCompraAutorizada = DateTime.Parse(Datos.Str(row, "OCF_FECHA_AUT")),
                        NumeroOrdenCompra = Datos.Str(row, "OCN_NUMERO")
                    };
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = null;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<int?> GetIdDetalleOrdenCompra(int IdOrdenCompra, int IdTipoPlaca, int Entidad)
        {
            var responseDB = new DBResponse<int?>();
            responseDB.ExecutionOK = false; 
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompra),
                    Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdTipoPlaca),
                    Db.CreateParameter("p_OCDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_ordenescompra.consulta_oc_tipoplaca", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = Datos.Int(row, "IdDetalleOrdenCompra");
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = null;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<int> GetCantidadOrdenCompraDetalleRenglon(int IdOrdenCompraDetalle, int Entidad)
        {
            var responseDB = new DBResponse<int>();
            responseDB.ExecutionOK = false;
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompraDetalle), 
                    Db.CreateParameter("p_OCDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, Entidad),
                    Db.CreateParameter("p_cursor_out", DbType.Object, 4, ParameterDirection.Output, false, null, DataRowVersion.Default, null, 0, 0, ObjectType.OracleDataReader)
                };

                DataRow row = Db.GetDataRow("spcpl_ordenescompra.consulta_oc_cant_det", CommandType.StoredProcedure, list);

                if (row == null)
                    return responseDB;
                else
                {
                    responseDB.ExecutionOK = true;
                    responseDB.Data = Datos.Int(row, "OCDN_CANTIDADPIEZAS");
                }

            }
            catch (Exception ex)
            {
                responseDB.Data = 0;
                responseDB.Message = ex.Message;
                responseDB.ExecutionOK = false;
            }

            return responseDB;
        }

        public DBResponse<OrdenesCompra> InsertOrdenesCompra(OrdenesCompra ordenesCompra)
        {
            var dbResponse = new DBResponse<OrdenesCompra>();
            try
            {
                IList<Parameter> listOrdenesCompraEnc = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, ordenesCompra.IdOrdenCompra),
                    Db.CreateParameter("p_OCN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                    Db.CreateParameter("p_DBN_IDRECIBE", DbType.Int32, 38, ParameterDirection.Input, true, null, DataRowVersion.Default, null),
                    Db.CreateParameter("p_OCF_FECHA", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, ordenesCompra.FechaOrdenCompra),
                    Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, ordenesCompra.IdProveedor),
                    Db.CreateParameter("p_OCN_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, ordenesCompra.NumeroOrdenCompra),
                    Db.CreateParameter("p_OCN_ENTIDAD", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                    Db.CreateParameter("p_OCF_FECHA_AUT", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, ordenesCompra.FechaOrdenCompraAutorizada),
                    Db.CreateParameter("p_OCC_ENTIDADINFOFIN", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, ordenesCompra.EntidadInfofin)
                };
                //Insertamos encabezado y obtenemos el ID de retorno
                Db.Insert("spcpl_ordenescompra_op.agrega_ordencompra", CommandType.StoredProcedure, listOrdenesCompraEnc);
                var idOrdenesCompraInserted = Convert.ToInt32(listOrdenesCompraEnc[0].Value);
                ordenesCompra.IdOrdenCompra = idOrdenesCompraInserted;
                foreach (var detalle in ordenesCompra.OrdenesCompra_Detalle)
                {
                    //Inicia insercion del detalle
                    detalle.IdOrdenCompra = idOrdenesCompraInserted;
                    IList<Parameter> listOrdenesCompraDet = new List<Parameter>
                    {
                        Db.CreateParameter("p_OCDN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, detalle.IdOrdenCompraDetalle),
                        Db.CreateParameter("p_OCN_ID", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, ordenesCompra.IdOrdenCompra),
                        Db.CreateParameter("p_OCDC_CUENTACONTABLE", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, null),
                        Db.CreateParameter("p_OCDN_COSTOTOTAL", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CostoTotal),
                        Db.CreateParameter("p_OCDN_COSTOPLACA", DbType.Decimal, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CostoPlaca),
                        Db.CreateParameter("p_OCDN_ENTIDAD", DbType.Int32, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                        Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.IdTipoPlaca),
                        Db.CreateParameter("p_OCDN_RENGLON_NE", DbType.Int32, 38, ParameterDirection.Input, true, null, DataRowVersion.Default, null),
                        Db.CreateParameter("p_OCDN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                        Db.CreateParameter("p_OCDC_COD_ART_TP", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CodigoArticulo_TipoPlaca),
                        Db.CreateParameter("p_OCDC_CEN_COSTOS_ALM", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, null),
                        Db.CreateParameter("p_OCDN_CANTIDADPIEZAS", DbType.Int32, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.CantidadPiezas),
                        Db.CreateParameter("p_OCDF_FECHA_REC_TP", DbType.Date, 12, ParameterDirection.Input, true, null, DataRowVersion.Default, null),
                        Db.CreateParameter("p_OCDN_RENGLON", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, detalle.RenglonOrdenCompra)
                    };
                    Db.Insert("spcpl_ordenescompra_op.agrega_ordencompra_det", CommandType.StoredProcedure, listOrdenesCompraDet);
                }

                dbResponse.Data = ordenesCompra;
                dbResponse.ExecutionOK = true;
                dbResponse.Message = "Se agregó correctamente la Orden de Compra";
            }
            catch (Exception ex)
            {
                dbResponse.Data = null;
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }

        public DBResponse<DBNull> UpdateOrdenesCompraEnc(int IdOrdenCompra, int IdDelegacionBanco)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompra),
                    Db.CreateParameter("p_DBN_IDRECIBE", DbType.Int32, 38, ParameterDirection.Input, true, null, DataRowVersion.Default, IdDelegacionBanco)
                };

                Db.Execute("spcpl_ordenescompra_op.actualiza_ordencompra", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
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

        public DBResponse<DBNull> UpdateOrdenesCompraDet(int IdOrdenCompraDetalle, string CuentaContable, int RenglonNE, string CentroCostosAlmacen, DateTime FechaRecepcionTP)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                IList<Parameter> list = new List<Parameter>
                {
                    Db.CreateParameter("p_OCDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, IdOrdenCompraDetalle),
                    Db.CreateParameter("p_OCDC_CUENTACONTABLE", DbType.VarNumeric, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, CuentaContable),
                    Db.CreateParameter("p_OCDN_RENGLON_NE", DbType.Int32, 38, ParameterDirection.Input, true, null, DataRowVersion.Default, RenglonNE),
                    Db.CreateParameter("p_OCDC_CEN_COSTOS_ALM", DbType.String, 50, ParameterDirection.Input, true, null, DataRowVersion.Default, CentroCostosAlmacen),
                    Db.CreateParameter("p_OCDF_FECHA_REC_TP", DbType.Date, 12, ParameterDirection.Input, true, null, DataRowVersion.Default, FechaRecepcionTP)
                };

                Db.Execute("spcpl_ordenescompra_op.actualiza_ordencompra_det", CommandType.StoredProcedure, list);


                dbResponse.ExecutionOK = true;
                dbResponse.Data = null;
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

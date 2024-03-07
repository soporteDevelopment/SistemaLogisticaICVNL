using Framework.Net.Transactions;
using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class OrdenesCompra_BL
    {
        public DBResponse<int?> GetIdDetalleOrdenCompra(int IdOrdenCompra, int IdTipoPlaca, int Entidad)
        {
            return new OrdenesCompra_DA().GetIdDetalleOrdenCompra(IdOrdenCompra, IdTipoPlaca, Entidad);
        }
        public DBResponse<int> GetCantidadOrdenCompraDetalleRenglon(int IdOrdenCompraDetalle, int Entidad)
        {
            return new OrdenesCompra_DA().GetCantidadOrdenCompraDetalleRenglon(IdOrdenCompraDetalle, Entidad);
        }
        public DBResponse<OrdenesCompra> GetOrdenesCompraEnc(int IdOrdenCompra, int Entidad)
        {
            return new OrdenesCompra_DA().GetOrdenesCompraEnc(IdOrdenCompra, Entidad);
        }
        public DBResponse<List<OrdenesCompra_Detalle>> GetOrdenesCompraDet(int IdOrdenCompra, int Entidad)
        {
            return new OrdenesCompra_DA().GetOrdenesCompraDet(IdOrdenCompra, Entidad);
        }
        public DBResponse<OrdenesCompra> GetOrdenesCompraNumeroOC_Enc(string NumerOrdenCompra, int Entidad)
        {
            return new OrdenesCompra_DA().GetOrdenesCompraNumeroOC_Enc(NumerOrdenCompra, Entidad);
        }
        public DBResponse<DBNull> UpdateOrdenesCompraEnc(int IdOrdenCompra, int IdDelegacionBanco)
        {
            var dbResponse = new DBResponse<DBNull>();
            try
            {
                using (var transaction = new TransactionDecorator())
                {
                    dbResponse = new OrdenesCompra_DA().UpdateOrdenesCompraEnc(IdOrdenCompra, IdDelegacionBanco);
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
        public DBResponse<OrdenesCompra> InsertOrdenesCompra(OrdenesCompra ordenesCompra, Usuarios usuarios)
        {
            var dbResponse = new DBResponse<OrdenesCompra>();
            try
            {
                var existsOC = new OrdenesCompra_BL().GetOrdenesCompraNumeroOC_Enc(ordenesCompra.NumeroOrdenCompra, usuarios.Entidad);
                if (existsOC.ExecutionOK)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "La Orden de Compra con Folio " + ordenesCompra.NumeroOrdenCompra + " ya existe";
                    return dbResponse;
                }

                using (var transaction = new TransactionDecorator())
                {
                    for (int i = 0; i < ordenesCompra.OrdenesCompra_Detalle.Count; i++)
                    {
                        var tipoPlaca = new TiposPlacas_BL().GetTipoPlacaCodigoInfofin(ordenesCompra.OrdenesCompra_Detalle[i].CodigoArticulo_TipoPlaca, int.Parse(ordenesCompra.EntidadInfofin));
                        if (tipoPlaca.ExecutionOK)
                        {
                            ordenesCompra.OrdenesCompra_Detalle[i].IdTipoPlaca = tipoPlaca.Data.IdTipoPlaca;
                        }
                        else
                        {
                            ordenesCompra.OrdenesCompra_Detalle.RemoveAt(i);
                        }
                    }
                    dbResponse = new OrdenesCompra_DA().InsertOrdenesCompra(ordenesCompra);
                    if (dbResponse.ExecutionOK)
                    {
                        var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                        {
                            Evento = "Inserta",
                            FechaEvento = DateTime.Now,
                            InstruccionRealizada = "Insert",
                            IP_Usuario = " ",
                            Usuario = usuarios.Usuario,
                            LugarEvento = "Proceso. Ordenes de Compora del Sistema GRP Infofin",
                            JsonObject = JsonConvert.SerializeObject(ordenesCompra),
                            Entidad = usuarios.Entidad
                        });

                        dbResponse.Message = "La Orden de Compra con Folio " + ordenesCompra.NumeroOrdenCompra + " fue insertada correctamente";


                        transaction.Complete();
                    }                    
                }
            }
            catch (Exception ex)
            {
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
                using (var transaction = new TransactionDecorator())
                {
                    dbResponse = new OrdenesCompra_DA().UpdateOrdenesCompraDet(IdOrdenCompraDetalle, CuentaContable, RenglonNE, CentroCostosAlmacen, FechaRecepcionTP);
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }
            return dbResponse;
        }
    }
}

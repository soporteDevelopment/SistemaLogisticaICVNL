using ICVNL_SistemaLogistica.API.Entities;
using ICVNL_SistemaLogistica.API.Models;
using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.ApiServices;
using ICVNL_SistemaLogistica.Web.Entities.Services;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace ICVNL_SistemaLogistica.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Inventario")]
    public class InventarioPlacasController : ApiController
    {
        [HttpPost]
        [Route("PlacasEntregadas")]
        public IHttpActionResult PlacasEntregadas(PlacasRequest _placaEntregada)
        {
            int EntidadGobierno = 0;
            var responseAPI = new RequestApi<int>();

            try
            {
                var parametrizacion_ = new Parametrizacion_BL().GetParametrizacion();
                if (parametrizacion_.ExecutionOK)
                {
                    if (String.IsNullOrEmpty(parametrizacion_.Data.ClaveEntidadGobierno))
                    {
                        responseAPI.ExecutionOK = false;
                        responseAPI.Data = 0;
                        responseAPI.Message = "No se pudo obtener la clave entidad gobierno parametrizada del Sistema Lógistica, Contacte al administrador";
                        responseAPI.NumRows = 0;

                        return Ok(responseAPI);
                    }
                    bool esNumerico = Int32.TryParse(parametrizacion_.Data.ClaveEntidadGobierno, out EntidadGobierno);
                    if (!esNumerico)
                    {
                        responseAPI.ExecutionOK = false;
                        responseAPI.Data = 0;
                        responseAPI.Message = "la clave entidad gobierno no es un númerico válido, Contacte al administrador";
                        responseAPI.NumRows = 0;
                        return Ok(responseAPI);
                    }
                }
                var validaDelegacion = new DelegacionesBancos_BL().ExisteDelegacionBancoByCentroCostos(_placaEntregada.AlmacenCentroCostos, EntidadGobierno);
                if (validaDelegacion.ExecutionOK)
                {
                    var DelegacionBanco = validaDelegacion.Data;

                    var existsPlaca = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(_placaEntregada.NumeroPlaca, DelegacionBanco.IdDelegacionBanco, EntidadGobierno);
                    if (existsPlaca.ExecutionOK)
                    {
                        var PlacasResponse_ = new PlacasResponse()
                        {
                            IdDelegacionBanco = DelegacionBanco.IdDelegacionBanco,
                            IdInventarioDetalle = existsPlaca.Data.IdInventarioDetalle,
                            FechaOperacionEntrega = _placaEntregada.FechaOperacion,
                            FechaOperacionVenta = new DateTime(1900, 01, 01),
                            PrecioVenta = _placaEntregada.PrecioVenta,
                            ReferenciaInfofin = _placaEntregada.ReferenciaInfofin,
                            TipoOperacion = "Entrega",
                            UsuarioRegistro = _placaEntregada.UsuarioRegistro,
                            IP_Usuario = _placaEntregada.IP_Usuario,
                            Entidad = EntidadGobierno
                        };

                        var insertDB = new InventarioPlacas_BL().InsertInventariosPlacas_Entrega_Venta(PlacasResponse_, existsPlaca.Data);
                        if (insertDB.ExecutionOK)
                        {
                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Insert",
                                FechaEvento = DateTime.Now,
                                Evento = "Registrar Placas Entregadas",
                                IP_Usuario = _placaEntregada.IP_Usuario,
                                Usuario = _placaEntregada.UsuarioRegistro,
                                LugarEvento = "Web Service. Placas Vendidas",
                                JsonObject = JsonConvert.SerializeObject(PlacasResponse_),
                                Entidad = 1
                            });

                            responseAPI.ExecutionOK = true;
                            responseAPI.Data = 1;
                            responseAPI.Message = "Placa reportada como entregada en el Sistema de Logistica del ICVNL";
                            responseAPI.NumRows = 1;
                            return Ok(responseAPI);
                        }
                        else
                        {
                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Error",
                                FechaEvento = DateTime.Now,
                                Evento = "Registrar Placas Entregadas",
                                IP_Usuario = _placaEntregada.IP_Usuario,
                                Usuario = _placaEntregada.UsuarioRegistro,
                                LugarEvento = "Web Service. Placas Entregadas",
                                JsonObject = JsonConvert.SerializeObject(PlacasResponse_),
                                Entidad = 1
                            });

                            responseAPI.ExecutionOK = false;
                            responseAPI.Data = 0;
                            responseAPI.Message = "Ocurrio un error al insertar la información " + insertDB.Message;
                            responseAPI.NumRows = 0;
                            return Ok(responseAPI);
                        }
                    }
                    else
                    {
                        var placasResponse = new PlacasNoInvResponse()
                        {
                            IdPlacaNoInv = 0,
                            IdDelegacionBanco = DelegacionBanco.IdDelegacionBanco,
                            DelegacionBanco = DelegacionBanco.NombreDelegacionBanco,
                            Entidad = EntidadGobierno,
                            FechaOperacionVenta = _placaEntregada.FechaOperacion,
                            NumeroPlaca = _placaEntregada.NumeroPlaca
                        };

                        var insertPlacaNoInventariada = new InventarioPlacas_BL().InsertPlacaSinInventariar(placasResponse);
                        responseAPI.ExecutionOK = true;
                        responseAPI.Data = 0;
                        responseAPI.Message = "la placa no se encuentra inventariada en la delegación/banco proporcionada, ";
                        responseAPI.NumRows = 0;
                        return Ok(responseAPI);
                    }
                }
                else
                {
                    responseAPI.ExecutionOK = false;
                    responseAPI.Data = 0;
                    responseAPI.Message = "la clave del centro de costos del almacen no existe en el catalogo de Delegaciones/Bancos, Contacte al administrador";
                    responseAPI.NumRows = 0;
                    return Ok(responseAPI);
                }

            }
            catch (Exception ex)
            {
                responseAPI.ExecutionOK = false;
                responseAPI.Data = 0;
                responseAPI.Message = "Ocurrio un error en el servicio | más información: " + ex.Message.ToString();
                responseAPI.NumRows = 0;
                return Ok(responseAPI);
            }
        }

        [HttpPost]
        [Route("PlacasVendidas")]
        public IHttpActionResult PlacasVendidas(PlacasRequest _placaEntregada)
        {
            int EntidadGobierno = 0;
            var responseAPI = new RequestApi<int>();

            try
            {
                var parametrizacion_ = new Parametrizacion_BL().GetParametrizacion();
                if (parametrizacion_.ExecutionOK)
                {
                    if (String.IsNullOrEmpty(parametrizacion_.Data.ClaveEntidadGobierno))
                    {
                        responseAPI.ExecutionOK = false;
                        responseAPI.Data = 0;
                        responseAPI.Message = "No se pudo obtener la clave entidad gobierno parametrizada del Sistema Lógistica, Contacte al administrador";
                        responseAPI.NumRows = 0;

                        return Ok(responseAPI);
                    }
                    bool esNumerico = Int32.TryParse(parametrizacion_.Data.ClaveEntidadGobierno, out EntidadGobierno);
                    if (!esNumerico)
                    {
                        responseAPI.ExecutionOK = false;
                        responseAPI.Data = 0;
                        responseAPI.Message = "la clave entidad gobierno no es un númerico válido, Contacte al administrador";
                        responseAPI.NumRows = 0;
                        return Ok(responseAPI);
                    }
                }
                var validaDelegacion = new DelegacionesBancos_BL().ExisteDelegacionBancoByCentroCostos(_placaEntregada.AlmacenCentroCostos, EntidadGobierno);
                if (validaDelegacion.ExecutionOK)
                {
                    var DelegacionBanco = validaDelegacion.Data;

                    var existsPlaca = new InventarioPlacas_BL().GetInventarioPlacas_InfoPlaca(_placaEntregada.NumeroPlaca, DelegacionBanco.IdDelegacionBanco, EntidadGobierno);
                    if (existsPlaca.ExecutionOK)
                    {
                        var PlacasResponse_ = new PlacasResponse()
                        {
                            IdDelegacionBanco = DelegacionBanco.IdDelegacionBanco,
                            IdInventarioDetalle = existsPlaca.Data.IdInventarioDetalle,
                            FechaOperacionVenta = _placaEntregada.FechaOperacion,
                            FechaOperacionEntrega = new DateTime(1900, 01, 01),
                            PrecioVenta = _placaEntregada.PrecioVenta,
                            ReferenciaInfofin = _placaEntregada.ReferenciaInfofin,
                            TipoOperacion = "Venta",
                            UsuarioRegistro = _placaEntregada.UsuarioRegistro,
                            IP_Usuario = _placaEntregada.IP_Usuario,
                            Entidad = EntidadGobierno
                        };

                        var insertDB = new InventarioPlacas_BL().InsertInventariosPlacas_Entrega_Venta(PlacasResponse_, existsPlaca.Data);
                        if (insertDB.ExecutionOK)
                        {
                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Insert",
                                FechaEvento = DateTime.Now,
                                Evento = "Registrar Placas Vendidas",
                                IP_Usuario = _placaEntregada.IP_Usuario,
                                Usuario = _placaEntregada.UsuarioRegistro,
                                LugarEvento = "Web Service. Placas Vendidas",
                                JsonObject = JsonConvert.SerializeObject(PlacasResponse_),
                                Entidad = 1
                            });

                            responseAPI.ExecutionOK = true;
                            responseAPI.Data = 1;
                            responseAPI.Message = "Placa reportada como vendida en el Sistema de Logistica del ICVNL";
                            responseAPI.NumRows = 1;
                            return Ok(responseAPI);
                        }
                        else
                        {
                            var insertaBitacora = new BitacoraEventos_BL().InsertBitacora(new BitacoraEventos()
                            {
                                InstruccionRealizada = "Error",
                                FechaEvento = DateTime.Now,
                                Evento = "Registrar Placas Vendidas",
                                IP_Usuario = _placaEntregada.IP_Usuario,
                                Usuario = _placaEntregada.UsuarioRegistro,
                                LugarEvento = "Web Service. Placas Vendidas",
                                JsonObject = JsonConvert.SerializeObject(PlacasResponse_),
                                Entidad = 1
                            });

                            responseAPI.ExecutionOK = false;
                            responseAPI.Data = 0;
                            responseAPI.Message = "Ocurrio un error al insertar la información " + insertDB.Message;
                            responseAPI.NumRows = 0;
                            return Ok(responseAPI);
                        }
                    }
                    else
                    {
                        var placasResponse = new PlacasNoInvResponse()
                        {
                            IdPlacaNoInv = 0,
                            IdDelegacionBanco = DelegacionBanco.IdDelegacionBanco,
                            DelegacionBanco = DelegacionBanco.NombreDelegacionBanco,
                            Entidad = EntidadGobierno,
                            FechaOperacionVenta = _placaEntregada.FechaOperacion,
                            NumeroPlaca = _placaEntregada.NumeroPlaca
                        };

                        var insertPlacaNoInventariada = new InventarioPlacas_BL().InsertPlacaSinInventariar(placasResponse);
                        responseAPI.ExecutionOK = true;
                        responseAPI.Data = 0;
                        responseAPI.Message = "la placa no se encuentra inventariada";
                        responseAPI.NumRows = 0;

                        return Ok(responseAPI);
                    }
                }
                else
                {
                    responseAPI.ExecutionOK = false;
                    responseAPI.Data = 0;
                    responseAPI.Message = "la clave del centro de costos del almacen no existe en el catalogo de Delegaciones/Bancos, Contacte al administrador";
                    responseAPI.NumRows = 0;
                    return Ok(responseAPI);
                }

            }
            catch (Exception ex)
            {
                responseAPI.ExecutionOK = false;
                responseAPI.Data = 0;
                responseAPI.Message = "Ocurrio un error en el servicio | más información: " + ex.Message.ToString();
                responseAPI.NumRows = 0;
                return Ok(responseAPI);
            }
        }
    }
}


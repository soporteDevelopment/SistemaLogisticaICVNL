using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using ICVNL_SistemaLogistica.Web.Services;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class ParametrizacionController : Controller
    {
        // GET: Parametrizacion
        public ActionResult Index()
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_Parametrizacion.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetParametrizacion());
        }

        public Detalle_ParametrizacionVM GetParametrizacion()
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_ParametrizacionVM parametrizacionVM = new Detalle_ParametrizacionVM();

            var objResponse = new Parametrizacion_BL().GetParametrizacion();
            if (objResponse.ExecutionOK)
            {
                parametrizacionVM += objResponse.Data;
            }
            else
            {
                parametrizacionVM += new Parametrizacion();
                parametrizacionVM.Id = 0;
            }

            return parametrizacionVM;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Detalle_ParametrizacionVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];


            if (ModelState.IsValid)
            {
                var objParametrizacion = new Parametrizacion()
                {
                    IdParametrizacion = viewModel.Id,
                    CentroCostosEntidadGobiernoPlacaReporte = viewModel.CentroCostosEntidadGobiernoPlacaReporte,
                    CentroCostosEntidadGobiernoPlacaVendida = viewModel.CentroCostosEntidadGobiernoPlacaVendida,
                    ClaveEntidadGobierno = viewModel.ClaveEntidadGobierno,
                    CuentaCostosVentaPlacaReporte = viewModel.CuentaCostosVentaPlacaReporte,
                    CuentaCostosVentaPlacaVendida = viewModel.CuentaCostosVentaPlacaVendida,
                    EmailDestinatariosNotificaPlacasVendidas = viewModel.EmailDestinatariosNotificaPlacasVendidas,
                    EmailDestinatariosSolicitudPlacas = viewModel.EmailDestinatariosSolicitudPlacas,
                    Entidad = usuarioLogin.Entidad,
                    TipoPolizaPlacaReporte = viewModel.TipoPolizaPlacaReporte,
                    TipoPolizaPlacaVendida = viewModel.TipoPolizaPlacaVendida
                };

                var responseUpsert = new Parametrizacion_BL().UpsertParametrizacion(objParametrizacion, usuarioLogin, viewModel.Id == 0);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("success", "Mensaje Sistema", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Parametrización Actualizada Correctamente ";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ValidaCuentaContable(string cuenta)
        {
            var dbResponse = new DBResponse<string>();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (usuarioLogin == null)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "No se pudo obtener la información del usuario, ¡inicie sesión al sistema nuevamente!";
                dbResponse.Data = "Ocurrio un error";
                return Json(dbResponse);
            }
            try
            {
                var _postCuentas = new PostCuenta()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Cuenta = new Entities.Services.Envio.Cuenta()
                    {
                        empresa = usuarioLogin.Entidad,
                        cuenta_sin_formato = cuenta,
                        niveles = 1,
                        tipos_cuentas = 1
                    }
                };

                var getCuentaApi = await Task.Run(() => ApiService.ApiPostCuentasContablesRequest<ResponseCuenta>(Helper.InfoApi.GetURLApi(),
                                                                                      "Api/Cuenta/",
                                                                                      "ObtieneCuenta",
                                                                                      usuarioLogin.Token.access_token,
                                                                                      _postCuentas));
                if (!getCuentaApi.ExecutionOK)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No existe la cuenta contable";
                    dbResponse.Data = "Ocurrio un error";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Cuenta Existente";
                    dbResponse.Data = "";
                    return Json(dbResponse);
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
                dbResponse.Data = "Ocurrio un error";
                return Json(dbResponse);
            }
        }
        [HttpPost]
        public async Task<JsonResult> ValidaCentroCostos(string CentroCosto)
        {
            var dbResponse = new DBResponse<string>();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (usuarioLogin == null)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "No se pudo obtener la información del usuario, ¡inicie sesión al sistema nuevamente!";
                dbResponse.Data = "Ocurrio un error";
                return Json(dbResponse);
            }
            try
            {
                var _postCentroCosto = new PostCentroDeCosto()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    CentroDeCosto = new Entities.Services.Envio.CentroDeCosto()
                    {
                        empresa = usuarioLogin.Entidad,
                        centrocostos = CentroCosto
                    }
                };

                var getCuentaApi = await Task.Run(() => ApiService.ApiPostCentroCostosRequest<ResponseCuenta>(Helper.InfoApi.GetURLApi(),
                                                                                      "Api/CentroDeCosto/",
                                                                                      "ObtieneCentroDeCosto",
                                                                                      usuarioLogin.Token.access_token,
                                                                                      _postCentroCosto));
                if (!getCuentaApi.ExecutionOK)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No existe el centro de costos";
                    dbResponse.Data = "Ocurrio un error";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Centro de costos Existente";
                    dbResponse.Data = "";
                    return Json(dbResponse);
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
                dbResponse.Data = "Ocurrio un error";
                return Json(dbResponse);
            }
        }


    }
}
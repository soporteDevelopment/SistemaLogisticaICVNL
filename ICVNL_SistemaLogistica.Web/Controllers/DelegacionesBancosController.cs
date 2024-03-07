using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.Services;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class DelegacionesBancosController : Controller
    {
        /// <summary>
        /// Despliega el listado de DelegacionBanco
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_DelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetDelegacionBanco("", new Listado_DelegacionesBancosVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_DelegacionesBancosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_DelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(GetDelegacionBanco(control, viewModel));
        }

        private Listado_DelegacionesBancosVM GetDelegacionBanco(string control, Listado_DelegacionesBancosVM viewModel)
        {
            Listado_DelegacionesBancosVM listadoVM = new Listado_DelegacionesBancosVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            int? nulleableValue = null;
            listadoVM.ListadoEstatusDDL = new Data_ControlesDDL().GetEstatusActivoInactivo(); 
            listadoVM.FiltroEstatus = (viewModel.FiltroEstatus == null ? 1 : (viewModel.FiltroEstatus == -1 ? nulleableValue : viewModel.FiltroEstatus));
            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<DelegacionesBancos>> response = new DelegacionesBancos_BL().GetDelegacionesBancos(listadoVM.FiltroEstatus, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_DelegacionesBancosModel> DelegacionesBancos = new List<Listado_DelegacionesBancosModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        DelegacionesBancos.Add(new Listado_DelegacionesBancosModel() + m);
                    }
                    listadoVM.Listado = DelegacionesBancos;
                }
            }

            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return listadoVM;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            //if (!usuarioLogin.UsuariosPermisos)
            //{
            //    return RedirectToAction("Index", "Login");
            //}

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_DelegacionesBancosVM DelegacionesBancosVM = new Detalle_DelegacionesBancosVM();

            return View(DelegacionesBancosVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detalle_DelegacionesBancosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var _postAlmacen = new PostAlmacenesListado()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Almacen = new Entities.Services.Envio.AlmacenesListado()
                    {
                        empresa = usuarioLogin.Entidad
                    }
                };

                var objAlmacen = new Almacenes();

                var getAlmacenesApi = await Task.Run(() => ApiService.ApiPostAlmacenesListadoRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURL_InventariosApi(),
                                                                                      "Api/Almacenes/",
                                                                                      "ListaAlmacenes",
                                                                                      usuarioLogin.TokenInventario.access_token,
                                                                                      _postAlmacen));

                var objListApi = ((ResponseListadoAlmacenes)getAlmacenesApi.Data).Almacenes;
                if (objListApi.Any(x => x.centro_costo == viewModel.CentroCostosInfoFin))
                {
                    objAlmacen = objListApi.Where(x => x.centro_costo == viewModel.CentroCostosInfoFin).FirstOrDefault();
                }

                var objDelegacionesBancos = new DelegacionesBancos()
                {
                    IdDelegacionBanco = 0,
                    AlmacenCentroCostos = objAlmacen.nombre,
                    CentroCostosInfoFin = viewModel.CentroCostosInfoFin,
                    NombreDelegacionBanco = viewModel.NombreDelegacionBanco,
                    Entidad = usuarioLogin.Entidad
                };

                DBResponse<string> responseValidation = new DelegacionesBancos_BL().ValidacionesDelegacionBanco(objDelegacionesBancos);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new DelegacionesBancos_BL().ExisteDelegacionBanco(objDelegacionesBancos, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", response.Message, "4", "0"); ;
                        return View(viewModel);
                    }
                }

                var responseUpsert = new DelegacionesBancos_BL().UpsertDelegacionBanco(objDelegacionesBancos, usuarioLogin, true);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.NombreDelegacionBanco = responseUpsert.Data.NombreDelegacionBanco;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }


            TempData["MensajeAIndex"] = "Delegación/Banco agregado correctamente: " + viewModel.NombreDelegacionBanco;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            //if (!usuarioLogin.UsuariosPermisos)
            //{
            //    return RedirectToAction("Index", "Login");
            //}

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_DelegacionesBancosVM DelegacionesBancosVM = new Detalle_DelegacionesBancosVM();

            var objResponse = new DelegacionesBancos_BL().GetDelegacionBanco(usuarioLogin.Entidad, (int)id.Value);
            if (objResponse.ExecutionOK)
            {
                DelegacionesBancosVM += objResponse.Data;
            }
            return View(DelegacionesBancosVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(Detalle_DelegacionesBancosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var _postAlmacen = new PostAlmacenesListado()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Almacen = new Entities.Services.Envio.AlmacenesListado()
                    {
                        empresa = usuarioLogin.Entidad
                    }
                };

                var objAlmacen = new Almacenes();

                var getAlmacenesApi = await Task.Run(() => ApiService.ApiPostAlmacenesListadoRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURL_InventariosApi(),
                                                                                      "Api/Almacenes/",
                                                                                      "ListaAlmacenes",
                                                                                      usuarioLogin.TokenInventario.access_token,
                                                                                      _postAlmacen));

                var objListApi = ((ResponseListadoAlmacenes)getAlmacenesApi.Data).Almacenes;
                if (objListApi.Any(x => x.centro_costo == viewModel.CentroCostosInfoFin))
                {
                    objAlmacen = objListApi.Where(x => x.centro_costo == viewModel.CentroCostosInfoFin).FirstOrDefault();
                }

                var objDelegacionesBancos = new DelegacionesBancos()
                {
                    IdDelegacionBanco = viewModel.Id,
                    AlmacenCentroCostos = objAlmacen.nombre,
                    CentroCostosInfoFin = viewModel.CentroCostosInfoFin,
                    NombreDelegacionBanco = viewModel.NombreDelegacionBanco,
                    Entidad = usuarioLogin.Entidad
                };

                DBResponse<string> responseValidation = new DelegacionesBancos_BL().ValidacionesDelegacionBanco(objDelegacionesBancos);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new DelegacionesBancos_BL().ExisteDelegacionBanco(objDelegacionesBancos, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", response.Message, "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new DelegacionesBancos_BL().UpsertDelegacionBanco(objDelegacionesBancos, usuarioLogin, false);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", response.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.NombreDelegacionBanco = responseUpsert.Data.NombreDelegacionBanco;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Delegación/Banco Actualizado Correctamente: " + viewModel.NombreDelegacionBanco;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ValidaDelegacionBanco(string CentroCostosDelegacion)
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
                var _postAlmacen = new PostAlmacenesListado()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Almacen = new Entities.Services.Envio.AlmacenesListado()
                    {
                        empresa = usuarioLogin.Entidad
                    }
                };

                var getAlmacenesApi = await Task.Run(() => ApiService.ApiPostAlmacenesListadoRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURL_InventariosApi(),
                                                                                      "Api/Almacenes/",
                                                                                      "ListaAlmacenes",
                                                                                      usuarioLogin.TokenInventario.access_token,
                                                                                      _postAlmacen));
                if (!getAlmacenesApi.ExecutionOK)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No existe el Almacén proprcionado";
                    dbResponse.Data = "Ocurrio un error";
                    return Json(dbResponse);
                }
                else
                {
                    var objListApi = ((ResponseListadoAlmacenes)getAlmacenesApi.Data).Almacenes;
                    if (objListApi.Any(x => x.centro_costo == CentroCostosDelegacion))
                    {
                        var objAlmacen = objListApi.Where(x => x.centro_costo == CentroCostosDelegacion).FirstOrDefault();
                        dbResponse.ExecutionOK = true;
                        dbResponse.Data = objAlmacen.nombre;
                        dbResponse.Message = "Ok";
                        return Json(dbResponse);
                    }
                    else
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Data = "error";
                        dbResponse.Message = "El Centro de Costos en Infofin de la delegación/banco no existe";
                        return Json(dbResponse);
                    }
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
        [ValidateAntiForgeryToken]
        public ActionResult CambiaEstatus(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            //if (!usuarioLogin.UsuariosPermisos)
            //{
            //    return RedirectToAction("Index", "Login");
            //}

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<DBNull> response = new DelegacionesBancos_BL().CambiaEstatusDelegacionBanco((int)id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Cambio de estatus de la Delegación/Banco realizado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }
    }
}
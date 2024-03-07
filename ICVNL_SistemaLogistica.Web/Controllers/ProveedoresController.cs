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
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        /// <summary>
        /// Despliega el listado de Proveedor
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Proveedores.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();
            TempData["FiltroNumeroProveedor"] = TempData["FiltroNumeroProveedor"]?.ToString();
            TempData["FiltroNombreProveedor"] = TempData["FiltroNombreProveedor"]?.ToString();

            return View(GetProveedor("", new Listado_ProveedoresVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_ProveedoresVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Proveedores.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetProveedor(control, viewModel));
        }

        private Listado_ProveedoresVM GetProveedor(string control, Listado_ProveedoresVM viewModel)
        {
            Listado_ProveedoresVM listadoVM = new Listado_ProveedoresVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            listadoVM.ListadoEstatusDDL = new Data_ControlesDDL().GetEstatusActivoInactivo();
            int? nulleableValue = null;
            listadoVM.FiltroNumeroProveedor = viewModel.FiltroNumeroProveedor;
            listadoVM.FiltroNombreProveedor = viewModel.FiltroNombreProveedor; 
            listadoVM.FiltroEstatus = (viewModel.FiltroEstatus == null ? 1 : (viewModel.FiltroEstatus == -1 ? nulleableValue : viewModel.FiltroEstatus));

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();
            TempData["FiltroNumeroProveedor"] = viewModel.FiltroNumeroProveedor;
            TempData["FiltroNombreProveedor"] = viewModel.FiltroNombreProveedor;
            TempData["FiltroEstatus"] = viewModel.FiltroEstatus;
            TempData["ListadoEstatusDDL"] = viewModel.ListadoEstatusDDL;

            DBResponse<List<Proveedores>> response = new Proveedores_BL().GetProveedores(listadoVM.FiltroNumeroProveedor, listadoVM.FiltroNombreProveedor, listadoVM.FiltroEstatus, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_ProveedoresModel> Proveedores = new List<Listado_ProveedoresModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        Proveedores.Add(new Listado_ProveedoresModel() + m);
                    }
                    listadoVM.Listado = Proveedores;
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
            Detalle_ProveedoresVM ProveedoresVM = new Detalle_ProveedoresVM();

            return View(ProveedoresVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Detalle_ProveedoresVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var _postProveedor = new PostProveedor()
            {
                AccessToken = usuarioLogin.Token.infofin_token,
                Proveedor = new Entities.Services.Envio.Proveedor()
                {
                    empresa = usuarioLogin.Entidad,
                    proveedor = viewModel.NumeroProveedor
                }
            };

            var getProveedorApi = await Task.Run(() => ApiService.ApiPostProveedorRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURLApi(),
                                                                                  "Api/Proveedor/",
                                                                                  "ObtieneProveedor",
                                                                                  usuarioLogin.Token.access_token,
                                                                                  _postProveedor));
            if (ModelState.IsValid)
            {

                var objProveedores = new Proveedores()
                {
                    Id = 0,
                    EmailProveedor = viewModel.EmailProveedor,
                    NombreProveedor = ((ResponseProveedor)getProveedorApi.Data).proveedor.nombre.Trim(),
                    NumeroProveedor = viewModel.NumeroProveedor,
                    Entidad = usuarioLogin.Entidad
                };


                DBResponse<string> responseValidation = new Proveedores_BL().ValidacionesProveedor(objProveedores);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }

                DBResponse<Boolean> response = new Proveedores_BL().ExisteProveedor(objProveedores, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", "El Proveedor ya existe", "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new Proveedores_BL().UpsertProveedor(objProveedores, usuarioLogin, true);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.Id = responseUpsert.Data.Id;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                viewModel.NombreProveedor = ((ResponseProveedor)getProveedorApi.Data).proveedor.nombre.Trim();
                return View(viewModel);
            }


            TempData["MensajeAIndex"] = "Proveedor agregado correctamente: " + viewModel.Id;
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
            Detalle_ProveedoresVM ProveedoresVM = new Detalle_ProveedoresVM();

            var objResponse = new Proveedores_BL().GetProveedor((int)id.Value, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                ProveedoresVM += objResponse.Data;
            }
            return View(ProveedoresVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(Detalle_ProveedoresVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var _postProveedor = new PostProveedor()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Proveedor = new Entities.Services.Envio.Proveedor()
                    {
                        empresa = usuarioLogin.Entidad,
                        proveedor = viewModel.NumeroProveedor
                    }
                };

                var getProveedorApi = await Task.Run(() => ApiService.ApiPostProveedorRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURLApi(),
                                                                                      "Api/Proveedor/",
                                                                                      "ObtieneProveedor",
                                                                                      usuarioLogin.Token.access_token,
                                                                                      _postProveedor));

                var objProveedores = new Proveedores()
                {
                    Id = viewModel.Id,
                    EmailProveedor = viewModel.EmailProveedor,
                    NombreProveedor = ((ResponseProveedor)getProveedorApi.Data).proveedor.nombre.Trim(),
                    NumeroProveedor = viewModel.NumeroProveedor,
                    Entidad = usuarioLogin.Entidad
                };


                DBResponse<string> responseValidation = new Proveedores_BL().ValidacionesProveedor(objProveedores);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }

                DBResponse<Boolean> response = new Proveedores_BL().ExisteProveedor(objProveedores, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", "El Proveedor ya existe", "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new Proveedores_BL().UpsertProveedor(objProveedores, usuarioLogin, false);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.Id = responseUpsert.Data.Id;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Proveedor Actualizado Correctamente: " + viewModel.NombreProveedor;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ValidaProveedor(string CodigoProveedor)
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
                var _postProveedor = new PostProveedor()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Proveedor = new Entities.Services.Envio.Proveedor()
                    {
                        empresa = usuarioLogin.Entidad,
                        proveedor = CodigoProveedor
                    }
                };

                var getProveedorApi = await Task.Run(() => ApiService.ApiPostProveedorRequest<ResponseListadoAlmacenes>(Helper.InfoApi.GetURLApi(),
                                                                                      "Api/Proveedor/",
                                                                                      "ObtieneProveedor",
                                                                                      usuarioLogin.Token.access_token,
                                                                                      _postProveedor));
                if (!getProveedorApi.ExecutionOK)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No existe el Número de Proveedor proprcionado";
                    dbResponse.Data = "Ocurrio un error";
                    return Json(dbResponse);
                }
                else
                {
                    var objApi = ((ResponseProveedor)getProveedorApi.Data);
                    dbResponse.ExecutionOK = true;
                    dbResponse.Data = objApi.proveedor.nombre;
                    dbResponse.Message = "Ok";
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
            DBResponse<DBNull> response = new Proveedores_BL().CambiaEstatusProveedor(id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Cambio de estatus del Proveedor realizado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }
    }
}
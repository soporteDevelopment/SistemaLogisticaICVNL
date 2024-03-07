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
    public class TiposPlacasController : Controller
    {
        /// <summary>
        /// Despliega el listado de TipoPlaca
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TiposPlacas.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetTipoPlaca("", new Listado_TiposPlacasVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_TiposPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TiposPlacas.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetTipoPlaca(control, viewModel));
        }

        private Listado_TiposPlacasVM GetTipoPlaca(string control, Listado_TiposPlacasVM viewModel)
        {
            Listado_TiposPlacasVM listadoVM = new Listado_TiposPlacasVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            listadoVM.ListadoEstatusDDL = new Data_ControlesDDL().GetEstatusActivoInactivo();
            int? nulleableValue = null;
            listadoVM.FiltroEstatus = (viewModel.FiltroEstatus == null ? 1 : (viewModel.FiltroEstatus == -1 ? nulleableValue : viewModel.FiltroEstatus));

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<TiposPlacas>> response = new TiposPlacas_BL().GetTiposPlacas(listadoVM.FiltroEstatus, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_TiposPlacasModel> TiposPlacas = new List<Listado_TiposPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        TiposPlacas.Add(new Listado_TiposPlacasModel() + m);
                    }
                    listadoVM.Listado = TiposPlacas;
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
            Detalle_TiposPlacasVM TiposPlacasVM = new Detalle_TiposPlacasVM();

            return View(TiposPlacasVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detalle_TiposPlacasVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var objTiposPlacas = new TiposPlacas()
                {
                    IdTipoPlaca = 0,
                    CantidadPlacas = viewModel.CantidadPlacas,
                    CantidadPlacasCaja = viewModel.CantidadPlacasCaja,
                    CodigoInfofin = viewModel.CodigoInfofin,
                    MascaraPlaca = viewModel.MascaraPlaca,
                    OrdenPlaca = viewModel.OrdenPlaca,
                    OrdenSeriePlaca = viewModel.OrdenSeriePlaca,
                    TipoPlaca = viewModel.TipoPlaca,
                    Entidad = usuarioLogin.Entidad
                };

                DBResponse<string> responseValidation = new TiposPlacas_BL().ValidacionesTipoPlaca(objTiposPlacas);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new TiposPlacas_BL().ExisteTipoPlaca(objTiposPlacas, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", response.Message, "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new TiposPlacas_BL().UpsertTipoPlaca(objTiposPlacas, usuarioLogin, true);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.TipoPlaca = responseUpsert.Data.TipoPlaca;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }


            TempData["MensajeAIndex"] = "Tipo de Placa agregada correctamente: " + viewModel.TipoPlaca;
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Details(int? id)
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
            Detalle_TiposPlacasVM TiposPlacasVM = new Detalle_TiposPlacasVM();

            var objResponse = new TiposPlacas_BL().GetTipoPlaca(usuarioLogin.Entidad, (int)id.Value);
            if (objResponse.ExecutionOK)
            {
                TiposPlacasVM += objResponse.Data;

                var _postArticulo = new PostArticulo()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Articulo = new Entities.Services.Envio.Articulo()
                    {
                        empresa = usuarioLogin.Entidad,
                        articulo = TiposPlacasVM.CodigoInfofin
                    }
                };

                var getArticulosApi = await Task.Run(() => ApiService.ApiPostArticuloRequest<ResponseArticulo>(Helper.InfoApi.GetURL_InventariosApi(),
                                                                      "Api/Articulos/",
                                                                      "ObtieneArticulo",
                                                                      usuarioLogin.TokenInventario.access_token,
                                                                      _postArticulo));
                if (getArticulosApi.ExecutionOK)
                {
                    var objArticulo = (ResponseArticulo)getArticulosApi.Data;
                    TiposPlacasVM.DescripcionInfofin = objArticulo.Articulo.descripcion;
                } 

            }
            return View(TiposPlacasVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Detalle_TiposPlacasVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var objTiposPlacas = new TiposPlacas()
                {
                    IdTipoPlaca = viewModel.Id,
                    CantidadPlacas = viewModel.CantidadPlacas,
                    CantidadPlacasCaja = viewModel.CantidadPlacasCaja,
                    CodigoInfofin = viewModel.CodigoInfofin,
                    MascaraPlaca = viewModel.MascaraPlaca,
                    OrdenPlaca = viewModel.OrdenPlaca,
                    OrdenSeriePlaca = viewModel.OrdenSeriePlaca,
                    TipoPlaca = viewModel.TipoPlaca,
                    Entidad = usuarioLogin.Entidad
                };

                DBResponse<string> responseValidation = new TiposPlacas_BL().ValidacionesTipoPlaca(objTiposPlacas);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new TiposPlacas_BL().ExisteTipoPlaca(objTiposPlacas, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", response.Message, "4", "0");
                        return View(viewModel);
                    }
                }
                var responseUpsert = new TiposPlacas_BL().UpsertTipoPlaca(objTiposPlacas, usuarioLogin, false);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.TipoPlaca = responseUpsert.Data.TipoPlaca;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Tipo de Placa Actualizada Correctamente: " + viewModel.TipoPlaca;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> ValidaTipoPlaca(string CodArticuloInfofin)
        {
            var dbResponse = new DBResponse<ResponseArticulo>();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (usuarioLogin == null)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "No se pudo obtener la información del usuario, ¡inicie sesión al sistema nuevamente!";
                dbResponse.Data = new ResponseArticulo();
                return Json(dbResponse);
            }
            try
            {
                var _postArticulo = new PostArticulo()
                {
                    AccessToken = usuarioLogin.Token.infofin_token,
                    Articulo = new Entities.Services.Envio.Articulo()
                    {
                        empresa = usuarioLogin.Entidad,
                        articulo = CodArticuloInfofin
                    }
                };

                var getArticulosApi = await Task.Run(() => ApiService.ApiPostArticuloRequest<ResponseArticulo>(Helper.InfoApi.GetURL_InventariosApi(),
                                                                                      "Api/Articulos/",
                                                                                      "ObtieneArticulo",
                                                                                      usuarioLogin.TokenInventario.access_token,
                                                                                      _postArticulo));
                if (!getArticulosApi.ExecutionOK)
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No existe el Artículo proprcionado";
                    dbResponse.Data = new ResponseArticulo();
                    return Json(dbResponse);
                }
                else
                {
                    var objArticulo = (ResponseArticulo)getArticulosApi.Data;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Data = objArticulo;
                    dbResponse.Message = "Ok";
                    return Json(dbResponse);
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
                dbResponse.Data = new ResponseArticulo();
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
            DBResponse<DBNull> response = new TiposPlacas_BL().CambiaEstatusTipoPlaca(id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Cambio de estatus del Tipo de Placa realizado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }
    }
}
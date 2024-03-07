using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class TiposIDSController : Controller
    {
        /// <summary>
        /// Despliega el listado de TipoID
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TiposIDS.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetTipoID("", new Listado_TiposIDSVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_TiposIDSVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_TiposIDS.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(GetTipoID(control, viewModel));
        }

        private Listado_TiposIDSVM GetTipoID(string control, Listado_TiposIDSVM viewModel)
        {
            Listado_TiposIDSVM listadoVM = new Listado_TiposIDSVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            listadoVM.ListadoEstatusDDL = new Data_ControlesDDL().GetEstatusActivoInactivo();
            int? nulleableValue = null;
            listadoVM.FiltroEstatus = (viewModel.FiltroEstatus == null ? 1 : (viewModel.FiltroEstatus == -1 ? nulleableValue : viewModel.FiltroEstatus));

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<TiposIDs>> response = new TiposIDS_BL().GetTiposIDs(listadoVM.FiltroEstatus, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_TiposIDSModel> TiposIDs = new List<Listado_TiposIDSModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        TiposIDs.Add(new Listado_TiposIDSModel() + m);
                    }
                    listadoVM.Listado = TiposIDs;
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
            Detalle_TiposIDSVM TiposIDSVM = new Detalle_TiposIDSVM();

            return View(TiposIDSVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detalle_TiposIDSVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var objTiposIDS = new TiposIDs()
                {
                    Id = 0,
                    TipoID = viewModel.TipoID,
                    Entidad = usuarioLogin.Entidad
                };

                DBResponse<string> responseValidation = new TiposIDS_BL().ValidacionesTipoID(objTiposIDS);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new TiposIDS_BL().ExisteTipoID(objTiposIDS, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", "El Tipo Id ya existe", "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new TiposIDS_BL().UpsertTipoID(objTiposIDS, usuarioLogin, true);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.TipoID = responseUpsert.Data.TipoID;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }


            TempData["MensajeAIndex"] = "Tipo de ID agregado correctamente: " + viewModel.TipoID;
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
            Detalle_TiposIDSVM TiposIDSVM = new Detalle_TiposIDSVM();

            var objResponse = new TiposIDS_BL().GetTipoID(usuarioLogin.Entidad, (int)id.Value);
            if (objResponse.ExecutionOK)
            {
                TiposIDSVM += objResponse.Data;
            }
            return View(TiposIDSVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Detalle_TiposIDSVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var objTiposIDS = new TiposIDs()
                {
                    Id = viewModel.Id,
                    TipoID = viewModel.TipoID,
                    Entidad = usuarioLogin.Entidad
                };

                DBResponse<string> responseValidation = new TiposIDS_BL().ValidacionesTipoID(objTiposIDS);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new TiposIDS_BL().ExisteTipoID(objTiposIDS, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", "El Tipo Id ya existe", "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new TiposIDS_BL().UpsertTipoID(objTiposIDS, usuarioLogin, false);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.TipoID = responseUpsert.Data.TipoID;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Tipo de ID Actualizado Correctamente: " + viewModel.TipoID;
            return RedirectToAction("Index");
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
            DBResponse<DBNull> response = new TiposIDS_BL().CambioEstatusTipoID(id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Cambio de estatus del Tipo de ID realizado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;


            return RedirectToAction("Index");
        }
    }
}
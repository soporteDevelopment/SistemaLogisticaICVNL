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
    public class EstadosController : Controller
    {
        /// <summary>
        /// Despliega el listado de Estado
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Estados.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetEstado("", new Listado_EstadosVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_EstadosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Estados.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetEstado(control, viewModel));
        }

        private Listado_EstadosVM GetEstado(string control, Listado_EstadosVM viewModel)
        {
            Listado_EstadosVM listadoVM = new Listado_EstadosVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            listadoVM.ListadoEstatusDDL = new Data_ControlesDDL().GetEstatusActivoInactivo();
            int? nulleableValue = null;
            listadoVM.FiltroEstatus = (viewModel.FiltroEstatus == null ? 1 : (viewModel.FiltroEstatus == -1 ? nulleableValue : viewModel.FiltroEstatus));

            //Se utliza TempData para guardar los valores de forma temporal
            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<Estados>> response = new Estados_BL().GetEstados(listadoVM.FiltroEstatus, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_EstadosModel> Estados = new List<Listado_EstadosModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        Estados.Add(new Listado_EstadosModel() + m);
                    }
                    listadoVM.Listado = Estados;
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
            Detalle_EstadosVM EstadosVM = new Detalle_EstadosVM();

            return View(EstadosVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detalle_EstadosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var objEstados = new Estados()
                {
                    Id = 0,
                    Estado = viewModel.Estado,
                    Entidad = usuarioLogin.Entidad
                };
                DBResponse<string> responseValidation = new Estados_BL().ValidacionesEstado(objEstados);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new Estados_BL().ExisteEstado(objEstados, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", "El Estado ya existe", "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new Estados_BL().UpsertEstado(objEstados, usuarioLogin, true);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }

                viewModel.Estado = responseUpsert.Data.Estado;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }


            TempData["MensajeAIndex"] = "Estado agregado correctamente: " + viewModel.Estado;
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
            Detalle_EstadosVM EstadosVM = new Detalle_EstadosVM();

            var objResponse = new Estados_BL().GetEstado(usuarioLogin.Entidad, (int)id.Value);
            if (objResponse.ExecutionOK)
            {
                EstadosVM += objResponse.Data;
            }
            return View(EstadosVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Detalle_EstadosVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var objEstados = new Estados()
                {
                    Id = viewModel.Id,
                    Estado = viewModel.Estado,
                    Entidad = usuarioLogin.Entidad
                };
                DBResponse<string> responseValidation = new Estados_BL().ValidacionesEstado(objEstados);
                if (responseValidation.ExecutionOK)
                {
                    this.ShowNotificacion("error", "Error", responseValidation.Message, "4", "0");
                    return View(viewModel);
                }
                DBResponse<Boolean> response = new Estados_BL().ExisteEstado(objEstados, usuarioLogin.Entidad);
                if (response.ExecutionOK)
                {
                    if (response.Data)
                    {
                        this.ShowNotificacion("error", "Error", "El Estado ya existe", "4", "0");
                        return View(viewModel);
                    }
                }

                var responseUpsert = new Estados_BL().UpsertEstado(objEstados, usuarioLogin, false);
                if (!responseUpsert.ExecutionOK || responseUpsert.Data == null)
                {
                    this.ShowNotificacion("error", "Error", responseUpsert.Message, "4", "0");
                    return View(viewModel);
                }
                viewModel.Estado = responseUpsert.Data.Estado;
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            TempData["MensajeAIndex"] = "Estado agregado correctamente: " + viewModel.Estado;
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
            DBResponse<DBNull> response = new Estados_BL().CambiaEstatusEstado(id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Cambio de estatus del Estado realizado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message; 

            return RedirectToAction("Index");
        }
    }
}
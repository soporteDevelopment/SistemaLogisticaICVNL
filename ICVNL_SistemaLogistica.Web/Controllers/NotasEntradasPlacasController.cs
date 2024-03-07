using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class NotasEntradasPlacasController : Controller
    {
        /// <summary>
        /// Despliega el listado de NotasEntradasPlaca
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            //if (!usuarioLogin.UsuariosPermisos)
            //{
            //    return RedirectToAction("Index", "Login");
            //}

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["FiltroIdNotasEntradasPlaca"] = TempData["FiltroIdNotasEntradasPlaca"] != null ? int.Parse(TempData["FiltroIdNotasEntradasPlaca"].ToString()) : 0;
            TempData["FiltroNumeroNotaEntrada"] = TempData["FiltroNumeroNotaEntrada"]?.ToString();
            TempData["FiltroNumeroOrden"] = TempData["FiltroNumeroOrden"]?.ToString();
            TempData["FiltroFechaNotaEntrada"] = TempData["FiltroFechaNotaEntrada"]?.ToString();
            TempData["FiltroFechaOrdenCompra"] = TempData["FiltroFechaOrdenCompra"]?.ToString();

            //Se obtienen los valores de los combos solo en la carga inicial de la página,
            //en los POST se mantendrán estos valores
            if (TempData["ListadoEstatusNotasEntradasDDL"] == null)
                TempData["ListadoEstatusNotasEntradasDDL"] = new Data_ControlesDDL().getEstatusNotasEntradasPlacas();

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetNotasEntradasPlaca("", new Listado_NotasEntradasPlacasVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_NotasEntradasPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            //if (!usuarioLogin.UsuariosPermisos)
            //{
            //    return RedirectToAction("Index", "Login");
            //}
            return View(GetNotasEntradasPlaca(control, viewModel));
        }

        private Listado_NotasEntradasPlacasVM GetNotasEntradasPlaca(string control, Listado_NotasEntradasPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_NotasEntradasPlacasVM listadoVM = new Listado_NotasEntradasPlacasVM();
            listadoVM.ListadoEstatusNotasEntradasDDL = new Data_ControlesDDL().getEstatusNotasEntradasPlacas();

            if (TempData["ListadoEstatusNotasEntradasDDL"] == null)
                TempData["ListadoEstatusNotasEntradasDDL"] = new Data_ControlesDDL().getEstatusNotasEntradasPlacas();


            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();
            //Se utliza TempData para guardar los valores de forma temporal
            TempData["FiltroIdEstatus"] = viewModel.FiltroIdEstatus;
            TempData["FiltroNumeroNotaEntrada"] = viewModel.FiltroNumeroNotaEntrada;
            TempData["FiltroNumeroOrden"] = viewModel.FiltroNumeroOrden;
            TempData["FiltroFechaNotaEntrada"] = viewModel.FiltroFechaNotaEntrada;
            TempData["FiltroFechaOrdenCompra"] = viewModel.FiltroFechaOrdenCompra;

            string NumeroEntrada = viewModel.FiltroNumeroNotaEntrada == null ? "" : viewModel.FiltroNumeroNotaEntrada;
            string NumeroOrden = viewModel.FiltroNumeroOrden == null ? "" : viewModel.FiltroNumeroOrden;
            int IdEstatus1 = viewModel.FiltroIdEstatus == null ? 0 : int.Parse(viewModel.FiltroIdEstatus);
            int Entidad = usuarioLogin.Entidad;


            var fechaNeInicio_ = DateTime.Parse("01/01/2000");
            var fechaNeFin_ = DateTime.Parse("01/01/2900");

            var fechaOcInicio_ = DateTime.Parse("01/01/2000");
            var fechaOcFin_ = DateTime.Parse("01/01/2900");

            if (!String.IsNullOrEmpty(viewModel.FiltroFechaNotaEntrada))
            {
                fechaNeInicio_ = DateTime.Parse(viewModel.FiltroFechaNotaEntrada.Split('-')[0].Trim());
                fechaNeFin_ = DateTime.Parse(viewModel.FiltroFechaNotaEntrada.Split('-')[1].Trim());
            }

            if (!String.IsNullOrEmpty(viewModel.FiltroFechaOrdenCompra))
            {
                fechaOcInicio_ = DateTime.Parse(viewModel.FiltroFechaOrdenCompra.Split('-')[0].Trim());
                fechaOcFin_ = DateTime.Parse(viewModel.FiltroFechaOrdenCompra.Split('-')[1].Trim());
            }

            DBResponse<List<NotasEntradasPlacas>> response = new NotasEntradasPlacas_BL().GetNotasEntradasPlacas(NumeroEntrada, fechaNeInicio_, fechaNeFin_, NumeroOrden, fechaOcInicio_, fechaOcFin_, IdEstatus1, Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_NotasEntradasPlacasModel> NotasEntradasPlacas = new List<Listado_NotasEntradasPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        NotasEntradasPlacas.Add(new Listado_NotasEntradasPlacasModel() + m);
                    }
                    listadoVM.Listado = NotasEntradasPlacas;
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
            Detalle_NotasEntradasPlacasVM NotasEntradasPlacasVM = new Detalle_NotasEntradasPlacasVM();
            NotasEntradasPlacasVM.NotasEntradasPlacas_Detalle = new List<Listado_NotasEntradasPlacasDetailsModel>();
            NotasEntradasPlacasVM.DetalleNotasEntradas = new Detalle_NotasEntradasPlacasDetailsVM();
            return View(NotasEntradasPlacasVM);
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
            Detalle_NotasEntradasPlacasVM NotasEntradasPlacasVM = new Detalle_NotasEntradasPlacasVM();
            NotasEntradasPlacasVM.NotasEntradasPlacas_Detalle = new List<Listado_NotasEntradasPlacasDetailsModel>();
            NotasEntradasPlacasVM.DetalleNotasEntradas = new Detalle_NotasEntradasPlacasDetailsVM();
            NotasEntradasPlacasVM.ListadoEstatusNotasEntradasDDL = new Data_ControlesDDL().getEstatusNotasEntradasPlacas();

            var objResponse = new NotasEntradasPlacas_BL().GetNotasEntradasPlaca((int)id.Value, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                NotasEntradasPlacasVM += objResponse.Data;
                NotasEntradasPlacasVM.IdNotaEntrada = (int)id.Value;
            }
            return View(NotasEntradasPlacasVM);
        }

        [HttpPost]
        public ActionResult FiltraDetalleNotaEntrada(int IdNotaEntrada, int IdEstatus)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Detalle_NotasEntradasPlacasVM NotasEntradasPlacasVM = new Detalle_NotasEntradasPlacasVM(); 
            var objResponse = new NotasEntradasPlacas_BL().GetNotasEntradasPlaca(IdNotaEntrada, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                NotasEntradasPlacasVM += objResponse.Data;
            }
            if (IdEstatus == 0)
            {
                return PartialView("PartialDetalleNotasEntradasPlaca", NotasEntradasPlacasVM.NotasEntradasPlacas_Detalle);
            }
            else
            {
                return PartialView("PartialDetalleNotasEntradasPlaca", NotasEntradasPlacasVM.NotasEntradasPlacas_Detalle.Where(x => x.IdEstatusNotaEntrada == IdEstatus));
            }
        }

    }
}
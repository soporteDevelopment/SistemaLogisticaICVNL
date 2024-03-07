using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Helper;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class InventarioPlacasController : Controller
    {
        /// <summary>
        /// Despliega el listado de Contrato
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_InventarioPlacas.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetConsultaPlacas("", new Listado_InventarioVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_InventarioVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_InventarioPlacas.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetConsultaPlacas(control, viewModel));
        }

        private Listado_InventarioVM GetConsultaPlacas(string control, Listado_InventarioVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            Listado_InventarioVM listadoVM = new Listado_InventarioVM();
            listadoVM.ListadoDelegaciones = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoTipoPlaca = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;


            if (TempData["ListadoTipoPlaca"] == null)
                TempData["ListadoTipoPlaca"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal 
            TempData["FiltroIdDelegacion"] = viewModel.FiltroIdDelegacion;
            TempData["FiltroIdTipoPlaca"] = viewModel.FiltroIdTipoPlaca;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            int IdDelegacion_ = viewModel.FiltroIdDelegacion == null ? 0 : int.Parse(viewModel.FiltroIdDelegacion);
            int idTipoPlaca_ = viewModel.FiltroIdTipoPlaca == null ? 0 : int.Parse(viewModel.FiltroIdTipoPlaca);

            DBResponse<List<InventarioPlacas>> response = new InventarioPlacas_BL().GetData(IdDelegacion_, idTipoPlaca_, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_InventarioPlacasModel> InventarioPlacas = new List<Listado_InventarioPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        InventarioPlacas.Add(new Listado_InventarioPlacasModel() + m);
                    }
                    listadoVM.Listado = InventarioPlacas;
                }
            }

            TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoTipoPlaca"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return listadoVM;
        }


        [HttpPost]
        public ContentResult ExportarExcelInventario(Listado_InventarioVM viewModel)
        {
            string base64 = "";
            var usuarioLogin = (Usuarios)Session["UserSC"];

            TempData["messages"] = new Dictionary<string, string[]>();
            Listado_InventarioVM listadoVM = new Listado_InventarioVM();

            int IdDelegacion_ = viewModel.FiltroIdDelegacion == null ? 0 : int.Parse(viewModel.FiltroIdDelegacion);
            int idTipoPlaca_ = viewModel.FiltroIdTipoPlaca == null ? 0 : int.Parse(viewModel.FiltroIdTipoPlaca);

            DBResponse<List<InventarioPlacas>> response = new InventarioPlacas_BL().GetData(IdDelegacion_, idTipoPlaca_, usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_InventarioPlacasModel> InventarioPlacas = new List<Listado_InventarioPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        InventarioPlacas.Add(new Listado_InventarioPlacasModel() + m);
                    }
                    listadoVM.Listado = InventarioPlacas;
                }
            }


            if (listadoVM.Listado.Count > 0)
            {
                var pathPlantillas = InfoRutasArchivos.GetInfoFilePath().DirectorioArchivos + "InventarioPlacas.xls";

                var responseGeneraArchivo = Helper.ExportacionExcel.ExportarInventarioExcel(listadoVM.Listado, pathPlantillas);
                if (!responseGeneraArchivo.ExecutionOK)
                    TempData["MensajeAIndex"] = responseGeneraArchivo.Message;

                TempData["MensajeAIndex"] = "";

                base64 = Convert.ToBase64String(responseGeneraArchivo.Data, 0, responseGeneraArchivo.Data.Length);
                return Content(base64);
            }
            else
            {
                return Content(base64);
            }
        }

    }
}
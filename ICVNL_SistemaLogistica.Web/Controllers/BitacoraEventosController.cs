using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class BitacoraEventosController : Controller
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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaBitacora.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetConsultaPlacas("", new Listado_BitacoraEventosVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_BitacoraEventosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaBitacora.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(GetConsultaPlacas(control, viewModel));
        }

        private Listado_BitacoraEventosVM GetConsultaPlacas(string control, Listado_BitacoraEventosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_BitacoraEventosVM listadoVM = new Listado_BitacoraEventosVM();
            listadoVM.ListadoInstruccionesBitacoraDDL = new Data_ControlesDDL().getInstruccionesRealizadasDDL();
            listadoVM.ListadoLugarEventoDDL = new Controles_DDL().GetBitacoraEventos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            var fechaInicio_ = DateTime.Parse("01/01/2000");
            var fechaFin_ = DateTime.Parse("01/01/2900");

            if (!String.IsNullOrEmpty(viewModel.FiltroFechaEvento))
            {
                fechaInicio_ = DateTime.Parse(viewModel.FiltroFechaEvento.Split('-')[0].Trim());
                fechaFin_ = DateTime.Parse(viewModel.FiltroFechaEvento.Split('-')[1].Trim());
            }

            var FiltroLugarEvento_ = viewModel.FiltroLugarEvento == null ? "" : viewModel.FiltroLugarEvento;
            var FiltroEvento_ = viewModel.FiltroEvento == "0" ? "" : viewModel.FiltroEvento;
            var FiltroUsuario_ = viewModel.FiltroUsuario == null ? "" : viewModel.FiltroUsuario;
            var FiltroInstruccionRealizada_ = GetInstruccionRealizada(viewModel.FiltroInstruccionRealizada == "0" ? "" : viewModel.FiltroInstruccionRealizada);
            var FiltroIP_ = viewModel.FiltroIP == null ? "" : viewModel.FiltroIP;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<BitacoraEventos>> response = new BitacoraEventos_BL().GetBitacoraEventos(fechaInicio_,
                                                                                                     fechaFin_,
                                                                                                     FiltroLugarEvento_,
                                                                                                     FiltroEvento_,
                                                                                                     FiltroUsuario_,
                                                                                                     FiltroInstruccionRealizada_,
                                                                                                     FiltroIP_,
                                                                                                     usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_BitacoraEventosModel> BitacoraEventos = new List<Listado_BitacoraEventosModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        BitacoraEventos.Add(new Listado_BitacoraEventosModel() + m);
                    }
                    listadoVM.Listado = BitacoraEventos;
                }
            }


            TempData["ListadoInstruccionesBitacoraDDL"] = new Data_ControlesDDL().getInstruccionesRealizadasDDL();
            TempData["ListadoLugarEventoDDL"] = new Controles_DDL().GetBitacoraEventos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return listadoVM;
        }

        private string GetInstruccionRealizada(string instruccion)
        {
            string retorno = "";
            switch (instruccion)
            {
                case "0":
                    retorno = "0";
                    break;
                case "1":
                    retorno = "Select";
                    break;
                case "2":
                    retorno = "Update";
                    break;
                case "3":
                    retorno = "Delete";
                    break;
                case "4":
                    retorno = "Insert";
                    break;
                default:
                    retorno = "0";
                    break;
            }
            return retorno;
        }
    }
}
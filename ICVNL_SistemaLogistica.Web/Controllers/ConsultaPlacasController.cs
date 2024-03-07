using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Entities.ViewModels;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class ConsultaPlacasController : Controller
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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetConsultaPlacas("", new Listado_ConsultaInformacionPlacasVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_ConsultaInformacionPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetConsultaPlacas(control, viewModel));
        }

        private Listado_ConsultaInformacionPlacasVM GetConsultaPlacas(string control, Listado_ConsultaInformacionPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_ConsultaInformacionPlacasVM listadoVM = new Listado_ConsultaInformacionPlacasVM();
            listadoVM = viewModel;
            listadoVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal
            TempData["FiltroFechaNotaEntrada"] = listadoVM.FiltroFechaNotaEntrada;
            TempData["FiltroFechaOrdenCompra"] = listadoVM.FiltroFechaOrdenCompra;
            TempData["FiltroIdDelegacion"] = listadoVM.FiltroIdDelegacion;
            TempData["FiltroNumeroNotaEntrada"] = listadoVM.FiltroNumeroNotaEntrada;
            TempData["FiltroNumeroOrdenCompra"] = listadoVM.FiltroNumeroOrdenCompra;
            TempData["FiltroNumeroPlaca"] = listadoVM.FiltroNumeroPlaca;

            string NumeroEntrada = viewModel.FiltroNumeroNotaEntrada == null ? "" : viewModel.FiltroNumeroNotaEntrada;
            string NumeroOrden = viewModel.FiltroNumeroOrdenCompra == null ? "" : viewModel.FiltroNumeroOrdenCompra; 
            int Entidad = usuarioLogin.Entidad;
            string NumeroPlaca = viewModel.FiltroNumeroPlaca == null ? "" : viewModel.FiltroNumeroPlaca;
            int IdDelegacion = viewModel.FiltroIdDelegacion == null ? 0 : int.Parse(viewModel.FiltroIdDelegacion);


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

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<ConsultaInformacionPlacas>> response = new ConsultaPlacas_BL().GetConsultaInformacionPlacas_List(NumeroEntrada, 
                                                                                                                             fechaNeInicio_,
                                                                                                                             fechaNeFin_,
                                                                                                                             NumeroOrden,
                                                                                                                             fechaOcInicio_,
                                                                                                                             fechaOcFin_,
                                                                                                                             IdDelegacion,
                                                                                                                             Entidad,
                                                                                                                             NumeroPlaca);
            if (response.ExecutionOK)
            {
                List<Listado_ConsultaPlacasModel> ConsultaInformacionPlacas = new List<Listado_ConsultaPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        ConsultaInformacionPlacas.Add(new Listado_ConsultaPlacasModel() + m);
                    }
                    listadoVM.Listado = ConsultaInformacionPlacas;
                }
            }

            TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

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
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.ConsultaInformacionDetalladaPlaca)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_ConsultaInformacionPlacasVM ConsultaInformacionPlacasVM = new Detalle_ConsultaInformacionPlacasVM();
            var objResponse = new ConsultaPlacas_BL().GetConsultaInformacionPlaca(id, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                ConsultaInformacionPlacasVM += objResponse.Data;
            }
            return View(ConsultaInformacionPlacasVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ColocaDisponible(string id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.ColocarPlacasDisponibles)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == "")
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var _cambioEstatus = new Placas_CambioEstatus()
            {
                NumeroPlaca = id,
                Entidad = usuarioLogin.Entidad,
                Estatus = 1,
                DescEstatus = "Disponible"
            };
            DBResponse<DBNull> response = new ConsultaPlacas_BL().ConsultaCambiaEstatusPlaca(_cambioEstatus, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = response.Message;
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ColocaObsoleta(string id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.ColocarPlacasObsoleta)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == "")
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var _cambioEstatus = new Placas_CambioEstatus()
            {
                NumeroPlaca = id,
                Entidad = usuarioLogin.Entidad,
                Estatus = 2,
                DescEstatus = "Obsoleta"
            };
            DBResponse<DBNull> response = new ConsultaPlacas_BL().ConsultaCambiaEstatusPlaca(_cambioEstatus, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = response.Message;
            else
                TempData["MensajeAIndex"] = response.Message;


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ColocaRechazada(string id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.ColocarPlacaRechazada)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == "")
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var _cambioEstatus = new Placas_CambioEstatus()
            {
                NumeroPlaca = id,
                Entidad = usuarioLogin.Entidad,
                Estatus = 3,
                DescEstatus = "Rechazada"
            };
            DBResponse<DBNull> response = new ConsultaPlacas_BL().ConsultaCambiaEstatusPlaca(_cambioEstatus, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = response.Message;
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ColocaPerdida(string id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_ConsultaInformacionPlacas.ColocarPlacasPerdida)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id == "")
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var _cambioEstatus = new Placas_CambioEstatus()
            {
                NumeroPlaca = id,
                Entidad = usuarioLogin.Entidad,
                Estatus = 4,
                DescEstatus = "Perdida"
            };
            DBResponse<DBNull> response = new ConsultaPlacas_BL().ConsultaCambiaEstatusPlaca(_cambioEstatus, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = response.Message;
            else
                TempData["MensajeAIndex"] = response.Message;

            return RedirectToAction("Index");
        }


    }
}
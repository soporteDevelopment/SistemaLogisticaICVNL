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
    public class SolicitudesPlacasRecepcionEventosController : Controller
    {
        /// <summary>
        /// Despliega el listado de SolicitudesPlacasRecepcion
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaConsultaEvento)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción 
            TempData["FiltroFechaQR"] = TempData["FiltroFechaQR"]?.ToString();
            TempData["FiltroIdProveedor"] = TempData["FiltroIdProveedor"] != null ? int.Parse(TempData["FiltroIdProveedor"].ToString()) : 0;
            TempData["FiltroIdDelegacionBanco"] = TempData["FiltroIdDelegacionBanco"] != null ? int.Parse(TempData["FiltroIdDelegacionBanco"].ToString()) : 0;
            TempData["FiltroIdTipoPlaca"] = TempData["FiltroIdTipoPlaca"] != null ? int.Parse(TempData["FiltroIdTipoPlaca"].ToString()) : 0;
            TempData["FiltroIdTiposEventosRecepcionPlacas"] = TempData["FiltroIdTiposEventosRecepcionPlacas"] != null ? int.Parse(TempData["FiltroIdTiposEventosRecepcionPlacas"].ToString()) : 0;



            //Se obtienen los valores de los combos solo en la carga inicial de la página,
            //en los POST se mantendrán estos valores
            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTipoEstructuraDDL"] == null)
                TempData["ListadoTipoEstructuraDDL"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTiposEventosRecepcionPlacasDDL"] == null)
                TempData["ListadoTiposEventosRecepcionPlacasDDL"] = new Data_ControlesDDL().getTipoEventos();


            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetEventosSolicitudPlacas("", new Listado_RecepcionSolicitudesPlacas_EventosVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_RecepcionSolicitudesPlacas_EventosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaConsultaEvento)
            {
                return RedirectToAction("Index", "Login");
            }

            return View(GetEventosSolicitudPlacas(control, viewModel));
        }

        private Listado_RecepcionSolicitudesPlacas_EventosVM GetEventosSolicitudPlacas(string control, Listado_RecepcionSolicitudesPlacas_EventosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_RecepcionSolicitudesPlacas_EventosVM listadoVM = new Listado_RecepcionSolicitudesPlacas_EventosVM();

            listadoVM = viewModel;
            listadoVM.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoDelegacionesBancosDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoTipoEstructuraDDL = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoTiposEventosRecepcionPlacasDDL = new Controles_DDL().GetTiposEventos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTipoEstructuraDDL"] == null)
                TempData["ListadoTipoEstructuraDDL"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTiposEventosRecepcionPlacasDDL"] == null)
                TempData["ListadoTiposEventosRecepcionPlacasDDL"] = new Controles_DDL().GetTiposEventos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal 
            TempData["FiltroFechaQR"] = listadoVM.FiltroFechaQR;
            TempData["FiltroIdProveedor"] = listadoVM.FiltroIdProveedor;
            TempData["FiltroIdDelegacionBanco"] = listadoVM.FiltroIdDelegacionBanco;
            TempData["FiltroIdTipoPlaca"] = listadoVM.FiltroIdTipoPlaca;
            TempData["FiltroIdTiposEventosRecepcionPlacas"] = listadoVM.FiltroIdTiposEventosRecepcionPlacas;

            var fechaInicio_ = DateTime.Parse("01/01/2000");
            var fechaFin_ = DateTime.Parse("01/01/2900");
            if (!String.IsNullOrEmpty(viewModel.FiltroFechaQR))
            {
                fechaInicio_ = DateTime.Parse(viewModel.FiltroFechaQR.Split('-')[0].Trim());
                fechaFin_ = DateTime.Parse(viewModel.FiltroFechaQR.Split('-')[1].Trim());
            }
            int IdProveedor_ = listadoVM.FiltroIdProveedor == null ? 0 : int.Parse(listadoVM.FiltroIdProveedor);
            int IdDelegacionBanco_ = listadoVM.FiltroIdDelegacionBanco == null ? 0 : int.Parse(listadoVM.FiltroIdDelegacionBanco);
            int IdTipoPlaca_ = listadoVM.FiltroIdTipoPlaca == null ? 0 : int.Parse(listadoVM.FiltroIdTipoPlaca);
            int IdTiposEventosRecepcionPlacas_ = listadoVM.FiltroIdTiposEventosRecepcionPlacas == null ? 0 : int.Parse(listadoVM.FiltroIdTiposEventosRecepcionPlacas);
            int Entidad = usuarioLogin.Entidad;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            var response = new SolicitudesPlacasRecepcion_BL().GetRecepcionesSolicitudesPlacas_ListEventos(fechaInicio_, fechaFin_, IdProveedor_, IdDelegacionBanco_, IdTipoPlaca_, IdTiposEventosRecepcionPlacas_, Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_RecepcionSolicitudesPlacas_EventosModel> SolicitudesPlacasRecepcion = new List<Listado_RecepcionSolicitudesPlacas_EventosModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        SolicitudesPlacasRecepcion.Add(new Listado_RecepcionSolicitudesPlacas_EventosModel() + m);
                    }
                    listadoVM.Listado = SolicitudesPlacasRecepcion;
                }
            }

            TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoTipoEstructuraDDL"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoTiposEventosRecepcionPlacasDDL"] = new Controles_DDL().GetTiposEventos_DDL(usuarioLogin.Entidad, "Seleccione").Data;


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
        public ActionResult Details(int? IdEventoRecepcion)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaConsultaEventoDetalle)
            {
                return RedirectToAction("Index", "Login");
            }

            if (IdEventoRecepcion == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_RecepcionSolicitudesPlacas_EventosVM  SolicitudesPlacasRecepcionVM = new Detalle_RecepcionSolicitudesPlacas_EventosVM();
             

            var objResponse = new SolicitudesPlacasRecepcion_BL().GetRecepcionesSolicitudesPlacas_Evento((int)IdEventoRecepcion.Value, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                SolicitudesPlacasRecepcionVM += objResponse.Data;
            }
            return View(SolicitudesPlacasRecepcionVM);
        }

        [HttpPost]
        public JsonResult DescargarArchivo(int IdArchivo)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var responseArchivo = new DBResponse<Listado_ContratosArchivosModel>();
            responseArchivo.Data = new Listado_ContratosArchivosModel();
            try
            {
                var dbResponse = new Archivos_BL().GetArchivo(IdArchivo, usuarioLogin.Entidad);
                if (dbResponse.Data.Archivo != null)
                {
                    responseArchivo.Data.ArchivoBase64 = Convert.ToBase64String(dbResponse.Data.Archivo);
                }
                responseArchivo.Data.NombreArchivo = dbResponse.Data.NombreArchivo;
                responseArchivo.ExecutionOK = dbResponse.ExecutionOK;
            }
            catch (Exception ex)
            {
                responseArchivo.ExecutionOK = false;
                responseArchivo.Message = ex.Message;
            }

            return Json(responseArchivo);
        }
    }
}
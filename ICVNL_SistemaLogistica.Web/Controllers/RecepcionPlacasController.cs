using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class RecepcionPlacasController : Controller
    {
        /// <summary>
        /// Despliega el listado de TransferenciaPlaca
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionPlacasEntreDelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }
            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["FiltroFolioTransferencia"] = TempData["FiltroNumeroTransferenciaPlaca"]?.ToString();
            TempData["FiltroFechaTransferencia"] = TempData["FiltroFechaTransferencia"]?.ToString();
            TempData["FiltroNombrePersona"] = TempData["FiltroNombrePersona"]?.ToString();
            TempData["FiltroApellidoPersona"] = TempData["FiltroApellidoPersona"]?.ToString();
            TempData["FiltroIdTipoIDs"] = TempData["FiltroIdTipoIDs"] != null ? int.Parse(TempData["FiltroIdTipoIDs"].ToString()) : 0;
            TempData["FiltroNumeroID"] = TempData["FiltroNumeroID"]?.ToString();
            TempData["FiltroIdDelegacionOrigen"] = TempData["FiltroIdDelegacionOrigen"] != null ? int.Parse(TempData["FiltroIdDelegacionOrigen"].ToString()) : 0;
            TempData["FiltroIdDelegacionDestino"] = TempData["FiltroIdDelegacionDestino"] != null ? int.Parse(TempData["FiltroIdDelegacionDestino"].ToString()) : 0;
            TempData["FiltroIdEstatusTransferencia"] = TempData["FiltroIdEstatusTransferencia"] != null ? int.Parse(TempData["FiltroIdEstatusTransferencia"].ToString()) : 0;
            TempData["FiltroIdEstatusPlacas"] = TempData["FiltroIdEstatusPlacas"] != null ? int.Parse(TempData["FiltroIdEstatusPlacas"].ToString()) : 0;

            //Se obtienen los valores de los combos solo en la carga inicial de la página,
            //en los POST se mantendrán estos valores
            if (TempData["ListadoTiposIDsDDL"] == null)
                TempData["ListadoTiposIDsDDL"] = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusTransferenciaDDL"] == null)
                TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusTransferenciaDDL"] == null)
                TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusPlacasDDL"] == null)
                TempData["ListadoEstatusPlacasDDL"] = new Controles_DDL().GetTiposEstatusPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetTransferenciaPlaca("", new Listado_RecepcionPlacasVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_RecepcionPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionPlacasEntreDelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(GetTransferenciaPlaca(control, viewModel));
        }

        private Listado_RecepcionPlacasVM GetTransferenciaPlaca(string control, Listado_RecepcionPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_RecepcionPlacasVM listadoVM = new Listado_RecepcionPlacasVM();
            listadoVM = viewModel;
            listadoVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTiposIDsDDL"] == null)
                TempData["ListadoTiposIDsDDL"] = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusTransferenciaDDL"] == null)
                TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusTransferenciaDDL"] == null)
                TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusPlacasDDL"] == null)
                TempData["ListadoEstatusPlacasDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal  
            TempData["FiltroFolioTransferencia"] = listadoVM.FiltroFolioTransferencia;
            TempData["FiltroFechaTransferencia"] = listadoVM.FiltroFechaTransferencia;
            TempData["FiltroNombrePersona"] = listadoVM.FiltroNombrePersona;
            TempData["FiltroApellidoPersona"] = listadoVM.FiltroApellidoPersona;
            TempData["FiltroIdTipoIDs"] = listadoVM.FiltroIdTipoIDs;
            TempData["FiltroNumeroID"] = listadoVM.FiltroNumeroID;
            TempData["FiltroIdDelegacionOrigen"] = listadoVM.FiltroIdDelegacionOrigen;
            TempData["FiltroIdDelegacionDestino"] = listadoVM.FiltroIdDelegacionDestino;
            TempData["FiltroIdEstatusTransferencia"] = listadoVM.FiltroIdEstatusTransferencia;
            TempData["FiltroIdEstatusPlacas"] = listadoVM.FiltroIdEstatusPlacas;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();
            var fechaInicio_ = DateTime.Parse("01/01/2000");
            var fechaFin_ = DateTime.Parse("01/01/2900");
            if (!String.IsNullOrEmpty(viewModel.FiltroFechaTransferencia))
            {
                fechaInicio_ = DateTime.Parse(viewModel.FiltroFechaTransferencia.Split('-')[0].Trim());
                fechaFin_ = DateTime.Parse(viewModel.FiltroFechaTransferencia.Split('-')[1].Trim());
            }

            string Folio_ = listadoVM.FiltroFolioTransferencia == null ? "" : listadoVM.FiltroFolioTransferencia;
            string NombreDP_ = listadoVM.FiltroNombrePersona == null ? "" : listadoVM.FiltroNombrePersona;
            string ApellidoDP_ = listadoVM.FiltroApellidoPersona == null ? "" : listadoVM.FiltroApellidoPersona;
            int NumeroIdDP_ = listadoVM.FiltroNumeroID == null ? 0 : int.Parse(listadoVM.FiltroNumeroID);
            int tipoIdDP_ = listadoVM.FiltroIdTipoIDs == null ? 0 : int.Parse(listadoVM.FiltroIdTipoIDs);
            int IdDelegacionOrigen_ = listadoVM.FiltroIdDelegacionOrigen == null ? 0 : int.Parse(listadoVM.FiltroIdDelegacionOrigen);
            int IdDelegacionDestino_ = listadoVM.FiltroIdDelegacionDestino == null ? 0 : int.Parse(listadoVM.FiltroIdDelegacionDestino);
            int IdEstatusTransferencia_ = listadoVM.FiltroIdEstatusTransferencia == null ? 0 : int.Parse(listadoVM.FiltroIdEstatusTransferencia);
            int IdEsatusPlacas_ = listadoVM.FiltroIdEstatusPlacas == null ? 0 : int.Parse(listadoVM.FiltroIdEstatusPlacas);
            int Entidad_ = usuarioLogin.Entidad;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<TransferenciaPlacas>> response = new TransferenciasPlacas_BL().GetTransferenciaPlacas(Folio_,
                                                                                                                  fechaInicio_,
                                                                                                                  fechaFin_,
                                                                                                                  NombreDP_,
                                                                                                                  ApellidoDP_,
                                                                                                                  NumeroIdDP_,
                                                                                                                  tipoIdDP_,
                                                                                                                  IdDelegacionOrigen_,
                                                                                                                  IdDelegacionDestino_,
                                                                                                                  IdEstatusTransferencia_,
                                                                                                                  IdEsatusPlacas_,
                                                                                                                  Entidad_,
                                                                                                                  "spcpl_transf_placas_pl.consulta_listado_recepcion");
            if (response.ExecutionOK)
            {
                List<Listado_RecepcionPlacasModel> TransferenciaPlacas = new List<Listado_RecepcionPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        TransferenciaPlacas.Add(new Listado_RecepcionPlacasModel() + m);
                    }
                    listadoVM.Listado = TransferenciaPlacas;
                }
            }

            TempData["ListadoTiposIDsDDL"] = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoEstatusPlacasDDL"] = new Controles_DDL().GetTiposEstatusPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

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
        public ActionResult RecibirPlacasIndividuales(int IdTransferencia, int IdTipoPlaca)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionPlacasEntreDelegacionesBancos.RecibirPlacasIndividualesAcceso)
            {
                return RedirectToAction("Index");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_RecepcionPlacas_IndividualesVM RecepcionPlacasVM = new Detalle_RecepcionPlacas_IndividualesVM();

            var objResponse = new TransferenciasPlacas_BL().GetRecibirPlacasIndividuales(IdTransferencia, IdTipoPlaca, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                RecepcionPlacasVM += objResponse.Data;

            }
            return View(RecepcionPlacasVM);
        }
        [HttpPost]
        public JsonResult GetRecibirPlacasIndividuales(int IdTransferencia, int IdTipoPlaca)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            DBResponse<Detalle_RecepcionPlacas_IndividualesVM> RecepcionPlacasVM = new DBResponse<Detalle_RecepcionPlacas_IndividualesVM>();
            var objResponse = new TransferenciasPlacas_BL().GetRecibirPlacasIndividuales(IdTransferencia, IdTipoPlaca, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                RecepcionPlacasVM.Data = new Detalle_RecepcionPlacas_IndividualesVM();

                RecepcionPlacasVM.Data += objResponse.Data;
                RecepcionPlacasVM.ExecutionOK = true;
            }

            return Json(RecepcionPlacasVM);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTransferencia"></param>
        /// <returns></returns>
        public ActionResult Details(int? IdTransferencia)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionPlacasEntreDelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index");
            }

            if (IdTransferencia == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_RecepcionPlacasVM RecepcionPlacasVM = new Detalle_RecepcionPlacasVM();
            RecepcionPlacasVM.RecepcionPlacas_DatosPersonaRecibe = new Detalle_RecepcionPlacas_DatosPersonaRecibeVM();
            RecepcionPlacasVM.RecepcionPlacas_TransporteRecibe = new Detalle_RecepcionPlacas_TransporteRecibeVM();
            RecepcionPlacasVM.RecepcionPlacas_Listado1 = new List<Listado_RecepcionPlacas_Listado1_Model>();
            RecepcionPlacasVM.RecepcionPlacas_Listado2 = new List<Listado_RecepcionPlacas_Listado2_Model>();

            RecepcionPlacasVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            RecepcionPlacasVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            RecepcionPlacasVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            RecepcionPlacasVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            var objResponse = new TransferenciasPlacas_BL().GetRecepcionTransferenciaPlaca_Details((int)IdTransferencia.Value, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                RecepcionPlacasVM += objResponse.Data;
            }
            return View(RecepcionPlacasVM);
        }

        [HttpPost]
        public JsonResult RecibirTransferenciaPlacasIndividual(int IdTransferencia, List<Listado_RecepcionPlacas_IndividualesModel> Listado)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"]; 
            var listadoDetalleTransferencia = new List<TransferenciaPlacas_Detalle>();

            foreach (var item in Listado.Where(x => x.Seleccionado))
            {
                listadoDetalleTransferencia.Add(new TransferenciaPlacas_Detalle()
                {
                    IdTransferencia = IdTransferencia,
                    IdTipoPlaca = item.IdTipoPlaca,
                    IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                    NumeroPlaca = item.NumeroPlaca,
                    TransferirPlacaIndividual = 1,
                    UsuarioRecibio = usuarioLogin.Usuario
                });
            }

            var responseDB = new TransferenciasPlacas_BL().RecibirPlacasIndividuales(IdTransferencia, listadoDetalleTransferencia, usuarioLogin);
            if (!responseDB.ExecutionOK)
            {
                responseDB.Message = "Ocurrio un error al recibir las placas individuales";
            } 

            return Json(responseDB);
        }

        [HttpPost]
        public JsonResult GuardarDatosPersona(TransferenciaPlacas_DatosPersona DatosPersona)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            DatosPersona.Entidad = usuarioLogin.Entidad;
            DatosPersona.Tipo = "Recibe";
            var dbResponse = new DBResponse<TransferenciaPlacas_DatosPersona>();

            var responseDatosPersona = new TransferenciasPlacas_BL().UpsertTransferencia_DatosPersona(DatosPersona);

            dbResponse = responseDatosPersona;

            return Json(dbResponse);
        }

        [HttpPost]
        public JsonResult GuardarDatosTransporte(TransferenciaPlacas_Transporte Transporte)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Transporte.Entidad = usuarioLogin.Entidad;
            Transporte.Tipo = "Recibe";
            var dbResponse = new DBResponse<TransferenciaPlacas_Transporte>();

            var responseTransporte = new TransferenciasPlacas_BL().UpsertTransferencia_Transporte(Transporte);

            dbResponse = responseTransporte;

            return Json(dbResponse);
        }

        [HttpPost]
        public JsonResult CambioEstatusTransferencia(int IdTransferencia, int EstatusTransferencia)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var infoTransferencia = new TransferenciasPlacas_BL().GetTransferenciaPlaca_Details(IdTransferencia, usuarioLogin.Entidad);
            var dbResponse = new DBResponse<DBNull>();
            if (infoTransferencia.ExecutionOK)
            {
                if (EstatusTransferencia == 2)
                {
                    if (infoTransferencia.Data.IdEstatusTransferencia != 1)
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "La transferencia no puede ser cancelada ya que se encuentra " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia;
                    }
                    else
                    {
                        dbResponse = new TransferenciasPlacas_BL().CambioEstatusTransferencia(IdTransferencia, EstatusTransferencia, usuarioLogin);
                        if (dbResponse.ExecutionOK)
                        {
                            dbResponse.Data = null;
                            dbResponse.Message = "Transferencia de placas " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia + " Correctamente";
                        }
                    }
                }
                if (EstatusTransferencia == 4)
                {
                    if (infoTransferencia.Data.IdEstatusTransferencia != 1)
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "La transferencia no puede ser Rechazada ya que se encuentra " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia;
                    }
                    else
                    {
                        dbResponse = new TransferenciasPlacas_BL().CambioEstatusTransferencia(IdTransferencia, EstatusTransferencia, usuarioLogin);
                        if (dbResponse.ExecutionOK)
                        {
                            dbResponse.Data = null;
                            dbResponse.Message = "Transferencia de placas " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia + " Correctamente";
                        }
                    }
                }
                if (EstatusTransferencia == 4)
                {
                    if (infoTransferencia.Data.IdEstatusTransferencia != 1)
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "La transferencia no puede ser Cerrada ya que se encuentra " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia;
                    }
                    else
                    {
                        dbResponse = new TransferenciasPlacas_BL().CambioEstatusTransferencia(IdTransferencia, EstatusTransferencia, usuarioLogin);
                        if (dbResponse.ExecutionOK)
                        {
                            dbResponse.Data = null;
                            dbResponse.Message = "Transferencia de placas " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia + " Correctamente";
                        }
                    }
                }
                else
                {
                    dbResponse = new TransferenciasPlacas_BL().CambioEstatusTransferencia(IdTransferencia, EstatusTransferencia, usuarioLogin);
                    if (dbResponse.ExecutionOK)
                    {
                        dbResponse.Data = null;
                        dbResponse.Message = "Transferencia de placas " + infoTransferencia.Data.TiposEstatusTransferencias.EstatusTransferencia + " Correctamente";
                    }
                }
            }
            return Json(dbResponse);
        }

        [HttpPost]
        public ActionResult GetDetalleTemp(List<Listado_RecepcionPlacas_IndividualesModel> ListPlacasIndividualesModel)
        {
            return PartialView("PartialRecibirPlacasIndividuales", ListPlacasIndividualesModel);
        }

    }
}
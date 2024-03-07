using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Helpers;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Helper;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class SolicitudesPlacasController : Controller
    {
        /// <summary>
        /// Despliega el listado de SolicitudPlacas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_SolicitudesPlacasProveedor.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["FiltroIdSolicitudPlacas"] = TempData["FiltroIdSolicitudPlacas"] != null ? int.Parse(TempData["FiltroIdSolicitudPlacas"].ToString()) : 0;
            TempData["FiltroIdProveedor"] = TempData["FiltroIdProveedor"] != null ? int.Parse(TempData["FiltroIdProveedor"].ToString()) : 0;
            TempData["FiltroNumeroSolicitudPlacas"] = TempData["FiltroNumeroSolicitudPlacas"]?.ToString();
            TempData["FiltroFolioSolicitud"] = TempData["FiltroFolioSolicitud"]?.ToString();

            //Se obtienen los valores de los combos solo en la carga inicial de la página,
            //en los POST se mantendrán estos valores
            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTiposPlacasDDL"] == null)
                TempData["ListadoTiposPlacasDDL"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetSolicitudPlacas("", new Listado_SolicitudesPlacasVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_SolicitudesPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_SolicitudesPlacasProveedor.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetSolicitudPlacas(control, viewModel));
        }

        private Listado_SolicitudesPlacasVM GetSolicitudPlacas(string control, Listado_SolicitudesPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            Listado_SolicitudesPlacasVM listadoVM = new Listado_SolicitudesPlacasVM();
            listadoVM.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal
            TempData["FiltroFechaEntrega"] = viewModel.FiltroFechaEntrega;
            TempData["FiltroFechaSolicitud"] = viewModel.FiltroFechaSolicitud;
            TempData["FiltroIdProveedor"] = viewModel.FiltroIdProveedor;
            TempData["FiltroNumeroContrato"] = viewModel.FiltroNumeroContrato;
            TempData["FiltroOrdenCompra"] = viewModel.FiltroOrdenCompra;
            TempData["FiltroFolioSolicitud"] = viewModel.FiltroFolioSolicitud;

            var fechaInicio_ = DateTime.Parse("01/01/2000");
            var fechaFin_ = DateTime.Parse("01/01/2900");

            var fechaEntregaInicio_ = DateTime.Parse("01/01/2000");
            var fechaEntregaFin_ = DateTime.Parse("01/01/2900");

            if (!String.IsNullOrEmpty(viewModel.FiltroFechaSolicitud))
            {
                fechaInicio_ = DateTime.Parse(viewModel.FiltroFechaSolicitud.Split('-')[0].Trim());
                fechaFin_ = DateTime.Parse(viewModel.FiltroFechaSolicitud.Split('-')[1].Trim());
            }

            if (!String.IsNullOrEmpty(viewModel.FiltroFechaEntrega))
            {
                fechaEntregaInicio_ = DateTime.Parse(viewModel.FiltroFechaEntrega.Split('-')[0].Trim());
                fechaEntregaFin_ = DateTime.Parse(viewModel.FiltroFechaEntrega.Split('-')[1].Trim());
            }
            int? intNulleable = null;
            var idProveedor_ = viewModel.FiltroIdProveedor == null ? 0 : int.Parse(viewModel.FiltroIdProveedor);
            var numeroContrato_ = viewModel.FiltroNumeroContrato == null ? "" : viewModel.FiltroNumeroContrato;
            var numeroOC_ = viewModel.FiltroOrdenCompra == null ? intNulleable : int.Parse(viewModel.FiltroOrdenCompra);
            var folioSolicitud_ = viewModel.FiltroFolioSolicitud == null ? "" : viewModel.FiltroFolioSolicitud;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<SolicitudesPlacas>> response = new SolicitudesPlacas_BL().GetSolicitudesPlacas(folioSolicitud_,
                                                                                                           fechaInicio_,
                                                                                                           fechaFin_,
                                                                                                           fechaEntregaInicio_,
                                                                                                           fechaEntregaFin_,
                                                                                                           idProveedor_,
                                                                                                           numeroContrato_,
                                                                                                           numeroOC_,
                                                                                                           usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_SolicitudesPlacasModel> SolicitudesPlacas = new List<Listado_SolicitudesPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        SolicitudesPlacas.Add(new Listado_SolicitudesPlacasModel() + m);
                    }
                    listadoVM.Listado = SolicitudesPlacas;
                }
            }

            TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_SolicitudesPlacasProveedor.Registrar)
            {
                return RedirectToAction("Index", "Home");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_SolicitudesPlacasVM SolicitudesPlacasVM = new Detalle_SolicitudesPlacasVM();
            SolicitudesPlacasVM.FechaSolicitud = DateTime.Now;
            SolicitudesPlacasVM.FechaSolicitudStr = DateTime.Now.ToString("dd/MM/yyyy");
            SolicitudesPlacasVM.FechaEntrega = DateTime.Now;
            SolicitudesPlacasVM.FolioSolicitud = new SolicitudesPlacas_BL().GetSolicitudesPlacasSiguienteFolio().Data.FolioSolicitud;
            SolicitudesPlacasVM.FechaEntregaStr = DateTime.Now.ToString("dd/MM/yyyy");
            SolicitudesPlacasVM.Detalle_SolicitudesPlacasDetailsVM = new List<Listado_SolicitudesPlacasDetailsModel>();
            SolicitudesPlacasVM.DetalleSolicitud.ListadoDelegacionesBancosDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            SolicitudesPlacasVM.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            SolicitudesPlacasVM.ListadoContratosDDL = new Data_ControlesDDL().getDataDefault();
            SolicitudesPlacasVM.DetalleSolicitud.ListadoTiposPlacasDDL = new Data_ControlesDDL().getDataDefault();
            SolicitudesPlacasVM.ListadoOrdenesCompraDDL = new Data_ControlesDDL().getDataDefault();
            return View(SolicitudesPlacasVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? IdSolicitud)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_SolicitudesPlacasProveedor.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            if (IdSolicitud == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_SolicitudesPlacasVM SolicitudesPlacasVM = new Detalle_SolicitudesPlacasVM();
            SolicitudesPlacasVM.DetalleSolicitud.ListadoDelegacionesBancosDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            SolicitudesPlacasVM.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            var dbResponseSolicitud = new SolicitudesPlacas_BL().GetSolicitudPlacas((int)IdSolicitud, usuarioLogin.Entidad);
            if (dbResponseSolicitud.ExecutionOK)
            {
                var objSolicitud = dbResponseSolicitud.Data;
                SolicitudesPlacasVM.ListadoContratosDDL = new Controles_DDL().GetContratosPorveedorSeleccionado_DDL(objSolicitud.IdProveedor, usuarioLogin.Entidad, "Seleccione").Data;
                SolicitudesPlacasVM.DetalleSolicitud.ListadoTiposPlacasDDL = new Data_ControlesDDL().getDataDefault();
                SolicitudesPlacasVM.ListadoOrdenesCompraDDL = new Controles_DDL().GetOrdenesCompraPorveedorSeleccionado_DDL(objSolicitud.IdProveedor, usuarioLogin.Entidad, "Seleccione").Data;
            }
            else
            {
                return RedirectToAction("Index", "SolicitudesPlacas");
            }

            return View(SolicitudesPlacasVM);
        }


        [HttpPost]
        public JsonResult SolicitudDetails(int IdSolicitud)
        {
            var responseSolicitudPlaca = new DBResponse<Detalle_SolicitudesPlacasVM>();
            responseSolicitudPlaca.Data = new Detalle_SolicitudesPlacasVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponse = new SolicitudesPlacas_BL().GetSolicitudPlacas(IdSolicitud, usuarioLogin.Entidad);
            if (dbResponse.ExecutionOK)
            {
                responseSolicitudPlaca.Data += dbResponse.Data;
                responseSolicitudPlaca.ExecutionOK = true;
                responseSolicitudPlaca.NumRows = 1;
            }
            else
            {
                responseSolicitudPlaca.Data = new Detalle_SolicitudesPlacasVM();
                responseSolicitudPlaca.ExecutionOK = false;
                responseSolicitudPlaca.NumRows = 1;
            }
            return Json(responseSolicitudPlaca);
        }

        [HttpPost]
        public JsonResult SolicitudNotaEntradaDetails(int IdSolicitud, string FolioEntrada)
        {
            var responseSolicitudPlaca = new DBResponse<Detalle_SolicitudesPlacasVM>();
            responseSolicitudPlaca.Data = new Detalle_SolicitudesPlacasVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponse = new SolicitudesPlacas_BL().GetSolicitudPlacas(IdSolicitud, usuarioLogin.Entidad);
            if (dbResponse.ExecutionOK)
            { 

                var dbResponseNE = new NotasEntradasPlacas_BL().GetNotasEntradasPlacasDet_Folio(FolioEntrada, usuarioLogin.Entidad);
                if (!dbResponseNE.ExecutionOK)
                {
                    responseSolicitudPlaca.Message = "No se encontro información en la Nota de Entrada";
                    responseSolicitudPlaca.ExecutionOK = false;
                    return Json(responseSolicitudPlaca);
                }
                var objNotaEntrada = dbResponseNE.Data;

                foreach (var item in dbResponse.Data.SolicitudesPlacas_Detalle)
                {
                    if (objNotaEntrada.Any(x => x.IdTipoPlaca == item.IdTipoPlaca))
                    {
                        var index = dbResponse.Data.SolicitudesPlacas_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca);
                        dbResponse.Data.SolicitudesPlacas_Detalle[index].CantidadPlacasNotasEntrada = objNotaEntrada.Where(x => x.IdTipoPlaca == item.IdTipoPlaca).FirstOrDefault().CantidadPlacas;
                    }
                }

                //dbResponse.Data.SolicitudesPlacas_Detalle = new List<SolicitudesPlacas_Detalle>();
                //dbResponse.Data.SolicitudesPlacas_Detalle = detalle;

                responseSolicitudPlaca.Data += dbResponse.Data;
                responseSolicitudPlaca.ExecutionOK = true;
                responseSolicitudPlaca.NumRows = 1;
            }
            else
            {
                responseSolicitudPlaca.Data = new Detalle_SolicitudesPlacasVM();
                responseSolicitudPlaca.ExecutionOK = false;
                responseSolicitudPlaca.NumRows = 1;
            }
            return Json(responseSolicitudPlaca);
        }



        [HttpPost]
        public JsonResult UpsertSolicitud(Detalle_SolicitudesPlacasVM viewModel)
        {

            var dbResponse = new DBResponse<SolicitudesPlacas>();
            try
            {
                var usuarioLogin = (Usuarios)Session["UserSC"];
                var objSolicitudesPlacas = new SolicitudesPlacas()
                {
                    IdSolicitud = viewModel.IdSolicitud,
                    IdContrato = viewModel.IdContrato,
                    Entidad = usuarioLogin.Entidad,
                    FechaEntrega = viewModel.FechaEntrega,
                    FechaRegistro = DateTime.Now,
                    FechaSolicitud = viewModel.FechaSolicitud,
                    IdOrdenCompra = viewModel.IdOrdenCompra,
                    FolioSolicitud = viewModel.FolioSolicitud,
                    IdProveedor = viewModel.IdProveedor,
                    UsuarioRegistra = usuarioLogin.Usuario,
                    SolicitudesPlacas_Detalle = new List<SolicitudesPlacas_Detalle>()
                };
                var consecutivo = 0;
                foreach (var item in viewModel.Detalle_SolicitudesPlacasDetailsVM)
                {
                    objSolicitudesPlacas.SolicitudesPlacas_Detalle.Add(new SolicitudesPlacas_Detalle()
                    {
                        IdSolicitud = objSolicitudesPlacas.IdSolicitud,
                        IdDelegacionBanco = item.IdDelegacionBanco,
                        IdDetalleOrdenCompra = item.IdDetalleOrdenCompra,
                        IdSolicitudDetalle = 0,
                        IdTipoPlaca = item.IdTipoPlaca,
                        RangoPlacaFinal = item.RangoPlacaFinal,
                        RangoPlacaInicial = item.RangoPlacaInicial,
                        CantidadPlacas = item.CantidadPlacas,
                        Entidad = usuarioLogin.Entidad
                    });
                    consecutivo++;
                }

                var esNuevo = viewModel.IdSolicitud == 0;
                var responseUpsert = new SolicitudesPlacas_BL().UpsertSolicitudPlacas(objSolicitudesPlacas, usuarioLogin, esNuevo);
                if (responseUpsert.ExecutionOK)
                {
                    dbResponse.Data = objSolicitudesPlacas;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Solicitud de Placas generada correctamente";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.Data = objSolicitudesPlacas;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Ocurrio un error al guardar la Solicitud de Placas";
                    return Json(dbResponse);
                }
            }
            catch (Exception ex)
            {
                dbResponse.Data = new SolicitudesPlacas();
                dbResponse.ExecutionOK = true;
                dbResponse.Message = ex.Message;
                return Json(dbResponse);
            }
        }


        public JsonResult ObtenContratosProveedor(int IdProveedor)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new Controles_DDL().GetContratosPorveedorSeleccionado_DDL(IdProveedor, usuarioLogin.Entidad, "Seleccione");
            return Json(dbResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenOrdenesCompraProveedor(int IdProveedor)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new Controles_DDL().GetOrdenesCompraPorveedorSeleccionado_DDL(IdProveedor, usuarioLogin.Entidad, "Seleccione");
            return Json(dbResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenTiposPlacasOrdenesCompra(int IdOrdenCompra, int IdContrato)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new Controles_DDL().GetTiposPlacasOrdenCompraSeleccionada_DDL(IdOrdenCompra, IdContrato, usuarioLogin.Entidad, "Seleccione");
            return Json(dbResponse.Data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenOrdenCompraDetalle(int IdOrdenCompra, int IdTipoPlaca)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var detalleOC = new DBResponse<OrdenesCompra_Detalle>();
            var dbResponse = new OrdenesCompra_BL().GetOrdenesCompraDet(IdOrdenCompra, usuarioLogin.Entidad);
            if (dbResponse.ExecutionOK)
            {
                detalleOC.ExecutionOK = true;
                detalleOC.Data = dbResponse.Data.Where(x => x.IdTipoPlaca == IdTipoPlaca).FirstOrDefault();
                detalleOC.NumRows = 1;
            }
            else
            {
                detalleOC.ExecutionOK = false;
                detalleOC.Data = new OrdenesCompra_Detalle();
                detalleOC.NumRows = 0;
            }
            return Json(detalleOC, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatosTipoPlaca(int IdOrdenCompra, int IdTipoPlaca, int IdContrato, int IdProveedor)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponse = new DBResponse<DatosTipoPlaca>();
            dbResponse.Data = new DatosTipoPlaca();
            try
            {
                var getDetalleOC = new OrdenesCompra_BL().GetIdDetalleOrdenCompra(IdOrdenCompra, IdTipoPlaca, usuarioLogin.Entidad);
                if (getDetalleOC.ExecutionOK)
                {
                    dbResponse.Data.IdDetalleOrdenCompra = (int)getDetalleOC.Data;
                    var getInfoDetContrato = new Contratos_BL().GetContratosTipoPlacaDet(IdContrato, IdTipoPlaca, usuarioLogin.Entidad);
                    if (getInfoDetContrato.ExecutionOK)
                    {
                        var contratoDetalle_ = new Contratos_BL().GetContrato(IdContrato, usuarioLogin.Entidad).Data.Contratos_Detalle.Where(x => x.IdTipoPlaca == IdTipoPlaca && x.IdProveedor == IdProveedor).FirstOrDefault();

                        dbResponse.Data.CantidadPlacasCaja = contratoDetalle_.CantidadPlacasCaja; 
                        dbResponse.Data.MascaraPlaca = contratoDetalle_.MascaraPlaca;
                        dbResponse.Data.OrdenPlaca = contratoDetalle_.OrdenPlaca;
                        dbResponse.Data.CantidadPlacas = getInfoDetContrato.Data.CantidadPlacas;
                        dbResponse.Data.RangoInicial = getInfoDetContrato.Data.RangoInicial;
                        dbResponse.Data.RangoFinal = getInfoDetContrato.Data.RangoFinal;

                        dbResponse.ExecutionOK = true;
                    }
                    else
                    {
                        dbResponse.ExecutionOK = false;
                        dbResponse.Message = "El tipo de placa seleccionado no existe en el contrato de la Solicitud de Placa";
                    }
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No se pudo obtener el detalle de la orden de compra";
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
                dbResponse.Data = new DatosTipoPlaca();
            }
            return Json(dbResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCalculoPlacas(GenerarPlacasRangos PlacasRangos)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponseConteo = new DBResponse<int>();
            try
            {
                //var getTipoPlaca = new TiposPlacas_BL().GetTipoPlaca(usuarioLogin.Entidad, PlacasRangos.IdTipoPlaca).Data;
                //PlacasRangos.TipoPlaca = new TiposPlacas();
                //PlacasRangos.TipoPlaca = getTipoPlaca;

                var dbResponseValida = new AlgoritmoGeneracionPlacas_BL().Validaciones(PlacasRangos);
                if (dbResponseValida.ExecutionOK)
                {
                    var calculaPlacas = new AlgoritmoGeneracionPlacas_BL().GeneraPlacasRangos(PlacasRangos);
                    if (calculaPlacas.ExecutionOK)
                    {
                        dbResponseConteo.Data = calculaPlacas.Data.Count;
                        dbResponseConteo.ExecutionOK = true;
                    }
                    else
                    {
                        dbResponseConteo.ExecutionOK = false;
                        dbResponseConteo.Message = calculaPlacas.Message;
                    }
                    return Json(dbResponseConteo);
                }
                else
                {
                    return Json(new DBResponse<TiposPlacas>());
                }
            }
            catch (Exception ex)
            {
                var response = new DBResponse<TiposPlacas>()
                {
                    ExecutionOK = false,
                    Data = new TiposPlacas(),
                    Message = ex.Message,
                    NumRows = 0
                };
                return Json(response);
            }
        }

        [HttpPost]
        public ActionResult GuardarDetalleTemp(List<Listado_SolicitudesPlacasDetailsModel> ListSolicitudesPlacasDetailsModel)
        {
            return PartialView("PartialDetalleSolicitud", ListSolicitudesPlacasDetailsModel);
        }

        [HttpPost]
        public ContentResult SolicitudPlacasGeneraPDF(int IdSolicitud, Boolean EnviaPDF)
        {
            string base64 = "";
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var getSolicitud = new SolicitudesPlacas_BL().GetSolicitudPlacas(IdSolicitud, usuarioLogin.Entidad);
            if (getSolicitud.ExecutionOK)
            {
                var objSolicitud = getSolicitud.Data;
                var pathPlantillas = InfoRutasArchivos.GetInfoFilePath().DirectorioArchivos;

                var responseGeneraArchivo = Helper.GeneracionEnviosPDF.SolicitudPlacasGeneraPDF(objSolicitud, pathPlantillas, EnviaPDF);
                if (!responseGeneraArchivo.ExecutionOK)
                    TempData["MensajeAIndex"] = responseGeneraArchivo.Message;

                TempData["MensajeAIndex"] = "";

                base64 = Convert.ToBase64String(responseGeneraArchivo.Data, 0, responseGeneraArchivo.Data.Length);
                return Content(base64);
            }

            return Content(base64);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_SolicitudesPlacasProveedor.Eliminar)
            {
                return RedirectToAction("Index", "Home");
            }

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<DBNull> response = new SolicitudesPlacas_BL().DeleteSolicitudesPlacas(id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Solicitud de Placas a Proveedor eliminado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;


            return RedirectToAction("Index");
        }
    }
}
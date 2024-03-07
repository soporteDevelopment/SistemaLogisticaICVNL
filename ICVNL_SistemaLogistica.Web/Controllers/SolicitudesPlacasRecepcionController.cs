using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Helpers;
using ICVNL_SistemaLogistica.Web.Entities.PrototypeData;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Helper;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using NPOI.HSSF.Record.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class SolicitudesPlacasRecepcionController : Controller
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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["FiltroFolioRecepcion"] = TempData["FiltroFolioRecepcion"]?.ToString();
            TempData["FiltroFolioSolicitud"] = TempData["FiltroFolioSolicitud"]?.ToString();
            TempData["FiltroIdDelegacionBanco"] = TempData["FiltroIdDelegacionBanco"] != null ? int.Parse(TempData["FiltroIdDelegacionBanco"].ToString()) : 0;
            TempData["FiltroIdProveedor"] = TempData["FiltroIdProveedor"] != null ? int.Parse(TempData["FiltroIdProveedor"].ToString()) : 0;
            TempData["FiltroNumeroContrato"] = TempData["FiltroNumeroSolicitudPlacas"]?.ToString();
            TempData["FiltroOrdenCompra"] = TempData["FiltroOrdenCompra"]?.ToString();
            TempData["FiltroNotaEntrada"] = TempData["FiltroNotaEntrada"]?.ToString();


            //Se obtienen los valores de los combos solo en la carga inicial de la página,
            //en los POST se mantendrán estos valores
            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoOrdenesCompraDDL"] == null)
                TempData["ListadoOrdenesCompraDDL"] = new Controles_DDL().GetOrdenesCompra_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;


            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetSolicitudPlacas("", new Listado_SolicitudesPlacasRecepcionVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_SolicitudesPlacasRecepcionVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(GetSolicitudPlacas(control, viewModel));
        }

        private Listado_SolicitudesPlacasRecepcionVM GetSolicitudPlacas(string control, Listado_SolicitudesPlacasRecepcionVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_SolicitudesPlacasRecepcionVM listadoVM = new Listado_SolicitudesPlacasRecepcionVM();
            listadoVM.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoDelegacionesBancosDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoOrdenesCompraDDL"] == null)
                TempData["ListadoOrdenesCompraDDL"] = new Controles_DDL().GetOrdenesCompra_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal 
            TempData["FiltroFolioRecepcion"] = viewModel.FiltroFolioRecepcion;
            TempData["FiltroFolioSolicitud"] = viewModel.FiltroFolioSolicitud;
            TempData["FiltroIdDelegacionBanco"] = viewModel.FiltroIdDelegacionBanco;
            TempData["FiltroIdProveedor"] = viewModel.FiltroIdProveedor;
            TempData["FiltroNumeroContrato"] = viewModel.FiltroNumeroContrato;
            TempData["FiltroOrdenCompra"] = viewModel.FiltroOrdenCompra;
            TempData["FiltroNotaEntrada"] = viewModel.FiltroNotaEntrada;

            string FolioRecepcion_ = viewModel.FiltroFolioRecepcion;
            int IdDelegacionBanco_ = viewModel.FiltroIdDelegacionBanco == null ? 0 : int.Parse(viewModel.FiltroIdDelegacionBanco);
            string FolioSolicitud_ = viewModel.FiltroFolioSolicitud;
            int IdProveedor_ = viewModel.FiltroIdProveedor == null ? 0 : int.Parse(viewModel.FiltroIdProveedor);
            string NumeroContrato_ = viewModel.FiltroNumeroContrato;
            int IdOrdenCompra_ = viewModel.FiltroOrdenCompra == null ? 0 : int.Parse(viewModel.FiltroOrdenCompra);
            int Entidad_ = usuarioLogin.Entidad;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<RecepcionSolicitudesPlacas>> response = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudesPlacas(FolioRecepcion_,
                                                                                                                                      IdDelegacionBanco_,
                                                                                                                                      FolioSolicitud_,
                                                                                                                                      IdProveedor_,
                                                                                                                                      NumeroContrato_,
                                                                                                                                      IdOrdenCompra_,
                                                                                                                                      Entidad_);
            if (response.ExecutionOK)
            {
                List<Listado_SolicitudesPlacasRecepcionModel> SolicitudesPlacasRecepcion = new List<Listado_SolicitudesPlacasRecepcionModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        SolicitudesPlacasRecepcion.Add(new Listado_SolicitudesPlacasRecepcionModel() + m);
                    }
                    listadoVM.Listado = SolicitudesPlacasRecepcion;
                }
            }

            TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoOrdenesCompraDDL"] = new Controles_DDL().GetOrdenesCompra_DDL(usuarioLogin.Entidad, "Seleccione").Data;
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
        /// <returns></returns>
        public ActionResult Create()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.Registrar)
            {
                return RedirectToAction("Index", "Login");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_SolicitudesPlacasRecepcionVM SolicitudesPlacasRecepcionVM = new Detalle_SolicitudesPlacasRecepcionVM();
            SolicitudesPlacasRecepcionVM.FolioRecepcion = new SolicitudesPlacasRecepcion_BL().GetRecepcionesSolicitudesPlacasSiguienteFolio().Data.FolioRecepcion;

            SolicitudesPlacasRecepcionVM.ListadoSolicitudesPlacasDDL = new Data_ControlesDDL().getDataDefault();
            SolicitudesPlacasRecepcionVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            return View(SolicitudesPlacasRecepcionVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? IdRecepcion)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }

            if (IdRecepcion == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles 
            TempData["messages"] = new Dictionary<string, string[]>();
            var dbResponse = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudPlacas((int)IdRecepcion, usuarioLogin.Entidad);
            Detalle_SolicitudesPlacasRecepcionVM SolicitudesPlacasRecepcionVM = new Detalle_SolicitudesPlacasRecepcionVM();
            SolicitudesPlacasRecepcionVM.IdRecepcion = dbResponse.Data.IdRecepcion;
            SolicitudesPlacasRecepcionVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            SolicitudesPlacasRecepcionVM.ListadoSolicitudesPlacasDDL = new Data_ControlesDDL().getDataDefault();
            return View(SolicitudesPlacasRecepcionVM);
        }

        [HttpPost]
        public JsonResult UpsertRecepcionSolicitud(Detalle_SolicitudesPlacasRecepcionVM viewModel)
        {
            var dbResponse = new DBResponse<RecepcionSolicitudesPlacas>();
            try
            {
                //aqui realizaremos la validacion:
                //•	Para poder hacer la recepción de las Placas, es obligatorio primero haber capturado en el GRP de Infofin
                //la Nota de Entada que corresponde a lo que se va a recibir, pues si no existe la Remisión en el GRP Infofin,
                //no se pueden recibir las placas relacionadas con esa Remisión.
                if (true) 
                {

                }
                var usuarioLogin = (Usuarios)Session["UserSC"];
                var objRecepcionSolicitudesPlacas = new RecepcionSolicitudesPlacas()
                {
                    IdSolicitud = viewModel.IdSolicitud,
                    IdRecepcion = viewModel.IdRecepcion,
                    IdDelegacionBanco = viewModel.IdDelegacionBanco,
                    Entidad = usuarioLogin.Entidad,
                    Fecha = DateTime.Now,
                    Estatus = 1,
                    FolioRecepcion = viewModel.FolioRecepcion,
                    NotaEntradaAutorizada = viewModel.NotaEntradaAutorizada,
                    Recibida = false,
                    UsuarioRegistro = usuarioLogin.Usuario,
                    RecepcionSolicitudesPlacas_Detalle = new List<RecepcionSolicitudesPlacas_Detalle>()
                };
                var consecutivo = 0;
                foreach (var item in viewModel.RecepcionSolicitudesPlacas_Detalle)
                {
                    objRecepcionSolicitudesPlacas.RecepcionSolicitudesPlacas_Detalle.Add(new RecepcionSolicitudesPlacas_Detalle()
                    {
                        IdRecepcion = objRecepcionSolicitudesPlacas.IdRecepcion,
                        IdRecepcionDetalle = 0,
                        IdTipoPlaca = item.IdTipoPlaca,
                        CantidadNotasEntradaAutorizada = item.CantidadNotasEntradaAutorizada,
                        CantidadRecibida = item.CantidadRecibida,
                        CantidadSolicitadaOrdenCompra = item.CantidadSolicitadaOrdenCompra,
                        Entidad = usuarioLogin.Entidad
                    });
                    consecutivo++;
                }

                var esNuevo = viewModel.IdRecepcion == 0;
                var responseUpsert = new SolicitudesPlacasRecepcion_BL().UpsertRecepcionSolicitudPlacas(objRecepcionSolicitudesPlacas, usuarioLogin, esNuevo);
                if (responseUpsert.ExecutionOK)
                {
                    dbResponse.Data = objRecepcionSolicitudesPlacas;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Recepción de la Solicitud de Placas generada correctamente";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.Data = objRecepcionSolicitudesPlacas;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Ocurrio un error al guardar la Recepción de la Solicitud de Placas";
                    return Json(dbResponse);
                }
            }
            catch (Exception ex)
            {
                dbResponse.Data = new RecepcionSolicitudesPlacas();
                dbResponse.ExecutionOK = true;
                dbResponse.Message = ex.Message;
                return Json(dbResponse);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RecepcionSolicitudDetails(int IdRecepcionSolicitud)
        {
            var responseSolicitudPlaca = new DBResponse<Detalle_SolicitudesPlacasRecepcionVM>();
            responseSolicitudPlaca.Data = new Detalle_SolicitudesPlacasRecepcionVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponse = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudPlacas(IdRecepcionSolicitud, usuarioLogin.Entidad);
            if (dbResponse.ExecutionOK)
            {
                responseSolicitudPlaca.Data += dbResponse.Data;
                responseSolicitudPlaca.ExecutionOK = true;
                responseSolicitudPlaca.NumRows = 1;
            }
            else
            {
                responseSolicitudPlaca.Data = new Detalle_SolicitudesPlacasRecepcionVM();
                responseSolicitudPlaca.ExecutionOK = false;
                responseSolicitudPlaca.NumRows = 1;
            }
            return Json(responseSolicitudPlaca);
        }

        public JsonResult GetDatosSolicitudesPlacas(int IdDelegacionBanco)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new Controles_DDL().GetDelegaciones_RecepcionSolicitudes_DDL(IdDelegacionBanco, usuarioLogin.Entidad, "Seleccione");
            return Json(dbResponse.Data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetDetalleRecepcionSolicitud(List<Listado_SolicitudesPlacasRecepcionDetailsModel> ListSolicitudesPlacasRecepcionDetails)
        {
            return PartialView("PartialDetalleRecepcionSolicitud", ListSolicitudesPlacasRecepcionDetails);
        }

        [HttpPost]
        public ActionResult GuardaCajaTemp(List<Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM> Listado, int IdRecepcion)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var listado_ = new List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel>();
            var consecutivo = Listado.Count  + 1; 
            if (Listado.Count > 0)
            {
                foreach (var item in Listado)
                {
                    var delegacion = new Detalle_DelegacionesBancosVM();
                    delegacion += new DelegacionesBancos_BL().GetDelegacionBanco(usuarioLogin.Entidad, item.IdDelegacionBanco).Data;

                    var tipoPlaca = new Detalle_TiposPlacasVM();
                    tipoPlaca += new TiposPlacas_BL().GetTipoPlaca(usuarioLogin.Entidad, item.IdTipoPlaca).Data;

                    var proveedor = new Detalle_ProveedoresVM();
                    proveedor += new Proveedores_BL().GetProveedor(usuarioLogin.Entidad, item.IdProveedor).Data;

                    var obj = new Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel();
                    obj.DelegacionesBancos = new Detalle_DelegacionesBancosVM();
                    obj.Proveedores = new Detalle_ProveedoresVM();
                    obj.TiposPlacas = new Detalle_TiposPlacasVM();
                    obj.FechaHoraRegistro = DateTime.Now;
                    obj.IdValidacionEvento = 0;
                    obj.IdEventoRecepcion = 0;
                    obj.Consecutivo = consecutivo;
                    obj.IdRecepcion = item.IdRecepcion;
                    obj.IdDelegacionBanco = item.IdDelegacionBanco;
                    obj.IdProveedor = item.IdProveedor;
                    obj.IdTipoPlaca = item.IdTipoPlaca;
                    obj.CantidadLaminas = item.CantidadLaminas;
                    obj.Rangos = item.RangosPlacas;
                    obj.RangoInicial = item.RangoInicial;
                    obj.RangoFinal = item.RangoFinal;
                    obj.DelegacionesBancos = delegacion;
                    obj.Proveedores = proveedor;
                    obj.TiposPlacas = tipoPlaca;
                    obj.NumeroCaja = item.NumeroCaja;
                    consecutivo++;
                    listado_.Add(obj);
                }

            }
            return PartialView("PartialDetalleRecibirCaja", listado_);
        }


        [HttpPost]
        public JsonResult RealizarRecepcionCajas(List<Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM> Listado, int IdRecepcion)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var listado_ = new List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel>();
            var dbResponseJson = new DBResponse<string>();
            var pathPlantillas = InfoRutasArchivos.GetInfoFilePath().DirectorioArchivos;

            try
            {
                var objResponseRecepcion = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudPlacas(IdRecepcion, usuarioLogin.Entidad);
                if (objResponseRecepcion.ExecutionOK)
                {

                    var objRecepcion = objResponseRecepcion.Data;
                    var dbResponseNotaEntrada = new NotasEntradasPlacas_BL().GetNotasEntradasPlacasEnc_Folio(objRecepcion.NotaEntradaAutorizada, usuarioLogin.Entidad).Data;
                    foreach (var item in Listado)
                    {
                        var tipoPlaca_ = objResponseRecepcion.Data.SolicitudesPlacas.SolicitudesPlacas_Detalle.Where(x => x.IdTipoPlaca == item.IdTipoPlaca).FirstOrDefault().TiposPlacas;
                        if (tipoPlaca_ == null)
                        {
                            return Json("Error al obtener el tipo de placa");
                        }

                        var generaRango = new GenerarPlacasRangos()
                        {
                            Cantidad = int.Parse((item.CantidadLaminas / 2).ToString()),
                            CantidadSolicitadaNotaEntrada = objResponseRecepcion.Data.SolicitudesPlacas.SolicitudesPlacas_Detalle.Where(x => x.IdTipoPlaca == item.IdTipoPlaca).Sum(y => y.CantidadPlacas),
                            IdTipoPlaca = item.IdTipoPlaca,
                            TipoPlaca = new TiposPlacas()
                            {
                                IdTipoPlaca = tipoPlaca_.IdTipoPlaca,
                                MascaraPlaca = tipoPlaca_.MascaraPlaca,
                                OrdenPlaca = tipoPlaca_.OrdenPlaca,
                                OrdenSeriePlaca = tipoPlaca_.OrdenSeriePlaca
                            },
                            RangoInicial = item.RangoInicial,
                            RangoFinal = item.RangoFinal
                        };

                        var validaRecibir = new SolicitudesPlacasRecepcion_BL().ValidaRecepcionRangosPlacas(generaRango);
                        if (validaRecibir.ExecutionOK)
                        {
                            var placasCalculadas = new AlgoritmoGeneracionPlacas_BL().GeneraPlacasRangos(generaRango).Data;
                            if (placasCalculadas.Count > 0)
                            {
                                var TipoProblema = 0;
                                var conteoPlacasCalculadas = placasCalculadas.Count;
                                if (conteoPlacasCalculadas > generaRango.Cantidad)
                                {
                                    TipoProblema++;
                                    var recibir = new RecepcionSolicitudesPlacas_Recibir
                                    {
                                        IdRecepcion = IdRecepcion,
                                        IdValidacionEvento = 0,
                                        IdEventoRecepcion = 0,
                                        IdDelegacionBanco = objRecepcion.IdDelegacionBanco,
                                        IdProveedor = objRecepcion.SolicitudesPlacas.IdProveedor,
                                        IdSolicitud = objRecepcion.IdSolicitud,
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        CantidadLaminas = item.CantidadLaminas,
                                        FechaHoraRegistro = DateTime.Now,
                                        NumeroCaja = item.NumeroCaja,
                                        Rangos = item.RangosPlacas,
                                        Entidad = usuarioLogin.Entidad,
                                        RecepcionSolicitudesPlacas_Recibir_Validaciones = new RecepcionSolicitudesPlacas_Recibir_Validaciones()
                                        {
                                            IdRecepcion = IdRecepcion,
                                            IdEventoRecepcion = 0,
                                            IdNotaEntrada = dbResponseNotaEntrada.IdNotaEntrada,
                                            IdProveedor = objRecepcion.SolicitudesPlacas.IdProveedor,
                                            IdTipoPlaca = item.IdTipoPlaca,
                                            IdTipoProblema = 2,
                                            CajaNdeM = item.NumeroCaja,
                                            IdContrato = objRecepcion.SolicitudesPlacas.IdContrato,
                                            Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                            Horas = DateTime.Now.ToString("hh:mm"),
                                            IdDelegacionesBancos = objRecepcion.IdDelegacionBanco,
                                            IdRecepcionValidaciones = 0,
                                            UsuarioNombre = usuarioLogin.Usuario,
                                            PartidaContrato = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca && x.IdProveedor == item.IdProveedor).ToString().ToString() + 1,
                                            NotaEntrada = objRecepcion.NotaEntradaAutorizada,
                                            Usuario = new Usuarios()
                                            {
                                                Nombre = usuarioLogin.Nombre,
                                                ClaveUsuario = usuarioLogin.ClaveUsuario,
                                                NumeroEmpleado = usuarioLogin.NumeroEmpleado == null ? " " : usuarioLogin.NumeroEmpleado,
                                                Puesto = usuarioLogin.Puesto == null ? " " : usuarioLogin.Puesto
                                            }
                                        }
                                    };

                                    var insertaEvento = new SolicitudesPlacasRecepcion_BL().InsertaRecepcionSolicitudPlaca_Recibir(recibir, usuarioLogin);
                                    if (insertaEvento.ExecutionOK)
                                    {
                                        var generaArchivoEvento = Helper.GeneracionEnviosPDF.RecepcionPlacasEventos(new PlacasRecibir_DocumentoGeneradoEvento()
                                        {
                                            DelegacionBanco = objRecepcion.DelegacionesBancos.NombreDelegacionBanco,
                                            FolioContrato = objRecepcion.SolicitudesPlacas.Contratos.NumeroContrato,
                                            NombreProveedor = objRecepcion.SolicitudesPlacas.Proveedores.NombreProveedor,
                                            NumEmpleado = usuarioLogin.NumeroEmpleado,
                                            Fecha = recibir.FechaHoraRegistro.ToString("dd") + " de " + recibir.FechaHoraRegistro.ToString("MMMM") + " del " + recibir.FechaHoraRegistro.ToString("yyyy"),
                                            Hora = recibir.FechaHoraRegistro.ToString("hh:mm"),
                                            FolioNotaEntrada = objRecepcion.NotaEntradaAutorizada,
                                            NumeroCaja = recibir.NumeroCaja,
                                            PartidaContrato = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca && x.IdProveedor == item.IdProveedor).ToString().ToString() + 1,
                                            TipoPlaca = tipoPlaca_.TipoPlaca,
                                            TipoProblema = "Cantidad de Placas Incorrectas"
                                        }, pathPlantillas);
                                        if (generaArchivoEvento.ExecutionOK)
                                        {
                                            var archivo = new Archivos()
                                            {
                                                IdOrigen = insertaEvento.Data.IdEventoRecepcion,
                                                TablaOrigen = "CPL_REC_SOL_PLACAS_EV",
                                                Archivo = generaArchivoEvento.Data,
                                                Borrado = 0,
                                                Entidad = usuarioLogin.Entidad,
                                                NombreArchivo = generaArchivoEvento.Message
                                            };

                                            var inserArchivo = new Archivos_BL().InsertaArchivo(archivo);

                                        }                                       
                                         
                                        dbResponseJson.ExecutionOK = true;
                                        dbResponseJson.Data = "error";
                                        dbResponseJson.Message = "Se encontraron las siguientes validaciones al momento de recibir las placas";
                                        break;
                                    }

                                }

                                var solicitudPL_TP = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.Where(x => x.IdTipoPlaca == item.IdTipoPlaca && x.IdProveedor == item.IdProveedor).ToList();
                                if (solicitudPL_TP.Count > 0)
                                {
                                    foreach (var itemPL_Sol in solicitudPL_TP)
                                    {
                                        var generaRangoSolicitado = new GenerarPlacasRangos()
                                        {
                                            Cantidad = 0,
                                            CantidadSolicitadaNotaEntrada = 0,
                                            IdTipoPlaca = item.IdTipoPlaca,
                                            TipoPlaca = new TiposPlacas()
                                            {
                                                IdTipoPlaca = tipoPlaca_.IdTipoPlaca,
                                                MascaraPlaca = tipoPlaca_.MascaraPlaca,
                                                OrdenPlaca = tipoPlaca_.OrdenPlaca,
                                                OrdenSeriePlaca = tipoPlaca_.OrdenSeriePlaca
                                            },
                                            RangoInicial = itemPL_Sol.RangoInicial,
                                            RangoFinal = itemPL_Sol.RangoFinal
                                        };
                                        var placasCalculadasSolicitud = new AlgoritmoGeneracionPlacas_BL().GeneraPlacasRangos(generaRangoSolicitado).Data;

                                        var contadorPlacasSolicitud = 0;
                                        foreach (var itemPlacaSolicitud in placasCalculadasSolicitud)
                                        {
                                            if (placasCalculadas.Any(x=>x.NumeroPlaca == itemPlacaSolicitud.NumeroPlaca))
                                            {
                                                contadorPlacasSolicitud++;
                                            }
                                        }

                                        if (contadorPlacasSolicitud != placasCalculadas.Count)
                                        {
                                            TipoProblema++;
                                            var recibir = new RecepcionSolicitudesPlacas_Recibir
                                            {
                                                IdRecepcion = IdRecepcion,
                                                IdValidacionEvento = 0,
                                                IdEventoRecepcion = 0,
                                                IdDelegacionBanco = objRecepcion.IdDelegacionBanco,
                                                IdProveedor = objRecepcion.SolicitudesPlacas.IdProveedor,
                                                IdSolicitud = objRecepcion.IdSolicitud,
                                                IdTipoPlaca = item.IdTipoPlaca,
                                                CantidadLaminas = item.CantidadLaminas,
                                                FechaHoraRegistro = DateTime.Now,
                                                NumeroCaja = item.NumeroCaja,
                                                Rangos = item.RangosPlacas,
                                                Entidad = usuarioLogin.Entidad,
                                                RecepcionSolicitudesPlacas_Recibir_Validaciones = new RecepcionSolicitudesPlacas_Recibir_Validaciones()
                                                {
                                                    IdRecepcion = IdRecepcion,
                                                    IdEventoRecepcion = 0,
                                                    IdNotaEntrada = dbResponseNotaEntrada.IdNotaEntrada,
                                                    IdProveedor = objRecepcion.SolicitudesPlacas.IdProveedor,
                                                    IdTipoPlaca = item.IdTipoPlaca,
                                                    IdTipoProblema = 3,
                                                    CajaNdeM = item.NumeroCaja,
                                                    IdContrato = objRecepcion.SolicitudesPlacas.IdContrato,
                                                    Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                                    Horas = DateTime.Now.ToString("hh:mm"),
                                                    IdDelegacionesBancos = objRecepcion.IdDelegacionBanco,
                                                    IdRecepcionValidaciones = 0,
                                                    UsuarioNombre = usuarioLogin.Usuario,
                                                    PartidaContrato = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca && x.IdProveedor == item.IdProveedor).ToString().ToString() + 1,
                                                    NotaEntrada = objRecepcion.NotaEntradaAutorizada,
                                                    Usuario = new Usuarios()
                                                    {
                                                        Nombre = usuarioLogin.Nombre,
                                                        ClaveUsuario = usuarioLogin.ClaveUsuario,
                                                        NumeroEmpleado = usuarioLogin.NumeroEmpleado == null ? " " : usuarioLogin.NumeroEmpleado,
                                                        Puesto = usuarioLogin.Puesto == null ? " " : usuarioLogin.Puesto
                                                    }
                                                }
                                            };

                                            var insertaEvento = new SolicitudesPlacasRecepcion_BL().InsertaRecepcionSolicitudPlaca_Recibir(recibir, usuarioLogin);
                                            if (insertaEvento.ExecutionOK)
                                            {
                                                var generaArchivoEvento = Helper.GeneracionEnviosPDF.RecepcionPlacasEventos(new PlacasRecibir_DocumentoGeneradoEvento()
                                                {
                                                    DelegacionBanco = objRecepcion.DelegacionesBancos.NombreDelegacionBanco,
                                                    FolioContrato = objRecepcion.SolicitudesPlacas.Contratos.NumeroContrato,
                                                    NombreProveedor = objRecepcion.SolicitudesPlacas.Proveedores.NombreProveedor,
                                                    NumEmpleado = usuarioLogin.NumeroEmpleado,
                                                    Fecha = recibir.FechaHoraRegistro.ToString("dd") + " de " + recibir.FechaHoraRegistro.ToString("MMMM") + " del " + recibir.FechaHoraRegistro.ToString("yyyy"),
                                                    Hora = recibir.FechaHoraRegistro.ToString("hh:mm"),
                                                    FolioNotaEntrada = objRecepcion.NotaEntradaAutorizada,
                                                    NumeroCaja = recibir.NumeroCaja,
                                                    PartidaContrato = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca && x.IdProveedor == item.IdProveedor).ToString().ToString() + 1,
                                                    TipoPlaca = tipoPlaca_.TipoPlaca,
                                                    TipoProblema = "Rango de Placas Incorrectas"
                                                }, pathPlantillas);
                                                if (generaArchivoEvento.ExecutionOK)
                                                {
                                                    var archivo = new Archivos()
                                                    {
                                                        IdOrigen = insertaEvento.Data.IdEventoRecepcion,
                                                        TablaOrigen = "CPL_REC_SOL_PLACAS_EV",
                                                        Archivo = generaArchivoEvento.Data,
                                                        Borrado = 0,
                                                        Entidad = usuarioLogin.Entidad,
                                                        NombreArchivo = generaArchivoEvento.Message
                                                    };

                                                    var inserArchivo = new Archivos_BL().InsertaArchivo(archivo);
                                                }

                                                dbResponseJson.ExecutionOK = true;
                                                dbResponseJson.Data = "error";
                                                dbResponseJson.Message = "Se encontraron las siguientes validaciones al momento de recibir las placas";
                                            }
                                        }
                                    }

                                } 

                                if (TipoProblema == 0)
                                {
                                    //Si no hay problema guardamos la informacion de las placas generadas en el sistema
                                    foreach (var placa in placasCalculadas)
                                    {
                                        var dbResponseNotaEntradaDet = new NotasEntradasPlacas_BL().GetNotasEntradasPlacasDet_Folio(objRecepcion.NotaEntradaAutorizada, objRecepcion.Entidad).Data;
                                        var dbResponseOrdenCompraDet = new OrdenesCompra_BL().GetOrdenesCompraDet(objRecepcion.SolicitudesPlacas.IdOrdenCompra, objRecepcion.Entidad).Data;
                                        var inventarioPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Enc(objRecepcion.IdDelegacionBanco, objRecepcion.Entidad).Data;

                                        var inventarioPlacasDetalle = new InventarioPlacas_Detalle()
                                        {
                                            IdInventario = inventarioPlacas.IdInventario,
                                            IdInventarioDetalle = 0,
                                            IdEstatusPlacas = 1,
                                            NumeroPlaca = placa.NumeroPlaca,
                                            Existencia = 1,
                                            Serie = new AlgoritmoGeneracionPlacas_BL().GetSeriePlaca(placa.NumeroPlaca, tipoPlaca_).Data,
                                            FechaEntrada = DateTime.Now,
                                            NumeroPolizaInfofin = " ",
                                            PlacaContabilizada = false,
                                            IdTipoPlaca = item.IdTipoPlaca,
                                            CostoPlaca = dbResponseNotaEntradaDet.Any(x => x.IdTipoPlaca == item.IdTipoPlaca) ? dbResponseNotaEntradaDet.Where(x => x.IdTipoPlaca == item.IdTipoPlaca).FirstOrDefault().CostoPlaca : 0.0M,
                                            Entidad = objRecepcion.Entidad,
                                            InventarioPlacas_Relacion_NE_OC = new InventarioPlacas_Relacion_NE_OC()
                                            {
                                                idRelacionInventario = 0,
                                                IdInventarioDetalle = 0,
                                                Entidad = objRecepcion.Entidad,
                                                IdNotaEntrada = dbResponseNotaEntrada.IdNotaEntrada,
                                                IdOrdenCompra = objRecepcion.SolicitudesPlacas.IdOrdenCompra,
                                                RenglonNE = dbResponseNotaEntradaDet.Any(x => x.IdTipoPlaca == item.IdTipoPlaca) ? dbResponseNotaEntradaDet.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca) + 1 : 0,
                                                RenglonOC = dbResponseOrdenCompraDet.Any(x => x.IdTipoPlaca == item.IdTipoPlaca) ? dbResponseOrdenCompraDet.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca) + 1 : 0
                                            }
                                        };

                                        var insertPlacaInventario = new InventarioPlacas_BL().InsertPlacaInventario(inventarioPlacasDetalle);
                                        if (insertPlacaInventario.ExecutionOK)
                                        {
                                            dbResponseJson.ExecutionOK = true; 
                                            dbResponseJson.Data = "success";
                                            dbResponseJson.Message = "";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (validaRecibir.Data.Any(x => x.EsEvento == 1))
                            {
                                var recibir = new RecepcionSolicitudesPlacas_Recibir
                                {
                                    IdRecepcion = IdRecepcion,
                                    IdValidacionEvento = 0,
                                    IdEventoRecepcion = 0,
                                    IdDelegacionBanco = objRecepcion.IdDelegacionBanco,
                                    IdProveedor = objRecepcion.SolicitudesPlacas.IdProveedor,
                                    IdSolicitud = objRecepcion.IdSolicitud,
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    CantidadLaminas = item.CantidadLaminas,
                                    FechaHoraRegistro = DateTime.Now,
                                    NumeroCaja = item.NumeroCaja,
                                    Rangos = item.RangosPlacas,
                                    Entidad = usuarioLogin.Entidad,
                                    RecepcionSolicitudesPlacas_Recibir_Validaciones = new RecepcionSolicitudesPlacas_Recibir_Validaciones()
                                    {
                                        IdRecepcion = IdRecepcion,
                                        IdEventoRecepcion = 0,
                                        IdNotaEntrada = dbResponseNotaEntrada.IdNotaEntrada,
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        IdTipoProblema = 1,
                                        CajaNdeM = item.NumeroCaja,
                                        IdContrato = objRecepcion.SolicitudesPlacas.IdContrato,
                                        Fecha = DateTime.Now.ToString("dd/MM/yyyy"),
                                        Horas = DateTime.Now.ToString("hh:mm"),
                                        IdDelegacionesBancos = objRecepcion.IdDelegacionBanco,
                                        IdRecepcionValidaciones = 0,
                                        UsuarioNombre = usuarioLogin.Usuario,
                                        PartidaContrato = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca).ToString().ToString() + 1,
                                        NotaEntrada = objRecepcion.NotaEntradaAutorizada,
                                        Usuario = new Usuarios()
                                        {
                                            Nombre = usuarioLogin.Nombre,
                                            ClaveUsuario = usuarioLogin.ClaveUsuario,
                                            NumeroEmpleado = usuarioLogin.NumeroEmpleado == null ? " " : usuarioLogin.NumeroEmpleado,
                                            Puesto = usuarioLogin.Puesto == null ? " " : usuarioLogin.Puesto
                                        }
                                    }
                                };

                                var insertaEvento = new SolicitudesPlacasRecepcion_BL().InsertaRecepcionSolicitudPlaca_Recibir(recibir, usuarioLogin);
                                if (insertaEvento.ExecutionOK)
                                {

                                    var generaArchivoEvento = Helper.GeneracionEnviosPDF.RecepcionPlacasEventos(new PlacasRecibir_DocumentoGeneradoEvento()
                                    {
                                        DelegacionBanco = objRecepcion.DelegacionesBancos.NombreDelegacionBanco,
                                        FolioContrato = objRecepcion.SolicitudesPlacas.Contratos.NumeroContrato,
                                        NombreProveedor = objRecepcion.SolicitudesPlacas.Proveedores.NombreProveedor,
                                        NumEmpleado = usuarioLogin.NumeroEmpleado,
                                        Fecha = recibir.FechaHoraRegistro.ToString("dd") + " de " + recibir.FechaHoraRegistro.ToString("MMMM") + " del " + recibir.FechaHoraRegistro.ToString("yyyy"),
                                        Hora = recibir.FechaHoraRegistro.ToString("hh:mm"),
                                        FolioNotaEntrada = objRecepcion.NotaEntradaAutorizada,
                                        NumeroCaja = recibir.NumeroCaja,
                                        PartidaContrato = objRecepcion.SolicitudesPlacas.Contratos.Contratos_Detalle.FindIndex(x => x.IdTipoPlaca == item.IdTipoPlaca).ToString().ToString() + 1,
                                        TipoPlaca = tipoPlaca_.TipoPlaca,
                                        TipoProblema = "Cantidad de Placas Incorrectas"
                                    }, pathPlantillas);
                                    if (generaArchivoEvento.ExecutionOK)
                                    {
                                        var archivo = new Archivos()
                                        {
                                            IdOrigen = insertaEvento.Data.IdEventoRecepcion,
                                            TablaOrigen = "CPL_REC_SOL_PLACAS_EV",
                                            Archivo = generaArchivoEvento.Data,
                                            Borrado = 0,
                                            Entidad = usuarioLogin.Entidad,
                                            NombreArchivo = generaArchivoEvento.Message
                                        };

                                        var inserArchivo = new Archivos_BL().InsertaArchivo(archivo);
                                    }


                                    dbResponseJson.ExecutionOK = true;
                                    dbResponseJson.Data = "error";
                                    dbResponseJson.Message = "Se encontraron las siguientes validaciones al momento de recibir las placas"; 
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                dbResponseJson.ExecutionOK = false;
                dbResponseJson.Message = ex.Message;
                dbResponseJson.Data = "error";
            }
            return Json(dbResponseJson);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RecibirPlacas(int? IdRecepcion)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaAcceso)
            {
                return RedirectToAction("Index", "Login");
            }

            if (IdRecepcion == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM listadoRecepcionPlacas = new Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM();
            listadoRecepcionPlacas.Listado = new List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel>();
            listadoRecepcionPlacas.Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM = new Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM();
            listadoRecepcionPlacas.Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM.FechaHoraRegistro = DateTime.Now;
            listadoRecepcionPlacas.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoRecepcionPlacas.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoRecepcionPlacas.Listado = new List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel>();
            listadoRecepcionPlacas.ListadoCajasRecibidas = new List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel>();

            var objResponse = new SolicitudesPlacasRecepcion_BL().GetListadoPlacasRecibidas((int)IdRecepcion.Value, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                var objResponseRecepcion = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudPlacas((int)IdRecepcion.Value, usuarioLogin.Entidad).Data;
                listadoRecepcionPlacas.Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM.IdProveedor = objResponseRecepcion.SolicitudesPlacas.IdProveedor;
                listadoRecepcionPlacas.Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM.IdDelegacionBanco = objResponseRecepcion.IdDelegacionBanco;
                listadoRecepcionPlacas.ListadoTiposPlacasDDL = new Controles_DDL().GetTiposPlacasOrdenCompraSeleccionada_DDL(objResponseRecepcion.SolicitudesPlacas.IdOrdenCompra, objResponseRecepcion.SolicitudesPlacas.IdContrato, usuarioLogin.Entidad, "Seleccione").Data;

                listadoRecepcionPlacas.IdSolicitud = objResponseRecepcion.IdSolicitud;
                listadoRecepcionPlacas.IdRecepcion = (int)IdRecepcion.Value;
                foreach (var item in objResponse.Data)
                {
                    item.RecepcionSolicitudesPlacas = new RecepcionSolicitudesPlacas();
                    item.RecepcionSolicitudesPlacas = objResponseRecepcion;
                    listadoRecepcionPlacas.ListadoCajasRecibidas.Add(new Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel() + item);
                }
            }
            return View(listadoRecepcionPlacas);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEventoRecepcion"></param>
        /// <returns></returns>
        public ActionResult ValidacionesRecepcionPlacas(int? IdEventoRecepcion, int? IdRecepcion)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.RecibirPlacaAcceso)
            {
                return RedirectToAction("Index", "Login");
            }

            if (IdEventoRecepcion == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM listadoRecepcionPlacasValidacion = new Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM();
            listadoRecepcionPlacasValidacion.Listado = new List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesModel>();
            var objResponseRecepcion = new SolicitudesPlacasRecepcion_BL().GetRecepcionSolicitudPlacas((int)IdRecepcion.Value, usuarioLogin.Entidad).Data;
            listadoRecepcionPlacasValidacion.IdSolicitud = objResponseRecepcion.IdSolicitud;
            listadoRecepcionPlacasValidacion.IdRecepcion = (int)IdRecepcion.Value;

            var objResponse = new SolicitudesPlacasRecepcion_BL().GetRecepcionesSolicitudesPlacas_Recibir_ValidacionesList((int)IdEventoRecepcion.Value, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {

                foreach (var item in objResponse.Data)
                {
                    listadoRecepcionPlacasValidacion.Listado.Add(new Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesModel() + item);
                }
            }
            return View(listadoRecepcionPlacasValidacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"]; 
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.Eliminar)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<DBNull> response = new SolicitudesPlacasRecepcion_BL().DeleteRecepcionSolicitudPlaca(id.Value);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "SolicitudPlacas eliminado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message; 

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQR(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_RecepcionSolicitudesPlacasProveedor.Eliminar)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            //DBResponse<DBNull> response = new SolicitudesPlacasRecepcion_BL().DeleteSolicitudPlacas(id.Value);
            //if (response.ExecutionOK)
            //    TempData["MensajeAIndex"] = "SolicitudPlacas eliminado correctamente";
            //else
            //    TempData["MensajeAIndex"] = response.Message;

            TempData["MensajeAIndex"] = "QR Leído eliminado correctamente";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ArchivosCargadosValidacion(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dbResponse = new SolicitudesPlacasRecepcion_BL().GetRecepcionesSolicitudesPlacas_Recibir_Validaciones((int)id, usuarioLogin.Entidad);
            var detalleVM = new Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM();

            detalleVM += dbResponse.Data;

            detalleVM.IdValidacionEvento = (int)id;

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();


            return PartialView("PartialDetalleArchivosAdjuntos", detalleVM);
        }

        [HttpPost]
        public ActionResult ObservacionesCargadosValidacion(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var dbResponse = new SolicitudesPlacasRecepcion_BL().GetRecepcionesSolicitudesPlacas_Recibir_Validaciones((int)id, usuarioLogin.Entidad);
            var detalleVM = new Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM();

            detalleVM += dbResponse.Data;

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();


            return PartialView("PartialDetalleObservaciones", detalleVM);
        }
 
        [HttpPost]
        public JsonResult EliminarArchivo(int IdArchivo)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new Archivos_BL().EliminaArchivo(IdArchivo);

            return Json(dbResponse);
        }

        [HttpPost]
        public JsonResult GuardarArchivos(List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel> ListArchivosValidacionModel, int IdValidacionEvento)
        {
            var dbResponseGuardaArchivo = new DBResponse<string>();
            try
            {
                var usuarioLogin = (Usuarios)Session["UserSC"];
                foreach (var item in ListArchivosValidacionModel)
                {
                    var archivo = new Archivos()
                    {
                        IdOrigen = IdValidacionEvento,
                        TablaOrigen = "CPL_REC_SOL_PLACAS_EV_VAL",
                        Archivo = Convert.FromBase64String(item.ArchivoBase64),
                        Borrado = 0,
                        Entidad = usuarioLogin.Entidad,
                        NombreArchivo = item.NombreArchivo
                    };

                    var inserArchivo = new Archivos_BL().InsertaArchivo(archivo);
                }
                dbResponseGuardaArchivo.ExecutionOK = true;
            }
            catch (Exception ex)
            {
                dbResponseGuardaArchivo.ExecutionOK = false;
                dbResponseGuardaArchivo.Message = ex.Message;
            }

            return Json(dbResponseGuardaArchivo);
        }
        public ActionResult EliminarCaja(List<Detalle_SolicitudesPlacasRecepcion_RecibirPlacasVM> Listado)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var listado_ = new List<Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel>();
            var consecutivo = Listado.Count + 1;
            if (Listado.Count > 0)
            {
                foreach (var item in Listado)
                {
                    var delegacion = new Detalle_DelegacionesBancosVM();
                    delegacion += new DelegacionesBancos_BL().GetDelegacionBanco(usuarioLogin.Entidad, item.IdDelegacionBanco).Data;

                    var tipoPlaca = new Detalle_TiposPlacasVM();
                    tipoPlaca += new TiposPlacas_BL().GetTipoPlaca(usuarioLogin.Entidad, item.IdTipoPlaca).Data;

                    var proveedor = new Detalle_ProveedoresVM();
                    proveedor += new Proveedores_BL().GetProveedor(usuarioLogin.Entidad, item.IdProveedor).Data;

                    var obj = new Listado_SolicitudesPlacasRecepcion_RecibirPlacasModel();
                    obj.DelegacionesBancos = new Detalle_DelegacionesBancosVM();
                    obj.Proveedores = new Detalle_ProveedoresVM();
                    obj.TiposPlacas = new Detalle_TiposPlacasVM();
                    obj.FechaHoraRegistro = DateTime.Now;
                    obj.IdValidacionEvento = 0;
                    obj.IdEventoRecepcion = 0;
                    obj.Consecutivo = consecutivo;
                    obj.IdRecepcion = item.IdRecepcion;
                    obj.IdDelegacionBanco = item.IdDelegacionBanco;
                    obj.IdProveedor = item.IdProveedor;
                    obj.IdTipoPlaca = item.IdTipoPlaca;
                    obj.CantidadLaminas = item.CantidadLaminas;
                    obj.Rangos = item.RangosPlacas;
                    obj.RangoInicial = item.RangoInicial;
                    obj.RangoFinal = item.RangoFinal;
                    obj.DelegacionesBancos = delegacion;
                    obj.Proveedores = proveedor;
                    obj.TiposPlacas = tipoPlaca;
                    obj.NumeroCaja = item.NumeroCaja;
                    consecutivo++;
                    listado_.Add(obj);
                }

            }
            return PartialView("PartialDetalleRecibirCaja", listado_);
        }
        public JsonResult GuardarObservacion(RecepcionSolicitudesPlacas_Recibir_Validaciones_Observaciones observacion)
        {
            var dbResponseGuardaObs = new DBResponse<string>();
            try
            {
                var usuarioLogin = (Usuarios)Session["UserSC"];
                observacion.Entidad = usuarioLogin.Entidad; 
                var insertaObservacion = new SolicitudesPlacasRecepcion_BL().InsertaRecepcionSolicitudPlaca_Validaciones_Observacion(observacion);
                dbResponseGuardaObs.ExecutionOK = insertaObservacion.ExecutionOK;
                dbResponseGuardaObs.Message = insertaObservacion.Message;
            }
            catch (Exception ex)
            {
                dbResponseGuardaObs.ExecutionOK = false;
                dbResponseGuardaObs.Message = ex.Message;
            }

            return Json(dbResponseGuardaObs);
        }

        [HttpPost]
        public JsonResult EliminarObservacion(int IdObservacion)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new SolicitudesPlacasRecepcion_BL().DeleteRecepcionSolicitudPlaca_Validaciones_Observacion(IdObservacion);

            return Json(dbResponse);
        }


        [HttpPost]
        public JsonResult ValidaNumeroEntrada(string NumeroEntrada)
        {
            var responseValida = new DBResponse<NotasEntradasPlacas>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var dbResponse = new NotasEntradasPlacas_BL().GetNotasEntradasPlacasEnc_Folio(NumeroEntrada, usuarioLogin.Entidad);
            if (!dbResponse.ExecutionOK)
            {
                responseValida.ExecutionOK = false;
                responseValida.Message = "El folio de entrada no se encuentra en el Sistema, verifiquelo";
            }
            else
            {
                responseValida.ExecutionOK = true;
                responseValida.Data = dbResponse.Data;
            }
            return Json(dbResponse);
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

        [HttpPost]
        public JsonResult UploadFiles()
        {
            var dbResponseUpload = new DBResponse<List<UploadFiles>>();

            var archivoCargado = new List<UploadFiles>();
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        fname = Path.Combine(Server.MapPath("~/Content/Uploads/"), fname);
                        file.SaveAs(fname);

                        Byte[] bytes = System.IO.File.ReadAllBytes(fname);
                        String fileBase64 = Convert.ToBase64String(bytes);
                        archivoCargado.Add(new UploadFiles()
                        {
                            ArchivoCargado = fileBase64,
                            NombreArchivo = file.FileName
                        });

                        System.IO.File.Delete(fname);
                    }

                    if (archivoCargado.Count > 0)
                    {
                        dbResponseUpload.Data = archivoCargado;
                        dbResponseUpload.ExecutionOK = true;
                        dbResponseUpload.Message = "";
                    }
                }
                catch (Exception ex)
                {
                    archivoCargado.Add(new UploadFiles()
                    {
                        ArchivoCargado = "",
                        NombreArchivo = ""
                    });

                    dbResponseUpload.Data = archivoCargado;
                    dbResponseUpload.ExecutionOK = false;
                    dbResponseUpload.Message = ex.Message;
                }
            }
            else
            {
                archivoCargado.Add(new UploadFiles()
                {
                    ArchivoCargado = "",
                    NombreArchivo = ""
                });

                dbResponseUpload.Data = archivoCargado;
                dbResponseUpload.ExecutionOK = false;
                dbResponseUpload.Message = "No se ha seleccionado ningún archivo";
            }

            return Json(dbResponseUpload);

        }
    }
}
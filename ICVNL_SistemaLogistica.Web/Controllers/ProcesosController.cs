using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Services.Envio;
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using ICVNL_SistemaLogistica.Web.Entities.Services;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using ICVNL_SistemaLogistica.Web.Services;
using System.Linq;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class ProcesosController : Controller
    {
        // GET: Procesos
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult ObtenerTipoPlacasManual()
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            //if (!usuarioLogin.UsuariosPermisos.pantalla_mane)
            //{
            //    return RedirectToAction("Index", "Login");
            //}

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            var model = new Detalle_ProcesoManualObtenerTiposPlacasVM();
            if (TempData["MensajeAIndex"] != null && TempData["MensajeAIndex"].ToString() != "")
            {
                this.ShowNotificacion("success", "Información", TempData["MensajeAIndex"].ToString(), "4", "0");
                TempData["MensajeAIndex"] = null;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ObtenerTipoPlacasManual(Detalle_ProcesoManualObtenerTiposPlacasVM viewModel)
        {
            TempData["messages"] = new Dictionary<string, string[]>();
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (ModelState.IsValid)
            {
                var taskCargaNotaEntrada = await Task.Run(() => CargaNotasEntrada(usuarioLogin, int.Parse(viewModel.FolioNotaEntrada)));
                if (taskCargaNotaEntrada.ExecutionOK)
                {
                    TempData["MensajeAIndex"] = taskCargaNotaEntrada.Message + viewModel.FolioNotaEntrada;
                }
            }
            else
            {
                this.ShowNotificacion("error", "Error", "Por Favor, revise los errores", "4", "0");
                return View(viewModel);
            }

            return RedirectToAction("ObtenerTipoPlacasManual");
        }


        public async Task<DBResponse<NotasEntradasPlacas>> CargaNotasEntrada(Usuarios usuarios, int NumeroNotaEntrada)
        {
            var dbResponseNE = new DBResponse<NotasEntradasPlacas>();
            try
            { 
                var usuarioApi = new Usuarios_API_BL().GetUserAPI();
                if (usuarioApi.ExecutionOK)
                {
                    var getInfoNotaentrada = await Task.Run(() => GetNotasEntradasPlacas(usuarios.TokenInventario, usuarios, Helper.InfoApi.GetURL_InventariosApi(), NumeroNotaEntrada));
                    if (getInfoNotaentrada.ExecutionOK)
                    {
                        dbResponseNE.Data = new NotasEntradasPlacas();
                        dbResponseNE.ExecutionOK = true;
                        dbResponseNE.Message = "Se ha obtenido las placas de manera puntual del folio de nota de entrada: ";
                    }
                }
            }
            catch (Exception ex)
            {
                dbResponseNE.Data = new NotasEntradasPlacas();
                dbResponseNE.ExecutionOK = false;
                dbResponseNE.Message = ex.Message;
            }

            return dbResponseNE;
        }

        public async Task<DBResponse<List<NotasEntradasPlacas>>> GetNotasEntradasPlacas(Token token, Usuarios usuario, string URL_InventariosApi, int NumeroNotaEntrada)
        {
             var delegacionesBanco = new DelegacionesBancos_BL().GetDelegacionesBancos(1, usuario.Entidad);

            System.Console.WriteLine("Obteniendo información de los almacenes");

            var getAlmacenes = await Task.Run(() => GetAlmacenes(token, URL_InventariosApi));
            if (!getAlmacenes.ExecutionOK)
            {
                return new DBResponse<List<NotasEntradasPlacas>>();
            }

            var notasEntradasPlacas = new DBResponse<List<NotasEntradasPlacas>>();
            notasEntradasPlacas.Data = new List<NotasEntradasPlacas>();
            var _postNotasEntrada = new PostNotaEntradaInd()
            {
                AccessToken = token.infofin_token,
                NotaEntrada = new NotaEntradaInd()
                {
                    empresa = 1,
                    entrada = NumeroNotaEntrada,
                }
            };             

            var getNotasentradasApi = await Task.Run(() => ApiService.ApiPostNotasEntradasRequest<NotasEntradas>(URL_InventariosApi,
                                                                                                                  "Api/NotaEntrada/",
                                                                                                                  "ObtieneNotaEntrada",
                                                                                                                  token.access_token,
                                                                                                                  _postNotasEntrada));

            if (getNotasentradasApi.ExecutionOK)
            {
                var objListApi = ((ResponseNotasEntradas)getNotasentradasApi.Data).NotasEntradas;
                if (objListApi != null)
                {
                    foreach (var itemNE in objListApi)
                    {
                        var objEntrada = new NotasEntradasPlacas();
                        objEntrada += itemNE;
                        if (getAlmacenes.Data.Any(x => x.almacen == itemNE.almacen))
                        {
                            var centroCostos_ = getAlmacenes.Data.Where(x => x.almacen == itemNE.almacen).FirstOrDefault().centro_costo;
                            var dbresponseDelegacion = new DelegacionesBancos_BL().ExisteDelegacionBancoByCentroCostos(centroCostos_, usuario.Entidad);
                            if (dbresponseDelegacion.ExecutionOK)
                            {
                                objEntrada.IdDelegacionBanco = dbresponseDelegacion.Data.IdDelegacionBanco;
                            }
                        }
                        notasEntradasPlacas.Data.Add(objEntrada);
                    }
                }
            }

            return notasEntradasPlacas;
        }

        public async Task<DBResponse<List<Almacenes>>> GetAlmacenes(Token token, string UrlBase)
        {
            var almacneResponse = new DBResponse<List<Almacenes>>();
            var _postAlmacen = new PostAlmacenesListado()
            {
                AccessToken = token.infofin_token,
                Almacen = new AlmacenesListado
                {
                    empresa = 1
                }
            };

            var objAlmacen = new Almacenes();

            var getAlmacenesApi = await Task.Run(() => ApiService.ApiPostAlmacenesListadoRequest<ResponseListadoAlmacenes>(UrlBase,
                                                                                  "Api/Almacenes/",
                                                                                  "ListaAlmacenes",
                                                                                  token.access_token,
                                                                                  _postAlmacen));
            if (getAlmacenesApi.ExecutionOK)
            {
                almacneResponse.Data = ((ResponseListadoAlmacenes)getAlmacenesApi.Data).Almacenes;
                almacneResponse.ExecutionOK = true;
            }
            return almacneResponse;
        }
    }
}
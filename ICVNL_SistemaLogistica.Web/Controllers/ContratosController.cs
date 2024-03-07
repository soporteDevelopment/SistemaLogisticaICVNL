using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Entities.Helpers;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class ContratosController : Controller
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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Contratos.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            //Se limpian los valores de TempData ya que el botón restablecer datos ocupa esta acción
            TempData["FiltroIdContrato"] = TempData["FiltroIdContrato"] != null ? int.Parse(TempData["FiltroIdContrato"].ToString()) : 0;
            TempData["FiltroIdProveedor"] = TempData["FiltroIdProveedor"] != null ? int.Parse(TempData["FiltroIdProveedor"].ToString()) : 0;
            TempData["FiltroNumeroContrato"] = TempData["FiltroNumeroContrato"]?.ToString();

            //Se obtienen los valores de los combos solo en la carga inicial de la página,
            //en los POST se mantendrán estos valores
            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTiposPlacasDDL"] == null)
                TempData["ListadoTiposPlacasDDL"] = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetContrato("", new Listado_ContratosVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_ContratosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_Contratos.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(GetContrato(control, viewModel));
        }

        private Listado_ContratosVM GetContrato(string control, Listado_ContratosVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            Listado_ContratosVM listadoVM = new Listado_ContratosVM();
            listadoVM.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoProveedoresDDL"] == null)
                TempData["ListadoProveedoresDDL"] = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            //Se utliza TempData para guardar los valores de forma temporal
            TempData["FiltroIdContrato"] = viewModel.FiltroIdContrato;
            TempData["FiltroNumeroContrato"] = viewModel.FiltroNumeroContrato;
            TempData["FiltroIdProveedor"] = viewModel.FiltroIdProveedor;

            int FiltroIdContrato_ = string.IsNullOrEmpty(viewModel.FiltroIdContrato) ? 0 : int.Parse(viewModel.FiltroIdContrato);
            int FiltroIdProveedor_ = string.IsNullOrEmpty(viewModel.FiltroIdProveedor) ? 0 : int.Parse(viewModel.FiltroIdProveedor);
            string FiltroNumeroContrato_ = string.IsNullOrEmpty(viewModel.FiltroNumeroContrato) ? "" : viewModel.FiltroNumeroContrato;

            //Estos valores se guardan solo por 1 post y se eliminan al cambiar de controlador
            TempData["messages"] = new Dictionary<string, string[]>();

            DBResponse<List<Contratos>> response = new Contratos_BL().GetContratos(FiltroIdContrato_,
                                                                                   FiltroNumeroContrato_,
                                                                                   FiltroIdProveedor_,
                                                                                   usuarioLogin.Entidad);
            if (response.ExecutionOK)
            {
                List<Listado_ContratosModel> Contratos = new List<Listado_ContratosModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        Contratos.Add(new Listado_ContratosModel() + m);
                    }
                    listadoVM.Listado = Contratos;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? IdContrato, string op)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Contratos.Acceso)
            {
                return RedirectToAction("Index", "Home");
            }

            if (IdContrato == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_ContratosVM ContratosVM = new Detalle_ContratosVM();
            ContratosVM.DetalleContrato.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            ContratosVM.DetalleContrato.ListadoTiposPlacasDDL = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            return View(ContratosVM);
        }
        public ActionResult Create()
        {
            TempData["messages"] = new Dictionary<string, string[]>();

            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Contratos.Registrar)
            {
                return RedirectToAction("Index", "Home");
            }
             
            Detalle_ContratosVM viewModel = new Detalle_ContratosVM();
            viewModel.DetalleContrato.ListadoProveedoresDDL = new Controles_DDL().GetProveedores_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            viewModel.DetalleContrato.ListadoTiposPlacasDDL = new Controles_DDL().GetTiposPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            return View(viewModel);
        }

        [HttpPost]
        public JsonResult ContratoDetails(int IdContrato)
        {
            var responseContrato = new DBResponse<Detalle_ContratosVM>();
            responseContrato.Data = new Detalle_ContratosVM();
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponse = new Contratos_BL().GetContrato(IdContrato, usuarioLogin.Entidad);
            Detalle_ContratosVM contratosVM = new Detalle_ContratosVM();
            if (dbResponse.ExecutionOK)
            {
                responseContrato.Data += dbResponse.Data;
                responseContrato.ExecutionOK = true;
                responseContrato.NumRows = 1;
            }
            else
            {
                responseContrato.Data = new Detalle_ContratosVM();
                responseContrato.ExecutionOK = false;
                responseContrato.NumRows = 1;
            }
            return Json(responseContrato);
        }

        [HttpPost]
        public JsonResult UpsertContrato(Detalle_ContratosVM viewModel)
        {
            var dbResponse = new DBResponse<Contratos>();
            try
            {
                var usuarioLogin = (Usuarios)Session["UserSC"];
                var objContratos = new Contratos()
                {
                    IdContrato = viewModel.IdContrato,
                    NumeroContrato = viewModel.NumeroContrato,
                    Entidad = usuarioLogin.Entidad,
                    FechaContrato = DateTime.Now,
                    Usuario = usuarioLogin.Usuario
                };
                objContratos.Contratos_Archivos = new List<Contratos_Archivos>();
                objContratos.Contratos_Detalle = new List<Contratos_Detalle>();
                var consecutivo = 0;
                foreach (var item in viewModel.Detalle_ContratosArchivosVM)
                {
                    objContratos.Contratos_Archivos.Add(new Contratos_Archivos()
                    {
                        IdContrato = viewModel.IdContrato,
                        Consecutivo = consecutivo,
                        NombreArchivo = item.NombreArchivo,
                        Archivo = System.Convert.FromBase64String(item.ArchivoBase64)
                    });
                    consecutivo++;
                }
                consecutivo = 0;
                foreach (var item in viewModel.Detalle_ContratosDetailsVM)
                {
                    var obj = new Contratos_Detalle()
                    {
                        IdContratoDetalle = item.IdContratoDetalle,
                        IdContrato = viewModel.IdContrato,
                        Consecutivo = consecutivo,
                        CantidadPlacasCaja = item.TipoPlacas.CantidadPlacasCaja,
                        CantidadPlacas = item.CantidadPlacas,
                        IdProveedor = item.IdProveedor,
                        IdTipoPlaca = item.IdTipoPlaca,
                        MascaraPlaca = item.TipoPlacas.MascaraPlaca,
                        OrdenPlaca = item.TipoPlacas.OrdenPlaca,
                        OficioSICT = item.OficioSICT,
                        RangoFinal = item.RangoFinal,
                        RangoInicial = item.RangoInicial,
                        Entidad = usuarioLogin.Entidad
                    };
                    obj.Contratos_Detalles_Rangos = new List<Contratos_Detalles_Rangos>();

                    var consecutivoRng = 0;
                    if (item.Detalle_ContratosDetailsRangosVM != null)
                    {
                        foreach (var itemRango in item.Detalle_ContratosDetailsRangosVM)
                        {
                            obj.Contratos_Detalles_Rangos.Add(new Contratos_Detalles_Rangos()
                            {
                                Consecutivo = consecutivoRng,
                                IdContratoDetalleRangos = itemRango.IdContratoDetalleRangos,
                                IdContratoDetalle = itemRango.IdContratoDetalle,
                                CantidadSerie = itemRango.CantidadSerie,
                                RangoFinal = itemRango.RangoFinal,
                                RangoInicial = itemRango.RangoFinal,
                                Entidad = usuarioLogin.Entidad
                            });
                            consecutivoRng++;
                        }
                    }

                    objContratos.Contratos_Detalle.Add(obj);
                    consecutivo++;
                }
                var esNuevo = viewModel.IdContrato == 0;
                var responseUpsert = new Contratos_BL().UpsertContrato(objContratos, usuarioLogin, esNuevo);
                if (responseUpsert.ExecutionOK)
                {
                    dbResponse.Data = objContratos;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Contrato generado correctamente";
                    return Json(dbResponse);
                }
                else
                {
                    dbResponse.Data = objContratos;
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Ocurrio un error al guardar el Contrato";
                    return Json(dbResponse);
                }
            }
            catch (Exception ex)
            {
                dbResponse.Data = new Contratos();
                dbResponse.ExecutionOK = true;
                dbResponse.Message = ex.Message;
                return Json(dbResponse);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_Contratos.Eliminar)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<DBNull> response = new Contratos_BL().DeleteContrato(id.Value, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Contrato eliminado correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;


            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetTipoPlaca(int idTipoPlaca)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            try
            {
                var dbResponseTipoPlaca = new TiposPlacas_BL().GetTipoPlaca(usuarioLogin.Entidad, idTipoPlaca);
                if (dbResponseTipoPlaca.ExecutionOK)
                {
                    return Json(dbResponseTipoPlaca);
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
        public JsonResult ValidaRangosPlacas(Listado_ContratosDetailsModel DetalleContrato)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponseConteo = new DBResponse<Boolean>();
            try
            {
                GenerarPlacasRangos PlacasRangos = new GenerarPlacasRangos();
                PlacasRangos.IdTipoPlaca = DetalleContrato.IdTipoPlaca;
                PlacasRangos.TipoPlaca = new TiposPlacas()
                {
                    IdTipoPlaca = DetalleContrato.IdTipoPlaca,
                    MascaraPlaca = DetalleContrato.TipoPlacas.MascaraPlaca,
                    OrdenPlaca = DetalleContrato.TipoPlacas.OrdenPlaca
                };
                PlacasRangos.RangoInicial = DetalleContrato.RangoInicial;
                PlacasRangos.RangoFinal = DetalleContrato.RangoFinal;

                var dbResponseValida = new AlgoritmoGeneracionPlacas_BL().Validaciones(PlacasRangos);
                if (dbResponseValida.ExecutionOK)
                {
                    var calculaPlacas = new AlgoritmoGeneracionPlacas_BL().GeneraPlacasRangos(PlacasRangos);
                    if (calculaPlacas.ExecutionOK)
                    {
                        ContratosDetalle_ConteoPlacas conteo = new ContratosDetalle_ConteoPlacas()
                        {
                            ConteoRangoPlacas = calculaPlacas.Data.Count,
                            ConteoRangoPlacasTipoPlacaSelect = DetalleContrato.Detalle_ContratosDetailsRangosVM.Sum(x => int.Parse(x.CantidadSerie))
                        };
                        if (conteo.ConteoRangoPlacas != conteo.ConteoRangoPlacasTipoPlacaSelect)
                        {
                            dbResponseConteo.ExecutionOK = false;
                            dbResponseConteo.Data = false;
                            dbResponseConteo.Message = "La cantidad de placas por tipo de rango de placas seleccionado es distinta al rango de placas del detalle del contrato";
                            return Json(dbResponseConteo);
                        }
                        else
                        {
                            dbResponseConteo.ExecutionOK = true;
                            dbResponseConteo.Data = true;
                        }
                    }
                    else
                    {
                        dbResponseConteo.ExecutionOK = false;
                        dbResponseConteo.Message = calculaPlacas.Message;
                    }
                }
                else
                {
                    dbResponseConteo.ExecutionOK = false;
                    dbResponseConteo.Message = dbResponseValida.Message;
                }

                return Json(dbResponseConteo);
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
        public JsonResult ValidaGuardadoRangosPlacas(List<Listado_ContratosDetailsModel> ListContratosDetailsModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var dbResponseConteo = new DBResponse<Boolean>();
            try
            { 
                foreach (var item in ListContratosDetailsModel)
                {
                    GenerarPlacasRangos PlacasRangos = new GenerarPlacasRangos();
                    PlacasRangos.IdTipoPlaca = item.IdTipoPlaca;
                    PlacasRangos.TipoPlaca = new TiposPlacas()
                    {
                        IdTipoPlaca = item.IdTipoPlaca,
                        MascaraPlaca = item.TipoPlacas.MascaraPlaca,
                        OrdenPlaca = item.TipoPlacas.OrdenPlaca
                    };
                    PlacasRangos.RangoInicial = item.RangoInicial;
                    PlacasRangos.RangoFinal = item.RangoFinal;

                    var dbResponseValida = new AlgoritmoGeneracionPlacas_BL().Validaciones(PlacasRangos);
                    if (dbResponseValida.ExecutionOK)
                    {
                        var calculaPlacas = new AlgoritmoGeneracionPlacas_BL().GeneraPlacasRangos(PlacasRangos);
                        if (calculaPlacas.ExecutionOK)
                        {
                            ContratosDetalle_ConteoPlacas conteo = new ContratosDetalle_ConteoPlacas()
                            {
                                ConteoRangoPlacas = calculaPlacas.Data.Count,
                                ConteoRangoPlacasTipoPlacaSelect = item.Detalle_ContratosDetailsRangosVM.Sum(x => int.Parse(x.CantidadSerie))
                            };
                            if (conteo.ConteoRangoPlacas != conteo.ConteoRangoPlacasTipoPlacaSelect)
                            {
                                dbResponseConteo.ExecutionOK = false;
                                dbResponseConteo.Data = false;
                                dbResponseConteo.Message = "La cantidad de placas por tipo de rango de placas seleccionado es distinta al rango de placas del detalle del contrato";
                                break;
                            }
                            else
                            {
                                dbResponseConteo.ExecutionOK = true;
                                dbResponseConteo.Data = true;
                            }
                        }
                        else
                        {
                            dbResponseConteo.ExecutionOK = false;
                            dbResponseConteo.Message = calculaPlacas.Message;
                        } 
                    }
                    else
                    {
                        dbResponseConteo.ExecutionOK = false;
                        dbResponseConteo.Message = dbResponseValida.Message;
                    }
                }

                return Json(dbResponseConteo);
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
        public ActionResult GuardarDetalleTemp(List<Listado_ContratosDetailsModel> ListContratosDetailsModel)
        {
            return PartialView("PartialDetalleContrato", ListContratosDetailsModel);
        }
        [HttpPost]
        public ActionResult GuardarRangoDetalleTemp(List<Listado_ContratosDetailsRangosModel> ListContratosDetailsRangosModel)
        {
            return PartialView("PartialDetalleTiposPlacasRangos", ListContratosDetailsRangosModel);
        }
        public ActionResult GuardarArchivosTemp(List<Listado_ContratosArchivosModel> ListContratosArchivosModel)
        {
            return PartialView("PartialArchivosAdjuntos", ListContratosArchivosModel);
        }
        public ActionResult DeleteArchivo(List<Listado_ContratosArchivosModel> ListContratosArchivosModel)
        {
            return PartialView("PartialArchivosAdjuntos", ListContratosArchivosModel);
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
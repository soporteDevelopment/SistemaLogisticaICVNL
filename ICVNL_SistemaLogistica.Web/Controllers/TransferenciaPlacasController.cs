using ICVNL_SistemaLogistica.Web.BL;
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Helper;
using ICVNL_SistemaLogistica.Web.Models;
using ICVNL_SistemaLogistica.Web.ViewModels;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ICVNL_SistemaLogistica.Web.Controllers
{
    public class TransferenciaPlacasController : Controller
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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Acceso)
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


            if (TempData["ListadoDelegacionesUsuarioDDL"] == null)
                TempData["ListadoDelegacionesUsuarioDDL"] = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusTransferenciaDDL"] == null)
                TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoEstatusPlacasDDL"] == null)
                TempData["ListadoEstatusPlacasDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            TempData["SortOrder"] = TempData["SortOrder"] != null ? TempData["SortOrder"].ToString() : " DESC";
            TempData["SortField"] = TempData["SortField"]?.ToString();

            return View(GetTransferenciaPlaca("", new Listado_TransferenciaPlacasVM()));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string control, Listado_TransferenciaPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];

            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(GetTransferenciaPlaca(control, viewModel));
        }

        private Listado_TransferenciaPlacasVM GetTransferenciaPlaca(string control, Listado_TransferenciaPlacasVM viewModel)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            Listado_TransferenciaPlacasVM listadoVM = new Listado_TransferenciaPlacasVM();
            listadoVM = viewModel;
            listadoVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoDelegacionesUsuarioDDL = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
            listadoVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            listadoVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoTiposIDsDDL"] == null)
                TempData["ListadoTiposIDsDDL"] = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            if (TempData["ListadoDelegacionesUsuarioDDL"] == null)
                TempData["ListadoDelegacionesUsuarioDDL"] = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;

            if (TempData["ListadoDelegacionesDDL"] == null)
                TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;

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


            var fechaInicio_ = DateTime.Parse("01/01/2000");
            var fechaFin_ = DateTime.Parse("01/01/2900");
            if (!String.IsNullOrEmpty(viewModel.FiltroFechaTransferencia))
            {
                fechaInicio_ = DateTime.Parse(viewModel.FiltroFechaTransferencia.Split('-')[0].Trim());
                fechaFin_ = DateTime.Parse(viewModel.FiltroFechaTransferencia.Split('-')[1].Trim());
            }

            String Folio_ = listadoVM.FiltroFolioTransferencia == null ? "" : listadoVM.FiltroFolioTransferencia;
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
                                                                                                                  "spcpl_transf_placas_pl.consulta_listado");
            if (response.ExecutionOK)
            {
                List<Listado_TransferenciaPlacasModel> TransferenciaPlacas = new List<Listado_TransferenciaPlacasModel>();
                if (response.Data != null && response.Data.Count > 0)
                {
                    foreach (var m in response.Data)
                    {
                        TransferenciaPlacas.Add(new Listado_TransferenciaPlacasModel() + m);
                    }
                    listadoVM.Listado = TransferenciaPlacas;
                }
            }

            TempData["ListadoTiposIDsDDL"] = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoDelegacionesUsuarioDDL"] = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
            TempData["ListadoDelegacionesDDL"] = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoEstatusTransferenciaDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TempData["ListadoEstatusPlacasDDL"] = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;


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
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Registrar)
            {
                return RedirectToAction("Index", "Login");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_TransferenciaPlacasVM TransferenciaPlacasVM = new Detalle_TransferenciaPlacasVM();
            TransferenciaPlacasVM.FolioTransferencia = new TransferenciasPlacas_BL().GetTransferenciaSiguienteFolio().Data.FolioSolicitud;
            TransferenciaPlacasVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesUsuarioDDL = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            return View(TransferenciaPlacasVM);
        }

        public ActionResult Details(int IdTransferencia)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Acceso)
            {
                return RedirectToAction("Index", "Login");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_TransferenciaPlacasVM TransferenciaPlacasVM = new Detalle_TransferenciaPlacasVM();
            var transferenciaDB = new TransferenciasPlacas_BL().GetTransferenciaPlaca_Details(IdTransferencia, usuarioLogin.Entidad);
            if (transferenciaDB.ExecutionOK)
            {
                TransferenciaPlacasVM.IdEstatusTransferencia = transferenciaDB.Data.IdEstatusTransferencia;
            }
            TransferenciaPlacasVM.FolioTransferencia = new TransferenciasPlacas_BL().GetTransferenciaSiguienteFolio().Data.FolioSolicitud;
            TransferenciaPlacasVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesUsuarioDDL = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            return View(TransferenciaPlacasVM);
        } 

        [HttpPost]
        public JsonResult UpsertTransferenciaPlaca(Detalle_TransferenciaPlacasVM viewModel)
        {
            var dbResponse = new DBResponse<int>();
            try
            {
                var usuarioLogin = (Usuarios)Session["UserSC"];

                var objTransferenciaPlacas = new TransferenciaPlacas()
                {
                    IdTransferencia = viewModel.IdTransferencia,
                    FechaHoraRegistro = DateTime.Now,
                    FolioTransferencia = viewModel.FolioTransferencia,
                    IdDelegacionBancoDestino = viewModel.IdDelegacionBancoDestino,
                    IdDelegacionBancoOrigen = viewModel.IdDelegacionBancoOrigen,
                    IdEstatusTransferencia = 1,
                    IdTransferenciaDatosPersona = viewModel.IdTransferenciaDatosPersonaEnvio,
                    IdTransferenciaTransporte = viewModel.IdTransferenciaTransporteEnvio,
                    Entidad = usuarioLogin.Entidad,
                    UsuarioRegistro = usuarioLogin.Usuario,
                    TransferenciaPlacas_DatosPersona = new TransferenciaPlacas_DatosPersona()
                    {
                        IdTransferencia = viewModel.IdTransferencia,
                        IdTransferenciaDatosPersona = 0,
                        IdTipoIDs = viewModel.TransferenciaPlacas_DatosPersonaEnvio.IdTipoIDs,
                        Apellido = viewModel.TransferenciaPlacas_DatosPersonaEnvio.Apellido,
                        Nombre = viewModel.TransferenciaPlacas_DatosPersonaEnvio.Nombre,
                        NumeroID = viewModel.TransferenciaPlacas_DatosPersonaEnvio.NumeroID,
                        Tipo = "Envia",
                        Entidad = usuarioLogin.Entidad
                    },
                    TransferenciaPlacas_Transporte = new TransferenciaPlacas_Transporte()
                    {
                        IdTransferencia = viewModel.IdTransferencia,
                        IdTransferenciaTransporte = 0,
                        MarcaVehiculo = viewModel.TransferenciaPlacas_TransporteEnvio.MarcaVehiculo ?? " ",
                        ModeloVehiculo = viewModel.TransferenciaPlacas_TransporteEnvio.ModeloVehiculo ?? " ",
                        NumeroEconomico = viewModel.TransferenciaPlacas_TransporteEnvio.NumeroEconomico ?? " ",
                        PlacasVehiculo = viewModel.TransferenciaPlacas_TransporteEnvio.PlacasVehiculo ?? " ",
                        Tipo = "Envia",
                        Entidad = usuarioLogin.Entidad
                    }
                };
                objTransferenciaPlacas.TransferenciaPlacas_Detalle = new List<TransferenciaPlacas_Detalle>(); 

                
                foreach (var item in viewModel.TransferenciaPlacas_Listado2)
                {              
                    objTransferenciaPlacas.TransferenciaPlacas_Detalle.Add(new TransferenciaPlacas_Detalle()
                    {
                        IdTransferencia = 0,
                        Automatico = item.Automatico,
                        IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                        IdTipoPlaca = item.IdTipoPlaca,
                        NumeroPlaca = item.NumeroPlaca,
                        TransferirPlaca = item.TransferirPlaca,
                        Entidad = usuarioLogin.Entidad,
                        FechaEstatusPlaca = new DateTime(1900, 01, 01),
                        TransferirPlacaIndividual = 0,
                        PlacaSeleccionadaManual = true,
                        UsuarioRecibio = " ",
                        IdTransferenciaDet = 0
                    });                                     
                }


                foreach (var item in viewModel.TransferenciaPlacas_Listado1)
                {
                    var countAutomaticos = objTransferenciaPlacas.TransferenciaPlacas_Detalle.Where(x => x.IdTipoPlaca == item.IdTipoPlaca && x.Automatico).Count();
                    if (item.CantidadDisponiblesSerTransferidas == 0)
                    {
                        for (int i = 0; i < countAutomaticos; i++)
                        {
                            var objDet = objTransferenciaPlacas.TransferenciaPlacas_Detalle.Where(x => x.Automatico).FirstOrDefault();
                            var index = objTransferenciaPlacas.TransferenciaPlacas_Detalle.FindIndex(x => x.NumeroPlaca == objDet.NumeroPlaca);

                            objTransferenciaPlacas.TransferenciaPlacas_Detalle[index].TransferirPlaca = false;
                            objTransferenciaPlacas.TransferenciaPlacas_Detalle[index].Automatico = false;
                        }
                    }
                    else
                    {

                        if (countAutomaticos != item.CantidadDisponiblesSerTransferidas)
                        {
                            if (item.CantidadDisponiblesSerTransferidas > countAutomaticos)
                            {
                                var diff = item.CantidadDisponiblesSerTransferidas - countAutomaticos;
                                for (int i = 0; i < diff; i++)
                                {
                                    var detalleTransferencia = new TransferenciaPlacas_Detalle()
                                    {
                                        IdTransferenciaDet = 0,
                                        IdTransferencia = viewModel.IdTransferencia,
                                        Automatico = true,
                                        Entidad = usuarioLogin.Entidad,
                                        FechaEstatusPlaca = new DateTime(1900, 01, 01),
                                        IdTipoEstatusPlacas = 1,
                                        TransferirPlaca = true,
                                        PlacaSeleccionadaManual = false,
                                        TransferirPlacaIndividual = 0,
                                        UsuarioRecibio = " ",
                                        IdTipoPlaca = item.IdTipoPlaca,
                                        NumeroPlaca = " "
                                    };
                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle.Add(detalleTransferencia);
                                }
                            }
                            else
                            {
                                var diff = countAutomaticos - item.CantidadDisponiblesSerTransferidas;
                                for (int i = 0; i < diff; i++)
                                {
                                    var objDet = objTransferenciaPlacas.TransferenciaPlacas_Detalle.Where(x => x.Automatico).FirstOrDefault();
                                    var index = objTransferenciaPlacas.TransferenciaPlacas_Detalle.FindIndex(x => x.NumeroPlaca == objDet.NumeroPlaca);

                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle[index].TransferirPlaca = false;
                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle[index].Automatico = false;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < item.CantidadDisponiblesSerTransferidas; i++)
                            {
                                var detalleTransferencia = new TransferenciaPlacas_Detalle()
                                {
                                    IdTransferenciaDet = 0,
                                    IdTransferencia = viewModel.IdTransferencia,
                                    Automatico = true,
                                    Entidad = usuarioLogin.Entidad,
                                    FechaEstatusPlaca = new DateTime(1900, 01, 01),
                                    IdTipoEstatusPlacas = 1,
                                    TransferirPlaca = true,
                                    PlacaSeleccionadaManual = false,
                                    TransferirPlacaIndividual = 0,
                                    UsuarioRecibio = " ",
                                    IdTipoPlaca = item.IdTipoPlaca,
                                    NumeroPlaca = " "
                                };
                                objTransferenciaPlacas.TransferenciaPlacas_Detalle.Add(detalleTransferencia);
                            }
                        }
                    }
                }
                var esNuevo = viewModel.IdTransferencia == 0;

                var upsert = new TransferenciasPlacas_BL().UpsertTransferenciaPlaca(objTransferenciaPlacas, esNuevo, usuarioLogin);
                if (upsert.ExecutionOK)
                {
                    dbResponse.Data = upsert.Data.IdTransferencia;
                    dbResponse.ExecutionOK = true;
                    dbResponse.Message = "Transferencia de Placa entre Delegaciones agregada correctamente: " + upsert.Data.IdTransferencia;
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Ocurrio un error al guardar la Transferencia";
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }

            return Json(dbResponse);
        }

        [HttpPost]
        public JsonResult GeneraPackingList(int IdTransferencia)
        {
            var dbResponse = new DBResponse<int>();
            try
            {
                var usuarioLogin = (Usuarios)Session["UserSC"];

                var viewModel = new TransferenciasPlacas_BL().GetTransferenciaPlaca_Details(IdTransferencia, usuarioLogin.Entidad).Data;

                var objTransferenciaPlacas = new TransferenciaPlacas();
                objTransferenciaPlacas = viewModel;
                objTransferenciaPlacas.Entidad = usuarioLogin.Entidad;
                objTransferenciaPlacas.TransferenciaPlacas_Detalle = new List<TransferenciaPlacas_Detalle>();


                foreach (var item in viewModel.TransferenciaPlacas_Listado2)
                {
                    if (item.TransferirPlaca)
                    {
                        objTransferenciaPlacas.TransferenciaPlacas_Detalle.Add(new TransferenciaPlacas_Detalle()
                        {
                            IdTransferencia = 0,
                            Automatico = false,
                            IdTipoEstatusPlacas = item.IdTipoEstatusPlacas,
                            IdTipoPlaca = item.IdTipoPlaca,
                            NumeroPlaca = item.NumeroPlaca,
                            TransferirPlaca = item.TransferirPlaca,
                            Entidad = usuarioLogin.Entidad,
                            FechaEstatusPlaca = new DateTime(1900, 01, 01),
                            TransferirPlacaIndividual = 0,
                            UsuarioRecibio = " ",
                            IdTransferenciaDet = 0
                        });
                    }
                }

                foreach (var item in viewModel.TransferenciaPlacas_Listado1.Where(x => x.CantidadDisponiblesSerTransferidas > 0))
                {
                    for (int i = 0; i < item.CantidadDisponiblesSerTransferidas; i++)
                    {
                        var detalleTransferencia = new TransferenciaPlacas_Detalle()
                        {
                            IdTransferenciaDet = 0,
                            IdTransferencia = viewModel.IdTransferencia,
                            Automatico = true,
                            Entidad = usuarioLogin.Entidad,
                            FechaEstatusPlaca = new DateTime(1900, 01, 01),
                            IdTipoEstatusPlacas = 6,
                            TransferirPlaca = true,
                            TransferirPlacaIndividual = 0,
                            UsuarioRecibio = " ",
                            IdTipoPlaca = item.IdTipoPlaca,
                            NumeroPlaca = " "
                        };
                        objTransferenciaPlacas.TransferenciaPlacas_Detalle.Add(detalleTransferencia);
                    }
                }

                var getPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Detalle(objTransferenciaPlacas.IdDelegacionBancoOrigen, usuarioLogin.Entidad).Data;
                if (objTransferenciaPlacas.TransferenciaPlacas_Detalle.Any(x=>x.Automatico))
                {
                    for (int i = 0; i < objTransferenciaPlacas.TransferenciaPlacas_Detalle.Count(); i++)
                    {
                        var objDet = objTransferenciaPlacas.TransferenciaPlacas_Detalle[i];
                        if (objDet.Automatico)
                        {
                            foreach (var itemPlacaAsignar in getPlacas.Where(x=> x.IdTipoPlaca == objDet.IdTipoPlaca && x.IdEstatusPlacas == 1))
                            {
                                if (!objTransferenciaPlacas.TransferenciaPlacas_Detalle.Any(x => x.NumeroPlaca == itemPlacaAsignar.NumeroPlaca))
                                {
                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle[i].NumeroPlaca = itemPlacaAsignar.NumeroPlaca;
                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle[i].PlacaSeleccionadaAutomatica =  true;                        
                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle[i].Automatico =  true;
                                    objTransferenciaPlacas.TransferenciaPlacas_Detalle[i].TransferirPlaca =  true;
                                    break;
                                }
                            }
                        }
                    }
                } 

                var upsert = new TransferenciasPlacas_BL().InsertPackingList_TransferenciaPlaca(objTransferenciaPlacas, usuarioLogin);
                if (upsert.ExecutionOK)
                {
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "Ocurrio un error al guardar la Transferencia";
                }
            }
            catch (Exception ex)
            {
                dbResponse.ExecutionOK = false;
                dbResponse.Message = ex.Message;
            }

            return Json(dbResponse);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TransferenciaDetails(int? IdTransferencia)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
 
            var dbResponse = new DBResponse<Detalle_TransferenciaPlacasVM>();
            dbResponse.Data = new Detalle_TransferenciaPlacasVM();
            try
            {
                Detalle_TransferenciaPlacasVM TransferenciaPlacasVM = new Detalle_TransferenciaPlacasVM();

                TransferenciaPlacasVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
                TransferenciaPlacasVM.ListadoDelegacionesUsuarioDDL = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
                TransferenciaPlacasVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
                TransferenciaPlacasVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
                TransferenciaPlacasVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
                TransferenciaPlacasVM.FechaHoraRegistro = DateTime.Now;
                var objResponse = new TransferenciasPlacas_BL().GetTransferenciaPlaca_Details((int)IdTransferencia.Value, usuarioLogin.Entidad);
                if (objResponse.ExecutionOK)
                {
                    dbResponse.Data += objResponse.Data;
                    dbResponse.ExecutionOK = true;
                }
                else
                {
                    dbResponse.Data = new Detalle_TransferenciaPlacasVM();
                    dbResponse.ExecutionOK = false;
                    dbResponse.Message = "No se encontro información";
                }
            }
            catch (Exception ex )
            {
                dbResponse.Data = new Detalle_TransferenciaPlacasVM();
                dbResponse.ExecutionOK = false;
                dbResponse.Message = "Ocurrio un error al obtener la información " + ex.Message;
            }

            return Json(dbResponse);
        } 

        [HttpPost]
        public JsonResult GetPlacasDelegacionOrigen(int IdDelegeacionOrigen)
        {
            var usuarioLogin = (Usuarios)Session["UserSC"];
            var placasResponse = new DBResponse<Placas_DelegacionOrigen_Listado>
            {
                Data = new Placas_DelegacionOrigen_Listado
                {
                    TransferenciaPlacas_Listado1 = new List<TransferenciaPlacas_Listado1>(),
                    TransferenciaPlacas_Listado2 = new List<TransferenciaPlacas_Listado2>()
                }
            };

            var getPlacas = new InventarioPlacas_BL().GetInventarioPlacas_Transferencia(IdDelegeacionOrigen, usuarioLogin.Entidad);
            if (getPlacas.ExecutionOK)
            {
                var listPlacas = getPlacas.Data;

                var getTiposPlacas = new TiposPlacas_BL().GetTiposPlacas(1, usuarioLogin.Entidad).Data;


                placasResponse.Data.TransferenciaPlacas_Listado1 = getTiposPlacas.GroupBy(x => new { x.IdTipoPlaca })
                                            .Select(w => new TransferenciaPlacas_Listado1
                                            {
                                                IdTransferenciaListado1 = 0,
                                                IdTransferencia = 0,
                                                IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                TiposPlacas = new TiposPlacas()
                                                {
                                                    IdTipoPlaca = w.FirstOrDefault().IdTipoPlaca,
                                                    TipoPlaca = w.FirstOrDefault().TipoPlaca
                                                },
                                                CantidadDisponiblesDelegacion = listPlacas.Where(x => x.IdEstatusPlacas == 1 && x.IdTipoPlaca == w.FirstOrDefault().IdTipoPlaca).Sum(s => s.Existencia),
                                                CantidadDisponiblesSerTransferidas = 0
                                            }).OrderBy(p => p.IdTipoPlaca).ToList();                

                foreach (var item in listPlacas)
                {
                    placasResponse.Data.TransferenciaPlacas_Listado2.Add(new TransferenciaPlacas_Listado2()
                    {
                        Automatico = false,
                        IdTipoPlaca = item.IdTipoPlaca,
                        TiposPlacas = new TiposPlacas()
                        {
                            IdTipoPlaca = item.IdTipoPlaca,
                            TipoPlaca = item.TiposPlacas.TipoPlaca
                        },
                        IdTipoEstatusPlacas = item.IdEstatusPlacas,
                        TiposEstatusPlacas = new TiposEstatusPlacas()
                        {
                            IdTipoEstatusPlacas = item.IdEstatusPlacas,
                            TipoEstatusPlacas = item.EstatusPlacas.TipoEstatusPlacas
                        },
                        IdTransferencia = 0,
                        IdTransferenciaListado2 = 0,
                        NumeroPlaca = item.NumeroPlaca,
                        TransferirPlaca = false
                    });
                }
                placasResponse.ExecutionOK = true;

            }
            else
            {
                placasResponse.ExecutionOK = false;
                placasResponse.Message = "No se encontro la información de la delegación origen";
            }

            return Json(placasResponse);
        }

        [HttpPost]
        public ActionResult GetTransferenciaPlacas_Listado1(List<Listado_TransferenciaPlacas_Listado1_Model> TransferenciaPlacas_Listado1)
        {
            return PartialView("PartialTransferenciaListado1", TransferenciaPlacas_Listado1);
        }

        [HttpPost]
        public ActionResult GetTransferenciaPlacas_Listado2(List<Listado_TransferenciaPlacas_Listado2_Model> TransferenciaPlacas_Listado2)
        {
            return PartialView("PartialTransferenciaListado2", TransferenciaPlacas_Listado2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name = "id" ></ param >
        /// < returns ></ returns >
        public ActionResult PackingList(int IdTransferencia)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.GenerarPackList)
            {
                return RedirectToAction("Index", "Login");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_TransferenciaPlacasVM TransferenciaPlacasVM = new Detalle_TransferenciaPlacasVM();
            TransferenciaPlacasVM.TransferenciaPlacas_DatosPersonaEnvio = new Detalle_TransferenciaPlacas_DatosPersonaEnvioVM();
            TransferenciaPlacasVM.TransferenciaPlacas_TransporteEnvio = new Detalle_TransferenciaPlacas_TransporteEnvioVM();
            TransferenciaPlacasVM.TransferenciaPlacas_Listado1 = new List<Listado_TransferenciaPlacas_Listado1_Model>();
            TransferenciaPlacasVM.TransferenciaPlacas_Listado2 = new List<Listado_TransferenciaPlacas_Listado2_Model>();

            TransferenciaPlacasVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesUsuarioDDL = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            var objResponse = new TransferenciasPlacas_BL().GetTransferenciaPlaca_PackingList(IdTransferencia, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                TransferenciaPlacasVM += objResponse.Data;

            }
            return View(TransferenciaPlacasVM);
        }

        public ActionResult TransferirPlacas(int IdTransferencia)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");

            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.TransferirPlacasAcceso)
            {
                return RedirectToAction("Index", "Login");
            }

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            Detalle_TransferenciaPlacasVM TransferenciaPlacasVM = new Detalle_TransferenciaPlacasVM();
            TransferenciaPlacasVM.TransferenciaPlacas_DatosPersonaEnvio = new Detalle_TransferenciaPlacas_DatosPersonaEnvioVM();
            TransferenciaPlacasVM.TransferenciaPlacas_TransporteEnvio = new Detalle_TransferenciaPlacas_TransporteEnvioVM();
            TransferenciaPlacasVM.TransferenciaPlacas_Listado1 = new List<Listado_TransferenciaPlacas_Listado1_Model>();
            TransferenciaPlacasVM.TransferenciaPlacas_Listado2 = new List<Listado_TransferenciaPlacas_Listado2_Model>();

            TransferenciaPlacasVM.ListadoTiposIDsDDL = new Controles_DDL().GetTiposIDS_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesUsuarioDDL = new Controles_DDL().GetDelegacionesBancosPermitidoUsuario_DDL(usuarioLogin.ListadoDelegacionesBancos, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoDelegacionesDDL = new Controles_DDL().GetDelegacionesBancos_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusTransferenciaDDL = new Controles_DDL().GetTiposEstatusTransferencias_DDL(usuarioLogin.Entidad, "Seleccione").Data;
            TransferenciaPlacasVM.ListadoEstatusPlacasDDL = new Controles_DDL().GetTiposEstatusPlacas_DDL(usuarioLogin.Entidad, "Seleccione").Data;

            var objResponse = new TransferenciasPlacas_BL().GetTransferenciaPlaca_PackingList(IdTransferencia, usuarioLogin.Entidad);
            if (objResponse.ExecutionOK)
            {
                TransferenciaPlacasVM += objResponse.Data;

            }
            return View(TransferenciaPlacasVM);
        }

        [HttpPost]
        public ContentResult TransferenciaPlacasGeneraPDF(int IdTransferenciaPlacas)
        {
            string base64 = "";
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var getTransferencia = new TransferenciasPlacas_BL().GetTransferenciaPlaca_PackingList(IdTransferenciaPlacas, usuarioLogin.Entidad);
            if (getTransferencia.ExecutionOK)
            {
                var objTransferencia = getTransferencia.Data;
                var pathPlantillas = InfoRutasArchivos.GetInfoFilePath().DirectorioArchivos;

                var responseGeneraArchivo = Helper.GeneracionEnviosPDF.TransferenciaPlacasPDF(objTransferencia, pathPlantillas);
                if (!responseGeneraArchivo.ExecutionOK)
                    TempData["MensajeAIndex"] = responseGeneraArchivo.Message;

                TempData["MensajeAIndex"] = "";

                base64 = Convert.ToBase64String(responseGeneraArchivo.Data, 0, responseGeneraArchivo.Data.Length);
                return Content(base64);
            }

            return Content(base64);

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
        public ContentResult TransferenciaPlacasPDF(int IdTransferencia)
        {
            string base64 = "";
            var usuarioLogin = (Usuarios)Session["UserSC"];

            var getTransferencia = new TransferenciasPlacas_BL().GetTransferenciaPlaca_Details(IdTransferencia, usuarioLogin.Entidad);
            if (getTransferencia.ExecutionOK)
            {
                var objTransferencia = getTransferencia.Data;
                var pathPlantillas = InfoRutasArchivos.GetInfoFilePath().DirectorioArchivos;

                var responseGeneraArchivo = Helper.GeneracionEnviosPDF.TransferenciaPlacasPDF(objTransferencia, pathPlantillas);
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
        public ActionResult CancelarTransferencia(int? id)
        {
            if (Session["UserSC"] == null)
                return RedirectToAction("Index", "Login");
            var usuarioLogin = (Usuarios)Session["UserSC"];
            if (!usuarioLogin.UsuariosPermisos.Pantallas_TransferenciasPlacasEntreDelegacionesBancos.Cancelar)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null || id.Value == 0)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Declaración de varibles
            TempData["messages"] = new Dictionary<string, string[]>();
            DBResponse<DBNull> response = new TransferenciasPlacas_BL().CambioEstatusTransferencia(id.Value, 2, usuarioLogin);
            if (response.ExecutionOK)
                TempData["MensajeAIndex"] = "Transferencia de Placas cancelada correctamente";
            else
                TempData["MensajeAIndex"] = response.Message;


            return RedirectToAction("Index");
        }
    }
}
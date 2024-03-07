using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM
    {
        public int IdRecepcion { get; set; }
        public int IdEventoRecepcion { get; set; }
        public int IdValidacionEvento { get; set; }
        public string Horas { get; set; }
        public string Fecha { get; set; }
        public UsuariosVM Usuario { get; set; } = new UsuariosVM();
        public int IdDelegacionesBancos { get; set; }
        public Detalle_DelegacionesBancosVM DelegacionesBancos { get; set; } = new Detalle_DelegacionesBancosVM();
        public string NotaEntrada { get; set; }
        public int IdProveedor { get; set; }
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();
        public int IdContrato { get; set; }
        public Detalle_ContratosVM Contrato { get; set; } = new Detalle_ContratosVM();
        public string PartidaContrato { get; set; }
        public int IdTipoProblema { get; set; }
        public TiposEventosRecepcionPlacasVM TiposProblemasPresentadosValidacion { get; set; } = new TiposEventosRecepcionPlacasVM();
        public string CajaNdeM { get; set; }
        public int IdTipoPlaca { get; set; }
        public Detalle_TiposPlacasVM TipoPlaca { get; set; } = new Detalle_TiposPlacasVM(); 
        public string Archivos { get; set; }
        public string Observaciones { get; set; }

        public List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel> ListadoArchivos { get; set; } = new List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel>();
        public List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel> ListadoObservaciones { get; set; } = new List<Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel>();

        public static Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM operator +(Detalle_RecepcionSolicitudesPlacas_Recibir_ValidacionesVM validacionesModel, RecepcionSolicitudesPlacas_Recibir_Validaciones recepcionSolicitudesPlacas)
        {
            validacionesModel.IdRecepcion = recepcionSolicitudesPlacas.IdRecepcion;
            validacionesModel.IdEventoRecepcion = recepcionSolicitudesPlacas.IdEventoRecepcion;
            validacionesModel.Horas = recepcionSolicitudesPlacas.Horas;
            validacionesModel.Fecha = recepcionSolicitudesPlacas.Fecha;
            validacionesModel.Usuario += recepcionSolicitudesPlacas.Usuario;
            validacionesModel.IdDelegacionesBancos = recepcionSolicitudesPlacas.IdDelegacionesBancos;
            validacionesModel.DelegacionesBancos += recepcionSolicitudesPlacas.DelegacionesBancos;
            validacionesModel.NotaEntrada = recepcionSolicitudesPlacas.NotaEntrada;
            validacionesModel.IdProveedor = recepcionSolicitudesPlacas.IdProveedor;
            validacionesModel.Proveedores += recepcionSolicitudesPlacas.Proveedores;
            validacionesModel.IdContrato = recepcionSolicitudesPlacas.IdContrato;
            validacionesModel.Contrato += recepcionSolicitudesPlacas.Contrato;
            validacionesModel.PartidaContrato = recepcionSolicitudesPlacas.PartidaContrato;
            validacionesModel.IdTipoProblema = recepcionSolicitudesPlacas.IdTipoProblema;
            validacionesModel.TiposProblemasPresentadosValidacion += recepcionSolicitudesPlacas.TiposEventosRecepcionPlacas;
            validacionesModel.CajaNdeM = recepcionSolicitudesPlacas.CajaNdeM;
            validacionesModel.IdTipoPlaca = recepcionSolicitudesPlacas.IdTipoPlaca;
            validacionesModel.TipoPlaca += recepcionSolicitudesPlacas.TipoPlaca; 

            foreach (var item in recepcionSolicitudesPlacas.Observaciones)
            {
                validacionesModel.ListadoObservaciones.Add(new Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesObservacionesModel() + item);
            }
            foreach (var item in recepcionSolicitudesPlacas.Archivos)
            {
                validacionesModel.ListadoArchivos.Add(new Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesArchivosModel() + item);
            }
            return validacionesModel;
        }
    }
}
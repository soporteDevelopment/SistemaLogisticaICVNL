using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.ViewModels;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesModel
    {
        public int IdRecepcionValidaciones { get; set; }
        public int IdArchivo { get; set; }
        public int IdRecepcion { get; set; }
        public int IdEventoRecepcion { get; set; }
        public UsuariosVM Usuario { get; set; } = new UsuariosVM();
        public Detalle_ProveedoresVM Proveedores { get; set; } = new Detalle_ProveedoresVM();
        public Detalle_ContratosVM Contrato { get; set; } = new Detalle_ContratosVM();
        public TiposEventosRecepcionPlacasVM TiposProblemasPresentadosValidacion { get; set; } = new TiposEventosRecepcionPlacasVM();
        public Detalle_TiposPlacasVM TipoPlaca { get; set; } = new Detalle_TiposPlacasVM(); 
        public static Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesModel operator +(Listado_RecepcionSolicitudesPlacas_Recibir_ValidacionesModel validacionesModel, RecepcionSolicitudesPlacas_Recibir_Validaciones recepcionSolicitudesPlacas)
        {
            validacionesModel.IdRecepcionValidaciones = recepcionSolicitudesPlacas.IdRecepcionValidaciones;
            validacionesModel.IdArchivo = recepcionSolicitudesPlacas.IdArchivo;
            validacionesModel.IdRecepcion = recepcionSolicitudesPlacas.IdRecepcion;
            validacionesModel.IdEventoRecepcion = recepcionSolicitudesPlacas.IdEventoRecepcion;
            validacionesModel.Usuario += recepcionSolicitudesPlacas.Usuario;
            validacionesModel.Proveedores += recepcionSolicitudesPlacas.Proveedores;
            validacionesModel.Contrato += recepcionSolicitudesPlacas.Contrato;
            validacionesModel.TiposProblemasPresentadosValidacion += recepcionSolicitudesPlacas.TiposEventosRecepcionPlacas;
            validacionesModel.TipoPlaca += recepcionSolicitudesPlacas.TipoPlaca; 
            return validacionesModel;
        }
    }
}
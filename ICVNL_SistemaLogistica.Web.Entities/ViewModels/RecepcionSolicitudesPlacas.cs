using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class RecepcionSolicitudesPlacas
    {
        public int IdRecepcion { get; set; }
        public int IdSolicitud { get; set; }
        public string FolioRecepcion { get; set; }
        public DateTime Fecha { get; set; }
        public SolicitudesPlacas SolicitudesPlacas { get; set; } = new SolicitudesPlacas();
        public int IdDelegacionBanco { get; set; }
        public DelegacionesBancos DelegacionesBancos { get; set; } = new DelegacionesBancos();
        public string NotaEntradaAutorizada { get; set; }
        public Boolean Recibida { get; set; }
        public string UsuarioRegistro { get; set; }
        public int Estatus { get; set; }
        public int Entidad { get; set; }
        public List<RecepcionSolicitudesPlacas_Detalle> RecepcionSolicitudesPlacas_Detalle { get; set; } = new List<RecepcionSolicitudesPlacas_Detalle>();
        public RecepcionSolicitudesPlacas(int IdRecepcion_,
                                          string FolioRecepcion_,
                                          int IdSolicitud_,
                                          SolicitudesPlacas SolicitudesPlacas_,
                                          int IdDelegacionBanco_,
                                          DelegacionesBancos DelegacionesBancos_,
                                          string NotaEntradaAutorizada_,
                                          Boolean Recibida_,
                                          List<RecepcionSolicitudesPlacas_Detalle> RecepcionSolicitudesPlacas_Detalle_)
        {
            this.IdRecepcion = IdRecepcion_;
            this.FolioRecepcion = FolioRecepcion_;
            this.IdSolicitud = IdSolicitud_;
            this.SolicitudesPlacas = SolicitudesPlacas_;
            this.IdDelegacionBanco = IdDelegacionBanco_;
            this.DelegacionesBancos = DelegacionesBancos_;
            this.NotaEntradaAutorizada = NotaEntradaAutorizada_;
            this.Recibida = Recibida_;
            this.RecepcionSolicitudesPlacas_Detalle = RecepcionSolicitudesPlacas_Detalle_;
        }
        public RecepcionSolicitudesPlacas()
        {

        }
    }
}

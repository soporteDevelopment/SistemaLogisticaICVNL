using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class SolicitudesPlacas
    {
        public int IdSolicitud { get; set; }
        public string FolioSolicitud { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaEntrega { get; set; }
        public int IdProveedor { get; set; }
        public Proveedores Proveedores { get; set; }
        public int IdContrato { get; set; }
        public Contratos Contratos { get; set; }
        public int IdOrdenCompra { get; set; }
        public OrdenesCompra OrdenesCompra { get; set; }
        public string UsuarioRegistra { get; set; }
        public int Entidad { get; set; }
        public List<SolicitudesPlacas_Detalle> SolicitudesPlacas_Detalle { get; set; } = new List<SolicitudesPlacas_Detalle>();

        public SolicitudesPlacas(int IdSolicitud_,
                                string FolioSolicitud_,
                                DateTime FechaSolicitud_,
                                DateTime FechaEntrega_,
                                int IdProveedor_,
                                Proveedores Proveedores_,
                                int IdContrato_,
                                Contratos Contratos_,
                                int IdOrdenCompra_,
                                List<SolicitudesPlacas_Detalle> SolicitudesPlacas_Detalle_)
        {
            this.IdSolicitud = IdSolicitud_;
            this.FolioSolicitud = FolioSolicitud_;
            this.FechaSolicitud = FechaSolicitud_;
            this.FechaEntrega = FechaEntrega_;
            this.IdProveedor = IdProveedor_;
            this.Proveedores = Proveedores_;
            this.IdContrato = IdContrato_;
            this.Contratos = Contratos_;
            this.IdOrdenCompra = IdOrdenCompra_;
            this.SolicitudesPlacas_Detalle = SolicitudesPlacas_Detalle_;
        }
        public SolicitudesPlacas()
        {

        }
    }
}

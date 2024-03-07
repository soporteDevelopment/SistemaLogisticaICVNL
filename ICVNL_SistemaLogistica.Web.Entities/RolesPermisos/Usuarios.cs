using ICVNL_SistemaLogistica.Web.Entities.RolesPermisos;
using ICVNL_SistemaLogistica.Web.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class Usuarios
    {
        public int ClaveUsuario { get; set; }
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string NumeroEmpleado { get; set; }
        public string IP_Usuario { get; set; }
        public int Entidad { get; set; }
        public Token Token { get; set; }
        public Token TokenInventario { get; set; }
        public UsuariosPermisos UsuariosPermisos { get; set; }
        public List<DelegacionesBancos> ListadoDelegacionesBancos { get; set; }

    }
     

    public class UsuariosPermisos
    {
        public Pantallas_Principal Pantallas_Principal { get; set; } = new Pantallas_Principal();
        public Pantallas_Parametrizacion Pantallas_Parametrizacion { get; set; } = new Pantallas_Parametrizacion();
        public Pantallas_DelegacionesBancos Pantallas_DelegacionesBancos { get; set; } = new Pantallas_DelegacionesBancos();
        public Pantallas_TiposPlacas Pantallas_TiposPlacas { get; set; } = new Pantallas_TiposPlacas();
        public Pantallas_TiposIDS Pantallas_TiposIDS { get; set; } = new Pantallas_TiposIDS();
        public Pantallas_Estados Pantallas_Estados { get; set; } = new Pantallas_Estados();
        public Pantallas_Proveedores Pantallas_Proveedores { get; set; } = new Pantallas_Proveedores();
        public Pantallas_Contratos Pantallas_Contratos { get; set; } = new Pantallas_Contratos();
        public Pantallas_SolicitudesPlacasProveedor Pantallas_SolicitudesPlacasProveedor { get; set; } = new Pantallas_SolicitudesPlacasProveedor();
        public Pantallas_RecepcionSolicitudesPlacasProveedor Pantallas_RecepcionSolicitudesPlacasProveedor { get; set; } = new Pantallas_RecepcionSolicitudesPlacasProveedor();                
        public Pantallas_TransferenciasPlacasEntreDelegacionesBancos Pantallas_TransferenciasPlacasEntreDelegacionesBancos { get; set; } = new Pantallas_TransferenciasPlacasEntreDelegacionesBancos();
        public Pantallas_RecepcionPlacasEntreDelegacionesBancos Pantallas_RecepcionPlacasEntreDelegacionesBancos { get; set; } = new Pantallas_RecepcionPlacasEntreDelegacionesBancos();
        public Pantallas_ConsultaInformacionPlacas Pantallas_ConsultaInformacionPlacas { get; set; } = new Pantallas_ConsultaInformacionPlacas(); 
        public Pantallas_InventarioPlacas Pantallas_InventarioPlacas { get; set; } = new Pantallas_InventarioPlacas();
        public Pantallas_ConsultaBitacora Pantallas_ConsultaBitacora { get; set; } = new Pantallas_ConsultaBitacora();
        public Pantallas_Reportes Pantallas_Reportes1 { get; set; } = new Pantallas_Reportes();
        public Pantallas_Reportes Pantallas_Reportes2 { get; set; } = new Pantallas_Reportes();
        public Pantallas_Reportes Pantallas_Reportes3 { get; set; } = new Pantallas_Reportes();
        public Pantallas_Reportes Pantallas_Reportes4 { get; set; } = new Pantallas_Reportes();
        public Pantallas_Reportes Pantallas_Reportes5 { get; set; } = new Pantallas_Reportes();
    }
}

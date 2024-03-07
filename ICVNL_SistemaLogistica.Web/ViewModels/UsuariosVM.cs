using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class UsuariosVM
    {
        public string Nombre { get; set; }
        public string Puesto { get; set; }
        public string NumeroEmpleado { get; set; }

        public static UsuariosVM operator +(UsuariosVM usuario, Usuarios users)
        {
            usuario.Nombre = users.Nombre;
            usuario.Puesto = users.Puesto;
            usuario.NumeroEmpleado = users.NumeroEmpleado;
            return usuario;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{ 
    public class Cuenta
    {
        public double empresa { get; set; }
        public string cuenta_con_formato { get; set; }
        public string cuenta_sin_formato { get; set; }
        public string nombre { get; set; }
        public int naturaleza { get; set; }
        public int nivel { get; set; }
        public int dependientes { get; set; }
        public int tipo { get; set; }
        public int maneja_centro_costos { get; set; }
        public int diario_cuenta { get; set; }
        public object bloqueada { get; set; }
        public int tipos_cuentas { get; set; }
        public int niveles { get; set; }
        public int startwith { get; set; }
        public object startwith_id { get; set; }
        public int moneda { get; set; }
        public object ipAddress { get; set; }
        public int idIdioma { get; set; }
        public object idUsuario { get; set; }
    }

    public class ResponseCuenta
    {
        public Cuenta cuenta { get; set; }
        public int acknowledge { get; set; }
        public object correlationId { get; set; }
        public object messageId { get; set; }
        public object messageParameters { get; set; }
        public string messageText { get; set; }
        public double idEvento { get; set; }
        public int totalRows { get; set; }
        public int language { get; set; }
        public int userId { get; set; }
        public int companyId { get; set; }
        public int idalmacen { get; set; }
        public object idsucursal { get; set; }
        public object idProveedor { get; set; }
    }


}

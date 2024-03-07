using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class Datos
    {
        /// <summary>
        /// Recupera el valor de un campo proveniente de la base de datos
        /// </summary>
        /// <param name="dr">DataRow con los datos.</param>
        /// <param name="campo">Nombre dEl Campo o columna dentro del DataRow de donde se obtendrá la información.</param>
        /// <returns>El valor dEl Campo solicitado, como tipo de dato cadena.</returns>
        public static string Str(DataRow dr, string campo)
        {
            string resultado = string.Empty;
            if (dr[campo] != DBNull.Value)
            {
                resultado = dr[campo].ToString().Trim();
            }
            return resultado;
        }

        /// <summary>
        /// Recupera el valor de un campo proveniente de la base de datos
        /// </summary>
        /// <param name="dr">DataRow con los datos.</param>
        /// <param name="campo">Nombre dEl Campo o columna dentro del DataRow de donde se obtendrá la información.</param>
        /// <returns>El valor dEl Campo solicitado, como tipo de dato entero.</returns>
        public static int Int(DataRow dr, string campo)
        {
            string campoString = Str(dr, campo);
            int resultado = -1;
            Int32.TryParse(campoString, out resultado);
            return resultado;
        }

        /// <summary>
        /// Recupera el valor de un campo proveniente de la base de datos
        /// </summary>
        /// <param name="dr">DataRow con los datos.</param>
        /// <param name="campo">Nombre dEl Campo o columna dentro del DataRow de donde se obtendrá la información.</param>
        /// <returns>El valor dEl Campo solicitado, como tipo de dato boleano.</returns>
        public static bool Bool(DataRow dr, string campo)
        {
            string campoString = Str(dr, campo);
            bool resultado = false;
            if (campoString.ToLower() == "true")
            {
                resultado = true;
            }
            return resultado;
        }

        /// <summary>
        /// Recupera el valor de un campo proveniente de la base de datos
        /// </summary>
        /// <param name="dr">DataRow con los datos.</param>
        /// <param name="campo">Nombre dEl Campo o columna dentro del DataRow de donde se obtendrá la información.</param>
        /// <returns>El valor dEl Campo solicitado, como tipo de dato doble.</returns>
        public static double Dou(DataRow dr, string campo)
        {
            string campoString = Str(dr, campo);
            double resultado = -999.999;
            double.TryParse(campoString, out resultado);
            return resultado;
        }

        public static decimal Dec(DataRow dr, string campo)
        {
            string campoString = Str(dr, campo);
            decimal resultado = new decimal(-999.999);
            decimal.TryParse(campoString, out resultado);
            return resultado;
        }
        public static Byte[] ArrBytes(DataRow dr, string campo)
        {
            Byte[] resultado = null;
            if (dr[campo] != DBNull.Value)
            {
                resultado = (Byte[])dr[campo];
            }
            return resultado;
        }
    }
}

using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class Archivos_BL
    {
        public DBResponse<List<Archivos>> GetArchivos_List(int IdOrigen, string TablaOrigen, int Entidad)
        {
            return new Archivos_DA().GetArchivos_List(IdOrigen, TablaOrigen, Entidad);
        }

        public DBResponse<Archivos> GetArchivo(int IdArchivo, int Entidad)
        {
            return new Archivos_DA().GetArchivo(IdArchivo, Entidad);
        }
        public DBResponse<DBNull> EliminaArchivo(int IdArchivo)
        {
            return new Archivos_DA().EliminaArchivo(IdArchivo);
        }
        public DBResponse<DBNull> InsertaArchivo(Archivos archivo)
        {
            return new Archivos_DA().InsertaArchivo(archivo);
        }
    }
}

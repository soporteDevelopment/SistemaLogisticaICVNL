using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess
{
    public class IListArchivos
    {
        public IList<Parameter> ParametersAgregaArchivos(Archivos _archivos)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_ARN_IDORIGEN", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _archivos.IdOrigen),
                Db.CreateParameter("p_ARC_TABLAORIGEN", DbType.String, 30, ParameterDirection.Input, false, null, DataRowVersion.Default, _archivos.TablaOrigen),
                Db.CreateParameter("p_ARN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_ARC_ARCHIVO", DbType.String, 250, ParameterDirection.Input, false, null, DataRowVersion.Default, _archivos.NombreArchivo),
                Db.CreateParameter("p_ARN_ENTIDAD", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, _archivos.Entidad),
                Db.CreateParameter("p_ARN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_ARB_ARCHIVO", DbType.Object, 2147483647, ParameterDirection.Input, false, null, DataRowVersion.Default, _archivos.Archivo, 0,0, ObjectType.Blob)
            };
        }
    }
}

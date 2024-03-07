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
    public class IListSolicitudesPlacas
    {
        public IList<Parameter> ParametersAgregaSolicitudPlacasEncabezado(SolicitudesPlacas solicitudesPlacas)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_OCN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.IdOrdenCompra),
                Db.CreateParameter("p_SOLN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.Entidad),
                Db.CreateParameter("p_SOLF_FECHA", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.FechaSolicitud),
                Db.CreateParameter("p_CONTN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.IdContrato),
                Db.CreateParameter("p_SOLN_ESTATUS", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                Db.CreateParameter("p_SOLC_USR_REG", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.UsuarioRegistra),
                Db.CreateParameter("p_SOLN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.IdProveedor),
                Db.CreateParameter("p_SOLC_FOLIO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.FolioSolicitud),
                Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, solicitudesPlacas.IdSolicitud),
                Db.CreateParameter("p_SOLF_FECHAENTREGA", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.FechaEntrega),
                Db.CreateParameter("p_SOLF_FECHA_REG", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, solicitudesPlacas.FechaRegistro)
            };
        }

        public IList<Parameter> ParametersAgregaSolicitudPlacasDetalle(SolicitudesPlacas_Detalle _Detalle)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_SOLDC_RNG_PL_INI", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.RangoPlacaInicial),
                Db.CreateParameter("p_SOLDN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, _Detalle.IdSolicitudDetalle),
                Db.CreateParameter("p_SOLDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.Entidad),
                Db.CreateParameter("p_SOLDC_RNG_PL_FIN", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.RangoPlacaFinal),
                Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdDelegacionBanco),
                Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdSolicitud),
                Db.CreateParameter("p_TPN_ID", DbType.String, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdTipoPlaca),
                Db.CreateParameter("p_SOLDN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_SOLDN_CANT_PLACAS", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.CantidadPlacas), 
                Db.CreateParameter("p_OCDN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdDetalleOrdenCompra) 
            };
        }
    }
}

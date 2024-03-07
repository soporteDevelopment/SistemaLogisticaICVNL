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
    public class IListRecepcionSolicitudesPlacas
    {
        public IList<Parameter> ParametersAgregaRecepcioSolicitudPlacasEncabezado(RecepcionSolicitudesPlacas recepcionSolicitudesPlacas)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_RECC_NE_AUTORIZADA", DbType.String, 75, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.NotaEntradaAutorizada),
                Db.CreateParameter("p_RECC_FOLIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.FolioRecepcion),
                Db.CreateParameter("p_RECF_FECHA_ENT", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Fecha),
                Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.IdDelegacionBanco),
                Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.IdSolicitud),
                Db.CreateParameter("p_RECN_RECIBIDO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Recibida ? 1 : 0),
                Db.CreateParameter("p_RECC_USR_REG", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.UsuarioRegistro),
                Db.CreateParameter("p_RECN_ESTATUS", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Estatus),
                Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.IdRecepcion),
                Db.CreateParameter("p_RECN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Entidad),
                Db.CreateParameter("p_RECN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0)
            };
        }

        public IList<Parameter> ParametersAgregaRecepcioSolicitudPlacasEncabezadoMod(RecepcionSolicitudesPlacas recepcionSolicitudesPlacas)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_RECC_NE_AUTORIZADA", DbType.String, 75, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.NotaEntradaAutorizada),
                Db.CreateParameter("p_RECC_FOLIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.FolioRecepcion),
                Db.CreateParameter("p_RECF_FECHA_ENT", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Fecha),
                Db.CreateParameter("p_DBN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.IdDelegacionBanco),
                Db.CreateParameter("p_SOLN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.IdSolicitud),
                Db.CreateParameter("p_RECN_RECIBIDO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Recibida ? 1 : 0),
                Db.CreateParameter("p_RECC_USR_REG", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.UsuarioRegistro),
                Db.CreateParameter("p_RECN_ESTATUS", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Estatus),
                Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.IdRecepcion),
                Db.CreateParameter("p_RECN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, recepcionSolicitudesPlacas.Entidad),
                Db.CreateParameter("p_RECN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0)
            };
        }

        public IList<Parameter> ParametersAgregaRecepcioSolicitudPlacasDetalle(RecepcionSolicitudesPlacas_Detalle _Detalle)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_RECDN_CANT_REC", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.CantidadRecibida),
                Db.CreateParameter("p_RECDN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, _Detalle.IdRecepcionDetalle),
                Db.CreateParameter("p_RECDN_CANT_NE_AUT", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.CantidadNotasEntradaAutorizada),
                Db.CreateParameter("p_RECDN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdTipoPlaca),
                Db.CreateParameter("p_RECDN_CANT_SOL_OC", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.CantidadSolicitadaOrdenCompra),
                Db.CreateParameter("p_RECDN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.Entidad),
                Db.CreateParameter("p_RECN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdRecepcion)
            };
        }
    }
}

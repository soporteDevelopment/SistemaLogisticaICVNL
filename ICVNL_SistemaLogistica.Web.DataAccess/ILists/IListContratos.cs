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
    public class IListContratos
    {
        public IList<Parameter> ParametersAgregaContratosEncabezado(Contratos contrato_)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_CONN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_CONN_ENTIDAD", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, contrato_.Entidad),
                Db.CreateParameter("p_CONC_NUMERO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, contrato_.NumeroContrato),
                Db.CreateParameter("p_CONC_USR_REG", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, contrato_.Usuario),
                Db.CreateParameter("p_CONF_REGISTRO", DbType.Date, 18, ParameterDirection.Input, false, null, DataRowVersion.Default, contrato_.FechaContrato),
                Db.CreateParameter("p_CONN_ESTATUS", DbType.String, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1),
                Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, contrato_.IdContrato),
                Db.CreateParameter("p_CONN_ACTIVO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 1)
            };
        }

        public IList<Parameter> ParametersAgregaContratosDetalle(Contratos_Detalle _Detalle)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_CONDC_RANGOFINAL", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.RangoFinal),
                Db.CreateParameter("p_CONDN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_CONDN_CantidadPlacas", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.CantidadPlacas),
                Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdTipoPlaca),
                Db.CreateParameter("p_CONDC_MASCARAPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.MascaraPlaca),
                Db.CreateParameter("p_CONN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdContrato),
                Db.CreateParameter("p_CONDC_ORDENPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.OrdenPlaca),
                Db.CreateParameter("p_CONDN_CANTIDADPLACASCAJA", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.CantidadPlacasCaja),
                Db.CreateParameter("p_CONDN_ENTIDAD", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.Entidad),
                Db.CreateParameter("p_CONDC_OFICIOSICT", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.OficioSICT),
                Db.CreateParameter("p_CONDN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, _Detalle.IdContratoDetalle),
                Db.CreateParameter("p_CONDC_RANGOINICIAL", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.RangoInicial),
                Db.CreateParameter("p_PROVN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdProveedor)
            };
        }

        public IList<Parameter> ParametersAgregaContratosDetalleRangos(Contratos_Detalles_Rangos _DetalleRangos)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_CDRN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, _DetalleRangos.Entidad),
                Db.CreateParameter("p_CONDN_ID", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, _DetalleRangos.IdContratoDetalle),
                Db.CreateParameter("p_CDRC_RANGOINICIAL", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, _DetalleRangos.RangoInicial),
                Db.CreateParameter("p_CDRN_CANTIDADSERIE", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _DetalleRangos.CantidadSerie),
                Db.CreateParameter("p_CDRN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_CDRN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, _DetalleRangos.IdContratoDetalleRangos),
                Db.CreateParameter("p_CDRC_RANGOFINAL", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, _DetalleRangos.RangoFinal)              
            };
        }

    }
}

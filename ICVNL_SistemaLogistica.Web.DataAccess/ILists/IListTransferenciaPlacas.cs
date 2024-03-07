using Framework.Net.Data;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.DataAccess.ILists
{
    public class IListTransferenciaPlacas
    {
        public IList<Parameter> ParametersEncabezadoTP(TransferenciaPlacas transferenciaPlacas)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, transferenciaPlacas.IdTransferencia),
                Db.CreateParameter("p_TRNC_USR_REG", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.UsuarioRegistro),
                Db.CreateParameter("p_TRNN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_TRNN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.Entidad),
                Db.CreateParameter("p_ETN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.IdEstatusTransferencia),
                Db.CreateParameter("p_TRNN_FOLIO", DbType.String, 20, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.FolioTransferencia),
                Db.CreateParameter("p_DBN_ID_ORIGEN", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.IdDelegacionBancoOrigen),
                Db.CreateParameter("p_DBN_ID_DESTINO", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.IdDelegacionBancoDestino),
                Db.CreateParameter("p_TRNF_FECHA", DbType.Date, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, transferenciaPlacas.FechaHoraRegistro)
            };
        }

        public IList<Parameter> ParametersDatosPersonaTP(TransferenciaPlacas_DatosPersona datosPersona)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_TDPC_NOMBRE", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.Nombre),
                Db.CreateParameter("p_TIN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.IdTipoIDs),
                Db.CreateParameter("p_TDPN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.IdTransferencia),
                Db.CreateParameter("p_TDPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.Entidad),
                Db.CreateParameter("p_TDPN_NUMEROID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.NumeroID),
                Db.CreateParameter("p_TDPC_APELLIDO", DbType.String, 200, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.Apellido),
                Db.CreateParameter("p_TDPC_TIPO", DbType.String, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, datosPersona.Tipo),
                Db.CreateParameter("p_TDPN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, datosPersona.IdTransferenciaDatosPersona)
            };
        }

        public IList<Parameter> ParametersTransportreTP(TransferenciaPlacas_Transporte transporte)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_TTC_MARCAVEHICULO", DbType.String, 35, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.MarcaVehiculo),
                Db.CreateParameter("p_TTC_TIPO", DbType.String, 6, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.Tipo),
                Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.IdTransferencia),
                Db.CreateParameter("p_TTC_NUMEROECONOMICO", DbType.String, 30, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.NumeroEconomico),
                Db.CreateParameter("p_TTN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, transporte.IdTransferenciaTransporte),
                Db.CreateParameter("p_TTC_PLACASVEHICULO", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.PlacasVehiculo),
                Db.CreateParameter("p_TTC_MODELOVEHICULO", DbType.String, 50, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.ModeloVehiculo),
                Db.CreateParameter("p_TTN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_TTN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, transporte.Entidad)
            };
        }

        public IList<Parameter> ParametersDetalleTP(TransferenciaPlacas_Detalle _Detalle)
        {
            return new List<Parameter>
            {
                Db.CreateParameter("p_DPN_ID", DbType.Int32, 38, ParameterDirection.Output, false, null, DataRowVersion.Default, _Detalle.IdTransferenciaDet),
                Db.CreateParameter("p_DPN_BORRADO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, 0),
                Db.CreateParameter("p_DPF_FECHAESTATUSPLACA", DbType.DateTime, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.FechaEstatusPlaca),
                Db.CreateParameter("p_DPN_ENTIDAD", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.Entidad),
                Db.CreateParameter("p_TRNN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdTransferencia),
                Db.CreateParameter("p_DPN_AUTOMATICO", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.Automatico == true ? 1 : 0),
                Db.CreateParameter("p_DPN_TRANSFERENCIA_IND", DbType.Int32, 1, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.TransferirPlacaIndividual),
                Db.CreateParameter("p_DPC_USUARIORECIBIO", DbType.String, 100, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.UsuarioRecibio),
                Db.CreateParameter("p_DPC_NUMEROPLACA", DbType.String, 12, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.NumeroPlaca),
                Db.CreateParameter("p_TPN_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdTipoPlaca),
                Db.CreateParameter("p_TENP_ID", DbType.Int32, 38, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.IdTipoEstatusPlacas),
                Db.CreateParameter("p_DPN_TRANSFERIRPLACA", DbType.Int32, 5, ParameterDirection.Input, false, null, DataRowVersion.Default, _Detalle.TransferirPlaca == true ? 1 : 0)
            };
        }
    }
}

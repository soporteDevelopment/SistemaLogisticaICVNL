using ICVNL_SistemaLogistica.Web.DataAccess;
using ICVNL_SistemaLogistica.Web.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.BL
{
    public class Controles_DDL
    {
        public DBResponse<IEnumerable<dynamic>> GetDelegacionesBancos_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetDelegacionesBancos_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposPlacas_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetTiposPlacas_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposIDS_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetTiposIDS_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetEstados_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetEstados_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetProveedores_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetProveedores_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetContratos_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetContratos_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetContratosPorveedorSeleccionado_DDL(int IdProveedor, int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetContratosPorveedorSeleccionado_DDL(IdProveedor, Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetOrdenesCompraPorveedorSeleccionado_DDL(int IdProveedor, int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetOrdenesCompraPorveedorSeleccionado_DDL(IdProveedor, Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposPlacasOrdenCompraSeleccionada_DDL(int IdOrdenCompra, int IdContrato, int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetTiposPlacasOrdenCompraSeleccionada_DDL(IdOrdenCompra, IdContrato, Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetBitacoraEventos_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetBitacoraEventos_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetOrdenesCompra_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetOrdenesCompra_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetDelegaciones_RecepcionSolicitudes_DDL(int IdDelegacionBanco, int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetDelegaciones_RecepcionSolicitudes_DDL(IdDelegacionBanco, Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposEstatusTransferencias_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetTiposEstatusTransferencias_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposEstatusPlacas_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetTiposEstatusPlacas_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetTiposEventos_DDL(int Entidad, string TextoInicial)
        {
            return new Controles_DDL_DA().GetTiposEventos_DDL(Entidad, TextoInicial);
        }
        public DBResponse<IEnumerable<dynamic>> GetDelegacionesBancosPermitidoUsuario_DDL(List<DelegacionesBancos> delegacionesBancos, string TextoInicial)
        {
            var response = new DBResponse<IEnumerable<dynamic>>();
            var dynamicObj = new List<DynamicDDL>();
            try
            {
                dynamicObj.Add(new DynamicDDL() { Valor = "0", Texto = "Seleccione" });
                if (delegacionesBancos.Count > 0)
                {
                    foreach (var item in delegacionesBancos)
                    {
                        dynamicObj.Add(new DynamicDDL() { Valor = item.IdDelegacionBanco.ToString(), Texto = item.NombreDelegacionBanco });
                    }
                }

                response.Data = dynamicObj;
                response.ExecutionOK = true;
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }
    }
}

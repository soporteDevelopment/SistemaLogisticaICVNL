using ICVNL_SistemaLogistica.Web.Entities;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ContratosModel
    {
        public int IdContrato { get; set; }
        public string NumeroContrato { get; set; }

        public static Listado_ContratosModel operator +(Listado_ContratosModel detalle_ContratosVM, Contratos contratos)
        {
            detalle_ContratosVM.IdContrato = contratos.IdContrato;
            detalle_ContratosVM.NumeroContrato = contratos.NumeroContrato;

            return detalle_ContratosVM;
        }
    }
}
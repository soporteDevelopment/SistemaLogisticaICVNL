using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_DelegacionesBancosModel
    {
        [Display(Name = "Id de la Delegación/Banco")]
        public int Id { get; set; }

        [Display(Name = "Nombre de la Delegación/Banco")]
        public string NombreDelegacionBanco { get; set; }

        [Display(Name = "Centro de Costos en Infofin")]
        public string CentroCostosInfoFin { get; set; }

        [Display(Name = "Almacén del Centro de Costos")]
        public string AlmacenCentroCostos { get; set; }
        public int Estatus { get; set; }


        public static Listado_DelegacionesBancosModel operator +(Listado_DelegacionesBancosModel delegacionesBancosVM, DelegacionesBancos delegacionesBancos)
        {
            delegacionesBancosVM.Id = delegacionesBancos.IdDelegacionBanco;
            delegacionesBancosVM.NombreDelegacionBanco = delegacionesBancos.NombreDelegacionBanco;
            delegacionesBancosVM.CentroCostosInfoFin = delegacionesBancos.CentroCostosInfoFin;
            delegacionesBancosVM.AlmacenCentroCostos = delegacionesBancos.AlmacenCentroCostos;
            delegacionesBancosVM.Estatus = delegacionesBancos.Estatus;

            return delegacionesBancosVM;
        }
    }
}
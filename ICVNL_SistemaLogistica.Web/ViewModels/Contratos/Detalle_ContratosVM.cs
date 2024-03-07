using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ContratosVM
    {
        [Key]
        [Display(Name = "Id Contrato")]
        public int IdContrato { get; set; }

        [Display(Name = "Número de Contrato")]
        public string NumeroContrato { get; set; }

        [Display(Name = "Adjuntar Archivos")]
        public string Archivos { get; set; }
        public string NumeroFila { get; set; }
        public Detalle_ContratosDetailsVM DetalleContrato { get; set; } = new Detalle_ContratosDetailsVM();
        public List<Listado_ContratosDetailsModel> Detalle_ContratosDetailsVM { get; set; } = new List<Listado_ContratosDetailsModel>();
        public List<Listado_ContratosArchivosModel> Detalle_ContratosArchivosVM { get; set; } = new List<Listado_ContratosArchivosModel>();
        public List<Listado_ContratosDetailsRangosModel> Detalle_ContratosDetailsRangosVM { get; set; } = new List<Listado_ContratosDetailsRangosModel>();

        public static Detalle_ContratosVM operator +(Detalle_ContratosVM detalle_ContratosVM, Contratos contratos)
        {
            detalle_ContratosVM.IdContrato = contratos.IdContrato;
            detalle_ContratosVM.NumeroContrato = contratos.NumeroContrato;

            detalle_ContratosVM.Detalle_ContratosDetailsVM = new List<Listado_ContratosDetailsModel>();
            detalle_ContratosVM.Detalle_ContratosDetailsRangosVM = new List<Listado_ContratosDetailsRangosModel>();
            foreach (var item in contratos.Contratos_Detalle)
            {
                detalle_ContratosVM.Detalle_ContratosDetailsVM.Add(new Listado_ContratosDetailsModel() + item);
                foreach (var itemRangos in item.Contratos_Detalles_Rangos)
                {
                    detalle_ContratosVM.Detalle_ContratosDetailsRangosVM.Add(new Listado_ContratosDetailsRangosModel() + itemRangos);
                }
            }

            detalle_ContratosVM.Detalle_ContratosArchivosVM = new List<Listado_ContratosArchivosModel>();
            foreach (var item in contratos.Contratos_Archivos)
            {
                detalle_ContratosVM.Detalle_ContratosArchivosVM.Add(new Listado_ContratosArchivosModel() + item);
            }

            return detalle_ContratosVM;
        }

    }
}
using ICVNL_SistemaLogistica.Web.Entities;
using ICVNL_SistemaLogistica.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_RecepcionPlacas_IndividualesVM
    {
        public int IdTransferencia { get; set; }
        public List<Listado_RecepcionPlacas_IndividualesModel> Listado { get; set; } = new List<Listado_RecepcionPlacas_IndividualesModel>();

        public static Detalle_RecepcionPlacas_IndividualesVM operator +(Detalle_RecepcionPlacas_IndividualesVM detalle_, List<TransferenciaPlacas_RecibirPlacasIndividuales> transferenciaPlacas)
        {
            detalle_.IdTransferencia = transferenciaPlacas.FirstOrDefault().IdTransferencia;
            foreach (var item in transferenciaPlacas.Where(x=>x.IdTransferenciaIndividual != 1))
            {
                detalle_.Listado.Add(new Listado_RecepcionPlacas_IndividualesModel() + item);
            }

            return detalle_;
        }
    }
}
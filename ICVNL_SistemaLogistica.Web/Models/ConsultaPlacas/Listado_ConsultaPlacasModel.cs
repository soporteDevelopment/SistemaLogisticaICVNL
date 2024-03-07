using ICVNL_SistemaLogistica.Web.Entities;
using System;

namespace ICVNL_SistemaLogistica.Web.Models
{
    public class Listado_ConsultaPlacasModel
    {
        public string NumeroPlaca { get; set; }
        public DateTime FechaNotaEntrada { get; set; }
        public string NumeroNotaEntrada { get; set; }
        public DateTime FechaOrdenCompra { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public string DelegacionPlaca { get; set; }
        public int IdTipoEstatusPlaca { get; set; }
        public int PlacaContabilizada { get; set; }

        public static Listado_ConsultaPlacasModel operator +(Listado_ConsultaPlacasModel consultaPlacasModel, ConsultaInformacionPlacas informacionPlacas)
        {
            consultaPlacasModel.NumeroPlaca = informacionPlacas.NumeroPlaca;
            consultaPlacasModel.FechaNotaEntrada = informacionPlacas.FechaNotaEntrada;
            consultaPlacasModel.NumeroNotaEntrada = informacionPlacas.NumeroNotaEntrada;
            consultaPlacasModel.FechaOrdenCompra = informacionPlacas.FechaOrdenCompra;
            consultaPlacasModel.NumeroOrdenCompra = informacionPlacas.NumeroOrdenCompra;
            consultaPlacasModel.DelegacionPlaca = informacionPlacas.DelegacionPlaca;
            consultaPlacasModel.IdTipoEstatusPlaca = informacionPlacas.IdTipoEstatusPlaca;
            consultaPlacasModel.PlacaContabilizada = informacionPlacas.PlacaContabilizada;
            return consultaPlacasModel;
        }

    }
}
using ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities
{
    public class NotasEntradasPlacas
    {
        public int IdNotaEntrada { get; set; }
        public int? IdDelegacionBanco { get; set; }
        public int NumeroNotaEntrada { get; set; }
        public DateTime FechaNotaEntrada { get; set; }
        public int IdOrdenCompra { get; set; }
        public string NumeroOrdenCompra { get; set; }
        public OrdenesCompra OrdenCompra { get; set; } = new OrdenesCompra();
        public int CantidadPlacasNotaEntrada { get; set; }
        public int CantidadNumerosPlacaIdentificada { get; set; }
        public int CantidadNumerosPlacaPorIdentificarse { get; set; }
        public int IdEstatusNotaEntrada { get; set; }
        public string EstatusNotaEntrada { get; set; }
        public TiposEstatusNotaEntrada TiposEstatusNotaEntrada { get; set; } = new TiposEstatusNotaEntrada();
        public string UsuarioRegistro { get; set; }
        public int Entidad { get; set; }

        public List<NotasEntradasPlacas_Detalle> NotasEntradasPlacas_Detalle { get; set; }

        public static NotasEntradasPlacas operator +(NotasEntradasPlacas notasEntradasPlacas, NotasEntradas notasEntradasService)
        {
            notasEntradasPlacas = new NotasEntradasPlacas()
            {
                Entidad = 1,
                IdEstatusNotaEntrada = 2,
                NumeroNotaEntrada = notasEntradasService.entrada,
                FechaNotaEntrada = notasEntradasService.fecha_entrada,
                NumeroOrdenCompra = notasEntradasService.orden.ToString(),                
                NotasEntradasPlacas_Detalle = new List<NotasEntradasPlacas_Detalle>(),
                CantidadNumerosPlacaPorIdentificarse = 0,
                CantidadNumerosPlacaIdentificada = 0,
                CantidadPlacasNotaEntrada = 0
            };

            foreach (var item in notasEntradasService.renglones)
            {
                notasEntradasPlacas.NotasEntradasPlacas_Detalle.Add(new Entities.NotasEntradasPlacas_Detalle()
                {
                    CantidadNumerosPlacaPorIdentificarse = int.Parse(item.cantidad.ToString()),
                    CantidadNumerosPlacaIdentificada = 0,
                    CantidadPlacas = int.Parse(item.cantidad.ToString()),
                    CostoPlaca = item.precio,
                    CostoTotal = item.total,
                    NumeroRenglon = item.ren_renglon,
                    CodigoArticuloInfofin = item.articulo,
                    CuentaContable = item.cuenta_contable,
                    CentroCostosAlmacen = item.centro_costos,
                    FechaRecepcionTP = item.fecha_recepcion
                });
            }

            return notasEntradasPlacas;
        }
    }
}

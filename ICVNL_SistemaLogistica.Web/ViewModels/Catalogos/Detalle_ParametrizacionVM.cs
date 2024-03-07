using ICVNL_SistemaLogistica.Web.Entities;
using System.ComponentModel.DataAnnotations;

namespace ICVNL_SistemaLogistica.Web.ViewModels
{
    public class Detalle_ParametrizacionVM
    {
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Emails Dest. Solicitud Placas")]
        public string EmailDestinatariosSolicitudPlacas { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Emails Dest. Notificar Placas Vendidas")]
        public string EmailDestinatariosNotificaPlacasVendidas { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Clave Entidad Gobierno")]
        public string ClaveEntidadGobierno { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cta. Costos Venta Placa Vendida")]
        public string CuentaCostosVentaPlacaVendida { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "C. Costos Entidad Gob. Placa Vendida")]
        public string CentroCostosEntidadGobiernoPlacaVendida { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "Cta. Costos Venta Placa con Reporte")]
        public string CuentaCostosVentaPlacaReporte { get; set; }

        [Required(ErrorMessage = "Campo Requerido")]
        [Display(Name = "C. Costos Entidad Gob. Placa con Reporte")]
        public string CentroCostosEntidadGobiernoPlacaReporte { get; set; }

        [Display(Name = "Última Ejecución Exitosa en Notas de Entradas")]
        public string FechaUltimaEjecucionNE { get; set; }

        [Display(Name = "Tipo Póliza Placas Vendidas")]
        public int TipoPolizaPlacaVendida { get; set; }

        [Display(Name = "Tipo Póliza Placas Reporte")]
        public int TipoPolizaPlacaReporte { get; set; }
        public static Detalle_ParametrizacionVM operator +(Detalle_ParametrizacionVM parametrizacionVM, Parametrizacion parametrizacion)
        {
            parametrizacionVM.Id = parametrizacion.IdParametrizacion;
            parametrizacionVM.EmailDestinatariosSolicitudPlacas = parametrizacion.EmailDestinatariosSolicitudPlacas;
            parametrizacionVM.EmailDestinatariosNotificaPlacasVendidas = parametrizacion.EmailDestinatariosNotificaPlacasVendidas;
            parametrizacionVM.ClaveEntidadGobierno = parametrizacion.ClaveEntidadGobierno;
            parametrizacionVM.CuentaCostosVentaPlacaVendida = parametrizacion.CuentaCostosVentaPlacaVendida;
            parametrizacionVM.CentroCostosEntidadGobiernoPlacaVendida = parametrizacion.CentroCostosEntidadGobiernoPlacaVendida;
            parametrizacionVM.CuentaCostosVentaPlacaReporte = parametrizacion.CuentaCostosVentaPlacaReporte;
            parametrizacionVM.CentroCostosEntidadGobiernoPlacaReporte = parametrizacion.CentroCostosEntidadGobiernoPlacaReporte;
            parametrizacionVM.FechaUltimaEjecucionNE = parametrizacion.FechaUltimaEjecucionNE == null ? "" : parametrizacion.FechaUltimaEjecucionNE.Value.ToString("dd/MM/yyyy hh:mm:ss tt");
            parametrizacionVM.TipoPolizaPlacaReporte = parametrizacion.TipoPolizaPlacaReporte;
            parametrizacionVM.TipoPolizaPlacaVendida = parametrizacion.TipoPolizaPlacaVendida;
            return parametrizacionVM;
        }
    }
}
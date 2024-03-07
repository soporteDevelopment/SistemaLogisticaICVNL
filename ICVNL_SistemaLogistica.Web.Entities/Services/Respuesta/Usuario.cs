using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICVNL_SistemaLogistica.Web.Entities.Services.Respuesta
{
   
    public class UsuarioApiResponse
    {
        public double clave { get; set; }
        public string nombre { get; set; }
        public string nombre_usuario { get; set; }
        public double tiene_restriccion_compaia { get; set; }
        public double tiene_restriccion_funcion { get; set; }
        public string firma_electronica { get; set; }
        public double tiene_restriccion_centro_costos { get; set; }
        public string puesto { get; set; }
        public object rfc { get; set; }
        public double tiene_restriccion_estado_financiero { get; set; }
        public double nivel_seguridad { get; set; }
        public double nivel_seguridad_cuenta { get; set; }
        public object curp { get; set; }
        public object direccion { get; set; }
        public object ciudad { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }
        public object telefono_casa { get; set; }
        public object telefono_movil { get; set; }
        public object bipper { get; set; }
        public string correo_electronico { get; set; }
        public DateTime fecha_alta { get; set; }
        public string numero_empleado { get; set; }
        public double idioma { get; set; }
        public double tiene_restriccion_modulo { get; set; }
        public double es_superusuario { get; set; }
        public double dias_cambio_contrasea { get; set; }
        public DateTime fecha_ultimo_cambio_contrasea { get; set; }
        public double numero_maximo_entradas { get; set; }
        public double bloqueado { get; set; }
        public double es_rol { get; set; }
        public double utiliza_rol { get; set; }
        public object cual_rol { get; set; }
        public double comprador { get; set; }
        public double almacenista { get; set; }
        public double almacen_default { get; set; }
        public double autoriza_moneda_base { get; set; }
        public double autoriza_moneda_secundaria { get; set; }
        public double autoriza_solicitud { get; set; }
        public double tipo_usuario { get; set; }
        public object clave_tipo_usuario { get; set; }
        public double cambia_jefe { get; set; }
        public double nivel_autorizacion_gasto { get; set; }
        public double simplificado { get; set; }
        public double autoriza_orden_compra { get; set; }
        public double monto_maximo { get; set; }
        public double tiene_restriccion_familias { get; set; }
        public double autoriza_orden_compra_sn { get; set; }
        public double monto_maximo_aut { get; set; }
        public string sucursal_asignada { get; set; }
        public object certificado_digital { get; set; }
        public double tiene_restriccion_unidad_administrativa { get; set; }
        public double tiene_restriccion_categoria { get; set; }
        public double taller { get; set; }
        public int tiene_restriccion_presupuesto { get; set; }
        public int tiene_restriccion_rep { get; set; }
        public int tiene_restriccion_secretarias { get; set; }
        public int tiene_restriccion_dependencias { get; set; }
        public int tiene_restriccion_programa { get; set; }
        public int tiene_restriccion_subprograma { get; set; }
        public int tiene_restriccion_presupuesto_programa { get; set; }
        public int tiene_restriccion_transf { get; set; }
        public double porcentaje_cotizacion { get; set; }
        public double porcentaje_en_pedidos { get; set; }
        public double porcentaje_en_pedidos_cotizacion { get; set; }
        public int exede_descuentos_autorizacion { get; set; }
        public DateTime fechainicio { get; set; }
        public int cencos_destino_trasp_sn { get; set; }
        public int usuario_compras_sn { get; set; }
        public int usuario_vi_sn { get; set; }
        public int usuario_pptos_sn { get; set; }
        public object cencos { get; set; }
        public object cotizacionIni { get; set; }
        public object cotizacionFin { get; set; }
        public double tiene_restriccion_tipo_proveedor { get; set; }
        public object formato_fecha { get; set; }
        public object ipAddress { get; set; }
        public int idIdioma { get; set; }
        public object idUsuario { get; set; }
    }
    public class ResponseUsuario
    {
        public UsuarioApiResponse usuario { get; set; }
        public int acknowledge { get; set; }
        public object correlationId { get; set; }
        public object messageId { get; set; }
        public object messageParameters { get; set; }
        public string messageText { get; set; }
        public double idEvento { get; set; }
        public int totalRows { get; set; }
        public int language { get; set; }
        public int userId { get; set; }
        public int companyId { get; set; }
        public int idalmacen { get; set; }
        public object idsucursal { get; set; }
        public object idProveedor { get; set; }
    }
}

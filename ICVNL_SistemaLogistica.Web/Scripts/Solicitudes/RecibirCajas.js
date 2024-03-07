var Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM = [];

function AbrirModalAgregarCaja() {
    $('#modalAgregarCaja').modal('show');
}
function CargaInformacion() {
    var IdRecepcion_ = $.urlParam('IdRecepcion') == null ? 0 : $.urlParam('IdRecepcion')
    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "AddCajas") {
        ShowNotificacion("success", "Mensaje Sistema", "Se agregaron correctamente las cajas", 0, 6);
    } else if (option_ == "ErrorCajas") {
        ShowNotificacion("error", "Error", "Hubo uno/s errores al recibir las cajas, puede consultar los eventos surgidos en el listado", 0, 6);
    }
}

function ValidaGuardarCaja() {
    if (!validarCamposObligatorios('validaCampos')) {
        if (!validarTextoCamposObligatorios('validaCamposText')) {
            return;
        }
        return;
    }

    var objCaja = {
        IdEventoRecepcion: 0,
        IdValidacionEvento: 0,
        Consecutivo: Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM.length + 1,
        IdRecepcion: $.urlParam('IdRecepcion') == null ? 0 : $.urlParam('IdRecepcion'),
        FechaHoraRegistro: null,
        IdDelegacionBanco: $("#txtRecibirPlacas_IdDelegacionBanco").val(),
        IdProveedor: $("#txtRecibirPlacas_IdProveedor").val(),
        NumeroCaja: $("#txtRecibirPlacas_NumeroCaja").val(),
        IdTipoPlaca: $("#txtRecibirPlacas_IdTipoPlaca").val(),
        RangosPlacas: $("#txtRecibirPlacas_RangoInicial").val() + " - " + $("#txtRecibirPlacas_RangoFinal").val(),
        RangoInicial: $("#txtRecibirPlacas_RangoInicial").val(),
        RangoFinal: $("#txtRecibirPlacas_RangoFinal").val(),
        CantidadLaminas: $("#txtRecibirPlacas_CantidadLaminas").val()
    }

    Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM.push(objCaja);

    $.ajax({
        data: { Listado: Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM, IdRecepcion: ($.urlParam('IdRecepcion') == null ? 0 : parseInt($.urlParam('IdRecepcion'))) },
        type: "POST",
        url: '../SolicitudesPlacasRecepcion/GuardaCajaTemp',
        success: function (partialView) {
            $("#listadoPartialDetalleRecibirCaja").html(partialView);
            ShowNotificacion("success", "Mensaje Sistema", "Se agrego correctamente la caja", 0, 6);
            limpiarCaptura();
        },
        error: function (reponse) {
            console.log(reponse);
        }
    }); 

    removeClassValidaObligatorio("validaCampos");
    removeClassValidaObligatorio("validaCamposText");
}

function limpiarCaptura() { 
    $("#txtRecibirPlacas_NumeroCaja").val('');
    $("#txtRecibirPlacas_IdTipoPlaca").val('0');
    $("#txtRecibirPlacas_RangoInicial").val(''); 
    $("#txtRecibirPlacas_RangoFinal").val('');
    $("#txtRecibirPlacas_CantidadLaminas").val('');
}

function EliminarCaja(Consecutivo) {
    var indexRow = Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));
    Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM.splice(indexRow, 1);

    var url = '../SolicitudesPlacasRecepcion/EliminarCaja';
    $.ajax({
        data: { Listado: Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM },
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoPartialDetalleRecibirCaja").html(partialView);
            ShowNotificacion("success", "Mensaje Sistema", "Caja Eliminada Correctamente", 0, 6);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function RealizarRecepcionCajas() {
    $.ajax({
        data: { Listado: Listado_SolicitudesPlacasRecepcion_RecibirPlacasVM, IdRecepcion: ($.urlParam('IdRecepcion') == null ? 0 : parseInt($.urlParam('IdRecepcion'))) },
        type: "POST",
        url: '../SolicitudesPlacasRecepcion/RealizarRecepcionCajas',
        success: function (response) {
            if (response.ExecutionOK) {
                if (response.Data == "success") {
                    window.location.href = "../SolicitudesPlacas/RecibirPlacas?IdRecepcion=" + response.Data.IdSolicitud + "&op=AddCajas";
                } else {
                    window.location.href = "../SolicitudesPlacas/RecibirPlacas?IdRecepcion=" + response.Data.IdSolicitud + "&op=ErrorCajas";
                }
            } else {
                ShowNotificacion("error", "Error", response.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    return decodeURI(results[1]) || 0;
}
 
$(document).ready(function () {
    

    const isMobile = {
        Android: function () {
            return navigator.userAgent.match(/Android/i);
        },
        BlackBerry: function () {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function () {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        Opera: function () {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        Windows: function () {
            return navigator.userAgent.match(/IEMobile/i) || navigator.userAgent.match(/WPDesktop/i);
        },
        any: function () {
            return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
        }
    };

    $('#tablaRegistros').DataTable({
        lengthChange: false,
        paging: true,
        searching: false,
        ordering: true,
        responsive: isMobile.any() ? true : false,
        bInfo: false,
        language: {
            emptyTable: "Sin información.",
            info: "Registros del <b>_START_</b> al <b>_END_</b> de <b>_TOTAL_</b> por página",
            infoEmpty: "Sin información",
            infoFiltered: "(filtered1 de _MAX_ total registros)",
            lengthMenu: "_MENU_ registros por p&#225;gina",
            search: "Buscar:",
            zeroRecords: "Sin coincidencias encontradas."
        },
    });

    $('#tablaRegistros').find('[data-toggle="tooltip"]').tooltip();

});
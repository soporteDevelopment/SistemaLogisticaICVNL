var objRecepcionSolicitudPlacas = {
    IdRecepcion: 0,
    FolioRecepcion: '',
    IdSolicitud: 0,
    SolicitudesPlacas: {},
    IdDelegacionBanco : 0,
    DelegacionesBancos: {},
    NotaEntradaAutorizada : '',
    Recibida: '',
    RecepcionSolicitudesPlacas_Detalle: []
}

var objSolicitudesPlacasRecepcionDetails = {
    IdRecepcion : 0,
    IdTipoPlaca : 0,
    TiposPlacas : { } ,
    CantidadSolicitadaOrdenCompra : 0,
    CantidadNotasEntradaAutorizada : 0,
    CantidadRecibida : 0
}

function CargaInformacionRecepcionSolicitud() {
    var IdRecepcion_ = $.urlParam('IdRecepcion') == null ? 0 : $.urlParam('IdRecepcion')
    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "Add") {
        ShowNotificacion("success", "Mensaje Sistema", "Se agregó correctamente la Recepción de la Solicitud de Placas", 0, 6);
    }
    else if (option_ == "Ed") {
        ShowNotificacion("success", "Mensaje Sistema", "Se modificó correctamente Recepción de la Solicitud de Placas", 0, 6);
    }
    var url = '../SolicitudesPlacasRecepcion/RecepcionSolicitudDetails';
    $.ajax({
        data: { IdRecepcionSolicitud: IdRecepcion_ },
        type: "POST",
        url: url,
        success: function (data) {
            console.log(data);
            if (data.ExecutionOK) {
                objRecepcionSolicitudPlacas = data.Data;

                //Llenamos el combo de Solicitudes
                $.ajax({
                    data: { "IdDelegacionBanco": objRecepcionSolicitudPlacas.IdDelegacionBanco },
                    type: "POST",
                    url: "../SolicitudesPlacasRecepcion/GetDatosSolicitudesPlacas",
                    success: function (data) {
                        // VACIAMOS EL DropDownList
                        $("#ddlListadoSolicitudesPlacasDDL").empty();
                        // CONSTRUIMOS EL DropDownList A PARTIR DEL RESULTADO Json (data)
                        $.each(data, function (index, row) {
                            $("#ddlListadoSolicitudesPlacasDDL").append("<option value='" + row.Valor + "'>" + row.Texto + "</option>")
                        });
                        $("#ddlListadoSolicitudesPlacasDDL").val(objRecepcionSolicitudPlacas.IdSolicitud)
                    },
                    error: function (reponse) {
                        ShowNotificacion("error", "Error", "Error al obtener las solicitudes relacionadadas a la delegación seleccionada", "4", "0")
                        console.log(reponse);
                    }
                });

                $("#txtFolioRecepcion").val(objRecepcionSolicitudPlacas.FolioRecepcion)
                $("#ddlListadoDelegacionesDDL").val(objRecepcionSolicitudPlacas.IdDelegacionBanco)
                $("#txtNombreProveedor").val(objRecepcionSolicitudPlacas.SolicitudesPlacas.Proveedores.NombreProveedor);
                $("#txtNumeroContrato").val(objRecepcionSolicitudPlacas.SolicitudesPlacas.Contratos.NumeroContrato);
                $("#txtOrdenCompra").val(objRecepcionSolicitudPlacas.SolicitudesPlacas.OrdenCompra)
                $("#txtNotaEntradaAutorizada").val(objRecepcionSolicitudPlacas.NotaEntradaAutorizada)

                $.ajax({
                    data: { ListSolicitudesPlacasRecepcionDetails: objRecepcionSolicitudPlacas.RecepcionSolicitudesPlacas_Detalle },
                    type: "POST",
                    url: '../SolicitudesPlacasRecepcion/GetDetalleRecepcionSolicitud',
                    success: function (partialView) {
                        $("#listadoPartialDetalleSolicitud").html(partialView);
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function CreateRecepcionSolicitud() {
    if (!validarCamposObligatorios('validaCamposRecepcionSolicitudPlacas')) {
        if (!validarTextoCamposObligatorios('validaCamposRecepcionSolicitudPlacasText')) {
            return;
        }
        return;
    }
    objRecepcionSolicitudPlacas.NotaEntradaAutorizada = $("#txtNotaEntradaAutorizada").val();

    $.ajax({
        data: { viewModel: objRecepcionSolicitudPlacas },
        type: "POST",
        url: "../SolicitudesPlacasRecepcion/UpsertRecepcionSolicitud",
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../SolicitudesPlacasRecepcion/Details?IdRecepcion=" + data.Data.IdRecepcion + "&op=Add";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });


    removeClassValidaObligatorio("validaCamposRecepcionSolicitudPlacas");
    removeClassValidaObligatorio("validaCamposRecepcionSolicitudPlacasText");

}

function ValidaNumeroEntradaAutorizada() {
    if ($("#txtNotaEntradaAutorizada").val() != "") {
        GetCargaDatosSolicitudesPlacas();

        $.ajax({
            data: { NumeroEntrada: $("#txtNotaEntradaAutorizada").val() },
            type: "POST",
            url: "../SolicitudesPlacasRecepcion/ValidaNumeroEntrada",
            success: function (data) {
                if (!data.ExecutionOK) {
                    ShowNotificacion("error", "Error", data.Message, 0, 6);
                }
            },
            error: function (reponse) {
                console.log(reponse);
            }
        }); 
    }
}

function UpdateRecepcionSolicitud() {
    if (!validarCamposObligatorios('validaCamposRecepcionSolicitudPlacas')) {
        if (!validarTextoCamposObligatorios('validaCamposRecepcionSolicitudPlacasText')) {
            return;
        }
        return;
    }
    var IdRecepcion_ = $.urlParam('IdRecepcion') == null ? 0 : $.urlParam('IdRecepcion')
    objRecepcionSolicitudPlacas.IdRecepcion = parseInt(IdRecepcion_);
    objRecepcionSolicitudPlacas.NotaEntradaAutorizada = $("#txtNotaEntradaAutorizada").val();

    $.ajax({
        data: { viewModel: objRecepcionSolicitudPlacas },
        type: "POST",
        url: "../SolicitudesPlacasRecepcion/UpsertRecepcionSolicitud",
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../SolicitudesPlacasRecepcion/Details?IdRecepcion=" + data.Data.IdRecepcion + "&op=Ed";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });


    removeClassValidaObligatorio("validaCamposRecepcionSolicitudPlacas");
    removeClassValidaObligatorio("validaCamposRecepcionSolicitudPlacasText");

}

function GetSolicitudesPlacas() { 
    limpiaControlesSolicitud();

    if ($('#ddlListadoDelegacionesDDL').val() != "0") {
        var delegacionSeleccionada = parseInt($('#ddlListadoDelegacionesDDL').val()); 

        //Llenamos el combo de Solicitudes
        $.ajax({
            data: { "IdDelegacionBanco": delegacionSeleccionada },
            type: "POST",
            url: "../SolicitudesPlacasRecepcion/GetDatosSolicitudesPlacas",
            success: function (data) { 
                // VACIAMOS EL DropDownList
                $("#ddlListadoSolicitudesPlacasDDL").empty();
                // CONSTRUIMOS EL DropDownList A PARTIR DEL RESULTADO Json (data)
                $.each(data, function (index, row) {
                    $("#ddlListadoSolicitudesPlacasDDL").append("<option value='" + row.Valor + "'>" + row.Texto + "</option>")
                });
            },
            error: function (reponse) {
                ShowNotificacion("error", "Error", "Error al obtener las solicitudes relacionadadas a la delegación seleccionada", "4", "0")
                console.log(reponse);
            }
        });
    }
}

function GetCargaDatosSolicitudesPlacas() {
    limpiaControles();
    if ($('#ddlListadoSolicitudesPlacasDDL').val() != "0" && $('#txtNotaEntradaAutorizada').val() != "") {
        var solicitudSeleccionada = parseInt($('#ddlListadoSolicitudesPlacasDDL').val());
        var folioEntrada = parseInt($('#txtNotaEntradaAutorizada').val());

        $.ajax({
            data: { "IdSolicitud": solicitudSeleccionada, "FolioEntrada": folioEntrada },
            type: "POST",
            url: "../SolicitudesPlacas/SolicitudNotaEntradaDetails",
            success: function (data) {
                console.log(data);
                if (data.ExecutionOK) {

                    var objSolicitud = data.Data;

                    objRecepcionSolicitudPlacas.IdRecepcion = 0;
                    objRecepcionSolicitudPlacas.FolioRecepcion = $("#txtFolioRecepcion").val();
                    objRecepcionSolicitudPlacas.IdSolicitud = objSolicitud.IdSolicitud;
                    objRecepcionSolicitudPlacas.SolicitudesPlacas = objSolicitud;
                    objRecepcionSolicitudPlacas.IdDelegacionBanco = parseInt($("#ddlListadoDelegacionesDDL").val());
                    objRecepcionSolicitudPlacas.DelegacionesBancos = {
                        NombreDelegacionBanco: $('#ddlListadoDelegacionesDDL').find(":selected").text()
                    };
                    objRecepcionSolicitudPlacas.NotaEntradaAutorizada =  '';
                    objRecepcionSolicitudPlacas.Recibida = 0;
                    objRecepcionSolicitudPlacas.RecepcionSolicitudesPlacas_Detalle = []

                    $.each(objSolicitud.Detalle_SolicitudesPlacasDetailsVM, function (index, row) {
                        objSolicitudesPlacasRecepcionDetails.IdRecepcion = 0;
                        objSolicitudesPlacasRecepcionDetails.IdTipoPlaca = row.IdTipoPlaca;
                        objSolicitudesPlacasRecepcionDetails.TiposPlacas = {
                            TipoPlaca: row.TiposPlacas.TipoPlaca
                        };
                        objSolicitudesPlacasRecepcionDetails.CantidadSolicitadaOrdenCompra = row.CantidadPlacasOrdenCompra;
                        objSolicitudesPlacasRecepcionDetails.CantidadNotasEntradaAutorizada = row.CantidadPlacasNotaEntrada;
                        objSolicitudesPlacasRecepcionDetails.CantidadRecibida = 0;

                        objRecepcionSolicitudPlacas.RecepcionSolicitudesPlacas_Detalle.push(objSolicitudesPlacasRecepcionDetails);
                        objSolicitudesPlacasRecepcionDetails = {};
                    });

                    $("#txtNombreProveedor").val(objSolicitud.Proveedores.NombreProveedor);
                    $("#txtNumeroContrato").val(objSolicitud.Contratos.NumeroContrato);
                    $("#txtOrdenCompra").val(objSolicitud.OrdenCompra)

                    $.ajax({
                        data: { ListSolicitudesPlacasRecepcionDetails: objRecepcionSolicitudPlacas.RecepcionSolicitudesPlacas_Detalle },
                        type: "POST",
                        url: '../SolicitudesPlacasRecepcion/GetDetalleRecepcionSolicitud',
                        success: function (partialView) {
                            $("#listadoPartialDetalleSolicitud").html(partialView); 
                        },
                        error: function (reponse) {
                            console.log(reponse);
                        }
                    });


                }
                else {
                    ShowNotificacion("error", "Error", data.Message, 0, 6);                    
                }
            },
            error: function (reponse) {
                ShowNotificacion("error", "Error", "Error la información de la Solicitud de Placas seleccionada", "4", "0")
                console.log(reponse);
            }
        });
    }
}

function limpiaControlesSolicitud() {
    objRecepcionSolicitudPlacas = {
        IdRecepcion: 0,
        FolioRecepcion: '',
        IdSolicitud: 0,
        SolicitudesPlacas: {},
        IdDelegacionBanco: 0,
        DelegacionesBancos: {},
        NotaEntradaAutorizada: '',
        Recibida: '',
        RecepcionSolicitudesPlacas_Detalle: []
    }
    $("#ddlListadoSolicitudesPlacasDDL").empty();
    $("#ddlListadoSolicitudesPlacasDDL").append("<option value='0'>Seleccione</option>") 
    $("#txtNombreProveedor").val('');
    $("#txtNumeroContrato").val('');
    $("#txtOrdenCompra").val('')

    $.ajax({
        data: { ListSolicitudesPlacasRecepcionDetails: objRecepcionSolicitudPlacas.RecepcionSolicitudesPlacas_Detalle },
        type: "POST",
        url: '../SolicitudesPlacasRecepcion/GetDetalleRecepcionSolicitud',
        success: function (partialView) {
            $("#listadoPartialDetalleSolicitud").html(partialView);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function limpiaControles() {
    objRecepcionSolicitudPlacas = {
        IdRecepcion: 0,
        FolioRecepcion: '',
        IdSolicitud: 0,
        SolicitudesPlacas: {},
        IdDelegacionBanco: 0,
        DelegacionesBancos: {},
        NotaEntradaAutorizada: '',
        Recibida: '',
        RecepcionSolicitudesPlacas_Detalle: []
    }

    $("#txtNombreProveedor").val('');
    $("#txtNumeroContrato").val('');
    $("#txtOrdenCompra").val('')

    $.ajax({
        data: { ListSolicitudesPlacasRecepcionDetails: objRecepcionSolicitudPlacas.RecepcionSolicitudesPlacas_Detalle },
        type: "POST",
        url: '../SolicitudesPlacasRecepcion/GetDetalleRecepcionSolicitud',
        success: function (partialView) {
            $("#listadoPartialDetalleSolicitud").html(partialView);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}


function DescargarArchivo(IdArchivo_) { 
    $.ajax({
        data: { IdArchivo: IdArchivo_ },
        type: "POST",
        url: "../SolicitudesPlacasRecepcion/DescargarArchivo",
        success: function (res) {
            if (res.ExecutionOK) {
                objArchivo = res.Data;
                var sampleArr = base64ToArrayBuffer(objArchivo.ArchivoBase64);
                saveByteArray(objArchivo.NombreArchivo, sampleArr);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}
function base64ToArrayBuffer(base64) {
    var binaryString = window.atob(base64);
    var binaryLen = binaryString.length;
    var bytes = new Uint8Array(binaryLen);
    for (var i = 0; i < binaryLen; i++) {
        var ascii = binaryString.charCodeAt(i);
        bytes[i] = ascii;
    }
    return bytes;
}
function saveByteArray(reportName, byte) {
    var blob = new Blob([byte], { type: "application/pdf" });
    var link = document.createElement('a');
    link.href = window.URL.createObjectURL(blob);
    var fileName = reportName;
    link.download = fileName;
    link.click();
};
$(document).ready(function () {
    datepicker = $('#txtFechaEntrega').datepicker({
        format: 'dd/mm/yyyy',
        language: 'es-MX',
        autoclose: true
    });

    if ($(".datemaskSD").val() == "1/1/0001 12:00:00 AM") {
        $(".datemaskSD").mask("");
    }

    $.urlParam = function (name) {
        var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (results == null) {
            return null;
        }
        return decodeURI(results[1]) || 0;
    }


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
        ordering: false,
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
});
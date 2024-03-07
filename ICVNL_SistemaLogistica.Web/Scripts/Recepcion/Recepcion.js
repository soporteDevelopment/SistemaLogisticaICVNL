var Listado_RecepcionPlacas_IndividualesModel = [];

function RecibirTransferencia() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    bootbox.confirm({
        message: "¿Estas seguro de recibir la transferencia totalmente?",
        buttons: {
            confirm: {
                label: 'Sí',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        }, 
        callback: function (result) {
            if (result) {
                $.ajax({
                    data: { "IdTransferencia": IdTransferencia_, "EstatusTransferencia" : 5 },
                    type: "POST",
                    url: '../RecepcionPlacas/CambioEstatusTransferencia',
                    success: function (response) {
                        if (response.ExecutionOK) {
                            window.location.href = "Details?IdTransferencia=" + IdTransferencia_ + "&op=TC"; 
                        }
                        else {
                            ShowNotificacion("error", "Error", response.Message);
                        }
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });
            }
        }
    });
}
function RechazarTransferencia() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    bootbox.confirm({
        message: "¿Estas seguro de rechazar la transferencia?",
        buttons: {
            confirm: {
                label: 'Sí',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    data: { "IdTransferencia": IdTransferencia_, "EstatusTransferencia": 4 },
                    type: "POST",
                    url: '../RecepcionPlacas/CambioEstatusTransferencia',
                    success: function (response) {
                        if (response.ExecutionOK) {
                            window.location.href = "Details?IdTransferencia=" + IdTransferencia_ + "&op=TR"; 
                        }
                        else {
                            ShowNotificacion("error", "Error", response.Message);
                        }
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });
            }
        }
    });
}
function CerrarTransferencia() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    bootbox.confirm({
        message: "¿Estas seguro de cerrar la transferencia?",
        buttons: {
            confirm: {
                label: 'Sí',
                className: 'btn-success'
            },
            cancel: {
                label: 'No',
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                $.ajax({
                    data: { "IdTransferencia": IdTransferencia_, "IdEstatusTransferencia": 6 },
                    type: "POST",
                    url: '../RecepcionPlacas/CambioEstatusTransferencia',
                    success: function (response) {
                        if (response.ExecutionOK) {
                            ShowNotificacion("success", "Mensaje Sistema", response.Message);
                        }
                        else {
                            ShowNotificacion("error", "Error", response.Message);
                        }
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });
            }
        }
    });
}

function GetRecibirPlacasIndividuales() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    var IdTipoPlaca_ = $.urlParam('IdTipoPlaca') == null ? 0 : $.urlParam('IdTipoPlaca')

    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "PLIC") {
        ShowNotificacion("success", "Mensaje Sistema", "Se recibieron placas individualmenente de la transferencia de placas entre delegaciones", 0, 6);
    }
    else if (option_ == "TC") {
        ShowNotificacion("success", "Mensaje Sistema", "Transferencia de placas Recibida Correctamente", 0, 6);
    }
    else if (option_ == "TR") {
        ShowNotificacion("success", "Mensaje Sistema", "Transferencia de placas Rechazada Correctamente", 0, 6);
    }

    var url = '../RecepcionPlacas/GetRecibirPlacasIndividuales';
    $.ajax({
        data: { "IdTransferencia": IdTransferencia_, "IdTipoPlaca": IdTipoPlaca_ },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                Listado_RecepcionPlacas_IndividualesModel = data.Data.Listado;
                console.log(Listado_RecepcionPlacas_IndividualesModel);

            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function SeleccionaEstatus(id, NumeroPlaca, tipoId) {
    for (var i = 1; i <= 4; i++) {
        document.getElementById("chkbEstatus_" + i + "_" +NumeroPlaca).checked = false;
    }
    document.getElementById(id).checked = true;

    var index = Listado_RecepcionPlacas_IndividualesModel.findIndex(x => x.NumeroPlaca == NumeroPlaca);
    Listado_RecepcionPlacas_IndividualesModel[index].Disponible = false;
    Listado_RecepcionPlacas_IndividualesModel[index].Obsoleta = false;
    Listado_RecepcionPlacas_IndividualesModel[index].Rechazada = false;
    Listado_RecepcionPlacas_IndividualesModel[index].Perdida = false;
    if (tipoId == 1) {
        Listado_RecepcionPlacas_IndividualesModel[index].Disponible = true;
        Listado_RecepcionPlacas_IndividualesModel[index].IdTipoEstatusPlacas = 1;
    } else if (tipoId == 2) {
        Listado_RecepcionPlacas_IndividualesModel[index].Obsoleta = true;
        Listado_RecepcionPlacas_IndividualesModel[index].IdTipoEstatusPlacas = 2;
    } else if (tipoId == 3) {        
        Listado_RecepcionPlacas_IndividualesModel[index].Rechazada = true;
        Listado_RecepcionPlacas_IndividualesModel[index].IdTipoEstatusPlacas = 3;
    } else if (tipoId == 4) {
        Listado_RecepcionPlacas_IndividualesModel[index].Perdida = true;
        Listado_RecepcionPlacas_IndividualesModel[index].IdTipoEstatusPlacas = 4;
    }

    if (Listado_RecepcionPlacas_IndividualesModel[index].Disponible == false
        && Listado_RecepcionPlacas_IndividualesModel[index].Obsoleta == false
        && Listado_RecepcionPlacas_IndividualesModel[index].Rechazada == false
        && Listado_RecepcionPlacas_IndividualesModel[index].Perdida == false) {
        Listado_RecepcionPlacas_IndividualesModel[index].Seleccionado = false;
    }
    else {
        Listado_RecepcionPlacas_IndividualesModel[index].Seleccionado = true;
    }
    console.log(Listado_RecepcionPlacas_IndividualesModel);

}

function RecibirTransferenciaPlacasIndividual() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')

    var url = '../RecepcionPlacas/RecibirTransferenciaPlacasIndividual';
    $.ajax({
        data: { "IdTransferencia": IdTransferencia_ , "Listado": Listado_RecepcionPlacas_IndividualesModel },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../RecepcionPlacas/Details?IdTransferencia=" + data.Data + "&op=PLIC";
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function AgregarDatosPersona() {
    $('#modalAgregarDatosPersona').modal('show');
}
function AgregarDatosTransporte() {
    $('#modalAgregarDatosTransporte').modal('show');
}

function ValidaGuardarDatosPersona() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    if (!validarCamposObligatorios('validaCamposPersona')) {
        return;
    } 

    var TransferenciaPlacas_DatosPersonaEnvio =
    {
        IdTransferencia : parseInt(IdTransferencia_),
        Nombre : $('#txtRecepcionPlacas_DatosPersonaRecibe_Nombre').val(),
        Apellido : $('#txtRecepcionPlacas_DatosPersonaRecibe_Apellido').val(),
        IdTipoIDs : parseInt($('#ddlRecepcionPlacas_DatosPersonaRecibe_TipoID').val()),
        NumeroID : $('#txtRecepcionPlacas_DatosPersonaRecibe_NumeroID').val(),
    };

    $.ajax({
        data: { "DatosPersona": TransferenciaPlacas_DatosPersonaEnvio },
        type: "POST",
        url: '../RecepcionPlacas/GuardarDatosPersona',
        success: function (response) {
            if (response.ExecutionOK) {
                ShowNotificacion("success", "Mensaje Sistema", response.Message);
            }
            else {
                ShowNotificacion("error", "Error", response.Message);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });

    $('#modalAgregarDatosPersona').modal('hide');
}
function ValidaGuardarDatosTransporte() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia') 
    var TransferenciaPlacas_TransporteEnvio = {
        IdTransferencia : parseInt(IdTransferencia_),
        MarcaVehiculo : $('#txtRecepcionPlacas_TransporteRecibe_MarcaVehiculo').val(),
        ModeloVehiculo : $('#txtRecepcionPlacas_TransporteRecibe_ModeloVehiculo').val(),
        PlacasVehiculo : $('#txtRecepcionPlacas_TransporteRecibe_PlacasVehiculo').val(),
        NumeroEconomico : $('#txtRecepcionPlacas_TransporteRecibe_NumeroEconomico').val()
    };

    $.ajax({
        data: { "Transporte": TransferenciaPlacas_TransporteEnvio },
        type: "POST",
        url: '../RecepcionPlacas/GuardarDatosTransporte',
        success: function (response) {
            if (response.ExecutionOK) {
                ShowNotificacion("success", "Mensaje Sistema", response.Message);
            }
            else {
                ShowNotificacion("error", "Error", response.Message);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

$(document).ready(function () {

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
    $('#tablaRegistrosListado1').DataTable({
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

    $('#tablaRegistrosListado2').DataTable({
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
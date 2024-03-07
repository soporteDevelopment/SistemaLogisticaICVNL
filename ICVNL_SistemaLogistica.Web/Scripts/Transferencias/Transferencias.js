var TransferenciaPlacas = {
    IdTransferencia: 0,
    FolioTransferencia: null,
    FechaHoraRegistro: null,
    IdTransferenciaDatosPersona: 0,
    TransferenciaPlacas_DatosPersonaEnvio: {
        IdTransferenciaDatosPersona: 0,
        IdTransferencia: 0,
        Nombre : '',
        Apellido: '',
        IdTipoIDs: '',
        TiposID: {
            Id: 0,
            TipoID: ''
        },
        NumeroID :  '',
    },
    IdTransferenciaTransporte: 0,
    TransferenciaPlacas_TransporteEnvio: {},
    IdDelegacionBancoOrigen: 0,
    DelegacionesBancosOrigen: {},
    IdDelegacionBancoDestino: 0,
    DelegacionesBancosDestino: {},
    IdEstatusTransferencia: 0,
    TiposEstatusTransferencias: {},
    TransferenciaPlacas_Detalle: [],
    TransferenciaPlacas_Listado1: [],
    TransferenciaPlacas_Listado2: []
}

function CargaInformacionTransferencia() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "Add") {
        ShowNotificacion("success", "Mensaje Sistema", "Se agregó correctamente la transferencia de placas entre delegaciones", 0, 6);
    }
    else if (option_ == "Ed") {
        ShowNotificacion("success", "Mensaje Sistema", "Se modificó correctamente la transferencia de placas entre delegaciones", 0, 6);
    }
    var url = '../TransferenciaPlacas/TransferenciaDetails';
    $.ajax({
        data: { "IdTransferencia": parseInt(IdTransferencia_) },
        type: "POST",
        url: url,
        async: false,
        success: function (data) {
            console.log(data);
            if (data.ExecutionOK) {
                TransferenciaPlacas = data.Data; 
                if (TransferenciaPlacas.IdEstatusTransferencia != 1) { 
                    $('#ddlListadoDelegacionesOrigenDDL').prop("disabled", true);
                    $('#ddlListadoDelegacionesDestinoDDL').prop("disabled", true);
                    $('#txtDatosPersonaEnvio_Nombre').prop("disabled", true);
                    $('#txtDatosPersonaEnvio_Apellido').prop("disabled", true);
                    $('#ddlDatosPersonaEnvio_TipoID').prop("disabled", true);
                    $('#txtDatosPersonaEnvio_NumeroID').prop("disabled", true);
                    $('#txtTransporteEnvio_MarcaVehiculo').prop("disabled", true);
                    $('#txtTransporteEnvio_ModeloVehiculo').prop("disabled", true);
                    $('#txtTransporteEnvio_PlacasVehiculo').prop("disabled", true);
                    $('#txtTransporteEnvio_NumeroEconomico').prop("disabled", true); 
                    setTimeout(function () {
                        $('#tablaRegistrosListado1 tr').find('th:last-child, td:last-child').remove();
                        $('#tablaRegistrosListado2 tr').find('th:last-child, td:last-child').remove();
                    }, 20)
                   
                }

                $('#txtIdTransferencia').val(TransferenciaPlacas.IdTransferencia);
                $('#txtFolioTransferencia').val(TransferenciaPlacas.FolioTransferencia);
                $('#txtFechaHoraRegistro').val(TransferenciaPlacas.FechaHoraRegistroStr);
                $('#ddlListadoDelegacionesOrigenDDL').val(TransferenciaPlacas.IdDelegacionBancoOrigen);
                $('#ddlListadoDelegacionesDestinoDDL').val(TransferenciaPlacas.IdDelegacionBancoDestino);
                $('#txtDatosPersonaEnvio_Nombre').val(TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.Nombre);
                $('#txtDatosPersonaEnvio_Apellido').val(TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.Apellido);
                setTimeout(function () {
                    $('#ddlDatosPersonaEnvio_TipoID').val(TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.IdTipoIDs);
                },150)
                $('#txtDatosPersonaEnvio_NumeroID').val(TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.NumeroID);
                $('#txtTransporteEnvio_MarcaVehiculo').val(TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.MarcaVehiculo);
                $('#txtTransporteEnvio_ModeloVehiculo').val(TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.ModeloVehiculo);
                $('#txtTransporteEnvio_PlacasVehiculo').val(TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.PlacasVehiculo);
                $('#txtTransporteEnvio_NumeroEconomico').val(TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.NumeroEconomico);

                $.ajax({
                    data: { TransferenciaPlacas_Listado1: TransferenciaPlacas.TransferenciaPlacas_Listado1 },
                    type: "POST",
                    async:false, 
                    url: '../TransferenciaPlacas/GetTransferenciaPlacas_Listado1',
                    success: function (partialView) {
                        $("#listadoPartialTransferenciaListado1").html(partialView);
                        for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado1.length; i++) {
                            $('#txtCantidadDisponiblesSerTransferidas_tp_' + TransferenciaPlacas.TransferenciaPlacas_Listado1[i].IdTipoPlaca).on('change', function () {
                                var valueTP = $(this).attr("data-value-tp");
                                var valueText = $(this).val();

                                CantidadTipoPlaca(valueTP, valueText);
                            });
                        }
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });

                $.ajax({
                    data: { TransferenciaPlacas_Listado2: TransferenciaPlacas.TransferenciaPlacas_Listado2 },
                    type: "POST",
                    async:false, 
                    url: '../TransferenciaPlacas/GetTransferenciaPlacas_Listado2',
                    success: function (partialView) {
                        $("#listadoPartialTransferenciaListado2").html(partialView);
                        for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado2.length; i++) {
                            if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].TransferirPlaca) {
                                $('#chbxTransferirPlaca_' + TransferenciaPlacas.TransferenciaPlacas_Listado2[i].NumeroPlaca).prop("checked")
                            }
                            $('#chbxTransferirPlaca_' + TransferenciaPlacas.TransferenciaPlacas_Listado2[i].NumeroPlaca).on('change', function () {
                                var value = $(this).attr("data-value");
                                var valueTP = $(this).attr("data-value-tp");
                                var valueText = $(this).prop("checked")

                                CantidadesPlacas(value, valueTP, valueText);
                            });
                        }
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

function CargaPackingList() {
    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "GPL") {
        ShowNotificacion("success", "Mensaje Sistema", "Se generó correctamente el Packing List", 0, 6);
    }
}

function GeneraPackingList() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')

    var url = '../TransferenciaPlacas/GeneraPackingList';

    $.ajax({
        data: { IdTransferencia: IdTransferencia_ },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../TransferenciaPlacas/PackingList?IdTransferencia=" + IdTransferencia_ + "&op=GPL";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    }); 
}

function EnviarTransferenciaPlacas() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    window.location.href = "../TransferenciaPlacas/TransferirPlacas?IdTransferencia=" + IdTransferencia_;
}


function CreateTransferencia() {

    if (!validarTextoCamposObligatoriosTP('validaCamposPersona')) {
        return;
    }
    if (!validarTextoCamposObligatorios('validaCampoTransferencia')) {
        return;
    }

    var cantidadTransferir = 0;
    for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado1.length; i++) {
        if (TransferenciaPlacas.TransferenciaPlacas_Listado1[i].CantidadDisponiblesSerTransferidas != "") {
            cantidadTransferir += parseInt(TransferenciaPlacas.TransferenciaPlacas_Listado1[i].CantidadDisponiblesSerTransferidas);
        }
    }
    for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado2.length; i++) {
        if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].TransferirPlaca) {
            cantidadTransferir++;
        }
    }
    if (cantidadTransferir == 0) {
        ShowNotificacion("error", "Error", "No hay tipos de placas y/o placas seleccionadas para ser transferidas")
        return;
    }
    TransferenciaPlacas.FolioTransferencia = $('#txtFolioTransferencia').val();
    TransferenciaPlacas.IdDelegacionBancoOrigen = parseInt($('#ddlListadoDelegacionesOrigenDDL').val());
    TransferenciaPlacas.IdDelegacionBancoDestino = parseInt($('#ddlListadoDelegacionesDestinoDDL').val());
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.Nombre = $('#txtDatosPersonaEnvio_Nombre').val();
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.Apellido = $('#txtDatosPersonaEnvio_Apellido').val();
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.IdTipoIDs = parseInt($('#ddlDatosPersonaEnvio_TipoID').val());
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.NumeroID = $('#txtDatosPersonaEnvio_NumeroID').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.MarcaVehiculo = $('#txtTransporteEnvio_MarcaVehiculo').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.ModeloVehiculo = $('#txtTransporteEnvio_ModeloVehiculo').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.PlacasVehiculo = $('#txtTransporteEnvio_PlacasVehiculo').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.NumeroEconomico = $('#txtTransporteEnvio_NumeroEconomico').val(); 

    var url = '../TransferenciaPlacas/UpsertTransferenciaPlaca';

    $.ajax({
        data: { viewModel: TransferenciaPlacas },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../TransferenciaPlacas/Details?IdTransferencia=" + data.Data + "&op=Add";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
     
}

function ModifyTransferencia() {

    if (!validarTextoCamposObligatoriosTP('validaCamposPersona')) {
        return;
    }
    if (!validarTextoCamposObligatorios('validaCampoTransferencia')) {
        return;
    }

    var cantidadTransferir = 0;
    for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado1.length; i++) {
        if (TransferenciaPlacas.TransferenciaPlacas_Listado1[i].CantidadDisponiblesSerTransferidas != "") {
            cantidadTransferir += parseInt(TransferenciaPlacas.TransferenciaPlacas_Listado1[i].CantidadDisponiblesSerTransferidas);
        }
    }
    for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado2.length; i++) {
        if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].TransferirPlaca) {
            cantidadTransferir++;
        }
    }
    if (cantidadTransferir == 0) {
        ShowNotificacion("error", "Error", "No hay tipos de placas y/o placas seleccionadas para ser transferidas")
        return;
    }
    TransferenciaPlacas.FolioTransferencia = $('#txtFolioTransferencia').val();
    TransferenciaPlacas.IdDelegacionBancoOrigen = parseInt($('#ddlListadoDelegacionesOrigenDDL').val());
    TransferenciaPlacas.IdDelegacionBancoDestino = parseInt($('#ddlListadoDelegacionesDestinoDDL').val());
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.Nombre = $('#txtDatosPersonaEnvio_Nombre').val();
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.Apellido = $('#txtDatosPersonaEnvio_Apellido').val();
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.IdTipoIDs = parseInt($('#ddlDatosPersonaEnvio_TipoID').val());
    TransferenciaPlacas.TransferenciaPlacas_DatosPersonaEnvio.NumeroID = $('#txtDatosPersonaEnvio_NumeroID').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.MarcaVehiculo = $('#txtTransporteEnvio_MarcaVehiculo').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.ModeloVehiculo = $('#txtTransporteEnvio_ModeloVehiculo').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.PlacasVehiculo = $('#txtTransporteEnvio_PlacasVehiculo').val();
    TransferenciaPlacas.TransferenciaPlacas_TransporteEnvio.NumeroEconomico = $('#txtTransporteEnvio_NumeroEconomico').val();

    var url = '../TransferenciaPlacas/UpsertTransferenciaPlaca';

    $.ajax({
        data: { viewModel: TransferenciaPlacas },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../TransferenciaPlacas/Details?IdTransferencia=" + TransferenciaPlacas.IdTransferencia + "&op=Ed";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    }); 
}
function GetTransferenciaPlacasOrigen() {
    validaDelegaciones();
    if ($('#ddlListadoDelegacionesOrigenDDL').val() != "0") {
        var idDelegacionOrigen_ = parseInt($('#ddlListadoDelegacionesOrigenDDL').val()); 
        var url = '../TransferenciaPlacas/GetPlacasDelegacionOrigen';
        $.ajax({
            data: { IdDelegeacionOrigen: idDelegacionOrigen_ },
            type: "POST",
            url: url,
            async: false,
            success: function (data) {
                console.log(data);
                if (data.ExecutionOK) {
                    TransferenciaPlacas.TransferenciaPlacas_Listado1 = data.Data.TransferenciaPlacas_Listado1;       
                    TransferenciaPlacas.TransferenciaPlacas_Listado2 = data.Data.TransferenciaPlacas_Listado2;       

                    $.ajax({
                        data: { TransferenciaPlacas_Listado1: TransferenciaPlacas.TransferenciaPlacas_Listado1 },
                        type: "POST",
                        url: '../TransferenciaPlacas/GetTransferenciaPlacas_Listado1',
                        success: function (partialView) {
                            $("#listadoPartialTransferenciaListado1").html(partialView);
                            for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado1.length; i++) {
                                $('#txtCantidadDisponiblesSerTransferidas_tp_' + TransferenciaPlacas.TransferenciaPlacas_Listado1[i].IdTipoPlaca).on('change', function () {
                                    var valueTP = $(this).attr("data-value-tp");
                                    var valueText = $(this).val();

                                    CantidadTipoPlaca(valueTP, valueText);
                                });
                            }
                        },
                        error: function (reponse) {
                            console.log(reponse);
                        }
                    });

                    $.ajax({
                        data: { TransferenciaPlacas_Listado2: TransferenciaPlacas.TransferenciaPlacas_Listado2 },
                        type: "POST",
                        url: '../TransferenciaPlacas/GetTransferenciaPlacas_Listado2',
                        success: function (partialView) {
                            $("#listadoPartialTransferenciaListado2").html(partialView);
                            for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado2.length; i++) {
                                $('#chbxTransferirPlaca_' + TransferenciaPlacas.TransferenciaPlacas_Listado2[i].NumeroPlaca).on('change', function () {
                                    var value = $(this).attr("data-value");
                                    var valueTP = $(this).attr("data-value-tp");
                                    var valueText = $(this).prop("checked")

                                    CantidadesPlacas(value, valueTP, valueText);
                                });
                            }
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
                console.log(reponse);
            }
        });
               


    }
}

function CantidadTipoPlaca(valueTP, valueText) {
    if (valueTP == undefined) {
        return;
    }
    var index = TransferenciaPlacas.TransferenciaPlacas_Listado1.findIndex(x => x.IdTipoPlaca == parseInt(valueTP));
    var cantidadDisponibles = parseInt(TransferenciaPlacas.TransferenciaPlacas_Listado1[index].CantidadDisponiblesDelegacion);
    var cantidadTransferir= parseInt(valueText);
    if (cantidadTransferir > cantidadDisponibles) {
        
        $('#txtCantidadDisponiblesSerTransferidas_tp_' + valueTP).val('')
        ShowNotificacion("error", "Error", "La cantidad disponible a ser Transferida no puede ser mayor que la cantidad disponibles de placas de la Delegación Origen")
        return;
    }

    var cantidadDisponibleLst2 = 0;
    for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado2.length; i++) {
        if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].IdTipoPlaca == parseInt(valueTP) && TransferenciaPlacas.TransferenciaPlacas_Listado2[i].IdTipoEstatusPlacas == 1) {

            if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].TransferirPlaca) {
                cantidadDisponibleLst2++;
            }
        }
    }
    if ((cantidadTransferir + cantidadDisponibleLst2)  > cantidadDisponibles) {

        $('#txtCantidadDisponiblesSerTransferidas_tp_' + valueTP).val('')
        ShowNotificacion("error", "Error", "La cantidad disponible a ser Transferida más la cantidad seleccionada del listado 2 no puede ser mayor que la cantidad disponibles de placas de la Delegación Origen")
        return;
    }
    var index = TransferenciaPlacas.TransferenciaPlacas_Listado1.findIndex(x => x.IdTipoPlaca == parseInt(valueTP));
    var cantidadDisponibles = parseInt(TransferenciaPlacas.TransferenciaPlacas_Listado1[index].CantidadDisponiblesDelegacion);



    TransferenciaPlacas.TransferenciaPlacas_Listado1[index].CantidadDisponiblesSerTransferidas = valueText;
    
}

function CantidadesPlacas(value, valueTP, valueText) {
    if (value == undefined) {
        return;
    }
    var index = TransferenciaPlacas.TransferenciaPlacas_Listado2.findIndex(x => x.NumeroPlaca == value && x.IdTipoPlaca == parseInt(valueTP)); 
    TransferenciaPlacas.TransferenciaPlacas_Listado2[index].TransferirPlaca = valueText; 

    var cantidadDisponibleLst2 = 0;
    for (var i = 0; i < TransferenciaPlacas.TransferenciaPlacas_Listado2.length; i++) {
        if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].IdTipoPlaca == parseInt(valueTP) && TransferenciaPlacas.TransferenciaPlacas_Listado2[i].IdTipoEstatusPlacas == 1) {

            if (TransferenciaPlacas.TransferenciaPlacas_Listado2[i].TransferirPlaca) {
                cantidadDisponibleLst2++;
            }
        }
    }

    var index = TransferenciaPlacas.TransferenciaPlacas_Listado1.findIndex(x => x.IdTipoPlaca == parseInt(valueTP));
    var cantidadDisponibles = parseInt(TransferenciaPlacas.TransferenciaPlacas_Listado1[index].CantidadDisponiblesDelegacion);
    var CantidadDisponiblesSerTransferidas = parseInt(TransferenciaPlacas.TransferenciaPlacas_Listado1[index].CantidadDisponiblesSerTransferidas);

    if ((CantidadDisponiblesSerTransferidas + cantidadDisponibleLst2) > cantidadDisponibles) {

        $('#chbxTransferirPlaca_' + value).removeProp("checked")
        ShowNotificacion("error", "Error", "La cantidad disponible a ser Transferida más la cantidad seleccionada del listado 2 no puede ser mayor que la cantidad disponibles de placas de la Delegación Origen")
        return;
    }

}

function validaDelegaciones(){
    var idDelegacionOrigen_ = parseInt($('#ddlListadoDelegacionesOrigenDDL').val());
    var idDelegacionDestino_ = parseInt($('#ddlListadoDelegacionesDestinoDDL').val());
    if (idDelegacionOrigen_ != "0" && idDelegacionDestino_ != "0") {
        if (idDelegacionOrigen_ == idDelegacionDestino_) {
            ShowNotificacion("error", "Error", "La Delegación Destino no puede ser la misma que la Delegación Origen")
            $('#ddlListadoDelegacionesDestinoDDL').val('0')
        }
    }
}

function AgregarDatosPersona() {
    $('#modalAgregarDatosPersona').modal('show');
}
function AgregarDatosTransporte() {
    $('#modalAgregarDatosTransporte').modal('show');
}

function ValidaGuardarDatosPersona() {
    if (!validarTextoCamposObligatorios('validaCamposPersona')) {
        return;
    }      

    $('#modalAgregarDatosPersona').modal('hide');
}
function ValidaGuardarDatosTransporte() {
    $('#modalAgregarDatosTransporte').modal('hide');
}

function GeneraPDF() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')

    var fileName = "TransferenciaPlacas.pdf"
    var url = '../TransferenciaPlacas/TransferenciaPlacasGeneraPDF';
    $.ajax({
        type: "POST",
        url: url,
        data: { IdTransferenciaPlacas: IdTransferencia_ },

        success: function (r) {
            //Convert Base64 string to Byte Array.
            var bytes = Base64ToBytes(r);
            setTimeout(function () {
                location.reload(true);
            }, 300)
            //Convert Byte Array to BLOB.
            var blob = new Blob([bytes], { type: "text/csv" });

            //Check the Browser type and download the File.
            var isIE = false || !!document.documentMode;
            if (isIE) {
                window.navigator.msSaveBlob(blob, fileName);
            } else {
                var url = window.URL || window.webkitURL;
                link = url.createObjectURL(blob);
                var a = $("<a />");
                a.attr("download", fileName);
                a.attr("href", link);
                $("body").append(a);
                a[0].click();
                $("body").remove(a);
            }
        }
    });
}

function ConfirmarTransferencia() {
    var IdTransferencia_ = $.urlParam('IdTransferencia') == null ? 0 : $.urlParam('IdTransferencia')
    var EstatusTransferencia_ = parseInt("3");
    $.ajax({
        data: { "IdTransferencia": IdTransferencia_, "EstatusTransferencia": EstatusTransferencia_ },
        type: "POST",
        url: '../TransferenciaPlacas/CambioEstatusTransferencia',
        success: function (response) {
            if (response.ExecutionOK) {
                ShowNotificacion("success", "Mensaje Sistema", "Se confirmo la transferencia correctamente");
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

function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
};

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

});
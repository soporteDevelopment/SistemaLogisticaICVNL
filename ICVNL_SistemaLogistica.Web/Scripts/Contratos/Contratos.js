var listadoRangos = [];
var tipoPlacaSeleccionada = {};
var Detalle_ContratosDetailsVM = [];
var Detalle_ContratosArchivosVM = [];

function CargaInformacionContrato() {
    var IdContrato_ = $.urlParam('IdContrato') == null ? 0 : $.urlParam('IdContrato')
    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "Add") {
        ShowNotificacion("success", "Mensaje Sistema", "Se agregó correctamente el contrato", 0, 6);
    }
    else if (option_ == "Ed") {
        ShowNotificacion("success", "Mensaje Sistema", "Se modificó correctamente el contrato", 0, 6);
    }
    var url = '../Contratos/ContratoDetails';
    $.ajax({
        data: { IdContrato: IdContrato_ },
        type: "POST",
        url: url,
        success: function (data) {
            console.log(data);
            if (data.ExecutionOK) {
                var objContratoResponse = data.Data;
                $('#txtIdContrato').val(objContratoResponse.IdContrato);
                $('#txtNumeroContrato').val(objContratoResponse.NumeroContrato);
                Detalle_ContratosDetailsVM = objContratoResponse.Detalle_ContratosDetailsVM;
                Detalle_ContratosArchivosVM = objContratoResponse.Detalle_ContratosArchivosVM;            


                var urlDet = '../Contratos/GuardarDetalleTemp';
                $.ajax({
                    data: { ListContratosDetailsModel: Detalle_ContratosDetailsVM },
                    type: "POST",
                    url: urlDet,
                    success: function (partialView) {
                        $("#listadoPartialDetalleContrato").html(partialView);
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });

                var urlArc = '../Contratos/GuardarDetalleTemp';
                $.ajax({
                    data: { ListContratosDetailsModel: Detalle_ContratosDetailsVM },
                    type: "POST",
                    url: urlArc,
                    success: function (partialView) {
                        $("#listadoPartialDetalleContrato").html(partialView);
                    },
                    error: function (reponse) {
                        console.log(reponse);
                    }
                });


                var urlArc = '../Contratos/GuardarArchivosTemp';
                $.ajax({
                    data: { ListContratosArchivosModel: Detalle_ContratosArchivosVM },
                    type: "POST",
                    url: urlArc,
                    success: function (partialView) {
                        $("#listadoArchivosAdjuntos").html(partialView);
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
function CreateContrato() {
    if (!validarTextoCamposObligatorios('validaCampoContrato')) {
        return;
    }
    if (Detalle_ContratosDetailsVM.length == 0) {
        ShowNotificacion("error", "Error", "Agregue al menos una partida en el detalle del Contrato", 0, 6);
        return;
    }
    var url = '../Contratos/UpsertContrato';
    var objContrato = {
        IdContrato: $.urlParam('IdContrato') == null ? 0 : $.urlParam('IdContrato'),
        NumeroContrato: $('#txtNumeroContrato').val(),
        DetalleContrato: {},
        Detalle_ContratosDetailsVM: [],
        Detalle_ContratosArchivosVM: [],
    };
    objContrato.Detalle_ContratosArchivosVM = Detalle_ContratosArchivosVM;
    objContrato.Detalle_ContratosDetailsVM = Detalle_ContratosDetailsVM; 

    $.ajax({
        data: { viewModel: objContrato },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../Contratos/Details?IdContrato=" + data.Data.IdContrato + "&op=Add";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function ModifyContrato() {
    if (!validarTextoCamposObligatorios('validaCampoContrato')) {
        return;
    }
    var url = '../Contratos/UpsertContrato';
    var objContrato = {
        IdContrato: $.urlParam('IdContrato') == null ? 0 : $.urlParam('IdContrato'),
        NumeroContrato: $('#txtNumeroContrato').val(),
        DetalleContrato: {},
        Detalle_ContratosDetailsVM: [],
        Detalle_ContratosArchivosVM: [],
    };
    objContrato.Detalle_ContratosArchivosVM = Detalle_ContratosArchivosVM;
    objContrato.Detalle_ContratosDetailsVM = Detalle_ContratosDetailsVM;

    $.ajax({
        data: { viewModel: objContrato },
        type: "POST",
        url: url,
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../Contratos/Details?IdContrato=" + data.Data.IdContrato + "&op=Ed";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}
function GetDataTiposPlacas() {
    if ($('#DetalleContrato_IdTipoPlaca').val() != "0") {
        var tipoPlaca = parseInt($('#DetalleContrato_IdTipoPlaca').val());
        var url = '../Contratos/GetTipoPlaca';
        $.ajax({
            data: { idTipoPlaca: tipoPlaca },
            type: "POST",
            url: url,
            success: function (data) {
                console.log(data);
                tipoPlacaSeleccionada = data.Data;
                $('#DetalleContrato_CantidadPlacasCaja').val(tipoPlacaSeleccionada.CantidadPlacasCaja);
                $('#DetalleContrato_CantidadPlacas').val(tipoPlacaSeleccionada.CantidadPlacas);
                $('#DetalleContrato_EstructuraPlaca').val(tipoPlacaSeleccionada.MascaraPlaca);
                $('#DetalleContrato_OrdenPlaca').val(tipoPlacaSeleccionada.OrdenPlaca);
            },
            error: function (reponse) {
                console.log(reponse);
            }
        });

    }
}

function ValidaGuardarDetalle() {

    if (!validarCamposObligatorios('validaCampos')) {
        return;
    }

    var consecutivoDetalle = $('#txtNumeroFila').val();
    if (consecutivoDetalle == "") {
        objDetalle = {
            IdContratoDetalle: 0,
            Consecutivo: Detalle_ContratosDetailsVM.length + 1,
            IdContrato: 0,
            IdProveedor: parseInt($('#DetalleContrato_IdProveedor').val()),
            Proveedores:
            {
                NombreProveedor: $('#DetalleContrato_IdProveedor').find(":selected").text()
            },
            IdTipoPlaca: parseInt($('#DetalleContrato_IdTipoPlaca').val()),
            TipoPlacas:
            {
                TipoPlaca: $('#DetalleContrato_IdTipoPlaca').find(":selected").text(),
                MascaraPlaca: $('#DetalleContrato_EstructuraPlaca').val(),
                OrdenPlaca: $('#DetalleContrato_OrdenPlaca').val(),
                CantidadPlacas: parseInt($('#DetalleContrato_CantidadPlacas').val()),
                CantidadPlacasCaja: parseInt($('#DetalleContrato_CantidadPlacasCaja').val())
            },
            CantidadPlacas: parseInt($('#DetalleContrato_CantidadPlacas').val()),
            CantidadPlacasCaja: parseInt($('#DetalleContrato_CantidadPlacasCaja').val()),
            EstructuraPlaca: $('#DetalleContrato_EstructuraPlaca').val(),
            OrdenPlaca: $('#DetalleContrato_OrdenPlaca').val(),
            RangoInicial: $('#DetalleContrato_RangoInicial').val(),
            RangoFinal: $('#DetalleContrato_RangoFinal').val(),
            OficioSICT: $('#DetalleContrato_OficioSICT').val(),
            Detalle_ContratosDetailsRangosVM: []
        };

        Detalle_ContratosDetailsVM.push(objDetalle);
    }
    else {

        index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(consecutivoDetalle));
        Detalle_ContratosDetailsVM[index].IdContratoDetalle = 0;
        Detalle_ContratosDetailsVM[index].IdProveedor = parseInt($('#DetalleContrato_IdProveedor').val());
        Detalle_ContratosDetailsVM[index].Proveedores.NombreProveedor = $('#DetalleContrato_IdProveedor').find(":selected").text()
        Detalle_ContratosDetailsVM[index].IdTipoPlaca = parseInt($('#DetalleContrato_IdTipoPlaca').val());
        Detalle_ContratosDetailsVM[index].TipoPlacas.TipoPlaca = $('#DetalleContrato_IdTipoPlaca').find(":selected").text();
        Detalle_ContratosDetailsVM[index].TipoPlacas.MascaraPlaca = $('#DetalleContrato_EstructuraPlaca').val();
        Detalle_ContratosDetailsVM[index].TipoPlacas.OrdenPlaca = $('#DetalleContrato_OrdenPlaca').val();
        Detalle_ContratosDetailsVM[index].TipoPlacas.CantidadPlacas = $('#DetalleContrato_CantidadPlacas').val();
        Detalle_ContratosDetailsVM[index].TipoPlacas.CantidadPlacasCaja = $('#DetalleContrato_CantidadPlacasCaja').val();
        Detalle_ContratosDetailsVM[index].CantidadPlacas = parseInt($('#DetalleContrato_CantidadPlacas').val());
        Detalle_ContratosDetailsVM[index].CantidadPlacasCaja = parseInt($('#DetalleContrato_CantidadPlacasCaja').val());
        Detalle_ContratosDetailsVM[index].EstructuraPlaca = $('#DetalleContrato_EstructuraPlaca').val();
        Detalle_ContratosDetailsVM[index].OrdenPlaca = $('#DetalleContrato_OrdenPlaca').val();
        Detalle_ContratosDetailsVM[index].RangoInicial = $('#DetalleContrato_RangoInicial').val();
        Detalle_ContratosDetailsVM[index].RangoFinal = $('#DetalleContrato_RangoFinal').val();
        Detalle_ContratosDetailsVM[index].OficioSICT = $('#DetalleContrato_OficioSICT').val();
    }

    var url = '../Contratos/GuardarDetalleTemp';
    $.ajax({
        data: { ListContratosDetailsModel: Detalle_ContratosDetailsVM },
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoPartialDetalleContrato").html(partialView);
            ShowNotificacion("success", "Mensaje Sistema", "Se agrego correctamente el detalle", 0, 6);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });

    limpiarDetalleContrato();

    $('#modalAgregarDetalle').modal('hide');
    removeClassValidaObligatorio("validaCampos");

}

function ValidaAgregarDetalleRangos() {
    if (!validarCamposObligatorios('validaCamposRangos')) {
        return;
    }

    var consecutivoDetalle = $('#txtNumeroFila').val();
    var consecutivoDetalleRango = $('#DetalleContrato_DetalleRangos_Consecutivo').val() == undefined ? "" : $('#DetalleContrato_DetalleRangos_Consecutivo').val();

    index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(consecutivoDetalle));

    if (consecutivoDetalleRango == "") {
        var objRangoDetalle = {
            IdContratoDetalleRangos: 0,
            Consecutivo: Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.length + 1,
            IdContratoDetalle: 0,
            RangoInicial: $('#DetalleContrato_DetalleRangos_RangoInicial').val(),
            RangoFinal: $('#DetalleContrato_DetalleRangos_RangoFinal').val(),
            CantidadSerie: $('#DetalleContrato_DetalleRangos_CantidadSerie').val()
        };

        Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.push(objRangoDetalle);

    }
    else { 
        var indexRango = Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.findIndex(x => x.Consecutivo === parseInt(consecutivoDetalle))
        Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM[indexRango].RangoInicial = $('#DetalleContrato_DetalleRangos_RangoInicial').val();
        Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM[indexRango].RangoFinal = $('#DetalleContrato_DetalleRangos_RangoFinal').val();
        Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM[indexRango].CantidadSerie = $('#DetalleContrato_DetalleRangos_CantidadSerie').val();
    }
    $('#DetalleContrato_DetalleRangos_Consecutivo').val('');

    var url = '../Contratos/GuardarRangoDetalleTemp';
    $.ajax({
        data: { ListContratosDetailsRangosModel: Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM },
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoRangosDetalle").html(partialView); 
            limpiarDetalleRangoContrato();
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });

    $('#modalAgregarDetalle').modal('hide');
    removeClassValidaObligatorio("validaCamposRangos");
    ShowNotificacion("success", "Mensaje Sistema", "Se agrego correctamente el detalle", 0, 6); 
}

function EditarDetalle(Consecutivo) {
    $('#tituloModalAgregar').text('Editar Detalle Contrato');
    $('#txtNumeroFila').val(Consecutivo);
    index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));
    var objContrato = Detalle_ContratosDetailsVM[index];

    $('#DetalleContrato_IdProveedor').val(objContrato.IdProveedor);
    $('#DetalleContrato_IdTipoPlaca').val(objContrato.IdTipoPlaca); 
    $('#DetalleContrato_EstructuraPlaca').val(objContrato.TipoPlacas.MascaraPlaca); 
    $('#DetalleContrato_OrdenPlaca').val(objContrato.TipoPlacas.OrdenPlaca); 
    $('#DetalleContrato_CantidadPlacas').val(objContrato.CantidadPlacas); 
    $('#DetalleContrato_CantidadPlacasCaja').val(objContrato.CantidadPlacasCaja);  
    $('#DetalleContrato_RangoInicial').val(objContrato.RangoInicial); 
    $('#DetalleContrato_RangoFinal').val(objContrato.RangoFinal); 
    $('#DetalleContrato_OficioSICT').val(objContrato.OficioSICT); 
    
    $('#modalAgregarDetalle').modal('show');
}

function EditarDetalleRango(Consecutivo) {
    $('#DetalleContrato_DetalleRangos_Consecutivo').val(Consecutivo)
    var consecutivoDetalle = $('#txtNumeroFila').val();

    var index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(consecutivoDetalle));
    var objContrato = Detalle_ContratosDetailsVM[index];

    var indexRango = objContrato.Detalle_ContratosDetailsRangosVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));
    var objRango = objContrato.Detalle_ContratosDetailsRangosVM[indexRango];

    $('#DetalleContrato_DetalleRangos_RangoInicial').val(objRango.RangoInicial);
    $('#DetalleContrato_DetalleRangos_RangoFinal').val(objRango.RangoFinal);
    $('#DetalleContrato_DetalleRangos_CantidadSerie').val(objRango.CantidadSerie); 
}

function limpiarDetalleContrato() {
    $('#DetalleContrato_IdProveedor').val('0');
    $('#DetalleContrato_IdTipoPlaca').val('0');
    $('#DetalleContrato_CantidadPlacas').val('0');
    $('#DetalleContrato_CantidadPlacasCaja').val('0');
    $('#DetalleContrato_EstructuraPlaca').val('');
    $('#DetalleContrato_OrdenPlaca').val('');
    $('#DetalleContrato_RangoInicial').val('');
    $('#DetalleContrato_RangoFinal').val('');
    $('#DetalleContrato_OficioSICT').val('');
}

function limpiarDetalleRangoContrato() {
    $('#DetalleContrato_DetalleRangos_RangoInicial').val('');
    $('#DetalleContrato_DetalleRangos_RangoFinal').val('');
    $('#DetalleContrato_DetalleRangos_CantidadSerie').val('');
}


function ConteoRangoPlacas() {

    if ($('#DetalleContrato_DetalleRangos_RangoInicial').val() == "") {
        return;
    }
    if ($('#DetalleContrato_DetalleRangos_RangoFinal').val() == "") {
        return;
    }
    var consecutivoDetalle = $('#txtNumeroFila').val(); 
    index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(consecutivoDetalle));

    var GenerarPlacasRangos_ = {
        IdTipoPlaca: Detalle_ContratosDetailsVM[index].IdTipoPlaca,
        TipoPlaca: {
            IdTipoPlaca: Detalle_ContratosDetailsVM[index].IdTipoPlaca,
            MascaraPlaca: Detalle_ContratosDetailsVM[index].EstructuraPlaca,
            OrdenPlaca: Detalle_ContratosDetailsVM[index].OrdenPlaca,
        },
        RangoInicial: $('#DetalleContrato_DetalleRangos_RangoInicial').val(),
        RangoFinal: $('#DetalleContrato_DetalleRangos_RangoFinal').val()
    }

    var url = '../Contratos/GetCalculoPlacas';
    $.ajax({
        data: { PlacasRangos: GenerarPlacasRangos_ },
        type: "POST",
        url: url,
        success: function (response) {
            if (response.ExecutionOK) {
                $('#DetalleContrato_DetalleRangos_CantidadSerie').val(response.Data);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}
function ValidaRangosPlacas(Consecutivo) {

    var index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));  


    var url = '../Contratos/ValidaRangosPlacas';
    $.ajax({
        data: { DetalleContrato: Detalle_ContratosDetailsVM[index] },
        type: "POST",
        url: url,
        success: function (response) {
            if (response.ExecutionOK) {
                ShowNotificacion("success", "Mensaje del Sistema", "Se han validado correctamente los rangos", 0, 10)
            }
            else {
                ShowNotificacion("error", "Error", response.Message, 0, 10)
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}
function AgregarRangos(Consecutivo) {
    $('#modalAgregarDetalleRangos').modal('show');
    $('#txtNumeroFila').val(Consecutivo);

    var index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));
    var objContrato = Detalle_ContratosDetailsVM[index];
    
    var url = '../Contratos/GuardarRangoDetalleTemp';
    $.ajax({
        data: { ListContratosDetailsRangosModel: objContrato.Detalle_ContratosDetailsRangosVM },
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoRangosDetalle").html(partialView);
            limpiarDetalleRangoContrato();
        },
        error: function (reponse) {
            console.log(reponse);
        }
    }); 
}

function VerArchivosCargados() {
    $('#modalArchivos').modal('show');
}

function AgregarDetalleContrato() {
    $('#tituloModalAgregar').text('Agregar Detalle Contrato');
    $('#modalAgregarDetalle').modal('show');
    limpiarDetalleContrato();
}

function EliminarConsecutivoDetalle(Consecutivo) {
    var index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));
    if (Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.length > 0) {
        ShowNotificacion("error", "Error", "Existen registros de Rangos de Placas por Serie del Tipo de Placa Seleccionado, elimine los rangos relacionados al detalle", 0, 6);
        return;
    }
    Detalle_ContratosDetailsVM.splice(index, 1);
    for (var i = 0; i < Detalle_ContratosDetailsVM.length; i++) {
        Detalle_ContratosDetailsVM[i].Consecutivo = i + 1;
    }

    var url = '../Contratos/GuardarDetalleTemp';
    $.ajax({
        data: { ListContratosDetailsModel: Detalle_ContratosDetailsVM },
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoPartialDetalleContrato").html(partialView);
            ShowNotificacion("success", "Mensaje Sistema", "Se eliminó el detalle correctamente", 0, 6);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function EliminarRango(Consecutivo) { 
    var consecutivoRango = $('#txtNumeroFila').val();

    var index = Detalle_ContratosDetailsVM.findIndex(x => x.Consecutivo === parseInt(consecutivoRango));
    var objContrato = Detalle_ContratosDetailsVM[index];
    var indexRango = Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo));

    Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.splice(indexRango, 1);
    for (var i = 0; i < Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM.length; i++) {
        Detalle_ContratosDetailsVM[index].Detalle_ContratosDetailsRangosVM[i].Consecutivo = i + 1;
    }

    var url = '../Contratos/GuardarRangoDetalleTemp';
    $.ajax({
        data: { ListContratosDetailsRangosModel: objContrato.Detalle_ContratosDetailsRangosVM },
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoRangosDetalle").html(partialView); 
            ShowNotificacion("success", "Mensaje Sistema", "Se eliminó correctamente el rango", 0, 6);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    }); 



}

function EliminarArchivo(index_) {
    var indexRow = Detalle_ContratosArchivosVM.findIndex(x => x.Consecutivo === parseInt(index_));
    Detalle_ContratosArchivosVM.splice(indexRow, 1);

    var url = '../Contratos/DeleteArchivo';
    $.ajax({
        data: { ListContratosArchivosModel: Detalle_ContratosArchivosVM},
        type: "POST",
        url: url,
        success: function (partialView) {
            $("#listadoArchivosAdjuntos").html(partialView);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}
function DescargarArchivo(index_) {
    var index = Detalle_ContratosArchivosVM.findIndex(x => x.Consecutivo === parseInt(index_));

    var objArchivo = Detalle_ContratosArchivosVM[index]; 
    var sampleArr = base64ToArrayBuffer(objArchivo.ArchivoBase64);
    saveByteArray(objArchivo.NombreArchivo, sampleArr);
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
ColocaNombre = function () {
    var input = document.getElementById('filArchivo');
    var output = document.getElementById('fileList');

    output.innerHTML = '<ul>';
    for (var i = 0; i < input.files.length; ++i) {
        output.innerHTML += '<li>' + input.files.item(i).name + '</li>';
    }
    output.innerHTML += '</ul>';
}

function clearFileInput(id) {
    var oldInput = document.getElementById(id);

    var newInput = document.createElement("input");

    newInput.type = "file";
    newInput.id = oldInput.id;
    newInput.name = oldInput.name;
    newInput.className = oldInput.className;
    newInput.style.cssText = oldInput.style.cssText;
    // TODO: copy any other relevant attributes 

    oldInput.parentNode.replaceChild(newInput, oldInput);
}

$(document).ready(function () {
    $('#txtNumeroFila').val('');
    $('#AdjuntarArchivos').click(function () { 
        if (window.FormData !== undefined) { 
            var fileUpload = $("#filArchivo").get(0);
            var files = fileUpload.files;
            var fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }

            $.ajax({
                url: '../Contratos/UploadFiles',
                type: "POST",
                contentType: false, 
                processData: false, 
                data: fileData,
                success: function (result) { 
                    for (var i = 0; i < result.Data.length; i++) {
                        var objArchivoCargado = {
                            Consecutivo: Detalle_ContratosArchivosVM.length + 1,
                            IdContrato: 0,
                            ArchivoBase64: result.Data[i].ArchivoCargado,
                            NombreArchivo: result.Data[i].NombreArchivo 
                        };
                        Detalle_ContratosArchivosVM.push(objArchivoCargado);
                    }
                    var url = '../Contratos/GuardarArchivosTemp';
                    $.ajax({
                        data: { ListContratosArchivosModel: Detalle_ContratosArchivosVM },
                        type: "POST",
                        url: url,
                        success: function (partialView) {
                            $("#listadoArchivosAdjuntos").html(partialView);
                        },
                        error: function (reponse) {
                            console.log(reponse);
                        }
                    });
                    var output = document.getElementById('fileList'); 
                    output.innerHTML = '';
                    clearFileInput("filArchivo");
                    ShowNotificacion("success", "Mensaje del Sistema", "Se han cargado los archivos seleccionado", 0, 0);
                },
                error: function (err) { 
                    ShowNotificacion("error", "Error", err.Message, 0, 0);
                }
            });
        } else {
            alert("FormData is not supported.");
        }
    }); 

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

    $('#tablaRegistrosArchivos').DataTable({
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

    $('#tablaRegistrosRangos').DataTable({
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
function CargArchivoAdjunto() {
    var ListArchivosValidacionVM = [];
    var idValidacionEvento_ = $('#txtIdValidacionEventoArchivo').val();
    var url_ = "../SolicitudesPlacasRecepcion/UploadFiles";
    console.log("Entra al upload")
    if (window.FormData !== undefined) {
        var fileUpload = $("#filArchivo").get(0);
        var files = fileUpload.files;
        var fileData = new FormData();
        for (var i = 0; i < files.length; i++) {
            fileData.append(files[i].name, files[i]);
        }

        $.ajax({
            url: url_,
            type: "POST",
            contentType: false,
            processData: false,
            data: fileData,
            success: function (result) {
                for (var i = 0; i < result.Data.length; i++) {
                    var objArchivoCargado = {
                        Consecutivo: ListArchivosValidacionVM.length + 1,
                        IdContrato: 0,
                        ArchivoBase64: result.Data[i].ArchivoCargado,
                        NombreArchivo: result.Data[i].NombreArchivo
                    };
                    ListArchivosValidacionVM.push(objArchivoCargado);
                }

                var urlGuaraArchivo_ = "../SolicitudesPlacasRecepcion/GuardarArchivos";

                $.ajax({
                    data: { "ListArchivosValidacionModel": ListArchivosValidacionVM, "IdValidacionEvento": idValidacionEvento_ },
                    type: "POST",
                    url: urlGuaraArchivo_,
                    success: function (response) {
                        $('#modalArchivos').modal('hide');
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
};


function GuardarObservacion() {
    var idValidacionEvento_ = $('#txtIdValidacionEventoObservaciones').val();
    var observacion_ = {
        IdObservacion: 0,
        IdEventoRecepcion: 0,
        IdRecepcionValidaciones: idValidacionEvento_,
        Observacion: $('#txtObservaciones').val(),
        Entidad: 0
    };
    var url = "../SolicitudesPlacasRecepcion/GuardarObservacion";
    $.ajax({
        data: { observacion: observacion_ },
        type: "POST",
        url: url,
        success: function (res) {
            if (res.ExecutionOK) {
                ShowNotificacion("success", "Mensaje del Sistema", "Se ha guardado correctamente la observación", 0, 0);
                $('#modalObservaciones').modal('hide');
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function EliminarObservacion(IdObservacion_) { 
    var url = "../SolicitudesPlacasRecepcion/EliminarObservacion";
    $.ajax({
        data: { IdObservacion: IdObservacion_ },
        type: "POST",
        url: url,
        success: function (res) {
            if (res.ExecutionOK) {
                ShowNotificacion("success", "Mensaje del Sistema", "Se ha eliminado correctamente la observación", 0, 0);
                $('#modalObservaciones').modal('hide');
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function DescargarArchivo(IdArchivo_) { 
    var url = "../SolicitudesPlacasRecepcion/DescargarArchivo";
    $.ajax({
        data: { IdArchivo: IdArchivo_ },
        type: "POST",
        url: url,
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

function EliminarArchivo(IdArchivo_) {
    var url = "../SolicitudesPlacasRecepcion/EliminarArchivo";
    $.ajax({
        data: { IdArchivo: IdArchivo_ },
        type: "POST",
        url: url,
        success: function (partialView) {
            $('#modalArchivos').modal('hide');
        },
        error: function (res) {
            console.log(reponse);
        }
    });
}

ColocaNombre = function () {
    var input = document.getElementById('filArchivo');
    var output = document.getElementById('fileList');

    output.innerHTML = '<ul>';
    for (var i = 0; i < input.files.length; ++i) {
        output.innerHTML += '<li>' + input.files.item(i).name + '</li>';
    }
    output.innerHTML += '</ul>';
}
function AgregarObservacion(IdRecepcionValidaciones_) {
    var url = "../SolicitudesPlacasRecepcion/ObservacionesCargadosValidacion"; 
    $.ajax({
        data: { id: IdRecepcionValidaciones_ },
        type: "POST",
        url: url,
        success: function (partialView) {
            $('#divObservaciones').html(partialView);
            $('#modalObservaciones').modal('show');
            $('#txtIdValidacionEventoObservaciones').val(IdRecepcionValidaciones_);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}
function SubirArchivos(IdRecepcionValidaciones_) {
    var url = "../SolicitudesPlacasRecepcion/ArchivosCargadosValidacion";  
    $.ajax({
        data: { id: IdRecepcionValidaciones_ },
        type: "POST",
        url: url,
        success: function (partialView) {
            $('#divArchivosAdjuntos').html(partialView);
            $('#modalArchivos').modal('show');
            $('#txtIdValidacionEventoArchivo').val(IdRecepcionValidaciones_);
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

$.urlParam = function (name) {
    var results = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
    if (results == null) {
        return null;
    }
    return decodeURI(results[1]) || 0;
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
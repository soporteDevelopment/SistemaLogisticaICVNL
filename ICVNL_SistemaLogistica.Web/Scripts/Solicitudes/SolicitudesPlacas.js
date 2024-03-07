var objSolicitud = {
    IdSolicitud: 0,
    FechaSolicitud: '',
    FechaEntrega: '',
    FolioSolicitud: '',
    IdProveedor: 0,
    IdContrato: 0,
    IdOrdenCompra: 0,
    Detalle_SolicitudesPlacasDetailsVM : []
}

var OrdenesCompra_Detalle = {};
function CargaInformacionSolicitud() {
    var IdSolicitud_ = $.urlParam('IdSolicitud') == null ? 0 : $.urlParam('IdSolicitud')
    var option_ = $.urlParam('op') == null ? "" : $.urlParam('op')
    if (option_ == "Add") {
        ShowNotificacion("success", "Mensaje Sistema", "Se agregó correctamente el Solicitud de Placas", 0, 6);
    }
    else if (option_ == "Ed") {
        ShowNotificacion("success", "Mensaje Sistema", "Se modificó correctamente el Solicitud de Placas", 0, 6);
    }
    var url = '../SolicitudesPlacas/SolicitudDetails';
    $.ajax({
        data: { IdSolicitud: IdSolicitud_ },
        type: "POST",
        url: url,
        success: function (data) {
            console.log(data);
            if (data.ExecutionOK) {
                objSolicitud = data.Data;
                $('#txtIdSolicitud').val(objSolicitud.IdSolicitud);
                $('#txtFolioSolicitud').val(objSolicitud.FolioSolicitud);
                $('#txtFechaSolicitud').val(objSolicitud.FechaSolicitudStr);
                $('#txtFechaEntrega').val(objSolicitud.FechaEntregaStr); 
                $('#ddlListadoProveedoresDDL').val(objSolicitud.IdProveedor);
                $('#ddlListadoContratosDDL').val(objSolicitud.IdContrato);
                $('#ddlListadoOrdenesCompraDDL').val(objSolicitud.IdOrdenCompra)  
                $.ajax({
                    data: { ListSolicitudesPlacasDetailsModel: objSolicitud.Detalle_SolicitudesPlacasDetailsVM },
                    type: "POST",
                    url: '../SolicitudesPlacas/GuardarDetalleTemp',
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

function CreateSolicitud() {
    if (!validarCamposObligatorios('validaCamposSolicitudPlacas')) {
        if (!validarTextoCamposObligatorios('validaCamposSolicitudPlacasText')) {
            return;
        }
        return;
    }
    if (objSolicitud.Detalle_SolicitudesPlacasDetailsVM.length == 0) {
        ShowNotificacion("error", "Error", "Agregue al menos una partida en el detalle de la Solicitud de Placas", 0, 6);
        return;
    }
    
    objSolicitud.IdSolicitud = $.urlParam('IdSolicitud') == null ? 0 : $.urlParam('IdSolicitud');
    objSolicitud.FechaSolicitud = $("#txtFechaSolicitud").val();
    objSolicitud.FechaEntrega = $("#txtFechaEntrega").val();
    objSolicitud.FolioSolicitud = $("#txtFolioSolicitud").val();
    objSolicitud.IdProveedor = parseInt($("#ddlListadoProveedoresDDL").val());
    objSolicitud.IdContrato = parseInt($("#ddlListadoContratosDDL").val());
    objSolicitud.IdOrdenCompra = parseInt($("#ddlListadoOrdenesCompraDDL").val());

    $.ajax({
        data: { viewModel: objSolicitud },
        type: "POST",
        url: "../SolicitudesPlacas/UpsertSolicitud",
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../SolicitudesPlacas/Details?IdSolicitud=" + data.Data.IdSolicitud + "&op=Add";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }            
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function UpdateSolicitud() {
    if (!validarCamposObligatorios('validaCamposSolicitudPlacas')) {
        if (!validarTextoCamposObligatorios('validaCamposSolicitudPlacasText')) {
            return;
        }
        return;
    }
    if (objSolicitud.Detalle_SolicitudesPlacasDetailsVM.length == 0) {
        ShowNotificacion("error", "Error", "Agregue al menos una partida en el detalle de la Solicitud de Placas", 0, 6);
        return;
    }

    objSolicitud.IdSolicitud = $.urlParam('IdSolicitud') == null ? 0 : $.urlParam('IdSolicitud');
    objSolicitud.FechaSolicitud = $("#txtFechaSolicitud").val();
    objSolicitud.FechaEntrega = $("#txtFechaEntrega").val();
    objSolicitud.FolioSolicitud = $("#txtFolioSolicitud").val();
    objSolicitud.IdProveedor = parseInt($("#ddlListadoProveedoresDDL").val());
    objSolicitud.IdContrato = parseInt($("#ddlListadoContratosDDL").val());
    objSolicitud.IdOrdenCompra = parseInt($("#ddlListadoOrdenesCompraDDL").val());

    $.ajax({
        data: { viewModel: objSolicitud },
        type: "POST",
        url: "../SolicitudesPlacas/UpsertSolicitud",
        success: function (data) {
            if (data.ExecutionOK) {
                window.location.href = "../SolicitudesPlacas/Details?IdSolicitud=" + data.Data.IdSolicitud + "&op=Ed";
            } else {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
            }
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function AgregarDetalleSolicitud() {
    $('#tituloModalAgregar').text('Agregar Detalle Solicitud de Placas');
    $('#modalAgregarDetalle').modal('show');
    $('#txtDetalleSolicitud_RangoPlacaInicial').val('');
    $('#ddlDetalleSolicitud_IdDelegacionBanco').val('0');
    $('#ddlDetalleSolicitud_IdTipoPlaca').val('0');
    $('#txtDetalleSolicitud_RangoPlacaFinal').val('');
    $('#txtDetalleSolicitud_CantidadPlacas').val('');
    $('#txtDetalleSolicitud_IdDetalleOrdenCompra').val('');
    $('#txtConsecutivo').val('');
    CargaComboDetalleTipoPlacas();
    removeClassValidaObligatorio("validaCampos");
    removeClassValidaObligatorio("validaCamposDet");
}

function ValidaGuardarDetalle() {
    var consecutivoSeleccionado = $('#txtConsecutivo').val();
    if (!validarCamposObligatorios('validaCampos')) {
        if (!validarTextoCamposObligatorios('validaCamposDet')) {
            return;
        }
        return;
    }
    var cantidadOC = parseInt(OrdenesCompra_Detalle.CantidadPiezas);
    var cantidadDetSol = parseInt($('#txtDetalleSolicitud_CantidadPlacas').val());
    if (cantidadDetSol > cantidadOC) {
        ShowNotificacion("error", "Error", "La cantidad solicitada excede a la cantidad establecida en la Orden de Compra", 0, 6);
        return;
    }
   

    if (consecutivoSeleccionado == "") {
        var detalle = {
            Consecutivo: objSolicitud.Detalle_SolicitudesPlacasDetailsVM.length + 1,
            IdSolicitudDetalle: 0,
            IdSolicitud: 0,
            IdDelegacionBanco: $('#ddlDetalleSolicitud_IdDelegacionBanco').val(),
            DelegacionesBancos: {
                NombreDelegacionBanco: $('#ddlDetalleSolicitud_IdDelegacionBanco').find(":selected").text()
            },
            IdTipoPlaca: $('#ddlDetalleSolicitud_IdTipoPlaca').val(),
            TiposPlacas: {
                TipoPlaca: $('#ddlDetalleSolicitud_IdTipoPlaca').find(":selected").text()
            },
            RangoPlacaInicial: $('#txtDetalleSolicitud_RangoPlacaInicial').val(),
            RangoPlacaFinal: $('#txtDetalleSolicitud_RangoPlacaFinal').val(),
            CantidadPlacas: $('#txtDetalleSolicitud_CantidadPlacas').val(),
            IdDetalleOrdenCompra: $('#txtDetalleSolicitud_IdDetalleOrdenCompra').val(),
            Entidad: 0,
        }

        objSolicitud.Detalle_SolicitudesPlacasDetailsVM.push(detalle);
    }
    else {
        var index = objSolicitud.Detalle_SolicitudesPlacasDetailsVM.findIndex(x => x.Consecutivo === parseInt(consecutivoSeleccionado))
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].IdSolicitudDetalle = 0;
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].IdSolicitud = 0;
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].IdDelegacionBanco = $('#ddlDetalleSolicitud_IdDelegacionBanco').val();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].DelegacionesBancos.NombreDelegacionBanco = $('#ddlDetalleSolicitud_IdDelegacionBanco').find(":selected").text();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].IdTipoPlaca = $('#ddlDetalleSolicitud_IdTipoPlaca').val();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].TiposPlacas.TipoPlaca = $('#ddlDetalleSolicitud_IdTipoPlaca').find(":selected").text();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].RangoPlacaInicial = $('#txtDetalleSolicitud_RangoPlacaInicial').val();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].RangoPlacaFinal = $('#txtDetalleSolicitud_RangoPlacaFinal').val();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].CantidadPlacas = $('#txtDetalleSolicitud_CantidadPlacas').val();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].IdDetalleOrdenCompra = $('#txtDetalleSolicitud_IdDetalleOrdenCompra').val();
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index].Entidad = 0;
    }
     
    $.ajax({
        data: { ListSolicitudesPlacasDetailsModel: objSolicitud.Detalle_SolicitudesPlacasDetailsVM },
        type: "POST",
        url: '../SolicitudesPlacas/GuardarDetalleTemp',
        success: function (partialView) {
            $("#listadoPartialDetalleSolicitud").html(partialView);
            ShowNotificacion("success", "Mensaje Sistema", "Se agrego correctamente el detalle", 0, 6);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });

    $('#modalAgregarDetalle').modal('hide');
    removeClassValidaObligatorio("validaCampos");
}

function EditarDetalle(Consecutivo) {
    $('#tituloModalAgregar').text('Editar Detalle Solicitud de Placas');
    $('#txtConsecutivo').val(Consecutivo);
    var index = objSolicitud.Detalle_SolicitudesPlacasDetailsVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo))
    var objDetalle = objSolicitud.Detalle_SolicitudesPlacasDetailsVM[index];
    CargaComboDetalleTipoPlacas();

    setTimeout(function () {
        $('#ddlDetalleSolicitud_IdDelegacionBanco').val(objDetalle.IdDelegacionBanco);
        $('#ddlDetalleSolicitud_IdTipoPlaca').val(objDetalle.IdTipoPlaca);
        $('#txtDetalleSolicitud_RangoPlacaInicial').val(objDetalle.RangoPlacaInicial);
        $('#txtDetalleSolicitud_RangoPlacaFinal').val(objDetalle.RangoPlacaFinal);
        $('#txtDetalleSolicitud_CantidadPlacas').val(objDetalle.CantidadPlacas);
        $('#txtDetalleSolicitud_IdDetalleOrdenCompra').val(objDetalle.IdDetalleOrdenCompra);
    }, 250);


    $('#modalAgregarDetalle').modal('show');
}

function EliminarConsecutivoDetalle(Consecutivo) {
    var index = objSolicitud.Detalle_SolicitudesPlacasDetailsVM.findIndex(x => x.Consecutivo === parseInt(Consecutivo))
    objSolicitud.Detalle_SolicitudesPlacasDetailsVM.splice(index, 1);
    for (var i = 0; i < objSolicitud.Detalle_SolicitudesPlacasDetailsVM.length; i++) {
        objSolicitud.Detalle_SolicitudesPlacasDetailsVM[i].Consecutivo = i + 1;
    }

    $.ajax({
        data: { ListSolicitudesPlacasDetailsModel: objSolicitud.Detalle_SolicitudesPlacasDetailsVM },
        type: "POST",
        url: '../SolicitudesPlacas/GuardarDetalleTemp',
        success: function (partialView) {
            $("#listadoPartialDetalleSolicitud").html(partialView);
            ShowNotificacion("success", "Mensaje Sistema", "Se elimino correctamente el detalle", 0, 6);
        },
        error: function (reponse) {
            console.log(reponse);
        }
    });
}

function GetDataTiposPlacas() {
    if ($('#ddlDetalleSolicitud_IdTipoPlaca').val() != "0") {
        var tipoPlaca = parseInt($('#ddlDetalleSolicitud_IdTipoPlaca').val());
        var IdOrdenCompra_ = parseInt($('#ddlListadoOrdenesCompraDDL').val());
        var IdContrato_ = parseInt($('#ddlListadoContratosDDL').val());
        var IdProveedor_ = parseInt($('#ddlListadoProveedoresDDL').val());
        var url = '../SolicitudesPlacas/GetDatosTipoPlaca';
        $.ajax({
            data: { IdOrdenCompra: IdOrdenCompra_, idTipoPlaca: tipoPlaca, IdContrato: IdContrato_, IdProveedor: IdProveedor_  },
            type: "POST",
            url: url,
            async: false,
            success: function (data) {
                console.log(data);
                if (data.ExecutionOK) {
                    tipoPlacaSeleccionada = data.Data;
                    console.log(tipoPlacaSeleccionada)
                    $('#txtDetalleSolicitud_RangoPlacaInicial').val(tipoPlacaSeleccionada.RangoInicial);
                    $('#txtDetalleSolicitud_IdDetalleOrdenCompra').val(tipoPlacaSeleccionada.IdDetalleOrdenCompra);
                }
                else {
                    ShowNotificacion("error", "Error", data.Message, 0, 6);
                    $('#txtDetalleSolicitud_RangoPlacaInicial').val('');
                    $('#txtDetalleSolicitud_CantidadPlacas').val('');
                    $('#txtDetalleSolicitud_IdDetalleOrdenCompra').val('');
                }
            },
            error: function (reponse) {
                console.log(reponse);
            }
        });

        $.ajax({
            data: { "IdOrdenCompra": IdOrdenCompra_, "IdTipoPlaca": tipoPlaca },
            type: "POST",
            url: "../SolicitudesPlacas/ObtenOrdenCompraDetalle",
            async: false,
            success: function (data) {
                if (data.ExecutionOK) {
                    OrdenesCompra_Detalle = data.Data;
                }
            },
            error: function (reponse) {
                ShowNotificacion("error", "Error", "Error al obtener los tipos de placa de la Orden de Compra seleccionada", "4", "0")
                console.log(reponse);
            }
        });
    }
}


function ValidaCantidadesPlacas() {
    if ($('#ddlDetalleSolicitud_IdTipoPlaca').val() != "0") {
        var tipoPlaca = parseInt($('#ddlDetalleSolicitud_IdTipoPlaca').val());
        var IdOrdenCompra_ = parseInt($('#ddlListadoOrdenesCompraDDL').val());
        var IdContrato_ = parseInt($('#ddlListadoContratosDDL').val());
        var IdProveedor_ = parseInt($('#ddlListadoProveedoresDDL').val());
        var url = '../SolicitudesPlacas/GetDatosTipoPlaca';
        $.ajax({
            data: { IdOrdenCompra: IdOrdenCompra_, idTipoPlaca: tipoPlaca, IdContrato: IdContrato_, IdProveedor: IdProveedor_ },
            type: "POST",
            url: url,
            async: false,
            success: function (data) {
                console.log(data);
                if (data.ExecutionOK) {
                    tipoPlacaSeleccionada = data.Data;
                    var GenerarPlacasRangos_ = {
                        IdTipoPlaca: tipoPlaca,
                        TipoPlaca: {
                            IdTipoPlaca: tipoPlacaSeleccionada.IdTipoPlaca,
                            MascaraPlaca: tipoPlacaSeleccionada.MascaraPlaca,
                            OrdenPlaca: tipoPlacaSeleccionada.OrdenPlaca,
                        },
                        RangoInicial: $('#txtDetalleSolicitud_RangoPlacaInicial').val(),
                        RangoFinal: $('#txtDetalleSolicitud_RangoPlacaFinal').val()
                    }

                    var url2 = '../SolicitudesPlacas/GetCalculoPlacas';
                    $.ajax({
                        data: { PlacasRangos: GenerarPlacasRangos_ },
                        type: "POST",
                        async: false,
                        url: url2,
                        success: function (response) {
                            if (response.ExecutionOK) {
                                if (response.Data > tipoPlacaSeleccionada.CantidadPlacas) {
                                    $('#txtDetalleSolicitud_CantidadPlacas').val('');
                                    ShowNotificacion("error", "Error", "La cantidad de placas calculada es mayor que la cantidad establecida en el contrato del tipo de placa y proveedor seleccionado", 0, 10)
                                }
                                else {
                                    $('#txtDetalleSolicitud_CantidadPlacas').val(response.Data);
                                }
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
}

function CargaCombosAnidados() {

    var idProveedor_ = $("#ddlListadoProveedoresDDL").val();
    if (idProveedor_ != "0") {
        //Llenamos el combo de Contratos
        $.ajax({
            data: { "IdProveedor": idProveedor_ },
            type: "POST",
            url: "../SolicitudesPlacas/ObtenContratosProveedor",
            success: function (data) {
                // VACIAMOS EL DropDownList
                $("#ddlListadoContratosDDL").empty();
                // CONSTRUIMOS EL DropDownList A PARTIR DEL RESULTADO Json (data)
                $.each(data, function (index, row) {
                    $("#ddlListadoContratosDDL").append("<option value='" + row.Valor + "'>" + row.Texto + "</option>")
                });
            },
            error: function (reponse) {
                ShowNotificacion("error", "Error", "Error al obtener los contratos relacionados al proveedor", "4", "0")
                console.log(reponse);
            }
        });

        //Llenamos el combo de Ordenes de Compra

        $.ajax({
            data: { "IdProveedor": idProveedor_ },
            type: "POST",
            url: "../SolicitudesPlacas/ObtenOrdenesCompraProveedor",
            success: function (data) {
                // VACIAMOS EL DropDownList
                $("#ddlListadoOrdenesCompraDDL").empty();
                // CONSTRUIMOS EL DropDownList A PARTIR DEL RESULTADO Json (data)
                $.each(data, function (index, row) {
                    $("#ddlListadoOrdenesCompraDDL").append("<option value='" + row.Valor + "'>" + row.Texto + "</option>")
                });
            },
            error: function (reponse) {
                ShowNotificacion("error", "Error", "Error al obtener las Ordenes de Compra relacionados al proveedor", "4", "0")
                console.log(reponse);
            }
        });
    }
    else {
        $("#ddlListadoOrdenesCompraDDL").empty();
        $("#ddlListadoOrdenesCompraDDL").append("<option value='0'>Seleccione</option>")
        $("#ddlListadoContratosDDL").empty();
        $("#ddlListadoContratosDDL").append("<option value='0'>Seleccione</option>")
        $("#ddlDetalleSolicitud_IdTipoPlaca").empty();
        $("#ddlDetalleSolicitud_IdTipoPlaca").append("<option value='0'>Seleccione</option>")
    }
}

function CargaComboDetalleTipoPlacas() {
    var idContrato_ = $("#ddlListadoContratosDDL").val();
    var idOrdenCompra_ = $("#ddlListadoOrdenesCompraDDL").val();
    if (idOrdenCompra_ != "0" && idContrato_ != "0") {
        //Llenamos el combo de los tipos de placas de una Ordenes de Compra seleccionada
        $.ajax({
            data: { "IdOrdenCompra": idOrdenCompra_, "IdContrato": idContrato_ },
            type: "POST",
            url: "../SolicitudesPlacas/ObtenTiposPlacasOrdenesCompra",
            async: false,
            success: function (data) {
                // VACIAMOS EL DropDownList
                $("#ddlDetalleSolicitud_IdTipoPlaca").empty();
                // CONSTRUIMOS EL DropDownList A PARTIR DEL RESULTADO Json (data)
                $.each(data, function (index, row) {
                    $("#ddlDetalleSolicitud_IdTipoPlaca").append("<option value='" + row.Valor + "'>" + row.Texto + "</option>")
                });
            },
            error: function (reponse) {
                ShowNotificacion("error", "Error", "Error al obtener los tipos de placa de la Orden de Compra seleccionada", "4", "0")
                console.log(reponse);
            }
        });
    }
}
function SolicitudGeneraPDF() {
    var IdSolicitud_ = $.urlParam('IdSolicitud') == null ? 0 : $.urlParam('IdSolicitud')
    var fileName = "SolicitudPlacas.pdf"
    var url = '../SolicitudesPlacas/SolicitudPlacasGeneraPDF';
    $.ajax({
        type: "POST",
        url: url,
        data: { "IdSolicitud": IdSolicitud_, "EnviaPDF": false },

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
function SolicitudGeneraEnviaPDF() {
    var IdSolicitud_ = $.urlParam('IdSolicitud') == null ? 0 : $.urlParam('IdSolicitud')
    var fileName = "SolicitudPlacas.pdf"
    var url = '../SolicitudesPlacas/SolicitudPlacasGeneraPDF';
    $.ajax({
        type: "POST",
        url: url,
        data: { "IdSolicitud": IdSolicitud_, "EnviaPDF" : true },

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

function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
};
$(document).ready(function () {
    datepicker = $('#txtFechaEntrega').datepicker({
        format: 'dd/mm/yyyy', 
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
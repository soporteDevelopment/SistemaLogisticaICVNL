function ValidaCamposDetails() {
    if (!ValidaProveedorDetails()) {
        return false
    } 
}
function ValidaCamposCreate() {
    if (!ValidaProveedorCreate()) {
        return false
    }
}

function ValidaProveedorDetails() {
    var resultado = false;
    if ($("#txtNumeroProveedor").val() == "") {
        ShowNotificacion("error", "Error", "Número del Proveedor no debe estar vacío")
        return false;
    }
    $.ajax({
        data: { CodigoProveedor: $("#txtNumeroProveedor").val() },
        type: "POST",
        async: false,
        url: "../../Proveedores/ValidaProveedor",
        success: function (data) {
            if (!data.ExecutionOK) { 
                ShowNotificacion("error", "Error", data.Message, 0, 6);
                $("#txtNombreProveedor").val('');
                resultado = false;
            }
            else { 
                $("#txtNombreProveedor").val(data.Data);
                resultado = true;
            }
        },
        error: function (reponse) {
            ShowNotificacion("error", "Error", reponse, 0, 6);
            resultado = false;
        }
    });
    return resultado;
}

function ValidaProveedorCreate() {
    var resultado = false;
    if ($("#txtNumeroProveedor").val() == "") {
        ShowNotificacion("error", "Error", "Número del Proveedor no debe estar vacío")
        return false;
    }
    $.ajax({
        data: { CodigoProveedor: $("#txtNumeroProveedor").val() },
        type: "POST",
        async: false,
        url: "../Proveedores/ValidaProveedor",
        success: function (data) {
            if (!data.ExecutionOK) {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
                $("#txtNombreProveedor").val('');
                resultado = false;
            }
            else {
                $("#txtNombreProveedor").val(data.Data);
                resultado = true;
            }
        },
        error: function (reponse) {
            ShowNotificacion("error", "Error", reponse, 0, 6);
            resultado = false;
        }
    });
    return resultado;
}



$(document).ready(function () {
});
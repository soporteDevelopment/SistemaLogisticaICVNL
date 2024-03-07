function ValidaCampos() {
    if (!ValidaCuentaContablePlacaVendida()) {
        return false
    } 
    if (!ValidaCuentaCostosVentaPlacaReporte()) {
        return false
    }
    if (!ValidaCentroCostosEntidadGobiernoPlacaVendida()) {
        return false
    }
    if (!ValidaCentroCostosEntidadGobiernoPlacaReporte()) {
        return false
    } 
}

function ValidaCuentaContablePlacaVendida() {
    var resultado = false;
    if ($("#txtCuentaCostosVentaPlacaVendida").val() == "") {
        ShowNotificacion("error", "Error", "la Cta. Costos Venta Placa Vendida no debe estar vacía")
        return false;
    }
    $.ajax({
        data: { cuenta: $("#txtCuentaCostosVentaPlacaVendida").val() },
        type: "POST",
        async: false,
        url: "Parametrizacion/ValidaCuentaContable",
        success: function (data) {
            if (!data.ExecutionOK) {
                console.log(data)
                ShowNotificacion("error", "Error", data.Message + " de la Cta. Costos Venta Placa Vendida, ¡Verifique!", 0, 6);
                resultado = false;
            }
            else {
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

function ValidaCuentaCostosVentaPlacaReporte() {
    var resultado = false;
    if ($("#txtCuentaCostosVentaPlacaReporte").val() == "") {
        ShowNotificacion("error", "Error", "la Cta. Costos Venta Placa con Reporte no debe estar vacía")
        return false;
    }
    $.ajax({
        data: { cuenta: $("#txtCuentaCostosVentaPlacaReporte").val() },
        type: "POST",
        async: false,
        url: "Parametrizacion/ValidaCuentaContable",
        success: function (data) {
            if (!data.ExecutionOK) {
                console.log(data)
                ShowNotificacion("error", "Error", data.Message + " de la Cta. Costos Venta Placa con Reporte, ¡Verifique!", 0, 6);
                resultado = false;
            }
            else {
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

function ValidaCentroCostosEntidadGobiernoPlacaVendida() {
    var resultado = false;
    if ($("#txtCentroCostosEntidadGobiernoPlacaVendida").val() == "") {
        ShowNotificacion("error", "Error", "El Centro de Costos Entidad Gob. Placa Vendida no debe estar vacío")
        return false;
    }
    $.ajax({
        data: { CentroCosto: $("#txtCentroCostosEntidadGobiernoPlacaVendida").val() },
        type: "POST",
        async: false,
        url: "Parametrizacion/ValidaCentroCostos",
        success: function (data) {
            if (!data.ExecutionOK) {
                console.log(data)
                ShowNotificacion("error", "Error", data.Message + " del Centro de Costos Entidad Gob. Placa Vendida, ¡Verifique!", 0, 6);
                resultado = false;
            }
            else {
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

function ValidaCentroCostosEntidadGobiernoPlacaReporte() {
    var resultado = false;
    if ($("#txtCentroCostosEntidadGobiernoPlacaReporte").val() == "") {
        ShowNotificacion("error", "Error", "el Centro de Costos Entidad Gob. Placa con Reporte no debe estar vacío")
        return false;
    }
    $.ajax({
        data: { CentroCosto: $("#txtCentroCostosEntidadGobiernoPlacaReporte").val() },
        type: "POST",
        async: false,
        url: "Parametrizacion/ValidaCentroCostos",
        success: function (data) {
            if (!data.ExecutionOK) {
                console.log(data)
                ShowNotificacion("error", "Error", data.Message + " del Centro de Costos Entidad Gob. Placa con Reporte, ¡Verifique!", 0, 6);
                resultado = false;
            }
            else {
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
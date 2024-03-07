function ValidaCamposDetails() {
    if (!ValidaDelegacionBancoDetails()) {
        return false;
    } 
}
function ValidaCamposCreate() {
    if (!ValidaDelegacionBancoCreate()) {
        return false;
    }
}

function ValidaDelegacionBancoDetails() {
    var resultado = false;
    if ($("#txtCentroCostosInfoFin").val() == "") {
        ShowNotificacion("error", "Error", "la Cta. Costos Venta Placa Vendida no debe estar vacía")
        return resultado;
    }
    $.ajax({
        data: { CentroCostosDelegacion: $("#txtCentroCostosInfoFin").val() },
        type: "POST",
        async: false,
        url: "../../DelegacionesBancos/ValidaDelegacionBanco",
        success: function (data) {
            if (!data.ExecutionOK) {
                console.log(data)
                $("#txtAlmacenCentroCostos").val('');
                ShowNotificacion("error", "Error", data.Message, 0, 6);
                resultado = false;
            }
            else {
				$("#txtAlmacenCentroCostos").val(data.Data)
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

function ValidaDelegacionBancoCreate() {
    var resultado = false;
    if ($("#txtCentroCostosInfoFin").val() == "") {
        ShowNotificacion("error", "Error", "la Cta. Costos Venta Placa Vendida no debe estar vacía")
        return resultado;
    }
    $.ajax({
        data: { CentroCostosDelegacion: $("#txtCentroCostosInfoFin").val() },
        type: "POST",
        async: false,
        url: "../DelegacionesBancos/ValidaDelegacionBanco",
        success: function (data) {
            if (!data.ExecutionOK) {
                console.log(data)
                $("#txtAlmacenCentroCostos").val('');
                ShowNotificacion("error", "Error", data.Message, 0, 6);
                resultado = false;
            }
            else {
                $("#txtAlmacenCentroCostos").val(data.Data)
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
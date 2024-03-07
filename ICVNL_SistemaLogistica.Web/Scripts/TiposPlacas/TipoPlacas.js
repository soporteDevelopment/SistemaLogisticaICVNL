
function ValidaCamposDetails() {
    if (!ValidaTipoPlacaDetails()) {
        return false;
    } 
}
function ValidaCamposCreate() {
    if (!ValidaTipoPlacaCreate()) {
        return false
    }
}

function ValidaTipoPlacaDetails() {
    var resultado = false;
    if ($("#txtCodigoInfofin").val() == "") {
        ShowNotificacion("error", "Error", "Código de Artículo en Infofin no debe estar vacío")
        return resultado;
    }
    $.ajax({
        data: { CodArticuloInfofin: $("#txtCodigoInfofin").val() },
        type: "POST",
        async: false,
        url: "../../TiposPlacas/ValidaTipoPlaca",
        success: function (data) {
            if (!data.ExecutionOK) {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
                resultado = false;
            }
            else {
                $("#txtDescripcionInfofin").val(data.Data.Articulo.descripcion);
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

function ValidaTipoPlacaCreate() {
    var resultado = false;
    if ($("#txtCodigoInfofin").val() == "") {
        ShowNotificacion("error", "Error", "Código de Artículo en Infofin no debe estar vacío")
        return resultado;
    }
    $.ajax({
        data: { CodArticuloInfofin: $("#txtCodigoInfofin").val() },
        type: "POST",
        async: false,
        url: "../TiposPlacas/ValidaTipoPlaca",
        success: function (data) {
            if (!data.ExecutionOK) {
                ShowNotificacion("error", "Error", data.Message, 0, 6);
                resultado = false;
            }
            else {
                $("#txtDescripcionInfofin").val(data.Data.Articulo.descripcion);
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
window.validarCamposObligatorios = function (claseAValidar) {
    var elementos = document.getElementsByClassName(claseAValidar);
    var numElementosFailed = 0;
    var mensaje_ = "";
    var nombreElementoFocus = "";

    for (var i = 0; i < elementos.length; i++) {
        $('[name="' + elementos[i].name + '"').val(elementos[i].value.replace(/^\s+|\s+$/gm, ''));
        if (elementos[i].tagName == "SELECT")
            mensaje_ = "El campo es requerido.";
        else
            mensaje_ = "El campo debe ser mayor que 0 (Cero)";

        if (elementos[i].value == '') {
            $('[name="' + elementos[i].name + '"]').addClass("resalta");
            nombreElementoFocus = elementos[i].name;
            numElementosFailed++;
        }
        else {
            if (elementos[i].value == '0' || elementos[i].value == '0.00') {
                $('[name="' + elementos[i].name + '"]').addClass("resalta");
                nombreElementoFocus = elementos[i].name;
                numElementosFailed++;
            }
            else {
                $('[name="' + elementos[i].name + '"]').removeClass("resalta");
            }
        }
    }

    for (var i = 0; i < elementos.length; i++) {
        $('[name="' + elementos[i].name + '"').val(elementos[i].value.replace(/^\s+|\s+$/gm, ''));

        if (elementos[i].value == '') {
            nombreElementoFocus = elementos[i].name;
            break;
        }
        else {
            if (elementos[i].value == '0' || elementos[i].value == '0.00') {
                nombreElementoFocus = elementos[i].name;
                break;
            }
        }
    }

    if (numElementosFailed > 0) {
        ShowNotificacion('error', 'Campo Obligatorio', mensaje_, 0, 6)
        $('[name="' + nombreElementoFocus + '"]').focus();
        return false;
    }

    return true;
}

window.validarTextoCamposObligatorios = function (claseAValidar) {
    var elementos = document.getElementsByClassName(claseAValidar);
    var numElementosFailed = 0;
    var mensaje_ = "";
    var nombreElementoFocus = "";

    for (var i = 0; i < elementos.length; i++) {
        $('[name="' + elementos[i].name + '"').val(elementos[i].value.replace(/^\s+|\s+$/gm, ''));
        mensaje_ = "El campo es obligatorio ";

        if (elementos[i].value == '') {
            $('[name="' + elementos[i].name + '"]').addClass("resalta");
            nombreElementoFocus = elementos[i].name;
            numElementosFailed++;
        }
        else {
            if (elementos[i].value == '0' || elementos[i].value == '0.00') {
                $('[name="' + elementos[i].name + '"]').addClass("resalta");
                nombreElementoFocus = elementos[i].name;
                numElementosFailed++;
            }
            else {
                $('[name="' + elementos[i].name + '"]').removeClass("resalta");
            }
        }
    }

    for (var i = 0; i < elementos.length; i++) {
        $('[name="' + elementos[i].name + '"').val(elementos[i].value.replace(/^\s+|\s+$/gm, ''));

        if (elementos[i].value == '') {
            nombreElementoFocus = elementos[i].name;
            break;
        }
        else {
            if (elementos[i].value == '0' || elementos[i].value == '0.00') {
                nombreElementoFocus = elementos[i].name;
                break;
            }
        }
    }

    if (numElementosFailed > 0) {
        ShowNotificacion('error', 'Campo Obligatorio', mensaje_, 0, 6)
        $('[name="' + nombreElementoFocus + '"]').focus();
        return false;
    }

    return true;
}

window.validarTextoCamposObligatoriosTP = function (claseAValidar) {
    var elementos = document.getElementsByClassName(claseAValidar);
    var numElementosFailed = 0;
    var mensaje_ = "";
    var nombreElementoFocus = "";

    for (var i = 0; i < elementos.length; i++) { 
        if (elementos[i].value == '') { 
            numElementosFailed++;
        }
        else {
            if (elementos[i].value == '0' || elementos[i].value == '0.00') { 
                numElementosFailed++;
            } 
        }
    } 

    if (numElementosFailed > 0) {
        ShowNotificacion('error', 'Campo Obligatorio', "Campos Obligatorios en Datos Persona que se va a llevar las placas", 0, 6);
        $('[name="' + nombreElementoFocus + '"]').focus();
        return false;
    }

    return true;
}

window.removeClassValidaObligatorio = function (claseAValidar) {
    var elementos = document.getElementsByClassName(claseAValidar);
    for (var i = 0; i < elementos.length; i++) {
        $('[name="' + elementos[i].name + '"]').removeClass("resalta");
    }
}


window.validarDatos = function () {
    //DECIMALES
    $(".campodecimales").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            return;
        }

        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    //SIN DECIMALES
    $(".camponumerico").keydown(function (e) {
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13]) !== -1 ||
            (e.keyCode == 65 && e.ctrlKey === true) ||
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            return;
        }

        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    $(".solonumero").attr("ondrop", "return false");
    //si te piden que no permita el click derecho pegar, descomenta la linea de abajo
    //$(".solonumero").attr("oncontextmenu", "return false");
}
//Asigna el control daterangepicker a todos los controles que tengan en su id "txtFecha"
$(function () {
    $('input[id*="txtFecha"]').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minYear: 1901,
        "drops": "up",
        maxYear: parseInt(moment().format('YYYY'), 10),
        "locale": {
            cancelLabel: 'Clear',
            "format": "DD/MM/YYYY", 
            "applyLabel": "APLICAR",
            "cancelLabel": "CANCELAR", 
            "customRangeLabel": "Personalizar...",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        }
    }, function (start, end, label) { 
    });

    $('input[id*="txtFiltroFecha"]').daterangepicker({
        "locale": {
            cancelLabel: 'Clear',
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "APLICAR",
            "cancelLabel": "CANCELAR",
            "fromLabel": "De",
            "toLabel": "A",
            "customRangeLabel": "Personalizar...",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        },
        opens: 'right',
        autoUpdateInput: false
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('DD-MM-YYYY') + ' to ' + end.format('DD-MM-YYYY'));
    });

    $('input[id*="txtFiltroFecha"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
        $('input[id*="txtFiltroFecha"]').addClass("edited");
    });

    $('input[id*="txtFiltroFecha"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $('input[id*="txtFiltroFecha"]').removeClass("form-control edited").addClass("form-control");
    });
});
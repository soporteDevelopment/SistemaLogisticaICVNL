$(document).ready(function (e) {
    /*
     * Se requiere agregar los siguientes archivos:
     
    <link href="Css/assets/global/plugins/datatables/datatables.min.css" rel="stylesheet" />
    <link href="Css/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css" rel="stylesheet" />

    <script src="Css/assets/global/scripts/datatable.js"></script>
    <script src="Css/assets/global/plugins/datatables/datatables.min.js"></script>
    <script src="Css/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"></script>

    */

    //Tabla sin botones
    $('.grid-dataTable-simple').DataTable({
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Registros del <b>_START_</b> al <b>_END_</b> de un total de <b>_TOTAL_</b>",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total registros)",
            "lengthMenu": "_MENU_ registros por p&#225;gina",
            "search": "Buscar:",
            "zeroRecords": "No matching records found"
        },
        "order": [
            [0, 'asc']
        ],

        "lengthMenu": [
            [50, 100, 150, 200, -1],
            [50, 100, 150, 200, "Todos"] // change per page values here
        ],
        // set the initial value
        "pageLength": 50,
        scrollY: '50vh',
        responsive: true
    });

    //Tabla con botones
    $('.grid-dataTable-buttons').DataTable({
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Registros del <b>_START_</b> al <b>_END_</b> de un total de <b>_TOTAL_</b>",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total registros)",
            "lengthMenu": "_MENU_ registros por p&#225;gina",
            "search": "Buscar:",
            "zeroRecords": "No matching records found"
        },
        dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        buttons: [
            { extend: 'print', className: 'btn dark btn-outline', text: 'imprimir' },
            { extend: 'copy', className: 'btn red btn-outline', text: 'copiar' },
            { extend: 'pdf', className: 'btn green btn-outline', text: 'pdf' },
            { extend: 'excel', className: 'btn yellow btn-outline ', text: 'excel' },
            { extend: 'csv', className: 'btn purple btn-outline ', text: 'csv' },
            { extend: 'colvis', className: 'btn dark btn-outline', text: 'columnas' }
        ],
        "order": [
            [0, 'asc']
        ],
        "lengthMenu": [
            [50, 100, 150, 200, -1],
            [50, 100, 150, 200, "Todos"] // change per page values here
        ],
        // set the initial value
        "pageLength": 50,
        scrollY: '50vh',
        responsive: true
    });

    //Tabla con botones, afectando scroll Y
    $('.grid-dataTable-buttons-scrollY').DataTable({
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Registros del <b>_START_</b> al <b>_END_</b> de un total de <b>_TOTAL_</b>",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total registros)",
            "lengthMenu": "_MENU_ registros por p&#225;gina",
            "search": "Buscar:",
            "zeroRecords": "No matching records found"
        },
        dom: "<'row' <'col-md-12'B>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>",
        buttons: [
            { extend: 'print', className: 'btn dark btn-outline', text: 'imprimir' },
            { extend: 'copy', className: 'btn red btn-outline', text: 'copiar' },
            { extend: 'pdf', className: 'btn green btn-outline', text: 'pdf' },
            { extend: 'excel', className: 'btn yellow btn-outline ', text: 'excel' },
            { extend: 'csv', className: 'btn purple btn-outline ', text: 'csv' },
            { extend: 'colvis', className: 'btn dark btn-outline', text: 'columnas' }
        ],
        "order": [
            [0, 'asc']
        ],
        "lengthMenu": [
            [50, 100, 150, 200, -1],
            [50, 100, 150, 200, "Todos"] // change per page values here
        ],
        // set the initial value
        "pageLength": 50,
        scrollY: '50vh',
        scrollCollapse: true,
        scrollX: '100%'
    });
})
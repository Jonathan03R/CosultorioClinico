//Este script contendrá la lógica del buscador personalizado y filtrado de pacientes.

$(document).ready(function () {
    inicializarBuscadorPersonalizado();
    inicializarFiltrosEstado();
});

function inicializarBuscadorPersonalizado() {
    $.fn.dataTable.ext.search.push(filtrarPacientes);

    $('#buscador-personalizado').on('keyup', function () {
        $('#tabla_pacientes').DataTable().draw();
    });
}

function filtrarPacientes(settings, data, dataIndex) {
    var searchTerm = $('#buscador-personalizado').val().toLowerCase();

    if (searchTerm === '') {
        return true;
    }

    var nombre = data[2].toLowerCase();
    var expediente = data[4].toLowerCase();

    if (nombre.startsWith(searchTerm) || expediente.startsWith(searchTerm)) {
        return true;
    }

    return false;
}

function inicializarFiltrosEstado() {
    $('#lista-pacientes-container .nav-link').on('click', function (e) {
        e.preventDefault();

        $('.nav-link').removeClass('active');
        $(this).addClass('active');

        var filterValue = $(this).data('filter');
        var table = $('#tabla_pacientes').DataTable();

        if (filterValue === 'all') {
            table.column(3).search('').draw();
        } else {
            table.column(3).search('^' + filterValue + '$', true, false).draw();
        }
    });
}

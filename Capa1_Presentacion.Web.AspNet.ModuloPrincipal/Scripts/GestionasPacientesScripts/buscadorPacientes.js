//Este script contendrá la lógica del buscador personalizado y filtrado de pacientes.

$(document).ready(function () {
    inicializarBuscadorPersonalizado();
    inicializarFiltrosEstado();
    inicializarValidacionDni();
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

    var dni = data[1].toLowerCase();
    var nombre = data[2].toLowerCase();
    var expediente = data[4].toLowerCase();

    if (nombre.startsWith(searchTerm) || expediente.startsWith(searchTerm) || dni.startsWith(searchTerm)) {
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


function inicializarValidacionDni() {
    $(document).on('input', '#PacienteDNI', function () {
        const dni = $(this).val().trim();

        if (dni.length === 8 && /^\d+$/.test(dni)) {
            verificarDni(dni);
        } else {
            $('#PacienteDNI').removeClass('is-valid is-invalid');
            $('#dniFeedback').hide();
        }
    });

    // Reiniciar validación al abrir el modal
    $('#modalAgregarPaciente').on('show.bs.modal', function () {
        $('#PacienteDNI').removeClass('is-valid is-invalid');
        $('#dniFeedback').hide();
    });
}

function verificarDni(dni) {
    $.ajax({
        url: urlBuscarDni,
        type: 'GET',
        dataType: 'json',
        data: { dni: dni },
        success: function (response) {
            if (response.consultaExitosa && response.data) {
                $('#PacienteDNI').removeClass('is-valid').addClass('is-invalid');
                $('#dniFeedback').show();
            } else {
                $('#PacienteDNI').removeClass('is-invalid').addClass('is-valid');
                $('#dniFeedback').hide();
            }
        },
        error: function () {
            $('#PacienteDNI').removeClass('is-valid').addClass('is-invalid');
            $('#dniFeedback').text('Error al verificar el DNI.');
            $('#dniFeedback').show();
        }
    });
}

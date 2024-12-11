
$(document).ready(function () {
    inicializarBuscadorPersonalizado();
});

function inicializarBuscadorPersonalizado() {
    $.fn.dataTable.ext.search.push(filtrarCitas);

    $('#buscador-personalizado').on('keyup', function () {
        $('#tabla_Citas').DataTable().draw();
    });
}

function filtrarCitas(settings, data, dataIndex) {
    var searchTerm = $('#buscador-personalizado').val().toLowerCase();

    if (searchTerm === '') {
        return true;
    }
    var nombrePaciente = data[1].toLowerCase();  // Nombre del paciente
    var medico = data[2].toLowerCase();  // Médico
    var especialidad = data[3].toLowerCase();  // Especialidad

    // Si el término de búsqueda se encuentra en alguna de las columnas, pasa el filtro
    if (nombrePaciente.includes(searchTerm) || medico.includes(searchTerm) || especialidad.includes(searchTerm)) {
        return true;
    }

    // Si no, se descarta la fila
    return false;
}

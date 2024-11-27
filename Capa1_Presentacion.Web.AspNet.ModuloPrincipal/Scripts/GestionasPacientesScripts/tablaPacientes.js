//Este script se encargará de inicializar y configurar la tabla de pacientes utilizando DataTables.
$(document).ready(function () {
    var table = inicializarTablaPacientes();

    manejarEventosTabla(table);
});

function inicializarTablaPacientes() {
    return $('#tabla_pacientes').DataTable({
        "dom": "lrtip",
        "ajax": {
            "url": $('#tabla_pacientes').data("url-listar-pacientes"),
            "type": "GET",
            "dataSrc": function (json) {
                if (json.consultaExitosa) {
                    var processedData = procesarDatosPacientes(json.data);
                    actualizarContadores(processedData);
                    return processedData;
                } else {
                    alert(json.mensaje);
                    return [];
                }
            }
        },
        "columns": obtenerColumnasTabla(),
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json"
        },
        "responsive": false,
        "ordering": false,
        "columnDefs": [
            { "targets": 0, "orderable": false, "className": "dt-center" },
            { "targets": [7, 8, 9, 10], "visible": true } 
        ]
    });
}

function procesarDatosPacientes(data) {
    return data.map(function (item) {
        return {
            PacienteCodigo: item.PacienteCodigo,
            PacienteDNI: item.PacienteDNI,
            PacienteNombreCompleto: item.PacienteNombreCompleto,
            PacienteFechaNacimiento: item.PacienteFechaNacimiento,
            PacienteDireccion: item.PacienteDireccion,
            PacienteTelefono: item.PacienteTelefono,
            PacienteCorreoElectronico: item.PacienteCorreoElectronico,
            PacienteEstado: item.PacienteEstado,
            PacienteHistorialClinicoCodigo: item.PacienteHistorialClinicoCodigo,
            PacienteSeguro: item.PacienteSeguro || 'Sin seguro',
            PacienteFechaActivacion: item.PacienteFechaActivacion || 'Fecha no disponible',
            PacienteNotas: item.PacienteNotas || 'No hay notas adicionales'
        };
    });
}

function actualizarContadores(data) {
    var totalCount = data.length;
    var activeCount = data.filter(function (item) {
        return item.PacienteEstado === 'Activo';
    }).length;
    var inactiveCount = data.filter(function (item) {
        return item.PacienteEstado === 'Inactivo';
    }).length;

    $('#total-count').text(totalCount);
    $('#active-count').text(activeCount);
    $('#inactive-count').text(inactiveCount);
}

function obtenerColumnasTabla() {
    return [
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": '<i class="fas fa-plus-circle text-success toggle-icon" style="cursor: pointer;"></i>',
            "width": "20px"
        },
        { "data": "PacienteDNI" },
        {
            "data": "PacienteNombreCompleto",
            "render": function (data) {
                return `<span class="nombre-paciente">${data}</span>`;
            }
        },
        {
            "data": "PacienteEstado",
            "render": function (data, type, row) {
                return renderizarEstadoPaciente(data, type, row);
            }
        },
        {
            "data": "PacienteHistorialClinicoCodigo",
            "render": function (data) {
                return `<span class="badge bg-light text-dark">${data}</span>`;
            }
        },
        { "data": "PacienteTelefono" },
        { "data": "PacienteSeguro" },

        // Columnas adicionales (ocultas)
        { "data": "PacienteCodigo", "visible": false },
        { "data": "PacienteFechaNacimiento", "visible": false },
        { "data": "PacienteDireccion", "visible": false },
        { "data": "PacienteCorreoElectronico", "visible": false }
    ];
}

function manejarEventosTabla(table) {
    $('#tabla_pacientes thead th').eq(0).html('');

    // Evento para expandir/contraer detalles
    $('#tabla_pacientes tbody').on('click', 'td.details-control', function () {
        toggleDetallesFila(table, $(this));
    });
}

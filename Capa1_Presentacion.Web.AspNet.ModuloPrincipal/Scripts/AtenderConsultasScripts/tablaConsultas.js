
$(document).ready(function () {
    var table = inicializarTablaPacientes();
    manejarEventosTabla(table);
});
function inicializarTablaPacientes() {

    return $('#tabla_consultas').DataTable({
        "dom": "lrtip",
        "ajax": {
            "url": $('#tabla_consultas').data("url-listar-consulta"),
            "type": "GET",
            "dataSrc": function (json) {
                if (json.consultaExitosa) {
                    actualizarContadores(json.data);
                    return json.data;
                } else {
                    alert(json?.mensaje || "Error en la consulta");
                    return [];
                }
            }
        },
        "columns": obtenerColumnasConsultas(),
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json"
        },
        "responsive": false,
        "ordering": false,
    });
}

function obtenerColumnasConsultas(){
    return [
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": '<i class="fas fa-plus-circle text-success toggle-icon" style="cursor: pointer;"></i>',
            "width": "20px"
        },
        {
            "data": "CitaCiodigo",
            "render": function (data) {
                return `<span class="badge bg-light text-dark">${data}</span>`;
            }
        },
        { "data": "PacienteNombre" },
        { "data": "ConsultaFechaCita" },
        { "data": "ConsultaHoraFecha" },
        { "data": "MedicoNombre" },
        {
            "data": "ConsultaEstado",
            "render": function (data) {
                const estadoClase = obtenerClaseEstado(data);
                return `<span class="text-crema badge ${estadoClase}">${data}</span>`;
            }
        }
        
    ];
}

function obtenerClaseEstado(estado) {
    switch (estado) {
        case "Pendiente": return "bg-warning";
        case "No asistio": return "bg-secondary";
        case "Atendido": return "bg-success";
        case "Cancelado": return "bg-danger";
        default: return "bg-dark"; // Clase para estado desconocido 
    }
}

function manejarEventosTabla(table) {
    $('#tabla_consultas thead th').eq(0).html('');

    // Evento para expandir/contraer detalles
    $('#tabla_consultas tbody').on('click', 'td.details-control', function () {
        toggleDetallesFila(table, $(this));
    });
}


function actualizarContadoresFiltros() {
    var totalCount = data.legth;
    var pendienteCount = data.filter(function (item) {
        return item.ConsultaEstado === 'Pendiente';
    }).legth;
    var noAsistioCount = data.filter(function (item) {
        return item.ConsultaEstado === 'No asistio';
    }).legth;
    var atentidoCount = data.filter(function (item) {
        return item.ConsultaEstado === 'Atendido';
    }).legth;
    var CanceladoCount = data.filter(function (item) {
        return item.ConsultaEstado === 'Cancelado';
    }).legth;

    $('#total-count').text(totalCount);
    $('#pending-count').text(pendienteCount);
    $('#no-show-count').text(noAsistioCount);
    $('#attended-count').text(atentidoCount);
    $('#cancelled-count').text(CanceladoCount);
}



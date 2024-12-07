$(document).ready(function () {
    var table = inicializarTablaConsultas();
    manejarEventosTabla(table);
    inicializarFiltrosCitas(table);
    aplicarFiltroPorDefecto(table, 'Pendiente');
    agregarEventosFiltros(table);
});

function inicializarTablaConsultas() {
    return $('#tabla_consultas').DataTable({
        "dom": "lrtip",
        "ajax": {
            "url": $('#tabla_consultas').data("url-listar-consulta"),
            "type": "GET",
            "dataSrc": function (json) {
                if (json?.consultaExitosa) {
                    console.log('Datos recibidos:', json.data);
                    actualizarContadoresFiltros(json.data); // Asegúrate de pasar los datos
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
        "ordering": false
    });
}

function obtenerColumnasConsultas() {
    return [
        {
            "className": 'details-control',
            "orderable": false,
            "data": null,
            "defaultContent": '<i class="fas fa-plus-circle text-success toggle-icon" style="cursor: pointer;"></i>',
            "width": "20px"
        },
        {
            "data": "CitaCodigo", // Corrección de "CitaCiodigo"
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
        },
        // columnas adicionales 
        {
            "data": "FechaCitaFilter",
            "visible": false,
        },
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

function actualizarContadoresFiltros(data) { 
    console.log('Datos recibidos para actualización de contadores:', data); 

    var totalCount = data.length; 
    var pendienteCount = data.filter(function (item) {
        return item.ConsultaEstado === 'Pendiente';
    }).length; 
    var noAsistioCount = data.filter(function (item) {
        return item.ConsultaEstado === 'No asistio';
    }).length;
    var atendidoCount = data.filter(function (item) {
        return item.ConsultaEstado === 'Atendido';
    }).length; 
    var canceladoCount = data.filter(function (item) {
        return item.ConsultaEstado === 'Cancelado';
    }).length; 

    //console.log('Total Count:', totalCount); 
    //console.log('Pendiente Count:', pendienteCount);
    //console.log('No Asistio Count:', noAsistioCount);
    //console.log('Atendido Count:', atendidoCount);
    //console.log('Cancelado Count:', canceladoCount);

    $('#total-count').text(totalCount);
    $('#pending-count').text(pendienteCount);
    $('#no-show-count').text(noAsistioCount);
    $('#attended-count').text(atendidoCount);
    $('#cancelled-count').text(canceladoCount);
}


function aplicarFiltroPorDefecto(table, filterValue) {
    $('.nav-link[data-filter="' + filterValue + '"]').addClass('active');
    if (filterValue === 'all') {
        table.column(6).search('').draw();
    } else {
        table.column(6).search('^' + filterValue + '$', true, false).draw();
    }
}



function agregarEventosFiltros(table) {
    
    $('#inputFechaCitas').on('change', function () {
        let selectedDate = this.value;  
        let formattedDate = selectedDate.split('-').reverse().join('-'); 
        table.column(7).search(formattedDate).draw();
    });

    // Resetear filtros
    $('#resetFilter').on('click', function () {
        $('#inputSelectEspecialidad').val('');
        $('#inputSelectMedico').val('');
        $('#inputSelectEstado').val('');
        $('#inputFecha').val('');

        // Limpiar búsqueda en todas las columnas
        table.search('').columns().search('').draw();
    });
}



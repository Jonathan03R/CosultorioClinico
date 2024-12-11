var table; // Variable global para la instancia de DataTable

$(document).ready(function () {
    inicializarTablaCitas();
    agregarEventosFiltros();
});

function inicializarTablaCitas() {
    var urlListarCitas = $('#tabla_Citas').data("urlListarCitas");

    if (!urlListarCitas) return;

    table = $('#tabla_Citas').DataTable({
        "dom": "lrtip",
        "ajax": {
            "url": urlListarCitas,
            "type": "GET",
            "dataSrc": function (json) {
                if (json?.consultaExitosa) {
                    return json.data;
                } else {
                    alert(json?.mensaje || "Error en la consulta");
                    return [];
                }
            }
        },
        "columns": obtenerColumnas(),
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json"
        },
        "responsive": false,
        "ordering": false,
    });
}
function obtenerColumnas() {
    return [
        { "data": "CitaCodigo" },
        { "data": "NombrePaciente"},
        { "data": "MedicoNombre"},
        { "data": "Especialidad"},
        { "data": "Fecha"},
        { "data": "Estado"},
        {
            data: null,
            render: function (data, type, row) {
                return `
                    <button class="btn btn-info" onclick="verCita('${row.CitaCodigo}')">Ver</button>
                    <button class="btn btn-danger" onclick="cancelarCita('${row.CitaCodigo}')">Cancelar</button>
                `;
            }
        },

        // Columnas adicionales (ocultas) por que el val es el codigo de la especilidad para filtrar
        { "data": "EspecialidadCod", "visible": false },
        { "data": "MedicoCodigo", "visible": false },
        {
            "data": "FechaFilter",
            "visible": false,
            "render": function (data, type, row) {
                const fecha = row.Fecha.split(' ')[0];  // Obtén solo la fecha (sin hora)
                return fecha;  // Devolverla en formato 'yyyy-mm-dd' (ya debería estar)
            }
        },
    ];
}


function agregarEventosFiltros() {
    // Filtro por Médico
    $('#inputSelectMedico').on('change', function () {
        table.column(8).search(this.value).draw();
    });

    // Filtro por Estado
    $('#inputSelectEstado').on('change', function () {
        table.column(5).search(this.value).draw();  
    });

    // Filtro por Especialidad
    $('#inputSelectEspecialidad').on('change', function () {
        table.column(7).search(this.value).draw();  
    });

    //fecha
    $('#inputFecha').on('change', function () {
        let selectedDate = this.value;  // La fecha seleccionada en formato 'yyyy-mm-dd'
        let formattedDate = selectedDate.split('-').reverse().join('-');  // Convierte a formato 'dd-mm-yyyy'

        // Filtra la columna "Fecha" comparando solo la fecha (sin la hora)
        table.column(9).search(formattedDate).draw();
    });

    // Resetear filtros
    $('#resetFilter').on('click', function () {
        // Restablecer valores de los filtros
        $('#inputSelectEspecialidad').val('');
        $('#inputSelectMedico').val('');
        $('#inputSelectEstado').val('');
        $('#inputFecha').val('');

        // Limpiar búsqueda en todas las columnas
        table.search('').columns().search('').draw();
    });
}


$(document).ready(function () {
    inicializarTablaCitas();
});

function inicializarTablaCitas() {
    $('#tablaCitas').DataTable({
        ajax: obtenerConfiguracionAjax(),
        columns: obtenerColumnas(),
        responsive: true,
        searching: false, // Desactiva el buscador por defecto
        language: { url: '//cdn.datatables.net/plug-ins/1.13.4/i18n/es-ES.json' }
    });
}

function obtenerConfiguracionAjax() {
    return {
        url: '/GestionarCitas/ObtenerTodasCitas',
        type: 'GET',
        dataType: 'json',
        dataSrc: procesarDatos
    };
}

function procesarDatos(json) {
    if (json.consultaExitosa) {
        return json.data;
    } else {
        alert(`Error al cargar las citas: ${json.mensaje}`);
        return [];
    }
}

function obtenerColumnas() {
    return [
        { data: 'CitaCodigo', title: 'Código' },
        { data: 'CitaPaciente.PacienteNombreCompleto', title: 'Paciente' },
        {
            data: 'CitaMedico.MedicoNombre',
            render: (data, type, row) => `${data} ${row.CitaMedico.MedicoApellido}`,
            title: 'Médico'
        },
        { data: 'CitaTipoConsulta.TipoConsultaCodigo', title: 'Especialidad' },
        {
            data: 'CitaFechaHora',
            render: (data) => {
                const fecha = new Date(parseInt(data.substr(6)));
                return fecha.toLocaleString();
            },
            title: 'Fecha y Hora'
        },
        {
            data: 'CitaEstado',
            render: (data) => {
                switch (data) {
                    case 'P': return 'Pendiente';
                    case 'C': return 'Confirmada';
                    case 'X': return 'Cancelada';
                    default: return 'Desconocido';
                }
            },
            title: 'Estado'
        },
        {
            data: null,
            render: () => `
                <button class="btn btn-sm btn-primary">Editar</button>
                <button class="btn btn-sm btn-danger">Eliminar</button>`,
            title: 'Acciones',
            orderable: false
        }
    ];
}


    //const tablaCitas = $('#tablaCitas').DataTable({
    //    ajax: {
    //        url: '@Url.Action("ObtenerCitasPorPaciente", "GestionarCitas")',
    //        data: function (d) {
    //            d.pacienteCodigo = $('#codigoPacienteBuscar').val(); // Enviar parámetro de búsqueda
    //            d.medico = $('#filtroMedico').val(); // Filtro por médico
    //            d.estado = $('#filtroEstado').val(); // Filtro por estado
    //            d.fecha = $('#filtroFecha').val(); // Filtro por fecha
    //            d.especialidad = $('#filtroEspecialidad').val(); // Filtro por especialidad
    //        },
    //        dataSrc: 'data',
    //        error: function (xhr, error, thrown) {
    //            console.log(xhr.responseText); // Log para depuración
    //        }
    //    },
    //    columns: [
    //        { data: 'citaCodigo' },
    //        { data: 'citaPaciente.nombre' },
    //        { data: 'citaMedico.nombre' },
    //        { data: 'citaMedico.especialidad' },
    //        { data: 'citaFechaHora', render: function (data) { return new Date(data).toLocaleString(); } },
    //        {
    //            data: 'citaEstado',
    //            render: function (data) {
    //                switch (data) {
    //                    case 'P': return 'Pendiente';
    //                    case 'C': return 'Confirmada';
    //                    case 'X': return 'Cancelada';
    //                    default: return 'Desconocido';
    //                }
    //            }
    //        },
    //        {
    //            data: null,
    //            render: function (data, type, row) {
    //                return `<button class="btn btn-warning btn-sm btnEditar" data-id="${row.citaCodigo}">Editar</button>
    //                                <button class="btn btn-danger btn-sm btnCancelar" data-id="${row.citaCodigo}">Cancelar</button>`;
    //            }
    //        }
    //    ],
    //    dom: 'rtip',
    //});

    //// Evento para buscar citas con filtros
    //$('#btnBuscarCitas').on('click', function () {
    //    tablaCitas.ajax.reload(); // Recargar tabla con los filtros aplicados
    //});

    //// Evento para actualizar la especialidad según el médico seleccionado
    //$('#medico').on('change', function () {
    //    const medicoId = $(this).val();
    //    if (medicoId) {
    //        $.ajax({
    //            url: '@Url.Action("ObtenerEspecialidadPorMedico", "GestionarCitas")',
    //            type: 'GET',
    //            data: { medicoId },
    //            success: function (data) {
    //                if (data.consultaExitosa) {
    //                    $('#especialidad').val(data.especialidad);
    //                } else {
    //                    alert('No se pudo obtener la especialidad del médico.');
    //                }
    //            },
    //            error: function (xhr, status, error) {
    //                console.error('Error al obtener la especialidad:', error);
    //            }
    //        });
    //    } else {
    //        $('#especialidad').val('');
    //    }
    //});


$(document).ready(function () {
    cargarEspecialidades(); 
    cargarEstados();
    cargarTipoConsulta();

    $('#inputSelectEspecialidadModal').on('change', function () {
        const especialidadSeleccionada = $(this).find('option:selected').text();

        if (especialidadSeleccionada) {
            cargarMedicosPorEspecialidad(especialidadSeleccionada); 
        } else {
            limpiarSelectMedico();
        }
    });
});


function cargarMedicosPorEspecialidad(especialidadSeleccionada) {
    $.ajax({
        url: '/GestionarCitas/ListarMedicosConEspecialidad',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.consultaExitosa) {
                const selectMedico = $('#inputSelectMedicoModal');
                selectMedico.empty(); // Limpia el select antes de llenarlo
                selectMedico.append('<option value="">Seleccione un médico</option>');

                // Filtra los médicos por el nombre de la especialidad seleccionada
                const medicosFiltrados = response.data.filter(medico => medico.EspecialidadNombre.trim() === especialidadSeleccionada.trim());

                medicosFiltrados.forEach(medico => {
                    const optionHtml = `<option value="${medico.MedicoCodigo.trim()}">${medico.MedicoNombre}</option>`;
                    selectMedico.append(optionHtml);
                });

                if (medicosFiltrados.length === 0) {
                    alert("No hay médicos disponibles para esta especialidad.");
                }
            } else {
                alert(`Error al cargar médicos: ${response.mensaje}`);
            }
        },
        error: function (xhr, status, error) {
            alert(`Error al realizar la solicitud: ${error}`);
        }
    });
}

function limpiarSelectMedico() {
    const selectMedico = $('#inputSelectMedicoModal');
    selectMedico.empty();
    selectMedico.append('<option value="">Seleccione un médico</option>');
}
function cargarEspecialidades() {
    $.ajax({
        url: '/GestionarCitas/ListarEspecialidades',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.consultaExitosa) {
                // Select en los filtros
                const selectEspecialidadFiltro = $('#inputSelectEspecialidad');
                selectEspecialidadFiltro.empty();
                selectEspecialidadFiltro.append('<option value="">Todos</option>');

                // Select en el modal
                const selectEspecialidadModal = $('#inputSelectEspecialidadModal');
                selectEspecialidadModal.empty();
                selectEspecialidadModal.append('<option value="">Seleccione una especialidad</option>');

                response.data.forEach(function (especialidad) {
                    const optionHtml = `<option value="${especialidad.EspecialidadCodigo}">${especialidad.EspecialidadNombre}</option>`;
                    selectEspecialidadFiltro.append(optionHtml);
                    selectEspecialidadModal.append(optionHtml);
                });
            } else {
                alert(`Error al cargar especialidades: ${response.mensaje}`);
            }
        },
        error: function (xhr, status, error) {
            alert(`Error al realizar la solicitud: ${error}`);
        }
    });
}

function cargarTipoConsulta()
{
    $.ajax({
        url: '/GestionarCitas/ListarTiposDeConsulta',
        type: 'GET',
        dataType: 'json',

        success: function (response) {
            if (response.consultaExitosa) {
                // Select en los filtros
                const selectEspecialidadFiltro = $('#inputSelectTipoConsulta');
                selectEspecialidadFiltro.empty();
                selectEspecialidadFiltro.append('<option value="">Seleccionar ...</option>');

                response.data.forEach(function (TipoConsulta) {
                    const optionHtml = `<option value="${TipoConsulta.TipoConsultaCodigo}">${TipoConsulta.TipoConsultaDescripcion}</option>`;
                    selectEspecialidadFiltro.append(optionHtml);
                });
            } else {
                alert(`Error al cargar especialidades: ${response.mensaje}`);
            }
        },
        error: function (xhr, status, error) {
            alert(`Error al realizar la solicitud: ${error}`);
        }
    });
}
function cargarEstados() {
    const estados = [
        { valor: 'P', texto: 'Pendiente' },
        { valor: 'C', texto: 'Confirmada' },
        { valor: 'X', texto: 'Cancelada' },
        { valor: '', texto: 'Desconocido' }
    ];

    const selectEstados = $('#inputSelectEstado');
    selectEstados.empty(); 

    selectEstados.append('<option value="">Todos</option>'); 

    estados.forEach(estado => {
        selectEstados.append(`<option value="${estado.valor}">${estado.texto}</option>`);
    });
}

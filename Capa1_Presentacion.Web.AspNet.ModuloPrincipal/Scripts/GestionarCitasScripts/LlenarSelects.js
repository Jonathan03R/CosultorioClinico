$(document).ready(function () {
    inicializarEventos(); // Configura todos los eventos de interacción
    cargarEspecialidades(); // Carga inicial de especialidades
    cargarEstados(); // Carga inicial de estados
    cargarTipoConsulta(); // Carga inici
});
function inicializarEventos() {
    $('#inputSelectEspecialidadModal').on('change', function () {
        const especialidadSeleccionada = $(this).find('option:selected').text().trim();

        if (especialidadSeleccionada) {
            cargarMedicosPorEspecialidad(especialidadSeleccionada);
        } else {
            limpiarSelectMedico();
        }
    });

    $('#inputSelectEspecialidad').on('change', function () {
        const especialidadSeleccionada = $(this).find('option:selected').text().trim();

        if (especialidadSeleccionada) {
            cargarMedicosPorEspecialidad(especialidadSeleccionada);
        } else {
            limpiarSelectMedicoFiltro();
        }
    });
}

function cargarEspecialidades() {
    $.ajax({
        url: '/GestionarCitas/ListarEspecialidades',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.consultaExitosa) {
                llenarSelectEspecialidades(response.data);
            } else {
                alert(`Error al cargar especialidades: ${response.mensaje}`);
            }
        },
        error: function () {
            alert("Error al realizar la solicitud para cargar especialidades.");
        }
    });
}

function llenarSelectEspecialidades(especialidades) {
    const selectEspecialidadFiltro = $('#inputSelectEspecialidad');
    const selectEspecialidadModal = $('#inputSelectEspecialidadModal');

    // Limpia los select antes de llenarlos
    selectEspecialidadFiltro.empty().append('<option value="">Todos</option>');
    selectEspecialidadModal.empty().append('<option value="">Seleccione una especialidad</option>');

    especialidades.forEach(especialidad => {
        const optionHtml = `<option value="${especialidad.EspecialidadCodigo}">${especialidad.EspecialidadNombre}</option>`;
        selectEspecialidadFiltro.append(optionHtml);
        selectEspecialidadModal.append(optionHtml);
    });
}

function cargarMedicosPorEspecialidad(especialidadSeleccionada) {
    $.ajax({
        url: '/GestionarCitas/ListarMedicosConEspecialidad',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.consultaExitosa) {
                llenarSelectMedicos(response.data, especialidadSeleccionada);
            } else {
                alert(`Error al cargar médicos: ${response.mensaje}`);
            }
        },
        error: function () {
            alert("Error al realizar la solicitud para cargar médicos.");
        }
    });
}

function llenarSelectMedicos(medicos, especialidadSeleccionada) {
    const selectMedico = $('#inputSelectMedicoModal');
    const SelectMedicoFiltler = $('#inputSelectMedico');
    selectMedico.empty().append('<option value="">Seleccione un médico</option>');
    SelectMedicoFiltler.empty().append('<option value="">Médicos</option>');
    const medicosFiltrados = medicos.filter(medico =>
        medico.EspecialidadNombre.trim() === especialidadSeleccionada.trim()
    );

    if (medicosFiltrados.length > 0) {
        medicosFiltrados.forEach(medico => {
            const optionHtml = `<option value="${medico.MedicoCodigo.trim()}">${medico.MedicoNombre}</option>`;
            selectMedico.append(optionHtml);
            SelectMedicoFiltler.append(optionHtml);
        });
    }
}

function limpiarSelectMedico() {
    $('#inputSelectMedicoModal')
        .empty()
        .append('<option value="">Seleccione un médico</option>');

}
function limpiarSelectMedicoFiltro() {
    $('#inputSelectMedico')
        .empty()
        .append('<option value="">Médicos</option>');
}


function cargarEstados() {
    const estados = [
        { valor: 'P', texto: 'Pendiente' },
        { valor: 'C', texto: 'Confirmada' },
        { valor: 'X', texto: 'Cancelada' },
        { valor: '', texto: 'Desconocido' }
    ];

    const selectEstados = $('#inputSelectEstado');
    selectEstados.empty().append('<option value="">Todos</option>');

    estados.forEach(estado => {
        selectEstados.append(`<option value="${estado.valor}">${estado.texto}</option>`);
    });
}

function cargarTipoConsulta() {
    $.ajax({
        url: '/GestionarCitas/ListarTiposDeConsulta',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.consultaExitosa) {
                llenarSelectTipoConsulta(response.data);
            } else {
                alert(`Error al cargar tipos de consulta: ${response.mensaje}`);
            }
        },
        error: function () {
            alert("Error al realizar la solicitud para cargar tipos de consulta.");
        }
    });
}

function llenarSelectTipoConsulta(tiposConsulta) {
    const selectTipoConsulta = $('#inputSelectTipoConsulta');
    selectTipoConsulta.empty().append('<option value="">Seleccionar ...</option>');

    tiposConsulta.forEach(tipo => {
        const optionHtml = `<option value="${tipo.TipoConsultaCodigo}">${tipo.TipoConsultaDescripcion}</option>`;
        selectTipoConsulta.append(optionHtml);
    });
}

$(document).ready(function () {
    cargarEspecialidades();
    cargarEstados();
    cargarTipoConsulta();
});

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

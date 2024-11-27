$(document).ready(function () {
    inicializarFormularioActualizarPaciente();

    $('#tabla_pacientes').on('click', '.btn-editar', function () {
        const paciente = $('#tabla_pacientes').DataTable().row($(this).closest('tr')).data(); 
        abrirFormModalPaciente(paciente); 
    });
});

/**
 * Abre el modal y llena los datos del paciente seleccionado.
 * @param {Object} paciente - Datos del paciente.
 */
function abrirFormModalPaciente(paciente) {
    $('#mostrarPacienteCodigo').text(paciente.PacienteCodigo);
    $('#mostrarPacienteEstado').text(paciente.PacienteEstado);
    $('#PacienteNombreCompleto').text(paciente.PacienteNombreCompleto);
    const estadoSpan = $('#mostrarPacienteEstado');
    if (paciente.PacienteEstado === "Activo") {
        estadoSpan.removeClass('bg-danger').addClass('bg-success');
    } else {
        estadoSpan.removeClass('bg-success').addClass('bg-danger');
    }
    $('#mostrarPacienteFechaNacimiento').text(paciente.PacienteFechaNacimiento || 'No disponible');
    $('#mostrarPacienteSeguro').text(paciente.PacienteSeguro || 'Sin seguro');
    $('#mostrarPacienteDNI').text(paciente.PacienteDNI || 'No disponible');
    $('#actualizarPacienteNombreCompleto').val(paciente.PacienteNombreCompleto);
    $('#actualizarPacienteDireccion').val(paciente.PacienteDireccion || '');
    $('#actualizarPacienteTelefono').val(paciente.PacienteTelefono || '');
    $('#actualizarPacienteCorreo').val(paciente.PacienteCorreoElectronico || '');
    $('#modalActualizarPaciente').modal('show');
}

function inicializarFormularioActualizarPaciente() {
    $('#formActualizarPaciente').on('submit', function (event) {
        event.preventDefault(); 

        const paciente = obtenerDatosFormularioActualizar();
        if (!validarDatosActualizarPaciente(paciente)) {
            return;
        }
        enviarDatosActualizarPaciente(paciente);
    });
}

/**
 * Captura los datos del formulario de actualización.
 * @returns {Object} Datos del paciente a actualizar.
 */
function obtenerDatosFormularioActualizar() {
    return {
        PacienteCodigo: $('#mostrarPacienteCodigo').text().trim(),
        PacienteNombreCompleto: $('#actualizarPacienteNombreCompleto').val().trim(),
        PacienteDireccion: $('#actualizarPacienteDireccion').val().trim(),
        PacienteTelefono: $('#actualizarPacienteTelefono').val().trim(),
        PacienteCorreoElectronico: $('#actualizarPacienteCorreo').val().trim()
    };
}

/**
 * Valida los datos del formulario de actualización.
 * @param {Object} paciente - Datos del paciente a validar.
 * @returns {boolean} Verdadero si los datos son válidos, falso en caso contrario.
 */
function validarDatosActualizarPaciente(paciente) {
    if (!paciente.PacienteCodigo) {
        alert("El código del paciente no está definido.");
        return false;
    }

    if (!paciente.PacienteNombreCompleto) {
        alert("El nombre completo del paciente es obligatorio.");
        return false;
    }

    if (
        paciente.PacienteCorreoElectronico &&
        !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(paciente.PacienteCorreoElectronico)
    ) {
        alert("El correo electrónico no tiene un formato válido.");
        return false;
    }

    return true; // Los datos son válidos
}

/**
 * Envía los datos del formulario al servidor mediante AJAX para actualizar el paciente.
 * @param {Object} paciente - Datos del paciente a enviar.
 */
function enviarDatosActualizarPaciente(paciente) {
    $.ajax({
        url: actualizarPacienteUrl, 
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(paciente),
        success: function (response) {
            manejarRespuestaActualizarPaciente(response);
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el paciente:', error);
            alert('Ocurrió un error al intentar actualizar el paciente.');
        }
    });
}

/**
 * Maneja la respuesta del servidor después de la solicitud de actualización.
 * @param {Object} response - Respuesta del servidor.
 */
function manejarRespuestaActualizarPaciente(response) {
    if (response.transaccionExitosa) {
        alert(response.mensaje); // Mostrar mensaje de éxito
        $('#modalActualizarPaciente').modal('hide'); // Cerrar el modal
        $('#tabla_pacientes').DataTable().ajax.reload(); // Recargar la tabla
    } else {
        alert(`Error: ${response.mensaje}`); // Mostrar mensaje de error
    }
}

$(document).ready(function () {
    // Inicializar el manejo del formulario de actualización al cargar la página
    inicializarFormularioActualizarPaciente();
});

/**
 * Inicializa el evento submit del formulario de actualización.
 */
function inicializarFormularioActualizarPaciente() {
    $('#formActualizarPaciente').on('submit', function (event) {
        event.preventDefault(); // Evitar el envío por defecto del formulario

        // Capturar los datos del formulario
        const paciente = obtenerDatosFormularioActualizar();

        // Validar los datos antes de enviar
        if (!validarDatosActualizarPaciente(paciente)) {
            return; // Detener el proceso si los datos no son válidos
        }

        // Enviar los datos al servidor mediante AJAX
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
        url: actualizarPacienteUrl, // URL generada desde Razor
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

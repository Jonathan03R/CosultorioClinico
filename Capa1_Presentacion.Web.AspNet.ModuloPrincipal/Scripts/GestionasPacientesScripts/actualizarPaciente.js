$(document).ready(function () {
    inicializarFormularioActualizarPaciente();

    $('#tabla_pacientes').on('click', '.btn-editar', function () {
        const paciente = $('#tabla_pacientes').DataTable().row($(this).closest('tr')).data(); 
        abrirFormModalPaciente(paciente); 
    });

    $('#listaContactosEmergencia').on('click', '.contacto-item', function () {
        const contacto = $(this).data('contacto');
        abrirFormModalContacto(contacto);
    });

    inicializarFormularioActualizarContacto();
});


function inicializarFormularioActualizarContacto() {
    $('#formActualizarContacto').on('submit', function (event) {
        event.preventDefault();

        const contacto = obtenerDatosFormularioActualizarContacto();
        if (!validarDatosActualizarContacto(contacto)) {
            return;
        }
        enviarDatosActualizarContacto(contacto);
    });
}

function abrirFormModalContacto(contacto) {
    $('#actualizarContactoCodigo').val(contacto.ContactoEmergenciaCodigo.trim());
    $('#actualizarContactoNombre').val(contacto.ContactoEmergenciaNombre);
    $('#actualizarContactoTelefono').val(contacto.ContactoEmergenciaTelefono);
    $('#actualizarContactoRelacion').val(contacto.ContactoEmergenciaRelacion);

    $('#modalActualizarContacto').modal('show');
}

function obtenerDatosFormularioActualizarContacto() {
    return {
        ContactoEmergenciaCodigo: $('#actualizarContactoCodigo').val().trim(),
        ContactoEmergenciaNombre: $('#actualizarContactoNombre').val().trim(),
        ContactoEmergenciaTelefono: $('#actualizarContactoTelefono').val().trim(),
        ContactoEmergenciaRelacion: $('#actualizarContactoRelacion').val().trim()
    };
}

function validarDatosActualizarContacto(contacto) {
    if (!contacto.ContactoEmergenciaCodigo) {
        alert("El código del contacto no está definido.");
        return false;
    }

    if (!contacto.ContactoEmergenciaNombre) {
        alert("El nombre del contacto es obligatorio.");
        return false;
    }

    if (!contacto.ContactoEmergenciaTelefono) {
        alert("El teléfono del contacto es obligatorio.");
        return false;
    }

    if (!contacto.ContactoEmergenciaRelacion) {
        alert("La relación con el paciente es obligatoria.");
        return false;
    }

    return true;
}

function enviarDatosActualizarContacto(contacto) {
    $.ajax({
        url: actualizarContactoUrl,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(contacto),
        success: function (response) {
            manejarRespuestaActualizarContacto(response);
        },
        error: function (xhr, status, error) {
            console.error('Error al actualizar el contacto:', error);
            alert('Ocurrió un error al intentar actualizar el contacto.');
        }
    });
}


function manejarRespuestaActualizarContacto(response) {
    if (response.transaccionExitosa) {
        alert(response.mensaje); // Mostrar mensaje de éxito
        $('#modalActualizarContacto').modal('hide'); // Cerrar el modal
        // Recargar la lista de contactos de emergencia
        const pacienteCodigo = $('#mostrarPacienteCodigo').text().trim();
        cargarContactosEmergencia(pacienteCodigo);
    } else {
        alert(`Error: ${response.mensaje}`); // Mostrar mensaje de error
    }
}

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

    cargarContactosEmergencia(paciente.PacienteCodigo);

    $('#actualizarPacienteNombreCompleto').val(paciente.PacienteNombreCompleto);
    $('#actualizarPacienteDireccion').val(paciente.PacienteDireccion || '');
    $('#actualizarPacienteTelefono').val(paciente.PacienteTelefono || '');
    $('#actualizarPacienteCorreo').val(paciente.PacienteCorreoElectronico || '');
    $('#modalActualizarPaciente').modal('show');
}

function cargarContactosEmergencia(pacienteCodigo) {
    $.ajax({
        url: listarContactosUrl,
        type: 'GET',
        data: { pacienteCodigo: pacienteCodigo },
        success: function (response) {
            if (response.success) {
                mostrarContactosEmergencia(response.data);
            } else {
                console.error('Error al obtener contactos de emergencia:', response.message);
                $('#listaContactosEmergencia').html('<li class="list-group-item">No se pudieron cargar los contactos de emergencia.</li>');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error en la solicitud AJAX:', error);
            $('#listaContactosEmergencia').html('<li class="list-group-item">No se pudieron cargar los contactos de emergencia.</li>');
        }
    });
}

function mostrarContactosEmergencia(contactos) {
    var $lista = $('#listaContactosEmergencia');
    $lista.empty(); // Limpiamos la lista

    if (contactos.length > 0) {
        contactos.forEach(function (contacto) {
            var $item = $('<li class="list-group-item d-flex justify-content-between align-items-center contacto-item" style="cursor: pointer;"></li>');
            $item.data('contacto', contacto); // Almacenar datos del contacto en el elemento
            $item.append('<div><strong>' + contacto.ContactoEmergenciaNombre + '</strong> (' + contacto.ContactoEmergenciaRelacion + ')</div>');
            $item.append('<div>Teléfono: ' + contacto.ContactoEmergenciaTelefono + '</div>');
            $lista.append($item);
        });
    } else {
        $lista.append('<li class="list-group-item">No hay contactos de emergencia registrados.</li>');
    }
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

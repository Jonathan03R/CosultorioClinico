
//Este script manejará el formulario para agregar y actualizar pacientes.
$(document).ready(function () {
    $('#formAgregarPaciente').on('submit', function (e) {
        e.preventDefault();
        if (appState.contactosEmergencia.length === 0) {
            alert('Debe agregar al menos un contacto de emergencia.');
            return;
        }
        var paciente = obtenerDatosPaciente();
        enviarFormularioPaciente(paciente);
    });
});

function obtenerDatosPaciente() {
    var pacienteNombreCompleto = ($('#PacienteApellidos').val().trim() + " " + $('#PacienteNombres').val().trim()).toUpperCase();
    return {
        PacienteDNI: $('#PacienteDNI').val(),
        PacienteNombreCompleto: pacienteNombreCompleto,
        PacienteFechaNacimiento: $('#PacienteFechaNacimiento').val(),
        PacienteDireccion: $('#PacienteDireccion').val(),
        PacienteTelefono: $('#PacienteTelefono').val(),
        PacienteCorreoElectronico: $('#PacienteCorreoElectronico').val(),
        PacienteEstado: $('#PacienteEstado').val()
    };
}

function enviarFormularioPaciente(paciente) {
    $.ajax({
        url: urlRegistrarPaciente,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ Paciente: paciente, ContactoEmergencia: appState.contactosEmergencia }),
        success: function (response) {
            if (response.transaccionExitosa) {
                alert(response.mensaje);
                resetearFormularioPaciente();
                $('#tabla_pacientes').DataTable().ajax.reload();
            } else {
                alert('Error: ' + response.mensaje);
            }
        },
        error: function () {
            alert('Ocurrió un error al registrar el paciente y los contactos de emergencia.');
        }
    });
}

function resetearFormularioPaciente() {
    $('#modalAgregarPaciente').modal('hide');
    $('#formAgregarPaciente')[0].reset();
    $('#contactosEmergenciaLista').empty();
    appState.contactosEmergencia.length = 0;
}

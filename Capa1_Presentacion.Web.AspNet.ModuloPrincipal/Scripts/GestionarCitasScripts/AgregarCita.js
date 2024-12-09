// AgregarCita.js

$(document).ready(function () {
    inicializarEventosCita();
});

function inicializarEventosCita() {
    $('#btnGuardarCita').on('click', function (e) {
        e.preventDefault();
        guardarCita();
    });

    $('#ModalNuevaCita').on('show.bs.modal', function () {
        reiniciarFormularioCita();
    });
}

function guardarCita() {
    var citaData = obtenerDatosFormularioCita();
    console.log(citaData);
    if (!validarDatosCita(citaData)) {
        alert('Por favor, complete todos los campos requeridos.');
        return;
    }
    $.ajax({
        url: urlRegistrarCita, 
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(citaData),
        success: function (response) {
            if (response.transaccionExitosa) {
                alert(response.mensaje);
                $('#ModalNuevaCita').modal('hide');
                table.ajax.reload(null, false);

            } else {
                alert('Error al registrar la cita: ' + response.mensaje);
            }
        },
        error: function (xhr, status, error) {
            alert('Error al realizar la solicitud: ' + error);
        }
    });
}

//aqui obtengo el valor de cada input
function obtenerDatosFormularioCita() {
    return {
        CitaFechaHora: $('#inputFechaHora').val(),
        PacienteCodigo: $('#pacienteCodigo').text(),
        MedicoCodigo: $('#inputSelectMedicoModal').val(),
        TipoConsultaCodigo: $('#inputSelectTipoConsulta').val()
    };

}

function validarDatosCita(citaData) {
    return citaData.CitaFechaHora &&
           citaData.PacienteCodigo &&
           citaData.MedicoCodigo &&
           citaData.TipoConsultaCodigo;
}

function reiniciarFormularioCita() {
    $('#formCita')[0].reset(); 
    $('#datosPaciente').collapse('hide');
    limpiarSelectMedico(); 
}

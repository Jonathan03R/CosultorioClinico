/* BEGIN EXTERNAL SOURCE */

$(document).ready(function () {
    function enviarDetallesConsulta() {
        const detallesConsulta = {
            DetallesConsultaMotivoConsulta: $('#motivoConsulta').val(),
            DetallesConsultaHistoriaEnfermedad: $('#historiaEnfermedades').val(),
            DetallesConsultaRevisiones: $('#revisionSistema').val(),
            DetallesConsultaEvaluacionPsico: $('#evaluacionPsicologica').val(),
            CodigoConsulta: $('#CodigoConsultaInput').val(),
        };
        console.log('Detalles de la consulta capturados:', detallesConsulta);  // Punto de depuraci�n

        $.ajax({
            url: '/AtenderConsultas/RegistrarDetallesConsulta',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(detallesConsulta),
            success: function (response) {
                console.log('Respuesta del servidor:', response);  // Punto de depuraci�n
                if (response.transaccionExitosa) {
                    alert(response.mensaje);
                    mostrarDetallesRegistrados(detallesConsulta);
                } else {
                    alert('Error: ' + response.mensaje);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error al registrar los detalles:', error);  // Punto de depuraci�n
                alert("Error al registrar los detalles.");
            }
        });
    }

    function mostrarDetallesRegistrados(detallesConsulta) {
        const detallesHTML = `
            <div class="card mt-3">
                <div class="card-body">
                    <h5 class="card-title">Detalles Registrados</h5>
                    <p><strong>Motivo de la Consulta:</strong> ${detallesConsulta.DetallesConsultaMotivoConsulta}</p>
                    <p><strong>Historia de Enfermedades:</strong> ${detallesConsulta.DetallesConsultaHistoriaEnfermedad}</p>
                    <p><strong>Revisi�n por el Sistema:</strong> ${detallesConsulta.DetallesConsultaRevisiones}</p>
                    <p><strong>Evaluaci�n Psicol�gica:</strong> ${detallesConsulta.DetallesConsultaEvaluacionPsico}</p>
                </div>
            </div>
        `;
        $('#detallesRegistrados').html(detallesHTML);
    }

    // Aseg�rate de que el formulario no se env�e por defecto
    $('#formularioConsulta').on('submit', function (e) {
        e.preventDefault();  // Esto debe evitar que la p�gina se recargue
        enviarDetallesConsulta();  // Llamada a la funci�n para enviar los datos
    });
});

/* END EXTERNAL SOURCE */

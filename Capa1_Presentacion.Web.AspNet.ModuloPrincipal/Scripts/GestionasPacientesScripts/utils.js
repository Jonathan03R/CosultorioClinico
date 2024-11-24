//Este script contendrá funciones utilitarias que pueden ser reutilizadas en otros scripts.
function toggleDetallesFila(table, elemento) {
    var tr = elemento.closest('tr');
    var row = table.row(tr);
    var icon = elemento.find('.toggle-icon');

    if (row.child.isShown()) {
        // Contraer
        row.child().find('.expansion-content').slideUp(400, function () {
            row.child.hide();
            tr.removeClass('shown');
            icon.removeClass('fa-minus-circle text-danger').addClass('fa-plus-circle text-success');
        });
    } else {
        // Expandir
        row.child(formatearDetalles(row.data())).show();


        row.child().find('.expansion-content').hide().slideDown(400);
        tr.addClass('shown');
        icon.removeClass('fa-plus-circle text-success').addClass('fa-minus-circle text-danger');
    }
}

function formatearDetalles(data) {
    const estadoClase = data.PacienteEstado === "Activo" ? "bg-success" : "bg-danger";
    const fechaActivacion = data.PacienteFechaActivacion || "Fecha no disponible";
    const notasAdicionales = data.PacienteNotas || "No hay notas adicionales";

    return `
        <div class="expansion-content">
            <div class="contenido">
                <p><b>${data.PacienteNombreCompleto}</b> se activó el día ${fechaActivacion}.</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.PacienteEstado}</span></p>
                <p>Notas adicionales: ${notasAdicionales}</p>
                <p>
                    ¿Quieres actualizar al paciente? 
                    <button class="btn btn-link p-0" onclick='abrirFormModalPaciente(${JSON.stringify(data)})'>Actualizar</button>
                </p>
            </div>
        </div>`;
}



function abrirFormModalPaciente(paciente) {

    console.log('Paciente seleccionado:', paciente); 
    // Datos de solo lectura
    $('#mostrarPacienteCodigo').text(paciente.PacienteCodigo);
    $('#mostrarPacienteEstado').text(paciente.PacienteEstado);
    $('#mostrarPacienteFechaNacimiento').text(paciente.PacienteFechaNacimiento || 'No disponible');
    $('#mostrarPacienteDNI').text(paciente.PacienteDNI || 'No disponible');
    $('#mostrarPacienteSeguro').text(paciente.PacienteSeguro || 'Sin seguro');

    // Datos editables
    $('#actualizarPacienteNombreCompleto').val(paciente.PacienteNombreCompleto);
    $('#actualizarPacienteDireccion').val(paciente.PacienteDireccion || '');
    $('#actualizarPacienteTelefono').val(paciente.PacienteTelefono || '');
    $('#actualizarPacienteCorreo').val(paciente.PacienteCorreoElectronico || '');

    // Abrir el modal
    $('#modalActualizarPaciente').modal('show');
}
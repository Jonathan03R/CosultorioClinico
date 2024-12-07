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
    // Ajustar las claves para coincidir con los datos correctos
    const estadoClase = obtenerClaseEstado(data.ConsultaEstado);
    const fechaActivacion = data.ConsultaFechaCita || "Error al cargar fecha";

    return `
        <div class="expansion-content">
            <div class="contenido">
                <p><b>${data.PacienteNombre}</b> Tiene cita ${fechaActivacion}.</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
                <p>Motivo de la consulta : ${data.ConsultaMotivo}</p>
                <p>
                    ¿Quieres Iniciar la consulta? 
                    <button class="btn btn-link p-0" onclick='abrirFormModalConsulta(${JSON.stringify(data)})'>Iniciar</button>
                </p>
            </div>
        </div>`;
}


function obtenerClaseEstado(estado) {
    switch (estado) {
        case "Pendiente": return "bg-warning";
        case "No asistio": return "bg-secondary";
        case "Atendido": return "bg-success";
        case "Cancelado": return "bg-danger";
        default: return "bg-dark"; // Clase para estado desconocido 
    } 
}


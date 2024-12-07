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
    const estadoClase = obtenerClaseEstado(data.ConsultaEstado);
    const fechaActivacion = data.ConsultaFechaCita || "Error al cargar fecha";

    let detallesHTML = `
        <div class="expansion-content">
            <div class="contenido">
                <p><b>${data.PacienteNombre}</b> `;

    switch (data.ConsultaEstado) {
        case "Pendiente":
            detallesHTML += `Tiene cita ${fechaActivacion}.</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
                <p>Motivo de la consulta : ${data.ConsultaMotivo}</p>
                <p>
                    ¿Quieres Iniciar la consulta? 
                    <button class="btn btn-link p-0" onclick='abrirFormModalConsulta(${JSON.stringify(data)})'>Iniciar</button>
                </p>
            `;
            break;
        case "No asistio":
            detallesHTML += `Su cita fue para la fecha ${fechaActivacion}.</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
                <p>Motivo de la consulta : ${data.ConsultaMotivo}</p>
                <p>
                    ¿Quieres reprogramar la cita? 
                    <button class="btn btn-link p-0" onclick='reprogramarCita(${JSON.stringify(data)})'>Reprogramar</button>
                </p>
            `;
            break;
        case "Atendido":
            detallesHTML += `Fue atendido el ${fechaActivacion}.</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
                <p>Motivo de la consulta : ${data.ConsultaMotivo}</p>
                <p>Notas adicionales: ${data.NotasAdicionales || "Sin notas adicionales"}</p>
            `;
            break;
        case "Cancelado":
            detallesHTML += `La consulta ha sido cancelada.</p>
                <p>Motivo de la cancelación : ${data.ConsultaMotivo}</p>
            `;
            break;
        default:
            detallesHTML += `Estado desconocido. Por favor, contacte al soporte.</p>
            `;
            break;
    }

    detallesHTML += `</div></div>`;
    return detallesHTML;
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


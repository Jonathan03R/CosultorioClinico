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
    console.log(data)
    let detallesHTML = `
        <div class="expansion-content">
            <div class="contenido">
                <p><b>${data.PacienteNombre}</b> `;

    switch (data.ConsultaEstado) {
        case "Pendiente":
            detallesHTML += `Tiene cita ${fechaActivacion} a las  ${data.ConsultaHoraFecha}</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
                
                <p>
                    ¿Quieres Iniciar la consulta? 
                   <button class="btn btn-link p-0" onclick='mostrarModalConfirmacion(${JSON.stringify(data)})'>Iniciar</button>
                </p>
            `;
            break;
        case "No asistio":
            detallesHTML += `Su cita fue para la fecha ${fechaActivacion}.</p>
                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
                <p>
                    ¿Quieres reprogramar la cita? 
                    <button class="btn btn-link p-0" onclick='reprogramarCita(${JSON.stringify(data)})'>Reprogramar</button>
                </p>
            `;
            break;
        case "Atendido":
            detallesHTML += `La cita se atendio el ${fechaActivacion}  las ${data.ConsultaHoraFecha}</p>
                <p>Finalizo el ${data.ConsultaFechaHoraFinal} </p>

                <p>Estado actual: <span class="badge ${estadoClase}">${data.ConsultaEstado}</span></p>
            `;
            break;
        case "Cancelado":
            detallesHTML += `La consulta ha sido cancelada.</p>
            `;
            break;
        case "Atendiendo":
            detallesHTML += `Esta Consulta esta actualmente atendiensoe</p>
                <p>Responsable: ${data.MedicoNombre}</p>
                 <p>
                    ¿Quieres reprogramar la cita?
                    <button class="btn btn-link p-0" onclick='ContinuarCita(${JSON.stringify(data)})'>Continuar</button>
                </p>
               
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


function mostrarModalConfirmacion(data) {
    console.log('Datos recibidos para la consulta:', data);  // Mensaje de depuración

    $('#confirmacionModal').modal('show');
    configurarBotonConfirmacion(data);
}

function configurarBotonConfirmacion(data) {
    $('#confirmarIniciarConsulta').off('click').on('click', function () {
        console.log('Continuando con la consulta:', data.HistoriaClinica); 
        console.log('Valor de consultaCodigo:', data.CitaCodigo); 
        actualizarEstadoConsultaprocesandoCita(data);
    });
}

//es cuando el estado esta en pendiente , entonces podre comenzar ua cita
function actualizarEstadoConsultaprocesandoCita(data) {
    // Llamar al endpoint para actualizar el estado
    $.ajax({
        url: '/AtenderConsultas/ActualizarEstadoConsultaProceso',
        type: 'POST',
        data: {
            citaCodigo: data.CitaCodigo,
        },
        success: function (response) {
            if (response.transaccionExitosa) {
                sessionStorage.setItem('consultaData', JSON.stringify(data));
                window.location.href = '/AtenderConsultas/Atendiendo';
            } else {
                alert('Error al actualizar el estado: ' + response.mensaje);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error en la solicitud AJAX:', textStatus, errorThrown);  // Mensaje de depuración
            alert('Error al actualizar el estado.');
        }
    });
}

//es cuando la cita es en estado atendiendo.... recupera los datos anteriores
function ContinuarCita(data) {
    console.log('Continuando con la consulta:', data.HistoriaClinica); 
    console.log('Continuando con la consulta:', data.CitaCodigo); 
    sessionStorage.setItem('consultaData', JSON.stringify(data));
    window.location.href = '/AtenderConsultas/Atendiendo';
}


function obtenerClaseEstado(estado) {
    switch (estado) {
        case "Pendiente": return "bg-warning";
        case "No asistio": return "bg-secondary";
        case "Atendido": return "bg-success";
        case "Cancelado": return "bg-danger";
        case "Atendiendo": return "custom-bg-yellow";
        default: return "bg-dark"; // Clase para estado desconocido 
    } 
}



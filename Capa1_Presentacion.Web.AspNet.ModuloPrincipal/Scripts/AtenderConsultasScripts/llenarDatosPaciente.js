function llenarDatosPaciente() {
    var data = JSON.parse(sessionStorage.getItem('consultaData'));

    if (data && data.HistoriaClinica) {
        console.log('data', data);
        $('#HistoriaClinica').text(data.HistoriaClinica || 'No disponible');
        $('#fechaCita').text(data.ConsultaFechaCita || 'No disponible');
        $('#Hora').text(data.ConsultaHoraFecha || 'No disponible');
        $('#medico').text(data.MedicoNombre || 'No disponible'); 
        $('#CodigoConsulta').text(data.CitaCodigo || 'No disponible');
        $('#CodigoConsultaInput').val(data.ConsultaCodigo);


        obtenerDatosConsulta(data.HistoriaClinica);
    } else {
        alert('No se encontró la información necesaria para completar la solicitud.');
    } 
}

function obtenerDatosConsulta(HistoriaCodigo) {
    $.ajax({
        url: '/AtenderConsultas/ObtenerDetallesConsulta',
        type: 'POST',
        data: {
            HistorialClinicoCodigo: HistoriaCodigo,
        },
        success: function (response) {
            if (response.transaccionExitosa) {

                response.data.forEach(consulta => {
                    obtenerDatosPaciente(consulta.Paciente.pacienteCodigo);
                    CitasAnteriores(consulta.Paciente.pacienteCodigo);
                    mostrarRecetas(consulta.recetas);
                });
            } else {
                alert(`Error: ${response.mensaje}`);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error en la solicitud AJAX:', textStatus, errorThrown);
            alert('Error al obtener los datos de la consulta.');
        }
    });
}

function mostrarRecetas(recetas) {
    const listaRecetas = document.getElementById("listaRecetas");
    listaRecetas.innerHTML = '';  // Limpiar la lista antes de agregar nuevas recetas

    if (recetas.length === 0) {
        listaRecetas.innerHTML = `
            <li class="list-group-item text-center text-muted">
                ¡Vaya! No hay recetas disponibles para esta consulta. Tal vez el médico no prescribió ningún medicamento.
            </li>
        `;
    } else {
        recetas.forEach(receta => {
            const recetaHTML = `
                <li class="list-group-item d-flex justify-content-between align-items-center shadow-sm mb-3">
                    <div class="d-flex flex-column">
                        <strong>${receta.recetaDescripcion}</strong>
                        <small class="text-muted">Tratamiento: ${receta.recetaTratamiento}</small>
                        <p class="mt-2 mb-1"><strong>Recomendaciones:</strong> ${receta.recetaRecomendaciones}</p>
                    </div>
                    <span class="badge bg-success text-light">Código: ${receta.recetaCodigo}</span>
                </li>
            `;
            listaRecetas.innerHTML += recetaHTML;
        });
    }
}

function obtenerDatosPaciente(pacienteCodigo) {
    $.ajax({
        url: '/AtenderConsultas/ObtenerDatosPaciente',
        type: 'POST',
        data: { pacienteCodigo: pacienteCodigo },
        success: function (response) {
            if (response.transaccionExitosa) {
                const paciente = response.data;
                // Aquí se actualizan los elementos del DOM con los datos del paciente
                $('#nombrePaciente').text(paciente.nombreCompleto || 'No disponible');
                $('#edad').text(`Edad: ${paciente.edad || 'No disponible'}`);
                $('#FechaNacimiento').text(`Fecha de Nacimiento: ${paciente.fechaNacimiento || 'No disponible'}`);
                $('#Dni').text(`DNI: ${paciente.dni || 'No disponible'}`);
                $('#direccion').text(`Dirección: ${paciente.direccion || 'No disponible'}`);
                $('#correo').text(`Correo: ${paciente.email || 'No disponible'}`);
                $('#celular').text(`Celular: ${paciente.telefono || 'No disponible'}`);
            } else {
                alert('Error: ' + response.mensaje);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error en la solicitud AJAX:', textStatus, errorThrown);
            alert('Error al obtener los datos del paciente.');
        }
    });
}

function CitasAnteriores(pacienteCodigo) {
    $.ajax({
        url: '/AtenderConsultas/ListarCitasPrevias',
        type: 'POST',
        data: { pacienteCodigo: pacienteCodigo },
        success: function (response) {
            if (response.consultaExitosa) {
                const citas = response.data;

                $('#listaCitas').empty(); // Limpiar la lista antes de agregar las citas

                if (citas.length === 0) {
                    // Si no hay citas previas, mostrar un mensaje creativo
                    $('#listaCitas').append(`
                        <li class="list-group-item text-center">
                            <span class="text-muted">¡No has tenido citas anteriores! 🎉</span>
                        </li>
                    `);
                } else {
                    citas.forEach(function (cita) {
                        const estado = cita.CitaEstado || 'No disponible';
                        const fecha = cita.CitaFecha || 'No disponible';
                        const codigo = cita.CitaCodigo || 'No disponible';
                        const doctor = cita.MedicoNombre || 'No disponible';

                        const citaHTML = `
                            <li class="list-group-item d-flex justify-content-between align-items-center shadow-sm mb-3">
                                <div>
                                    <span class="badge bg-warning text-dark">${estado}</span>
                                    <span class="text-primary ms-2">${fecha}</span>
                                    <p class="mt-2 mb-1"><strong>Cita Código:</strong> ${codigo}</p>
                                </div>
                                <div>
                                    <p class="h6 mb-0">${doctor}</p>
                                </div>
                            </li>
                        `;

                        $('#listaCitas').append(citaHTML);
                    });
                }
            } else {
                alert('Error: ' + response.mensaje);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error en la solicitud AJAX:', textStatus, errorThrown);
            alert('Error al obtener los datos de la cita.');
        }
    });
}

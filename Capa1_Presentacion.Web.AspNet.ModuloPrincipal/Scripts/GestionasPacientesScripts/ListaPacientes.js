$(document).ready(function () {
    var table = $('#tabla_pacientes').DataTable({
        "dom": "lrtip",
        "ajax": {
            "url": $('#tabla_pacientes').data("url-listar-pacientes"),
            "type": "GET",
            "dataSrc": function (json) {
                if (json.consultaExitosa) {
                    var processedData = json.data.map(function (item) {
                        return {
                            PacienteCodigo: item.PacienteCodigo,
                            PacienteNombreCompleto: item.PacienteNombreCompleto,
                            PacienteEstado: item.PacienteEstado,
                            PacienteHistorialClinicoCodigo: item.PacienteHistorialClinicoCodigo,
                            PacienteTelefono: item.PacienteTelefono,
                            PacienteSeguro: item.PacienteSeguro || 'Sin seguro',
                            PacienteFechaActivacion: item.PacienteFechaActivacion || 'Fecha no disponible',
                            PacienteNotas: item.PacienteNotas || 'No hay notas adicionales'
                        };
                    });
                    var totalCount = processedData.length;
                    var activeCount = processedData.filter(function (item) {
                        return item.PacienteEstado === 'Activo';
                    }).length;
                    var inactiveCount = processedData.filter(function (item) {
                        return item.PacienteEstado === 'Inactivo';
                    }).length;

                    // Actualizar los contadores en el DOM
                    $('#total-count').text(totalCount);
                    $('#active-count').text(activeCount);
                    $('#inactive-count').text(inactiveCount);
                    return processedData;

                } else {
                    alert(json.mensaje);
                    return [];
                }
            }
        },
        "columns": [
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": '<i class="fas fa-plus-circle text-success toggle-icon" style="cursor: pointer;"></i>',
                "width": "20px"
            },
            { "data": "PacienteCodigo" },
            {
                "data": "PacienteNombreCompleto",
                "render": function (data) {
                    return `<span class="nombre-paciente">${data}</span>`;
                }
            },
            {
                "data": "PacienteEstado",
                "render": function (data, type, row) {
                    if (type === 'display') {
                        // Aquí retornas el HTML personalizado para mostrar en la tabla
                        let estadoActual = data === "Activo" ? "Desactivar" : "Activar";
                        let estadoClase = data === "Activo" ? "badge bg-success" : "badge bg-danger";

                        return `
                            <div class="dropdown">
                                <span class="${estadoClase} dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false" style="cursor: pointer;">
                                    ${data}
                                </span>
                                <ul class="dropdown-menu">
                                    <li>
                                        <a class="dropdown-item cambiar-estado" href="#" data-id="${row.PacienteCodigo}" data-estado="${estadoActual}">
                                            ${estadoActual}
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        `;
                    } else {
                        // Para filtrado y ordenamiento, retornas el valor original de los datos
                        return data;
                    }
                }
            },
              //return data === "Activo" ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-danger">Inactivo</span>';
            {
                "data": "PacienteHistorialClinicoCodigo",
                "render": function (data) {
                    return `<span class="badge bg-light text-dark">${data}</span>`;
                }
            },
            { "data": "PacienteTelefono" },
            { "data": "PacienteSeguro" },
            //{ // Columna para el botón de acción
            //    "data": null,
            //    "orderable": false,
            //    "className": 'text-center',
            //    "defaultContent": '<button class="btn btn-danger btn-sm btn-eliminar">Eliminar</button>'
            //}
        ],
        "language": {
            "url": "//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json"
        },
        "responsive": false, 
        "ordering": false,
        "columnDefs": [
            { "targets": 0, "orderable": false },
            { "targets": 0, "className": "dt-center" }
        ]
    });

    $('#tabla_pacientes thead th').eq(0).html('');

    // Evento de clic para expandir/contraer detalles
    $('#tabla_pacientes tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var icon = $(this).find('.toggle-icon');

        if (row.child.isShown()) {
            // Contraer
            row.child().find('.expansion-content').slideUp(400, function () {
                row.child.hide();
                tr.removeClass('shown');
                icon.removeClass('fa-minus-circle text-danger').addClass('fa-plus-circle text-success');
            });
        } else {
            // Expandir
            row.child(format(row.data())).show();
            row.child().find('.expansion-content').hide().slideDown(400); // Inicia escondido y muestra con deslizamiento
            tr.addClass('shown');
            icon.removeClass('fa-plus-circle text-success').addClass('fa-minus-circle text-danger');
        }
    });

    // Función para definir el contenido expandido
    function format(data) {
        return `
                    <div class="expansion-content">
                        <div class="contenido">
                            <p><b>${data.PacienteNombreCompleto}</b> se activó el día ${data.PacienteFechaActivacion}.</p>
                            <p>Estado actual: <span class="badge bg-success">${data.PacienteEstado}</span></p>
                            <p>Notas adicionales: ${data.PacienteNotas}</p>
                            <p>¿Quieres actualizar al paciente? <a href="#" onclick='abrirFormModalPaciente(${JSON.stringify(data)})'>Actualizar</a></p>
                        </div>
                    </div>`;
    }

    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var searchTerm = $('#buscador-personalizado').val().toLowerCase();

            if (searchTerm === '') {
                return true;
            }

            var nombre = data[2].toLowerCase(); // Columna 'Nombre Pacientes'
            var expediente = data[4].toLowerCase(); // Columna 'Expediente'

            // Verificar si el nombre o expediente comienzan con el término de búsqueda
            if (nombre.startsWith(searchTerm) || expediente.startsWith(searchTerm)) {
                return true;
            }

            return false;
        }
    );

    // Buscador personalizado
    $('#buscador-personalizado').on('keyup', function () {
        table.draw();
    });


    $('#lista-pacientes-container .nav-link').on('click', function (e) {
        e.preventDefault();

        $('.nav-link').removeClass('active');
        $(this).addClass('active');

        var filterValue = $(this).data('filter');

        if (filterValue === 'all') {
            table.column(3).search('').draw(); 
        } else {
            table.column(3).search('^' + filterValue + '$', true, false).draw(); 
        }
    });

    $('#formAgregarPaciente').on('submit', function (e) {
        e.preventDefault();

        var pacienteNombreCompleto = ($('#PacienteApellidos').val().trim() + " " + $('#PacienteNombres').val().trim()).toUpperCase();
        var paciente = {
            PacienteDNI: $('#PacienteDNI').val(),
            PacienteNombreCompleto: pacienteNombreCompleto, 
            PacienteFechaNacimiento: $('#PacienteFechaNacimiento').val(),
            PacienteDireccion: $('#PacienteDireccion').val(),
            PacienteTelefono: $('#PacienteTelefono').val(),
            PacienteCorreoElectronico: $('#PacienteCorreoElectronico').val(),
            PacienteEstado: $('#PacienteEstado').val()
        };

        $.ajax({
            url: urlRegistrarPaciente,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(paciente),
            success: function (response) {
                if (response.transaccionExitosa) {
                    alert(response.mensaje);
                    $('#modalAgregarPaciente').modal('hide'); 
                    $('#formAgregarPaciente')[0].reset(); 
                    table.ajax.reload(); 
                } else {
                    alert("Error: " + response.mensaje);
                }
            },
            error: function () {
                alert("Ocurrió un error al registrar el paciente.");
            }
        });
    });

    // Evento para cambiar el estado
    $('#tabla_pacientes tbody').on('click', '.cambiar-estado', function (e) {
        e.preventDefault();

        let pacienteId = $(this).data('id');
        let nuevoEstado = $(this).data('estado') === "Activar" ? "Activo" : "Inactivo";

        if (confirm(`¿Estás seguro de que deseas ${nuevoEstado === "Activo" ? "activar" : "desactivar"} este paciente?`)) {
            $.ajax({
                url: '/GestionarPacientes/EliminarPaciente',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ PacienteCodigo: pacienteId }),
                success: function (response) {
                    if (response.transaccionExitosa) {
                        alert(response.mensaje);
                        table.ajax.reload();
                    } else {
                        alert("Error: " + response.mensaje);
                    }
                },
                error: function () {
                    alert("Ocurrió un error al actualizar el estado.");
                }
            });
        }
    });
    
});





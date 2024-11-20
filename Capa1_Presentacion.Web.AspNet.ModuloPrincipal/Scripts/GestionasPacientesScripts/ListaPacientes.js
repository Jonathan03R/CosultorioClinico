/*este script actualmente no se usando , 300 lineas aprox no es recomendado ,malas practicas xd, por eso cree mas archivos js para 
mejorar la legibilidad escalabilidad etc etc. 

    -- tablaPacientes.js: Configuración y manejo de la tabla de pacientes.
    -- formPacientes.js: Manejo del formulario para agregar y actualizar pacientes.
    -- contactosEmergencia.js: Gestión de contactos de emergencia.
    -- estadoPacientes.js: Control del cambio de estado de los pacientes.
    -- buscadorPacientes.js: Lógica del buscador personalizado y filtrado.
    -- utilidades.js: Funciones utilitarias que pueden ser reutilizadas.


este script lo deje por secaso no funcione algo , regreso aqui para ver que esta mal.
*/


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

    const contactosEmergencia = [];

    // Evitar cerrar el modal principal al abrir el secundario
    $('#btnAgregarContacto').on('click', function (e) {
        e.preventDefault();
        $('#modalAgregarContacto').modal('show');
    });

    // Cerrar solo el modal secundario
    $('.btnCerrarModalContacto').on('click', function () {
        $('#modalAgregarContacto').modal('hide');
    });

    // Manejo de datos del formulario de contacto
    $('#formAgregarContacto').on('submit', function (e) {
        e.preventDefault();

        const contacto = {
            ContactoEmergenciaNombre: $('#contactoNombre').val().trim(),
            ContactoEmergenciaRelacion: $('#contactoRelacion').val().trim(),
            ContactoEmergenciaTelefono: $('#contactoTelefono').val().trim()
        };

        if (contacto.ContactoEmergenciaNombre && contacto.ContactoEmergenciaRelacion && contacto.ContactoEmergenciaTelefono) {
            contactosEmergencia.push(contacto);
            actualizarListaContactos();
            $('#formAgregarContacto')[0].reset();
            $('#modalAgregarContacto').modal('hide');
        } else {
            alert('Completa todos los campos del contacto.');
        }
    });

    $('#contactosEmergenciaLista').on('click', '.btnEliminarContacto', function () {
        const index = $(this).data('index');
        contactosEmergencia.splice(index, 1);
        actualizarListaContactos();
    });

    function actualizarListaContactos() {
        $('#contactosEmergenciaLista').empty();
        contactosEmergencia.forEach((contacto, index) => {
            $('#contactosEmergenciaLista').append(`
                <div class="contacto-item d-flex justify-content-between align-items-center">
                    <p><strong>${contacto.ContactoEmergenciaRelacion}</strong>: ${contacto.ContactoEmergenciaTelefono}</p>
                    <button type="button" class="btn btn-danger btn-sm btnEliminarContacto" data-index="${index}">
                        Eliminar
                    </button>
                </div>
            `);
        });
    }


    $('#formAgregarPaciente').on('submit', function (e) {
        e.preventDefault();
        if (contactosEmergencia.length === 0) {
            alert('Debe agregar al menos un contacto de emergencia.');
            return; 
        }
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
            data: JSON.stringify({ Paciente: paciente, ContactoEmergencia: contactosEmergencia }),
            success: function (response) {
                if (response.transaccionExitosa) {
                    alert(response.mensaje);
                    // Cerrar el modal y resetear el formulario
                    $('#modalAgregarPaciente').modal('hide');
                    $('#formAgregarPaciente')[0].reset();
                    $('#contactosEmergenciaLista').empty();
                    contactosEmergencia.length = 0; // Limpiar el arreglo de contactos
                    table.ajax.reload();
                } else {
                    alert('Error: ' + response.mensaje);
                }
            },
            error: function () {
                alert('Ocurrió un error al registrar el paciente y los contactos de emergencia.');
            }
        });
    });

    // Evento para cambiar el estado
    $('#tabla_pacientes tbody').on('click', '.cambiar-estado', function (e) {
        e.preventDefault();

        let pacienteId = $(this).data('id');
        let accion = $(this).data('estado'); // "Activar" o "Desactivar"
        let accionNormalizada = accion.trim().toLowerCase();
        console.log("Valor de accion:", accionNormalizada);

        let url = accionNormalizada === "activar"
            ? '/GestionarPacientes/RecuperarPaciente'
            : '/GestionarPacientes/EliminarPaciente';

        if (confirm(`¿Estás seguro de que deseas ${accionNormalizada} este paciente?`)) {
            $.ajax({
                url: url,
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



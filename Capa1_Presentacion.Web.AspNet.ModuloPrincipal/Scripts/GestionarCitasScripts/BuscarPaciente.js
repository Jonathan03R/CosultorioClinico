﻿$(document).ready(function () {
    // Evento del botón de búsqueda
    $('#button-addon2').on('click', function () {
        const dni = $('#inputDniPaciente').val().trim();

        if (!dni) {
            alert('Por favor, ingresa un DNI válido.');
            return;
        }

        buscarPacientePorDni(dni);
    });

    // Evento para agregar un nuevo paciente
    $('#btnAgregarPaciente').on('click', function () {
        $('#modalAgregarPaciente').modal('hide');
        window.location.href = '/GestionarPacientes/ListaPacientes'; // Redirige a la página de registro
    });
});

/**
 * Realiza una solicitud AJAX para buscar al paciente por DNI.
 * @param {string} dni - DNI del paciente.
 */
function buscarPacientePorDni(dni) {
    $.ajax({
        url: '/GestionarCitas/ObtenerPacientePorDni', // Ruta al controlador
        type: 'GET',
        data: { dni: dni },
        success: function (response) {
            if (response.consultaExitosa && response.data) {
                // Llenar y mostrar los datos en el collapse
                mostrarDatosPaciente(response.data);
            } else {
                // Mostrar el modal de "Paciente no encontrado"
                $('#modalAgregarPaciente').modal('show');
            }
        },
        error: function (xhr, status, error) {
            alert('Ocurrió un error al buscar el paciente. Intenta nuevamente.');
            console.error(error);
        }
    });
}


function mostrarDatosPaciente(paciente) {
    // Llenar los datos en el collapse
    $('#pacienteNombre').text(paciente.PacienteNombreCompleto || 'N/A');
    const estado = paciente.PacienteEstado || 'Desconocido';

    const badgeClass = estado === 'Activo' ? 'bg-success' : 'bg-danger'; // Cambia la clase según el estado
    $('#pacienteEstado').text(estado) // Establecer el texto del estado
        .removeClass('bg-success bg-danger') // Elimina las clases anteriores
        .addClass(badgeClass); // Agrega la clase correcta (verde o rojo)

    // Guardar el código del paciente en un campo oculto
    $('#pacienteCodigo').text(paciente.PacienteCodigo);

    // Activar el collapse con Bootstrap
    $('#datosPaciente').collapse('show'); // Muestra el collapse
}
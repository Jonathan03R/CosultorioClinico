//Este script controlará la lógica para cambiar el estado de los pacientes.

$(document).ready(function () {
    $('#tabla_pacientes tbody').on('click', '.cambiar-estado', function (e) {
        e.preventDefault();
        cambiarEstadoPaciente($(this));
    });
});

function cambiarEstadoPaciente(elemento) {
    let pacienteId = elemento.data('id');
    let accion = elemento.data('estado');
    let accionNormalizada = accion.trim().toLowerCase();

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
                    $('#tabla_pacientes').DataTable().ajax.reload();
                } else {
                    alert("Error: " + response.mensaje);
                }
            },
            error: function () {
                alert("Ocurrió un error al actualizar el estado.");
            }
        });
    }
}

function renderizarEstadoPaciente(data, type, row) {
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

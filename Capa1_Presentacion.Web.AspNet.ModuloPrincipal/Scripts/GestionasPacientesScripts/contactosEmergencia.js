//Este script gestionará la lógica para agregar y eliminar contactos de emergencia.

const appState = {
    contactosEmergencia: []
};

$(document).ready(function () {
    inicializarEventosContactos();
});

function inicializarEventosContactos() {
    $('#btnAgregarContacto').on('click', function (e) {
        e.preventDefault();
        $('#modalAgregarContacto').modal('show');
    });

    $('.btnCerrarModalContacto').on('click', function () {
        $('#modalAgregarContacto').modal('hide');
    });

    $('#formAgregarContacto').on('submit', function (e) {
        e.preventDefault();
        agregarContactoEmergencia();
    });

    $('#contactosEmergenciaLista').on('click', '.btnEliminarContacto', function () {
        const index = $(this).data('index');
        eliminarContactoEmergencia(index);
    });
}

function agregarContactoEmergencia() {
    const contacto = {
        ContactoEmergenciaNombre: $('#contactoNombre').val().trim(),
        ContactoEmergenciaRelacion: $('#contactoRelacion').val().trim(),
        ContactoEmergenciaTelefono: $('#contactoTelefono').val().trim()
    };

    if (contacto.ContactoEmergenciaNombre && contacto.ContactoEmergenciaRelacion && contacto.ContactoEmergenciaTelefono) {
        appState.contactosEmergencia.push(contacto);
        actualizarListaContactos();
        $('#formAgregarContacto')[0].reset();
        $('#modalAgregarContacto').modal('hide');
    } else {
        alert('Completa todos los campos del contacto.');
    }
}

function eliminarContactoEmergencia(index) {
    appState.contactosEmergencia.splice(index, 1);
    actualizarListaContactos();
}

function actualizarListaContactos() {
    $('#contactosEmergenciaLista').empty();
    appState.contactosEmergencia.forEach((contacto, index) => {
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

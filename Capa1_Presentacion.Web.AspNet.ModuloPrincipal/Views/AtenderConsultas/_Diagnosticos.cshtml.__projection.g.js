/* BEGIN EXTERNAL SOURCE */

    $(document).ready(function () {
        let debounceTimer;

        // Filtrar datos mientras se escribe
        $('#terminoBusqueda').on('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                let termino = $(this).val().trim();

                if (termino === '') {
                    $('#resultadosBusqueda').empty();
                    return;
                }
                $.ajax({
                    url: '/Cie11/FiltrarDatos',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ termino: termino }),
                    success: function (response) {
                        if (response.success) {
                            $('#resultadosBusqueda').empty();
                            response.data.forEach(function (item) {
                                $('#resultadosBusqueda').append(`
                                    <tr>
                                        <td>${item.Codigo}</td>
                                        <td>${item.Titulo}</td>
                                        <td>
                                            <button type="button" class="btn btn-primary btnSeleccionar" data-codigo="${item.Codigo}" data-titulo="${item.Titulo}">
                                                Seleccionar
                                            </button>
                                        </td>
                                    </tr>
                                `);
                            });
                        } else {
                            $('#resultadosBusqueda').empty();
                            alert(response.message);
                        }
                    },
                    error: function () {
                        alert("Error al realizar la búsqueda.");
                    }
                });
            }, 300);
        });

        // Seleccionar código y descripción, limpiando HTML
        $(document).on('click', '.btnSeleccionar', function () {
            let codigo = $(this).data('codigo');
            let titulo = $(this).data('titulo');

            // Limpia las etiquetas HTML de la descripción
            let descripcionLimpia = $('<div>').html(titulo).text();

            // Asigna los valores limpios a los inputs correspondientes
            $('#cie11Codigo').val(codigo);
            $('#descripcionDiagnostico').val(descripcionLimpia);

            // Cierra el modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalSeleccionarCIE11'));
            modal.hide();
        });
    });

/* END EXTERNAL SOURCE */
/* BEGIN EXTERNAL SOURCE */

    $(document).ready(function () {
        let debounceTimer;

        // Filtrar datos mientras se escribe
        $('#terminoBusqueda').on('input', function () {
            clearTimeout(debounceTimer);
            debounceTimer = setTimeout(() => {
                let termino = $(this).val().trim();

                if (termino === '') {
                    $('#resultadosBusqueda').empty();
                    return;
                }
                $.ajax({
                    url: '/Cie11/FiltrarDatos',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ termino: termino }),
                    success: function (response) {
                        if (response.success) {
                            $('#resultadosBusqueda').empty();
                            response.data.forEach(function (item) {
                                $('#resultadosBusqueda').append(`
                                    <tr>
                                        <td>${item.Codigo}</td>
                                        <td>${item.Titulo}</td>
                                        <td>
                                            <button type="button" class="btn btn-primary btnSeleccionar" data-codigo="${item.Codigo}" data-titulo="${item.Titulo}">
                                                Seleccionar
                                            </button>
                                        </td>
                                    </tr>
                                `);
                            });
                        } else {
                            $('#resultadosBusqueda').empty();
                            alert(response.message);
                        }
                    },
                    error: function () {
                        alert("Error al realizar la búsqueda.");
                    }
                });
            }, 300);
        });

        // Seleccionar código y descripción, limpiando HTML
        $(document).on('click', '.btnSeleccionar', function () {
            let codigo = $(this).data('codigo');
            let titulo = $(this).data('titulo');

            // Limpia las etiquetas HTML de la descripción
            let descripcionLimpia = $('<div>').html(titulo).text();

            // Asigna los valores limpios a los inputs correspondientes
            $('#cie11Codigo').val(codigo);
            $('#descripcionDiagnostico').val(descripcionLimpia);

            // Cierra el modal
            const modal = bootstrap.Modal.getInstance(document.getElementById('modalSeleccionarCIE11'));
            modal.hide();
        });
    });

/* END EXTERNAL SOURCE */

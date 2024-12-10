$(document).ready(function () {
    // Función para manejar los clics en los enlaces del navbar
    function handleNavClick(event, contentContainerId) {
        event.preventDefault();
        const $navLinks = $(event.target).closest('ul').find('a.nav-link');
        $navLinks.removeClass('active');
        $(event.target).addClass('active');

        const contentId = $(event.target).data('content');
        const contentContainer = $('#' + contentContainerId);

        $.ajax({
            url: '/AtenderConsultas/' + contentId,
            type: 'GET',
            success: function (result) {
                contentContainer.html(result);

                // Llamar a la función para llenar los datos del paciente después de cargar el contenido
                llenarDatosPaciente();
            },
            error: function () {
                contentContainer.html('<p>Error al cargar el contenido</p>');
            }
        });
    }

    // Agregar eventos a los enlaces del primer navbar
    $('.links-nav:first .nav-link').on('click', function (event) {
        handleNavClick(event, 'navbar-content-top');
    });

    $('.links-nav:last .nav-link').on('click', function (event) {
        handleNavClick(event, 'navbar-content-bottom');
    });

    // Cargar contenido por defecto al cargar la página
    function loadDefaultContent() {
        // Cargar contenido por defecto para el primer navbar
        const defaultTopNavLink = $('.links-nav:first .nav-link.active').data('content');
        $.ajax({
            url: '/AtenderConsultas/' + defaultTopNavLink,
            type: 'GET',
            success: function (result) {
                $('#navbar-content-top').html(result);
                // Llamar a la función para llenar los datos del paciente cuando se carga el contenido
                llenarDatosPaciente();
            },
            error: function () {
                $('#navbar-content-top').html('<p>Error al cargar el contenido</p>');
            }
        });

        // Cargar contenido por defecto para el segundo navbar
        const defaultBottomNavLink = $('.links-nav:last .nav-link.active').data('content');
        $.ajax({
            url: '/AtenderConsultas/' + defaultBottomNavLink,
            type: 'GET',
            success: function (result) {
                $('#navbar-content-bottom').html(result);
            },
            error: function () {
                $('#navbar-content-bottom').html('<p>Error al cargar el contenido</p>');
            }
        });
    }

    loadDefaultContent();
});

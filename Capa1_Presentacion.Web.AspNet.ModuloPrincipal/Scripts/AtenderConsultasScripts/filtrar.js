$(document).ready(function () {
    inicializarFiltrosCitas();
});

function inicializarFiltrosCitas() {
    $('#sub-nav .nav-link').on('click', function (e) {
        e.preventDefault();

        $('#sub-nav .nav-link').removeClass('active');
        $(this).addClass('active');

        var filterValue = $(this).data('filter');
        console.log('Valor del filtro:', filterValue);  // Depuración
        var table = $('#tabla_consultas').DataTable();

        if (filterValue === 'all') {
            table.column(6).search('').draw();
        } else if (filterValue === 'Pendiente') {
            table.column(6).search('^(Pendiente|Atendiendo)$', true, false).draw();  // Filtrar tanto "Pendiente" como "Atendiendo"
        } else {
            table.column(6).search('^' + filterValue + '$', true, false).draw();
        }
    });
}


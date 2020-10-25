$(document).ready(function () {
    $('#one-column-table').DataTable({
        'columnDefs': [{ 'orderable': false, 'targets': 1 }]
    });
});

$(document).ready(function () {
    $('#three-columns-table').DataTable({
        'columnDefs': [{ 'orderable': false, 'targets': 3 }]
    });
});
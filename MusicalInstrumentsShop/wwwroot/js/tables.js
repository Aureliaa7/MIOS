
$(document).ready(function () {
    $('#three-columns-table').DataTable({
        'columnDefs': [{ 'orderable': false, 'targets': 3 }]
    });
});

$(document).ready(function () {
    $('#six-columns-table').DataTable({
        'columnDefs': [{ 'orderable': false, 'targets': 6 }]
    });
});
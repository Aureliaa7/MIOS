$(document).ready(function () {
    $('.custom-file-input').on("change", function () {
        var fileLabel = $(this).next('.custom-file-label');
        var files = $(this)[0].files;
        if (files.length > 1) {
            fileLabel.html(files.length + ' files selected');
        }
        else if (files.length == 1) {
            fileLabel.html(files[0].name);
        }
    });
});

function get_categories() {
    $.ajax({
        type: "GET",
        dataType: 'json',
        contextType: 'application/json',
        url: "../Categories/GetExistingCategories",

        success: function (result) {
            $("#category-field option").remove();
            console.log(result);
            $.each(result, function (index, item) {
                $("#category-field").append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function get_suppliers() {
    $.ajax({
        type: "GET",
        dataType: 'json',
        contextType: 'application/json',
        url: "../Suppliers/GetExistingSuppliers",

        success: function (result) {
            $("#supplier-field option").remove();
            console.log(result);
            $.each(result, function (index, item) {
                $("#supplier-field").append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}
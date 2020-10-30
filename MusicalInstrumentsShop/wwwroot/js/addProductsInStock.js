$(function () {
    get_categories();
});

function get_products() {
    var search_details = {
        categoryId: $("#category-field").val()
    };
    console.log(search_details);

    $.ajax({
        type: "GET",
        data: search_details,
        dataType: 'json',
        contextType: 'application/json',
        url: "../Products/GetByCategory",

        success: function (result) {
            $("#product-field option").remove();
            console.log(result);
            $.each(result, function (index, item) {
                $("#product-field").append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function get_supplier() {
    var search_details = {
        productId: $("#product-field").val()
    };
    console.log(search_details);

    $.ajax({
        type: "GET",
        data: search_details,
        dataType: 'json',
        contextType: 'application/json',
        url: "../Suppliers/GetSupplierByProduct",

        success: function (result) {
            $("#supplier-field option").remove();
            console.log(result);
            $("#supplier-field").append('<option value="' + result.id + '">' + result.name + '</option>');
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}
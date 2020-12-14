$.noConflict();

$(function () {
    get_delivery_methods();
    get_payment_methods();
});

var product_id;
var quantity;
var quantity_product_id;

$(".quantity").change(function (event) {
    quantity_product_id = $(this).attr("id");
    console.log(quantity_product_id);
    var data = quantity_product_id.split("-");
    quantity = data[0];
    product_id = data[1];
});

function get_delivery_methods() {
    $.ajax({
        type: "GET",
        dataType: 'json',
        contextType: 'application/json',
        url: "../DeliveryMethods/GetDeliveryMethods",

        success: function (result) {
            $("#delivery-field option").remove();
            console.log(result);
            $.each(result, function (index, item) {
                $("#delivery-field").append('<option value="' + item.id + '">' + item.method + '</option>');
            });
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}


function get_payment_methods() {
    $.ajax({
        type: "GET",
        dataType: 'json',
        contextType: 'application/json',
        url: "../Orders/GetPaymentMethods",

        success: function (result) {
            $("#payment-field option").remove();
            console.log(result);
            $.each(result, function (index, item) {
                $("#payment-field").append('<option value="' + item.id + '">' + item.name + '</option>');
            });
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function update_quantity_and_sub_total() {
    var quantity_id = "#" + quantity_product_id;
    var update_details = {
        quantity: $(quantity_id).val(),
        id: product_id
    };
    console.log(update_details);

    $.ajax({
        type: "GET",
        data: update_details,
        dataType: 'json',
        contextType: 'application/json',
        url: "../CartProducts/UpdateQuantity",

        success: function (result) {
            if (result == 'updated') {
                update_total_sum();
                update_sub_total(update_details);
            }
            else {
                var quantity_field = document.getElementById(quantity_product_id);
                quantity_field.value = quantity;
            }
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function update_total_sum() {
    $.ajax({
        type: "GET",
        dataType: 'json',
        contextType: 'application/json',
        url: "../CartProducts/UpdateTotalSum",

        success: function (result) {
            var sum = document.getElementById('sum-id');
            sum.value = result;
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function update_sub_total(update_details) {

    $.ajax({
        type: "GET",
        data: update_details,
        dataType: 'json',
        contextType: 'application/json',
        url: "../CartProducts/UpdateSubTotal",

        success: function (result) {
            if (result != -1) {
                var sub_total_id = "sub-total-" + update_details.id;
                var sub_total = document.getElementById(sub_total_id);
                sub_total.value = result;
            }
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}

function update_delivery_price() {
    var search_details = {
        id: $("#delivery-field").val()
    };
    console.log(search_details);

    $.ajax({
        type: "GET",
        data: search_details,
        dataType: 'json',
        contextType: 'application/json',
        url: "../DeliveryMethods/GetDeliveryById",

        success: function (result) {
            var price = document.getElementById('delivery-price-field');
            price.value = result.price;
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}
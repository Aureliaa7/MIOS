﻿$.noConflict();

$(function () {
    get_delivery_methods();
    get_payment_methods();
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

function update_quantity() {
    var update_details = {
        quantity: $("#quantity-field").val(),
        id: $("#id-field").val()
    };
    console.log(update_details);

    $.ajax({
        type: "GET",
        data: update_details,
        dataType: 'json',
        contextType: 'application/json',
        url: "../Cart/UpdateQuantity",

        success: function (result) {
            if (result == 'updated') {
                update_total_sum();
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
        url: "../Cart/UpdateTotalSum",

        success: function (result) {
            var sum = document.getElementById('sum-id');
            sum.value = result;
        },
        error: function () {
            console.log("Something went wrong");
        }
    });
}
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

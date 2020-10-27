$('#product-carousel').each(function () {
    $(this).carousel({
        interval: false
    });
});

$('.carousel-control-prev').click(function () {
    $('#product-carousel').carousel('prev');
});

$('.carousel-control-next').click(function () {
    $('#product-carousel').carousel('next');
});
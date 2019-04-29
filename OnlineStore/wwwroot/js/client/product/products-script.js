
jQuery(function () {
    jQuery('.starbox').each(function () {
        var starbox = jQuery(this);
        starbox.starbox({
            average: starbox.attr('data-start-value'),
            changeable: starbox.hasClass('unchangeable') ? false : starbox.hasClass('clickonce') ? 'once' : true,
            ghosting: starbox.hasClass('ghosting'),
            autoUpdateAverage: starbox.hasClass('autoupdate'),
            buttons: starbox.hasClass('smooth') ? false : starbox.attr('data-button-count') || 5,
            stars: starbox.attr('data-star-count') || 5
        }).bind('starbox-value-changed', function (event, value) {
            if (starbox.hasClass('random')) {
                var val = Math.random();
                starbox.next().text(' ' + val);
                return val;
            }
        })
    });
});


$(window).load(function () {
    $('.flexslider').flexslider({
        animation: "slide",
        controlNav: "thumbnails"
    });
});

$(function () {
    $('a.picture').Chocolat();
});



$('.value-plus').on('click', function () {
    var divUpd = $(this).parent().find('.value'), newVal = parseInt(divUpd.text(), 10) + 1;
    divUpd.text(newVal);
});

$('.value-minus').on('click', function () {
    var divUpd = $(this).parent().find('.value'), newVal = parseInt(divUpd.text(), 10) - 1;
    if (newVal >= 1) divUpd.text(newVal);
});


jQuery.ajaxSettings.traditional = true;
var listBrandName = [];
function sortProduct() {
    $('#CurrentPage').val(1);
    $.ajax({
        type: "GET",
        url: "/Product/Index?handler=Search",
        data: {
            "currentSearchString": "@Model.CurrentSearchString",
            "currentSort": $('#CurrentSort option:selected').val(),
            "currentBrand": listBrandName
        }
    }).done(function (result) {
        $(".bottom-product").html(result);
    }).fail(function (result) {
        console.log(result);
    });
}

/*comment-box*/
$(document).ready(function () {
    $("[data-toggle=tooltip]").tooltip();
});
/*comment-box*/
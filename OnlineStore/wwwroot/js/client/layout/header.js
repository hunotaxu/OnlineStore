jQuery(document).ready(function ($) {
    var $filter = $('.header-inner');
    var $filterSpacer = $('<div />', {
        "class": "vnkings-spacer",
        "height": $filter.outerHeight()
    });
    if ($filter.size()) {
        $(window).scroll(function () {
            if (!$filter.hasClass('fix') && $(window).scrollTop() > $filter.offset().top) {
                $filter.before($filterSpacer);
                $filter.addClass("fix");
            }
            else if ($filter.hasClass('fix') && $(window).scrollTop() < $filterSpacer.offset().top) {
                $filter.removeClass("fix");
                $filterSpacer.remove();
            }
        });
    }
});

var loadItemMyCart = (function () {
    var init = function () {
        loadItemCart();       
    };   
    loadItemCart = function () {       
        $.ajax({
            type: "GET",
            url: "/Home/Index?handler=LoadNumberItemCart",
            //beforeSend: function () {
            //    commons.startLoading();
            //},            
            success: function (response) {
                if (response !== '') {
                    //$('#cart-itemtotal').html(`${commons.formatNumber((response), 0)}đ`);
                    $('#cart-itemtotal').html(response);
                } else {
                    $('#cart-itemtotal').html(``);
                }
                //commons.stopLoading();
                document.getElementById("cart-itemtotal").innerHTML.replace;
            },
            error: function () {
                //commons.stopLoading();
            }
        });
    };
    return {
        init
    };
})();


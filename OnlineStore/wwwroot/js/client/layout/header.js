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
        loadCartLayout();
    };
    var loadItemCart = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/NumberOfItemsInCart?handler=LoadNumberItemCart",
            //beforeSend: function () {
            //    commons.startLoading();
            //},
            success: function (response) {
                if (response !== '') {
                    //$('#cart-itemtotal').html(`${commons.formatNumber((response), 0)}đ`);
                    $('#cart-itemtotal').html(response);
                } else {
                    $('#cart-itemtotal').html(`0`);
                }
                //commons.stopLoading();
                document.getElementById("cart-itemtotal").innerHTML.replace;
            },
            error: function () {
                //commons.stopLoading();
                $('#cart-itemtotal').html(`0`);
            }
        });
    };
    var loadCartLayout = function () {
        var PriceTotaltmp = 0;
        $.ajax({
            type: "GET",
            url: "/Cart/Index?handler=LoadCartLayout",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#template-cart-layout').html(), {
                            ItemId: item.itemId,
                            ProductName: item.productName,
                            Image: item.image,
                            Price: `${commons.formatNumber(item.price, 0)}đ`,
                            //TotalPrice: `${commons.formatNumber(item.price * item.quantity, 0)}đ`,
                            Quantity: item.quantity
                        });
                        PriceTotaltmp += item.price * item.quantity;
                    });
                }
                if (render !== '') {
                    $('#container-pay-cart-layout').html(render);
                    $('#PriceTotaltmpLayout').html(`${commons.formatNumber(PriceTotaltmp, 0)}đ`);
                    $('.error-load-cart-layout').hide();
                } else {
                    $('.error-load-cart-layout').html(`<div style='text-align: center;'><br/><h3 class="productnamecheckout">Không có sản phẩm nào trong giỏ hàng</h3></div>`);
                }
                commons.stopLoading();
            },
            error: function (response) {
                if (response !== undefined && response !== '') {
                    commons.notify(response.responseText, 'error');
                }
                else {
                    commons.notify('Lỗi tải giỏ hàng', 'error');
                }

                commons.stopLoading();
            }
        });
    };
    return {
        init
    };
})();
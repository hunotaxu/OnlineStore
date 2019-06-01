var cart = (function () {
    var init = function () {
        loadData();
    };

    var loadData = function () {
        var subTotal = 0;
        $.ajax({
            type: "GET",
            url: "/Cart/Index?handler=LoadCart",
            beforeSend: function () {
                commons.startLoading();
            },
            
            success: function (response) {
                var render = "";
                $.each(response, function (i, item) {
                    render += Mustache.render($('#template-cart').html(), {
                        ItemId: item.itemId,
                        ProductName: item.productName,
                        Image: item.image,
                        Price: `${commons.formatNumber(item.price, 0)}đ`,
                        Quantity: item.quantity
                        //Subtotal: `${commons.formatNumber(item.subtotal, 0)}đ`
                    });
                    subTotal += item.price * item.quantity;
                });
                if (render !== '') {
                    $('#container-cart').html(render);
                    $('#subTotal').html(`${commons.formatNumber(subTotal, 0)}đ`);
                } else {
                    $('#container-cart').html('Không có sản phẩm nào trong giỏ hàng');
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không thể tải giỏ hàng', 'error');
                commons.stopLoading();
            }
        });
    };

    return {
        init
    };
})();
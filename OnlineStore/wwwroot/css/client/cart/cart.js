var cart = (function () {
    var init = function () {
        loadData();
    };

    var loadData = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Index?handler=LoadCart",
            data: {

            },
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                initCartData(response);
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không thể tải đơn hàng', 'error');
                commons.stopLoading();
            }
        });
    };

    var initCartData = function (response) {
        $.each(response, function (i, item){

        });
    };

    return {
        init
    };
})();
//var Cart = function() {
//    function loadData() {
//        $.ajax({
//            url: '/Cart/GetCart',
//            type: 'GET',
//            dataType: 'json',
//            success: function (response) {
//                var template = $('#template-cart').html();
//                var render = "";
//                var totalAmount = 0;
//                $.each(response, function (i, item) {
//                    render += Mustache.render(template,
//                        {
//                            ProductId: item.Product.Id,
//                            ProductName: item.Product.Name,
//                            Image: item.Product.Image,
//                            Price: tedu.formatNumber(item.Price, 0),
//                            Quantity: item.Quantity,                           
//                            Amount: tedu.formatNumber(item.Price * item.Quantity, 0),
//                            Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
//                        });
//                    totalAmount += item.Price * item.Quantity;
//                });
//                $('#lblTotalAmount').text(tedu.formatNumber(totalAmount, 0));
//                if (render !== "")
//                    $('#table-cart-content').html(render);
//                else
//                    $('#table-cart-content').html('You have no product in cart');
//            }
//        });
//        return false;
//    }
//}
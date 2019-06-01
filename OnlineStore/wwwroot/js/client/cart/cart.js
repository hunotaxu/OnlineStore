

function loadData() {
    $.ajax({
        url: '/Cart/GetCart',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            var template = $('#template-cart').html();
            var render = "";
            var totalAmount = 0;
            $.each(response, function (i, item) {
                render += Mustache.render(template,
                    {
                        ProductId: item.Product.Id,
                        ProductName: item.Product.Name,
                        Image: item.Product.Image,
                        Price: tedu.formatNumber(item.Price, 0),
                        Quantity: item.Quantity,                        
                        Amount: tedu.formatNumber(item.Price * item.Quantity, 0),
                        Url: '/' + item.Product.SeoAlias + "-p." + item.Product.Id + ".html"
                    });
                totalAmount += item.Price * item.Quantity;
            });
            $('#lblTotalAmount').text(tedu.formatNumber(totalAmount, 0));
            if (render !== "")
                $('#table-cart-content').html(render);
            else
                $('#table-cart-content').html('Không có sản phẩm trong giỏ');
        }
    });
    return false;
}
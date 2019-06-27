var jshome = (function () {
    var init = function () {
        jQuery('.mega-menu-index').slideDown();
        $(document).ready(function () {
            $('#Carousel').carousel({
                interval: 5000
            }
            );
            registerEvents();
        });
    };

    var registerEvents = function () {
        $('.btnAddToCart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            if ($('#rateit_star').data('customerid') === '' || $('#rateit_star').data('customerid') === undefined) {
                commons.confirm('Bạn chưa đăng nhập, bạn có muốn chuyển tiếp sang trang đăng nhập?', function () {
                    window.location.replace(`/Identity/Account/Login?returnUrl=/Product/Detail?id=${id}`);
                });
            }

            else {
                $.ajax({
                    url: "/Product/AddToCart?handler=AddToCart",
                    type: "POST",
                    dataType: "json",
                    contentType: 'application/json;charset=utf-8',
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    data: JSON.stringify({
                        ItemId: id,
                        Quantity: parseInt($('#txtQuantity').val())
                    }),
                    success: function () {
                        commons.notify('Thêm vào giỏ hàng thành công', 'success');
                        loadItemMyCart.init();
                        commons.stopLoading();
                    },
                    error: function (response) {
                        if (response.responseText !== undefined && response.responseText !== '') {
                            commons.notify(response.responseText, 'error');
                        }
                        else {
                            commons.notify('Đã có lỗi xãy ra', 'error');
                        }
                        commons.stopLoading();
                    }
                });
            }
        });

    };
    

    return {
        init
    };
})();

var myOrder = (function () {
    var init = function () {
        $(document).ready(function () {
            registerEvents();
        });
    };

    var registerEvents = function () {
        $(document).ready(function () {
            $('body').on('click', '#cancelOrder', function (e) {
                e.preventDefault();
                var that = $(this).data('id');
                cancelOrder(that);
            });
        });
    };

    function cancelOrder(that) {
        commons.confirm('Bạn có chắc chắn muốn hủy đơn hàng này?', function () {
            $.ajax({
                type: "POST",
                url: "/Order/MyOrder?handler=CancelOrder",
                data: JSON.stringify({ Id: that }),
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function (response) {
                    commons.notify('Hủy đơn hàng thành công', 'success');
                    window.location.href = window.location.href;
                    //loadData(true);
                },
                error: function (response) {
                    if (response.responseText !== undefined && response.responseText !== '') {
                        commons.notify(response.responseText, 'error');
                    }
                    else {
                        commons.notify('Đã có lỗi xãy ra', 'error');
                    }
                },
                complete: function () {
                    commons.stopLoading();
                }
            });
        });
    }
    return {
        init
    };
})();
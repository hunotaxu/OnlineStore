var productDetail = (function () {
    var init = function () {
        initRating();
        initCommenting();
        initCartManagement();

        // giữ tab active sau khi reload
        $(document).ready(function () {
            if (location.hash) {
                $("a[href='" + location.hash + "']").tab("show");
            }
            $(document.body).on("click", "a[data-toggle='tab']", function (event) {
                location.hash = this.getAttribute("href");
            });
        });
        $(window).on("popstate", function () {
            var anchor = location.hash || $("a[data-toggle='tab']").first().attr("href");
            $("a[href='" + anchor + "']").tab("show");
        });
    };

    var initRating = function () {
        $('#rateit_star').rateit({
            step: 1, min: 0, max: 5, resetable: false
        });
    };
    var initCartManagement = function () {
        $('#btnAddToCart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            if ($('#data-cus-hidden').data('customerid') === '' || $('#data-cus-hidden').data('customerid') === undefined || ($('#data-cus-hidden').data('customerid') !== '' && $('#is-customer').data('is-customer') === "False")) {
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
                        header.init();
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
        let maximumQty = document.getElementById('itemQty').value;
        if (maximumQty < 1) {
            $('#txtQuantity').attr('disabled', true);
            $('#btnAddToCart').attr('disabled', true);
            $("#btnAddToCart").css({
                "background-color": "#e5e5e5",
                "border": "1px #e5e5e5 solid",
                "color": "#000000"
            });
            $(".qtybutton").css({
                "pointer-events": "none"
            });
        }

        $("#txtQuantity").on("keypress keyup", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
            let enteredQty = document.getElementById('txtQuantity').value;
            if (enteredQty > maximumQty) {
                commons.notify(`Số lượng tối đa cho phép là: ${maximumQty}`, 'error');
                $(this).val(maximumQty);
            }
        });
    }
    let initCommenting = function () {
        $('#gotocomment').on('click', function (e) {
            $("#review a").trigger('click');
        });
        $('#gotocomment1').on('click', function (e) {
            $("#review a").trigger('click');
        });
        $('#btnReview').on('click', function (e) {
            //if ($('#frmEvaluation').valid()) {
            e.preventDefault();
            var star = $('#rateit_star').rateit('value');
            var content = $('#txtContent').val();
            var itemid = $('#rateit_star').data('itemid');
            var customerid = $('#rateit_star').data('customerid');
            //alert('Bạn đã đánh giá ' + value + ' sao cho sản phẩm có id là:' + itemid);

            $.ajax({
                type: "POST",
                url: "/Product/AddComment?handler=SaveEntity",
                data: JSON.stringify({
                    Evaluation: star,
                    Content: content,
                    ItemId: itemid,
                    CustomerId: customerid
                }),
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                beforeSend: function (xhr) {
                    commons.startLoading();
                },
                success: function (response) {
                    commons.notify('Thành công', 'success');
                    //resetFormMaintainance();
                    commons.stopLoading();
                    $('#txtContent').val('');
                    //calculateEval();
                    location.reload();
                    //loadData(true);
                },
                error: function () {
                    commons.notify('Đã có lỗi xãy ra', 'error');
                    commons.stopLoading();
                }
            });

            return false;
        });
    };
    return {
        init
    };
})();
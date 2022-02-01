﻿var cart = (function () {
    var init = function () {
        loadData();
        editQuantity();
        deleteItem();
        confirmOrder();
        //loadItemCart();
        //loadCartLayout();
    };

    var confirmOrder = function () {
        $('#btnConfirmOrder').on('click', function () {
            $.ajax({
                url: "/Cart/Index?handler=ConfirmOrder",
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function () {
                    window.location.href = "/cart/checkout";
                },
                error: function (response) {
                    if (response !== undefined && response !== '') {
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
    };

    var deleteItem = function () {
        $('body').on('click', '#btnDeleteItem', function () {
            var itemId = $(this).data("itemid");
            commons.confirm("Bạn có chắc chắn muốn xóa sản phẩm này ra khỏi giỏ hàng", function () {
                $.ajax({
                    type: 'post',
                    url: "/Cart/Index?handler=DeleteItem",
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        ItemId: itemId
                    }),
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    success: function () {
                        loadData();
                        header.init();
                        commons.stopLoading();
                    },
                    error: function () {
                        commons.notify('Xóa không thành công', 'error');
                        commons.stopLoading();
                    }
                });
            });
        });
    };

    var editQuantity = function () {
        $('body').on('change', '#btnQuantity', function (e) {
            e.preventDefault();
            if ($(this).val() !== '' && $(this).val() !== undefined) {
                if (parseInt($(this).val()) > parseInt($(this).attr('max'))) {
                    $(this).val($(this).attr('max'));
                    commons.notify('Xin lỗi, bạn chỉ có thể mua sản phẩm này với số lượng cho phép', 'error');
                }
                else if (parseInt($(this).val()) < 1) {
                    $(this).val(1);
                    commons.notify('Vui lòng nhập số lượng cho chính xác', 'error');
                }
                $.ajax({
                    type: 'post',
                    url: '/Cart/Index?handler=UpdateQuantity',
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify({
                        ItemId: $(this).data("itemid"),
                        Quantity: $(this).val()
                    }),
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    success: function () {
                        loadData();
                        header.init();
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

    //var loadItemCart = function () {
    //    $.ajax({
    //        type: "GET",
    //        url: "/Cart/NumberOfItemsInCart?handler=LoadNumberItemCart",
    //        //beforeSend: function () {
    //        //    commons.startLoading();
    //        //},
    //        success: function (response) {
    //            if (response !== '') {
    //                $('#cart-itemtotal').html(response);
    //            } else {
    //                $('#cart-itemtotal').html(`0`);
    //            }
    //            document.getElementById("cart-itemtotal").innerHTML.replace;
    //        },
    //        error: function () {
    //            $('#cart-itemtotal').html(`0`);
    //        }
    //    });
    //};
    //var loadCartLayout = function () {
    //    var PriceTotaltmp = 0;
    //    $.ajax({
    //        type: "GET",
    //        url: "/Cart/Index?handler=LoadCartLayout",
    //        beforeSend: function () {
    //            commons.startLoading();
    //        },
    //        success: function (response) {
    //            var render = '';
    //            if (response !== undefined) {
    //                $.each(response, function (i, item) {
    //                    render += Mustache.render($('#template-cart-layout').html(), {
    //                        ItemId: item.itemId,
    //                        ProductName: item.productName,
    //                        Image: item.image,
    //                        Price: `${commons.formatNumber(item.price, 0)}đ`,
    //                        Quantity: item.quantity
    //                    });
    //                    PriceTotaltmp += item.price * item.quantity;
    //                });
    //            }
    //            if (render !== '') {
    //                $('#container-pay-cart-layout').html(render);
    //                $('#PriceTotaltmpLayout').html(`${commons.formatNumber(PriceTotaltmp, 0)}đ`);
    //                $('.error-load-cart-layout').hide();
    //            } else {
    //                $('.error-load-cart-layout').html(`<div style='text-align: center;'><br/><h3 class="productnamecheckout">Không có sản phẩm nào trong giỏ hàng</h3></div>`);
    //            }
    //            commons.stopLoading();
    //        },
    //        error: function (response) {
    //            if (response !== undefined && response !== '') {
    //                commons.notify(response.responseText, 'error');
    //            }
    //            else {
    //                commons.notify('Lỗi tải giỏ hàng', 'error');
    //            }

    //            commons.stopLoading();
    //        }
    //    });
    //};

    var loadData = function () {
        var subTotal = 0;
        $.ajax({
            type: "GET",
            url: "/Cart/Index?handler=LoadCart",
            beforeSend: function () {
                commons.startLoading();
            },

            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#template-cart').html(), {
                            ItemId: item.itemId,
                            ProductName: item.productName,
                            BrandName: item.brandName,
                            Image: item.image,
                            Price: `${commons.formatNumber(item.price, 0)}đ`,
                            Quantity: item.quantity,
                            MaxQuantity: item.maxQuantity,
                            IsOutOfStock: item.maxQuantity > 0 ? "" : "disabled",
                            OutOfStockNotifyCation: item.maxQuantity > 0 ? "" : `Sản phẩm này đã hết hàng`
                        });
                        if (item.maxQuantity > 0) {
                            subTotal += item.price * item.quantity;
                        }
                    });
                }
                if (render !== '') {
                    $('#container-cart').html(render);
                    $('#subTotal').html(`${commons.formatNumber(subTotal, 0)}đ`);
                } else {
                    $('.container__row--center').html(`<br/><div style='text-align: center;'><h3>Không có sản phẩm nào trong giỏ hàng</h3><a href='/' class='btn btn-warning'>Tiếp tục mua sắm</a></div>`);
                    $('.container__div--subtotal').remove();
                    header.init();
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
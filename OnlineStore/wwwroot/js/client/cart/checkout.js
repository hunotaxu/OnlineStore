var checkoutcart = (function () {
    
    var init = function () {
        loadData();
        registerEvents();
    };
    var loadCreditcard = function () {
        //hiện creditcard
        $("#creditcardtDIV").removeClass("none");
        $("#creditcardtDIV").addClass("showDIV");

        //đóng momo
        $("#ecashDIV").addClass("none");
        $("#ecashDIV").removeClass("showDIV");
    };
    var loadCheckin = function () {
        //đóng creditcard
        $("#creditcardtDIV").addClass("none");
        $("#creditcardtDIV").removeClass("showDIV");

        //đóng momo
        $("#ecashDIV").addClass("none");
        $("#ecashDIV").removeClass("showDIV");
    };
    var loadAtm = function () {
        //đóng creditcard
        $("#creditcardtDIV").addClass("none");
        $("#creditcardtDIV").removeClass("showDIV");

        //đóng momo
        $("#ecashDIV").addClass("none");
        $("#ecashDIV").removeClass("showDIV");
    };
    var loadmomo = function () {
        //đóng creditcard
        $("#creditcardtDIV").addClass("none");
        $("#creditcardtDIV").removeClass("showDIV");

        //hiện momo
        $("#ecashDIV").removeClass("none");
        $("#ecashDIV").addClass("showDIV");
    };

    var registerEvents = function () {
        $(document).ready(function () { 
            $("#btnSave").on('click', function () {
                debugger;
                e.preventDefault();
                commons.notify('Không được chọn danh mục cha là chính nó', 'error');

                alert("hi");
            });
            //$("#btnSave").on('click', function () {
            //    debugger;
            //    if ($('input[name=radio]:checked').length > 0) {
            //        $('#txtAddress').val(this.data.Address);
            //    }               
            //});


            $('#btneditaddress').off('click').on('click', function () {
                $('#modal-edit-address').modal('hide');
                $('#modal-select-address').modal('show');
                loadAddress();

            });
            $('#btnaddaddress').off('click').on('click', function () {
                $('#modal-select-address').modal('hide');
                $('#modal-edit-address').modal('show');
            });
        });
       
    };

    var loadData = function () {
        var PriceTotaltmp = 0;
        var itemTotal = 0;
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadTmpCart",
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
                            Image: item.image,
                            TotalPrice: `${commons.formatNumber(item.price * item.quantity, 0)}đ`,
                            Quantity: item.quantity
                        });
                        if (item.maxQuantity > 0) {
                            PriceTotaltmp += item.price * item.quantity;
                        }
                        itemTotal++;

                    });
                }
                if (render !== '') {
                    $('#container-pay-cart').html(render);
                    $('#itemTotal').html(`${commons.formatNumber(itemTotal, 0)}`);
                    $('#itemTotal1').html(`${commons.formatNumber(itemTotal, 0)}`);
                    $('#PriceTotaltmp').html(`${commons.formatNumber(PriceTotaltmp, 0)}đ`);
                    $('#PriceTotaltmp1').html(`${commons.formatNumber(PriceTotaltmp, 0)}đ`);

                } else {
                    $('.order-detail-contentr').html(`<div style='text-align: center;'><h3>Không có sản phẩm nào trong giỏ hàng</h3><a href='/' class='btn btn-warning'>Tiếp tục mua sắm</a></div>`);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Lỗi tải giỏ hàng', 'error');
                commons.stopLoading();
            }
        });
    };
    var loadAddress = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadAddress",
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {                        
                        render += Mustache.render($('#script-select-address').html(),
                            {
                            RecipientName: item.recipientName,
                            Detail: item.detail,
                            Address: item.province + '-' + item.district + '-' + item.ward,
                            PhoneNumber: item.phoneNumber,
                            AddressId: item.addressId
                        });
                    });
                }
                if (render !== '') {
                    $('#user-address-content').html(render);
                    
                } else {
                    $('.user-address-label').html(`<div style='text-align: center;'><h3>Không có sẵn địa chỉ</h3></div>`);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không thể tải địa chỉ', 'error');
                commons.stopLoading();
            }
        });
    };
    return {
        init
    };
})();
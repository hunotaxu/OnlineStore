var checkoutcart = (function () {

    var init = function () {
        loadData();
        registerEvents();
        loadAddressDefault();
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
        var Province, District, Ward, Detail, RecipientName, PhoneNumber ;
        $('#frmselectaddress').click(function () {           
            if ($('.radiobutton').is(':checked')) {
                var x = $('input[name=radio]:checked');
                Province = x.data("province");
                District = x.data("district");
                Ward = x.data("ward");
                Detail = x.data("detail");
                RecipientName = x.data("recipientname");
                PhoneNumber = x.data("phonenumber");
            }
        });
        $("#btnSaveAddress").on('click', function () {
            document.getElementById('labelName').innerHTML = RecipientName;
            document.getElementById('labelAddress').innerHTML = Detail + ', ' + Province + ' - ' + District + ' -' + Ward;
            document.getElementById('labelPhoneNumber').innerHTML = PhoneNumber;
            $('#modal-select-address').modal('hide');
        });

        $('#btneditaddress').off('click').on('click', function () {
            $('#modal-add-address').modal('hide');
            $('#modal-select-address').modal('show');
            loadAddress();
        });
        $('#btnaddaddress').off('click').on('click', function () {
            $('#modal-select-address').modal('hide');
            $('#modal-add-address').modal('show');
        });
        $('#btneditdeliveryMethod').off('click').on('click', function () {
            $("#deliveryMethod").css({
                "display": "block"                
            });
            
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
                                ProVince: item.province,
                                District: item.district,
                                Ward: item.ward,
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
    var loadAddressDefault = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadAddress",
            dataType: "json",           
            success: function (response) {
                var RecipientName, Detail, ProVince, District, Ward, PhoneNumber;
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        RecipientName = item.recipientName;
                        Detail = item.detail;
                        ProVince = item.province;
                        District = item.district;
                        Ward = item.ward;
                        PhoneNumber = item.phoneNumber;
                        AddressId = item.addressId;
                        return false;
                    });
                }
                else {
                    RecipientName = '';
                    Detail = '';
                    ProVince = '';
                    District = '';
                    Ward = '';
                    PhoneNumber = '';
                    AddressId = '';
                }
                document.getElementById('labelName').innerHTML = RecipientName;
                document.getElementById('labelAddress').innerHTML = Detail + ',' + ProVince + '-' + District + '-' + Ward;
                document.getElementById('labelPhoneNumber').innerHTML = PhoneNumber;
            },
            error: function () {
                commons.notify('Không thể tải địa chỉ', 'error');
                commons.stopLoading();
            }
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
                        itemTotal += item.quantity
                            ;

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

   
    
    return {
        init
    };



})();
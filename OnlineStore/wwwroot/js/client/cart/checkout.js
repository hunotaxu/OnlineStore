var checkoutcart = (function () {
    var init = function () {
        $(document).ready(function () {
            registerEvents();
            loadAddressDefault();
            loadShowroom();
            loadReceivingType();
            loadData();
            //loadDefaultValue();            
        });
    };

    var registerEvents = function () {
        $('#frmselectshowroom').parsley();
        $('#frmaddaddress').parsley();
        var Province, District, Ward, Detail, RecipientName, PhoneNumber;
        var selectedAddressId;
        $('#frmselectaddress').click(function () {
            if ($('.radiobutton').is(':checked')) {
                var x = $('input[name=radio]:checked');
                selectedAddressId = x.data('id');
                Province = x.data("province");
                District = x.data("district");
                Ward = x.data("ward");
                Detail = x.data("detail");
                RecipientName = x.data("recipientname");
                PhoneNumber = x.data("phonenumber");
            }
        });

        $("#btnSaveSelectAddress").on('click', function () {
            $('#addressId').val(selectedAddressId);
            document.getElementById('labelName').innerHTML = RecipientName;
            document.getElementById('labelAddress').innerHTML = Detail + ', ' + Province + ' - ' + District + ' - ' + Ward;
            document.getElementById('labelPhoneNumber').innerHTML = PhoneNumber;
            //loadAddress();
            $('#modal-select-address').modal('hide');
        });
        $("#btnSaveAddAddress").on('click', function () {
            if ($('#frmaddaddress').parsley().validate()) {
                document.getElementById('labelName').innerHTML = $('#txtHoTen').val();
                document.getElementById('labelAddress').innerHTML = $('#txtDetail').val() + ', ' + $('#frmselectward').val() + ', ' + $('#frmselectdistrict').val() + ', ' + $('#frmselectprovince').val();
                document.getElementById('labelPhoneNumber').innerHTML = $('#txtPhoneNumber').val();
                saveAddress();
                $('#modal-add-address').modal('hide');
            }
            return false;
        });
        $("#txtPhoneNumber").on("keypress keyup", function (event) {
            $(this).val($(this).val().replace(/[^\d].+/, ""));
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
        });

        $('#btneditaddress').off('click').on('click', function (e) {
            e.preventDefault();
            $('#modal-add-address').modal('hide');
            $('#modal-select-address').modal('show');
            loadAddress();
        });
        $('#btnaddaddress').off('click').on('click', function (e) {
            e.preventDefault();
            $('#modal-select-address').modal('hide');
            $('#modal-add-address').modal('show');
            loadProvince();
        });

        $('#radio_button_creditcard').on('change', function () {
            $("#btnorder").prop('disabled', true);
        });

        $('#frmPaypalCheckout').submit(function () {
            var addressObj = {
                PhoneNumber: $('#txtRecipientPhoneNumber').val(),
                RecipientName: $('#txtRecipientName').val(),
                ShowRoomAddressId: $('#selectshowroom').children('option:selected').data('showroomaddressid')
            };
            var orderObj = {
                AddressId: $('#addressId').val(),
                ShippingFee: $('input[name="radio-receivingthod"]:checked').data('receivingfee'),
                PaymentType: $('input[name="paymentType"]:checked').val(),
                ReceivingTypeId: 1,
                SaleOff: 0,
                Status: 2
            };
            var sendObj = {
                Order: orderObj,
                Address: addressObj
            };
            $.ajax({
                type: "POST",
                url: "/Cart/Checkout?handler=SaveOrder",
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                data: JSON.stringify(sendObj),
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function () {
                },
                error: function (response) {
                    if (response.responseText !== undefined && response.responseText !== '') {
                        commons.notify(response.responseText, 'error');
                    }
                    else {
                        commons.notify('Đặt hàng thất bại', 'error');
                    }
                    return false;
                },
                complete: function () {
                    commons.stopLoading();
                }
            });
        });

        $('#radio_button_checkin').on('change', function () {
            $("#btnorder").prop('disabled', false);
        });

        $('#frmselectreceivingtype').click(function () {
            if ($('.radio-receivingtype').is(':checked')) {
                var x = $('input[name=radio-receivingthod]:checked').attr("data-receivingId");
                if (x !== undefined && x === "3") {
                    $("#frmselectshowroom").show();
                    $('#user-address').hide();
                    //$("#user-address").css("display", "none");
                    //$("#select-showroom-receiving").css("display", "block");
                    //$("#select-showroom-receiving").css({
                    //    "display": "none"
                    //});
                } else {
                    $("#frmselectshowroom").hide();
                    $('#user-address').show();
                }
            }
        });

        $('#frmselectreceivingtype').click(function () {
            if ($('.radio-receivingtype').is(':checked')) {
                var x = $('input[name=radio-receivingthod]:checked');
                $('#frmselectreceivingtype').attr('data-receivingValue', x.attr("data-receivingValue"));
                $('#frmselectreceivingtype').attr('data-receivingTypeId', x.attr("data-receivingId"));
                $('#ShippingFee').text(x.attr("data-receivingValue"));

               // chèn phí ship vào form thanh toán paypal
                var fee = x.attr("data-receivingfee");
                var _shippingfee = parseFloat(fee / 23000).toFixed(2);
                $('#paypal_shippingfee').attr('value', _shippingfee);


                var total = parseInt(x.attr("data-receivingFee")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1"));
                //$('#Total1').attr('data-Total', parseInt(x.attr("data-receivingValue")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1")));
                //document.getElementById('Total1').innerHTML = `${commons.formatNumber(parseInt(x.attr("data-receivingValue")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1")), 0)}đ`;
                $('#Total1').attr('data-Total', total);
                $('#Total1').text(`${commons.formatNumber(total, 0)}đ`);
            }
        });

        $('#frmselectprovince').change(function () {
            var provinceid = $(this.options[this.selectedIndex]).attr('data-provinceid');
            loadDistrict(provinceid);
            //$('#frmselectward').val('');
        });
        $('#frmselectdistrict').change(function () {
            var districtid = $(this.options[this.selectedIndex]).attr('data-districtid');
            loadWard(districtid);
        });

        $('#btn-payment-continue').on('click', function (e) {
            e.preventDefault();
            document.getElementById("btnorder").disabled = false;
        });

        $('#btnorder').on('click', function (e) {
            var x = $('input[name=radio-receivingthod]:checked').attr("data-receivingId");
            if (x !== undefined && x === "3") {
                if ($('#frmselectshowroom').parsley().validate()) {
                    e.preventDefault();
                    saveOrder(x);
                }
            }
            else {
                e.preventDefault();
                saveOrder(x);
            }
        });
    };
    var loadAddress = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadAddress",
            dataType: "json",
            data: {
                availableAddressId: $('#addressId').val()
            },
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
                                AddressId: item.addressId,
                                DefaultChecked: item.defaultChecked
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
            //url: "/Cart/Checkout?handler=LoadAddress",
            url: "/Cart/Checkout?handler=LoadDefaultAddress",
            dataType: "json",
            success: function (response) {
                //var RecipientName, Detail, Province, District, Ward, PhoneNumber;
                if (response !== undefined) {
                    //$.each(response, function (i, item) {
                    //    RecipientName = item.recipientName;
                    //    Detail = item.detail;
                    //    ProVince = item.province;
                    //    District = item.district;
                    //    Ward = item.ward;
                    //    PhoneNumber = item.phoneNumber;
                    //    AddressId = item.addressId;
                    //    return false;
                    //});
                    //$.each(response, function (i, item) {
                    //RecipientName = response.recipientName;
                    //Detail = response.detail;
                    //Province = response.province;
                    //District = response.district;
                    //Ward = response.ward;
                    //PhoneNumber = response.phoneNumber;
                    //AddressId = response.addressId;
                    //return false;
                    //});
                    //$('#labelName').innerHTML = RecipientName;
                    $('#labelName').text(response.recipientName || '');
                    $('#addressId').val(response.addressId);
                    //$('#labelAddress').val(Detail + ',' + Province + ', ' + District + ', ' + Ward);
                    response.detail = response.detail === null ? '' : response.detail + ', ';
                    response.province = response.province === null ? '' : response.province + ', ';
                    response.district = response.district === null ? '' : response.district + ', ';
                    response.ward = response.ward === null ? '' : response.ward + ', ';
                    //$('#labelAddress').text(response.detail + ', ' + response.ward + ', ' + response.district + ', ' + response.province);
                    $('#labelAddress').text(response.detail + response.ward + response.district + response.province);
                    $('#labelPhoneNumber').text(response.phoneNumber || '');
                }
                else {
                    RecipientName = '';
                    Detail = '';
                    ProVince = '';
                    District = '';
                    Ward = '';
                    PhoneNumber = '';
                    AddressId = '';
                    $('#labelAddress').innerHTML = "Thêm địa chỉ của bạn";
                }
            },
            error: function () {
                commons.notify('Không thể tải địa chỉ', 'error');
                commons.stopLoading();
            }
        });
    };
    var loadWard = function (districtid) {
        $.ajax({
            type: "POST",
            url: "/Cart/Checkout?handler=LoadWard",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                DistrictId: districtid
            }),
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#script-select-ward').html(), {
                            WardId: item.id,
                            Type: item.type,
                            Name: item.name
                        });
                    });
                }
                if (render !== '') {
                    $('#frmselectward').html(render);
                } else {
                    $('.error-loadward').html(`<div style='text-align: center;'><h3>Dữ liệu phường/ xã không khả dụng</h3>`);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu phường/ xã', 'error');
                commons.stopLoading();
            }
        });
    };

    var loadDistrict = function (provinceid) {
        $.ajax({
            type: "POST",
            url: "/Cart/Checkout?handler=LoadDistrict",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({
                ProvinceId: provinceid
            }),
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#script-select-district').html(), {
                            DistrictId: item.id,
                            Type: item.type,
                            Name: item.name
                        });
                    });
                }
                if (render !== '') {
                    $('#frmselectdistrict').html(render);
                } else {
                    $('.error-loaddistrict').html(`<div style='text-align: center;'><h3>Dữ liệu quận/huyện không khả dụng</h3>`);
                }
                loadWard($('#frmselectdistrict').children('option:selected').data('districtid'));
                commons.stopLoading();

            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/ thành phố', 'error');
                commons.stopLoading();
            }
        });
    };

    var loadDefaultValue = function () {
        $(".radio-receivingtype").first().prop('checked', true);
        $('#frmselectshowroom').hide();
        $('#ShippingFee').text($(".radio-receivingtype").first().data('receivingvalue'));
        var x = $('input[name=radio-receivingthod]:checked');
        var total = parseInt(x.attr("data-receivingFee")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1"));
        $('#Total1').attr('data-Total', total);
        //document.getElementById('Total1').innerHTML = `${commons.formatNumber(parseInt(x.attr("data-receivingFee")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1")), 0)}đ`;
        $('#Total1').text(`${commons.formatNumber(total, 0)}đ`);
        //chèn phí ship vào form thanh toán paypal
        debugger;

        var fee = $('input[name="radio-receivingthod"]:checked').data('receivingfee');
        var _shippingfee = parseFloat(fee / 23000).toFixed(2);
        $('#paypal_shippingfee').attr('value', _shippingfee);
    };

    //var loadProvinceDistrictWard = function () {
    //    $.ajax({
    //        type: "GET",
    //        url: "/Cart/Checkout?handler=LoadProvince",
    //        datatype: "json",
    //        beforeSend: function () {
    //            commons.startLoading();
    //        },
    //        success: function (response) {
    //            var render = '';
    //            if (response !== undefined) {
    //                $.each(response, function (i, item) {
    //                    render += Mustache.render($('#script-select-province').html(), {
    //                        ProVinceId: item.id,
    //                        Type: item.type,
    //                        Name: item.name,
    //                        Detail: item.detail
    //                    });
    //                });
    //            }
    //            if (render !== '') {
    //                $('#frmselectprovince').html(render);
    //            } else {
    //                $('.error-loaddiprovince').html(`<div style='text-align: center;'><h3>Dữ liệu tỉnh/ thành phố không khả dụng</h3>`);
    //            }
    //            loadDistrict($('#frmselectprovince').children('option:selected').data('provinceid'));
    //            loadWard($('#frmselectdistrict').children('option:selected').data('districtid'));
    //            commons.stopLoading();
    //        },
    //        error: function () {
    //            commons.notify('Không tải được dữ liệu tỉnh/ thành phố', 'error');
    //            commons.stopLoading();
    //        }
    //    });
    //};

    var loadProvince = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadProvince",
            datatype: "json",
            beforeSend: function () {
                commons.startLoading();
            },

            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#script-select-province').html(), {
                            ProVinceId: item.id,
                            Type: item.type,
                            Name: item.name,
                            Detail: item.detail
                        });
                    });
                }
                if (render !== '') {
                    $('#frmselectprovince').html(render);
                } else {
                    $('.error-loaddiprovince').html(`<div style='text-align: center;'><h3>Dữ liệu tỉnh/ thành phố không khả dụng</h3>`);
                }
                loadDistrict($('#frmselectprovince').children('option:selected').data('provinceid'));
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/thành phố', 'error');
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
                            Price: `${commons.formatNumber(item.price, 0)}đ`,
                            //TotalPrice: `${commons.formatNumber(item.price * item.quantity, 0)}đ`,
                            Quantity: item.quantity
                        });
                        PriceTotaltmp += item.price * item.quantity;
                        //if (item.maxQuantity > 0) {
                        //    PriceTotaltmp += item.price * item.quantity;
                        //}
                        itemTotal += item.quantity;
                    });
                }
                if (render !== '') {
                    $('#container-pay-cart').html(render);
                    $('#itemTotal1').html(`${commons.formatNumber(itemTotal, 0)}`);
                    $('#PriceTotaltmp1').html(`${commons.formatNumber(PriceTotaltmp, 0)}đ`);
                    $('#PriceTotaltmp1').data('priceTotaltmp1', PriceTotaltmp);
                    $('#PriceTotaltmp1').attr('data-priceTotaltmp1', PriceTotaltmp);
                    var n = parseFloat(PriceTotaltmp / 23000).toFixed(2);
                    $('#paypal_price').attr('value', n);

                    loadDefaultValue();
                } else {
                    $('.order-detail-contentr').html(`<div style='text-align: center;'><h3>Không có sản phẩm nào trong giỏ hàng</h3><a href='/' class='btn btn-warning'>Tiếp tục mua sắm</a></div>`);
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
    var loadReceivingType = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadReceivingType",
            datatype: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        var date = new Date();
                        date.setDate(date.getDate() + item.numberShipDay);
                        render += Mustache.render($('#script-receivingmethod').html(), {
                            ReceivingName: item.name,
                            ReceivingValue: `${commons.formatNumber(item.value, 0)}đ`,
                            ReceivingFee: item.value,
                            ReceivingId: item.id,
                            //NumberShipDay: item.numberShipDay
                            NumberShipDay: commons.dateFormatJson(date)
                        });
                    });
                }
                if (render !== '') {
                    $('#content-receivingmethod').html(render);
                    loadDefaultValue();
                } else {
                    $('.error-loaddiprovince').html(`<div style='text-align: center;'><h3>Lỗi tải dữ liệu</h3>`);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Lỗi tải dữ liệu', 'error');
                commons.stopLoading();
            }
        });
    };
    var loadShowroom = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadShowroom",
            datatype: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#script-select-showroom').html(), {
                            ProvinceId: item.province.id,
                            ProvinceType: item.province.type,
                            ProvinceName: item.province.name,
                            DistrictId: item.district.id,
                            DistrictType: item.district.type,
                            DistrictName: item.district.name,
                            WardId: item.ward.id,
                            WardType: item.ward.type,
                            WardName: item.ward.name,
                            Detail: item.detail,
                            ShowroomAddressId: item.id
                        });

                    });
                }
                if (render !== '') {
                    $('#selectshowroom').html(render);
                } else {
                    $('.error-loadshowroom').html(`<div style='text-align: center;'><h3>Dữ liệu địa chỉ không khả dụng</h3>`);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/ thành phố', 'error');
                commons.stopLoading();
            }
        });
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
    var loadMomo = function () {
        //đóng creditcard
        $("#creditcardtDIV").addClass("none");
        $("#creditcardtDIV").removeClass("showDIV");

        //hiện momo
        $("#ecashDIV").removeClass("none");
        $("#ecashDIV").addClass("showDIV");
    };
    var saveAddress = function () {
        if ($('#frmaddaddress').parsley().validate()) {
            var recipientName = $('#txtHoTen').val(),
                phoneNumber = $('#txtPhoneNumber').val(),
                detail = $('#txtDetail').val(),
                province = $('#frmselectprovince').val(),
                district = $('#frmselectdistrict').val(),
                ward = $('#frmselectward').val();
            $.ajax({
                type: "POST",
                url: "/Cart/Checkout?handler=SaveAddress",
                contentType: 'application/json; charset=utf-8',
                dataType: "json",
                data: JSON.stringify({
                    RecipientName: recipientName,
                    PhoneNumber: phoneNumber,
                    Detail: detail,
                    Province: province,
                    District: district,
                    Ward: ward
                }),
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function (response) {
                    $('#addressId').val(response.id);
                    commons.stopLoading();

                },
                error: function () {
                    commons.notify('Không thể thêm địa chỉ mới', 'error');
                    commons.stopLoading();
                }
            });
        }
    };

    var saveOrder = function (receivingType) {
        var addressId = $('#addressId').val();
        if (receivingType === "1" || receivingType === "2") {
            if (addressId === undefined || addressId === '' || addressId === '0') {
                commons.notify('Bạn phải chọn địa chỉ nhận hàng hợp lệ', 'error');
                return false;
            }
        }
        var addressObj = {
            PhoneNumber: $('#txtRecipientPhoneNumber').val(),
            RecipientName: $('#txtRecipientName').val(),
            ShowRoomAddressId: $('#selectshowroom').children('option:selected').data('showroomaddressid')
        };
        var orderObj = {
            AddressId: $('#addressId').val(),
            //DeliveryDate: $('input[name="radio-receivingthod"]:checked').data('deliverydate'),
            ShippingFee: $('input[name="radio-receivingthod"]:checked').data('receivingfee'),
            PaymentType: $('input[name="paymentType"]:checked').val(),
            ReceivingTypeId: receivingType,
            SaleOff: 0
        };
        var sendObj = {
            Order: orderObj,
            Address: addressObj
        };
        $.ajax({
            type: "POST",
            url: "/Cart/Checkout?handler=SaveOrder",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify(sendObj),
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                commons.stopLoading();
                window.location.href = `/Order/ConfirmAndThanksForOrder?orderId=${response.orderId}&email=${response.email}`;
            },
            error: function (response) {
                if (response.responseText !== undefined && response.responseText !== '') {
                    commons.notify(response.responseText, 'error');
                }
                else {
                    commons.notify('Đặt hàng thất bại', 'error');
                }
                commons.stopLoading();
            }
        });
    };


    return {
        init
    };
})();
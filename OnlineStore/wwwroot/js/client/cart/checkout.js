var checkoutcart = (function () {

    var init = function () {

        loadData();
        registerEvents();
        loadAddressDefault();
        loadShowroom();
        loadReceivingType();
       
    };
    var registerEvents = function () {
        $(document).ready(function () {
            $('#frmaddaddress').validate({
                errorClass: 'red',
                ignore: [],
                lang: 'en',
                rules: {
                    txtHoTen: { required: true },
                    txtDetail: { required: true },
                    txtPhoneNumber: { required: true },
                    frmselectprovince: { required: true },
                    frmselectward: { required: true },
                    frmselectdistrict: { required: true }
                }
            });

            var Province, District, Ward, Detail, RecipientName, PhoneNumber;
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

            $("#btnSaveSelectAddress").on('click', function () {
                document.getElementById('labelName').innerHTML = RecipientName;
                document.getElementById('labelAddress').innerHTML = Detail + ', ' + Province + ' - ' + District + ' -' + Ward;
                document.getElementById('labelPhoneNumber').innerHTML = PhoneNumber;
                loadAddress();
                $('#modal-select-address').modal('hide');
            });
            $("#btnSaveAddAddress").on('click', function () {
                if ($('#frmaddaddress').valid()) {
                    document.getElementById('labelName').innerHTML = $('#txtHoTen').val();
                    document.getElementById('labelAddress').innerHTML = $('#txtDetail').val() + ', ' + $('#frmselectprovince').val() + ' - ' + $('#frmselectdistrict').val() + ' -' + $('#frmselectward').val();
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
            //$('#btneditdeliveryMethod').off('click').on('click', function (e) {
            //    e.preventDefault();
            //    $("#deliveryMethod").css({
            //        "display": "block"
            //    });
            //});



           
            //$('#frmselectreceivingtype').click(function () {                
            //    if ($('.radio-receivingtype').is(':checked')) {
            //        var x = $('input[name=radio-receivingthod]:checked');
            //        alert(x.val());
            //        if (x.val() === 3)
            //            $("#select-showroom-receiving").css({
            //                "display": "none"
            //            });

            //    }
            //});
           
            $('#frmselectreceivingtype').click(function () {              
                if ($('.radio-receivingtype').is(':checked')) {
                    var x = $('input[name=radio-receivingthod]:checked');
                    $('#frmselectreceivingtype').attr('data-receivingValue', x.attr("data-receivingValue"));
                    $('#frmselectreceivingtype').attr('data-receivingTypeId', x.attr("data-receivingId"));

                    $('#Total1').attr('data-Total', parseInt(x.attr("data-receivingValue")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1")));
                    document.getElementById('Total1').innerHTML = `${commons.formatNumber(parseInt(x.attr("data-receivingValue")) + parseInt($('#PriceTotaltmp1').data("priceTotaltmp1")), 0)}đ`;
                }
            });           

            $('#frmselectprovince').change(function () {
                var provinceid = $(this.options[this.selectedIndex]).attr('data-provinceid');
                loadDistrict(provinceid);
                $('#frmselectward').val('');
            });
            $('#frmselectdistrict').change(function () {
                var districtid = $(this.options[this.selectedIndex]).attr('data-districtid');
                loadWard(districtid);
            });
            $('#btn-continue-payment').on('click', function (e) {
                e.preventDefault();
                $("#frmPayment").css({
                    "display": "block"
                });  
                
            });
            $('#btn-payment-continue').on('click', function (e) {
                e.preventDefault();
                document.getElementById("btnorder").disabled = false;
            });
            $('#btnorder').on('click', function (e) {
                e.preventDefault();
                saveOrder();
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
                if (response !== undefined && response.length > 0) {
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
                    document.getElementById('labelName').innerHTML = RecipientName;
                    document.getElementById('labelAddress').innerHTML = Detail + ',' + ProVince + '-' + District + '-' + Ward;
                    document.getElementById('labelPhoneNumber').innerHTML = PhoneNumber;
                }
                else {
                    RecipientName = '';
                    Detail = '';
                    ProVince = '';
                    District = '';
                    Ward = '';
                    PhoneNumber = '';
                    AddressId = '';
                    document.getElementById('labelAddress').innerHTML = "Thêm địa chỉ của bạn";
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
                    $('.error-loaddistrict').html(`<div style='text-align: center;'><h3>Dữ liệu quận/ huyện không khả dụng</h3>`);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/ thành phố', 'error');
                commons.stopLoading();
            }
        });
    };

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
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/ thành phố', 'error');
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
                    $('#itemTotal1').html(`${commons.formatNumber(itemTotal, 0)}`);
                    $('#PriceTotaltmp1').html(`${commons.formatNumber(PriceTotaltmp, 0)}đ`);
                    $('#PriceTotaltmp1').data('priceTotaltmp1', PriceTotaltmp);
                    $('#PriceTotaltmp1').attr('data-priceTotaltmp1', PriceTotaltmp);


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
    var loadReceivingType = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadReceivingType",
            datatype: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var now = new Date();
                var render = '';
                if (response !== undefined) {
                    $.each(response, function (i, item) {
                        render += Mustache.render($('#script-receivingmethod').html(), {
                            ReceivingName: item.name,
                            ReceivingValue: item.value,
                            ReceivingId: item.id,
                            NumberShipDay: item.numberShipDay                         
                        });
                    });
                }
                if (render !== '') {
                    $('#content-receivingmethod').html(render);
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
                            AddressId:item.id
                        });

                    });
                }
                if (render !== '') {
                    $('#frmselectshowroom').html(render);
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
            success: function () {
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Lỗi thêm địa chỉ', 'error');
                commons.stopLoading();
            }
        });
    };
    var saveOrder = function () {
        var ShippingFee, AddressId, ReceivingTypeId, PaymentType, SubTotal, Total;
       
        ShippingFee = $('#frmselectshowroom').data("receivingValue");
        
        $.ajax({
            type: "POST",
            url: "/Cart/Checkout?handler=SaveOrder",
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
            data: JSON.stringify({
                
            }),
            beforeSend: function () {
                commons.startLoading();
            },
            success: function () {
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Lỗi thêm địa chỉ', 'error');
                commons.stopLoading();
            }
        });
    };

    return {
        init
    };



})();
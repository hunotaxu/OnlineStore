var checkoutcart = (function () {

    var init = function () {

        loadData();
        registerEvents();
        loadAddressDefault();

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

            $('#btneditaddress').off('click').on('click', function () {
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
            $('#btneditdeliveryMethod').off('click').on('click', function (e) {
                e.preventDefault();
                $("#deliveryMethod").css({
                    "display": "block"
                });
            });


            $('#radio_button_atm').off('click').on('click', function (e) {
                e.preventDefault();
                loadAtm();
            });
            $('#radio_button_creditcard').off('click').on('click', function (e) {
                e.preventDefault();
                loadCreditcard();
            }); $('#radio_button_momo').off('click').on('click', function (e) {
                e.preventDefault();
                loadMomo();
            });



            if ($('.radio-delivery').is(':checked')) {
                var x = $('input[name=radiodelivery]:checked');

                $(".li-radio-delivery").css({
                    "display": "none"
                });
                alert(x.val());
                x.css({
                    "display": "block"
                });
                document.getElementById("deliveryMethod").innerHTML.replace;
            }

            $('#frmselectprovince').change(function () {
                var provinceid = $(this.options[this.selectedIndex]).attr('data-provinceid');
                loadDistrict(provinceid);
                $('#frmselectward').val('');
            });
            $('#frmselectdistrict').change(function () {
                var districtid = $(this.options[this.selectedIndex]).attr('data-districtid');
                loadWard(districtid);
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
                            Name: item.name
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
    var loadDeiveryType = function () {
        $.ajax({
            type: "GET",
            url: "/Cart/Checkout?handler=LoadDeiveryType",
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
                            Name: item.name
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
        debugger;
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

    return {
        init
    };



})();
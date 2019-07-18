var accountManagement = (function () {
    var init = function () {
        //onTypeCurrency();
        //Dropzone.autoDiscover = false;
        $(document).ready(function () {
            //initDropzone();
            //loadCategories();
            //loadData();
            registerEvents();
            //registerControls();
        });
    };

    var registerEvents = function () {
        $('#frmselectprovince').change(function () {
            var provinceid = $(this.options[this.selectedIndex]).attr('data-provinceid');
            loadDistrict(provinceid);
        });

        $('#frmselectdistrict').change(function () {
            var districtid = $(this.options[this.selectedIndex]).attr('data-districtid');
            loadWard(districtid);
        });

        $('#btnaddaddress').off('click').on('click', function (e) {
            e.preventDefault();
            $('#modal-add-address').modal('show');
            $('#txtHoTen').val('');
            $('#txtPhoneNumber').val('');
            $('#txtDetail').val('');
            loadProvince();
        });

        $("#btnSaveAddAddress").on('click', function () {
            if ($('#frmaddaddress').parsley().validate()) {
                saveAddress();
            }
            return false;
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            $('#modal-add-address').modal('show');
            loadDetails(that);
        });

        $('input[name=radio]').on('change', function (e) {
            var addressId = $(this).data('id');
            console.log(addressId);
            $.ajax({
                type: 'POST',
                url: '/Identity/Account/Manage/AddressBook?handler=UpdateDefaultAddress',
                data: JSON.stringify({
                    Id: addressId
                }),
                contentType: 'application/json;charset=utf-8',
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function () {
                    commons.notify('Cập nhật địa chỉ mặc định thành công', 'success');
                },
                error: function () {
                    commons.notify('Cập nhật địa chỉ mặc định thất bại', 'error');
                },
                complete: function () {
                    commons.stopLoading();
                }
            });
        });
    };


    var saveAddress = function () {
        if ($('#frmaddaddress').parsley().validate()) {
            var recipientName = $('#txtHoTen').val();
            var phoneNumber = $('#txtPhoneNumber').val();
            var detail = $('#txtDetail').val();
            var province = $('#frmselectprovince').val();
            var district = $('#frmselectdistrict').val();
            var ward = $('#frmselectward').val();
            var addressId = $('#address-id').val() === undefined || $('#address-id').val() === '' ? 0 : $('#address-id').val();
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
                    Ward: ward,
                    AddressId: addressId
                }),
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function () {
                    window.location.href = window.location.href;
                    commons.stopLoading();
                },
                error: function () {
                    commons.notify('Không thể thêm địa chỉ mới', 'error');
                    commons.stopLoading();
                }
            });
        }
    };

    function loadDetails(that) {
        $.ajax({
            type: "GET",
            url: "/Identity/Account/Manage/AddressBook?handler=ById",
            data: { id: that },
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#txtHoTen').val(data.recipientName);
                $('#txtPhoneNumber').val(data.phoneNumber);
                $('#txtDetail').val(data.detail);
                $('#address-id').val(data.id);
                loadProvince(data.province, data.district, data.ward);
            },
            error: function () {
                commons.notify('Có lỗi xảy ra', 'error');
            },
            complete: function () {
                commons.stopLoading();
            }
        });
    }

    var loadProvince = function (provinceName, districtName, wardName) {
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
                    //$('.error-loaddiprovince').html(`<div style='text-align: center;'><h3>Dữ liệu tỉnh/ thành phố không khả dụng</h3>`);
                }
                if (provinceName !== undefined && provinceName !== '') {
                    $('#frmselectprovince').val(provinceName);
                }
                else {
                    $("#frmselectprovince option[data-name='Hà Nội']").attr('selected', true);
                }
                loadDistrict($('#frmselectprovince').children('option:selected').data('provinceid'), districtName, wardName);
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/thành phố', 'error');
                commons.stopLoading();
            }
        });
    };

    var loadDistrict = function (provinceid, districtName, wardName) {
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
                    //$('.error-loaddistrict').html(`<div style='text-align: center;'><h3>Dữ liệu quận/huyện không khả dụng</h3>`);
                }
                if (districtName !== undefined && districtName !== '') {
                    $('#frmselectdistrict').val(districtName);
                }
                loadWard($('#frmselectdistrict').children('option:selected').data('districtid'), wardName);
                commons.stopLoading();

            },
            error: function () {
                commons.notify('Không tải được dữ liệu tỉnh/ thành phố', 'error');
                commons.stopLoading();
            }
        });
    };

    var loadWard = function (districtid, wardName) {
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
                    //$('.error-loadward').html(`<div style='text-align: center;'><h3>Dữ liệu phường/ xã không khả dụng</h3>`);
                }

                if (wardName !== undefined && wardName !== '') {
                    $('#frmselectward').val(wardName);
                }

                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu phường/ xã', 'error');
                commons.stopLoading();
            }
        });
    };

    //var loadData = function (isPageChanged) {
    //    var template = $('#table-template').html();
    //    var render = '';
    //    $.ajax({
    //        type: 'GET',
    //        data: {
    //            "categoryId": $('#ddlCategorySearch').val(),
    //            "keyword": $('#txtKeyword').val(),
    //            "pageIndex": commons.configs.pageIndex,
    //            "pageSize": commons.configs.pageSize
    //        },
    //        beforeSend: function () {
    //            commons.startLoading();
    //        },
    //        dataType: 'JSON',
    //        url: '/Admin/Product/Index?handler=AllPaging',
    //        success: function (response) {
    //            if (response.authenticate === false) {
    //                window.location.href = "/Identity/Account/AccessDenied";
    //            }
    //            $('#tbl-content').empty();
    //            $.each(response.results, function (i, item) {
    //                render += Mustache.render(template,
    //                    {
    //                        Id: item.id,
    //                        Name: item.name,
    //                        CategoryName: item.category.name,
    //                        BrandName: item.brandName,
    //                        Quantity: item.quantity,
    //                        Price: `${commons.formatNumber(item.price, 0)}đ`,
    //                        CreatedDate: commons.dateTimeFormatJson(item.dateCreated)
    //                    });
    //            });
    //            $('#lblTotalRecords').text(response.rowCount);
    //            if (render !== '') {
    //                $('#tbl-content').html(render);
    //            }
    //            wrapPaging(response.rowCount,
    //                function () {
    //                    loadData();
    //                }, isPageChanged);
    //        },
    //        error: function (status) {
    //            commons.notify('Không thể tải dữ liệu', 'error');
    //        },
    //        complete: function () {
    //            commons.stopLoading();
    //        }
    //    });
    //};

    //function wrapPaging(recordCount, callBack, changePageSize) {
    //    var totalSize = 1;
    //    if (recordCount !== 0) {
    //        totalSize = Math.ceil(recordCount / commons.configs.pageSize);
    //    }
    //    // Unbind pagination if it existed or click change page size
    //    if ($('#paginationUL a').length === 0 || changePageSize === true) {
    //        $('#paginationUL').empty();
    //        $('#paginationUL').removeData("twbs-pagination");
    //        $('#paginationUL').unbind("page");
    //    }
    //    // Bind Pagination Event
    //    $('#paginationUL').twbsPagination({
    //        totalPages: totalSize,
    //        visiblePages: 7,
    //        first: 'Đầu',
    //        prev: 'Trước',
    //        next: 'Tiếp',
    //        last: 'Cuối',
    //        onPageClick: function (event, p) {
    //            commons.configs.pageIndex = p;
    //            setTimeout(callBack(), 200);
    //        }
    //    });
    //};

    //var onTypeCurrency = function () {
    //    $("#txtOriginalPriceM").on({
    //        keyup: function () {
    //            formatCurrency($(this));
    //        }
    //        //},
    //        //blur: function () {
    //        //    formatCurrency($(this), "blur");
    //        //}
    //    });

    //    $("#txtPriceM").on({
    //        keyup: function () {
    //            formatCurrency($(this));
    //        }
    //        //},
    //        //blur: function () {
    //        //    formatCurrency($(this), "blur");
    //        //}
    //    });
    //};

    //function formatNumber(n) {
    //    // format number 1000000 to 1,234,567
    //    return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    //}

    //function formatCurrency(input, blur) {
    //    // appends $ to value, validates decimal side
    //    // and puts cursor back in right position.

    //    // get input value
    //    var input_val = input.val();

    //    // don't validate empty input
    //    if (input_val === "") { return; }

    //    // original length
    //    var original_len = input_val.length;

    //    // initial caret position 
    //    var caret_pos = input.prop("selectionStart");

    //    // check for decimal
    //    if (input_val.indexOf(".") >= 0) {

    //        // get position of first decimal
    //        // this prevents multiple decimals from
    //        // being entered
    //        var decimal_pos = input_val.indexOf(".");

    //        // split number by decimal point
    //        var left_side = input_val.substring(0, decimal_pos);
    //        var right_side = input_val.substring(decimal_pos);

    //        // add commas to left side of number
    //        left_side = formatNumber(left_side);

    //        // validate right side
    //        right_side = formatNumber(right_side);

    //        // On blur make sure 2 numbers after decimal
    //        if (blur === "blur") {
    //            right_side += "00";
    //        }

    //        // Limit decimal to only 2 digits
    //        right_side = right_side.substring(0, 2);

    //        // join number by .
    //        //input_val = "$" + left_side + "." + right_side;
    //        input_val = left_side + "." + right_side;

    //    } else {
    //        // no decimal entered
    //        // add commas to number
    //        // remove all non-digits
    //        input_val = formatNumber(input_val);
    //        //input_val = "$" + input_val;
    //        input_val = input_val;

    //        // final formatting
    //        if (blur === "blur") {
    //            input_val += ".00";
    //        }
    //    }

    //    // send updated string to input
    //    input.val(input_val);

    //    // put caret back in the right position
    //    var updated_len = input_val.length;
    //    caret_pos = updated_len - original_len + caret_pos;
    //    input[0].setSelectionRange(caret_pos, caret_pos);
    //}

    return {
        init
    };
})();
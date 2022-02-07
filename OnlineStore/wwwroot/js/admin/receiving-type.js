var receivingType = (function () {
    var init = function () {
        //onTypeCurrency();
        //Dropzone.autoDiscover = false;
        $(document).ready(function () {
            //initDropzone();
            //loadCategories();
            loadData();
            registerEvents();
            //registerControls();
        });
    };

    var registerEvents = function () {
        $('body').on('click', '#btnCreate', function (e) {
            e.preventDefault();
            //initDropzone(0);
            resetFormMaintainance();
            //resetImageTemporary();
            //initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '.btn-edit', function (e) {
            e.preventDefault();
            $('#frmMaintainance').parsley().reset();
            var that = $(this).data('id');
            loadDetails(that);
        });

        $('body').on('click', '.btn-delete', function (e) {
            e.preventDefault();
            var that = $(this).data('id');
            deleteReceivingType(that);
        });

        $('#btnSave').on('click', function (e) {
            saveReceivingType(e);
        });

        //$('#btn-import').on('click', function () {
        //    //initTreeDropDownCategory();
        //    $('#modal-import-excel').modal('show');
        //});
    };

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        $('#txtValue').val('');
        $('#txtNumberShipDay').val('');
        //initTreeDropDownCategory('');
        //$('#txtPriceM').val('0');
        //$('#txtPromotionPriceM').val('0');
        //CKEDITOR.instances.txtDescM.setData('');
    }

    function deleteReceivingType(that) {
        commons.confirm('Bạn có chắc chắn muốn xóa phương thức nhận hàng này và các đơn hàng đã sử dụng phương thức này?', function () {
            $.ajax({
                type: "POST",
                url: "/Admin/Order/ReceivingTypeManagement?handler=Delete",
                data: JSON.stringify({ Id: that }),
                contentType: 'application/json; charset=utf-8',
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function (response) {
                    commons.notify('Thành công', 'success');
                    commons.stopLoading();
                    loadData(true);
                },
                error: function (status) {
                    commons.notify('Đã xảy ra lỗi', 'error');
                    commons.stopLoading();
                }
            });
        });
    }

    function saveReceivingType(e) {
        if ($('#frmMaintainance').parsley().validate()) {
            e.preventDefault();
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            //var categoryId = $('#ddlCategoryIdM').combotree('getValue');
            //var description = CKEDITOR.instances.txtDescM.getData();
            var value = $('#txtValue').val();
            //var originalPrice = $('#txtOriginalPriceM').val();
            //var image = $('#txtImage').val();
            //var brandName = $('#txtBrandName').val();
            var numberShipDay = $('#txtNumberShipDay').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Order/ReceivingTypeManagement?handler=SaveEntity",
                data: JSON.stringify({
                    Id: id,
                    Name: name,
                    Value: value,
                    NumberShipDay: numberShipDay
                    //BrandName: brandName,
                    //Quantity: quantity,
                    //Price: price,
                    //OriginalPrice: originalPrice,
                    //Description: description
                }),
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function (response) {
                    commons.notify('Thành công', 'success');
                    $('#modal-add-edit').modal('hide');
                    //resetFormMaintainance();
                    commons.stopLoading();
                    loadData(true);
                },
                error: function (response) {
                    commons.notify('Đã xảy ra lỗi', 'error');
                    commons.stopLoading();
                }
            });
            return false;
        }
    }

    function loadDetails(that) {
        $.ajax({
            type: "GET",
            url: "/Admin/Order/ReceivingTypeManagement?handler=ById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                //initDropzone(that);
                var data = response;
                $('#hidIdM').val(data.id);
                $('#txtNameM').val(data.name);
                $('#txtValue').val(data.value);
                $('#txtNumberShipDay').val(data.numberShipDay);
                //initTreeDropDownCategory(data.categoryId);
                //$('#txtPriceM').val(`${commons.formatNumber(data.price, 0)}`);
                //$('#txtOriginalPriceM').val(`${commons.formatNumber(data.originalPrice, 0)}`);
                //CKEDITOR.instances.txtDescM.setData(data.description);
                $('#modal-add-edit').modal('show');
                commons.stopLoading();
            },
            error: function (status) {
                commons.notify('Có lỗi xảy ra', 'error');
                commons.stopLoading();
            }
        });
    }


    var loadData = function () {
        var template = $('#table-template').html();
        var render = '';
        $.ajax({
            type: 'GET',
            //data: {
            //    "categoryId": $('#ddlCategorySearch').val(),
            //    "keyword": $('#txtKeyword').val(),
            //    "pageIndex": commons.configs.pageIndex,
            //    "pageSize": commons.configs.pageSize
            //},
            beforeSend: function () {
                commons.startLoading();
            },
            dataType: 'JSON',
            url: '/Admin/Order/ReceivingTypeManagement?handler=AllPaging',
            success: function (response) {
                debugger;
                $('#tbl-content').empty();
                $.each(response, function (i, item) {
                    render += Mustache.render(template,
                        {
                            Id: item.id,
                            Name: item.name,
                            Value: item.value,
                            NumberShipDay: item.numberShipDay,
                            //BrandName: item.brandName,
                            //Quantity: item.quantity,
                            //Price: `${commons.formatNumber(item.price, 0)}đ`,
                            CreatedDate: commons.dateTimeFormatJson(item.dateCreated)
                        });
                });
                $('#lblTotalRecords').text(response.rowCount);
                if (render !== '') {
                    $('#tbl-content').html(render);
                }
                //wrapPaging(response.rowCount,
                //    function () {
                //        loadData();
                //    }, isPageChanged);
            },
            error: function (status) {
                commons.notify('Không thể tải dữ liệu', 'error');
            },
            complete: function () {
                commons.stopLoading();
            }
        });
    };

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
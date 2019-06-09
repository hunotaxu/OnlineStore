var topSellingProducts = (function () {
    var init = function () {
        $(document).ready(function () {
            commons.initDateRangePicker();
            loadCategories();
            applyDateRange();
            onChangeCategory();
            onSearch();
            onResetDateRangePicker();
            loadData();
            //commons.applyDateRangePicker();
        });
    };

    var applyDateRange = function () {
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            //console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
            //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
            //var startDate = picker.startDate.format("DD/MM/YYYY");
            //var endDate = picker.endDate.format("DD/MM/YYYY");
            //var categoryId = $('#ddlCategorySearch').val();
            //var productName = $('#txtKeyword').val();
            loadData();
        });
    };

    var onChangeCategory = function () {
        $('#ddlCategorySearch').on('change', function () {
            //debugger;
            ////var startDate = $('#reportrange').data('datarangepicker').startDate.format("DD/MM/YYYY");
            //if ($('#reportrange span').text() !== '') {
            //    var startDate = $('#reportrange').data('daterangepicker').startDate.format("DD/MM/YYYY");
            //    var endDate = $('#reportrange').data('daterangepicker').endDate.format("DD/MM/YYYY");
            //}
            //var categoryId = $('#ddlCategorySearch').val();
            //var productName = $('#txtKeyword').val();
            loadData();
        });
    };

    var loadCategories = function () {
        $.ajax({
            url: '/Admin/Product/Index?handler=AllCategories',
            type: 'GET',
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var render = `<option value=''>Chọn loại sản phẩm</option>`;
                $.each(response, function (i, item) {
                    render += `<option value=${item.id}>${item.name}</option>`;
                });
                $('#ddlCategorySearch').html(render);
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu', 'error');
            }
        });
    };

    var onResetDateRangePicker = function () {

    };

    var onSearch = function () {
        $('#btnSearch').on('click', function () {
            //if ($('#reportrange span').text() !== '') {
            //    var startDate = $('#reportrange').data('daterangepicker').startDate.format("DD/MM/YYYY");
            //    var endDate = $('#reportrange').data('daterangepicker').endDate.format("DD/MM/YYYY");
            //}
            //var categoryId = $('#ddlCategorySearch').val();
            //var productName = $('#txtKeyword').val();
            loadData();
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData();
            }
        });
    };

    //var applyDateRangePicker = function () {
    //    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    //        //console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
    //        //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
    //        loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
    //    });
    //};

    function loadData(isPageChanged) {
        if ($('#reportrange span').text() !== '') {
            var startDate = $('#reportrange').data('daterangepicker').startDate.format("DD/MM/YYYY");
            var endDate = $('#reportrange').data('daterangepicker').endDate.format("DD/MM/YYYY");
        }
        var categoryId = $('#ddlCategorySearch').val();
        var productName = $('#txtKeyword').val();
        $.ajax({
            type: "GET",
            url: "/Admin/Reports/TopSellingProducts?handler=BestSellerProduct",
            data: {
                fromDate: startDate,
                toDate: endDate,
                categoryId: categoryId,
                productName: productName,
                pageIndex: commons.configs.pageIndex,
                pageSize: commons.configs.pageSize
            },
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (responses) {
                var template = $('#table-template').html();
                var render = '';
                $('#tbl-content').empty();
                $.each(responses, function (i, item) {
                    render += Mustache.render(template, {
                        ProductName: item.productName,
                        CategoryName: item.categoryName,
                        AmountTotal: `${commons.formatNumber(item.amountTotal, 0)}đ`,
                        QuantityTotal: item.quantityTotal
                    });
                });
                if (render !== '') {
                    $('#tbl-content').html(render);
                }
                $('#lblTotalRecords').text(Object.keys(responses).length);
                //commons.wrapPaging(Object.keys(responses).length, loadData(), commons.configs.pageSize);
                wrapPaging(Object.keys(responses).length,
                    function () {
                        loadData();
                    }, isPageChanged);
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu', 'error');
                commons.stopLoading();
            }
        });
    }
    function wrapPaging(recordCount, callBack, changePageSize) {
        var totalSize = Math.ceil(recordCount / commons.configs.pageSize);
        // Unbind pagination if it existed or click change page size
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        // Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalSize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                commons.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
    //function loadData(from, to, cateId, prodName) {
    //    debugger;
    //    $.ajax({
    //        type: 'GET',
    //        url: "/Admin/Reports/TopSellingProducts?handler=BestSellerProductAsync",
    //        //dataType: 'json',
    //        //contentType: 'application/json;charset=utf8',
    //        //contentType: "application/json",
    //        data: {
    //            fromDate: from,
    //            toDate: to
    //            //categoryId: cateId,
    //            //productName: prodName
    //        },
    //        beforeSend: function () {
    //            commons.startLoading();
    //        },
    //        success: function (response) {
    //            debugger;
    //            commons.stopLoading();
    //        },
    //        error: function (response) {
    //            debugger;
    //            commons.notify('Không tải được dữ liệu', 'error');
    //            commons.stopLoading();
    //        }
    //    });
    //}

    return {
        init
    };
})();
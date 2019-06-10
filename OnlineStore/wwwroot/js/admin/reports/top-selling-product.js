var topSellingProducts = (function () {
    var init = function () {
        $(document).ready(function () {
            commons.initDateRangePicker();
            loadCategories();
            applyDateRange();
            onChangeCategory();
            onSearch();
            onResetDateRangePicker();
            loadData(true);
        });
    };

    var applyDateRange = function () {
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            loadData(true);
        });
    };

    var onChangeCategory = function () {
        $('#ddlCategorySearch').on('change', function () {
            loadData(true);
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
            loadData(true);
        });
        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });
    };

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
            success: function (response) {
                var template = $('#table-template').html();
                var render = '';
                $('#tbl-content').empty();
                $.each(response.results, function (i, item) {
                    render += Mustache.render(template, {
                        ProductName: item.productName,
                        CategoryName: item.categoryName,
                        AmountTotal: (item.amountTotal === undefined) ? '0đ' : `${commons.formatNumber(item.amountTotal, 0)}đ`,
                        QuantityTotal: item.quantityTotal
                    });
                });
                if (render !== '') {
                    $('#tbl-content').html(render);
                }
                $('#lblTotalRecords').text(response.rowCount);
                wrapPaging(response.rowCount,
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
        var totalSize = 1;
        if (recordCount !== 0) {
            totalSize = Math.ceil(recordCount / commons.configs.pageSize);
        }

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
    
    return {
        init
    };
})();
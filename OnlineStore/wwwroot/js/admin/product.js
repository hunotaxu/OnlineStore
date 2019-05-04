var itemPage = (function () {
    var init = function () {
        $(document).ready(function () {
            loadCategories();
            loadData();
            registerEvents();
            onSearchEvents();
        });
    };

    var onSearchEvents = function() {
        $('#btnSearch').on('click',
            function () {
                loadData();
            });

        $('#txtKeyword').on('keypress',
            function(e) {
                if (e.which === 13) {
                    loadData();
                }
            });
    };

    var registerEvents = function () {
        $(document).ready(function () {
            $('#ddlShowPage').on('change',
                function () {
                    commons.configs.pageSize = $(this).val();
                    commons.configs.pageIndex = 1;
                    loadData(true);
                });
        });
    };

    var loadCategories = function () {
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            url: '/Admin/Products/Index?handler=AllCategories',
            success: function (response) {
                var render = `<option value=''>Chọn loại sản phẩm</option>`;
                $.each(response, function (i, item) {
                    render += `<option value='${item.id}'>${item.name}</option>`;
                });
                $('#ddlCategorySearch').html(render);
            },
            error: function (response) {
                commons.notify('Không thể tải dữ liệu', 'error');
            }
        });
    };

    var loadData = function (isPageChanged) {
        var template = $('#table-template').html();
        var render = '';
        $.ajax({
            type: 'GET',
            data: {
                "categoryId": $('#ddlCategorySearch').val(),
                "keyword": $('#txtKeyword').val(),
                "pageIndex": commons.configs.pageIndex,
                "pageSize": commons.configs.pageSize
            },
            dataType: 'JSON',
            url: '/Admin/Products/Index?handler=AllPaging',
            success: function (response) {
                if (response.authenticate === false) {
                    window.location.href = "/Identity/Account/AccessDenied";
                }
                $('#tbl-content').empty();
                $.each(response.results, function (i, item) {
                    render += Mustache.render(template,
                        {
                            Id: item.id,
                            Name: item.name,
                            Image: item.image ? `<img src='~/images/client/ProductImages/${item.image}' width=25 />` : `<img src='/images/admin/user.png' width=25 />`,
                            CategoryName: item.category.name,
                            Price: `${commons.formatNumber(item.price, 0)}đ`,
                            CreatedDate: commons.dateTimeFormatJson(item.dateCreated),
                            Status: commons.getStatus(item.status)
                        });
                });
                $('#lblTotalRecords').text(response.rowCount);
                if (render !== '') {
                    $('#tbl-content').html(render);
                }
                wrapPaging(response.rowCount,
                    function () {
                        loadData();
                    }, isPageChanged);
            },
            error: function (status) {
                commons.notify('Không thể tải dữ liệu', 'error');
            }
        });
    };

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
    };

    return {
        init,
        loadData,
        registerEvents
    };
})();
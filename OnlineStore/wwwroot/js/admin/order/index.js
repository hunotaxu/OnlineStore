var order = (function () {
    var init = function () {
        $(document).ready(function () {
            onLoadDefaultOrderStatus();
            registerEvents();
            loadData();
        });
    };

    var onLoadDefaultOrderStatus = function () {
        $('#ddlOrderStatus option[value="1"]').prop('selected', true);
    };

    var registerEvents = function () {
        $('#ddlShowPage').on('change', function () {
            commons.configs.pageSize = $(this).val();
            commons.configs.pageIndex = 1;
            loadData(true);
        });
        $('#ddlReceivingType').on('change', function () {
            loadData(true);
        });
        $('#ddlOrderStatus').on('change', function () {
            loadData(true);
        });


        $('#btnSearch').click(function () {
            alert("Handler for .click() called.");
            loadData();
        });

        $('#txtKeyword').on('keypress', function (e) {
            if (e.which === 13) {
                e.preventDefault();
                loadData(true);
            }
        });
    };

    var loadData = function (isPageChanged) {
        var template = $('#table-template').html();
        var render = '';
        $.ajax({
            type: 'GET',
            data: {
                "orderStatus": $('#ddlOrderStatus').val(),
                "receivingTypeId": $('select#ddlReceivingType').children('option:selected').val(),
                "keyword": $('#txtKeyword').val(),
                "pageIndex": commons.configs.pageIndex,
                "pageSize": commons.configs.pageSize
            },
            beforeSend: function () {
                commons.startLoading();
            },
            dataType: 'JSON',
            url: '/Admin/Order/Index?handler=AllPaging',
            success: function (response) {
                if (response.authenticate === false) {
                    window.location.href = "/Identity/Account/AccessDenied";
                }
                $('#tbl-content').empty();
                $.each(response.results, function (i, item) {
                    render += Mustache.render(template,
                        {
                            Id: item.id,
                            OrderDate: commons.dateTimeFormatJson(item.orderDate),
                            ReceivingType: item.receivingType.name,
                            Status: getOrderStatus(item.status)
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
            },
            complete: function () {
                commons.stopLoading();
            }
        });
    };

    function getOrderStatus(orderStatus) {
        switch (orderStatus) {
            case 1:
                return `<span class='badge bg-orange'>Đang chờ xử lý</span>`;
            case 2:
                return `<span class='badge bg-blue'>Sẵn sàng để giao</span>`;
            case 3:
                return `<span class='badge bg-sky-blue'>Đang vận chuyển</span>`;
            case 4:
                return `<span class='badge bg-green'>Đã giao</span>`;
            case 5:
                return `<span class='badge bg-red'>Đã hủy</span>`;
        }
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
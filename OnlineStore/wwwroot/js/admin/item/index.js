var itemPage = (function () {
    var init = function () {
        $(document).ready(function () {

        });
    };

    var loadData = function () {
        $(document).ready(function () {
            var template = $('#table-template').html();
            var render = '';
            $.ajax({
                type: 'GET',
                //data: {
                //    categoryId: $('#ddlCategorySearch').val(),
                //    keyword: $('#txtKeyword').val(),
                //    page: commons.configs.pageIndex,
                //    pageSize: commons.configs.pageSize
                //},
                dataType: 'JSON',
                url: '/Admin/Products/Index?handler=LoadAll',
                success: function (response) {
                    $.each(response, function (i, item) {
                        render += Mustache.render(template,
                            {
                                Id: item.Id,
                                Name: item.name,
                                Image: item.image ? `<img src='/images/client/ProductImages/${item.image}' width=25 />` : `<img src='/images/admin/user.png' width=25 />`,
                                CategoryName: item.category.name,
                                Price: `${commons.formatNumber(item.price, 0)}đ`,
                                CreatedDate: commons.dateTimeFormatJson(item.dateCreated),
                                Status: commons.getStatus(item.status)
                            });
                        if (render !== '') {
                            $('#tbl-content').html(render);
                        }
                    });
                },
                error: function (status) {
                    commons.notify('Cannot loading data', 'error');
                }
            });
        });
    };

    return {
        init,
        loadData
    };
})();
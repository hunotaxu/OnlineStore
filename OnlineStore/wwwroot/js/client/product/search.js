var search = (function () {
    var init = function () {
        $(document).ready(function () {
            //formatPrice();
            loadData();
            onSortProducts();
            //changeBrandNames();
        });
    };

    var onSortProducts = function sortProduct() {
        var listBrandName = [];
        $('#SearchProductViewModel_Sort').on('change', function () {
            loadData(true);
        });
        //$('#CurrentPage').val(1);
        commons.configs.pageIndex = 1;
        //$.ajax({
        //    type: "GET",
        //    url: "/Product/Index?handler=Search",
        //    data: {
        //        "currentSearchString": "@Model.CurrentSearchString",
        //        "currentSort": $('#CurrentSort option:selected').val(),
        //        "currentBrand": listBrandName
        //    }
        //}).done(function (result) {
        //    $(".bottom-product").html(result);
        //}).fail(function (result) {
        //    console.log(result);
        //});
    };

    //var changeBrandNames = function () {
    //    $("input[type=checkbox]").change(function () {
    //        var brand = $(this).closest("li").text();
    //        $('#CurrentPage').val(1);
    //        if (this.checked) {
    //            listBrandName.push(brand);
    //        } else {
    //            listBrandName.splice(listBrandName.indexOf(brand, 1));
    //        }
    //        $.ajax({
    //            type: "GET",
    //            url: "/Product/Index?handler=Search",
    //            beforeSend: function (xhr) {
    //                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
    //            },
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "html",
    //            cache: false,
    //            data: {
    //                "currentSearchString": "@Model.CurrentSearchString",
    //                "currentSort": $('#CurrentSort option:selected').val(),
    //                "currentBrand": listBrandName
    //            }
    //        }).done(function (result) {
    //            $(".bottom-product").empty();
    //            $(".bottom-product").html(result);
    //        }).fail(function (result) {
    //            console.log(result);
    //        });
    //    });
    //};

    var loadData = function (isPageChanged) {
        var template = $('#search-result-template').html();
        var render = '';
        $.ajax({
            type: 'GET',
            data: {
                "Sort": $('#SearchProductViewModel_Sort').val(),
                "SearchString": $('#search-string').val(),
                //"Rating": "4",
                "pageIndex": commons.configs.pageIndex,
                "pageSize": commons.configs.pageSize
                //"MinPrice": "",
                //"MaxPrice": "",
            },
            dataType: 'JSON',
            url: '/Product/Search?handler=AllPaging',
            success: function (response) {
                $('.products-grid').empty();
                $.each(response.results, function (i, item) {
                    debugger;
                    render += Mustache.render(template,
                        {
                            Image: (item.productImages !== undefined && item.productImages.length > 0) ?
                                `/images/client/ProductImages/${item.productImages[0].name}` : `/images/client/ProductImages/default-image.jpg`,
                            ProductName: item.name,
                            ProductId: item.id,
                            AvgRating: (item.averageEvaluation !== undefined && item.averageEvaluation !== '' && item.averageEvaluation !== null) ?
                                `Đánh giá: ${item.averageEvaluation}/5` : 'Chưa có đánh giá nào',
                            OriginalPrice: `${commons.formatNumber(item.originalPrice, 0)}đ`,
                            Price: `${commons.formatNumber(item.price, 0)}đ`
                        });
                });
                $('#lblTotalRecords').text(response.rowCount);
                if (render !== '') {
                    $('.products-grid').html(render);
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
    };

    var formatPrice = function () {
        //$("#min-price").on({
        //    keyup: function () {
        //        commons.formatCurrency($(this));
        //    }
        //    //},
        //    //blur: function () {
        //    //    commons.formatCurrency($(this), "blur");
        //    //}
        //});
    };

    return {
        init
    };
})();
var search = (function () {
    var categoryId = $('#category-id').val();
    var rating = '';
    var minPrice = '';
    var maxPrice = '';
    var listBrandNamesSelected = [];
    jQuery.ajaxSettings.traditional = true;
    var init = function () {
        $(document).ready(function () {
            //formatPrice();
            loadData();
            onSortProducts();
            changeBrandNames();
            filterByRating();
            filterByCategoryName();
            filterByPriceRange();
            preventNegativeNumber();
            addToCart();
        });
    };

    var onSortProducts = function sortProduct() {
        $('#SearchProductViewModel_Sort').on('change', function () {
            commons.configs.pageIndex = 1;
            loadData(true);
        });
    };

    var filterByPriceRange = function () {
        $('body').on('click', '#btn-price-range', function () {
            commons.configs.pageIndex = 1;
            minPrice = $('#min-price').val();
            maxPrice = $('#max-price').val();
            loadData(true);
        });
    };

    var filterByCategoryName = function () {
        $('body').on('click', '.category-filter', function (e) {
            e.preventDefault();
            categoryId = $(this).data('category-id');
            commons.configs.pageIndex = 1;
            loadData(true);
        });
    };

    var filterByRating = function () {
        $('body').on('click', '.rating', function (e) {
            e.preventDefault();
            rating = $(this).data('rating-number');
            commons.configs.pageIndex = 1;
            loadData(true);
        });
    };

    var preventNegativeNumber = function () {
        $("body").delegate('#min-price', 'focusout', function () {
            if ($('#min-price').val() < 0) {
                $('#min-price').val('0');
            }
        });

        $("body").delegate('#max-price', 'focusout', function () {
            if ($('#min-price').val() !== undefined && $('#min-price').val() !== '') {
                if ($('#max-price').val() < $('#min-price').val()) {
                    $('#max-price').val($('#min-price').val());
                }
            }
            else {
                if ($('#max-price').val() < 0) {
                    $('#max-price').val('0');
                }
            }
        });
    };

    var changeBrandNames = function () {
        $('body').on('change', `input[name='BrandName']`, function () {
            commons.configs.pageIndex = 1;
            var brand = $(this).val();
            if (this.checked) {
                listBrandNamesSelected.push(brand);
            } else {
                listBrandNamesSelected.splice(listBrandNamesSelected.indexOf(brand, 1));
            }
            loadData(true);
        });
    };

    var addToCart = function () {
        $('body').on('click', '#btnAddToCart', function (e) {
            //$('#btnAddToCart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            //if ($('#datahidden-cus').data('customerid') === '' || $('#datahidden-cus').data('customerid') === undefined) {
            if ($('#user-id').val() === '' || $('#user-id').val() === undefined) {
                commons.confirm('Bạn chưa đăng nhập, bạn có muốn chuyển tiếp sang trang đăng nhập?', function () {
                    window.location.replace(`/Identity/Account/Login?returnUrl=/Product/Detail?id=${id}`);
                });
            }
            else {
                $.ajax({
                    url: "/Product/AddToCart?handler=AddToCart",
                    type: "POST",
                    dataType: "json",
                    contentType: 'application/json;charset=utf-8',
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    data: JSON.stringify({
                        ItemId: id,
                        Quantity: 1
                    }),
                    success: function () {
                        commons.notify('Thêm vào giỏ hàng thành công', 'success');
                        loadItemMyCart.init();
                    },
                    error: function (response) {
                        debugger;
                        if (response.responseText !== undefined && response.responseText !== '') {
                            commons.notify(response.responseText, 'error');
                        }
                        else {
                            commons.notify('Đã có lỗi xãy ra', 'error');
                        }
                    },
                    complete: function () {
                        commons.stopLoading();
                    }
                });
            }
        });
    };

    var loadData = function (isPageChanged) {
        var template = $('#search-result-template').html();
        var render = '';
        var renderCategories = '';
        var renderbrandNames = '';
        $.ajax({
            type: 'GET',
            data: {
                "Sort": $('#SearchProductViewModel_Sort').val(),
                "SearchString": $('#search-string').val(),
                "Rating": rating,
                "CategoryId": categoryId,
                "pageIndex": commons.configs.pageIndex,
                "pageSize": commons.configs.pageSize,
                "Brand": listBrandNamesSelected,
                "MinPrice": minPrice,
                "MaxPrice": maxPrice
            },
            //dataType: 'JSON',
            contentType: 'application/json; charset=utf-8',
            url: '/Product/Search?handler=AllPaging',
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                if (response !== undefined && response !== '') {
                    $('.products-grid').empty();
                    var listCategoryName = [];
                    var listBrandNames = [];
                    $.each(response.all, function (i, item) {
                        if (categoryId === '' || categoryId === undefined) {
                            if (!listCategoryName.includes(item.category.name)) {
                                renderCategories += Mustache.render($('#categoies-template').html(), {
                                    CategoryName: item.category.name,
                                    CategoryId: item.category.id
                                });
                                listCategoryName.push(item.category.name);
                            }
                        }
                        if (listBrandNamesSelected.length === 0) {
                            if (!listBrandNames.includes(item.brandName)) {
                                renderbrandNames += Mustache.render($('#brand-names-template').html(), {
                                    BrandName: item.brandName
                                });
                                listBrandNames.push(item.brandName);
                            }
                        }
                        if (renderCategories !== '') {
                            $('#list-categories').html(renderCategories);
                        }

                        if (renderbrandNames !== '') {
                            $('#list-brand-names').html(renderbrandNames);
                        }
                    });
                    $.each(response.results, function (i, item) {
                        render += Mustache.render(template,
                            {
                                Image: (item.productImages !== undefined && item.productImages.length > 0) ?
                                    `/images/client/ProductImages/${item.productImages[0].name}` : `/images/client/ProductImages/no-image.jpg`,
                                ProductName: item.name,
                                ProductId: item.id,
                                AvgRating: (item.averageEvaluation !== undefined && item.averageEvaluation !== '' && item.averageEvaluation !== null) ?
                                    `Đánh giá: ${item.averageEvaluation}/5` : 'Chưa có đánh giá nào',
                                OriginalPrice: `${commons.formatNumber(item.originalPrice, 0)}đ`,
                                Price: `${commons.formatNumber(item.price, 0)}đ`
                            });
                        //if (categoryId === '' || categoryId === undefined) {
                        //    if (!listCategoryName.includes(item.category.name)) {
                        //        renderCategories += Mustache.render($('#categoies-template').html(), {
                        //            CategoryName: item.category.name,
                        //            CategoryId: item.category.id
                        //        });
                        //        listCategoryName.push(item.category.name);
                        //    }
                        //}
                        //if (listBrandNamesSelected.length === 0) {
                        //    if (!listBrandNames.includes(item.brandName)) {
                        //        renderbrandNames += Mustache.render($('#brand-names-template').html(), {
                        //            BrandName: item.brandName
                        //        });
                        //        listBrandNames.push(item.brandName);
                        //    }
                        //}

                    });
                    $('#lblTotalRecords').text(response.rowCount);

                    if (render !== '') {
                        $('.products-grid').html(render);
                        wrapPaging(response.rowCount,
                            function () {
                                loadData();
                            }, isPageChanged);

                    } else {
                        //$('.container__row--center').html(`<div style='text-align: center;'><h3>Không có sản phẩm nào trong giỏ hàng</h3><a href='/' class='btn btn-warning'>Tiếp tục mua sắm</a></div>`);
                        $('.products-grid').html(`<br /><br /><div class="clol-md-5 center" ><h3 style="text-align: center; font-family: 'Roboto','Montserrat', sans-serif; font-weight: 400; font-size: 24;">Xin lỗi, chúng tôi không thể tìm được kết quả hợp với tìm kiếm của bạn</h3></div>`);
                        $('#paginationUL').empty();
                    }

                    //if (renderCategories !== '') {
                    //    $('#list-categories').html(renderCategories);
                    //}

                    //if (renderbrandNames !== '') {
                    //    $('#list-brand-names').html(renderbrandNames);
                    //}
                }
            },
            error: function (status) {
                commons.notify('Không thể tải dữ liệu', 'error');

            },
            complete: function () {
                commons.stopLoading();
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
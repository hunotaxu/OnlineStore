var itemPage = (function () {
    var init = function () {
        $(document).ready(function () {
            loadCategories();
            loadData();
            registerEvents();
            registerControls();
            onSearchEvents();
            initDropzone();
        });
    };

    var initDropzone = function () {
        // required dropzone.js
        Dropzone.autoDiscover = false;
        $("#dzUpload").dropzone({
            url: "/Admin/Upload/TemporaryStoreAttachment",
            maxFiles: 5,
            maxFilesize: 10, //MB
            thumbnailWidth: 80,
            thumbnailHeight: 80,
            acceptedFiles: "image/*",
            addRemoveLinks: true,
            success: function (file, response) {
                if (response === "duplicated") {
                    var _ref = file.previewElement;
                    if (_ref) {
                        removeThumbnail(file);
                    }
                } else {
                    file.previewElement.classList.add("dz-success");
                }
            },
            removedfile: function (file) { removeAttachment(file); },
            error: function (file, response) {
                removeThumbnail(file);
                $(generalError).text(response);
            }
        });
    };

    var removeAttachment = function (file) {
        if (file) {
            $.ajax({
                url: '/Admin/Upload/TemporaryRemoveAttachment',
                data: { fileName: file.name },
                success: function () {
                    removeThumbnail(file);
                    $(generalError).text("");
                },
                error: function (file, response) {
                    $(generalError).text(response);
                }
            });
        }
    };

    var removeThumbnail = function (file) {
        var _ref = file.previewElement;
        if (_ref) {
            _ref.parentNode.removeChild(file.previewElement);
        }
    };

    var onSearchEvents = function () {
        $('#btnSearch').on('click',
            function () {
                loadData();
            });

        $('#txtKeyword').on('keypress',
            function (e) {
                if (e.which === 13) {
                    loadData();
                }
            });
    };

    var registerEvents = function () {
        $(document).ready(function () {
            // Init validation
            $('#frmMaintainance').validate({
                errorClass: 'red',
                ignore: [],
                lang: 'vi',
                rules: {
                    txtNameM: { required: true },
                    ddlCategoryIdM: { required: true },
                    txtPriceM: {
                        required: true,
                        number: true
                    },
                    txtPromotionPriceM: {
                        required: true,
                        number: true
                    }
                }
            });
            //todo: binding events to controls
            $('#ddlShowPage').on('change', function () {
                commons.configs.pageSize = $(this).val();
                commons.configs.pageIndex = 1;
                loadData(true);
            });

            $('#btnSearch').on('click', function () {
                loadData();
            });

            $('#txtKeyword').on('keypress', function (e) {
                if (e.which === 13) {
                    loadData();
                }
            });

            $("#btnCreate").on('click', function () {
                resetFormMaintainance();
                initTreeDropDownCategory();
                $('#modal-add-edit').modal('show');
            });

            //$('#btnSelectImg').on('click', function () {
            //    $('#fileInputImage').click();
            //});

            //$("#fileInputImage").on('change', function () {
            //    var fileUpload = $(this).get(0);
            //    var files = fileUpload.files;
            //    var data = new FormData();
            //    for (var i = 0; i < files.length; i++) {
            //        data.append(files[i].name, files[i]);
            //    }
            //    $.ajax({
            //        type: "POST",
            //        url: "/Admin/Upload/UploadImage",
            //        contentType: false,
            //        processData: false,
            //        data: data,
            //        success: function (path) {
            //            $('#txtImage').val(path);
            //            commons.notify('Tải ảnh thành công!', 'success');

            //        },
            //        error: function () {
            //            commons.notify('Đã có lỗi xãy ra', 'error');
            //        }
            //    });
            //});

            $('body').on('click', '.btn-edit', function (e) {
                e.preventDefault();
                var that = $(this).data('id');
                loadDetails(that);
            });

            $('body').on('click', '.btn-delete', function (e) {
                e.preventDefault();
                var that = $(this).data('id');
                commons.confirm('Bạn có chắc chắn muốn xóa?', function () {
                    $.ajax({
                        type: "GET",
                        url: "/Admin/Product/Index?handler=Delete",
                        data: { id: that },
                        beforeSend: function () {
                            commons.startLoading();
                        },
                        success: function (response) {
                            commons.notify('Thành công', 'success');
                            commons.stopLoading();
                            loadData();
                        },
                        error: function (status) {
                            commons.notify('Đã có lỗi xãy ra', 'error');
                            commons.stopLoading();
                        }
                    });
                });
            });

            $('#btnSave').on('click', function (e) {
                saveProduct(e);
            });

            $('#btn-import').on('click', function () {
                initTreeDropDownCategory();
                $('#modal-import-excel').modal('show');
            });

            $('#btnImportExcel').on('click', function () {
                var fileUpload = $("#fileInputExcel").get(0);
                var files = fileUpload.files;

                // Create FormData object  
                var fileData = new FormData();
                // Looping over all files and add it to FormData object  
                for (var i = 0; i < files.length; i++) {
                    fileData.append("files", files[i]);
                }
                // Adding one more key to FormData object  
                fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
                $.ajax({
                    url: '/Admin/Product/ImportExcel',
                    type: 'POST',
                    data: fileData,
                    processData: false,  // tell jQuery not to process the data
                    contentType: false,  // tell jQuery not to set contentType
                    success: function (data) {
                        $('#modal-import-excel').modal('hide');
                        loadData();

                    }
                });
                return false;
            });

            $('#btn-export').on('click', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/Product/ExportExcel",
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    success: function (response) {
                        window.location.href = response;
                        commons.stopLoading();
                    },
                    error: function () {
                        commons.notify('Has an error in progress', 'error');
                        commons.stopLoading();
                    }
                });
            });
        });
    };

    function registerControls() {
        CKEDITOR.replace('txtDescM', {});

        //Fix: cannot click on element ck in modal
        $.fn.modal.Constructor.prototype.enforceFocus = function () {
            $(document)
                .off('focusin.bs.modal') // guard against infinite focus loop
                .on('focusin.bs.modal', $.proxy(function (e) {
                    if (
                        this.$element[0] !== e.target && !this.$element.has(e.target).length
                        // CKEditor compatibility fix start.
                        && !$(e.target).closest('.cke_dialog, .cke').length
                        // CKEditor compatibility fix end.
                    ) {
                        this.$element.trigger('focus');
                    }
                }, this));
        };
    }

    function saveProduct(e) {
        if ($('#frmMaintainance').valid()) {
            e.preventDefault();
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            var categoryId = $('#ddlCategoryIdM').combotree('getValue');

            var description = CKEDITOR.instances.txtDescM.getData();
            //var unit = $('#txtUnitM').val();

            var price = $('#txtPriceM').val();
            //var originalPrice = $('#txtOriginalPriceM').val();
            var promotionPrice = $('#txtPromotionPriceM').val();

            var image = $('#txtImage').val();

            //var tags = $('#txtTagM').val();
            //var seoKeyword = $('#txtMetakeywordM').val();
            //var seoMetaDescription = $('#txtMetaDescriptionM').val();
            //var seoPageTitle = $('#txtSeoPageTitleM').val();
            //var seoAlias = $('#txtSeoAliasM').val();

            //var content = CKEDITOR.instances.txtDescM.getData();
            //var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
            //var hot = $('#ckHotM').prop('checked');
            //var showHome = $('#ckShowHomeM').prop('checked');

            $.ajax({
                type: "POST",
                url: "/Admin/Product/Index?handler=SaveEntity",
                data: JSON.stringify({
                    Id: id,
                    Name: name,
                    CategoryId: categoryId,
                    Image: image,
                    Price: price,
                    //OriginalPrice: originalPrice,
                    PromotionPrice: promotionPrice,
                    Description: description
                    //Content: content,
                    //HomeFlag: showHome,
                    //HotFlag: hot,
                    //Tags: tags,
                    //Unit: unit,
                    //Status: status,
                    //SeoPageTitle: seoPageTitle,
                    //SeoAlias: seoAlias,
                    //SeoKeywords: seoKeyword,
                    //SeoDescription: seoMetaDescription
                }),
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function (response) {
                    commons.notify('Thành công', 'success');
                    $('#modal-add-edit').modal('hide');
                    resetFormMaintainance();
                    commons.stopLoading();
                    loadData(true);
                },
                error: function (response) {
                    commons.notify('Đã có lỗi xãy ra', 'error');
                    commons.stopLoading();
                }
            });
            return false;
        }
    }

    function loadDetails(that) {
        $.ajax({
            type: "GET",
            url: "/Admin/Product/Index?handler=ById",
            data: { id: that },
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var data = response;
                $('#hidIdM').val(data.id);
                $('#txtNameM').val(data.name);
                initTreeDropDownCategory(data.categoryId);

                //$('#txtDescM').val(data.description);
                //$('#txtUnitM').val(data.Unit);

                $('#txtPriceM').val(data.price);
                //$('#txtOriginalPriceM').val(data.OriginalPrice);
                $('#txtPromotionPriceM').val(data.promotionPrice);

                // $('#txtImage').val(data.ThumbnailImage);

                //$('#txtTagM').val(data.Tags);
                //$('#txtMetakeywordM').val(data.SeoKeywords);
                //$('#txtMetaDescriptionM').val(data.SeoDescription);
                //$('#txtSeoPageTitleM').val(data.SeoPageTitle);
                //$('#txtSeoAliasM').val(data.SeoAlias);

                CKEDITOR.instances.txtDescM.setData(data.description);
                //$('#ckStatusM').prop('checked', data.status === 1);
                //$('#ckHotM').prop('checked', data.HotFlag);
                //$('#ckShowHomeM').prop('checked', data.HomeFlag);

                $('#modal-add-edit').modal('show');
                commons.stopLoading();
            },
            error: function (status) {
                commons.notify('Có lỗi xảy ra', 'error');
                commons.stopLoading();
            }
        });
    }

    function initTreeDropDownCategory(selectedId) {
        $.ajax({
            url: "/Admin/Category/Index?handler=All",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response, function (i, item) {
                    data.push({
                        id: item.id,
                        text: item.name,
                        parentId: item.parentId,
                        sortOrder: item.sortOrder
                    });
                });
                var arr = commons.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr,
                    'panelWidth': 'auto',
                    'panelHeight': 'auto'
                });

                $('#ddlCategoryIdImportExcel').combotree({
                    data: arr,
                    'panelWidth': 'auto',
                    'panelHeight': 'auto'
                });
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    }

    function resetFormMaintainance() {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        //$('#txtDescM').val('');
        //$('#txtUnitM').val('');

        $('#txtPriceM').val('0');
        //$('#txtOriginalPriceM').val('');
        $('#txtPromotionPriceM').val('0');

        $('#txtImage').val('');

        //$('#txtTagM').val('');
        //$('#txtMetakeywordM').val('');
        //$('#txtMetaDescriptionM').val('');
        //$('#txtSeoPageTitleM').val('');
        //$('#txtSeoAliasM').val('');

        CKEDITOR.instances.txtDescM.setData('');
        //$('#ckStatusM').prop('checked', true);
        //$('#ckHotM').prop('checked', false);
        //$('#ckShowHomeM').prop('checked', false);

    }

    var loadCategories = function () {
        $.ajax({
            type: 'GET',
            dataType: 'JSON',
            url: '/Admin/Product/Index?handler=AllCategories',
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
            url: '/Admin/Product/Index?handler=AllPaging',
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
                            Image: item.image ? `<img src='/images/client/ProductImages/${item.image}' width=25 />` : `<img src='/images/admin/user.png' width=25 />`,
                            CategoryName: item.category.name,
                            Price: `${commons.formatNumber(item.price, 0)}đ`,
                            CreatedDate: commons.dateTimeFormatJson(item.dateCreated)
                            //Status: commons.getStatus(item.status)
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
        initDropzone
    };
})();
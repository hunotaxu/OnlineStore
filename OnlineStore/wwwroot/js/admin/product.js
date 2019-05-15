var itemPage = (function () {

    var init = function () {
        Dropzone.autoDiscover = false;
        $(document).ready(function () {
            initDropzone();
            loadCategories();
            loadData();
            registerEvents();
            registerControls();
            onSearchEvents();
        });
    };

    var clearDropzone = function () {
        $('.dropzone').each(function () {
            let dropzoneControl = $(this)[0].dropzone;
            if (dropzoneControl) {
                dropzoneControl.destroy();
            }
        });
    };

    var initDropzone = function (productId) {
        Dropzone.autoDiscover = false;
        var existDropzone = $("#dzUpload")[0].dropzone;
        if (existDropzone) {
            clearDropzone();
        }
        $("#dzUpload").dropzone({
            init: function () {
                var myDropzone = this;
                //Call the action method to load the images from the server
                if (productId !== 0 && productId !== null && productId !== undefined) {
                    $.getJSON('/Admin/Product/Index?handler=LoadAttachments', { 'productId': productId }).done(function (data) {
                        if (data.attachmentsList !== undefined && data.attachmentsList.length > 0) {
                            $.each(data.attachmentsList, function (index, item) {
                                //// Create the mock file:
                                var mockFile = {
                                    name: item.name
                                };
                                // Call the default addedfile event handler
                                myDropzone.emit("addedfile", mockFile);
                                // And optionally show the thumbnail of the file:
                                myDropzone.emit("thumbnail", mockFile, `/images/client/ProductImages/${item.name}`);
                                $(".dz-size").remove();
                                $('.dz-progress').remove();
                                myDropzone.files.push(mockFile);
                            });
                        }
                    });
                }
            },
            url: "/Admin/Upload/TemporaryStoreAttachment",
            maxFiles: 5,
            maxFilesize: 10, // MB
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
                initDropzone(0);
                resetFormMaintainance();
                initTreeDropDownCategory();
                $('#modal-add-edit').modal('show');
            });

            $('body').on('click', '.btn-edit', function (e) {
                e.preventDefault();
                var that = $(this).data('id');
                loadDetails(that);
            });

            $('body').on('click', '.btn-delete', function (e) {
                e.preventDefault();
                var that = $(this).data('id');
                deleteProduct(that);
            });

            $('#btnSave').on('click', function (e) {
                saveProduct(e);
            });

            $('#btn-import').on('click', function () {
                initTreeDropDownCategory();
                $('#modal-import-excel').modal('show');
            });

            $('#btnImportExcel').on('click', function () {
                var files = $('#fileInputExcel').get(0).files;
                var fileData = new FormData();
                for (var i = 0; i < files.length; i++) {
                    fileData.append('files', files[i]);
                }
                fileData.append('categoryId', $('#ddlCategoryIdImportExcel').combotree('getValue'));
                $.ajax({
                    url: '/Admin/Product/Index?handler=ImportExcel',
                    type: 'post',
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    data: fileData,
                    processData: false, // default is true, 
                    contentType: false, // not set content type, nếu không có dòng này sẽ xãy ra lỗi error 500
                    success: function (data) {
                        $('#modal-import-excel').modal('hide');
                        commons.notify('Nhập sản phẩm thành công', 'success');
                        commons.stopLoading();
                        loadData(true);
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
                        commons.notify('Đã xãy ra lỗi', 'error');
                        commons.stopLoading();
                    }
                });
            });
        });
    };

    function deleteProduct(that) {
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
    }

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
            var price = $('#txtPriceM').val();
            var promotionPrice = $('#txtPromotionPriceM').val();
            var image = $('#txtImage').val();
            var brandName = $('#txtBrandName').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Product/Index?handler=SaveEntity",
                data: JSON.stringify({
                    Id: id,
                    Name: name,
                    CategoryId: categoryId,
                    Image: image,
                    BrandName: brandName,
                    Price: price,
                    PromotionPrice: promotionPrice,
                    Description: description
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
                initDropzone(that);
                var data = response;
                $('#hidIdM').val(data.id);
                $('#txtNameM').val(data.name);
                $('#txtBrandName').val(data.brandName);
                initTreeDropDownCategory(data.categoryId);
                $('#txtPriceM').val(data.price);
                $('#txtPromotionPriceM').val(data.promotionPrice);
                CKEDITOR.instances.txtDescM.setData(data.description);
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
                            CategoryName: item.category.name,
                            BrandName: item.brandName,
                            Price: `${commons.formatNumber(item.price, 0)}đ`,
                            CreatedDate: commons.dateTimeFormatJson(item.dateCreated)
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
        init
    };
})();
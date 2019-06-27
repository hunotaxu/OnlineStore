var itemPage = (function () {
    var init = function () {
        onTypeCurrency();
        Dropzone.autoDiscover = false;
        $(document).ready(function () {
            initDropzone();
            loadCategories();
            loadData();
            registerEvents();
            registerControls();
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
                //$(generalError).text(response);
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
                    //$(generalError).text("");
                },
                error: function (file, response) {
                    //$(generalError).text(response);
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

    var registerEvents = function () {
        $(document).ready(function () {
            // Init validation
            //$('#frmMaintainance').validate({
            //    errorClass: 'red',
            //    ignore: [],
            //    lang: 'vi',
            //    rules: {
            //        txtNameM: { required: true },
            //        ddlCategoryIdM: { required: true },
            //        txtPriceM: {
            //            required: true,
            //            number: true
            //        },
            //        txtQuantity: {
            //            required: true,
            //            number: true
            //        },
            //        txtOriginalPriceM: {
            //            required: true,
            //            number: true
            //        }
            //    }
            //});
            //todo: binding events to controls
            $('#ddlShowPage').on('change', function () {
                commons.configs.pageSize = $(this).val();
                commons.configs.pageIndex = 1;
                loadData(true);
            });

            $('#btnSearch').on('click', function () {
                loadData(true);
            });

            $('#txtKeyword').on('keypress', function (e) {
                if (e.which === 13) {
                    e.preventDefault();
                    loadData(true);
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
                $('#frmMaintainance').parsley().reset();
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
                    },
                    error: function () {
                        commons.notify('Nhập sản phẩm thất bại', 'error');
                        commons.stopLoading();
                    },
                    timeout: 7000
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
                type: "POST",
                url: "/Admin/Product/Index?handler=Delete",
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
        if ($('#frmMaintainance').parsley().validate()) {
            e.preventDefault();
            var id = $('#hidIdM').val();
            var name = $('#txtNameM').val();
            var categoryId = $('#ddlCategoryIdM').combotree('getValue');
            var description = CKEDITOR.instances.txtDescM.getData();
            var price = $('#txtPriceM').val();
            var originalPrice = $('#txtOriginalPriceM').val();
            var image = $('#txtImage').val();
            var brandName = $('#txtBrandName').val();
            var quantity = $('#txtQuantity').val();
            $.ajax({
                type: "POST",
                url: "/Admin/Product/Index?handler=SaveEntity",
                data: JSON.stringify({
                    Id: id,
                    Name: name,
                    CategoryId: categoryId,
                    Image: image,
                    BrandName: brandName,
                    Quantity: quantity,
                    Price: price,
                    OriginalPrice: originalPrice,
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
                $('#txtQuantity').val(data.quantity);
                initTreeDropDownCategory(data.categoryId);
                $('#txtPriceM').val(`${commons.formatNumber(data.price, 0)}`);
                $('#txtOriginalPriceM').val(`${commons.formatNumber(data.originalPrice, 0)}`);
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
        $('#txtBrandName').val('');
        $('#txtQuantity').val('');
        initTreeDropDownCategory('');
        $('#txtPriceM').val('0');
        $('#txtOriginalPriceM').val('0');
        CKEDITOR.instances.txtDescM.setData('');
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
                            Quantity: item.quantity,
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

    var onTypeCurrency = function () {
        $("#txtOriginalPriceM").on({
            keyup: function () {
                formatCurrency($(this));
            }
            //},
            //blur: function () {
            //    formatCurrency($(this), "blur");
            //}
        });

        $("#txtPriceM").on({
            keyup: function () {
                formatCurrency($(this));
            }
            //},
            //blur: function () {
            //    formatCurrency($(this), "blur");
            //}
        });
    };

    function formatNumber(n) {
        // format number 1000000 to 1,234,567
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    }

    function formatCurrency(input, blur) {
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position 
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(".") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(".");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = formatNumber(left_side);

            // validate right side
            right_side = formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            if (blur === "blur") {
                right_side += "00";
            }

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            //input_val = "$" + left_side + "." + right_side;
            input_val = left_side + "." + right_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = formatNumber(input_val);
            //input_val = "$" + input_val;
            input_val = input_val;

            // final formatting
            if (blur === "blur") {
                input_val += ".00";
            }
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }

    return {
        init
    };
})();
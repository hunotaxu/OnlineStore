var category = (function () {
    var init = function () {
        loadData();
        resetFormMaintainance();
        initTreeDropDownCategory();
        registerEvents();
    };

    var registerEvents = function () {
        $('#frmMaintainance').validate({
            errorClass: 'red',
            ignore: [],
            lang: 'en',
            rules: {
                txtNameM: { required: true },
                txtOrderM: { number: true },
                txtHomeOrderM: { number: true }
            }
        });
        $('#btnCreate').off('click').on('click', function () {
            initTreeDropDownCategory();
            $('#modal-add-edit').modal('show');
        });

        $('body').on('click', '#btnEdit', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            $.ajax({
                type: 'GET',
                url: "/Admin/Category/Index?handler=ById",
                data: {
                    id: that
                },
                beforeSend: function () {
                    commons.startLoading();
                },
                dataType: "json",
                success: function (response) {
                    var data = response;
                    $('#hidIdM').val(data.Id);
                    $('#txtNameM').val(data.Name);
                    initTreeDropDownCategory(data.CategoryId);

                    $('#txtDescM').val(data.Description);

                    $('#txtImageM').val(data.ThumbnailImage);

                    $('#txtSeoKeywordM').val(data.SeoKeywords);
                    $('#txtSeoDescriptionM').val(data.SeoDescription);
                    $('#txtSeoPageTitleM').val(data.SeoPageTitle);
                    $('#txtSeoAliasM').val(data.SeoAlias);

                    $('#ckStatusM').prop('checked', data.Status === 1);
                    $('#ckShowHomeM').prop('checked', data.HomeFlag);
                    $('#txtOrderM').val(data.SortOrder);
                    $('#txtHomeOrderM').val(data.HomeOrder);

                    $('#modal-add-edit').modal('show');
                    commons.stopLoading();
                },
                error: function (status) {
                    commons.notify('Có lỗi xảy ra', 'error');
                    commons.stopLoading();
                }
            });
        });

        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            var that = $('#hidIdM').val();
            commons.confirm('Are you sure to delete?', function () {
                $.ajax({
                    type: "POST",
                    url: "/Admin/ProductCategory/Delete",
                    data: { id: that },
                    dataType: "json",
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    success: function (response) {
                        commons.notify('Deleted success', 'success');
                        commons.stopLoading();
                        loadData();
                    },
                    error: function (status) {
                        commons.notify('Has an error in deleting progress', 'error');
                        commons.stopLoading();
                    }
                });
            });
        });

        $('#btnSave').on('click', function (e) {
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                //var description = $('#txtDescM').val();

                //var image = $('#txtImageM').val();
                //var order = parseInt($('#txtOrderM').val());
                //var homeOrder = $('#txtHomeOrderM').val();

                //var seoKeyword = $('#txtSeoKeywordM').val();
                //var seoMetaDescription = $('#txtSeoDescriptionM').val();
                //var seoPageTitle = $('#txtSeoPageTitleM').val();
                //var seoAlias = $('#txtSeoAliasM').val();
                //var status = $('#ckStatusM').prop('checked') === true ? 1 : 0;
                //var showHome = $('#ckShowHomeM').prop('checked');

                $.ajax({
                    type: "POST",
                    url: "/Admin/Category/Index?handler=SaveEntity",
                    data: JSON.stringify({
                        Id: id,
                        Name: name,
                        //Description: description,
                        ParentId: parentId,
                        //HomeOrder: homeOrder,
                        //SortOrder: order,
                        //HomeFlag: showHome,
                        //Image: image,
                        //Status: status,
                        //SeoPageTitle: seoPageTitle,
                        //SeoAlias: seoAlias,
                        //SeoKeywords: seoKeyword,
                        //SeoDescription: seoMetaDescription
                    }),
                    contentType: 'application/json;charset=utf-8',
                    dataType: "json",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                        commons.startLoading();
                    },
                    success: function (response) {
                        commons.notify('Thành công', 'success');
                        $('#modal-add-edit').modal('hide');

                        resetFormMaintainance();

                        commons.stopLoading();
                        loadData(true);
                    },
                    error: function () {
                        commons.notify('Đã có lỗi xãy ra', 'error');
                        commons.stopLoading();
                    }
                });
            }
            return false;
        });
    };

    var resetFormMaintainance = function () {
        $('#hidIdM').val(0);
        $('#txtNameM').val('');
        initTreeDropDownCategory('');

        $('#txtDescM').val('');
        $('#txtOrderM').val('');
        $('#txtHomeOrderM').val('');
        $('#txtImageM').val('');

        $('#txtMetakeywordM').val('');
        $('#txtMetaDescriptionM').val('');
        $('#txtSeoPageTitleM').val('');
        $('#txtSeoAliasM').val('');

        $('#ckStatusM').prop('checked', true);
        $('#ckShowHomeM').prop('checked', false);
    };

    var initTreeDropDownCategory = function (selectedId) {
        $.ajax({
            url: "/Admin/Category/Index?handler=All",
            type: 'GET',
            dataType: 'json',
            async: false,
            success: function (response) {
                var data = [];
                $.each(response,
                    function (i, item) {
                        data.push({
                            id: item.id,
                            text: item.name,
                            parentId: item.parentId,
                            sortOrder: item.sortOrder
                        });
                    });
                var arr = commons.unflattern(data);
                $('#ddlCategoryIdM').combotree({
                    data: arr
                });

                // trường hợp edit
                if (selectedId !== undefined) {
                    $('#ddlCategoryIdM').combotree('setValue', selectedId);
                }
            }
        });
    };

    var loadData = function () {
        $.ajax({
            url: '/Admin/Category/Index?handler=All',
            dataType: 'json',
            success: function (response) {
                var data = [];
                $.each(response, function (index, item) {
                    data.push({
                        id: item.id,
                        text: item.name,
                        parentId: item.parentId,
                        sortOrder: item.sortOrder
                    });
                });
                var treeArr = commons.unflattern(data);
                treeArr.sort(function (a, b) {
                    return a.sortOrder - b.sortOrder;
                });

                $('#treeProductCategory').tree({
                    data: treeArr,
                    dnd: true,
                    onContextMenu: function (e, node) {
                        e.preventDefault();
                        $('#hidIdM').val(node.id);
                        $('#contextMenu').menu('show', {
                            left: e.pageX,
                            top: e.pageY
                        });
                    },
                    onDrop: function (target, source, point) {
                        var targetNode = $(this).tree('getNode', target);
                        if (point === 'append') {
                            var children = [];
                            $.each(targetNode.children, function (i, item) {
                                children.push(item.id);
                            });
                            // Update to database
                            $.ajax({
                                type: 'POST',
                                traditional: true,
                                contentType: 'application/json; charset=utf-8',
                                url: "/Admin/Category/Index?handler=UpdateParentId",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                data: JSON.stringify({
                                    "sourceId": source.id,
                                    "targetId": targetNode.id,
                                    "items": children
                                }),
                                dataType: "json",
                                success: function () {
                                    loadData();
                                },
                                failure: function (res) {
                                    commons.notify(res, 'error');
                                }
                            });
                        }
                        else if (point === 'top' || point === 'bottom') {
                            $.ajax({
                                url: "/Admin/Category/Index?handler=ReOrder",
                                beforeSend: function (xhr) {
                                    xhr.setRequestHeader("XSRF-TOKEN",
                                        $('input:hidden[name="__RequestVerificationToken"]').val());
                                },
                                type: 'POST',
                                traditional: true,
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify({
                                    "sourceId": source.id,
                                    "targetId": targetNode.id
                                }),
                                success: function () {
                                    loadData();
                                },
                                failure: function (res) {
                                    commons.notify(res, 'error');
                                }
                            });
                        }
                    }
                });
            },
            error: function (status) {
                console.log(`status: ${status}`);
            }
        });
    };

    return {
        init,
        loadData,
        registerEvents
    };
})();
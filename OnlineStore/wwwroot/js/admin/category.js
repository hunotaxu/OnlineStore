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
            resetFormMaintainance();
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
                    $('#hidIdM').val(data.id);
                    $('#txtNameM').val(data.name);
                    initTreeDropDownCategory(data.parentId);
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
            commons.confirm('Bạn có muốn xóa không?', function () {
                $.ajax({
                    type: 'GET',
                    url: '/Admin/Category/Index?handler=Delete',
                    data: {
                        id: that
                    },
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    success: function (response) {
                        commons.notify('Xóa thành công', 'success');
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
            if ($('#frmMaintainance').valid()) {
                e.preventDefault();
                var id = parseInt($('#hidIdM').val());
                var name = $('#txtNameM').val();
                var parentId = $('#ddlCategoryIdM').combotree('getValue');
                $.ajax({
                    type: "POST",
                    url: "/Admin/Category/Index?handler=SaveEntity",
                    data: JSON.stringify({
                        Id: id,
                        Name: name,
                        ParentId: parentId
                    }),
                    contentType: 'application/json;charset=utf-8',
                    dataType: "json",
                    beforeSend: function (xhr) {
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
                    data: arr,
                    'panelWidth': 'auto',
                    'panelHeight': 'auto'
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
                                //traditional: true,
                                contentType: 'application/json; charset=utf-8',
                                url: "/Admin/Category/Index?handler=UpdateParentId",
                                data: JSON.stringify({
                                    "sourceId": source.id,
                                    "targetId": targetNode.id,
                                    "items": children
                                }),
                                dataType: "json",
                                success: function () {
                                    commons.notify('Cập nhật thành công', 'success');
                                    loadData();
                                },
                                failure: function (res) {
                                    commons.notify('Đã có lỗi xãy ra', 'error');
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
                                //traditional: true,
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                data: JSON.stringify({
                                    "sourceId": source.id,
                                    "targetId": targetNode.id
                                }),
                                success: function () {
                                    commons.notify('Cập nhật thành công', 'success');
                                    loadData();
                                },
                                failure: function (res) {
                                    commons.notify('Đã có lỗi xãy ra', 'error');
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
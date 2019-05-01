var category = (function () {
    var init = function () {

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
        loadData
    };
})();
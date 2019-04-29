var category = (function () {
    var init = function () {

    };

    var loadData = function () {
        $.ajax({
            url: '/Admin/Category/Index?handler=All',
            dataType: 'json',
            success: function (response) {
                console.log('response:' + response);
                var data = [];
                $.each(response, function (index, item) {
                    data.push({
                        id: item.id,
                        text: item.name,
                        parentId: item.parentId,
                        sortOrder: item.sortOrder
                    });
                    console.log(`${index}: ${data}`);
                });
                $('#treeProductCategory').tree({
                    data: commons.unflattern(data),
                    dnd: true
                });
            },
            error: function(status) {
                console.log(`status: ${status}`);
            }
        });
    };

    return {
        init,
        loadData
    };
})();
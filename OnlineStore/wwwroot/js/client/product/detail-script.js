$('#btnReview').on('click', function (e) {
    //if ($('#frmEvaluation').valid()) {
        e.preventDefault();
        var star = $('#rateit_star').rateit('value');
        var content = $('#txtContent').val();
        var itemid = $('#rateit_star').data('itemid');
        var customerid = $('#rateit_star').data('customerid');
        //alert('Bạn đã đánh giá ' + value + ' sao cho sản phẩm có id là:' + itemid);

        $.ajax({
            type: "POST",
            url: "/Product/Detail?handler=SaveEntity",
            data: JSON.stringify({                
                Evaluation: star,
                Content: content,
                ItemId: itemid,
                CustomerId: customerid
            }),
            contentType: 'application/json;charset=utf-8',
            dataType: "json",
            beforeSend: function (xhr) {
                commons.startLoading();
            },
            success: function (response) {
                commons.notify('Thành công', 'success');
                //resetFormMaintainance();
                commons.stopLoading();
                $('#txtContent').val('');
                location.reload();

                //loadData(true);
            },
            error: function () {
                commons.notify('Đã có lỗi xãy ra', 'error');
                commons.stopLoading();
            }
        });
    //}
    return false;
});

$('#gotocomment').on('click', function (e) {
    //document.getElementById("review").classList.add("active");
    //document.getElementById("description").classList.remove("active");
    ////$("button").attr("aria-expanded", "true");
    //$("#review").attr("aria-expanded", "true");
    //$("#description").attr("aria-expanded", "false");
    //$("#product_tag").attr("aria-expanded", "false");
    //$("#customer_tag").attr("aria-expanded", "false");
    $("#review a").trigger('click');
});
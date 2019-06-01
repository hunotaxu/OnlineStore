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
    $("#review a").trigger('click');
});


$('#btnAddToCart').on('click', function (e) {
    debugger
    e.preventDefault();
    var id = parseInt($(this).data('id'));
    $.ajax({
        url: "/Product/Detail?handler=AddToCart",
        type: "POST",
        dataType: "json",
        contentType: 'application/json;charset=utf-8',
        beforeSend: function () {
            commons.startLoading();
        },
        data: JSON.stringify({
            ItemId: id,
            Quantity: parseInt($('#txtQuantity').val())
        }),
        success: function () {
            commons.notify('Thành công', 'success');
            commons.stopLoading();
        },
        error: function () {
            commons.notify('Đã có lỗi xãy ra', 'error');
            commons.stopLoading();
        }
    });
});
//function loadHeaderCart() {
//    $("#headerCart").load("/AjaxContent/HeaderCart");
//}
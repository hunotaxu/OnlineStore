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
    return false;
});

$('#gotocomment').on('click', function (e) {
    $("#review a").trigger('click');
});
$('#gotocomment1').on('click', function (e) {
    $("#review a").trigger('click');
});
$('#btnAddToCart').on('click', function (e) {
    e.preventDefault();
    loadItemMyCart.init();
    var id = parseInt($(this).data('id'));
    if ($('#rateit_star').data('customerid') === '' || $('#rateit_star').data('customerid') === undefined) {
        commons.confirm('Bạn chưa đăng nhập, bạn có muốn chuyển tiếp sang trang đăng nhập?', function () {
            window.location.replace(`/Identity/Account/Login?returnUrl=/Product/Detail?id=${id}`);
        });
    }  
    else {
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
                commons.notify('Thêm vào giỏ hàng thành công', 'success');
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Đã có lỗi xãy ra', 'error');
                commons.stopLoading();
            }
        });
    };
});

var x = document.getElementById('itemQty').value;
if (x < 1) {
    $('#txtQuantity').attr('disabled', true);
    $('#btnAddToCart').attr('disabled', true);
    $("#btnAddToCart").css({
        "background-color": "#e5e5e5",
        "border": "1px #e5e5e5 solid",
        "color": "#000000"
    });
    $(".qtybutton").css({
        "pointer-events": "none"
    });
}
$("#txtQuantity").on("keypress keyup", function (event) {
    $(this).val($(this).val().replace(/[^\d].+/, ""));
    if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
    $("#txtQuantity").val();       

    var y = document.getElementById('txtQuantity').value;
    if (y > x) {
        commons.notify('Số sản phẩm bạn nhập quá số lượng tồn', 'error');
        

        $(this).val('');
        $(this).val(x);
    }
});

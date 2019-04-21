
jQuery.ajaxSettings.traditional = true;
var listBrandName = [];

function sortProduct() {
    $('#CurrentPage').val(1);
    $.ajax({
        type: "GET",
        url: "/Product/Index?handler=Search",
        data: {
            "currentSearchString": "@Model.CurrentSearchString",
            "currentSort": $('#CurrentSort option:selected').val(),
            "currentBrand": listBrandName
        }
    }).done(function (result) {
        $(".bottom-product").html(result);
    }).fail(function (result) {
        console.log(result);
    });
}

$("input[type=checkbox]").change(function () {
    var brand = $(this).closest("li").text();
    $('#CurrentPage').val(1);
    if (this.checked) {
        listBrandName.push(brand);
    } else {
        listBrandName.splice(listBrandName.indexOf(brand, 1));
    }
    $.ajax({
        type: "GET",
        url: "/Product/Index?handler=Search",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        cache: false,
        data: {
            "currentSearchString": "@Model.CurrentSearchString",
            "currentSort": $('#CurrentSort option:selected').val(),
            "currentBrand": listBrandName
        }
    }).done(function (result) {
        $(".bottom-product").empty();
        $(".bottom-product").html(result);
    }).fail(function (result) {
        console.log(result);
    });
});

function loadPage(currentPage) {
    $.ajax({
        type: "GET",
        url: "/Product/Index?handler=Search",
        data: {
            "currentSearchString": "@Model.CurrentSearchString",
            "currentSort": $('#CurrentSort option:selected').val(),
            "currentBrand": listBrandName,
            "currentPage": currentPage
        }
    }).done(function (result) {
        $(".bottom-product").html(result);
        $('#CurrentPage').val(currentPage);
    }).fail(function (result) {
        console.log(result);
    });

}

$(document).on('click',
    '#btnPrevious',
    function () {
        loadPage(Number($('#CurrentPage').val()) - 1);
    });

$(document).on('click',
    '#btnNext',
    function () {
        loadPage(Number($('#CurrentPage').val()) + 1);
    });
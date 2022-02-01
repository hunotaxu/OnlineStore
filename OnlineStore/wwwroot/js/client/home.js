var home = (function () {
    var init = function () {
        jQuery('.mega-menu-index').slideDown();
        $(document).ready(function () {
            $('#Carousel').carousel({
                interval: 5000
            });
            registerEvents();
            showSlide();
        });
    };

    var registerEvents = function () {
        $('.btnAddToCart').on('click', function (e) {
            e.preventDefault();
            var id = parseInt($(this).data('id'));
            if ($('#data-cus-hidden-index').data('customerid') === '' || $('#data-cus-hidden-index').data('customerid') === undefined) {
                commons.confirm('Bạn chưa đăng nhập, bạn có muốn chuyển tiếp sang trang đăng nhập?', function () {
                    window.location.replace(`/Identity/Account/Login?returnUrl=/Product/Detail?id=${id}`);
                });
            }

            else {
                $.ajax({
                    url: "/Product/AddToCart?handler=AddToCart",
                    type: "POST",
                    dataType: "json",
                    contentType: 'application/json;charset=utf-8',
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    data: JSON.stringify({
                        ItemId: id,
                        Quantity: 1
                    }),
                    success: function () {
                        commons.notify('Thêm vào giỏ hàng thành công', 'success');
                        header.init();
                        commons.stopLoading();
                    },
                    error: function (response) {
                        if (response.responseText !== undefined && response.responseText !== '') {
                            commons.notify(response.responseText, 'error');
                        }
                        else {
                            commons.notify('Đã có lỗi xãy ra', 'error');
                        }
                        commons.stopLoading();
                    }
                });
            }
        });
    };

    function showSlide() {
        jQuery('#rev_slider_4').show().revolution({
            dottedOverlay: 'none',
            delay: 5000,
            startwidth: 865,
            startheight: 450,

            hideThumbs: 200,
            thumbWidth: 200,
            thumbHeight: 50,
            thumbAmount: 2,

            navigationType: 'thumb',
            navigationArrows: 'solo',
            navigationStyle: 'round',

            touchenabled: 'on',
            onHoverStop: 'on',

            swipe_velocity: 0.7,
            swipe_min_touches: 1,
            swipe_max_touches: 1,
            drag_block_vertical: false,

            spinner: 'spinner0',
            keyboardNavigation: 'off',

            navigationHAlign: 'center',
            navigationVAlign: 'bottom',
            navigationHOffset: 0,
            navigationVOffset: 20,

            soloArrowLeftHalign: 'left',
            soloArrowLeftValign: 'center',
            soloArrowLeftHOffset: 20,
            soloArrowLeftVOffset: 0,

            soloArrowRightHalign: 'right',
            soloArrowRightValign: 'center',
            soloArrowRightHOffset: 20,
            soloArrowRightVOffset: 0,

            shadow: 0,
            fullWidth: 'on',
            fullScreen: 'off',

            stopLoop: 'off',
            stopAfterLoops: -1,
            stopAtSlide: -1,

            shuffle: 'off',

            autoHeight: 'off',
            forceFullWidth: 'on',
            fullScreenAlignForce: 'off',
            minFullScreenHeight: 0,
            hideNavDelayOnMobile: 1500,

            hideThumbsOnMobile: 'off',
            hideBulletsOnMobile: 'off',
            hideArrowsOnMobile: 'off',
            hideThumbsUnderResolution: 0,

            hideSliderAtLimit: 0,
            hideCaptionAtLimit: 0,
            hideAllCaptionAtLilmit: 0,
            startWithSlide: 0,
            fullScreenOffsetContainer: ''
        });
    }

    return {
        init
    };
})();
jQuery(document).ready(function ($) {
    var $filter = $('.head-top');
    var $filterSpacer = $('<div />', {
        "class": "vnkings-spacer",
        "height": $filter.outerHeight()
    });
    if ($filter.size()) {
        $(window).scroll(function () {
            if (!$filter.hasClass('fix') && $(window).scrollTop() > $filter.offset().top) {
                $filter.before($filterSpacer);
                $filter.addClass("fix");
            }
            else if ($filter.hasClass('fix') && $(window).scrollTop() < $filterSpacer.offset().top) {
                $filter.removeClass("fix");
                $filterSpacer.remove();
            }
        });
    }
});
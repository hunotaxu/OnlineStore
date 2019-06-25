var search = (function () {
    var init = function () {
        $(document).ready(function () {
            formatPrice();
        });
    };

    var formatPrice = function () {
        //$("#min-price").on({
        //    keyup: function () {
        //        commons.formatCurrency($(this));
        //    }
        //    //},
        //    //blur: function () {
        //    //    commons.formatCurrency($(this), "blur");
        //    //}
        //});
    };

    return {
        init
    };
})();
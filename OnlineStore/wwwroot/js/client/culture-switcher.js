var cultureSwitcher = (function () {
    function init() {
        $(document).ready(function () {
            $('input[type=radio][name=culture]').change(function () {
                $('#culture-switcher').submit();
            });
            $(".dropdown").hover(function () {
                let dropdownMenu = $(this).children(".dropdown-menu");
                if (dropdownMenu.is(":visible")) {
                    dropdownMenu.parent().toggleClass("open");
                }
            });
        });
    }
    return {
        init
    }
})();
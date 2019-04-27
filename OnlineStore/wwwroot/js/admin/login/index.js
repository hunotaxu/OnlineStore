var login = (function () {
    var init = function () {
        $(document).ready(function() {
            onValidateForm();
        });
    };

    var onValidateForm = function () {
        $(document).ready(function() {
            $('#frmLogin').parsley();
        });
    };

    return {
        init
    };
})();
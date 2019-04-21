var login = (function () {
    // #region Publics
    var init = function () {
        $(document).ready(function() {
            onValidateForm();
        });
    };
    // #endregion

    var onValidateForm = function () {
        $(document).ready(function() {
            $('#frmLogin').parsley();
        });
    };

    return {
        init
    };
})();
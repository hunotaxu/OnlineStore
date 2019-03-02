var login = (function () {

    // #region Variants
    var login;
    var onRegister;
    // #endregion

    // #region Publics
    var init = function() {
        onRegister();
    };
    // #endregion

    // #region Events
    onRegister = function () {
        $('#btnLogin').on('click',
            function (event) {
                event.preventDefault();
                var username = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(username, password);
            });
    };

    login = function (user, pass) {
        $.ajax({
            type: "POST",
            url: "~/Areas/Admin/Pages/Login/",
            data: {
                username: user,
                password: pass
            },
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    window.location.href = "~/Areas/Admin/Pages/Home/Index";
                } else {
                    commons.notify('Đăng nhập không đúng', 'error');
                }
            }
        });
    };
    // #endregion

    return {
        init
    };
})();
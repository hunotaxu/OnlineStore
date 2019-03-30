var login = (function () {
    // #region Publics
    var init = function () {
        $(document).ready(function() {
            onRegister();
        });
    };
    // #endregion

    // #region Events
    var onRegister = function () {
        //$('#frmLogin').validate({
        //    errorClass: 'red',
        //    ignore: [],
        //    lang: 'en',
        //    rules: {
        //        userName: {
        //            required: true
        //        },
        //        password: {
        //            required: true
        //        }
        //    }
        //});
        $('#btnLogin').on('click',
            function (event) {
                event.preventDefault();
                var username = $('#txtUserName').val();
                var password = $('#txtPassword').val();
                login(username, password);
            });
    };

    var login = function (user, pass) {
        $.ajax({
            type: "POST",
            url: "/Admin/Login/Index",
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
            },
            error: function (res) {
                console.log(res);
            },
            complete: function (res) {
                console.log(res);
            }
        });
    };
    // #endregion

    return {
        init
    };
})();
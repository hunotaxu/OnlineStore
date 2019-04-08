var login = (function () {
    // #region Publics
    var init = function () {
        $(document).ready(function() {
            onValidateForm();
            onRegister();
        });
    };
    // #endregion

    // #region Events
    var onRegister = function () {
        //$('#frmAdminLogin').validate({
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
                var email = $('#txtEmail').val();
                var password = $('#txtPassword').val();
                //loginFunc(email, password);
                $.ajax({
                    type: "POST",
                    url: "/Login/Index",
                    beforeSend: function (xhr) {
                        xhr.setRequestHeader("XSRF-TOKEN",
                            $('input:hidden[name="__RequestVerificationToken"]').val());
                    },
                    data: {
                        Email: email,
                        Password: $('#txtPassword').val()
                    },
                    //contentType: "text",
                    //dataType: "json",
                    success: function (response) {
                        if (response.success) {
                            window.location.href = "~/Areas/Admin/Pages/Home/Index";
                        } else {
                            commons.notify('Đăng nhập không đúng', 'error');
                        }
                    },
                    failure: function (response) {
                        console.log(response);
                    },
                    complete: function (res) {
                        console.log(res);
                    }
                });
            });
    };

    const onValidateForm = function() {
        $('#frmAdminLogin').parsley();
    };

    var loginFunc = function (email, pass) {
        $.ajax({
            type: "POST",
            url: "/Login/Index",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                "Email": email,
                "Password": pass
            },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.success) {
                    window.location.href = "~/Areas/Admin/Pages/Home/Index";
                } else {
                    commons.notify('Đăng nhập không đúng', 'error');
                }
            },
            failure: function (response) {
                console.log(response);
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
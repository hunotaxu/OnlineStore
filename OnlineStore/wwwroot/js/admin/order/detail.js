﻿var orderDetail = (function () {
    var init = function () {
        $(document).ready(function () {
            onTypeCurrency();
            onDateTimePicker();
            registerEvents();
        });
    };
    var onDateTimePicker = function () {
        var orderDate = moment($('#OrderDate').val(), "DD/MM/YYYY hh:mm A").toDate();
        var defaultOrderDate = new Date(orderDate.getFullYear(), orderDate.getMonth(), orderDate.getDate(), orderDate.getHours(), orderDate.getMinutes());
        $('#divOrderDate').datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: defaultOrderDate
        });
        var deliveryDate = moment($('#DeliveryDate').val(), "DD/MM/YYYY hh:mm A").toDate();
        var defaultDeliveryDate = new Date(deliveryDate.getFullYear(), deliveryDate.getMonth() + 1, deliveryDate.getDate(), deliveryDate.getHours(), deliveryDate.getMinutes());
        $('#divDeliveryDate').datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: defaultDeliveryDate
        });
    };

    var registerEvents = function () {
        $('#btnSaveOrderStatus').click(function (e) {
            saveOrderGeneralInfo(e);
        });
        $('#btnSaveOrderDeliveryInfo').click(function (e) {
            saveOrderDeliveryInfo(e);
        });
        $('body').on('click', '#btnDelete', function (e) {
            e.preventDefault();
            commons.confirm('Bạn chắc chắn muốn xóa?', function () {
                $.ajax({
                    type: 'POST',
                    url: '/Admin/Order/Index?handler=Delete',
                    data: JSON.stringify({
                        Id: $('#btnDelete').data('order-id')
                    }),
                    contentType: 'application/json;charset=utf-8',
                    beforeSend: function () {
                        commons.startLoading();
                    },
                    success: function () {
                        commons.notify('Xóa thành công', 'success');
                        commons.stopLoading();
                        window.location.href = '/Admin/Order';
                    },
                    error: function () {
                        commons.notify('Đã có lỗi xãy ra');
                        commons.stopLoading();
                    }
                });
            });
        });
    };

    var saveOrderGeneralInfo = function (e) {
        if ($('#frmOrderGeneralInfo').valid()) {
            e.preventDefault();
            var id = $('#orderId').text();
            var status = $("input[name='Status']:checked").val();
            var parts = $('#divDeliveryDate').data('date').split(" ")[0].split("/");
            var deliveryDate = new Date(parseInt(parts[2], 10), parseInt(parts[1], 10) - 1, parseInt(parts[0], 10) + 1);
            $.ajax({
                type: "POST",
                url: "/Admin/Order/Details?handler=SaveEntity",
                data: JSON.stringify({
                    Id: id,
                    Status: status,
                    DeliveryDate: deliveryDate
                }),
                contentType: 'application/json;charset=utf-8',
                dataType: "json",
                beforeSend: function () {
                    commons.startLoading();
                },
                success: function (response) {
                    commons.notify('Cập nhật thành công', 'success');
                    $('#modal-add-edit').modal('hide');
                    commons.stopLoading();
                },
                error: function (response) {
                    commons.notify('Đã có lỗi xãy ra', 'error');
                    commons.stopLoading();
                }
            });
            return false;
        }
    };

    var onTypeCurrency = function () {
        $("#txtShippingFee").on({
            keyup: function () {
                formatCurrency($(this));
            }
        });
    };

    function formatNumber(n) {
        // format number 1000000 to 1,234,567
        return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }

    function formatCurrency(input, blur) {
        // appends $ to value, validates decimal side
        // and puts cursor back in right position.

        // get input value
        var input_val = input.val();

        // don't validate empty input
        if (input_val === "") { return; }

        // original length
        var original_len = input_val.length;

        // initial caret position 
        var caret_pos = input.prop("selectionStart");

        // check for decimal
        if (input_val.indexOf(".") >= 0) {

            // get position of first decimal
            // this prevents multiple decimals from
            // being entered
            var decimal_pos = input_val.indexOf(".");

            // split number by decimal point
            var left_side = input_val.substring(0, decimal_pos);
            var right_side = input_val.substring(decimal_pos);

            // add commas to left side of number
            left_side = formatNumber(left_side);

            // validate right side
            right_side = formatNumber(right_side);

            // On blur make sure 2 numbers after decimal
            if (blur === "blur") {
                right_side += "00";
            }

            // Limit decimal to only 2 digits
            right_side = right_side.substring(0, 2);

            // join number by .
            //input_val = "$" + left_side + "." + right_side;
            input_val = left_side + "." + right_side;

        } else {
            // no decimal entered
            // add commas to number
            // remove all non-digits
            input_val = formatNumber(input_val);
            //input_val = "$" + input_val;
            input_val = input_val;

            // final formatting
            if (blur === "blur") {
                input_val += ".00";
            }
        }

        // send updated string to input
        input.val(input_val);

        // put caret back in the right position
        var updated_len = input_val.length;
        caret_pos = updated_len - original_len + caret_pos;
        input[0].setSelectionRange(caret_pos, caret_pos);
    }

    return {
        init
    };
})();
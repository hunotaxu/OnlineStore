var orderDetail = (function () {
    var init = function () {
        onDateTimePicker();
        $("#divOrderDate").on("dp.change", function (e) {
            //$('#datetimepicker7').data("DateTimePicker").minDate(e.date);
            //console.log($("#divOrderDate").data('DateTimePicker').getDate());
            console.log($("#divOrderDate").find("input").val());
        });
    };
    var onDateTimePicker = function () {
        var orderDate = new Date($('#OrderDate').val());
        var defaultOrderDate = new Date(orderDate.getFullYear(), orderDate.getMonth() + 1, orderDate.getDate(), orderDate.getHours(), orderDate.getMinutes());
        $('#divOrderDate').datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: defaultOrderDate
        });
        var deliveryDate = new Date($('#DeliveryDate').val());
        var defaultDeliveryDate = new Date(deliveryDate.getFullYear(), deliveryDate.getMonth() + 1, deliveryDate.getDate(), deliveryDate.getHours(), deliveryDate.getMinutes());
        $('#divDeliveryDate').datetimepicker({
            format: 'DD/MM/YYYY hh:mm A',
            defaultDate: defaultDeliveryDate
        });
    };
    return {
        init
    };
})();
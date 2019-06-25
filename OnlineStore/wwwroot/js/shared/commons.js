var commons = {
    configs: {
        pageSize: 10,
        pageIndex: 1
    },

    initDateRangePicker: function initDateRangePicker() {
        if (typeof ($.fn.daterangepicker) === 'undefined') { return; }
        //console.log('init_daterangepicker');

        var cb = function (start, end, label) {
            //console.log(start.toISOString(), end.toISOString(), label);
            //$('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
            $('#reportrange span').html(start.format('DD/MM/YYYY') + ' - ' + end.format('DD/MM/YYYY'));
            //if (start._isValid && end._isValid) {
            //    $('#reportrange span').val(start.format('Do MMM YYYY') + ' - ' + end.format('Do MMM YYYY'));
            //}
            //else {
            //    $('#daterange input').val('');
            //}
        };

        var optionSet1 = {
            startDate: moment().subtract(30, 'days'), // ngày hiện tại trừ cho 30 ngày
            endDate: moment(), // ngày hiện tại
            minDate: '01/01/2000',
            //maxDate: moment().format('MM/DD/YYYY'),
            maxDate: moment().format('DD/MM/YYYY'),
            dateLimit: {
                days: 100000000
            },
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            ranges: {
                'Hôm nay': [moment(), moment()],
                'Ngày hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                '7 ngày qua': [moment().subtract(6, 'days'), moment()],
                '30 ngày qua': [moment().subtract(30, 'days'), moment()],
                'Tháng này': [moment().startOf('month'), moment().endOf('month')],
                'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            opens: 'left',
            buttonClasses: ['btn btn-default'],
            applyClass: 'btn-small btn-primary',
            cancelClass: 'btn-small',
            //format: 'MM/DD/YYYY',
            //format: 'DD/MM/YYYY',
            separator: ' - ',
            locale: {
                format: 'DD/MM/YYYY',
                applyLabel: 'Xác nhận',
                cancelLabel: 'Xóa bỏ',
                fromLabel: 'Từ',
                toLabel: 'tới',
                customRangeLabel: 'Tùy chọn',
                daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                monthNames: ['Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6', 'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'],
                firstDay: 1
            }
        };

        //$('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        $('#reportrange span').html(moment().subtract(30, 'days').format('DD/MM/YYYY') + ' - ' + moment().format('DD/MM/YYYY'));
        $('#reportrange').daterangepicker(optionSet1, cb);
        //$('#reportrange').on('show.daterangepicker', function () {
        //    console.log("show event fired");
        //});
        //$('#reportrange').on('hide.daterangepicker', function () {
        //    console.log("hide event fired");
        //});
        //$('#reportrange').on('apply.daterangepicker', function (ev, picker) {
        //    //console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
        //    //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
        //    loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
        //});
        $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
            console.log("cancel event fired");
            $("#reportrange span").html('');
            //$('#reportrange').data('daterangepicker').remove();
        });
        $('#options1').click(function () {
            $('#reportrange').data('daterangepicker').setOptions(optionSet1, cb);
        });
        $('#options2').click(function () {
            $('#reportrange').data('daterangepicker').setOptions(optionSet2, cb);
        });
        $('#destroy').click(function () {
            $('#reportrange').data('daterangepicker').remove();
        });
    },
    //applyDateRangePicker: function () {
    //    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    //        //console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
    //        //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
    //        loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
    //    });
    //},

    notify: function (message, type) {
        $.notify(message, {
            // whether to hide the notification on click
            clickToHide: true,
            // whether to auto-hide the notification
            autoHide: true,
            // if autoHide, hide after milliseconds
            autoHideDelay: 5000,
            // show the arrow pointing at the element
            arrowShow: true,
            // arrow size in pixels
            arrowSize: 5,
            // position defines the notification position though uses the defaults below
            position: 'bottom center',
            // default positions
            elementPosition: 'top right',
            globalPosition: 'top center',
            // default style
            style: 'bootstrap',
            // default class (string or [string])
            className: type,
            // show animation
            showAnimation: 'slideDown',
            // show animation duration
            showDuration: 400,
            // hide animation
            hideAnimation: 'slideUp',
            // hide animation duration
            hideDuration: 200,
            // padding between element and notification
            gap: 2
        });
    },
    //wrapPaging: function (recordCount, callBack, changePageSize) {
    //    var totalSize = Math.ceil(recordCount / commons.configs.pageSize);
    //    // Unbind pagination if it existed or click change page size
    //    if ($('#paginationUL a').length === 0 || changePageSize === true) {
    //        $('#paginationUL').empty();
    //        $('#paginationUL').removeData("twbs-pagination");
    //        $('#paginationUL').unbind("page");
    //    }
    //    // Bind Pagination Event
    //    $('#paginationUL').twbsPagination({
    //        totalPages: totalSize,
    //        visiblePages: 7,
    //        first: 'Đầu',
    //        prev: 'Trước',
    //        next: 'Tiếp',
    //        last: 'Cuối',
    //        onPageClick: function (event, p) {
    //            commons.configs.pageIndex = p;
    //            setTimeout(callBack(), 200);
    //        }
    //    });
    //},
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Xác nhận',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Hủy bỏ',
                    className: 'btn-danger'
                }
            },
            callback: function (result) {
                if (result === true) {
                    okCallback();
                }
            }
        });
    },
    dateFormatJson: function (datetime) {
        if (datetime === null || datetime === '') {
            return '';
        }
        //var newDate = new Date(parseInt(datetime.substr(6)));
        var newDate = datetime;
        var month = newDate.getMonth() + 1;
        var day = newDate.getDate();
        var year = newDate.getFullYear();
        //var hh = newDate.getHours();
        //var mm = newDate.getMinutes();
        if (month < 10)
            month = `0${month}`;
        //if (hh < 10)
        //    hh = `0${hh}`;
        //if (mm < 10)
        //    mm = `0${mm}`;
        return `${day}/${month}/${year}`;
    },

    dateTimeFormatJson: function (datetime) {
        if (datetime === null || datetime === '')
            return '';
        var newdate = new Date(datetime);
        var month = newdate.getMonth() + 1;
        var day = newdate.getDate();
        var year = newdate.getFullYear();
        var hh = newdate.getHours();
        var mm = newdate.getMinutes();
        var ss = newdate.getSeconds();
        if (month < 10)
            month = `0${month}`;
        if (day < 10)
            day = `0${day}`;
        if (hh < 10)
            hh = `0${hh}`;
        if (mm < 10)
            mm = `0${mm}`;
        if (ss < 10)
            ss = `0${ss}`;
        return `${day}/${month}/${year} ${hh}:${mm}:${ss}`;
    },

    startLoading: function () {
        $(".dv-bg-loading").show();
    },
    stopLoading: function () {
        $('.dv-bg-loading').hide();
    },

    getStatus: function (status) {
        if (status === 1)
            return '<span class="badge bg-green">Kích hoạt</span>';
        else {
            return '<span class="badge bg-red">Khóa</span>';
        }
    },
    formatNumber: function (number, precision) {
        if (!isFinite(number)) {
            return number.toString();
        }

        var a = number.toFixed(precision).split('.');
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&.');
        return a.join('.');
    },

    unflattern: function (arr) {
        var map = {};
        var roots = [];
        for (var i = 0; i < arr.length; i += 1) {
            var node = arr[i];
            if (node.children === undefined) {
                node.children = [];
            };
            map[node.id] = i; // use map to look-up the parents
            if (node.parentId !== null) {
                if (map[node.parentId] === undefined) {
                    for (var j = i + 1; j < arr.length; j++) {
                        var nodeJ = arr[j];
                        nodeJ.children = [];
                        if (nodeJ.id === node.parentId) {
                            arr[j].children.push(node);
                        }
                    }
                } else {
                    arr[map[node.parentId]].children.push(node);
                }
            } else {
                roots.push(node);
            }
        }
        return roots;
    }
};


$(document).ajaxSend(function (e, xhr, options) {
    if (options.type.toUpperCase() === "POST" || options.type.toUpperCase() === "PUT") {
        xhr.setRequestHeader("XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    }
});
var home = (function () {
    var init = function () {
        $(document).ready(function () {
            initDateRangePicker();
            loadData();
        });
    };

    function loadData(from, to) {
        debugger;
        $.ajax({
            type: "GET",
            url: "/Admin/Home/Index?handler=Revenue",
            data: {
                fromDate: from,
                toDate: to
            },
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                debugger;
                initChart(response);
                commons.stopLoading();
            },
            error: function (status) {
                commons.notify('Có lỗi xảy ra', 'error');
                commons.stopLoading();
            }
        });
    }

    //var randNum = function () {
    //    return (Math.floor(Math.random() * (1 + 40 - 20))) + 20;
    //};

    //function formatTime(input) {
    //    for (var i = 0; i < input.length; i++) {
    //        var date = input[i][0];
    //        input[i][0] = new Date(date).getTime();
    //    }
    //    return input;
    //}

    //var jsonResponse = [["2013-11-05", 8.3333333333333], ["2013-12-05", 0]];

    //function onDataReceived(series) {
    //    series = formatTime(series);
    //    data.push(series);
    //    $.plot(placeholder, data, options);
    //    //setData(data);not shown in question
    //}

    function initChart(data) {
        debugger;
        var arrRevenue = [];
        var arrProfit = [];

        $.each(data, function (i, item) {
            arrRevenue.push([new Date(item.date).getTime(), item.revenue]);
        });
        $.each(data, function (i, item) {
            arrProfit.push([new Date(item.date).getTime(), item.profit]);
        });

        //var chart_plot_02_data = [];

        //for (var i = 0; i < 30; i++) {
        //    chart_plot_02_data.push([new Date(Date.today().add(i).days()).getTime(), randNum() + i + i + 10]);
        //}

        var chart_plot_02_settings = {
            grid: {
                show: true,
                aboveData: true,
                color: "#3f3f3f",
                labelMargin: 10,
                axisMargin: 0,
                borderWidth: 0,
                borderColor: null,
                minBorderMargin: 5,
                clickable: true,
                hoverable: true,
                autoHighlight: true,
                mouseActiveRadius: 100
            },
            series: {
                lines: {
                    show: true,
                    fill: true,
                    lineWidth: 2,
                    steps: false
                },
                points: {
                    show: true,
                    radius: 4.5,
                    symbol: "circle",
                    lineWidth: 3.0
                }
            },
            legend: {
                position: "ne",
                margin: [0, -25],
                noColumns: 0,
                labelBoxBorderColor: null,
                labelFormatter: function (label, series) {
                    return label + '&nbsp;&nbsp;';
                },
                width: 40,
                height: 1
            },
            colors: ['#96CA59', '#3F97EB', '#72c380', '#6f7a8a', '#f7cb38', '#5a8022', '#2c7282'],
            shadowSize: 0,
            tooltip: true,
            tooltipOpts: {
                content: "%s: %y.0",
                xDateFormat: "%d/%m",
                shifts: {
                    x: -30,
                    y: -50
                },
                defaultTheme: false
            },
            yaxis: {
                min: 0
            },
            xaxis: {
                mode: "time",
                minTickSize: [1, "day"],
                timeformat: "%d/%m/%y",
                //min: chart_plot_02_data[0][0],
                //max: chart_plot_02_data[20][0]
                min: data[0] !== undefined ? new Date(data[0].fromDate).getTime() : "",
                max: data[0] !== undefined ? new Date(data[0].toDate).getTime() : ""
                //min: new Date(fromDate).getTime(),
                //max: new Date(toDate).getTime()
            }
        };
        if ($("#chart_plot_02").length) {
            console.log('Plot2');

            $.plot($("#chart_plot_02"),
                [{
                    label: "Doanh thu",
                    data: arrRevenue,
                    lines: {
                        fillColor: "rgba(150, 202, 89, 0.12)"
                    },
                    points: {
                        fillColor: "#fff"
                    }
                },
                    //{
                    //    label: "Lợi nhuận",
                    //    data: arrProfit,
                    //    lines: {
                    //        fillColor: "rgba(140, 232, 289, 0.12)"
                    //    },
                    //    points: {
                    //        fillColor: "#fff"
                    //    }
                    //}
                ], chart_plot_02_settings);
        }
    }

    function initDateRangePicker() {
        debugger;
        if (typeof ($.fn.daterangepicker) === 'undefined') { return; }
        console.log('init_daterangepicker');

        var cb = function (start, end, label) {
            console.log(start.toISOString(), end.toISOString(), label);
            $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        };

        var optionSet1 = {
            startDate: moment().subtract(29, 'days'), // ngày hiện tại trừ cho 29 ngày
            endDate: moment(), // ngày hiện tại
            minDate: '01/01/2012',
            //maxDate: moment().format('MM/DD/YYYY'),
            maxDate: moment().format('DD/MM/YYYY'),
            dateLimit: {
                days: 60
            },
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            ranges: {
                'Today': [moment(), moment()],
                'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                'This Month': [moment().startOf('month'), moment().endOf('month')],
                'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
            },
            opens: 'left',
            buttonClasses: ['btn btn-default'],
            applyClass: 'btn-small btn-primary',
            cancelClass: 'btn-small',
            //format: 'MM/DD/YYYY',
            format: 'DD/MM/YYYY',
            separator: ' to ',
            locale: {
                applyLabel: 'Submit',
                cancelLabel: 'Clear',
                fromLabel: 'From',
                toLabel: 'To',
                customRangeLabel: 'Custom',
                daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
                monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                firstDay: 1
            }
        };

        $('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        $('#reportrange').daterangepicker(optionSet1, cb);
        $('#reportrange').on('show.daterangepicker', function () {
            console.log("show event fired");
        });
        $('#reportrange').on('hide.daterangepicker', function () {
            console.log("hide event fired");
        });
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
            //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
            loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
        });
        $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
            console.log("cancel event fired");
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
    }

    return {
        init
    };
})();
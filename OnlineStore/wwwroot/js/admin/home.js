var home = (function () {
    var init = function () {
        $(document).ready(function () {
            commons.initDateRangePicker();
            loadData();
            applyDateRange();
            loadBestCategories();
            loadBestDeliverMethod();
        });
    };

    function loadData(from, to) {
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
                initChart(response);
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu', 'error');
                commons.stopLoading();
            }
        });
    }

    function loadBestCategories() {
        $.ajax({
            type: "GET",
            url: "/Admin/Home/Index?handler=Categories",
            //data: {
            //    fromDate: from,
            //    toDate: to
            //},
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                var totalDeliveredItems = 0;
                $.each(response, function (i, item) {
                    totalDeliveredItems += item.numberOfDeliverdItems;
                });
                init_chart_doughnut(response, totalDeliveredItems);
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu', 'error');
                commons.stopLoading();
            }
        });
    }

    function loadBestDeliverMethod() {
        var template = $('#table-template').html();
        var render = '';
        $.ajax({
            type: "GET",
            url: "/Admin/Home/Index?handler=DeliverMethod",
            //data: {
            //    fromDate: from,
            //    toDate: to
            //},
            dataType: "json",
            beforeSend: function () {
                commons.startLoading();
            },
            success: function (response) {
                $('#listDeliverMethods').empty();
                var totalOfTop3 = 0;
                if (response !== undefined && response !== '') {
                    $.each(response, function (i, item) {
                        render += Mustache.render(template,
                            {
                                Color: item.colors[i],
                                MethodName: item.deliveryName,
                                ProportionOfMethod: `${item.proportionOfDeliverdItems}%`
                                //BrandName: item.brandName,
                                //Quantity: item.quantity,
                                //Price: `${commons.formatNumber(item.price, 0)}đ`,
                                //CreatedDate: commons.dateTimeFormatJson(item.dateCreated)
                            });
                        totalOfTop3 += item.proportionOfDeliverdItems;
                    });
                    if (totalOfTop3 < 100) {
                        render += Mustache.render(template,
                            {
                                Color: response[0].colors[4],
                                MethodName: 'Các phương thức khác',
                                ProportionOfMethod: `${100 - totalOfTop3}%`
                                //BrandName: item.brandName,
                                //Quantity: item.quantity,
                                //Price: `${commons.formatNumber(item.price, 0)}đ`,
                                //CreatedDate: commons.dateTimeFormatJson(item.dateCreated)
                            });
                    }
                }
                if (render !== '') {
                    $('#listDeliverMethods').html(render);
                    initBestMethodChart(response);
                }
                commons.stopLoading();
            },
            error: function () {
                commons.notify('Không tải được dữ liệu', 'error');
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

    function initBestMethodChart(data) {
        if (typeof (Chart) === 'undefined') { return; }

        console.log('init_chart_doughnut');

        if ($('.canvasBestDeliveryMethod').length) {
            var labels = [];
            var proportionData = [];
            var backgroundColors = [];
            $.each(data, function (i, item) {
                labels.push(item.deliveryName);
                proportionData.push(item.proportionOfDeliverdItems);
                backgroundColors.push(item.colors[i]);
            });

            var chart_doughnut_settings = {
                type: 'doughnut',
                //tooltipFillColor: "rgba(51, 51, 51, 0.55)",
                data: {
                    labels: labels,
                    //labels: [
                    //    "Symbian",
                    //    "Blackberry",
                    //    "Other",
                    //    "Android",
                    //    "IOS"
                    //],
                    datasets: [{
                        //data: [15, 20, 30, 10, 30],
                        data: proportionData,
                        backgroundColor: backgroundColors
                        //backgroundColor: [
                        //    "#BDC3C7",
                        //    "#9B59B6",
                        //    "#E74C3C"
                        //    //"#26B99A",
                        //    //"#3498DB"
                        //],
                        //hoverBackgroundColor: [
                        //    "#CFD4D8",
                        //    "#B370CF",
                        //    "#E95E4F",
                        //    "#36CAAB",
                        //    "#49A9EA"
                        //]
                    }]
                },
                toolTip: {
                    enabled: false
                },
                options: {
                    legend: false,
                    responsive: false
                }
            };

            $('.canvasBestDeliveryMethod').each(function () {
                var chart_element = $(this);
                var chart_doughnut = new Chart(chart_element, chart_doughnut_settings);
            });
        }

    }

    function init_chart_doughnut(data, total) {

        if (typeof (Chart) === 'undefined') { return; }

        console.log('init_chart_doughnut');

        if ($('.canvasBestCategories').length) {
            var labels = [];
            var proportionData = [];
            $.each(data, function (idx, item) {
                labels.push(item.categoryName);
                proportionData.push((item.numberOfDeliverdItems * 100) / total);
            });

            var chart_doughnut_settings = {
                type: 'doughnut',
                //tooltipFillColor: "rgba(51, 51, 51, 0.55)",
                data: {
                    labels: labels,
                    //labels: [
                    //    "Symbian",
                    //    "Blackberry",
                    //    "Other",
                    //    "Android",
                    //    "IOS"
                    //],
                    datasets: [{
                        //data: [15, 20, 30, 10, 30],
                        data: proportionData,
                        backgroundColor: [
                            "#BDC3C7",
                            "#9B59B6",
                            "#E74C3C"
                            //"#26B99A",
                            //"#3498DB"
                        ],
                        //hoverBackgroundColor: [
                        //    "#CFD4D8",
                        //    "#B370CF",
                        //    "#E95E4F",
                        //    "#36CAAB",
                        //    "#49A9EA"
                        //]
                    }]
                },
                toolTip: {
                    enabled: false
                },
                options: {
                    legend: false,
                    responsive: false
                }
            };

            $('.canvasBestCategories').each(function () {
                var chart_element = $(this);
                var chart_doughnut = new Chart(chart_element, chart_doughnut_settings);
            });
        }

    }

    function initChart(data) {
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
                }
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

    var applyDateRange = function () {
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            //console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
            //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
            loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
        });
    };

    //function initDateRangePicker() {
    //    if (typeof ($.fn.daterangepicker) === 'undefined') { return; }
    //    console.log('init_daterangepicker');

    //    var cb = function (start, end, label) {
    //        console.log(start.toISOString(), end.toISOString(), label);
    //        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    //    };

    //    var optionSet1 = {
    //        startDate: moment().subtract(29, 'days'), // ngày hiện tại trừ cho 29 ngày
    //        endDate: moment(), // ngày hiện tại
    //        minDate: '01/01/2012',
    //        //maxDate: moment().format('MM/DD/YYYY'),
    //        maxDate: moment().format('DD/MM/YYYY'),
    //        dateLimit: {
    //            days: 60
    //        },
    //        showDropdowns: true,
    //        showWeekNumbers: true,
    //        timePicker: false,
    //        timePickerIncrement: 1,
    //        timePicker12Hour: true,
    //        ranges: {
    //            'Hôm nay': [moment(), moment()],
    //            'Ngày hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    //            '7 ngày qua': [moment().subtract(6, 'days'), moment()],
    //            '30 ngày qua': [moment().subtract(29, 'days'), moment()],
    //            'Tháng này': [moment().startOf('month'), moment().endOf('month')],
    //            'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    //        },
    //        opens: 'left',
    //        buttonClasses: ['btn btn-default'],
    //        applyClass: 'btn-small btn-primary',
    //        cancelClass: 'btn-small',
    //        //format: 'MM/DD/YYYY',
    //        format: 'DD/MM/YYYY',
    //        separator: ' to ',
    //        locale: {
    //            applyLabel: 'Submit',
    //            cancelLabel: 'Clear',
    //            fromLabel: 'From',
    //            toLabel: 'To',
    //            customRangeLabel: 'Custom',
    //            daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
    //            monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
    //            firstDay: 1
    //        }
    //    };

    //    $('#reportrange span').html(moment().subtract(29, 'days').format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
    //    $('#reportrange').daterangepicker(optionSet1, cb);
    //    $('#reportrange').on('show.daterangepicker', function () {
    //        console.log("show event fired");
    //    });
    //    $('#reportrange').on('hide.daterangepicker', function () {
    //        console.log("hide event fired");
    //    });
    //    $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    //        console.log("apply event fired, start/end dates are " + picker.startDate.format('MMMM D, YYYY') + " to " + picker.endDate.format('MMMM D, YYYY'));
    //        //loadData(picker.startDate.format("MM/DD/YYYY"), picker.endDate.format('MM/DD/YYYY'));
    //        loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
    //    });
    //    $('#reportrange').on('cancel.daterangepicker', function (ev, picker) {
    //        console.log("cancel event fired");
    //    });
    //    $('#options1').click(function () {
    //        $('#reportrange').data('daterangepicker').setOptions(optionSet1, cb);
    //    });
    //    $('#options2').click(function () {
    //        $('#reportrange').data('daterangepicker').setOptions(optionSet2, cb);
    //    });
    //    $('#destroy').click(function () {
    //        $('#reportrange').data('daterangepicker').remove();
    //    });
    //}

    return {
        init
    };
})();
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
                                MethodName: item.receivingTypeName,
                                ProportionOfMethod: `${item.proportionOfDeliverdItems}%`
                            });
                        totalOfTop3 += item.proportionOfDeliverdItems;
                    });
                    if (totalOfTop3 < 100) {
                        render += Mustache.render(template,
                            {
                                Color: response[0].colors[4],
                                MethodName: 'Các phương thức khác',
                                ProportionOfMethod: `${100 - totalOfTop3}%`
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

    function initBestMethodChart(data) {
        if (typeof (Chart) === 'undefined') { return; }

        console.log('init_chart_doughnut');

        if ($('.canvasBestDeliveryMethod').length) {
            var labels = [];
            var proportionData = [];
            var backgroundColors = [];
            $.each(data, function (i, item) {
                labels.push(item.receivingTypeName);
                proportionData.push(item.proportionOfDeliverdItems);
                backgroundColors.push(item.colors[i]);
            });

            var chart_doughnut_settings = {
                type: 'doughnut',
                //tooltipFillColor: "rgba(51, 51, 51, 0.55)",
                data: {
                    labels: labels,
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
        var onedArrayRevenue = [];
        for (var i = 0; i < arrRevenue.length; i++) {
            onedArrayRevenue.push(arrRevenue[i][1]);
        }
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
                min: 0,
                max: onedArrayRevenue.length > 0 ? Math.max(...onedArrayRevenue) : 100000000
            },
            xaxis: {
                mode: "time",
                minTickSize: [1, "day"],
                timeformat: "%d/%m/%y",
                min: $('#reportrange').data('daterangepicker').startDate,
                max: $('#reportrange').data('daterangepicker').endDate
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
                }], chart_plot_02_settings);
        }
    }

    var applyDateRange = function () {
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            loadData(picker.startDate.format("DD/MM/YYYY"), picker.endDate.format('DD/MM/YYYY'));
        });
    };

    return {
        init
    };
})();
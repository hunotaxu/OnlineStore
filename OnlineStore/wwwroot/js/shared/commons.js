var commons = {
    configs: {
        pageSize: 10,
        pageIndex: 1
    },
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
            position: 'top center',
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
    confirm: function (message, okCallback) {
        bootbox.confirm({
            message: message,
            buttons: {
                confirm: {
                    label: 'Confirm',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'Cancel',
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
        var newDate = new Date(parseInt(datetime.substr(6)));
        var month = newDate.getMonth() + 1;
        var day = newDate.getDate();
        var year = newDate.getFullYear();
        var hh = newDate.getHours();
        var mm = newDate.getMinutes();
        if (month < 10)
            month = `0${day}`;
        if (hh < 10)
            hh = `0${hh}`;
        if (mm < 10)
            mm = `0${mm}`;
        return `${day}/${month}/${year}`;
    },
    dateTimeFormatJson: function (datetime) {
        if (datetime === null || datetime === '')
            return '';
        var newdate = new Date(parseInt(datetime.substr(6)));
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
        a[0] = a[0].replace(/\d(?=(\d{3})+$)/g, '$&,');
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
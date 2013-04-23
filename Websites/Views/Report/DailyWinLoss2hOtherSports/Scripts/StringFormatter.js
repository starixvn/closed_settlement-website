

var StringFormatter = {
    formatNumber: function (value) {
        //        var lastDigit = value.toFixed(3) * 1000 % 10;
        //        if (lastDigit < 0) {
        //            lastDigit = lastDigit + 10;
        //        }
        //        if (lastDigit >= 5) {
        //            if (value > 0) {
        //                value = value + 0.005;
        //            }
        //            else {
        //                value = value - 0.005;
        //            }
        //        }
        if (value.toFixed(2).toString() == "-0.00") {
            return "0.00";
        }
        return StringFormatter.addCommas(value.toFixed(2));
    },
    addCommas: function (nStr) {
        nStr += '';
        x = nStr.split('.');
        x1 = x[0];
        x2 = x.length > 1 ? '.' + x[1] : '';
        var rgx = /(\d+)(\d{3})/;
        while (rgx.test(x1)) {
            x1 = x1.replace(rgx, '$1' + ',' + '$2');
        }
        return x1 + x2;
    },

    removeSpaceIE9: function (html) {
        var expr = new RegExp('>[ \t\r\n\v\f]*<', 'g');
        html = html.replace(expr, '><');
        return html;
    },
    getDisplayColorOfValue: function (value) {
        var str = "";
        if (value && value.toFixed(2) > 0) {
            str = "<font color ='red'>" + StringFormatter.formatNumber(value) + "</font>";
        }
        else {
            str = StringFormatter.formatNumber(value);
        }
        return str;
    },
    getDisplayColorOfValueWithSpecialText: function (value, specialText) {
        var str = "";
        if (value && value.toFixed(2) > 0) {
            str = "<font color ='red'>" + StringFormatter.formatNumber(value) + specialText + "</font>";
        }
        else {
            str = StringFormatter.formatNumber(value) + specialText;
        }
        return str;
    },
    get_MMDDYYYY_String: function (date) {
        var month = date.getMonth() + 1;
        if (month < 10) {
            month = "0" + month;
        }

        var day = date.getDate();
        if (day < 10) {
            day = "0" + day;
        }

        var year = date.getFullYear();
        return month + "/" + day + "/" + year;
    }
}

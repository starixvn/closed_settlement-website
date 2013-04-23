/** Array Utilities Function **/
var AUtils = {
    /** array searching function **/
    search: {
        /** array search **/
        exec: function (arr_input, s_key, field) { // field is required when arr_input is an array of object
            var arr_search = [], _s_value = "";
            if (typeof s_key == 'undefined') { var s_key = ""; }
            if (typeof arr_input == 'undefined' || arr_input.length == 0 || s_key == "") { return arr_input; }
            for (var i = 0, len = arr_input.length; i < len; i++) {
                if (typeof (field) != 'undefined') {
                    eval("_s_value = arr_input[" + i + "]." + field + "");
                } else {
                    _s_value = arr_input[i];
                }
                if (typeof (_s_value) == 'undefined' || typeof (_s_value) == 'object') { break; }
                if (_s_value.toString().toLowerCase().indexOf(s_key.toString().toLowerCase()) != -1) {
                    arr_search.push(arr_input[i]);
                }
            }
            return arr_search;
        }
    },
    /** array sorting function **/
    sort: {
        /** field array sort **/
        field: "",
        /** array sort **/
        exec: function (arr_input, orderby, field) { // field is required when arr_input is an array of object
            var arr_sort = [], _s_value = "";;
            if (typeof arr_input == 'undefined' || arr_input.length == 0) { return arr_input; }
            if (typeof (field) != 'undefined') {
                eval("_s_value = arr_input[" + 0 + "]." + field + "");
                this.field = field;
            } else {
                _s_value = arr_input[0];
                this.field = "";
            }
            if (typeof (_s_value) == 'undefined') { return arr_input; }
            switch (typeof (_s_value)) {
                case "number":
                    arr_sort = arr_input.sort(AUtils.sort._compareNumber);
                    if (orderby == 'desc') { arr_sort.reverse(); }
                    break;
                case "string":
                    arr_sort = arr_input.sort(AUtils.sort._compareString);
                    if (orderby == 'desc') { arr_sort.reverse(); }
                    break;
                default:
                    arr_sort = arr_input;
                    break;
            }
            return arr_sort;
        },
        /** compare array number function **/
        _compareNumber: function (a, b) {
            if (AUtils.sort.field != "") {
                eval("a = a." + AUtils.sort.field + "");
                eval("b = b." + AUtils.sort.field + "");
            }
            return a - b;
        },
        /** compare array string function **/
        _compareString: function (a, b) {
            if (AUtils.sort.field != "") {
                eval("a = a." + AUtils.sort.field + "");
                eval("b = b." + AUtils.sort.field + "");
            }
            if (a.toLowerCase() > b.toLowerCase()) { return 1; }
            if (a.toLowerCase() < b.toLowerCase()) { return -1; }
            return 0;
        }
    }
};


var nexcelUtils = {
    trimStart: function (value) {
        while (value[0] != "undefined" && value[0] == " ") {
            if (value.length == 1) return "";
            value = value.substr(1, value.length - 1);
        }
        return value;
    },
    trimEnd: function (value) {
        while (value.length > 0 && value[value.length - 1] != "undefined" && value[value.length - 1] == " ") {
            if (value.length == 1) return "";
            value = value.substr(0, value.length - 1);
        }
        return value;
    },
    trimStartEnd: function (value) {
        return this.trimEnd(this.trimStart(value));
    }
};
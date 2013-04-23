var _utils = {
    blockUI: function () {
        if (parent.parent.BlockUI) {
            try { parent.parent.BlockUI(); } catch (e) { }
        }
        else {
            try { parent.BlockUI(); } catch (e) { }
        }
    },
    unBlock: function () {
        if (parent.parent.$ && parent.parent.$.unblockUI) {
            try { parent.parent.$.unblockUI(); } catch (e) { }
        }
        else {
            try { parent.$.unblockUI(); } catch (e) { }
        }
    },
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
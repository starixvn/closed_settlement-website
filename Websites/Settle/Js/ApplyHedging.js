var apply = {
    updatedMatchHedgingResult: function () {
        var elements = document.getElementsByName("chkApplyHedging");
        var _ids = '';
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].checked) {
                _ids += elements[i].value + '^';
            }
        }

        document.getElementById('txtMatchIds_Hedging').value = _ids;
    }
};
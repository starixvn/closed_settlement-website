var GoalRecords = {
    CheckNoGoal: function () {
        var isDisabled = $('chkNoGoal').checked;
        $('chkFgHome').disabled = isDisabled;
        $('chkFgAway').disabled = isDisabled;
        $('chkLgHome').disabled = isDisabled;
        $('chkLgAway').disabled = isDisabled;
        var chkFGLG = Form.getInputs('frmBill', 'radio', 'chkFGLG').find(function (radio) { return radio.checked; }).value;
        if (!isDisabled && chkFGLG == 2) {
            $('chkLgHome').disabled = true;
            $('chkLgAway').disabled = true;
        }
    },

    CheckValidFGLG: function () {
        if ($('chkRefundAll').checked == false && $('chkNoGoal').checked == false) {
            try {
                var fg = Form.getInputs('frmBill', 'radio', 'chkFg').find(function (radio) { return radio.checked; }).value;
                if ($('chkBillFGNG').checked == false) {
                    var lg = Form.getInputs('frmBill', 'radio', 'chkLg').find(function (radio) { return radio.checked; }).value;
                }
            }
            catch (ex) {
                $('errMsg').style.display = '';
                return false;
            }
        }
        return true;
    },

    FocusFGAndNoGoal: function (rdo) {
        $('chkNoGoal').disabled = false;
        if (rdo.id == 'chksBillFGNG') {
            $('chkLgHome').disabled = true;
            $('chkLgAway').disabled = true;
        }
        else if ($('chkNoGoal').checked == false) {
            $('chkLgHome').disabled = false;
            $('chkLgAway').disabled = false;
        }
    },

    InputGoalRecord: function (e) {
        var code = '';
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;

        //alert(code);
        //A:65, 97| h:72, 104| back:8| move left:37| move right:39| delete:46
        if (code == 65 || code == 97 || code == 72 || code == 104 || code == 8 || code == 35 || code == 36 || code == 37 || code == 39 || code == 46 || code == 9)
            return true;
        else
            return false;
    },

    DisplayGoalHistory: function () {
        $('errMsg').style.display = 'none';
        var goalrecord = $('GR').value;
        var param = '?mode=1&goalrecord=' + goalrecord;
        var myajax = new Ajax.Request("ProcessData.ashx", {
            asynchronous: true,
            method: 'post',
            parameters: param,
            onComplete: function (rs) {
                var str = rs.responseText;
                $('spGoalHistory').innerHTML = str;
            }
        });
    },

    RePlaceStr: function (e) {
        $('GR').value = ReplaceAll($('GR').value, '\'', '');

        var code = '';
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;

        if (code == 9 || code == 13) {
            GoalRecords.DisplayGoalHistory();
        }
        if ($('sHasFGLG').value == '1') {
            if ($('GR').value == "") {
                $('hfgoal').style.display = 'none';
                $('hlgoal').style.display = 'none';
                $('afgoal').style.display = 'none';
                $('algoal').style.display = 'none';
            }
            else {
                if ($('GR').value.substring(0, 1) == "h" || $('GR').value.substring(0, 1) == "H") {
                    $('hfgoal').style.display = "inline";
                    $('afgoal').style.display = "none";
                }
                else {
                    $('hfgoal').style.display = "none";
                    $('afgoal').style.display = "inline";
                }
                if ($('homeF').value != "" & $('awayF').value != "") {
                    if ($('GR').value.substring($('GR').value.length - 1, $('GR').value.length) == "h" || $('GR').value.substring($('GR').value.length - 1, $('GR').value.length) == "H") {
                        $('hlgoal').style.display = "inline";
                        $('algoal').style.display = "none";
                    }
                    else {
                        $('hlgoal').style.display = "none";
                        $('algoal').style.display = "inline";
                    }
                }
                else {
                    $('hlgoal').style.display = "none";
                    $('algoal').style.display = "none";
                }


            }
        }


    }
};
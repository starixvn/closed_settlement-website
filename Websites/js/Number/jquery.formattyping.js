jQuery(document).ready(function(){
//(2, '.', ',')
Number.prototype.formatMoney = function(c, d, t) {
    var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0;
    return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "jQuery1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : "");
};

jQuery.fn.ForceDecimalOnly = function() {
    return this.each(function() {
        jQuery(this).keydown(function(e) {
            //at keydown event, new-value is not available on textbox, still old-value
            //at keydown event, prevent "not allow keys" is a main action
            //not format at this event
            //-------------------------------------------
            //check dot(.), just for currency
            var key = e.charCode || e.keyCode || 0;
            if (key == 110 || key == 190) {//dot(.)
                if (jQuery(this).val() == "" || jQuery(this).val().indexOf(".") >= 0) {
                    return false;
                }
            }
            //-------------------------------------------
            var isAccept =
             (
                (e.ctrlKey && e.keyCode == 67) || //accept ctrl + c(copy)
                (e.ctrlKey && e.keyCode == 86) || //accept ctrl + v(paste)
                key == 16 ||//shift
                key == 17 ||//ctrl
                key == 8 || //backspace
                key == 36 || //home
                key == 35 ||//end
                key == 110 ||  //.
                key == 190 ||  //.
                key == 9 ||//tab
                key == 46 ||//delete
                key == 109 || //-
                key == 189 //-
                (key >= 37 && key <= 40) ||//left , right , up , down (arrow)
                (key >= 48 && key <= 57) ||//0-9
                (key >= 96 && key <= 105));//numpad 0-9
            return isAccept;
        });
    });
};

jQuery.fn.ForceCurrencyOnly = function() {
    return this.each(function() {
        jQuery(this).keyup(function(e) {
            //if at keydown event is stopped, keyup event has still be fired
            //at keyup event, new-value is available on textbox
            //at keyup event, format is a main action , except some follow cases
            //--------------------------------------
            //do not format for these cases
            var key = e.charCode || e.keyCode || 0;
            if (key == 36 //home
                || key == 17 //ctrl
                || key == 16 //shift
                || key == 35 //end
                || key == 37 //left arrow
                || key == 38 //up arrow
                || key == 39//right arrow
                || key == 40//down arrow
                || key == 109//-
                key == 189 //-
                ) {
                return true;
            }
            //--------------------------------------
            
            var value = jQuery(this).val();
            
            if (isNaN(parseFloat(value))) {
                jQuery(this).val('');
                return;
            }
            
            var backupValue = jQuery(this).val();
            
            if (value.indexOf(".") == value.length - 1 || parseFloat(value) == 0) {
                return true;
            }
            
            if (value.indexOf(".") >= 0) {
                var arr = value.split('.');
                if (parseInt(arr[1]) == 0) {
                    return true;
                }
            }
            
            while (value.indexOf(",") >= 0) {
                value = value.replace(/,/g, '');
            }
            var strFormated = parseFloat(value).formatMoney(4, ".", ",");
            while ((strFormated.substr(strFormated.length - 1, 1) == "0" && strFormated.indexOf(".") >= 0) || strFormated.substr(strFormated.length - 1, 1) == ".") {
                strFormated = strFormated.substr(0, strFormated.length - 1);
            }
            //backup current caret position
            var currentPosition = jQuery(this).caret().start;
            
            //set formated value
            jQuery(this).val(strFormated);
            
            //recover current caret position
            var numberIncreasedChars = strFormated.length - backupValue.length;
            if (numberIncreasedChars >= 0) {
                jQuery(this).setCaretPosition(currentPosition + numberIncreasedChars);
            }
            else {
                jQuery(this).setCaretPosition(currentPosition);
            }
        });
    });
};

});
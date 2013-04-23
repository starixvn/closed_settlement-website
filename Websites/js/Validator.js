//regexp-validate-numbers
function uInteger(str) {
    /* Verify unsigned integer
     *        ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[0-9]+$/.test(str);
}//eof - uInteger

function uInteger2(str) {
    /* Verify unsigned integer
     *        ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^\d+$/.test(str);
}//eof uInteger2

function uInteger3to5(str) {
    /* Verify unsigned integer 3 to 5 digits
     *        ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^\d{3,5}$/.test(str);
}//eof - uInteger3to5

function sInteger(str) {
    /* Verify signed integer at least one digit
     *        ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[-+]?[0-9]+$/.test(str);
}//eof - sInteger

function sInteger2(str) {
    /* Verify signed integer at least one digit
     *        ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[-+]?\d+$/.test(str);
}//eof - sInteger2

function sInteger3to5(str) {
    /* Verify signed integer 3 to 5 digits
     *        ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[-+]?\d{3,5}$/.test(str);
}//eof - sInteger3to5

function isFloat(str) {
    /* Verify signed float decimal optional
     *    ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[-+]?[0-9]+(\.[0-9]+)?$/.test(str);
}//eof isFloat

function isFloat2(str) {
    /* Verify signed float decimal optional
     *    ignoring leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[-+]?\d+(\.\d+)?$/.test(str);
}//eof isFloat2

function isFloat3to5d0to3(str) {
    /* Verify float 3 to 5 digits, decimal max 3 digits
     *    if decimal point must have at least 1 decimal digit
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^[-+]?\d{3,5}(\.\d{1,3})?$/.test(str);
}//eof - isFloat3to5d10to3

function isCurrency1(str) {
    /* Verify formatted currency optional $, no leading zero
     *    reguire comma separator, 2 digit decimal if any
     *    ignore leading and trailing spaces
     * Return boolean
     */
    str = str.replace(/^\s+|\s+$/g, '');
    return /^\$?[1-9][0-9]{0,2}(,[0-9]{3})*(\.[0-9]{2})?$/.test(str);
}//eof - isCurrency1

function isCurrency2(str) {
    /*  Verify formatted currency optional $, no leading zero
     *    reguire comma separator, 2 digit decimal if any
     *    fillin missing decimal
     *    ignore leading and trailing spaces
     * Return string
     */
    str = str.replace(/^\s+|\s+$/g, '');
    if (/^\$?[1-9][0-9]{0,2}(,[0-9]{3})*(\.[0-9]{0,2})?$/.test(str) ) {

        if (/\.[0-9]$/.test(str) ) {
            //needs trailing zero
            str += "0";
        }
        else if (/\.$/.test(str)) {
            //needs both trailing zeroes
            str += "00";
        }
        else if (!/\.[0-9]{2}$/.test(str) ) {
            //needs everything
            str += ".00";
        }
        return str;
    }
    else {
        return "Invalid";
    }
}//eof - isCurrency2

function isCurrency3(str) {
    /* Verify formatted currency optional $, no leading zero
     *    reguire comma separator, 2 digit decimal if any
     *    fillin missing decimal
     *    ignore leading and trailing spaces
     * Return string
     */
    str = str.replace(/^\s+|\s+$/g, '');

    if (/^[-+]?\$?[1-9]\d{0,2}(,\d{3})*(\.\d{0,2})?$/.test(str) ) {

        if (/\.\d$/.test(str)) {
            str += "0";
        }
        else if (/\.$/.test(str) ) {
            str += "00";
        }
        else if (!/\.\d{2}$/.test(str) ) {
            str += ".00";
        }
        return str;
    }
    else {
        return "Invalid";
    }
}//eof - isCurrency3
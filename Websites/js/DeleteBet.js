function obj(id){
	return document.getElementById(id);
}
function hideLanguage() {
    if (String(obj('other_display').innerHTML).indexOf("-") != -1) {
        //current opened
        obj("otherLanguage_title").style.display = "none";
        obj("otherLanguage_content").style.display = "none";
        obj('other_display').innerHTML = "<span style=\"color: Black;\">Other Language</span> [+]";
    }
    else {
        //current closed
        obj("otherLanguage_title").style.display = "";
        obj("otherLanguage_content").style.display = "";
        obj('other_display').innerHTML = "<span style=\"color: Black;\">Other Language</span> [-]";
    }
}
function check_UnameAndPwd() {
    $('#errBox').removeAttr('class');
    
    obj('errormsg').innerHTML = '';
    if (obj('username').value == "") {

        showAlertMessage("errormsg", "Please enter username", 1);
        obj('username').focus();
        return false;
    } else if (obj('password').value == "") {
        showAlertMessage("errormsg", "Please enter password", 1);
        obj('password').focus();
        return false;
    }

    if (obj('betfrom') && obj('betfrom').value == 'p') 
    {
        var str = obj('men').value + obj('mch').value;
        if (str.length > 0)
            return CheckValidPrivateMsg();
        else
            return true;
    }
    else
        return CheckValidPrivateMsg();
}
function CheckValidPrivateMsg() { 
    if (obj('men') && obj('men').value == "") {
        showAlertMessage("errormsg", "You must enter English message", 1);
        obj('men').focus();
        return false;
    }
    else if (obj('men') && obj('men').value.indexOf('@@') == -1) {
        showAlertMessage("errormsg", "You must include @@ in English message for replacing ticket numbers", 1);
        obj('men').focus();
        return false;
    }
    else if (obj('mch') && obj('mch').value == "") {
        showAlertMessage("errormsg", "You must enter Chinese message", 1);
        obj('mch').focus();
        return false;
    }
    else if (obj('mch') && obj('mch').value.indexOf('@@') == -1) {
        showAlertMessage("errormsg", "You must include @@ in Chinese message for replacing ticket numbers", 1);
        obj('mch').focus();
        return false;
    }
    else if (obj('mcs') && (obj('mcs').value != "") && (obj('mcs').value.indexOf('@@') == -1)) {
        showAlertMessage("errormsg", "You must include @@ in Simple Chinese message for replacing ticket numbers", 1);
        obj('mcs').focus();
        return false;
    }
    else if (obj('mth') && (obj('mth').value != "") && (obj('mth').value.indexOf('@@') == -1)) {
        showAlertMessage("errormsg", "You must include @@ in Thai message for replacing ticket numbers", 1);
        obj('mth').focus();
        return false;
    }
    else if (obj('mit') && (obj('mit').value != "") && (obj('mit').value.indexOf('@@') == -1)) {
        showAlertMessage("errormsg", "You must include @@ in Italian message for replacing ticket numbers", 1);
        obj('mit').focus();
        return false;
    }
    else if (obj('mjp') && (obj('mjp').value != "") && (obj('mjp').value.indexOf('@@') == -1)) {
        showAlertMessage("errormsg", "You  must include @@ in Japanese message for replacing ticket numbers", 1);
        obj('mjp').focus();
        return false;
    }
    else if (obj('mes') && (obj('mes').value != "") && (obj('mes').value.indexOf('@@') == -1)) {
        showAlertMessage("errormsg", "You must include @@ in Spanish message for replacing ticket numbers", 1);
        obj('mes').focus();
        return false;
    }
    return true;
}
function executeDel()
{
    if (check_UnameAndPwd()== true)
    {
        obj('errormsg').innerHTML = '';
        var link = '../Handlers/DeleteBet.ashx?betid=' + obj('betID').value;
        
        link = link + '&betfrom=' + obj('betfrom').value;
        link = link + '&matchid=' + obj('matchID').value;
        link = link + '&username=' + encodeURIComponent(obj('username').value);
        link = link + '&password=' + encodeURIComponent(obj('password').value);
        link = link + '&men=' + encodeURIComponent(obj('men').value);
        link = link + '&mch=' + encodeURIComponent(obj('mch').value);
        link = link + '&mcs=' + encodeURIComponent(obj('mcs').value);
        link = link + '&mth=' + encodeURIComponent(obj('mth').value);
        link = link + '&mit=' + encodeURIComponent(obj('mit').value);
        link = link + '&mjp=' + encodeURIComponent(obj('mjp').value);
        link = link + '&mes=' + encodeURIComponent(obj('mes').value);
        link = link + '&bettype=' + obj('Bettype').value;
        
         $.ajax({
        url: link,
        dataType:'html',
        success: function(data)
        {
            if (data!="")
            {
                var input = data.split("|");
                if (input.length>1)
                {
                    if (input[0]=="Error")
                    {
                        $('#errBox').attr('class', 'alertboxerr');
                        obj('errormsg').innerHTML = input[1];
                        //obj('username').value = "";
                        obj('password').value = "";
                    }
                    else if (input[0] == "Success")
                    {
                        obj('errormsg').innerHTML = '';
                        alert(input[1]);
                        opener.location.reload();
                        close();
                    }   
                }
            }
            else
                obj('errormsg').innerHTML = '';
        }
    });
    }
    else
    {
        $('#errBox').attr('class', 'alertboxerr');
    }
}
function Reset()
{
    obj('username').value = "";
    obj('password').value = "";
    obj('men').value = "";
    obj('mch').value = "";
    obj('mcs').value = "";
    obj('mth').value = "";
    obj('mit').value = "";
    obj('mjp').value = "";
    obj('mes').value = "";
    obj('errormsg').innerHTML = '';
    obj("otherLanguage_title").style.display = "none";
    obj("otherLanguage_content").style.display = "none";
    obj('other_display').innerHTML = "<span style=\"color: Black;\">Other Language</span> [+]";
    
}

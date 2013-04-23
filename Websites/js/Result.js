// JScript File

function DateSubmit() {
    try { parent.BlockUI(); } catch (e) { BlockUI(); }
    var myajax = new Ajax.Request("GetResultMatch.ashx", {
        asynchronous: true,
        method: 'post',
        parameters: 'type=getLeagues&lid=0|0&fdate=' + $('fdate').value,
        onSuccess: function (rs) {
            $('selLeague').innerHTML = rs.responseText;
            try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
        }
    });
    ChangeLeague('0|0');
}

function ChangeLeague(lid) {
    try { parent.BlockUI(); } catch (e) { BlockUI(); } 
    var leagueID =(lid) ?lid :$('ddlLeagues').options[$('ddlLeagues').selectedIndex].value; 
    var myajax= new Ajax.Request("GetResultMatch.ashx",{
        asynchronous:true,
        method:'post',        
        parameters:'type=getMatchs&lid=' +leagueID +'&fdate=' +$('fdate').value,
        onComplete:BindMatchs
    });
}

function BindMatchs(rs) {
    $('_sContent').innerHTML =rs.responseText;
    try { parent.jQuery.unblockUI(); } catch (e) { jQuery.unblockUI(); }
}


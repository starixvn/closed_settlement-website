<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CSSyncList.aspx.cs" Inherits="CSSyncList"
    Theme="" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <title>Casino Synchronised List</title>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/prototype-full.js"></script>

    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ViewReport(SearchBy) {
            if ($('txtUserName').value != '') {
                var url = 'CSSyncList.aspx?UserName=' + $('txtUserName').value + '&SearchBy=' + SearchBy;
                location.href = url;
            }
            else
                showAlertMessage("errMsg","Please input Username!", 1);
        }
        function OpenSyncUpd(id) {
            var allowUpd = confirm('Do you want to synchronise this customer to Casino system?');
            if (allowUpd) {
                var param = 'CustID=' + id
                var myajax = new Ajax.Request('../Handlers/UpdateSyncCasino.ashx', {
                    method: 'post',
                    parameters: param,
                    onComplete: function(data) {
                        if (data.responseText != 'succeed')
                            alert(data.responseText);
                        else {
                            //parent.opener.DelayReloadPage();
                            DelayReloadPage(null, 2000);
                            setTimeout('ViewReport(0);', 2500);
                        }
                    }
                });
            }
        }
        function OpenEditCS(id) {
            var url = "Customer/PositionTakingCS.aspx?set=2&FlagUnSyn=1&custId=" + id;
            showPopup("Update Casino Setting", url ,"","",965,650, false, 2000, null);
        }
        function KeyPressEnter(e) {
            if ((e.charCode ? e.charCode : e.keyCode) == 13)
                ViewReport($('SearchBy').value);
        }
    </script>

</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Casino Sync/Enable Check</td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
        <div class="maincontent_box ">   
            <div id="errMsg"></div>
            <div id="clear2"></div>         
            <b>UserName: </b>
            <input type="text" class="txt" id="txtUserName" onkeypress="KeyPressEnter(event)"
                value="<%=vlUserName %>" />
            <input type="hidden" id="SearchBy" value="<%=vlSearchBy%>" />&nbsp;
            <input type="submit" onclick="ViewReport(0)" class="btn" value="Search (IBC)" />
            <input type="submit" onclick="ViewReport(1)" class="btn" value="Search (Casino)" />
        </div>
        <div class="bg_table">
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <asp:Literal ID="headerDatetime" runat="server"></asp:Literal>
        </div>
    </div>
</body>
</html>

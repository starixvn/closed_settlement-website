<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditGroup.aspx.cs" Inherits="UserControl_EditGroup" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Group</title>
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/common.js?v=<%=AccCommon.Version() %>"></script>
    <script src="../js/BlockUI.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.min.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.blockUI.1.33.js"></script>
    <script type="text/javascript">
        function OnUpdate(type) {
            if (obj("txtDept").value == "") {
                alert_error('Please input Department name.', 'err');
            }
            else {
                BlockUI();
                document.fEditDept.submit();
            }
        }
        function OnCancel(type) {
            alert_error("", 'err');
            if (typeof parent.ClosePopup != "undefined")
                parent.ClosePopup();
        }
    </script>

</head>
<body style="margin:5px 10px 5px 5px">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">Edit Group
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                <div id="sContent">
                    <iframe style="display: none" name="frUpd"></iframe>
                    <asp:Literal ID="htmlEdit" runat="server"></asp:Literal></div>
            </div>
        </div>
    </div>
</body>
</html>

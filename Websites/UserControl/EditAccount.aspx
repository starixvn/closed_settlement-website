<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditAccount.aspx.cs" Inherits="UserControl_EditAccount" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" Theme="">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Edit Account</title>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/common.js?v=<%=AccCommon.Version() %>"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
</head>
<body style="margin:5px 10px 5px 5px">
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC"> Edit Account
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box">
                 <div class="errMsg" id="errMsg"></div>
            </div>
            <div class="bg_table">
                <div id="sContent">
                    <iframe style="display: none" name="frUpd"></iframe>
                    <asp:Literal ID="htmlEditAcc" runat="server"></asp:Literal></div>
            </div>
        </div>
    </div>
</body>
</html>

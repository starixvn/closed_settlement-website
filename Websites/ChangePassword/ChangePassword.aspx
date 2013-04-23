<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="ChangePassword_ChangePassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    
</head>
<body style="margin: 5px 10px 5px 5px">
    <div class="main">
        <div class="maincontent" style="width:350">
            <div class="maincontent_box ">
                <%=msg%>
                <!--Input box-->
                <div id="clear2">
                </div>
                <asp:Literal ID="content" runat="server"></asp:Literal>
                <%=strscript%>
            </div>
        </div>
    </div>
</body>
</html>

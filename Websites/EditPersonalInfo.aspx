<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="EditPersonalInfo.aspx.cs" Inherits="EditPersonalInfo" Title="Untitled Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="js/common.js"></script>
    <script src="js/showhidepopup.js" type="text/javascript"></script>
 
    
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">Edit Your Info
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box ">
                <form action="Handlers/UpdateAccInformation.ashx" method="post" target="frUpdate">
                <asp:Literal ID="sBody" runat="server"></asp:Literal>
                </form>
                <iframe name="frUpdate" frameborder="0" height="0" width="0"></iframe>
            </div>
        </div>
    </div>
</body>
</html>

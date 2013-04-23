<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VerifyPassword.aspx.cs" Inherits="VerifyPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="js/common.js"></script>

    <script type="text/javascript">
        function checkKeyPress(e) {
            if (typeof e == 'undefined') e = window.event;
            var key = (typeof e.keyCode == 'undefined') ? e.which : e.keyCode;
            if (key == 13) return true;
        }
    </script>

</head>
<body>
    <div class="main">
        <div class="maincontent">

            <div id="tabmain_conts">
                <div id="content_tab">
                    <ul>
                        <li id="current"><a href="#"><span>
                            Personal Setting</span></a></li>
                    </ul>
                </div>
            </div>

            <div class="maincontent_box ">
                <div class="alertboxwrn"><div>To protect your account, you need to change your password periodically.</div></div>  
            </div>
            <div class="bg_table">
                <form method="post" action="Handlers/VerifyPassword.ashx" target="frmVerify">
                    <div class="error-message" id="errMsg" name="errMsg"></div>
                    <table class="tbllayout" style="width:220px;">
                        <tr>
                            <td colspan="2" class="title">
                                Please verify your password
                            </td>
                        </tr>
                        <tr>
                            <td width="120px" class="l b">
                                UserName:
                            </td>
                            <td width="120px" class="l b">
                                <%=accname%>
                            </td>
                        </tr>
                        <tr>
                            <td class="l b">
                                Password:
                            </td>
                            <td>
                                <input type="password" id="password" name="password" class="txt" onkeypress="return checkKeyPress(event);" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="c">
                                <input type="submit" id="submit" name="submit" class="btn" value="Submit" />
                            </td>
                        </tr>
                    </table>
                </form>
                <iframe id="frmVerify" name="frmVerify" style="display: none" width="0" height="0">
                </iframe>
            </div>
        </div>
    </div>
</body>
</html>

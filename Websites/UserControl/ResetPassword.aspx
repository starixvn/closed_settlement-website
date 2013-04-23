<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ResetPassword.aspx.cs" Inherits="UserControl_ResetPassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/common.js"></script>

    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript">
        function validation() {
            try {
                var pass =document.getElementById('password').value.trim();
                if (pass.isNullOrEmpty()) return raiseErr('Sorry, password can not be empty!!!');
                if (pass.length <4) return raiseErr('Password must have at least 4 characters!!!');
                
                var confirm =document.getElementById('confirm').value.trim();
                if (confirm.isNullOrEmpty()) return raiseErr('Please input confirm password!!!');
                if (confirm.compare(pass, true) != 0) return raiseErr('The password was not correctly confirmed!!!');                
            }
            catch (ex){
                alert(ex);
            }
            return true;
        }
        
        function raiseErr(msg) {
            showAlertMessage("errMsg", msg,1 );
           // alert(msg);
            return false;
        }
    </script>
</head>
<body>    
    <form method="post" action="ResetPassword.aspx">
     <div class="main" style="width:400px;">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Reset Password
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div class="maincontent_box ">
                <div class="errMsg" id="errMsg"></div>
                <!--Input box-->
                <div id="clear2">
                </div>
                <asp:literal ID="sBody" runat="server"></asp:literal>
            </div>
        </div>
    </div>  
    </form>
</body>
</html>


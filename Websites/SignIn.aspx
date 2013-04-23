<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SignIn.aspx.cs" Inherits="SignIn" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SignIn</title>
    <link href="App_Themes/SettleTheme2012/Login/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form action="PreLogIn/ProcessLogin.ashx" method="post" id="LoginForm" target="controlFr">
    <center>
        <div class="contents_box">               
                <div class="login_box">
                    <table width="100%" align="left" border="0" cellpadding="0" cellspacing="0" >
                            <tbody>
                                <tr>
                                    <td class="top_left">
                                        &nbsp;
                                    </td>
                                    <td class="login_bg_middle_top" colspan="2">
                                        &nbsp;
                                    </td>
                                    
                                    <td class="top_right">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="middle_left">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="login_bg_middle ">
                                        <div class="systemlogo"></div>
                                    </td>
                                    
                                    <td class="middle_right">
                                        &nbsp;
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="middle_left">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="middle" class="left_text login_bg_middle">
                                        <span class="normaltext_pad">Username </span>
                                    </td>
                                    <td width="287" align="left" valign="middle" class="login_bg_middle">
                                        <input name="TxtUsername" type="text" class="log_text" id="TxtUsername" style="width: 230px;"
                                            autocomplete="off" />
                                    </td>
                                    <td  class="middle_right">
                                        &nbsp;
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td class="middle_left">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="middle" class="left_text login_bg_middle">
                                        <span class="normaltext_pad">Password</span>
                                    </td>
                                    <td align="left" class="login_bg_middle">
                                        <input name="TxtPassword" type="password" class="log_text" id="TxtPassword" style="width: 230px;"
                                            autocomplete="off" onkeypress="return signIn.checkKeyPress(event);" />
                                    </td>
                                     <td  class="middle_right">
                                        &nbsp;
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="middle_left">
                                        &nbsp;
                                    </td>
                                    <td align="left" valign="middle" colspan="2" class="login_bg_middle">
                                        <div class="war_box">
                                            <div id="ErrorMessage" class="comment" style="display: none;">
                                                <span></span><span id="msgContent"></span>
                                                <ul>
                                                </ul>
                                                <div id="divLoginFail" style="display: none;">
                                                    Please try again after 1 minute(s) (<span id="spanDownTime"></span>).
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    
                                    <td class="middle_right"></td>
                                </tr>
                                <tr>
                                    <td class="bottom_left">
                                        &nbsp;
                                    </td>
                                    <td colspan="2" class="login_bg_middle_bottom">
                                        <div class="recommendtext">Recommended Browsers</div>
                                        <div class="viewIE">IE8</div>
                                        <div class="viewFF">Firefox 4</div>
                                        <input value="" onclick="return signIn.login();" class="log_button" type="submit" />
                                    </td>
                                    
                                    <td class="bottom_right">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                </div>
            </div>
    </center>
    </form>
     
    <iframe id="controlFr" name="controlFr" style="display: none;"></iframe>
    <script src="js/jQuery/jquery.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var plsInputUserNameErr = '<%=Resources.ErrorMsg.lblPlsEnterUserName %>';
        var plsInputPassErr = '<%=Resources.ErrorMsg.lblPlsEnterPassword %>';

        var signIn = {
            login: function () {
                $('#ErrorMessage').hide();
                

                var userName = document.getElementById('TxtUsername').value;
                var password = document.getElementById('TxtPassword').value;

                if (this.trimSpace(userName) == '') {
                    this.loginError(plsInputUserNameErr);
                    $('#TxtUsername').focus();
                    return false;
                }
                else if (this.trimSpace(password) == '') {
                    this.loginError(plsInputPassErr);
                    $('#TxtPassword').focus();
                    return false;
                }
                return true;
            },
            loginError: function (errMsg) {
                $('#TxtPassword').val('');
                $('#ErrorMessage').html(errMsg);
                $('#ErrorMessage').show();
            },
            trimSpace: function (str) {
                return str.replace(/\s/g, "");
            },

            checkKeyPress: function (e) {
                try {
                    if (typeof e == 'undefined') e = window.event;
                    var key = (typeof e.keyCode == 'undefined') ? e.which : e.keyCode;

                    if (key == 13) {
                        if (!signIn.isRequestLogin)
                            signIn.login();
                    }
                } catch (ex) {
                    showError(ex);
                }
            }

        }
    </script>
    
</body>
</html>
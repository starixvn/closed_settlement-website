<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MeetingComm.aspx.cs" Inherits="UserControl_Menu" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Configurations" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/common.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/table.css" />
    <link rel="stylesheet" type="text/css" href="../App_Themes/Public/input.css" />
    <link href="css/meetingcomlist.css" rel="stylesheet" type="text/css" />
    <script src="../js/jQuery/jquery.min.js" type="text/javascript"></script>
    <script src="../js/Common/Functions.js" type="text/javascript"></script>
    <script src="../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script src="../js/jQuery/jquery.jcache.js" type="text/javascript"></script>
    <script src="js/MeetingList.js?v=<%=WebSettings.Version%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            Meeting.getPartnerList();
        });
    </script>
</head>
<body>
    <div class="maincontent">
        <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
            <tr>
                <td class="maincontent_headerL">
                </td>
                <td class="maincontent_headerC">
                    Meeting Commission List
                </td>
                <td class="maincontent_headerR">
                </td>
            </tr>
        </table>
        <div class="maincontent_box " style="float: left; width: 716px;">
            <div id="partner_container" style="float: left; padding: 7px 0px 10px 0px;">
                <div class="panel_search">
                    <b>Partner Type</b>&nbsp;
                    <select id="partner_type" style="width: 135px;">
                        <option value="-1">Choose a partner</option>
                    </select>
                </div>
                <div id="partner_detail" class="partner_detail" style="display: none;">
                    <div class="header">
                        <div class="line">
                            <div class="label noborder" id="span_title">
                                Partner Type</div>
                        </div>
                        <div class="line noborder">
                            <div class="number hrow">
                                #</div>
                            <div class="meeting hrow">
                                Meeting</div>
                            <div class="currency hrow">
                                Pool Currency</div>
                            <div style="float: left; width: 302px; overflow: hidden;">
                                <div style="float: left; width: 302px; border-bottom: 1px solid #7C98A4; padding: 3px 0px;">
                                    Commission(%)</div>
                                <div class="noborder" style="width: 302px;">
                                    <div class="comm">
                                        Win</div>
                                    <div class="comm">
                                        Place</div>
                                    <div class="comm noborder">
                                        WinPlace</div>
                                </div>
                            </div>
                            <div class="fee hrow">
                                Fee(%)</div>
                        </div>
                    </div>
                    <div id="partner_content" class="content">
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>

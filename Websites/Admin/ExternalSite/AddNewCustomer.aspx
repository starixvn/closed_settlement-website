<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewCustomer.aspx.cs" Inherits="Admin_ExternalSite_AddNewCustomer" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>External Site</title>
  
    <link href="../../App_Themes/Public/common.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/UI/css/custom-theme/jquery-ui-1.8.10.custom.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../../js/jQuery/Pager/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Report/reportgrid.css" rel="stylesheet" type="text/css" />
    <link href="style/AddCustomer.css" rel="stylesheet" type="text/css" />
   
   
   
    

</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table cellspacing="0px" cellpadding="0px" border="0" class="maincontent_header">
                <tbody>
                    <tr>
                        <td class="maincontent_headerL">
                        </td>
                        <td class="maincontent_headerC">
                            Manage External Accounts
                        </td>
                        <td class="maincontent_headerR">
                        </td>
                    </tr>
                </tbody>
            </table>
            <div class="maincontent_box" style="width: 796px">
                <div style="display: none;" id="trMsg">
                    <div id="dvMsg">
                    </div>
                </div>
                <table>
                    <tr>
                    <td>
                       Username
                    </td>
                    <td colspan="2">
                     <input id="txt_search" type="text" onkeyup="javascript:onKeyEnter(event);" class="text" value="" style="width:97%" />
                    </td>
                    
                    </tr>
                    <tr>
                        <td>
                            System
                        </td>
                        <td>
                            <select id="groupName" onchange="changeGroup()"  style="width: 102px; border: 1px solid Black">
                            </select>
                        </td>
                        <td>
                            <input type="button" id="btnSearch" value="Submit" class="btn" onclick="ExterSite.SubmitData()" />
                        </td>
                       
                    </tr>
                </table>
            </div>
            <div style="display: none; width:801px" id="nodata">
                <div id="err_nodata">
                </div>
            </div>
          
            <div id="gridExAccControl" class="content-warper hidden">
                        <div class="content-toolbox bgtitle">
                           
                            <div class="btngroup">
                            <input type="button" title="Add Existing Super" class="btnAdd" id="btnAdd" name="btnAdd" value="  " onclick="showPopup();"/>
                             <input type="button" title="Refresh" class="btnRefresh" id="btnRefresh" name="btnRefresh" value="  " onclick="ExterSite.refreshPage();"/>
                           
                            </div>
                        </div>
                        <div id="nodata_search" style="display:none"></div>
                    <div class="content-header clearfix">
                        <div id="headerscrollbettype" class="header-scroll">
                            <table class="tbl-header-scroll">
                                <tr>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w50 c b">
                                            No</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w180 c">
                                            Username</div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w180 c">
                                            <%=Resources.Language.FirstName %></div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w180 c">
                                            <%=Resources.Language.LastName %></div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w120 c">
                                            <%=Resources.Language.iplogin%></div>
                                    </td>
                                    <td style="border-bottom: none 0px;">
                                        <div class="w55 c">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="content-body clearfix">
                        <div class="body-scroll">
                            <table id="tblexternalsite" class="tbl-body-scroll">
                            </table>
                        </div>
                    </div>
                    <div class="content-pagination bgtitle clearfix">
                        <div class="pagination-scroll">
                            <table style="width: 100%; padding-top: 2px" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <div id="pagination">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
       
    </div>
    <div id="addNewSuper" class="hidden">
        <span id="errMsg"></span>
        <table>
        <tr>
            <td>
                <div class="w60 c">Username
                </div>
            </td>
            <td>
                <div class="w150">
                    <input type="text" id="txtUserName" style="width:140px" /> 
                </div>
            </td>
            <td>
                <input type="button" id="Button2" value="Add" class="btn" onclick="ExterSite.addCustomer()" /> 
            </td>
        </tr>
        </table>
    </div> 
    <script src="../../js/jQuery/jquery.js" type="text/javascript"></script>
    <script src="../../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js" type="text/javascript"></script>
    <script src="../../js/jQuery/gridscroller.js" type="text/javascript"></script>
    <script src="../../js/jQuery/jquery.json-2.2.min.js" type="text/javascript"></script>
    <script src="../../js/common.js" type="text/javascript"></script>
    <script src="../../js/jQuery/Pager/jpager.js" type="text/javascript"></script>
    <script src="../../js/showhidepopup.js" type="text/javascript"></script>
    <script src="../../js/Common/ArrayUtils.js" type="text/javascript"></script>
    <script src="js/ExternalSite.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        renderGroup();
    </script>
</body>
</html>

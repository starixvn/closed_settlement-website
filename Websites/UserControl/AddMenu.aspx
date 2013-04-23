<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddMenu.aspx.cs" Inherits="UserControl_AddMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/users.css" rel="stylesheet" type="text/css" />
    <link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css"/>      

    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script src="../js/jQuery/jquery-jtemplates.js" type="text/javascript"></script>
     <script src="../js/jQuery/Template/jquery.tmpl.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Customer.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/user.js"></script>
    <script type="text/javascript" src="../js/IBCWnd/IBCWnd.js"></script>
    <script src="JS/ManageMenu.js" type="text/javascript"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
</head>
<body>
    <script id="systemTmpl" type="text/x-jquery-tmpl">
    <option value="${Name}">${Name}</option>
    </script> 
    <!--/System Option-->
    <!--End Template-->

    <script type="text/javascript">
        window.onload = function () {
            GetSystemList();

            var funcName = $('#funcName').val();
            LoadFuncGroup(funcName);
            $('#trsystem').hide();
//            if ($('#edit').val() == 1) //edit
//            {
//                $('#trsystem').hide();
//            }
//            else { //Add new
//                $('#ddlSystem').val($('#system').val());
//            }
        }

    </script>
    <div style="width:490px">
       <div class="main">
            <div class="maincontent">
                <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                    <tr>
                        <td class="maincontent_headerL"></td>
                        <td class="maincontent_headerC"><%=ActionTypeMenu %></td>
                        <td class="maincontent_headerR"></td>
                    </tr>
                </table>
                
                <div id="dvContent" class="bg_table">                
                        <table>
                            <tr>
                                <td colspan="2"><span id="errMsg"></span></td>
                            </tr>
                            <tr>
                                <td>Menu</td>
                                <td><input type="text" id="txtMenu" name="txtMenu" value="<%=Request["Name"] %>" /></td>
                            </tr>
                            <tr>
                                <td>Function Name</td>
                                <td><span id="spFuncGroup"></span></td>
                            </tr>
                            <tr>
                                <td>Url</td>
                                <td><input type="text" size="70" id="txtUrl" name="txtUrl" value="<%= strUrl %>" /></td>
                            </tr>
                            <tr id="trsystem">
                                <td>System</td>
                                <td>
                                    <select id="ddlSystem" name="ddlSystem">
                                       
                                    </select>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td>Target</td>
                                <td><input type="text" id="txtTarget" name="txtTarget" value="<%=Request["Target"] %>" /></td>
                            </tr>
                            <tr>
                                <td>IsHide</td>
                                <td>
                                    <input type="checkbox" id="chkIsHide" name="chkIsHide" <%=strChecked %> />
                                 </td>
                            </tr>
                            <tr>
                                <td>Order</td>
                                <td><input type="text" size="4" id="txtOrder" name="txtOrder" onkeypress="return CheckDecNumber(event);" value="<%=Request["Order"] %>" /></td>
                            </tr>
                             <tr>
                                <td colspan="2"> 
                                    <input class="btn" type="button" id="btnSubmit" name="btnSubmit" value="Submit" onclick="SubmitData();" />                                     
                                    <input type="hidden" id="parentId" name="parentId" value="<%= DNAide.Core.CastAide.AsString(Request["parentId"]) %>" />
                                    <input type="hidden" id="MenuId" name="MenuId" value="<%=DNAide.Core.CastAide.AsString(Request["MenuId"]) %>" />
                                    <input type="hidden" id="edit" name="edit" value="<%=DNAide.Core.CastAide.AsString(Request["edit"]) %>" />
                                    <input type="hidden" id="funcName" name="funcName" value="<%=DNAide.Core.CastAide.AsString(Request["funcName"]) %>" />                                    
                                    <input type="hidden" id="system" name="system" value="<%=Request["system"] %>" />
                                </td>
                            </tr>
                        </table>
                    </div>                
            </div>
        </div>
    </div>
    
    
</body>
</html>

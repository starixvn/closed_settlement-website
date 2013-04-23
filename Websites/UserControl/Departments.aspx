<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Departments.aspx.cs" Inherits="UserControl_Departments"
    Title="Untitled Page" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Public/Common.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/input.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/table.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Public/Message.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/users.css" rel="stylesheet" type="text/css" />
    <link href="../js/IBCWnd/IBCWnd.css" rel="stylesheet" type="text/css" />
    <link href="../js/Controls/StyledCheckbox/form[1].css" rel="stylesheet" type="text/css" />
    <link href="css/table.css" rel="stylesheet" type="text/css" />
    <link type="text/css" href="css/ui-lightness/jquery-ui-1.8.12.custom.css" rel="stylesheet" />
    <link href="../js/jQuery/Bubblepopup/css/jquery.bubblepopup.v2.3.1.css" rel="stylesheet"
        type="text/css" />
    <link type="text/css" href="css/jquery.msg.css" rel="stylesheet" />

    <script src="../js/Controls/StyledCheckbox/custom-form-elements.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.ui.core.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.ui.widget.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.ui.mouse.js"></script>
    <script type="text/javascript" src="../js/jQuery/jquery.ui.draggable.js"></script>
    <script type="text/javascript" src="../js/Customer.js"></script>
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/user.js"></script>
    <script type="text/javascript" src="../js/IBCWnd/IBCWnd.js"></script>
    <script src="../js/showhidepopup.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jQuery/UI/jquery-ui-1.8.10.custom.min.js"></script>
    <script type="text/javascript" src="../js/jQuery/Template/jquery.tmpl.js"></script>
    <script type="text/javascript" src="../js/jQuery/corner/jquery.corner.js"></script>
    <script type="text/javascript" src="JS/viewAllAccounts.js"></script>
    <script type="text/javascript" src="../js/jQuery/knockout/knockout-1.2.0.debug.js"></script>
    <script type="text/javascript" src="../js/dialogbox.js"></script>
    <script type="text/javascript" src="../js/jQuery/jlinq/jlinq.jquery.js"></script>
    <script src="../js/jQuery/Bubblepopup/jquery.bubblepopup.v2.3.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jQuery/message/jquery.center.min.js"></script>
    <script type="text/javascript" src="../js/jQuery/message/jquery.msg.js"></script>
    <script type="text/javascript" src="../js/jQuery/highlight/jquery.highlight-3.js"></script>
    <style type="text/css">
        .highlight { color:  #3399FF}
    </style>
</head>
<body>
    <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL">
                    </td>
                    <td class="maincontent_headerC">
                        Account Management
                    </td>
                    <td class="maincontent_headerR">
                    </td>
                </tr>
            </table>
            <div id="tabs" style="width: 98%; display: none; background: white none">
                <ul id="tabs_menu">
                    <li><a href="#tabs-1">View All Departments</a></li>
                    <li><a href="#tabs-2">Add Department</a></li>
                    <li><a href="#tabs-3">Add Account</a></li>
                    <li><a href="#tabs-4">Add Existed Account</a></li>
                    <li><a href="#tabs-5" onclick="javascript:accountManagement.viewAllAccount.SelectAccounts();">View All Accounts</a></li>
                    <li style="display: none"><a href="#tabs-6" onclick="javascript:accountManagement.utils.hideTab7();">
                        Role Policy</a></li>
                    <li style="display: none"><a href="#tabs-7" onclick="javascript:accountManagement.utils.hideTab6();">
                        Account Policy</a></li>
                </ul>
                <div id="tabs-1">
                    <iframe name="frUpd" style="display: none"></iframe>
                    <iframe name="frDelete" style="display: none"></iframe>
                    <div id="htmlGroupList" style="display: none; width: 620px;">
                        <div style="background-color: #F2F7FA; border: 1px solid #7C98A4; padding: 2px;"
                            searcharea="true">
                            <table style="border-collapse: collapse;" border="0" cellpadding="3">
                                <tbody>
                                    <tr>
                                        <td>
                                            <div>
                                                Keyword:
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <input type="text" style="height: 14px; width: 90px" keyword="true" value="All" />
                                            </div>
                                        </td>
                                        <td>
                                            <div>
                                                <input type="button" class="btn" value="Search" />
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div style="margin-top: 4px">
                            <table class="tbllayout2">
                                <thead>
                                    <tr class="title">
                                        <td>
                                            <div style="width: 80px" title="Sort by deparment name" class="canchange" sortable="true">
                                                Department
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 70px" title="Sort by no of user" class="canchange" sortable="true">
                                                No of User
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 300px; cursor: default" class="canchange">
                                                Description
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 80px; cursor: default" class="canchange">
                                                Role Policy
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 30px; cursor: default" class="canchange">
                                                Edit
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 41px; cursor: default" class="canchange">
                                                Delete
                                            </div>
                                        </td>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div style="text-align: center; border: 1px solid #7C98A4" data-bind="visible: dataTable.length <= 0">
                            No data
                        </div>
                        <div style="height: 600px; overflow-y: auto; overflow-x: hidden" id="divViewAllDepartmentsTemplate"
                            data-bind="visible: dataTable.length > 0">
                            <table class="tbllayout2">
                                <tbody data-bind="template:{name:'srciptViewAllDepartment', foreach: dataTable}">
                                </tbody>
                            </table>
                            <script id="srciptViewAllDepartment" type="text/html">
                                <tr>
                                    <td class="c">
                                        <div style="width: 80px; overflow: hidden" canchange="true" title="${department}">
                                            ${department}
                                        </div>
                                    </td>
                                    <td class="c">
                                        <div style="width: 70px">
                                            <span class="canchange" onclick="javascript:accountManagement.viewAllAccount.SelAccountByGroupName(${groupid},'${department}');">${nouser}</span>
                                        </div>
                                    </td>
                                    <td class="c">
                                        <div style="width: 300px; overflow: hidden" canchange="true" title="${desc}">
                                            ${desc}
                                        </div>
                                    </td>
                                    <td class="c" style="width: 75px">
                                        <div style="width: 80px">
                                            <span class="canchange" onclick="javascript:accountManagement.rolePolicy.SelGroupPolicy(${groupid});">Role Policy</span>
                                        </div>
                                    </td>
                                    <td class="c">
                                        <div style="width: 30px">
                                            <img style="cursor: pointer" onclick="javascript:accountManagement.viewAllDepartments.beginEditMode(event, ${groupid});" src="../App_Themes/Images/icn_edit.png"
                                                alt="Edit" />
                                        </div>
                                    </td>
                                    <td class="c">
                                        <div style="width: 41px" flagEndCol='endCol' oldwidth='41'>
                                            <img style="cursor: pointer" onclick="javascript:accountManagement.viewAllDepartments.DelDept(${groupid});" alt="Del" border="0"
                                                src="../App_Themes/Images/icon_del.gif" />
                                        </div>
                                    </td>
                                </tr>
                            </script>
                        </div>
                    </div>
                </div>
                <div id="tabs-2">
                    <form name="fAddDept" method="post" action="UpdateDept.ashx?type=add" target="frUpd">
                    <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                        <tr>
                            <td align="left" style="border: 1px solid #7C98A4; background-color: #B5C4D0; padding-left: 4px;
                                padding-top: 4px; padding-bottom: 4px;">
                                <div style="font-weight: bold; color: Black">
                                    Add Deparment
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <div style="width: 80px">
                                                    Department:
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 150px; padding-left: 20px">
                                                    <input type="text" onkeypress="return accountManagement.viewAllDepartments.OnEnter(event);" id="txtDept" name="txtDept"
                                                        style="height: 16px" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <div style="width: 80px">
                                                    Description:
                                                </div>
                                            </td>
                                            <td align="left">
                                                <div style="width: 150px; padding-left: 20px">
                                                    <input type="text" onkeypress="return accountManagement.viewAllDepartments.OnEnter(event);" id="txtDesc" name="txtDesc"
                                                        style="height: 16px" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px">
                                    <input type="button" style="width: 50px" onclick="javascript:accountManagement.viewAllDepartments.OnUpdate('add');" id="btnAddDept"
                                        name="btnAddDept" value="Add" class="btn" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div id="tabs-3">
                    <form name="fAddAcc" method="post" action="UpdateAccount.ashx?type=add" target="frUpd">
                    <table border="0" style="border-collapse: collapse">
                        <tr>
                            <td colspan="4" style="border: 1px solid #7C98A4; background-color: #B5C4D0;">
                                <div style="font-weight: bold; color: black; padding-left: 4px">
                                    Add New Account
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-right: 0 none"
                                align="right">
                                <div style="width: 80px; padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    User Name:
                                </div>
                            </td>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-left: 0 none;
                                border-right: 0 none">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    <input style="height: 16px" type="text" id="txtUName" name="txtUName" />
                                </div>
                            </td>
                            <td class="r" style="border: 1px solid #7C98A4; background-color: #EEECEE; border-right: 0 none;
                                border-left: 0 none" align="right">
                                <div style="width: 80px; padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    Password:
                                </div>
                            </td>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-left: 0 none">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px; padding-right: 4px">
                                    <input style="height: 16px" type="password" id="txtPwd" name="txtPwd" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-right: 0 none"
                                align="right">
                                <div style="width: 80px; padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    First Name:
                                </div>
                            </td>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-left: 0 none;
                                border-right: 0 none">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    <input style="height: 16px" type="text" id="txtFName" name="txtFName" />
                                </div>
                            </td>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-right: 0 none;
                                border-left: 0 none" align="right">
                                <div style="width: 80px; padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    Last Name:
                                </div>
                            </td>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-left: 0 none">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px; padding-right: 4px">
                                    <input style="height: 16px" type="text" id="txtLName" name="txtLName" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-right: 0 none"
                                align="right">
                                <div style="width: 80px; padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    Department:
                                </div>
                            </td>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE; border-left: 0 none;
                                border-right: 0 none">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    <select style="height: 20px; width: 112px" id="selGroup" name="selGroup">
                                        <asp:Literal ID="literalDepartmentList" runat="server"></asp:Literal>
                                    </select>
                                </div>
                            </td>
                            <td colspan="2" style="border: 1px solid #7C98A4; background-color: #EEECEE; border-left: 0 none">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="border: 1px solid #7C98A4; background-color: #EEECEE;">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    <input type="button" style="width: 50px" onclick="javascript:accountManagement.viewAllDepartments.OnUpdate('acc');" id="btnAddAcc"
                                        name="btnAddAcc" class="btn" value="Add" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div id="tabs-4">
                    <form name="fAddExistAcc" method="post" action="UpdateAccount.ashx?type=exist" target="frUpd">
                    <table border="1" cellpadding="0" cellspacing="0" style="border-collapse: collapse">
                        <tr>
                            <td align="left" style="border: 1px solid #7C98A4; background-color: #B5C4D0; padding-left: 4px;
                                padding-top: 4px; padding-bottom: 4px;">
                                <div style="font-weight: bold; color: Black">
                                    Add Existed Account
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <div style="width: 80px">
                                                    <span>User Name:</span>
                                                </div>
                                            </td>
                                            <td align="left">
                                                <div style="width: 150px; padding-left: 20px">
                                                    <input type="text" name="txtEUName" id="txtEUName" style="height: 16px;" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="right">
                                                <div style="width: 80px">
                                                    <span style="floatl: left">Department:</span>
                                                </div>
                                            </td>
                                            <td align="left">
                                                <div style="width: 150px; padding-left: 20px">
                                                    <select style="float: left; height: 20px; width: 112px" id="selEGroup" name="selEGroup">
                                                        <asp:Literal ID="literalGroupList" runat="server"></asp:Literal>
                                                    </select>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid #7C98A4; background-color: #EEECEE">
                                <div style="padding-left: 4px; padding-top: 4px; padding-bottom: 4px;">
                                    <input type="button" style="width: 50px; font-weight: bold" onclick="javascript:accountManagement.viewAllDepartments.OnUpdate('exist');"
                                        align="middle" id="btnAddExist" name="btnAddExist" value="Add" class="btn" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    </form>
                </div>
                <div id="tabs-5">
                    <form name="fAdminList">
                    <div style="width: 1210px">
                        <div style="background-color: #F2F7FA; border: 1px solid #7C98A4;
                            padding: 2px">
                            <table border="0" style="border-collapse: collapse">
                                <tr>
                                    <td>
                                        <div style="width: 70px">
                                            <font class="b">Keyword:</font>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="width: 85px">
                                            <input type="text" name="txtSearch" id="txtSearch" value="All" style="width: 80px;
                                                height: 14px; vertical-align: baseline;" onkeypress="javascript:return event.keyCode == 13 ? false : true;" />
                                        </div>
                                    </td>
                                    <td>
                                        <div style="width: 85px">
                                            <font class="b">First Letter:</font>
                                        </div>
                                    </td>
                                    <td>
                                        <div style="width: 70px">
                                            <select style="width: 60px; height: 18px" name="listAlpha" id="listAlpha">
                                            </select>
                                        </div>
                                    </td>
                                    <td class="r">
                                        <div style="width: 50px">
                                            <input class="btn" type="button" name="btnGo" id="btnGo" value="Search" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divViewAllAccount" style="display: none; margin-top: 4px">
                            <div>
                                <table class="tbllayout2">
                                    <thead>
                                        <tr class="title">
                                            <td>
                                                <div style="width: 36px; cursor: pointer" class="canchange" sortable="true">
                                                    No
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 150px; cursor: pointer" sortable="true" title="Sort by User name"
                                                    class="canchange">
                                                    User Name
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 200px; cursor: default" class="canchange">
                                                    First Name
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 200px; cursor: default" class="canchange">
                                                    Last Name
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 78px; cursor: default" class="canchange">
                                                    Status
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 79px; cursor: pointer" sortable="true" title="Sort by creator"
                                                    class="canchange">
                                                    Creator
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 150px; cursor: pointer" sortable="true" title="Sort by created date"
                                                    class="canchange">
                                                    Created Date
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 100px; cursor: default" class="canchange">
                                                    Policy
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 90px; cursor: pointer" sortable="true" title="Sort by Department"
                                                    class="canchange">
                                                    Department
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 57px; cursor: default" class="canchange">
                                                    Reset
                                                </div>
                                            </td>
                                            <td>
                                                <div style="width: 36px; cursor: default" class="canchange">
                                                    Edit
                                                </div>
                                            </td>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                            <div style="text-align: center; border: 1px solid #7C98A4" data-bind="visible: dataTable.length <= 0">
                                No data
                            </div>
                            <div style="height: 600px; overflow-y: auto; overflow-x: hidden" id="divViewAllAccountsTemplate" data-bind="visible: dataTable.length > 0">
                                <table class="tbllayout2">
                                    <tbody data-bind="template: {name: 'scriptViewAllAccounts', foreach: dataTable}">
                                    </tbody>
                                </table>
                            </div>
                            <script id="scriptViewAllAccounts" type="text/html">
                                    <tr id="r_${no}" class="${style}">
                                        <td>
                                            <div style="width:36px">
                                                ${no}
                                            </div>
                                        </td>
                                        <td>
                                            <div class="canchange" title="${user}" 
                                                onclick="javascript:accountManagement.viewAllAccount.beginEditMode(event, ${custid});" 
                                                style="width:150px; overflow: hidden">
                                                ${user}
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width:200px; overflow: hidden" title="${fname}" canchange="true">
                                                ${fname}
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width:200px; overflow: hidden" title="${lname}" canchange="true">
                                                ${lname}
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width:78px">
                                                <table cellpadding="0" cellspacing="0" border="0" style="border:0px none" width="68">
                                                    <tr>
                                                        <td style="border:0px none">
                                                            <div>${strstatus}</div>
                                                        </td>
                                                        <td style="border:0px none">
                                                            <div>
                                                                <input type='checkbox' id='${custid}_closed' name='${custid}_closed' ${status} onclick='javascript:accountManagement.viewAllAccount.updateStatus(${custid},${groupid}, "${dept}")' class='styled' />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            <div>
                                        </td>
                                        <td>
                                            <div style="width:79px">
                                                ${creator}
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width:150px">
                                                ${createddate}
                                            </div>
                                        </td>  
                                        <td>
                                            <div class="canchange"  onclick="javascript:accountManagement.accountPolicy.SelAccountPolicy(${custid},'${dept}',${groupid},'${user}')" style="width:100px">
                                                ${policy}
                                            </div>
                                        </td>
                                        <td>
                                            <div class="canchange"  onclick="javascript:accountManagement.viewAllAccount.SelAccountByGroupName(${groupid},'${dept}');" style="width:90px" canchange="true">
                                                ${dept}
                                            </div>
                                        </td>
                                        <td>
                                            <div class="canchange"  onclick="javascript:OpenChangePwd(${custid},'${user}')" style="width:57px">
                                                ${pass}
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width:36px" flagEndCol='endCol' oldwidth='36'>
                                                <img alt="Edit" 
                                                onclick="javascript:accountManagement.viewAllAccount.beginEditMode(event, ${custid});" 
                                                src="../App_Themes/Images/icn_edit.png" title="Edit account" style="cursor:pointer"
                                                imgedit="true"
                                                 />
                                            </div>
                                        </td>
                                    </tr>
                            </script>
                        </div>
                    </div>
                    </form>
                </div>
                <div id="tabs-6">
                    <div id="divRolePolicy" data-bind="template:{name:'scriptRolePolicyTemplate'}" style="width: 510px; background-color:White">
                    </div>
                    <script type="text/html" id="scriptRolePolicyTemplate">
                        <div>
                            <div align="left" style="padding:4px;border-right:1px solid #7C98A4;border-left:1px solid #7C98A4;border-top:1px solid #7C98A4">
                                <span><b>Department:</b><span class="canchange" style="font-weight: bold; cursor: pointer; margin-left:4px"
                                    onclick="javascript:accountManagement.viewAllAccount.SelAccountByGroupName('${groupId}', '${groupName}');accountManagement.rolePolicy.cancelUpdate();">${groupName} </span>
                                </span>
                            </div>
                            <form target="frUpd" action="UpdatePolicy.ashx" method="post" name="fUpdatePolicy">
                            <input type="hidden" name="arrFuncId" id="arrFuncId" value="" />
                            <input type="hidden" name="groupId" id="groupId" value="${groupId}" />
                            <input type="hidden" name="dept" id="dept" value="${groupName}" />
                            <div>
                                <table class="tbllayout2">
                                    <tr class="title">
                                        <td style="white-space: nowrap" rowspan="2">
                                            <div style="width: 300px">
                                                Function
                                            </div>
                                        </td>
                                        <td colspan="2">
                                            <div style="width: 200px">
                                                Group Permission
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="title">
                                        <td>
                                            <div style="width: 100px">
                                                Read
                                            </div>
                                        </td>
                                        <td>
                                            <div style="width: 100px">
                                                Host
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div style="border-right:1px solid #7C98A4">
                                <div style="overflow-y: auto;overflow-x: hidden; height:550px" id="divRolePolicyData">
                                    <table class="tbllayout2">
                                        <tbody id="tbodyRolePolicyData">
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <div style="background-color:#B5C4D0">
                                <table class="tbllayout2">
                                    <tbody>
                                        <tr>
                                            <td colspan="3">
                                                <div style="width:506px; text-align:center">
                                                    <input type="button" class="btn" value="Update" name="btnUpdate" onclick="javascript:accountManagement.rolePolicy.onSubmit();accountManagement.rolePolicy.cancelUpdate();" />
                                                    <input type="button" class="btn" value="Cancel" name="btnCancel" onclick="javascript:accountManagement.rolePolicy.cancelUpdate();" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            </form>
                        </div>
                    </script>
                </div>
                <div id="tabs-7">
                    <div id="divAccountPolicy" data-bind="template:{name: 'scriptAccountPolicyTemplate'}" style="width: 706px; background-color:White">
                    </div>
                    <script type="text/html" id="scriptAccountPolicyTemplate">
                        <div>
                            <div align="left" style="padding:4px;border-right:1px solid #7C98A4;border-left:1px solid #7C98A4;border-top:1px solid #7C98A4">
                                <span><b>Department:</b><span class="canchange" style="font-weight: bold; cursor: pointer"
                                    onclick="javascript:accountManagement.viewAllAccount.SelAccountByGroupName('${groupId}','${groupName}', true);accountManagement.accountPolicy.cancelUpdate();"> ${groupName}</span>
                                    <b style="margin-left:3px">
                                        <span style="margin-left:4px">>></span>
                                        <span>Account:</span>
                                    </b>
                                    <span style="margin-left:3px">${userName}</span>
                                </span>
                            </div>
                            <div>
                                <form target="frUpd" action="UpdatePolicy.ashx" method="post" name="fUpdatePolicy">
                                <input type="hidden" name="arrFuncId" id="arrFuncId" value="" />
                                <input type="hidden" name="groupId" id="groupId" value="${groupId}" />
                                <input type="hidden" name="custId" id="custId" value="${custId}" />
                                <input type="hidden" name="user" id="user" value="${userName}" />
                                <input type="hidden" name="dept" id="dept" value="${groupName}" />
                                <div>
                                    <table class="tbllayout2">
                                        <thead>
                                            <tr class="title">
                                                <td rowspan="2">
                                                    <div style="width: 300px;">
                                                        Function
                                                    </div>
                                                </td>
                                                <td colspan="2">
                                                    <div>
                                                        Individual Permission
                                                    </div>
                                                </td>
                                                <td colspan="2">
                                                    <div>
                                                        Group Permission
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr class="title">
                                                <td>
                                                    <div style="width: 100px;">
                                                        Read
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="width: 100px;">
                                                        Host
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="width: 100px;">
                                                        Read
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="width: 90px;">
                                                        Host
                                                    </div>
                                                </td>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                                <div style="border-right:1px solid #7C98A4">
                                    <div style="height: 550px; overlow-y: auto; overflow-x: hidden">
                                        <div style="overflow-y:auto; height:550px; overflow-x:hidden">
                                        <table class="tbllayout2">
                                            <tbody id="tbodyAccountPolicyData">
                                            </tbody>
                                        </table>
                                        </div>
                                    </div>
                                <div>
                                <div style="background-color:#B5C4D0; border:1px solid #7C98A4; border-right:0px none">
                                    <table>
                                        <tr>
                                            <td>
                                                <div style="width: 700px; text-align: center">
                                                    <input type="button" class="btn" value="Update" id="btnUpdate" name="btnUpdate" onclick="javascript:accountManagement.accountPolicy.onSubmit();accountManagement.accountPolicy.cancelUpdate();" />
                                                    <input type="button" class="btn" value="Cancel" id="btnCancel" name="btnCancel" onclick="javascript:accountManagement.accountPolicy.cancelUpdate();" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                </form>
                            </div>
                        </div>                        
                    </script>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(
            function () {
                accountManagement.init();
            }
        );
    </script>
    <div id="errfunc">
    </div>
    <div style="display: none">
        <select id="selectDepartmentListTemp">
            <asp:Literal ID="literalDepartmentListTemp" runat="server"></asp:Literal>
        </select>
    </div>
</body>
</html>

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageMenu.aspx.cs" Inherits="UserControl_ManageMenu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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

    <script src="../js/showhidepopup.js" type="text/javascript"></script>

    <script src="JS/ManageMenu.js" type="text/javascript"></script>
 
</head>
<body>
    <!--Template Area-->
    <!--System Option-->
   <script id="systemTmpl" type="text/x-jquery-tmpl">
    <option value="${Name}">${Name}</option>
    </script> 
    <!--/System Option-->
    <!--End Template-->

    <script type="text/javascript">
        window.onload = function () {
            GetSystemList();
            GetMenuList();
        }

    </script>

   <div class="main">
        <div class="maincontent">
            <table border="0" cellpadding="0px" cellspacing="0px" class="maincontent_header">
                <tr>
                    <td class="maincontent_headerL"></td>
                    <td class="maincontent_headerC">Manage Menu</td>
                    <td class="maincontent_headerR"></td>
                </tr>
            </table>
            <div class="maincontent_box">
                Search by 
                <select id="ddlSystem" name="ddlSystem">
                   
                </select>
                <input type="button" class="btn" value="Submit" onclick="GetMenuList();" />
             
            </div>
            <div>    
                <div id="dvContent" class="bg_table">                
                    <span id="errMsg"></span>
                    <table id="table" class="tbllayout" cellspacing="1px" cellpadding="1px"  style="width:1000px">
                    <thead>
                        <tr class="title" id="trHeader">
                            <th>Menu</th>
                            <th style="width:50px;"><span class="canchange" onclick="ShowPopupAddMenu();">Insert</span></th>      
                            <th style="width:50px;">Edit</th>      
                            <th style="width:50px;">Delete</th>  
                            <th>Function Name</th>
                            <th>Url</th>
                            <th>System</th>
                            <th style="width:80px;">Target</th>
                            <th style="width:50px;">IsHide</th>            
                            <th style="width:50px;">Order</th> 
                            <th style="width:50px;">Level</th>                            
                               
                        </tr>
                    </thead>
                    <tbody id="tblcontent">
                    
                     
                    </tbody>
                </table>    
                </div>
            </div>
        </div>
    </div>
</body>
</html>

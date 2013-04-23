<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CalculationHR.aspx.cs" Inherits="Settle_CalculationHR" %>
<%@ Import Namespace="Sunplus.Accounting.Core.Common" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Calculation HR</title>
    <script src="../js/Settlement.js" type="text/javascript"></script>

    <script src="../js/prototype.js" type="text/javascript"></script>
</head>
<body>
    <script type="text/javascript" src="../js/prototype.js"></script> 
    <script type="text/javascript" src="../js/common.js?vers=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="../js/Calculation.js?vers=<%=AccCommon.Version() %>"></script>
    <script type="text/javascript" src="../js/Window/dhtmlwindow/dhtmlwindow.js?vers=<%=AccCommon.Version() %>"></script>
     
    <table width="100%" height="25" border="0">           
        <tr>
            <td class="calign fontbold">SPORT:&nbsp;<%Response.Write(sportName); %></td>
        </tr>
    </table> 
    <div id="_sContent" >
        <asp:Literal id="sContent" runat="server" />
    </div>  
   
</body>
</html>

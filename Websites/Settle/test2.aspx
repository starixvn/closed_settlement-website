<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/SteelBlue/AccountMenu.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/CustInfo.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/Customer.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/Homepage.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/LogIn.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/pagination.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/report.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/report2a-old.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/SteelBlue/reportV2.css" rel="stylesheet" type="text/css" />
    <script>
        var currentHNumber="";
        var currentHName="";
        function setCurrentHNumber(obj)
        {
            currentHNumber=obj[obj.selectedIndex].text;
            currentHName=obj[obj.selectedIndex].value;
        }
        function changePosition(obj, total)
        {
            //alert(currentHNumber);
            for (var i=1; i<=total; i++)
            {
                if (i!=parseInt(obj.id.substring(obj.id.length-1)) && document.getElementById('sHName_' + i)[document.getElementById('sHName_' + i).selectedIndex].text==obj[obj.selectedIndex].text)
                {
                    var curr;
                    for (var j=0;j<document.getElementById('sHName_' + i).length;j++)
                    {
                    
                        if (document.getElementById('sHName_' + i)[j].text==currentHNumber)
                        {
                            curr=j;
                        }
                    }
                    document.getElementById('sHName_' + i).selectedIndex=curr;
                    document.getElementById(obj.id.replace(/sHName/,"HName")).innerHTML=obj[obj.selectedIndex].value;
                    document.getElementById('HName_' + i).innerHTML=currentHName;

                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <table class="tbllayout" border="1">
            <tbody>
                 <tr>
                    <td colspan="6" class="title2">
                        *..::¤&nbsp;EXETER(UK-EX)&nbsp;¤::..*
                    </td>
                </tr>
                 <tr class="c ReportTitle">
                    <td colspan="2" style="text-transform: uppercase;">
                        Race 1 - Running
                    </td>
                    <td colspan="4" >
                        EVENT 02/26/2009
                    </td>
                </tr>
                <tr class="c report-header">
                    <th width="60">
                        Position
                    </th>
                    <th width="200">
                        Horse
                    </th>
                    <th width="60">
                        SCR
                    </th>
                    <th width="60" >
                        TF
                    </th>
                    <th width="60">
                        Win
                    </th>
                    <th width="60">
                        Place
                    </th>
                </tr>
                <tr class="rowEven">
                    <td class="c">1st</td>
                    <td >
                        <select id="sHName_1" onclick="setCurrentHNumber(this)" onchange="changePosition(this, 6);" style=" width: 40px;">
                            <option value="Horse Name A" selected="selected" >1</option>
                            <option value="Horse Name B" >2</option>
                            <option value="Horse Name C" >3</option>
                            <option value="Horse Name D">4</option>
                            <option value="Horse Name E">5</option>
                            <option value="Horse Name F">6</option>
                        </select> 
                        <span id="HName_1">Horse Name A</span>
                        </td>
                    <td class="c">
                        <!--<input id="Checkbox2" type="checkbox" style=" width: 50px;" />-->
                    </td>
                    <td class="c">
                        <input id="Checkbox1" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <input id="Text2" type="text" style=" width: 50px;" /></td>
                    <td class="c">
                        <input id="Text1" type="text" style=" width: 50px;" /></td>
                </tr>
                 <tr class="rowEven">
                    <td class="c">2nd</td>
                    <td >
                        <select id="sHName_2" onclick="setCurrentHNumber(this)" onchange="changePosition(this, 6);" style=" width: 40px;">
                            <option value="Horse Name A" >1</option>
                            <option value="Horse Name B" selected="selected" >2</option>
                            <option value="Horse Name C">3</option>
                            <option value="Horse Name D">4</option>
                            <option value="Horse Name E">5</option>
                            <option value="Horse Name F">6</option>
                        </select> 
                        <span id="HName_2">Horse Name B</span>
                        </td>
                    <td >
                        <!--<input id="Checkbox3" type="checkbox" style=" width: 50px;" />-->
                    </td>
                    <td class="c">
                        <input id="Checkbox4" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <!--<input id="Text3" type="text" style=" width: 50px;" />--></td>
                    <td class="c">
                        <input id="Text4" type="text" style=" width: 50px;" /></td>
                </tr>
                 <tr class="rowEven">
                    <td class="c">3rd</td>
                    <td >
                        <select id="sHName_3" onclick="setCurrentHNumber(this)" onchange="changePosition(this, 6);" style=" width: 40px;">
                            <option value="Horse Name A" >1</option>
                            <option value="Horse Name B" >2</option>
                            <option value="Horse Name C" selected="selected">3</option>
                            <option value="Horse Name D">4</option>
                            <option value="Horse Name E">5</option>
                            <option value="Horse Name F">6</option>
                        </select> 
                        <span id="HName_3">Horse Name C</span>
                        </td>
                    <td class="c">
                        <!--<input id="Checkbox5" type="checkbox" style=" width: 50px;" />-->
                    </td>
                    <td class="c">
                        <input id="Checkbox6" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <!--<input id="Text5" type="text" style=" width: 50px;" /></td>-->
                    <td class="c">
                        <input id="Text6" type="text" style=" width: 50px;" /></td>
                </tr>
                 <tr>
                    <td class="c">4th</td>
                    <td >
                       <select id="sHName_4" onclick="setCurrentHNumber(this)" onchange="changePosition(this, 6);" style=" width: 40px;">
                            <option value="Horse Name A" >1</option>
                            <option value="Horse Name B" >2</option>
                            <option value="Horse Name C" >3</option>
                            <option value="Horse Name D" selected="selected">4</option>
                            <option value="Horse Name E">5</option>
                            <option value="Horse Name F">6</option>
                        </select> 
                        <span id="HName_4">Horse Name D</span>
                        </td>
                    <td class="c">
                        <!--<input id="Checkbox5" type="checkbox" style=" width: 50px;" />-->
                    </td>
                    <td class="c">
                        <input id="Checkbox7" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <!--<input id="Text11" type="text" style=" width: 50px;" /></td>-->
                    <td class="c">
                        <!--<input id="Text12" type="text" style=" width: 50px;" /></td>-->
                </tr>
                 <tr>
                    <td class="c"></td>
                    <td >
                        <select id="sHName_5" onclick="setCurrentHNumber(this)" onchange="changePosition(this, 6);" style=" width: 40px;">
                            <option value="Horse Name A" >1</option>
                            <option value="Horse Name B" >2</option>
                            <option value="Horse Name C" >3</option>
                            <option value="Horse Name D" >4</option>
                            <option value="Horse Name E" selected="selected">5</option>
                            <option value="Horse Name F">6</option>
                        </select> 
                        <span id="HName_5">Horse Name E</span>
                        </td>
                    <td class="c">
                        <input id="Checkbox2" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <input id="Checkbox3" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <!--<input id="Text11" type="text" style=" width: 50px;" /></td>-->
                    <td class="c">
                        <!--<input id="Text12" type="text" style=" width: 50px;" /></td>-->
                </tr>
                 <tr>
                    <td class="c"></td>
                    <td >
                         <select id="sHName_6" onclick="setCurrentHNumber(this)" onchange="changePosition(this, 6);" style=" width: 40px;">
                            <option value="Horse Name A" >1</option>
                            <option value="Horse Name B" >2</option>
                            <option value="Horse Name C" >3</option>
                            <option value="Horse Name D" >4</option>
                            <option value="Horse Name E" >5</option>
                            <option value="Horse Name F" selected="selected">6</option>
                        </select> 
                        <span id="HName_6">Horse Name F</span>
                        </td>
                    <td class="c">
                        <input id="Checkbox11" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <input id="Checkbox12" type="checkbox" style=" width: 50px;" />
                    </td>
                    <td class="c">
                        <!--<input id="Text11" type="text" style=" width: 50px;" /></td>-->
                    <td class="c">
                        <!--<input id="Text12" type="text" style=" width: 50px;" /></td>-->
                </tr>
            </tbody>
        </table>
        <br />
        <table class="tbllayout" border="1" width="518px" >
            <tr>
                <td>
                    <input id="Button1" type="button" value="Caclulate" style=" width:100px;" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>

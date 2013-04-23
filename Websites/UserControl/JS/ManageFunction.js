



function GetFunctionList() {
    functionList = null;
    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: "UserControlService.svc/GetFunctionList",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function(rs) {
            $('#tblcontent').html('');
            var title="";
            title+='<tr class="title">';
            title+='<td style="width:100px; white-space: nowrap;" >Func Name</td>';
            title+='<td >Func Desc</td>';
            title += '<td style="width:400px; word-wrap: break-word;ms-word-wrap:break-word;word-break:break-all;">File Names</td>';
            title+='<td style="width:50px;">Sub-Func</td>';
	        title+='<td style="width:80px;">System</td>';
            title+='<td style="width:35px;">Edit</td>';
            title+='<td style="width:35px;">Del</td>';
            title+='</tr>';
            $('#tblcontent').append(title);
            WriteData(rs, 0);
           
        }
    });
}

function WriteData(listFunction, level) {
    for (var i = 0; i < listFunction.length; i++) {
        var strRow="";
        strRow="<tr>";
        
        strRow+="<td class='l indent" + level + "'><span>";
        strRow+=listFunction[i].Name;
        strRow+="</span></td>";
        
        strRow+="<td class='l'>";
        strRow+=listFunction[i].Desc;
        strRow+="</td>";

        strRow += "<td class='l' style='width:400px; word-wrap: break-word;ms-word-wrap:break-word;word-break:break-all;'>";
        strRow+=listFunction[i].Files;
        strRow += "</td>";

        strRow += "<td >";
        strRow += listFunction[i].TotalSub;
        strRow += "</td>";
        
        strRow+="<td >";
        strRow+=listFunction[i].System;
        strRow+="</td>";
        
        strRow+="<td >";
        strRow += '<img class="hand" src="../App_Themes/Images/icn_edit.png" onclick="OpenEditDept(\'' + listFunction[i].ID + '\',\'' + listFunction[i].Name + '\',\'' + listFunction[i].Desc + '\',\'' + listFunction[i].Files + '\',\'' + listFunction[i].ParentId + '\',\'' + listFunction[i].System + '\')" />';
        strRow+="</td>";
        
        strRow+="<td >";
        strRow+="<img class='hand' src='../App_Themes/Images/icn_del.png' onclick='DelFunction(" +listFunction[i].ID + ")' />";
        strRow+="</td>";
        
        strRow+="</tr>";
        
        $("#tblcontent").append(strRow);
        WriteData(listFunction[i].SubFuncs, level+1);
    }
     //add event for rows data only, ignore title row
            $("#tblcontent tr:not(:.title)").mouseover(function() {
                rowover(this)
            });
            $("#tblcontent tr:not(:.title)").mouseout(function()
            { rowout() });
     try { parent.jQuery.unblockUI(); } catch (e) { }
 }



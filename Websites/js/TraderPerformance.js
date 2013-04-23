function ExportToExcel()
{
    //obj('export').click();
    location.href = 'TraderPerformance.aspx?fdate='+ obj('fdate').value + '&tdate=' + obj('tdate').value + '&IsSoccer=' + obj('IsSoccer').value + '&IsExport=1';
}

function DoPostBack()
{
    //location.href = 'TraderPerformance.aspx?fdate='+ obj('fdate').value + '&tdate=' + obj('tdate').value + '&IsSoccer=' + obj('IsSoccer').value;
    GetDataReport();
}

function LoadTraderDetail()
{
    window.open('TraderPerformanceDetail.aspx?fdate='+ obj('fdate').value + '&tdate=' + obj('tdate').value + '&IsSoccer=' + obj('IsSoccer').value,'TraderPerformanceDetail');
}

function Sort(o)
{    
    var sortCol = obj('sortCol');
    var order = obj('order');        
    if(sortCol.value == o.id)// sort revert
    {
        order.value = (order.value=="desc")?"asc":"desc";
    }
    else // new sorting
    {
        sortCol.value = o.id;
        order.value = "asc";
    }
    //location.href='TraderPerformance.aspx?fdate='+ obj('fdate').value + '&tdate=' + obj('tdate').value + '&IsSoccer=' + obj('IsSoccer').value + '&sortCol=' + obj('sortCol').value + '&order=' + obj('order').value;
    GetDataReport();
}

function GetDataReport()
{    
    try{parent.BlockUI();}catch(e){}
    $.ajax({
    url: 'GetData.ashx?fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value +  '&IsSoccer=' + obj('IsSoccer').value  +   '&sortCol=' + obj('sortCol').value + '&order=' + obj('order').value  +  '&mode=20',
    dataType:'html',
    success: function(data)
    {
        
        obj('sData').innerHTML = data;
        var tbl=$("#btable");
//             if (tbl.height()<600)
//             {
//                $("#bdiv").height(tbl.height() + "px");
//                $("#bdiv").width("6230px");
//                $("#gbox_JQGrid1").width("6230px");
//                $("#divtitle").width("6230px");
//                
//                $(".ui-jqgrid-sdiv").css("margin-top","-1px");
//             }
        parent.jQuery.unblockUI();
        LoadSortCol();
    }
    });   
}


function SwitchViewTrader(object)
{
    var soccer = obj('soccer');
    var other = obj('other');
    var IsSoccer = obj('IsSoccer');


    IsSoccer.value = (object.id == 'soccer') ? '1' : '0';    
    
    if(object.id == 'soccer')
    {
        soccer.className = '';        
        soccer.onlick = '';
        
        other.className = 'lnk';
        other.onlick = 'SwitchViewTrader(this);';
    }
    else
    {
        soccer.className = 'lnk';
        soccer.onlick = 'SwitchViewTrader(this);';
        
        other.className = '';
        other.onlick = '';
    }

    DoPostBack();
}




function Max(a,b)
{
    if(a>b)
        return a;
    return b;    
}

function LoadSortCol()
{
    var sortCol = obj('sortCol');
    if(sortCol.value + "" != "")
    {
        var order = obj('order');
        var colId = obj(sortCol.value);
        if (colId)
            colId.innerHTML += order.value=="desc"?imgAsc:imgDesc;
    }  
}

    
    
   
    
    

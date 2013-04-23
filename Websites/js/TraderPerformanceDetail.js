function ExportToExcel()
{
   // obj('export').click();
   location.href = 'TraderPerformanceDetail.aspx?fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value +  '&IsSoccer=' + obj('IsSoccer').value + '&IsExport=1';
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
    obj('sort').click();
}


//fixed header
var headerMain, clientDocument, container, tblRpt,rowCnt
var isScroll = false;
function GetHeight()
{
    var boxHeaderHeight=0,height=0;
    if(headerMain){    
       boxHeaderHeight+=headerMain.offsetHeight;
    } 
    
    if(clientDocument)
    {
        if (isIE())
            height = clientDocument.clientHeight-boxHeaderHeight;
        else
            height = window.innerHeight - boxHeaderHeight;   
    }
   
    if (isIE())
            height = Max(100,height - 80);
        else
            height = Max(100,height - 100);
   
    return height;
}
function GetWidth()
{
    var width=Max(1000,tblRpt.offsetWidth);
    tblRpt.style.width = width;
    return width + 30;
}    
function Init()
{
    tblRpt=obj("tbl-rpt");
    headerMain=obj("header_main");
    rowCnt = obj("RowCnt");
    if(document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight))
         clientDocument=document.documentElement;
    else
        clientDocument=document.body;
    
    
     var height = 0;
    var width = 0;
    height = GetHeight();
    width = GetWidth();
    
    var topHeader = getRealTop('trHeader');
    var topFooter = getRealTop('trFooter');           
    var innerHeight = window.innerHeight;
    var delta = 10;
    if (!innerHeight) {
         innerHeight = clientDocument.clientHeight;
     }
    var RealHeight = innerHeight - topHeader - delta;
    
    if (rowCnt)
    {
        if (rowCnt.value>=20)
        {
            if (topFooter > innerHeight) {
                jQuery('#tbl-rpt').Scrollable(RealHeight, width);
                isScroll = true;
            }        
        }
    }
    //when finishing creating scrollable table then div_container exists
    container = obj("div_container")
}
function Resize()
{
    if (isScroll)
    {
         
        var innerHeight = window.innerHeight;
        
        if (!innerHeight) {
             innerHeight = clientDocument.clientHeight;
         }
        var height = GetHeight();
        
        if (!isIE())
        {
            if (tblRpt)
            {
                tblRpt.style.height = (height - 90) + 'px';
            }
        }
        if (container)
        {
            if (tblRpt.offsetHeight < innerHeight)
                container.style.height = tblRpt.offsetHeight + 'px';
            else
                container.style.height = height;
        }  
    }
}
function Max(a,b)
{
    if(a>b)
        return a;
    return b;    
}
function GetDataReport()
{
    BeginLoad();
    $.ajax({
    url: 'GetData.ashx?fdate=' + obj('fdate').value + '&tdate=' + obj('tdate').value +  '&IsSoccer=' + obj('IsSoccer').value  +   '&sortCol=' + obj('sortCol').value + '&order=' + obj('order').value  +  '&mode=21',
    dataType:'html',
    success: function(data)
    {
        
        obj('sData').innerHTML = data;
        FinishLoad();
        Init();
        LoadSortCol();
    }
    });   
}

function BeginLoad()
{
    obj('inProgressBar').style.display = 'inline'
    if (obj('btnReport')) {
        obj('btnReport').disabled = true;
        obj('btnExport').disabled = true;
    }
}

function FinishLoad()
{
    obj('inProgressBar').style.display='none';
    if (obj('btnReport')) {
        obj('btnReport').disabled = false;
        obj('btnExport').disabled = false;
        
    }
}

function getRealTop(el) {
    try {
        if (!$(el)) return 0;
        var yPos = obj(el).offsetTop;
        var tempEl = obj(el).offsetParent;
        while (tempEl != null) {
            yPos += tempEl.offsetTop;
            tempEl = tempEl.offsetParent;
        }
    }
    catch (e)
                { }

    return yPos;
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
function DoPostBack()
{
    //location.href = 'TraderPerformanceDetail.aspx?fdate='+ obj('fdate').value + '&tdate=' + obj('tdate').value + '&IsSoccer=' + obj('IsSoccer').value;
    GetDataReport();
}

window.onresize = Resize;
//end fixed header


//image sorting
var imgAsc = '&nbsp;<img src="../App_Themes/SteelBlue/Images/asc.gif" />';
var imgDesc = '&nbsp;<img src="../App_Themes/SteelBlue/Images/desc.gif" />';

if (isIE())
    document.write("<link rel='Stylesheet' type ='text/css' href ='../App_Themes/SteelBlue/IEFixedHeader.css' />");
else
    document.write("<link rel='Stylesheet' type ='text/css' href ='../App_Themes/SteelBlue/NonIEFixedHeader.css' />");

 
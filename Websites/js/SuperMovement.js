var headerMain, clientDocument, container, tblRpt,rowCnt
function ExportToExcel()
{
    var bt = GetBalanceType();
    url = "SuperMovement.aspx?balancetype=" + bt + "&isexport=1";
    document.location.href = url;
}
function DoPostBack()
{
   GetDataReport();
}

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
            height = Max(100,height - 90);
   
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
//    jQuery('#mainTbl').Scrollable(609, 900);

    tblRpt=obj("mainTbl");
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
    
    if (isIE()==false)
    {
        if (rowCnt)
        {
            if (rowCnt.value>=20)
            {
                 jQuery('#mainTbl').Scrollable(height, width);
            }
        }
	}            
	else
	    jQuery('#mainTbl').Scrollable(height, width);
    
    //when finishing creating scrollable table then div_container exists
    container = obj("div_container")
}
function Resize()
{
    var height = GetHeight();
    if (container)
        container.style.height = height + 'px';
    if (!isIE())
    {
        if (tblRpt)
        {
            if (rowCnt)
            {
                if (rowCnt.value>=20)
                tblRpt.style.height = (height - 90) + 'px';
            }
        }
    }
}
function Max(a,b)
{
    if(a>b)
        return a;
    return b;    
}
function GetBalanceType()
{
    var bt = 0;
    var rdOriginal = obj('rdOriginal');
    
    if (rdOriginal.checked==true)
        bt = 1;
    else
        bt = 0;
        
    return bt;    
}
function GetDataReport()
{
    var bt = GetBalanceType();
            
    //BeginLoad();
    try{parent.BlockUI();}catch(e){}

    $.ajax({
    url: 'GetData.ashx?balancetype=' + bt + '&mode=18',
    dataType:'html',
    success: function(data)
    {
        obj('sData').innerHTML = data;
        //FinishLoad();
        parent.jQuery.unblockUI();
        Init();
    }
    });   
}

function BeginLoad()
{
    obj('inProgressBar').style.display = 'inline'
    if (obj('btnGo')) {
        obj('btnGo').disabled = true;
        obj('btnExport').disabled = true;
    }
}

function FinishLoad()
    {
        obj('inProgressBar').style.display='none';
        if (obj('btnGo')) {
            obj('btnGo').disabled = false;
            obj('btnExport').disabled = false;
        }
    }


window.onresize = Resize;


if (isIE())
    document.write("<link rel='Stylesheet' type ='text/css' href ='../App_Themes/SteelBlue/IEFixedHeader.css' />");
else
    document.write("<link rel='Stylesheet' type ='text/css' href ='../App_Themes/SteelBlue/NonIEFixedHeader.css' />");
    
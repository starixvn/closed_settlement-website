function obj(id)
{
    return document.getElementById(id);
}
function isIE(){
    return (navigator.appName.indexOf("Internet Explorer")>-1)
}
function getRealTop(el) {
    try {
        if (!$(el)) return 0;
        var yPos = document.getElementById(el).offsetTop;
        var tempEl = document.getElementById(el).offsetParent;
        while (tempEl != null) {
            yPos += tempEl.offsetTop;
            tempEl = tempEl.offsetParent;
        }
    }
    catch (e)
                { }

    return yPos;
}
//fixed header
var headerMain, clientDocument, container, tbody,tblRpt
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
            height = Max(100,height - 40);
        else
            height = Max(100,height - 30);
  
    return height;
}
  
function Init(width)
{
    isScroll = false;
    tblRpt=document.getElementById("tbl-rpt");
    if (tblRpt!=null)
    {
        headerMain=document.getElementById("header_main");
        tbody = tblRpt.getElementsByTagName('tbody');
        tbody = (tbody[0]) ? tbody[0] : null;

        
        if(document.documentElement && ( document.documentElement.clientWidth || document.documentElement.clientHeight))
             clientDocument=document.documentElement;
        else
            clientDocument=document.body;
      
        var topHeader = getRealTop('trHeader');
        var topFooter = getRealTop('trFooter');           
        
        
        
        var innerHeight = window.innerHeight;
        

        var delta = 10;
        
        if (!innerHeight) {
             var innerHeight = document.documentElement.clientHeight;
         }
        var RealHeight = innerHeight - topHeader - delta ;
        
        if (topFooter > innerHeight) {
            jQuery('#tbl-rpt').Scrollable(RealHeight, width);
            isScroll = true;
            if(!isIE())
            {
                var trs = tbody.getElementsByTagName('tr');
                for (x=0; x<trs.length; x++) 
                {
		            var tds = trs[x].getElementsByTagName('td');
		            tds[tds.length-1].style.paddingRight += 17 + 'px';
		        }
		        var footer = tblRpt.getElementsByTagName('tfoot');
		        footer = (footer[0]) ? footer[0] : null;
		        var trs2 = footer.getElementsByTagName('tr');
		        for (x=0; x<trs2.length; x++) 
                {
		            var tds = trs2[x].getElementsByTagName('td');
		            tds[tds.length-1].style.paddingRight += 17 + 'px';
		        }
            } 
        } 
    }
    
    //when finishing creating scrollable table then div_container exists
    container = document.getElementById("div_container")
}
 
function Max(a,b)
{
    if(a>b)
        return a;
    return b;    
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
            if (tbody)
            {
               tbody.style.height = (height - 80) + 'px'; 
            }
        }
        if (container)
        {
            if (tblRpt.offsetHeight < innerHeight)
            {
                container.style.height = tblRpt.offsetHeight + 'px';
            }
            else
            {
                container.style.height = height;
            }
        }  
    }
    
}


var header = document.getElementById('header');
var content = document.getElementById('content');
var wrapper = document.getElementById('wrapper');
var wrapper1 = document.getElementById('wrapper1');

var imgAsc = '&nbsp;<img src="../App_Themes/SteelBlue/Images/asc.gif" />';
var imgDesc = '&nbsp;<img src="../App_Themes/SteelBlue/Images/desc.gif" />';


window.onload=CopyHeader;

function CopyHeader()
{
    header = document.getElementById('header');
    content = document.getElementById('content');
    wrapper = document.getElementById('wrapper');
    wrapper1 = document.getElementById('wrapper1');

	if(content.rows.length <= 3)
	{
		header.style.display = 'none';
		content.style.visibility = 'visible';
		wrapper.style.borderLeftWidth = '0px';
		wrapper.style.overflow = 'visible';
		wrapper.style.marginTop = 0;
		wrapper.style.paddingLeft = '2px';
		wrapper.style.paddingTop = '2px';
		return;
	}
	else
	{
	    if (window.attachEvent) {
	        document.body.scroll = 'no';	        
	    }
	    else {
	        document.body.style.overflow = 'hidden';
	        //document.getElementById('wrapper').style.overflow = true;
	    }
    }
    
    wrapper.style.filter = 'alpha(opacity=0)';
    wrapper.style.opacity = '0.0';        
	
	if(content.rows.length <= 38) dy = 5;
	
    var c1 = content.rows[0].cells.length; // header
    var c2 = content.rows[1].cells.length; // header
    

    //alert('c1: ' + c1 + '\nc2: ' + c2 );
    
    // sorting
    var sortCol = document.getElementById('sortCol');
    if(sortCol.value + "" != "")
    {
        var order = document.getElementById('order');
        var colId = document.getElementById(sortCol.value);
        colId.style.whiteSpace = 'nowrap';
        colId.innerHTML += order.value=="desc"?imgAsc:imgDesc;
    }        
    
    // copy header
    var totalW = 0;
    for(var i=0; i<c1; i++)
    {
        var w = content.rows[0].cells[i].offsetWidth;
        content.rows[0].cells[i].style.width = header.rows[0].cells[i].style.width = w + 'px';
    }
    
    for(var i=0; i<c2; i++)
    {
        var w = content.rows[2].cells[i].offsetWidth;
        
        if (header.rows[1].cells[i].id == "HT_header" || header.rows[1].cells[i].id == "FT_header")
        {
            content.rows[2].cells[i].style.width = header.rows[1].cells[i].style.width = 50 + 'px';       
            totalW += 50; //hard code column HT, FT to 50 px
        }
        else
        {
            if (header.rows[1].cells[i].id!= "" && header.rows[1].cells[i].id==document.getElementById('sortCol').value)
            {
                content.rows[2].cells[i].style.width = header.rows[1].cells[i].style.width = w + 11 + 'px';
                totalW += w + 11; //this for the width of up/down image
            }
            else
            {
                content.rows[2].cells[i].style.width = header.rows[1].cells[i].style.width = w + 'px';       
                totalW += w;
            }
        }
    }
    
    header.style.width = content.style.width = (totalW + dd) + 'px';

    wrapper.style.height = (document.documentElement.clientHeight - dx) + 'px';
    wrapper.style.width = (document.documentElement.clientWidth - 5) + 'px';
    wrapper1.style.width = (document.documentElement.clientWidth - dy) + 'px';
    
    wrapper.onscroll = function()
    {
        wrapper1.scrollLeft = wrapper.scrollLeft;
    }
    
    wrapper.style.marginTop = window.attachEvent?'-1px':'0px';
    
    content.deleteRow(0); // header
    content.deleteRow(0); // header
    
    wrapper.style.overflow = 'auto';
    setTimeout(_RestoreScroll, 100);
    
    window.onresize = _Resize;	
}

function _Resize()
{
    wrapper.style.height = (document.documentElement.clientHeight - dx) + 'px';
    wrapper.style.width = (document.documentElement.clientWidth - 5) + 'px';
    wrapper1.style.width = (document.documentElement.clientWidth - dy) + 'px';
}	

function _RestoreScroll()
{
    // scroll pos
    var scrollLeft = document.getElementById('scrollLeft');
    wrapper1.scrollLeft = wrapper.scrollLeft = parseInt(scrollLeft.value, 10);
    
    header.style.visibility = 'visible';
    content.style.visibility = 'visible';
    
    wrapper.style.filter = 'alpha(opacity=100)';
    wrapper.style.opacity = '1';
}

function ExportToExcel()
{
    document.getElementById('export').click();
}

function Sort(o)
{
    var sortCol = document.getElementById('sortCol');
    var order = document.getElementById('order');
    var scrollLeft = document.getElementById('scrollLeft');
    scrollLeft.value = wrapper.scrollLeft;
    
    if(sortCol.value == o.id)// sort revert
    {
        order.value = (order.value=="desc")?"asc":"desc";
    }
    else // new sorting
    {
        sortCol.value = o.id;
        order.value = "asc";
    }
    document.getElementById('sort').click();
}

function SwitchViewTrader(obj)
{
    var soccer = document.getElementById('soccer');
    var other = document.getElementById('other');
    var IsSoccer = document.getElementById('IsSoccer');


    IsSoccer.value = (obj.id == 'soccer') ? '1' : '0';    
    
    if(obj.id == 'soccer')
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
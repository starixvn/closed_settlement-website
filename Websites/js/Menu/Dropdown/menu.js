
var timeout         = 300;
var closetimer		= 0;
var ddmenuitem ;

// open hidden layer
function mopen(id)
{	
	// cancel close timer
	mcancelclosetime();
	// close old layer
	if (ddmenuitem) ddmenuitem.style.display = "none";
	// get new layer
	ddmenuitem = document.getElementById(id);
	ddmenuitem.style.display = 'block';
}
// close showed layer
function mclose() {
    if (ddmenuitem) ddmenuitem.style.display = 'none';
}

// go close timer
function mclosetime()
{
	closetimer = window.setTimeout(mclose, timeout);
}

// cancel close timer
function mcancelclosetime()
{
	if(closetimer)
	{
		window.clearTimeout(closetimer);
		closetimer = null;
	}
}

// close layer when click-out
document.onclick = mclose; 
// -->
function getScrollWidth()
{
   var w = window.pageXOffset ||
           document.body.scrollLeft ||
           document.documentElement.scrollLeft;
           
   return w ? w : 0;
}

function getScrollHeight()
{
   var h = window.pageYOffset ||
           document.body.scrollTop ||
           document.documentElement.scrollTop;
           
   return h ? h : 0;
}

function getMainWindow() {
    var obj = window;
    while (obj.name != "centerFr" && obj.name != "leftPanel" && obj.parent) {
        obj = obj.parent;
    }
    return obj.parent;
}

function showPopup(title, url, left, top, width, height, isreload, duration, fnRecall) {
    obj = getMainWindow();
    obj.currentExecFrame = window;
    if (fnRecall) obj.currentExecFrame.callback = fnRecall;
    obj.currentExecFrame.isreload = isreload;
    obj.currentExecFrame.duration = duration;

    if (left == "") {
        var theWidth = obj.innerWidth ? obj.innerWidth : obj.document.body.offsetWidth;
        left = (theWidth - width) / 2;
        if (left < 0) left = 0;
    }
    if (top == "") {
        var theHeight = obj.innerHeight ? obj.innerHeight : obj.document.body.offsetHeight;
        top = (theHeight - height) / 2;
        if (top < 0) top = 0
    }
    obj.ibcWnd.open(url, title, left, top, width, height);
    obj.ibcWnd.setRect(left, top, width, height);
}
function hidePopup() {
    
    if (parent.ibcWnd) parent.ibcWnd.close();
}

function afterUpdate(isreload, duration) {
     if (parent.currentExecFrame) //Called from ibcWnd
     {
          
            duration=parent.currentExecFrame.duration;
            if (duration==null) duration=0;
         
            if (parent.currentExecFrame.isreload) 
            {
                setTimeout('parent.currentExecFrame.location.reload(true);',duration);
            }
            setTimeout('hidePopup()', duration); 
            if (parent.currentExecFrame.callback) setTimeout('parent.currentExecFrame.callback()', duration);
            parent.currentExecFrame.PTUnBlockUI();
            setTimeout('parent.currentExecFrame=null;',duration + 1000);
        }  
     else 
        { 
            if (isreload)
            {
                if (duration!=null)
                {
                     setTimeout('PTUnBlockUI();location.href=location.href;', duration);
                }
                else
                {
                    PTUnBlockUI();
                    location.href=location.href;
                }
            }
            
        }
}

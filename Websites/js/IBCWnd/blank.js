function loadUrl() {
    try {
        if (!window.parent.IBCWnd._W.loaded && window.parent.IBCWnd._W.url != null) {
            window.parent.IBCWnd._W.iframe.src = window.parent.IBCWnd._W.url;
            window.parent.IBCWnd._W.loaded = true;
        }
        else if (window.parent.IBCWnd._W.url == null || window.parent.IBCWnd._W.loaded) {
            window.parent.IBCWnd._W.close();
        }
    }
    catch (e) {  }
}

window.onload = loadUrl;
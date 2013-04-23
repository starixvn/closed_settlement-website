// --- CRUID === //
function DeleteNews(id) {
    var strC = confirm("Are you sure you want to delete this news?");
    if (strC) {
        try{parent.BlockUI();}catch(e){}
        $.ajax({
            type: "POST",
            url: "../Handlers/Others/NewsDelete.ashx",
            async: true,
            data: { NewsID: id },
            success: function(data) {
                try{parent.jQuery.unblockUI();}catch(e){}
                if (data.length > 0) {
                    document.getElementById('news_content').innerHTML = data;
                }
                else {
                    //parent.refreshLastestNews();
                    GetNewsByPager(1);
                }
            }
        });        
    }
}

function NewsDetail(id) {    
    try{parent.BlockUI();}catch(e){}        
    $.ajax({
        type: "POST",
        url: "../Handlers/Others/NewsDetail.ashx",
        async: true,
        data: {NewsID: id},        
        success: function(data) {
            obj('news_content').innerHTML = data;            
            try{parent.jQuery.unblockUI();}catch(e){}
        }
    });

}

function AddNews() {
    try{parent.BlockUI();}catch(e){}    
    $.ajax({
        type: "POST",
        url: "../Handlers/Others/NewsEdit.ashx",
        async: true,
        data: { NewsID: 0 },
        success: function(data) {
            obj('news_content').innerHTML = data;
            initHTMLArea();
            SetupCalendar();         
            
            try{parent.jQuery.unblockUI();}catch(e){}
            
        }
    });  
}

function EditNews(id) {
    try{parent.BlockUI();}catch(e){}
    $.ajax({
        type: "POST",
        url: "../Handlers/Others/NewsEdit.ashx",
        async: true,
        data: { NewsID: id },
        success: function(data) {
            obj('news_content').innerHTML = data;
            initHTMLArea();
            try{parent.jQuery.unblockUI();}catch(e){}
            SetupCalendar();            
        }
    });
}


function GoToNewsList() {    
    GetNewsByPager(1);
}

function SaveNews(id) {
    var divErrorContainer = 'divEditNewsErrorContainer';
    var divErrorMessage = 'divEditNewsErrorMsg';
    
    var newsDate=obj('NewsDateTextBox').value;
    var newsTitle=obj('NewsTitleTextBox').value;
    var newsContent = $("#NewsContentTextArea").htmlarea("toHtmlString");
    
    // Check data
    var isOK = dataCheck('NewsTitleTextBox', 'NewsContentTextArea', divErrorContainer, divErrorMessage);
    if (isOK) {
        try{parent.BlockUI();}catch(e){}
        $.ajax({
            type: "POST",
            url: "../Handlers/Others/NewsUpdate.ashx",
            async: true,
            data: { NewsID: id, NewsDateTextBox: newsDate, NewsTitleTextBox: newsTitle, NewsContentTextArea: newsContent },
            success: function(data) {
                try{parent.jQuery.unblockUI();}catch(e){}

                var dataMessage = data.split('|');
                var isError = dataMessage[0] == "True" ? true : false;
                var errorMessage = dataMessage[1];
                var newsID = dataMessage[2];

                obj(divErrorContainer).style.display = isError ? '' : 'none';
                if (isError) {
                    showAlertMessage(divErrorMessage, errorMessage, 1);
                }
                else {
                    //parent.refreshLastestNews();
                    NewsDetail(newsID);
                }
            }
        });
    }
}
function CancelNews(id) {
    if (id > 0) NewsDetail(id);    
    else GetNewsByPager(1);
}
function ResetNews(id) {
    // editing
    if (id > 0) {
        EditNews(id);
    }
    // adding
    else {
        var currentTime = new Date()
        var month = currentTime.getMonth() + 1
        var day = currentTime.getDate()
        var year = currentTime.getFullYear()        

        obj('NewsTitleTextBox').value = '';
        obj('NewsDateTextBox').value = month + "/" + day + "/" + year;
        obj('NewsContentTextArea').value = '';
        $("#NewsContentTextArea").htmlarea("updateHtmlArea");
    }

}

// --- End CRUID === //



// --- Pager --- //
function GetNewsByPager(pageIndex) {
    if (pageIndex == null) {
        if (isNumberNotNull("txtpage"))
            pageIndex = $("#txtpage").val();
        else
            pageIndex = "1";
    }
    try{parent.BlockUI();}catch(e){}
    $.ajax({
        type: "POST",
        url: "../Handlers/Others/NewsList.ashx",
        async: true,
        data: { PageIndex: pageIndex },
        success: BindListNews
    });


}

function BindListNews(data) {
    var news_tpl = $("#news_tpl").html();
    $.template("news_tpl", news_tpl);
    $.tmpl("news_tpl", data).appendTo("#news_content");

    //$("#news_content").html(content);
    try{parent.jQuery.unblockUI();}catch(e){}

    //bind event for pager
    $(".pagenav_ibtn").click(function() {
        if (isNumberNotNull("txtpage")) {
            var pageIndex = $("#txtpage").val();
            var hash = this.href + pageIndex;
            hash = hash.replace(/^.*#/, '');
            $.historyLoad(hash);
            return false;
        }
    });

    $(".pagerlink").click(function() {
        var hash = this.href;
        hash = hash.replace(/^.*#/, '');
        $.historyLoad(hash);
        return false;
    });

}
// --- End Pager --- //

// --- Upload File --- // 
function obj(id) {
    return document.getElementById(id);
}

function CancelUploadImage() {
    $("#divUploadBar").slideUp("slow");
}

function IsValidFileExtension(fileID, divContainer, divErrorMessage, uploadButtonID) {

    var filename = obj(fileID).value;
    var isOK = false;
    if (filename != "") {
        var ext = filename.substring(filename.lastIndexOf('.') + 1, filename.length).toLowerCase();
        var validExtension = "|bmp|dib|gif|jpg|jpeg|jpe|jfif|tif|tiff|png|";

        isOK = validExtension.indexOf("|" + ext + "|") >= 0 ? true : false;
        obj(divContainer).style.display = isOK ? 'none' : '';
        if (!isOK) {
            showAlertMessage(divErrorMessage, "Sorry, You have chosen an invalid file extension. Please choose an image file.", 1);
        }
        else {
            obj(divErrorMessage).innerHTML = "";
            isOK = true;
        }
        obj(uploadButtonID).disabled = isOK ? false : true;
    }
    return isOK;
}

function UploadFile(fileID, divContainer, divErrorMessage, uploadButtonID) {
    if (IsValidFileExtension(fileID, divContainer, divErrorMessage, uploadButtonID)) {
        try{parent.BlockUI();}catch(e){}
        $.ajaxFileUpload
            (
                {
                    url: '../Handlers/Others/NewsImageUpload.ashx',
                    secureuri: false,
                    fileElementId: fileID,
                    dataType: 'html',
                    success: function(data, status) {
                        var dataMessage = data.split('|');
                        var isError = dataMessage[0] == "True" ? true : false;
                        var errorMessage = dataMessage[1];
                        var imageUrl = dataMessage[2];
                        obj(divContainer).style.display = isError ? '' : 'none';
                        if (isError) {
                            showAlertMessage(divErrorMessage, errorMessage, 1);
                        }
                        else {
                            $("#NewsContentTextArea").htmlarea("image", imageUrl);
                        }
                        try{parent.jQuery.unblockUI();}catch(e){}
                    }
                }
            )
    }
}
// --- End Upload File --- //


// --- Validation --- //
function dataCheck(title, content, divContainer, divErrorMessage) {    
    var dataOK = false;
    var titleTextBox = obj(title);
    if (titleTextBox) {
        var contentHtml = $("#" + content).htmlarea("toHtmlString");
        dataOK = titleTextBox.value.length > 0 && contentHtml.length > 0 ? true : false;
    }
    obj(divContainer).style.display = dataOK ? 'none' : '';
    if (!dataOK) {
        showAlertMessage(divErrorMessage, 'The news title and news content can not be blank', 1);
    }
    return dataOK;
}


// --- Utility --- //

function pageload(hash) {
    var pageindex = 1;
    if (hash) {
        // restore ajax loaded state
        if ($.browser.msie) {
            hash = encodeURIComponent(hash);
        }
        pageindex = hash.toString().replace(/Page/i, '');
    } else {
        pageindex = 1;
    }
    GetNewsByPager(pageindex);
}

// --- Html area --- //
function initHTMLArea() {
    $("#NewsContentTextArea").htmlarea({
        toolbar: [
                ["html"], ["bold", "forecolor", "italic", "underline", "strikethrough", "|", "subscript", "superscript"],
                ["increasefontsize", "decreasefontsize"],
                ["orderedlist", "unorderedlist"],
                ["indent", "outdent"],
                ["justifyleft", "justifycenter", "justifyright"],
                ["link", "unlink"],["image", "horizontalrule"],
                ["p", "h1", "h2", "h3", "h4", "h5", "h6"],
                ["cut", "copy", "paste"]
                  , [{
                      // This is how to add a completely custom Toolbar Button
                      css: "image",
                      text: "Upload image",
                      action: function(btn) {
                          $("#divUploadBar").css("display", "none");
                          $("#divUploadBar").slideDown("slow");
                      }
                    }]
                  ]
    });
    var uploadBar = '<div style="clear:both;float:left;display:none;" id="divUploadBar">'
                            + '    <div style="float:left;width:420px;">'
                            + '	        <input type="file" class="inputfile" onchange="return IsValidFileExtension(\'txtfile\',\'divcontainer\',\'msgError\',\'btnUpload\')"'
                            + '            size="32" name="txtfile" id="txtfile" />'
                            + '         <div class="uploadbuttonbar">'
                            + '             <a href="#" class="btn" id="btnUpload"  onclick="UploadFile(\'txtfile\',\'divcontainer\',\'msgError\',\'btnUpload\')">Upload</a>'
                            + '             <a href="#" class="btn" onclick="CancelUploadImage();">Cancel</a>'                            
                            + '         </div>'
                            + '    </div>'
                            + '    <div id="divcontainer" class="bg_table" style="display:none;float:left;clear:both;">'
                            + '        <div id="msgError" class="errMsg">'
                            + '            &nbsp</div>'
                            + '    </div>'
                            + '</div>'
    $("div.jHtmlArea div.ToolBar").append(uploadBar);
    $("div.jHtmlArea div.ToolBar ul li a.html").click(function() {
        obj('divUploadBar').style.display = 'none';
    });
}


// -- Calendar --- //
function SetupCalendar(selectionDate) {    
    // Disable event when user press key enter
    try{
      if (this.jQuery)
      {
           $(document).bind('keypress', function(e) {
                var key = e.charCode ? e.charCode : e.keyCode ? e.keyCode : 0;
                if (key === 13)
                 {
                    e.preventDefault();
                }
            });
      }
    }
    catch (ex) { }

    //  Set up calendar
    var newsDate = obj('NewsDateTextBox').value;
    var dates = newsDate.split('/');
    var year=dates[2];
    var month=dates[0];
    var day=dates[1];
    month=month<10?'0'+month:month;
    day=day<10?'0'+day:day;
    var selectionDate = year + month + dates;
    
    var cal=Calendar.setup({
         animation: false,
         showTime :false,
         bottomBar:true,
         trigger: "NewsDateTextBox_Trigger",
         inputField: "NewsDateTextBox",
         dateFormat: "%m/%d/%Y",
         selection: [selectionDate],      
         onSelect: function() {            
            this.hide();
            }
        });
        cal.selection.set(selectionDate);

     
}



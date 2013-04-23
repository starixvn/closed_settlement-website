using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;
using Sunplus.Accounting.Core.Cache.Extension;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Entities.Others;

public partial class Others_News : PageBase
{

    protected int NewsID = 0;
    protected int CurrentPage = 1;
    protected string BaseUrl = string.Empty;
    protected int AppID = 1;
    public string releaseFeature = string.Empty;
    public bool viewWhatNews = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String strNewsPage = DNAide.Core.CastAide.AsString(Request["p"]);
            Int32.TryParse(strNewsPage, out CurrentPage);
            CurrentPage = (CurrentPage == 0) ? 1 : CurrentPage;

            String strNewsID = DNAide.Core.CastAide.AsString(Request["news"]);
            Int32.TryParse(strNewsID, out NewsID);

            BaseUrl = Request.Path;

            // check view whatnews function
            viewWhatNews = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);

            if (viewWhatNews)
            {
                var news = new News();
                var newsItem = news.GetNewsFeature(AppID);
                var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                releaseFeature = oSerializer.Serialize(newsItem);	
            }
            else
            {
                RedirectDeny();
            }
        }
    }
    
}

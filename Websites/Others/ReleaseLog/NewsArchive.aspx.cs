using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Configurations;

public partial class Others_NewsArchive : PageBase
{
    protected int NewsID = 0;
    protected int CurrentPage = 1;
    protected string BaseUrl = string.Empty;
    protected string AppID = "1";
    public string Recipient = string.Empty;

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
            var viewWhatNews = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
            if (!viewWhatNews)
            {
                RedirectDeny();
            }
            // Get Reception
            var mailRelease = MailSection.Instance.Send.Cast<MailElement>().Where(mail => mail.Name == "ReleaseLog").SingleOrDefault<MailElement>();
            if (mailRelease != null)
            {
                var mailTo = (from mail in mailRelease.Mails.Cast<MailElement>() select mail.To).ToArray<string>();
                Recipient = string.Join(";", mailTo);
            }
        }
    }
}
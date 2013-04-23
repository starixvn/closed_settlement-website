using System;
using Sunplus.Accounting.Business.Customer;
using System.Collections;
using DNAide.Core;

public partial class CustInfo :PageBase
{
    //protected override object FunctionName
    //{
    //    get
    //    {
    //        return Enumerations.Functions.Customer.ViewInfo;

    //    }
    //}
    public bool isBA;
    protected void Page_Load(object sender, EventArgs e)
    {
        string username = System.Web.HttpUtility.UrlDecode(Request.QueryString["username"]);
        if (string.IsNullOrEmpty(username))
        {
            HtmlHolder.Text = string.Empty;
            return;
        }
        CustomerManager customer = new CustomerManager();
        Hashtable hash = customer.GetHtmlCustInfoContent(username.Trim());
        //HtmlHolder.Text = customer.GetHtmlCustInfoContent(username.Trim());
        HtmlHolder.Text = CastAide.AsString(hash["strcontent"]);
        isBA = CastAide.AsBoolean(hash["isba"]);
    }
}

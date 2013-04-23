using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.PositionTaking.Racing;

/// <summary>
/// Summary description for RacingPage
/// </summary>
public class RacingPage : PageBase
{
    public string PageRoles;

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        GetPageRole();
    }
    
    public void GetPageRole()
    {
        var custid = Request["custid"];
        var jsonData = new PageRole {CurrRole = GetCurrentPage()};
        jsonData.PageCrypt = IBCCrypt.IBCCrypt.GenMD5Key(custid + jsonData.CurrRole.ToString()) + IBCCrypt.IBCCrypt.GenMD5Key("^") + custid + jsonData.CurrRole.ToString();
        jsonData.Message = GetRacingMessage();

        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        PageRoles = serializer.Serialize(jsonData);        
    }

    private int GetCurrentPage()
    {
        var levelId = 0;
        string url = Request.CurrentExecutionFilePath;
        if (Request.ApplicationPath.Equals("/"))
            url = "/" + url;
        url = url.Replace(Request.ApplicationPath + "/", "").ToLower();
        System.Threading.Tasks.Parallel.ForEach(_listPage, page =>
        {
            if (url.IndexOf(page.Key.ToLower()) != -1)
            {
                levelId = page.Value;
            }
        });
        return levelId;
    }

    private readonly Dictionary<string, int> _listPage = new Dictionary<string, int>(){
        {"SuperUpdate.aspx", (int) CustInfo.Roles.Super}, 
        {"MasterUpdate.aspx", (int) CustInfo.Roles.Master},
        {"AgentUpdate.aspx", (int) CustInfo.Roles.Agent}, 
        {"MemberUpdate.aspx", (int) CustInfo.Roles.Member},
        {"SuperOnMemberUpdate.aspx", (int) CustInfo.Roles.SuperOnMember}, 
        {"MasterOnMemberUpdate.aspx", (int) CustInfo.Roles.MasterOnMember},
        {"SuperMinUpdate.aspx", (int) CustInfo.Roles.SuperMin}, 
        {"MasterMinMaxUpdate.aspx", (int) CustInfo.Roles.MasterMinMax},
        {"AgentMinMaxUpdate.aspx", (int) CustInfo.Roles.AgentMinMax}
    };

    public Dictionary<string, string> GetRacingMessage()
    {
        var racingKey = new string[] { "alertMinbet", "alertMaxbet", "alertMaxpermatch", "alertMinbet1", "alertMinbet2", "alertMaxbet1", "alertMaxbet2", "alertMaxbet3", "alertMaxpermatch1", "alertMaxpermatch2", "noRacingData", "confirmUpdateMember" };
        var racingMessage = new Dictionary<string, string>();
        foreach (var key in racingKey)
        {
            var msg = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Racing, key);
            racingMessage.Add(key, msg);
        }
        return racingMessage;
    }
    
    public void SetStartupScript()
    {
        var jsScript = new StringBuilder();
        jsScript.Append("<script type=\"text/javascript\">");
        jsScript.Append("if (window.parent.location.toString().toLowerCase().indexOf('default.aspx') == -1) {");
        jsScript.Append("var loc_dir = window.location.toString().substr(7).split('/');" +
                        "var loc_url = 'http://'.concat(window.location.host).concat('/' + loc_dir[1].toString() + '/index.aspx?url=').concat(escape(window.location.toString()));" +
                        "window.location = loc_url;");
        jsScript.Append("}");
        jsScript.Append("</script>");
        
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CheckIsParent", jsScript.ToString(), false);
    }
}

public class PageRole
{
    public int CurrRole { get; set; }
    public int Super = (int) CustInfo.Roles.Super;
    public int Master = (int)CustInfo.Roles.Master;
    public int Agent = (int)CustInfo.Roles.Agent;
    public int Member = (int)CustInfo.Roles.Member;
    public int SuperOnMember = (int)CustInfo.Roles.SuperOnMember;
    public int MasterOnMember = (int)CustInfo.Roles.MasterOnMember;
    public int SuperMin = (int)CustInfo.Roles.SuperMin;
    public int MasterMinMax = (int)CustInfo.Roles.MasterMinMax;
    public int AgentMinMax = (int)CustInfo.Roles.AgentMinMax;
    public string PageCrypt { get; set; }
    public Dictionary<string, string> Message { get; set; }
}

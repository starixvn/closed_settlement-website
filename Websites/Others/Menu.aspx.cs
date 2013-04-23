using System;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web;

public partial class Others_Menu : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string tplMenu = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Others/Menu.htm");
        if(WebSettings.disabledHR)
            TemplateHandler.RemoveBlock(ref tplMenu, "HR-meeting-comm");
        if (WebSettings.disableCS)
            TemplateHandler.RemoveBlock(ref tplMenu, "p2pCasino");
        if (WebSettings.DisableVR)
            TemplateHandler.RemoveBlock(ref tplMenu, "ImportVirtualRate");
        this.sMenu.Text = tplMenu;        
    }
}

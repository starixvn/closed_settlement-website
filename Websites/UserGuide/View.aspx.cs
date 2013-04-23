using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;

public partial class UserGuide_View : System.Web.UI.Page
{
    private string extension = ".htm";
    private string path = "../UserGuide/Data";

    protected void Page_Load(object sender, EventArgs e)
    {   
        var functionName = Request.QueryString["functionName"];
        if (string.IsNullOrEmpty(functionName)) return;

        var directoryInfo = new DirectoryInfo(Server.MapPath(path));
        var arrFileInfo = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in arrFileInfo)
        {
            if (fileInfo.Name == functionName + extension)
            {
                Response.Redirect(path + "/" + fileInfo.Name);
                return;
            }
        }
    }
}

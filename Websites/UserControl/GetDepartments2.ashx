<%@ WebHandler Language="C#" Class="GetDepartments2" %>

using System;
using System.Text;
using System.Data;
using System.Web;
using System.Web.SessionState;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using IBCUtils.DAL;
using DNAide.Core;
using Sunplus.Accounting.DataAccess.IBCDLL;
using System.Data.SqlClient;
using System.Collections;

using DNAide.Web;

public class GetDepartments2 : IHttpHandler, IRequiresSessionState
{
    private bool hasData = false;
    private string strContent = string.Empty;
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        try
        {
            string type=context.Request["t"] as string ?? "";
            if (type.Equals("all"))
            {
                context.Response.Write(GetGroupInfo());
            }
            else if (type.Equals("list"))
            {
                context.Response.Write(GetGroupList());
            }
            else if (type.Equals("pol"))
            {
                int id = CastAide.AsInt32(context.Request["groupId"]);
                context.Response.Write(GetGroupPolicy(id));
            }
            else if (type.Equals("mem"))
            {
                int id = CastAide.AsInt32(context.Request["groupId"]);
                string dname = CastAide.AsString(context.Request["dname"]);
                if (id == 0)
                {
                    context.Response.Write(GetAllAccountList(context));
                }
                else
                {
                    context.Response.Write(GetAccountList(id, dname));
                }
            }
            else if (type.Equals("acc"))
            {
                int id = CastAide.AsInt32(context.Request["custId"]);
                string dept = CastAide.AsString(context.Request["dept"]);
                context.Response.Write(GetAccountPolicy(id,dept));
            }
            else if (type.Equals("mult"))
            {
                string arrCustId = CastAide.AsString(context.Request["custId"]);
                string arrUser = CastAide.AsString(context.Request["user"].Trim());
                string dept = CastAide.AsString(context.Request["dept"]);
                if (arrUser.Length > 1) arrUser = arrUser.Substring(0, arrUser.Length - 1);
                context.Response.Write(GetMultUpdPolicy(arrCustId,arrUser,dept));
            }
        }            
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }  
    }
    
    private static string GetGroupInfo()
    {
        var htmlGroupList = new StringBuilder();
        try
        {
            using (var dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect())
            {
                int count = 0;
                htmlGroupList.Append("{\"dataTable\":[");
                while (dr.Read())
                {
                    if (string.Compare(CastAide.AsString(dr["groupname"]), "None", true) == 0) continue;
                    htmlGroupList.Append(count == 0 ? "{" : ",{");
                    htmlGroupList.Append("\"nouser\":" + CastAide.AsString(dr["nouser"]));
                    htmlGroupList.Append(",\"department\":\"" + CastAide.AsString(dr["groupname"]) + "\"");
                    htmlGroupList.Append(",\"groupid\":\"" + CastAide.AsString(dr["groupid"]) + "\"");
                    htmlGroupList.Append(",\"desc\":\"" + CastAide.AsString(dr["groupdesc"]) + "\"");
                    htmlGroupList.Append("}");
                    ++count;
                }
                htmlGroupList.Append("]}");
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return htmlGroupList.ToString();
    }

    private static string GetGroupList()
    {        
        string content = string.Empty;        
        try
        {            
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect())
            {
                content += "<select style=\"width:100%\" id=\"selEGroup\" name=\"selEGroup\">";
                content += "<option value=\"0\">[Select]</option>";                    
                while (dr.Read())
                {                     
                    content += "<option value=\"" + CastAide.AsString(dr["groupid"]) + "\">" + CastAide.AsString(dr["groupname"]) + "</option>";                    
                }
                content += "</select>";
            }            
        }            
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }        
        return content;
        
    }

    private string GetGroupPolicy(int groupId)
    {
        string headerTpl = string.Empty;
        string rowTpl = string.Empty;
        try
        {
            headerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/FunctionHeader.htm");            
            TemplateHandler.RemoveBlock(ref headerTpl, "individual");
            TemplateHandler.RemoveBlock(ref headerTpl, "nonegpolicy");
            TemplateHandler.Assign(ref headerTpl, "{dept}", "Department");
            rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/FunctionRow.htm");
            TemplateHandler.RemoveBlock(ref rowTpl, "individual");
            string groupname = string.Empty;
            string strContent = string.Empty;
            string strFuncId = string.Empty;
            string _strsubid = "";
            //DataResult dr =Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectGroupPolicy(groupId);      
            DbResult dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectGroupPolicy(groupId);      
            DataTable dtb=dr.ResultSet.Tables[0];         
            if (dtb.Rows.Count > 0)
            {
                for (int i = 0; i < dtb.Rows.Count; i++)                
                {
                      
                    string row = rowTpl;
                    strFuncId += CastAide.AsString(dtb.Rows[i]["funcid"]) + ",";
                    groupname = CastAide.AsString(dtb.Rows[i]["groupname"]);
                    TemplateHandler.Assign(ref row, "{funcid}", CastAide.AsString(dtb.Rows[i]["funcid"]));
                 
                    if (CastAide.AsInt32(dtb.Rows[i]["subfunccount"]) > 0)
                    {                        
                        string strsubid = string.Empty;  
                        for (int j = 1; j < CastAide.AsInt32(dtb.Rows[i]["subfunccount"]) + 1; j++)
                        {
                            if (j == (CastAide.AsInt32(dtb.Rows[i]["subfunccount"])))
                            {
                                strsubid += CastAide.AsString(dtb.Rows[i + j]["funcid"]);
                            }
                            else
                            {
                                strsubid += CastAide.AsString(dtb.Rows[i + j]["funcid"]) + ";";
                            }
                        }                      
                        TemplateHandler.Assign(ref row, "{num_sub}", strsubid);

                        
                        TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                        TemplateHandler.Assign(ref row, "{indent}", string.Empty);
                        TemplateHandler.Assign(ref row, "{color}", "");                       
                        
                        _strsubid = strsubid;
                    }
                    else
                    {                     
                        if (CastAide.AsInt32(dtb.Rows[i]["funcid"]) != CastAide.AsInt32(dtb.Rows[i]["parentid"]))
                        {
                            TemplateHandler.Assign(ref row, "{funcname}", "<div class='indent'>&nbsp;</div>&nbsp;" + CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                            TemplateHandler.Assign(ref row, "{indent}", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div class='indent'>&nbsp;</div>");
                            TemplateHandler.Assign(ref row, "{color}", "subfunction");
                            
                            TemplateHandler.Assign(ref row, "{num_sub}", "S-" + CastAide.AsString(dtb.Rows[i]["parentid"]) + "_" + _strsubid);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                            TemplateHandler.Assign(ref row, "{indent}", string.Empty);
                            TemplateHandler.Assign(ref row, "{color}", "");

                            TemplateHandler.Assign(ref row, "{num_sub}", "S-" + CastAide.AsString(dtb.Rows[i]["parentid"]));
                        }                  
                    }
                   
                    TemplateHandler.Assign(ref row, "{funcidHR}", CastAide.AsString(dtb.Rows[i]["funcid"]));
                                        
                    if (CastAide.AsInt32(dtb.Rows[i]["policy"]) == 1)
                    {
                        TemplateHandler.Assign(ref row, "{read-checked}", "checked=\"true\"");
                        TemplateHandler.Assign(ref row, "{host-checked}", "");
                    }
                    else if (CastAide.AsInt32(dtb.Rows[i]["policy"]) >= 2)
                    {
                        TemplateHandler.Assign(ref row, "{read-checked}", "checked=\"true\"");
                        TemplateHandler.Assign(ref row, "{host-checked}", "checked=\"true\"");
                    }
                    else
                    {
                        TemplateHandler.Assign(ref row, "{read-checked}", "");
                        TemplateHandler.Assign(ref row, "{host-checked}", "");
                    }
                    strContent += row;
                }
                if (strFuncId.Length > 0) strFuncId = strFuncId.Substring(0, strFuncId.Length - 1);
                TemplateHandler.Assign(ref headerTpl, "{funcid}", strFuncId);
                TemplateHandler.Assign(ref headerTpl, "{groupid}", groupId.ToString());
                TemplateHandler.Assign(ref headerTpl, "{custid}", "");
                TemplateHandler.Assign(ref headerTpl, "{deptname}", groupname);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref headerTpl, "{no-update}");
                strContent = AccCommon.GenNoInformation("3");                
            }                        
            TemplateHandler.Assign(ref headerTpl, "{content}", strContent);
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }
        return headerTpl;
    }

    private static string GetAccountList(int groupId,string deptname)
    {
        var htmlGroupPolicy = new StringBuilder();
        try
        {
            int no = 1;
            var hash = new Hashtable();
            hash.Add("groupid", groupId);
            int totalcol = 8;
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminByGroup(hash))
            {
                string templateCheckBox = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/CheckBoxcontrol.htm");
                htmlGroupPolicy.Append("{\"dataTable\":[");
                string strStyle;       
                while (dr.Read())
                {
                    bool isClosed = (!Convert.IsDBNull(dr["closed"])) ? (bool)dr["closed"] : false;
                    if (isClosed)
                    {
                        strStyle = "closedacc";
                    }
                    else if (no % 2 == 0)
                    {
                        strStyle = "evenrow";
                    }
                    else
                    {
                        strStyle = "";
                    }
                    htmlGroupPolicy.Append(no == 1 ? "{" : ",{");
                    htmlGroupPolicy.Append("\"no\":" + no.ToString());
                    htmlGroupPolicy.Append(",\"style\":\"" + strStyle + "\"");
                    htmlGroupPolicy.Append(",\"custid\":\"" + CastAide.AsString(dr["custid"]) + "\"");
                    htmlGroupPolicy.Append(",\"user\":\"" + CastAide.AsString(dr["username"]) + "\"");
                    htmlGroupPolicy.Append(",\"fname\":\"" + CastAide.AsString(dr["firstname"]) + "\"");
                    htmlGroupPolicy.Append(",\"lname\":\"" + CastAide.AsString(dr["lastname"]) + "\"");
                    htmlGroupPolicy.Append(",\"strstatus\":\"" + ((isClosed) ? "Closed" : "Active") + "\"");
                    htmlGroupPolicy.Append(",\"status\":\"" + ((!isClosed) ? "checked='checked'" : string.Empty) + "\"");
                    htmlGroupPolicy.Append(",\"creator\":\"" + CastAide.AsString(dr["creator"]) + "\"");
                    htmlGroupPolicy.Append(",\"createddate\":\"" + CastAide.AsString(dr["createdate"]) + "\"");
                    htmlGroupPolicy.Append(",\"policy\":\"" + "Account Policy" + "\"");
                    htmlGroupPolicy.Append(",\"dept\":\"" + CastAide.AsString(dr["groupname"]) + "\"");
                    htmlGroupPolicy.Append(",\"groupid\":\"" + CastAide.AsString(dr["groupid"]) + "\"");
                    htmlGroupPolicy.Append(",\"pass\":\"" + "Password" + "\"");
                    htmlGroupPolicy.Append("}");
                    //-------------------------------------
                    no++;
                }
                htmlGroupPolicy.Append("]}");                           
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return htmlGroupPolicy.ToString();
    }

    private static string GetAllAccountList(HttpContext context)
    {
        var htmlGroupPolicy = new StringBuilder();
        try
        {
            string searchText = context.Request["txtSearch"];
            searchText = (!string.IsNullOrEmpty(searchText) && searchText.Equals("All",StringComparison.CurrentCultureIgnoreCase)) ? "" : searchText;

            string selectAlpha = context.Request["listAlpha"];
            selectAlpha = (!string.IsNullOrEmpty(selectAlpha) && selectAlpha.Equals("All", StringComparison.CurrentCultureIgnoreCase)) ? "" : selectAlpha;

            var hash = new Hashtable();
            if (!string.IsNullOrEmpty(searchText))
            {
                string keyword = "%" + searchText.Replace("'", "") + "%";
                hash.Add("username", keyword);
            }
            else
                searchText = "All";
            if (!string.IsNullOrEmpty(selectAlpha))
            {
                string firstchar = selectAlpha;
                if (string.Compare(firstchar, "0-9", true) == 0)
                {
                    firstchar = "[" + firstchar + "]";
                }
                firstchar = firstchar + "%";
                hash.Add("firstchar", firstchar);
            }
            
            int no = 1;
            
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminByGroup(hash))
            {
                string templateCheckBox = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/CheckBoxcontrol.htm");
                htmlGroupPolicy.Append("{\"dataTable\":[");
                string strStyle;
                while (dr.Read())
                {
                    bool isClosed = (!Convert.IsDBNull(dr["closed"])) ? (bool)dr["closed"] : false;
                    if (isClosed)
                    {
                        strStyle = "closedacc";
                    }
                    else if (no % 2 == 0)
                    {
                        strStyle = "evenrow";
                    }
                    else
                    {
                        strStyle = "";
                    }
                    htmlGroupPolicy.Append(no == 1 ? "{" : ",{");
                    htmlGroupPolicy.Append("\"no\":" + no.ToString());
                    htmlGroupPolicy.Append(",\"style\":\"" + strStyle + "\"");
                    htmlGroupPolicy.Append(",\"custid\":\"" + CastAide.AsString(dr["custid"]) + "\"");
                    htmlGroupPolicy.Append(",\"user\":\"" + CastAide.AsString(dr["username"]) + "\"");
                    htmlGroupPolicy.Append(",\"fname\":\"" + CastAide.AsString(dr["firstname"]) + "\"");
                    htmlGroupPolicy.Append(",\"lname\":\"" + CastAide.AsString(dr["lastname"]) + "\"");
                    htmlGroupPolicy.Append(",\"strstatus\":\"" + ((isClosed) ? "Closed" : "Active") + "\"");
                    htmlGroupPolicy.Append(",\"status\":\"" + ((!isClosed) ? "checked='checked'" : string.Empty) + "\"");
                    htmlGroupPolicy.Append(",\"creator\":\"" + CastAide.AsString(dr["creator"]) + "\"");
                    htmlGroupPolicy.Append(",\"createddate\":\"" + CastAide.AsString(dr["createdate"]) + "\"");
                    htmlGroupPolicy.Append(",\"policy\":\"" + "Account Policy" + "\"");
                    htmlGroupPolicy.Append(",\"dept\":\"" + CastAide.AsString(dr["groupname"]) + "\"");
                    htmlGroupPolicy.Append(",\"groupid\":\"" + CastAide.AsString(dr["groupid"]) + "\"");
                    htmlGroupPolicy.Append(",\"pass\":\"" + "Password" + "\"");
                    htmlGroupPolicy.Append("}");
                    //-------------------------------------
                    no++;
                }
                htmlGroupPolicy.Append("]}");
            }            
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return htmlGroupPolicy.ToString();
    }

    private static string GetStatusCheckBox(string templateString, bool isClosed, int custId, int groupId, string dept)
    {
        string s = templateString;
        try
        {
            TemplateHandler.Assign(ref s, "{name}", (custId.ToString() + "_closed"));
            string options = (!isClosed) ? "checked='checked' " : string.Empty;            
            TemplateHandler.Assign(ref s, "{options}", options);
            TemplateHandler.Assign(ref s, "{onclickAction}", "updateStatus(" + custId + "," + groupId + ",'"+ dept +"');");
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return s;
    }

    private string GetAccountPolicy(int custId,string deptname)
    {
        string headerTpl = string.Empty;
        string rowTpl = string.Empty;
        try
        {
            headerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/FunctionHeader.htm");
            TemplateHandler.Assign(ref headerTpl, "{dept}", "Department");
            TemplateHandler.Assign(ref headerTpl, "{acc}", "Account");
            rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/FunctionRow.htm");
            string strContent = string.Empty;
            string username = string.Empty;
            string strFuncId = string.Empty;
            string _strsubid = "";
            //DataResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminPolicy(custId);
            DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminPolicy(custId);
            DataTable dtb = ds.ResultSet.Tables[0];
          
            if (ds!=null)
            {
                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dtb.Rows.Count; i++)
                    {
                        
                        hasData = true;
                        string row = rowTpl;
                        strFuncId += CastAide.AsString(dtb.Rows[i]["funcid"]) + ",";
                        username = CastAide.AsString(dtb.Rows[i]["username"]);                                             
                        TemplateHandler.Assign(ref row, "{funcid}", CastAide.AsString(dtb.Rows[i]["funcid"]));

                        if (CastAide.AsInt32(dtb.Rows[i]["subfunccount"]) > 0)
                        {
                            string strsubid = string.Empty;
                            for (int j = 1; j < CastAide.AsInt32(dtb.Rows[i]["subfunccount"]) + 1; j++)
                            {
                                if (j == (CastAide.AsInt32(dtb.Rows[i]["subfunccount"])))
                                {
                                    strsubid += CastAide.AsString(dtb.Rows[i + j]["funcid"]);
                                }
                                else
                                {
                                    strsubid += CastAide.AsString(dtb.Rows[i + j]["funcid"]) + ";";
                                }
                            }
                            TemplateHandler.Assign(ref row, "{num_sub}", strsubid);


                            TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                            TemplateHandler.Assign(ref row, "{indent}", string.Empty);
                            TemplateHandler.Assign(ref row, "{color}", "#ffffff");

                            _strsubid = strsubid;
                        }
                        else
                        {
                            if (CastAide.AsInt32(dtb.Rows[i]["funcid"]) != CastAide.AsInt32(dtb.Rows[i]["parentid"]))
                            {
                                TemplateHandler.Assign(ref row, "{funcname}", "<div class='indent'>&nbsp;</div>&nbsp;" + CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                                TemplateHandler.Assign(ref row, "{indent}", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div class='indent'>&nbsp;</div>");
                                TemplateHandler.Assign(ref row, "{color}", "#f0f1ec");

                                TemplateHandler.Assign(ref row, "{num_sub}", "S-" + CastAide.AsString(dtb.Rows[i]["parentid"]) + "_" + _strsubid);
                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                                TemplateHandler.Assign(ref row, "{indent}", string.Empty);
                                TemplateHandler.Assign(ref row, "{color}", "#ffffff");

                                TemplateHandler.Assign(ref row, "{num_sub}", "S-" + CastAide.AsString(dtb.Rows[i]["parentid"]));
                            }
                        }
                                               
                        TemplateHandler.Assign(ref row, "{funcidHR}", CastAide.AsString(dtb.Rows[i]["funcid"]));
                                              
                        if (CastAide.AsInt32(dtb.Rows[i]["policy"]) == 1)
                        {
                            TemplateHandler.Assign(ref row, "{read-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{host-checked}", "");
                        }
                        else if (CastAide.AsInt32(dtb.Rows[i]["policy"]) >= 2)
                        {
                            TemplateHandler.Assign(ref row, "{read-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{host-checked}", "checked=\"true\"");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{read-checked}", "");
                            TemplateHandler.Assign(ref row, "{host-checked}", "");
                        }
                        
                        //display policy of group which this account belong to
                        if (CastAide.AsInt32(dtb.Rows[i]["gpolicy"]) == 1)
                        {
                            TemplateHandler.Assign(ref row, "{gread-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{ghost-checked}", "");
                        }
                        else if (CastAide.AsInt32(dtb.Rows[i]["gpolicy"]) >= 2)
                        {
                            TemplateHandler.Assign(ref row, "{gread-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{ghost-checked}", "checked=\"true\"");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{gread-checked}", "");
                            TemplateHandler.Assign(ref row, "{ghost-checked}", "");
                        }

                        strContent += row;
                    }
                    TemplateHandler.Assign(ref headerTpl, "{groupid}", CastAide.AsString(ds.Outputs["groupid"]));
                }              
                if (strFuncId.Length > 0) strFuncId = strFuncId.Substring(0, strFuncId.Length - 1);
                TemplateHandler.Assign(ref headerTpl, "{funcid}", strFuncId);
                
                TemplateHandler.Assign(ref headerTpl, "{arruser}", "");
                TemplateHandler.Assign(ref headerTpl, "{custid}", custId.ToString());                
            }
            TemplateHandler.Assign(ref headerTpl, "{deptname}", deptname);
            TemplateHandler.Assign(ref headerTpl, "{user}", username);
            
            if (!hasData)
            {                
                TemplateHandler.RemoveBlock(ref headerTpl, "{no-update}");
                strContent= AccCommon.GenNoInformation("3");
            }
            TemplateHandler.Assign(ref headerTpl, "{content}", strContent);
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return headerTpl;
    }

    private string GetMultUpdPolicy(string arrId,string arrUser,string dept)
    {
        string headerTpl = string.Empty;
        string rowTpl = string.Empty;
        try
        {
            headerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/FunctionHeader.htm");
            TemplateHandler.Assign(ref headerTpl, "{dept}", "Department");
            TemplateHandler.Assign(ref headerTpl, "{deptname}", dept);
            TemplateHandler.Assign(ref headerTpl, "{acc}", "Account");
            rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/FunctionRow.htm");
            string strContent = string.Empty;
            string username = string.Empty;
            string strFuncId = string.Empty;
            string _strsubid = "";
            //DataResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectMultUpdPolicy(arrId);
            DbResult ds = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectMultUpdPolicy(arrId);
            DataTable dtb = ds.ResultSet.Tables[0];
            if(ds!=null)
            {
                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.ResultSet.Tables[0].Rows.Count; i++)
                    {
                        DataRow dr = ds.ResultSet.Tables[0].Rows[i];
                        hasData = true;
                        string row = rowTpl;
                        strFuncId += CastAide.AsString(dr["funcid"]) + ",";
                                                                   
                        
                        //TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dr["funcdesc"]));
                        TemplateHandler.Assign(ref row, "{funcid}", CastAide.AsString(dr["funcid"]));

                        if (CastAide.AsInt32(dtb.Rows[i]["subfunccount"]) > 0)
                        {
                            string strsubid = string.Empty;
                            for (int j = 1; j < CastAide.AsInt32(dtb.Rows[i]["subfunccount"]) + 1; j++)
                            {
                                if (j == (CastAide.AsInt32(dtb.Rows[i]["subfunccount"])))
                                {
                                    strsubid += CastAide.AsString(dtb.Rows[i + j]["funcid"]);
                                }
                                else
                                {
                                    strsubid += CastAide.AsString(dtb.Rows[i + j]["funcid"]) + ";";
                                }
                            }
                            TemplateHandler.Assign(ref row, "{num_sub}", strsubid);


                            TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                            TemplateHandler.Assign(ref row, "{indent}", string.Empty);
                            TemplateHandler.Assign(ref row, "{color}", "#ffffff");

                            _strsubid = strsubid;
                        }
                        else
                        {
                            if (CastAide.AsInt32(dtb.Rows[i]["funcid"]) != CastAide.AsInt32(dtb.Rows[i]["parentid"]))
                            {
                                TemplateHandler.Assign(ref row, "{funcname}", "<div class='indent'>&nbsp;</div>&nbsp;" + CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                                TemplateHandler.Assign(ref row, "{indent}", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div class='indent'>&nbsp;</div>");
                                TemplateHandler.Assign(ref row, "{color}", "#f0f1ec");

                                TemplateHandler.Assign(ref row, "{num_sub}", "S-" + CastAide.AsString(dtb.Rows[i]["parentid"]) + "_" + _strsubid);
                            }
                            else
                            {
                                TemplateHandler.Assign(ref row, "{funcname}", CastAide.AsString(dtb.Rows[i]["funcdesc"]));
                                TemplateHandler.Assign(ref row, "{indent}", string.Empty);
                                TemplateHandler.Assign(ref row, "{color}", "#ffffff");

                                TemplateHandler.Assign(ref row, "{num_sub}", "S-" + CastAide.AsString(dtb.Rows[i]["parentid"]));
                            }
                        }

                        TemplateHandler.Assign(ref row, "{funcidHR}", CastAide.AsString(dtb.Rows[i]["funcid"]));
                        //-------------
                        
                        
                        if (CastAide.AsInt32(dr["policy"]) == 1)
                        {
                            TemplateHandler.Assign(ref row, "{read-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{host-checked}", "");
                        }
                        else if (CastAide.AsInt32(dr["policy"]) >= 2)
                        {
                            TemplateHandler.Assign(ref row, "{read-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{host-checked}", "checked=\"true\"");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{read-checked}", "");
                            TemplateHandler.Assign(ref row, "{host-checked}", "");
                        }
                        //display policy of group which this account belong to
                        if (CastAide.AsInt32(dr["gpolicy"]) == 1)
                        {
                            TemplateHandler.Assign(ref row, "{gread-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{ghost-checked}", "");
                        }
                        else if (CastAide.AsInt32(dr["gpolicy"]) >= 2)
                        {
                            TemplateHandler.Assign(ref row, "{gread-checked}", "checked=\"true\"");
                            TemplateHandler.Assign(ref row, "{ghost-checked}", "checked=\"true\"");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref row, "{gread-checked}", "");
                            TemplateHandler.Assign(ref row, "{ghost-checked}", "");
                        }

                        strContent += row;
                    }
                    TemplateHandler.Assign(ref headerTpl, "{groupid}",CastAide.AsString( ds.Outputs["groupid"]));
                }
                if (strFuncId.Length > 0) strFuncId = strFuncId.Substring(0, strFuncId.Length - 1);
                TemplateHandler.Assign(ref headerTpl, "{funcid}", strFuncId);                
                TemplateHandler.Assign(ref headerTpl, "{custid}", arrId);
                TemplateHandler.Assign(ref headerTpl, "{arruser}", arrUser);
            }
            TemplateHandler.Assign(ref headerTpl, "{user}", arrUser);
            if (!hasData)
            {
                TemplateHandler.RemoveBlock(ref headerTpl, "{no-update}");
                strContent = AccCommon.GenNoInformation("3");
            }
            TemplateHandler.Assign(ref headerTpl, "{content}", strContent);
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return headerTpl;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
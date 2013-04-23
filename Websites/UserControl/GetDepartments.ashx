<%@ WebHandler Language="C#" Class="GetDepartments" %>

using System;
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

public class GetDepartments : IHttpHandler, IRequiresSessionState
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
    
    private string GetGroupInfo()
    {
        string headerTpl = string.Empty;
        string contentTpl = string.Empty;
        try
        {
            headerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/DepartmentHeader.htm");
            contentTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/DepartmentRow.htm");             
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect())
            {
                while (dr.Read())
                {
                    hasData = true;
                    string row = contentTpl;
                    if (string.Compare(CastAide.AsString(dr["groupname"]), "None", true)==0)
                    {
                        TemplateHandler.RemoveBlock(ref row, "none-edit");
                    }
                    TemplateHandler.Assign(ref row, "{department}", CastAide.AsString(dr["groupname"]));
                    TemplateHandler.Assign(ref row, "{nouser}", CastAide.AsString(dr["nouser"]));
                    TemplateHandler.Assign(ref row, "{desc}", CastAide.AsString(dr["groupdesc"]));
                    TemplateHandler.Assign(ref row, "{rolep}", "");
                    TemplateHandler.Assign(ref row, "{groupid}", CastAide.AsString(dr["groupid"]));
                    headerTpl += row;
                }
            }
            if (!hasData)
            {
                headerTpl += AccCommon.GenNoInformation("4");
            }
            headerTpl += "</table>";
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return headerTpl;
    }

    private string GetGroupList()
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

    private string GetAccountList(int groupId,string deptname)
    {
        string headerTpl = string.Empty;
        string rowTpl = string.Empty;
        try
        {
            headerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/AccountListHeader.htm");
            TemplateHandler.Assign(ref headerTpl, "{dept}", deptname);
            rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/AccountListRow.htm");
            string templateCheckBox = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/CheckBoxcontrol.htm");
            string strStyle = string.Empty;
            string strContent = string.Empty;
            int no = 1;
            Hashtable hash = new Hashtable();
            hash.Add("groupid", groupId);
            int totalcol = 8;
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminByGroup(hash))
            {                
                while (dr.Read())
                {
                    hasData = true;
                    string row = rowTpl;
                    bool isClosed = (!Convert.IsDBNull(dr["closed"])) ? (bool)dr["closed"] : false;
                    int custId = CastAide.AsInt32(dr["custid"]);
                    if (isClosed)
                    {
                        strStyle = "class=\"closedacc\"";
                    }
                    else if (no % 2 == 0)
                    {
                        strStyle = "class=\"evenrow\"";
                    }
                    else
                    {
                        strStyle = "";
                    }
                    TemplateHandler.Assign(ref row, "{groupid}", groupId);
                    TemplateHandler.Assign(ref row, "{style}",strStyle);
                    TemplateHandler.Assign(ref row, "{dept}", deptname);
                    TemplateHandler.Assign(ref row, "{no}",no.ToString());
                    TemplateHandler.Assign(ref row, "{user}", CastAide.AsString(dr["username"]));
                    TemplateHandler.Assign(ref row, "{fname}", CastAide.AsString(dr["firstname"]));
                    TemplateHandler.Assign(ref row, "{lname}", CastAide.AsString(dr["lastname"]));
                    TemplateHandler.Assign(ref row, "{creator}", CastAide.AsString(dr["creator"]));                    
                    TemplateHandler.Assign(ref row, "{createddate}", CastAide.AsString(dr["createdate"]));
                    TemplateHandler.Assign(ref row, "{pass}", "Password");
                    TemplateHandler.Assign(ref row, "{policy}", "Account Policy");
                    TemplateHandler.Assign(ref row, "{custid}", CastAide.AsString(dr["custid"]));
                    TemplateHandler.Assign(ref row, "{strstatus}", (isClosed) ? "Closed" : "Active");
                    TemplateHandler.Assign(ref row, "{status}", GetStatusCheckBox(templateCheckBox, isClosed, custId, groupId, deptname));
                    
                    no++;
                    strContent += row;
                }                                
            }
            if (no < 2)
            {
                TemplateHandler.RemoveBlock(ref headerTpl, "noupdate");
                TemplateHandler.RemoveBlock(ref headerTpl, "noupdate");
                totalcol = totalcol + 2;
            }
            if (!hasData)
            {                
                strContent=AccCommon.GenNoInformation("10");                
            }
           TemplateHandler.Assign(ref headerTpl, "{deptname}", deptname);   
            TemplateHandler.Assign(ref headerTpl, "{content}", strContent);
            TemplateHandler.Assign(ref headerTpl, "{colspan}", totalcol.ToString());         
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return headerTpl;
    }

    private string GetAllAccountList(HttpContext context)
    {
        string headerTpl = string.Empty;
        string rowTpl = string.Empty;
        try
        {
            headerTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/AllAccountListHeader.htm");
            rowTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/AllAccountListRow.htm");
            string templateCheckBox = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/CheckBoxcontrol.htm");
            
            ///////////////////////////////////            
            TemplateHandler.Assign(ref headerTpl, "{functionName}", "SelAccount('','')");
            string searchText = context.Request["txtSearch"];
            searchText = (!string.IsNullOrEmpty(searchText) && searchText.Equals("All",StringComparison.CurrentCultureIgnoreCase)) ? "" : searchText;

            string selectAlpha = context.Request["listAlpha"];
            selectAlpha = (!string.IsNullOrEmpty(selectAlpha) && selectAlpha.Equals("All", StringComparison.CurrentCultureIgnoreCase)) ? "" : selectAlpha;

            Hashtable hash = new Hashtable();
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

            //strQuery += "&txtSearch=" + searchText;
            //strQuery += "&listAlpha=" + selectAlpha;
            //strQuery += "&strSearch=Submit";

            TemplateHandler.Assign(ref headerTpl, "{search-Val}", searchText);
            TemplateHandler.Assign(ref headerTpl, "{listAlphaValue}", AccCommon.GetListAlPhabet(selectAlpha));            
            //////////////////////////////////           
            
            string strStyle = string.Empty;
            string strContent = string.Empty;
            int no = 1;
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminByGroup(hash))
            {
                while (dr.Read())
                {
                    hasData = true;
                    string row = rowTpl;
                    bool isClosed = (!Convert.IsDBNull(dr["closed"])) ? (bool)dr["closed"] : false;
                    int custId = CastAide.AsInt32(dr["custid"]);
                    if (isClosed)
                    {
                        strStyle = "class=\"closedacc\"";
                    }
                    else if (no % 2 == 0)
                    {
                        strStyle = "class=\"evenrow\"";
                    }
                    else
                    {
                        strStyle = "";
                    }
                    string deptname=CastAide.AsString(dr["groupname"]);
                    int groupid=CastAide.AsInt32(dr["groupid"]);
                    TemplateHandler.Assign(ref row, "{style}", strStyle);
                    TemplateHandler.Assign(ref row, "{dept}", deptname);
                    TemplateHandler.Assign(ref row, "{groupid}", groupid.ToString());
                    TemplateHandler.Assign(ref row, "{no}", no.ToString());
                    TemplateHandler.Assign(ref row, "{user}", CastAide.AsString(dr["username"]));
                    TemplateHandler.Assign(ref row, "{fname}", CastAide.AsString(dr["firstname"]));
                    TemplateHandler.Assign(ref row, "{lname}", CastAide.AsString(dr["lastname"]));
                    TemplateHandler.Assign(ref row, "{creator}", CastAide.AsString(dr["creator"]));
                    TemplateHandler.Assign(ref row, "{createddate}", CastAide.AsString(dr["createdate"]));
                    TemplateHandler.Assign(ref row, "{pass}", "Password");
                    TemplateHandler.Assign(ref row, "{policy}", "Account Policy");
                    TemplateHandler.Assign(ref row, "{custid}", CastAide.AsString(dr["custid"]));
                    TemplateHandler.Assign(ref row, "{strstatus}", (isClosed) ? "Closed" : "Active");
                    TemplateHandler.Assign(ref row, "{status}", GetStatusCheckBox(templateCheckBox, isClosed, custId,groupid , deptname));

                    no++;
                    strContent += row;
                }
            }            
            if (!hasData)
            {
                strContent = AccCommon.GenNoInformation("11");
            }            
            TemplateHandler.Assign(ref headerTpl, "{content}",strContent);
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return headerTpl;
    }

    private string GetStatusCheckBox(string templateString, bool isClosed, int custId, int groupId, string dept)
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
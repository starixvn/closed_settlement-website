using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Business.Interface;
using Fanex.Tarus.DataAccess.Implement;
using Fanex.Tarus.DataAccess.Interface;
using Fanex.Tarus.Entities;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Business.Implement
{
    public class AdminService : BaseService, IAdminService
    {
        private IAdminAccess _adminAccess;

        public AdminService()
            : this(null)
        {
        }

        public AdminService(IAdminAccess adminAccess)
        {
            _adminAccess = adminAccess ?? new AdminAccess();
        }



        public List<AgentAccess> GetListAgentAcess()
        {
            return _adminAccess.GetListAgentAcess();
            //var list = new List<AgentAccess>();
            //return list;
        }


        public Result AddUser(string userName, int creator, string remark)
        {
            return _adminAccess.AddUser(userName, creator, remark);
        }


        public Result Update(int custId, bool isAllowed, string remark)
        {
            return _adminAccess.Update(custId, isAllowed, remark);
        }


        public Result Delete(int custId)
        {
            return _adminAccess.Delete(custId);
        }

        //handle menu search
        public int CountNumberAppearText(string data, string keyWord)
        {
            int tempCount = 0;
            //if keyword == "" get every thing and do not sort
            if (keyWord == "")
                return tempCount;
            for (int i = 0; i < data.Length; i++)
            {
                var temString = data.Substring(i);
                if (temString.ToLower().StartsWith(keyWord.ToLower()))
                    tempCount++;
            }
            return tempCount;
        }
        public void ConvertMenu(MenuBO menu, List<QuickLink> listTemp, string keyWord, string parentName, string serverPath, int ancestorMenuId, bool isReportMenu)
        {
            serverPath += (serverPath == "") ? menu.Name : " > " + menu.Name;
            if (!menu.Url.Contains("N/A"))
            {
                //compose to score search
                string strCompose = menu.Name + " " + serverPath; //+ " " + menu.Url;
                if (strCompose.ToLower().Contains(keyWord.ToLower()))
                {
                    listTemp.Add(new QuickLink
                    {
                        Id = menu.Id,
                        AncestorId = ancestorMenuId,
                        IsReportMenu = isReportMenu,
                        Name = menu.Name,
                        Url = menu.Url,//menu.Url.Contains("../") ? menu.Url.Replace("../", "") : menu.Url,
                        NumOfAppear = CountNumberAppearText(strCompose, keyWord),
                        ParentName = parentName,
                        ServerPath = serverPath, //menu.Url.Replace(".aspx","").Replace(".cshtml","")
                        Target = menu.Target
                    });
                }
            }
            foreach (var item in menu.SubMenu)
            {
                ConvertMenu(item, listTemp, keyWord, menu.Name, serverPath, ancestorMenuId, isReportMenu);
            }

        }
        public List<QuickLink> GetSearchMenu(string keyWord, bool isSuggestion, List<MenuBO> listUserMenu, List<MenuBO> listReportMenu)
        {
            List<QuickLink> listTemp = new List<QuickLink>();
            try
            {
                if (listUserMenu.Count == 0 && listReportMenu.Count == 0)
                    return listTemp;
                foreach (var menu in listUserMenu)
                {
                    ConvertMenu(menu, listTemp, keyWord, "", "", menu.Id, false);
                }
                foreach (var menu in listReportMenu)
                {
                    ConvertMenu(menu, listTemp, keyWord, "", "Report", menu.Id, true);
                }
                //sort result
                if (keyWord != "" && listTemp.Count > 1)
                {
                    listTemp.Sort(delegate(QuickLink menuA, QuickLink menuB)
                    {
                        return menuB.NumOfAppear.CompareTo(menuA.NumOfAppear);
                    });
                }
                //handle select 10 items for suggestion
                if (isSuggestion == true)
                {
                    return listTemp.Skip(0).Take(10).ToList();
                }
                return listTemp;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return listTemp;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using DNAide.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.Business
{
    //using Sunplus.Core.LoggingLib;

    /// <summary>
    /// Summary description for MenuManager
    /// </summary>
    /// 
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MenuManager : IMenuManager
    {
        private readonly IUserDAO _userDAO;

       

        public MenuManager(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        #region IMenuManager Members

        public List<MenuBO> GetMenuList()
        {
            return AppSession.UserMenu;
        }

        public void GetUserMenu()
        {
            AppSession.UserMenu = _userDAO.GetUserMenu(AppSession.AccId);
            //AppSession.UserMenu = _userDAO.GetReportMenu(AppSession.AccId, WebSettings.SystemName);
        }

        public List<MenuBO> ManageMenuGetMenuList(string system)
        {
            return _userDAO.ManageMenuGetMenuList(system);
        }


        public Result InsMenuItem(MenuBO menu)
        {
            return _userDAO.InsMenuItem(menu);
        }

        public Result UdpMenuItem(MenuBO menu)
        {
            return _userDAO.UdpMenuItem(menu);
        }

        public Result DelMenuItem(int menuId)
        {
            return _userDAO.DelMenuItem(menuId);
        }

        public List<MenuBO> GetGroupFunc()
        {
            return _userDAO.GetGroupFunc();
        }

        public void GetReportMenu(string system)
        {
            AppSession.ReportMenu = _userDAO.GetReportMenu(AppSession.AccId, system);
        }

        public void GetBAMenu()
        {
            AppSession.BAMenu = _userDAO.GetReportMenu(AppSession.AccId, "BA");
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.WcfServiceLibrary.Menu
{
    //using Sunplus.Core.LoggingLib;

    /// <summary>
    /// Summary description for MenuManager
    /// </summary>
    /// 
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MenuService : IMenuService
    {
        public List<MenuBO> GetMenuList()
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
            return menuService.GetMenuList();
        }

        public List<MenuBO> ManageMenuGetMenuList(string system)
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
            return menuService.ManageMenuGetMenuList(system);
        }

        public Result InsMenuItem(MenuBO menu)
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
            return menuService.InsMenuItem(menu);
        }

        public Result UdpMenuItem(MenuBO menu)
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
            return menuService.UdpMenuItem(menu);
        }

        public Result DelMenuItem(int menuId)
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
            return menuService.DelMenuItem(menuId);
        }

        public List<MenuBO> GetGroupFunc()
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
            return menuService.GetGroupFunc();
        }

        public void GetUserMenu()
        {
            var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
             menuService.GetUserMenu();
        }

        public List<MenuBO> GetReportMenu()
        {
            List<MenuBO> menu = (List<MenuBO>)AppSession.ReportMenu;
            return menu;
        }
    }
}
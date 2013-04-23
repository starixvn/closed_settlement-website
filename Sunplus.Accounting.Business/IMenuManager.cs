using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for IMenuManager
    /// </summary>
    /// 
    [ServiceContract]
    public interface IMenuManager
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<MenuBO> GetMenuList();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<MenuBO> ManageMenuGetMenuList(string system);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result InsMenuItem(MenuBO menu);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UdpMenuItem(MenuBO menu);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result DelMenuItem(int menuId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<MenuBO> GetGroupFunc();

        void GetUserMenu();

        void GetReportMenu(string system);
        void GetBAMenu();
    }
}
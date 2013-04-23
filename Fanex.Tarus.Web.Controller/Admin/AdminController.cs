namespace Fanex.Tarus.Web.Controllers.Admin
{
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;

    public class AdminController : BaseController
    {
        //
        // GET: /Admin/
        private IAdminService _adminService;

        public AdminController()
            : this(null)
        {
            _adminService = new AdminService();
        }

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService ?? new AdminService();
        }

        
    }
}

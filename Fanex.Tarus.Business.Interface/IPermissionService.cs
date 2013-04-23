namespace Fanex.Tarus.Business.Interface
{
    using System.Collections.Generic;

    public interface IPermissionService
    {
        bool HasPermission(string actionPath, Dictionary<string, int> pageAccess);

        bool GetHostPermission<T>(T function, Dictionary<string, int> roleAccess);

        bool GetHostPermission(string actionPath, Dictionary<string, int> pageAccess);
    }
}
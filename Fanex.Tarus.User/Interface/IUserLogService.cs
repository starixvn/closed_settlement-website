using Fanex.Tarus.User.Entities;
using System;
using System.Collections.Generic;
namespace Fanex.Tarus.User.Business.Interface
{
    interface IUserLogService
    {
        object UserControlLogSelect(DateTime fromDate, DateTime toDate, int type, string userName, int pageIndex, int pageSize);
    }
}

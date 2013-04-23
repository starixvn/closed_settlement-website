namespace Fanex.Tarus.User.Business.Services
{
    using Fanex.Tarus.User.Business.Interface;
    using Fanex.Tarus.User.DataAccess;
    using System.Collections.Generic;
    using Fanex.Tarus.User.Entities;
    using System;
    using System.Linq;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Entities;
    using FunctionBO = Fanex.Tarus.User.Entities.FunctionBO;
    using Sunplus.Accounting.Core.Common;
    using Fanex.Tarus.User.DataAccess.Sql;

    public class FunctionService : IFunctionService
    {
        private readonly IFunctionAccess _functionAccess;

        public FunctionService()
        {
            _functionAccess = new FunctionAccess();
        }

        public List<FunctionBO> GetAccountingFunctionList()
        {
            try
            {
                var listEntity = _functionAccess.GetFunctionList().OrderBy(f => f.Name).ToList();
                Sort(listEntity);
                return listEntity;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<FunctionBO>();
            }
        }

        private void Sort(IEnumerable<FunctionBO> listEntity)
        {
            foreach (var functionBo in listEntity.Where(functionBo => functionBo.TotalSub > 0))
            {
                functionBo.SubFuncs = functionBo.SubFuncs.OrderBy(f => f.Name).ToList();
                Sort(functionBo.SubFuncs);
            }
        }

        public List<FunctionBO> GetFunctionParentList(string systemName)
        {
            try
            {
                return _functionAccess.GetFunctionParentList(systemName).OrderBy(f => f.Name).ToList();
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<FunctionBO>();
            }
        }

        public Result EditFunction(int funcId, string name, string desc, string fileNames, string parent, string systemName)
        {
            try
            {
                return _functionAccess.UpdateFunction(funcId, AppSession.AccId, name, desc, fileNames, systemName, parent);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result AddFunction(string name, string desc, string fileNames, string parent, string systemName)
        {
            try
            {
                return _functionAccess.InsertFunction(name, desc, fileNames, parent, AppSession.AccId, systemName);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result DeleteFunction(int funcId)
        {
            try
            {
                return _functionAccess.DeleteFunction(funcId, AppSession.AccId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Tarus.Entities.Customer.DepositCustomer;

using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.Business.Implement.Customer
{
    using Interface.Customer;
    using DataAccess.Implement.Customer;
    using DataAccess.Interface.Customer;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Entities.BetAggregator;
    using DNAide.Core;
   
    public class DepositCustomerService: BaseService, IDepositCustomerService
    {

        private static IDepositCustomerAccess _depositCustomerAccess;

        public DepositCustomerService(IDepositCustomerAccess depositCustomerAccess)
        {
            _depositCustomerAccess = depositCustomerAccess ?? new DepositCustomerAccess();
        }

        public DepositCustomerService():this(null)
        {}

        public Result NewLicenseeAddNewSuper(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.NewLicenseeAddNewSuper(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result NewLicenseeAddNewMaster(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.NewLicenseeAddNewMaster(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result NewLicenseeAddNewAgent(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.NewLicenseeAddNewAgent(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result NewLicenseeAddNewMember(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.NewLicenseeAddNewMember(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public string GetSuperSetting(int custId)
        {
            try
            {
                var ds = _depositCustomerAccess.GetSuperSetting(custId);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return string.Empty;
                var tbl = ds.Tables[0];
                var row = ds.Tables[0].Rows[0];
                return BuildSettingJsonDataString(row, tbl);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public string GetMasterSetting(int custId)
        {
            try
            {
                var ds = _depositCustomerAccess.GetMasterSetting(custId);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return string.Empty;
                var tbl = ds.Tables[0];
                var row = ds.Tables[0].Rows[0];
                return BuildSettingJsonDataString(row, tbl);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public string GetAgentSetting(int custId)
        {
            try
            {
                var ds = _depositCustomerAccess.GetAgentSetting(custId);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return string.Empty;
                var tbl = ds.Tables[0];
                var row = ds.Tables[0].Rows[0];
                return BuildSettingJsonDataString(row, tbl);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        public string GetMemberSetting(int custId)
        {
            try
            {
                var ds = _depositCustomerAccess.GetMemberSetting(custId);
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return string.Empty;
                var tbl = ds.Tables[0];
                var row = ds.Tables[0].Rows[0];
                return BuildSettingJsonDataString(row, tbl);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return string.Empty;
            }
        }

        private string BuildSettingJsonDataString(DataRow row, DataTable tbl)
        {
            var listProp = new List<string>();
            for (var i = 0; i < tbl.Columns.Count; i++)
            {
                var columName = tbl.Columns[i].ColumnName;
                listProp.Add("\"" + columName.ToLower() + "\":\"" + DNAide.Core.CastAide.AsString(row[columName]) + "\"");
            }
            return "{" + string.Join(",", listProp.ToArray()) + "}";
        }

        public Result UpdateSuperSetting(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.UpdateSuperSetting(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result UpdateMasterSetting(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.UpdateMasterSetting(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result UpdateAgentSetting(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.UpdateAgentSetting(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }

        public Result UpdateMemberSetting(NewLicenseePostParam newLicenseePostParam)
        {
            try
            {
                return _depositCustomerAccess.UpdateMemberSetting(newLicenseePostParam);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new Result(1, ex.Message);
            }
        }


        public List<LicenseeConfig> SelAllLicensee()
        {
            try
            {
                var table = _depositCustomerAccess.SelAllLicensee();
                if (table == null) return new List<LicenseeConfig>();
                var listEntity = new List<LicenseeConfig>();
                foreach (DataRow row in table.Rows)
                {
                    listEntity.Add(new LicenseeConfig 
                    {
                        Id = CastAide.AsInt32(row["licenseeid"]),
                        Name = CastAide.AsString(row["licenseename"])
                    });
                }
                return listEntity;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return new List<LicenseeConfig>();
            }
        }
    }
}

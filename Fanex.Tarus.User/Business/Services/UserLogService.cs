using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.User.Business.Services
{
    using Fanex.Tarus.User.Business.Interface;
    using Fanex.Tarus.User.Entities;
    using Fanex.Tarus.User.DataAccess;
    using Fanex.Tarus.User.DataAccess.Sql;
    using Sunplus.Accounting.Core.Extensions;
    using System.Data;
    using DNAide.Core;

    public class UserLogService : IUserLogService
    {
        private readonly IUserLogAccess _userLogAccess;

        public UserLogService()
        {
            _userLogAccess = new UserLogAccess();
        }

        public object UserControlLogSelect(DateTime fromDate, DateTime toDate, int type, string userName, int pageIndex, int pageSize)
        {
            var defaultObject = new { Logs = new List<UserLog>(), RowsTotal = 0, Message = string.Empty };
            try
            {
                string message = string.Empty;
                var custId = _userLogAccess.GetNameFromId(type, userName, out message);
                if (custId == -1) return new { Logs = new List<UserLog>(), RowsTotal = 0, Message = message };
                
                toDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);
                var table = _userLogAccess.UserControlLogSelect(fromDate, toDate, type, custId, userName, pageIndex, pageSize);
                if (table == null) return defaultObject;
                var listEntity = new List<UserLog>();
                foreach (DataRow row in table.Rows)
                {
                    var userLog = new UserLog
                    {
                        LogId = CastAide.AsInt32(row["logid"]),
                        TypeId = CastAide.AsInt32(row["typeid"]),
                        LogDate = CastAide.AsDateTime(row["logdate"]).ToString("yyyy-MM-dd hh:mm tt"),
                        AccId = CastAide.AsInt32(row["custid"]),
                        AccName = CastAide.AsString(row["custname"]),
                        UserId = CastAide.AsInt32(row["userid"]),
                        UserName = CastAide.AsString(row["username"]),
                        ActionId = CastAide.AsInt32(row["actionid"]),
                        ActionName = CastAide.AsString(row["actionname"]),
                        Doing = CastAide.AsString(row["doing"])
                    };

                    if (string.IsNullOrWhiteSpace(userLog.AccName)) // group was deleted
                    {
                        //get AccId, AccName from "doing" field
                        var groupName = userLog.Doing.Split(new string[] { "<GroupName>", "</GroupName>" }, StringSplitOptions.None);
                        if (groupName.Length >= 2)
                        {
                            userLog.AccName = groupName[1];
                        }
                    }

                    switch (userLog.ActionId)
                    {
                        case 1708:
                            userLog.GroupInfo = ParseLogGroupInfo(userLog.Doing);
                            break;
                        case 1709:
                            if (userLog.TypeId == 0)
                                userLog.GroupPolicy = ParseLogGroupPolicy(userLog.Doing);
                            else
                                userLog.GroupPolicy = ParseLogAdminGroupPolicy(userLog.Doing);
                            break;
                        case 1700:
                            userLog.GroupPolicy = ParseLogChangeGroupPolicy(userLog.Doing);
                            break;
                        case 1704:
                            userLog.AccountInfo = ParseLogAccountInfo(userLog.Doing);
                            break;
                        case 1703:
                            userLog.AdminPolicy = ParseLogAdminPolicy(userLog.Doing);
                            break;
                        case 1701:
                        case 1702:
                            var toGroup = userLog.Doing.Split(new string[] { "<ToGroup>", "</ToGroup>" }, StringSplitOptions.None);
                            userLog.ToGroup = toGroup.Length >= 2 ? toGroup[1] : string.Empty;
                            if (userLog.ActionId == 1701)
                            {
                                userLog.NewAccountInfo = ParseNewAccountInfo(userLog.Doing);
                            }
                            break;
                        case 1705:
                            userLog.Status = ParseLogChangeStatus(userLog.Doing);
                            break;
                        case 1707:
                            userLog.GroupDescription = ParseGroupDescription(userLog.Doing);
                            break;
                    }
                    listEntity.Add(userLog);
                }
                return new { Logs = listEntity, RowsTotal = _userLogAccess.GetPagesTotal(fromDate, toDate, type, custId, userName), Message = string.Empty };
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return defaultObject;
            }
        }

        private object ParseNewAccountInfo(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return null;
            return new 
            {
                FirstName = doing.Split(new string[] { "<FirstName>", "</FirstName>" }, StringSplitOptions.None)[1],
                LastName = doing.Split(new string[] { "<LastName>", "</LastName>" }, StringSplitOptions.None)[1],
                ToGroup = doing.Split(new string[] { "<ToGroup>", "</ToGroup>" }, StringSplitOptions.None)[1]
            };
        }

        private string ParseGroupDescription(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return string.Empty;
            var groupDesc = doing.Split(new string[] { "<GroupDesc>", "</GroupDesc>" }, StringSplitOptions.None);
            return groupDesc[1];
        }

        private LogGroupInfo ParseLogGroupInfo(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return new LogGroupInfo();
            var groupName = doing.Split(new string[] { "<Groupname>", "</Groupname>" }, StringSplitOptions.None);
            var groupNameOld = groupName[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var groupNameNew = groupName[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            var description = doing.Split(new string[] { "<GroupDescription>", "</GroupDescription>" }, StringSplitOptions.None);
            var descriptionOld = description[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var descriptionNew = description[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);
            return new LogGroupInfo
            {
                GroupNameOld = groupNameOld[1],
                GroupNameNew = groupNameNew[1],
                DescriptionOld = descriptionOld[1],
                DescriptionNew = descriptionNew[1]
            };
        }

        private LogAccountInfo ParseLogAccountInfo(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return new LogAccountInfo();
            var firstName = doing.Split(new string[] { "<FirstName>", "</FirstName>" }, StringSplitOptions.None);
            var firstNameOld = firstName[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var firstNameNew = firstName[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            var lastName = doing.Split(new string[] { "<LastName>", "</LastName>" }, StringSplitOptions.None);
            var lastNameOld = lastName[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var lastNameNew = lastName[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            return new LogAccountInfo
            {
                FirstNameOld = firstNameOld[1],
                FirstNameNew = firstNameNew[1],
                LastNameOld = lastNameOld[1],
                LastNameNew = lastNameNew[1]
            };
        }

        private LogGroupPolicy ParseLogGroupPolicy(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return new LogGroupPolicy();
            var permission = doing.Split(new string[] { "<Permission>", "</Permission>" }, StringSplitOptions.None);
            var permissionOld = permission[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var permissionNew = permission[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            var members = doing.Split(new string[] { "<Members>", "</Members>" }, StringSplitOptions.None);
            var listMember = members[1].Split(';').ToList();
            var listAccount = new List<Account>();
            foreach (string member in listMember)
            {
                if (member != "")
                {
                    listAccount.Add(new Account { UserName = member });
                }
            }
            return new LogGroupPolicy
            {
                PoliciesOld = permissionOld[1],
                PoliciesNew = permissionNew[1],
                Members = listAccount
            };
        }

        private LogAdminPolicy ParseLogAdminPolicy(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return new LogAdminPolicy();
            var permission = doing.Split(new string[] { "<Permission>", "</Permission>" }, StringSplitOptions.None);
            var permissionOld = permission[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var permissionNew = permission[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            return new LogAdminPolicy
            {
                PoliciesOld = permissionOld[1],
                PoliciesNew = permissionNew[1]
            };
        }

        private LogGroupPolicy ParseLogAdminGroupPolicy(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return new LogGroupPolicy();
            var permission = doing.Split(new string[] { "<Permission>", "</Permission>" }, StringSplitOptions.None);
            var permissionOld = permission[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var permissionNew = permission[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            var groupId = doing.Split(new string[] { "<GroupID>", "</GroupID>" }, StringSplitOptions.None);
            var groupName = doing.Split(new string[] { "<GroupName>", "</GroupName>" }, StringSplitOptions.None);

            return new LogGroupPolicy
            {
                PoliciesOld = permissionOld[1],
                PoliciesNew = permissionNew[1],
                GroupId = CastAide.AsInt32(groupId[1]),
                GroupName = groupName[1]
            };
        }

        private LogGroupPolicy ParseLogChangeGroupPolicy(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return new LogGroupPolicy();
            var permission = doing.Split(new string[] { "<Permission>", "</Permission>" }, StringSplitOptions.None);
            var permissionOld = permission[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var permissionNew = permission[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            var groupName = doing.Split(new string[] { "<GroupName>", "</GroupName>" }, StringSplitOptions.None);
            var groupNameOld = groupName[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var groupNameNew = groupName[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            var groupId = doing.Split(new string[] { "<GroupID>", "</GroupID>" }, StringSplitOptions.None);
            var groupIdOld = groupId[1].Split(new string[] { "<Old>", "</Old>" }, StringSplitOptions.None);
            var groupIdNew = groupId[1].Split(new string[] { "<New>", "</New>" }, StringSplitOptions.None);

            return new LogGroupPolicy
            {
                PoliciesOld = permissionOld[1],
                PoliciesNew = permissionNew[1],
                GroupNameOld = groupNameOld[1],
                GroupNameNew = groupNameNew[1],
                GroupIdOld = CastAide.AsInt32(groupIdOld[1]),
                GroupIdNew = CastAide.AsInt32(groupIdNew[1])
            };
        }

        private string ParseLogChangeStatus(string doing)
        {
            if (string.IsNullOrWhiteSpace(doing)) return string.Empty;
            var status = doing.Split(new string[] { "<Status>", "</Status>" }, StringSplitOptions.None);
            return status[1];
        }
    }
}

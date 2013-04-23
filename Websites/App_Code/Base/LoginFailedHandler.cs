using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Runtime.Serialization;

using Fanex.Tarus.Business.Implement;

using Sunplus.Data.DataAbstract.Storage.Cache;
using DNAide.Core;



/// <summary>
/// Summary description for LoginFailedHandler
/// </summary>
public class LoginFailedHandler
{
    private static ICache _cache;

    
    /// <summary>
    /// Before pass info through into DB, we will invoke this function to check "Reach max times wrong password."
    /// Get info of this username from the caching server.
    /// If (this username is being locked)//Reach max times wrong password.
    /// {
    ///     Lock UI
    ///     Show count down time in web page.
    /// }
    /// else
    /// {
    ///     Normal process(pass info throug int DB)
    /// }
    /// </summary>
    /// <param name="username"></param>
    public static bool IsLocked(string username)
    {
        return CheckReachMaxLoginTimes(username);
    }

    /// <summary>
    /// User login success.
    /// Remove all log info about "failed login previous" in the caching server.
    /// </summary>
    /// <param name="username">username</param>
    public static void LoginSuccess(string username)
    {
        RemoveLoginInformation(username);
    }

    /// <summary>
    /// User login failed.
    /// If (username is existed in Database)
    /// {
    ///     If ("log failed info" of this user has existed on the caching server)
    ///     {
    ///         Increase counter, update logtime
    ///     }
    ///     Else
    ///     {
    ///         Add "log failed info" of this user into the caching server.
    ///     }
    ///  }
    ///  Else//User is not exist in Database
    ///  {
    ///  }
    /// </summary>
    /// <param name="username"></param>
    public static void LoginFailed(string username, int ErrorCode)
    {
        if (ErrorCode == 103)//User is existed in database
        {
            IncreaseCount(username);
        }
        else if (ErrorCode == 101) //User is not exist in database
        {

        }
    }

    private static LogFailedInformation GetInformation(string username)
    {
        return CacheService.Get<LogFailedInformation>(GetKey(username));
    }

    private static string GetKey(string username)
    {
        return "login_failed_" + username.ToLower();
    }

    private static void AddLoginInformation(string username)
    {
        CacheService.Set(
            GetKey(username),
            new LogFailedInformation() { UserName = username, Count = 1, LoginTime = DateTime.Now },
            new TimeSpan(0, 100, 0));
    }

    private static void IncreaseCount(string username)
    {
        if (GetInformation(username) == null)
        {
            AddLoginInformation(username);
        }
        else
        {
            CacheService.Set(GetKey(username), new LogFailedInformation()
            {
                UserName = username
                ,
                Count = GetCount(username) + 1
                ,
                LoginTime = DateTime.Now
                ,
                UnLockTime = DateTime.Now.AddMinutes(CastAide.AsInt32(ConfigurationManager.AppSettings["loginfailed_countdown_minute"]))
            });
        }
    }

    private static bool CheckReachMaxLoginTimes(string username)
    {
        LogFailedInformation info = GetInformation(username);
        if (info != null && info.Count == CastAide.AsInt32(ConfigurationManager.AppSettings["NumberWrongPassword"]))
        {
            if (info.UnLockTime < DateTime.Now)
            {
                RemoveLoginInformation(username);
                return false;
            }
            return true;
        }
        return false;
    }

    private static bool RemoveLoginInformation(string username)
    {
        CacheService.Delete(GetKey(username));
        return true;
    }

    private static int GetCount(string username)
    {
        LogFailedInformation info = GetInformation(username);
        if (info == null) return 0;
        return info.Count;
    }

    public static string GetRemainingTime(string username)
    {
        if (string.IsNullOrEmpty(username)) return string.Empty;
        var info = GetInformation(username);
        if (info != null)
        {
            TimeSpan timeSpan = info.UnLockTime - DateTime.Now;
            return timeSpan.Minutes + ";" + timeSpan.Seconds;
        }
        return string.Empty;
    }

    public static void Test(string username)
    {
        CacheService.Set(
            GetKey(username),
            new LogFailedInformation()
                {
                    UserName = username,
                    Count = CastAide.AsInt32(ConfigurationManager.AppSettings["NumberWrongPassword"]),
                    LoginTime = DateTime.Now,
                    UnLockTime = DateTime.Now.AddMinutes(1000)
                });
    }

    public static string WriteJS(string username)
    {
        var strDownTime = LoginFailedHandler.GetRemainingTime(username);
        string minute = strDownTime.Split(';')[0];
        string second = strDownTime.Split(';')[1];
        string message = "You have entered wrong user name or password 6 times.<br/><div>Please try again after 5 minutes (<span id=\"spanDownTime\"></span>).</div>";
        return @"
                var minute = " + minute + @";
                var second = " + second + @";
                var html = '" + message + @"';
                parent.document.getElementById('ErrorMessage').innerHTML = html;
                parent.document.getElementById('ErrorMessage').style.visibility = 'visible';
                var timerID = setInterval(function() {
                    if (second <= 0) {
                        if (minute <= 0) {
                            clearInterval(timerID);
                            parent.document.getElementById('TxtPassword').value = '';
                            parent.document.getElementById('ErrorMessage').style.visibility = 'hidden';
                        }
                        else {
                            --minute;
                            second = 60;
                            parent.document.getElementById('TxtPassword').value = '';
                            parent.document.getElementById('spanDownTime').innerHTML = minute + ' : ' + second;
                        }
                    }
                    else {
                        --second;
                        parent.document.getElementById('TxtPassword').value = '';
                        parent.document.getElementById('spanDownTime').innerHTML = minute + ' : ' + second;
                    }
                }, 999);";
    }
}

[DataContract]
[Serializable]
public class LogFailedInformation
{
    [DataMember]
    public int LogID { get; set; }

    [DataMember]
    public string UserName { get; set; }

    [DataMember]
    public DateTime LoginTime { get; set; }

    [DataMember]
    public int Count { get; set; }

    [DataMember]
    public DateTime UnLockTime { get; set; }
}

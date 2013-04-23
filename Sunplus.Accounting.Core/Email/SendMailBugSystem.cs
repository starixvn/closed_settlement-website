using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.Core.Email
{
    /// <summary>
    /// Summary description for SendMailBugSystem
    /// </summary>
    public class SendMailBugSystem
    {
        public SendMailBugSystem()
        {
        
        }

        public static void NotificationBugSystem(string msgContent)
        {
            string subject = "The Accounting system has encountered a problem";
            string content = "<span style='color:red'>" + msgContent + "</span>";
            DNAide.Utils.SendMailService.SendMail2(MailSection.MailServer, MailSection.ToMailBugSystem, MailSection.FromMail, MailSection.CCMailBugSystem, MailSection.BCCMailBugSystem, subject, content);
        }
    }
}

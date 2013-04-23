using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.Mail;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Newtonsoft.Json;
using MailMessage = System.Net.Mail.MailMessage;
using MailPriority = System.Net.Mail.MailPriority;


namespace Sunplus.Accounting.Core.Extensions
{
    using Fanex.MailDeliver.MailClient.API;
    using Fanex.MailDeliver.Model;

    public class SendMailExtension
    {
        #region private variables
        private static BackgroundWorker _worker = null;
        private static Queue<MailSend> _queueMail = new Queue<MailSend>();
        private static bool _sendMailStatus = true;
        #endregion
        /**
         * Accounting - Send mail & notification
         **/
        public static bool AccSendMail(MailSection.HostName hostName, string to, string cc, string mailTitle, string subject, string body)
        {
            return AccSendMail(hostName, to, cc, mailTitle, subject, body, null);
        }
        public static bool AccSendMail(MailSection.HostName hostName, string to, string cc, string mailTitle, string subject, string body, MailAttachment mailAttachment)
        {
            return AccSendMail(hostName, to, cc, string.Empty, mailTitle, subject, body, mailAttachment, null);
        }
        public static bool AccSendMail(MailSection.HostName hostName, string to, string cc, string bcc, string mailTitle, string subject, string body, MailAttachment mailAttachment)
        {
            return AccSendMail(hostName, to, cc, bcc, mailTitle, subject, body, mailAttachment, null);
        }
        public static bool AccSendMail(MailSection.HostName hostName, string to, string cc, string bcc, string mailTitle, string subject, string body, MailAttachment mailAttachment, AlternateView altView)
        {
            try
            {

                var mailSend = new MailSend()
                {
                    HostName = hostName,
                    To = to,
                    Cc = cc,
                    Bcc = bcc,
                    Title = mailTitle,
                    Subject = subject,
                    Body = body,
                    Attachment = mailAttachment,
                    AltView = altView
                };


                bool IsLocal = System.Configuration.ConfigurationManager.AppSettings["LocalTest"] == "1";

                if (IsLocal)
                {
                    var mailSetting = MailSection.Instance[MailSection.Setting.Server][MailSection.Instance.Active];
                    bool f = SendEmail3rdParty.SendMail(subject, subject, body, string.Empty, to, cc, bcc, mailSetting.Username, mailSetting.Password);
                    return f;
                }
                else
                {
                    _queueMail.Enqueue(mailSend);

                    using (_worker = new BackgroundWorker())
                    {
                        _worker.WorkerSupportsCancellation = true;
                        _worker.DoWork += new DoWorkEventHandler(WorkerRunning);
                        _worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(WorkerCompleted);
                        _worker.RunWorkerAsync();
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return false;
        }
        /**
         * Function: Backgroundworker running
         */
        private static void WorkerRunning(object sender, DoWorkEventArgs e)
        {
            if (_queueMail.Count > 0)
            {
                var queueMail = _queueMail.Dequeue();
                e.Result = SendMail(queueMail);
            }
            else
            {
                _worker.CancelAsync();
            }
        }
        /**
         * Function: Backgroundworker commpleted
         */
        private static void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _sendMailStatus = (bool)e.Result;
        }

        #region send mail function
        public static bool SendMail_Old(MailSend mailSend)
        {
            try
            {
                //var mailSetting = MailSection.Instance[MailSection.Setting.Server][mailSend.HostName.ToString()];
                var mailSetting = MailSection.Instance[MailSection.Setting.Server][MailSection.Instance.Active];

                var msg = new MailMessage();

                #region to

                if (!string.IsNullOrEmpty(mailSend.To))
                {
                    mailSend.To.Split(';').ForEach<string>(toMail =>
                    {
                        msg.To.Add(toMail);
                    });
                }

                #endregion to

                msg.From = new MailAddress(mailSetting.Username, mailSend.Title, Encoding.UTF8);

                #region cc

                if (!string.IsNullOrEmpty(mailSend.Cc))
                {
                    mailSend.Cc.Split(';').ForEach<string>(ccMail =>
                    {
                        msg.CC.Add(ccMail);
                    });
                }

                #endregion

                #region bcc
                if (!string.IsNullOrEmpty(mailSend.Bcc))
                {
                    mailSend.Bcc.Split(';').ForEach<string>(bccMail => msg.Bcc.Add(bccMail));
                }

                #endregion
                msg.Subject = mailSend.Subject;
                msg.SubjectEncoding = Encoding.UTF8;

                msg.IsBodyHtml = true;
                msg.Body = mailSend.Body;
                msg.BodyEncoding = Encoding.UTF8;

                var attachFile = mailSend.Attachment;
                if (attachFile != null && attachFile.Content != null)
                {
                    attachFile.Content.Seek(0, SeekOrigin.Begin);
                    var attachment = new Attachment(attachFile.Content, attachFile.FileName, attachFile.ContentType);
                    msg.Attachments.Add(attachment);
                }

                if (mailSend.AltView != null)
                {
                    msg.AlternateViews.Add(mailSend.AltView);
                }
                msg.Priority = MailPriority.High;

                var client = new SmtpClient
                {
                    Port = mailSetting.Port,
                    Host = mailSetting.Host
                };
                if (mailSetting.Auth)
                {
                    client.Credentials = new System.Net.NetworkCredential(mailSetting.Username, mailSetting.Password);
                    client.EnableSsl = true;
                }
                client.Send(msg);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return false;
        }
        public static bool SendMail(MailSend mailSend)
        {
            try
            {
                using (var client = new MailDeliver())
                {
                    var mailItem = new Fanex.MailDeliver.Model.MailRecord 
                    {
                        MailTo = mailSend.To,
                        MailCc = mailSend.Cc,
                        MailBcc = mailSend.Bcc,
                        Title = mailSend.Title,
                        Subject = mailSend.Subject,
                        Body = mailSend.Body
                    };
                    if (mailSend.Attachment != null && mailSend.Attachment.FileName != null)
                    {
                        mailItem.Attachments = new List<Fanex.MailDeliver.Model.MailAttachment>();
                        var attItem = new Fanex.MailDeliver.Model.MailAttachment
                        {
                            FileName = mailSend.Attachment.FileName,
                            Content = mailSend.Attachment.Content
                        };
                        if (mailSend.Attachment.ContentType.ToLower().IndexOf("excel") != -1)
                        {
                            attItem.ContentType = FileExtentions.MS_EXCEL;
                        }
                        else if (mailSend.Attachment.ContentType.ToLower().IndexOf("doc") != -1)
                        {
                            attItem.ContentType = FileExtentions.MS_DOC;
                        }
                        else
                        {
                            attItem.ContentType = FileExtentions.IMG_GIF;
                        }
                        mailItem.Attachments.Add(attItem);
                    }
                    if (mailSend.AltView != null)
                    {
                        mailItem.AlternateView = new List<MailLinkedResource>();
                        var linkedResource = mailSend.AltView.LinkedResources;
                        foreach (var linked in linkedResource)
                        {
                            var stream = linked.ContentStream;
                            byte[] buffer = new byte[stream.Length];
                            stream.Read(buffer, 0, buffer.Length);
                            var resource = new MailLinkedResource
                            {
                                FileName = string.Empty,
                                ImageType = FileExtentions.IMG_GIF,
                                ContentCID = linked.ContentId,
                                StreamContent = new MemoryStream(buffer)
                            };
                            mailItem.AlternateView.Add(resource);
                        }
                    }
                    // send mail
                    client.SendMail(mailItem);
                }
                
                return true;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return false;
        }
        #endregion
    }
    public class MailSend
    {
        public MailSection.HostName HostName { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public MailAttachment Attachment { get; set; }
        public AlternateView AltView { get; set; }

    }

    public class MailAttachment
    {
        public MemoryStream Content { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }
}

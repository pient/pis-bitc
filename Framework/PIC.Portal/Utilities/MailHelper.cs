using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Mail;
using PIC.Portal.Model;

namespace PIC.Portal.Utilities
{
    /// <summary>
    /// 邮件发送
    /// </summary>
    public class MailHelper
    {
        public static void SysSend(IList<MailMessage> messages)
        {

        }

        /// <summary>
        /// 发送系统邮件
        /// </summary>
        /// <param name="mailAddresses"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        public static void SysSend(IList<string> mailAddresses, string subject, string htmlBody)
        {
            if (mailAddresses.Count == 0)
            {
                return;
            }

            var cfg = GetMailConfig();

            MailMessage message = new MailMessage();

            message.From = cfg.Get<String>("Address");

            foreach (var rvr in mailAddresses)
            {
                message.To.Add(rvr);
            }

            message.Subject = subject;
            message.HtmlBody = htmlBody;

            SmtpClient client = new SmtpClient();
            client.Host = cfg.Get<String>("SmtpHost");
            client.Username = cfg.Get<String>("Username");
            client.Password = cfg.Get<String>("Password");
            client.Port = cfg.Get<int>("Port", 25);

            client.Send(message);

            // 发送系统邮件计入日志
            LogContent logContent = new LogContent();
            logContent.Set("Receivers", JsonHelper.GetJsonString(mailAddresses));
            logContent.Set("Subject", subject);
            LogService.Log(logContent, "SysMail");
        }

        /// <summary>
        /// 发送系统邮件
        /// </summary>
        /// <param name="receiver"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        public static void SysSend(string mailAddress, string subject, string htmlBody)
        {
            var rvrs = new List<string>();
            rvrs.Add(mailAddress);

            SysSend(rvrs, subject, htmlBody);
        }

        /// <summary>
        /// 根据用户名发送系统邮件
        /// </summary>
        /// <param name="receivers"></param>
        /// <param name="subject"></param>
        /// <param name="htmlBody"></param>
        public static void SysSendByUserIds(IList<string> receiverIds, string subject, string htmlBody)
        {
            var reveivers = OrgUser.FindAllByPrimaryKeys(receiverIds.ToArray());

            var mailAddresses = reveivers.Where(ent=>!String.IsNullOrWhiteSpace(ent.Email)).Select(ent => ent.Email).Distinct().ToList();

            SysSend(mailAddresses, subject, htmlBody);
        }

        /// <summary>
        /// 获取邮件配置
        /// 格式：{ Address:"Address", SmtpHost:"SmtpHost", Username:"Username", Password:"Password", Port:25 }
        /// </summary>
        /// <returns></returns>
        public static EasyDictionary GetMailConfig()
        {
            // 发送邮件配置
            var p = Parameter.Get("System.Mail.SenderConfig");
            var cfg = JsonHelper.GetObject<EasyDictionary>(p.Value);

            return cfg;
        }
    }
}

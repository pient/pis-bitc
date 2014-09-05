using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Mail;
using NUnit.Framework;
using PIC.Portal;
using PIC.Portal.Model;

namespace PIC.Component.Testing.MsOffice
{
    [TestFixture]
    public class MailTest
    {
        [SetUp]
        public void Init()
        {
            // 初始化PortalService
            PortalService.Initialize();
        }

        [Test]
        public void MailSendTest()
        {
            MailMessage message = new MailMessage();

            // 发送邮件配置
            var p = Parameter.Get("System.Mail.SenderConfig");
            var p_dict = JsonHelper.GetObject<EasyDictionary>(p.Value);

            message.From = p_dict.Get<String>("Address");
            message.To = "nkxt@bitc.edu.cn";

            message.Subject = "测试邮件发送程序 - Ray Liu";
            message.HtmlBody = @"<html>请登录内控审批系统查看您的审批项，<a href='http://10.0.7.154:8345'>审批流程</a></html>";

            SmtpClient client = new SmtpClient();
            client.Host = p_dict.Get<String>("SmtpHost");
            client.Username = p_dict.Get<String>("Username");
            client.Password = p_dict.Get<String>("Password");
            client.Port = p_dict.Get<int>("Port", 25);

            client.Send(message);
        }
    }
}

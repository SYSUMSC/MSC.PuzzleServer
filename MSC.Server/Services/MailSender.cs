using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MSC.Server.Services.Interface;
using MSC.Server.Utils;
using NLog;
using System.Reflection;

namespace MSC.Server.Services
{
    public class MailSender : IMailSender
    {
        private readonly IConfiguration configuration;
        private static readonly Logger logger = LogManager.GetLogger("MailSender");

        public MailSender(IConfiguration _configuration)
            => configuration = _configuration;

        public async Task<bool> SendEmailAsync(string subject, string content, string to)
        {
            var username = configuration["EmailConfig:UserName"];
            var password = configuration["EmailConfig:Password"];
            var domain = configuration["EmailConfig:Domain"];
            var smtpHost = configuration["EmailConfig:Smtp:Host"];
            var smtpPort = int.Parse(configuration["EmailConfig:Smtp:Port"]);
            bool isSuccess = false;

            MimeMessage message = new();
            message.Subject = subject;
            message.From.Add(MailboxAddress.Parse(username + "@" + domain));
            message.To.Add(MailboxAddress.Parse(to));
            message.Body = new TextPart(TextFormat.Html) { Text = content };

            try
            {
                using var smtp = new SmtpClient();
                smtp.MessageSent += (sender, args) =>
                {
                    isSuccess = true;
                    LogHelper.Log(logger, "已发送邮件至" + to, "-", TaskStatus.Success);
                };
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(username + "@" + domain, password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception e)
            {
                logger.Error(e, "邮件发送遇到问题。");
                isSuccess = false;
            }

            return isSuccess;
        }

        public async void SendUrl(string title, string infomation, string btnmsg, string userName, string email, string url)
        {
            string _namespace = MethodBase.GetCurrentMethod()!.DeclaringType!.Namespace!;
            Assembly _assembly = Assembly.GetExecutingAssembly();
            string resourceName = _namespace + ".Assets.URLEmailTemplate.html";
            string emailContent = await
                new StreamReader(_assembly.GetManifestResourceStream(resourceName)!)
                .ReadToEndAsync();
            emailContent = emailContent
                .Replace("{title}", title)
                .Replace("{infomation}", infomation)
                .Replace("{btnmsg}", btnmsg)
                .Replace("{email}", email)
                .Replace("{userName}", userName)
                .Replace("{url}", url)
                .Replace("{nowtime}", DateTime.UtcNow.ToString("u"));
            if (!await SendEmailAsync(title, emailContent, email))
                LogHelper.Log(logger, "邮件发送失败！", "-", TaskStatus.Fail);
        }

        public void SendConfirmEmailUrl(string userName, string email, string confirmLink)
            => SendUrl("验证你的注册邮箱",
                "需要验证你的邮箱：" + email,
                "确认邮箱", userName, email, confirmLink);

        public void SendResetPwdUrl(string userName, string email, string resetLink)
            => SendUrl("重置密码",
                "点击下方按钮重置你的密码。",
                "重置密码", userName, email, resetLink);

        public void SendChangeEmailUrl(string userName, string email, string resetLink)
            => SendUrl("更改邮箱",
                "点击下方按钮更改你的邮箱。",
                "更改邮箱", userName, email, resetLink);

        public void SendResetPasswordUrl(string userName, string email, string resetLink)
            => SendUrl("重置密码",
                "点击下方按钮重置你的密码。",
                "重置密码", userName, email, resetLink);
    }
}
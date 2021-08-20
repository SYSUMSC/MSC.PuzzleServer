﻿namespace MSC.Server.Services.Interface
{
    public interface IMailSender
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="content">HTML内容</param>
        /// <param name="to">收件人</param>
        /// <returns>发送是否成功</returns>
        public Task<bool> SendEmailAsync(string subject, string content, string to);

        /// <summary>
        /// 发送带有链接的邮件
        /// </summary>
        /// <param name="title">邮件标题</param>
        /// <param name="infomation">邮件正文</param>
        /// <param name="btnmsg">按钮信息</param>
        /// <param name="userName">用户名</param>
        /// <param name="email">电子邮件地址</param>
        /// <param name="url">链接</param>
        public void SendUrl(string? title, string? infomation, string? btnmsg, string? userName, string? email, string? url);

        /// <summary>
        /// 发送新用户验证URL
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">用户新注册的Email</param>
        /// <param name="confirmLink">确认链接</param>
        public void SendConfirmEmailUrl(string? userName, string? email, string? confirmLink);

        /// <summary>
        /// 发送密码重置邮件
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">用户的电子邮件</param>
        /// <param name="resetLink">重置链接</param>
        public void SendResetPwdUrl(string? userName, string? email, string? resetLink);

        /// <summary>
        /// 发送邮箱重置邮件
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">用户的电子邮件</param>
        /// <param name="resetLink">重置链接</param>
        public void SendChangeEmailUrl(string? userName, string? email, string? resetLink);

        /// <summary>
        /// 发送密码重置邮件
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">用户的电子邮件</param>
        /// <param name="resetLink">重置链接</param>
        public void SendResetPasswordUrl(string? userName, string? email, string? resetLink);
    }
}
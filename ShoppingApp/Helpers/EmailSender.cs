using MailKit.Net.Smtp;
using MimeKit;
using ShoppingApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingApp.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendEmailAsync(Message message, string username)
        {
            var emailMessage = CreateEmailMessage(message, username);

            await SendAsync(emailMessage);
        }

        public async Task SendResetPasswordAsync(Message message, string username)
        {
            var emailMessage = CreateEmailForResetMessage(message, username);

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailForResetMessage(Message message, string username)
        {
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format("<h3 style='color:Black'>Hi {0}, click the link to reset your password </h3>" +
                                     "<p>{1}</p>", myTI.ToTitleCase(username), message.Content)
            };
            

            return emailMessage;
        }

        private MimeMessage CreateEmailMessage(Message message, string username)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format("<h3 style='color:green'>{0} </h3>" +
                                  "<h4 style='color:black'>Hi {1}, " +
                                  "<p style='color:black'> Welcome To India's Largest Online Shopping Portal</p>" +
                                  "</h4><br>" + "contact <strong>shopmeportal@gmail.com</strong> for your queries related to ShopMe",
                                   message.Content, username.ToUpper())
            };

            return emailMessage;
        }
        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                await client.SendAsync(mailMessage);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}

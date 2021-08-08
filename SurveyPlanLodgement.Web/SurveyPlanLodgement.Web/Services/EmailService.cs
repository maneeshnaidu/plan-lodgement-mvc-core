using Microsoft.Extensions.Options;
using SurveyPlanLodgement.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SurveyPlanLodgement.Web.Services
{
    public class EmailService : IEmailService
    {
        //Get template path
        private const string templatePath = @"EmailTemplates/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholders("Hello {{Username}}, This is a test email from BookStore app", userEmailOptions.Placeholders);
            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody("TestMail"), userEmailOptions.Placeholders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendEmailConfirmationMail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholders("Hello {{Username}}, Confirm your email id", userEmailOptions.Placeholders);
            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody("EmailConfirm"), userEmailOptions.Placeholders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendForgotPasswordMail(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceholders("Hello {{Username}}, Reset password", userEmailOptions.Placeholders);
            userEmailOptions.Body = UpdatePlaceholders(GetEmailBody("ForgotPasswordMail"), userEmailOptions.Placeholders);

            await SendEmail(userEmailOptions);
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHtml,
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.Username, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSsl,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);


        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));

            return body;
        }

        private string UpdatePlaceholders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
    }
}

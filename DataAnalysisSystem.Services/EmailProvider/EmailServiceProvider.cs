using DataAnalysisSystem.DTO.AdditionalFunctionalities;
using DataAnalysisSystem.ServicesInterfaces.EmailProvider;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using MimeKit.Utils;
using System.IO;
using System.Threading.Tasks;

namespace DataAnalysisSystem.Services.EmailProvider
{
    public class EmailServiceProvider : IEmailProvider
    {
        private IEmailProviderConfigurationProfile _emailConfigurationProfile;
        private IHostingEnvironment _environment;

        public EmailServiceProvider(IEmailProviderConfigurationProfile emailConfigurationProfile, IHostingEnvironment environment)
        {
            _emailConfigurationProfile = emailConfigurationProfile;
            _environment = environment;
        }

        public Task SendEmailMessageAsync(EmailMessageContentViewModel emailMessageContent)
        {
            var message = new MimeMessage();

            if (string.IsNullOrWhiteSpace(emailMessageContent.EmailSenderAddress))
            {
                message.From.Add(new MailboxAddress(_emailConfigurationProfile.SenderName, _emailConfigurationProfile.SmtpUsername));
            }
            else
            {
                message.From.Add(new MailboxAddress(emailMessageContent.EmailSenderAddress, _emailConfigurationProfile.SmtpUsername));
                message.To.Add(new MailboxAddress(emailMessageContent.EmailSenderAddress, emailMessageContent.EmailSenderAddress));
            }

            message.To.Add(new MailboxAddress(emailMessageContent.EmailRecipientFullName, emailMessageContent.EmailRecipientAddress));
            message.Subject = emailMessageContent.EmailTopic;

            var builder = new BodyBuilder();
            builder.TextBody = $@"
Data Analysis System

{emailMessageContent.HeaderOfEmailContent}

{emailMessageContent.PrimaryContent}

{emailMessageContent.URLActionText}: {emailMessageContent.AdditionalURLToAction}


______________________
Data Analysis System Administration Team";

            var logo = builder.LinkedResources.Add(Path.Combine(_environment.WebRootPath, @"images\logo_fill_transparent.png"));
            logo.ContentId = MimeUtils.GenerateMessageId();

            string html = File.ReadAllText(Path.Combine(_environment.WebRootPath, @"resources\StructureOfEmailMessage\index.htm"));

            builder.HtmlBody = html
                                  .Replace("{BrandLogo}", logo.ContentId)
                                  .Replace("{Topic}", emailMessageContent.EmailTopic)
                                  .Replace("{PrimaryContent}", emailMessageContent.PrimaryContent)
                                  .Replace("{SecondaryContent}", emailMessageContent.SecondaryContent);

            if (!string.IsNullOrWhiteSpace(emailMessageContent.AdditionalURLToAction))
            {
                builder.HtmlBody = builder.HtmlBody
                                                 .Replace("display: none", "display: inline-block")
                                                 .Replace("{AdditionalURLToAction}", emailMessageContent.AdditionalURLToAction)
                                                 .Replace("{URLCaptionText}", emailMessageContent.URLActionText);
            };

            message.Body = builder.ToMessageBody();

            if (emailMessageContent.AttachmentsPaths.Count != 0)
            {
                foreach (var path in emailMessageContent.AttachmentsPaths)
                {
                    builder.Attachments.Add(path);
                }
            }

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Connect(_emailConfigurationProfile.SmtpServer, _emailConfigurationProfile.SmtpPort, false);
                client.Authenticate(_emailConfigurationProfile.SmtpUsername, _emailConfigurationProfile.SmtpPassword);

                client.Send(message);
                client.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using DIgitalCard.Lib.Models;

namespace DigitalCard.Api.Services
{
    public interface IEmailService
    {

        Task<bool> SendEmailWithSendGrid(Message message);


    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;

        }


        public async Task<bool> SendEmailWithSendGrid(Message message)
        {
            var client = new SendGridClient(_emailConfig.SendGridKey);
            var from = new EmailAddress(_emailConfig.From, _emailConfig.Name);
            List<EmailAddress> recipients = new List<EmailAddress>();

            foreach (var item in message.To)
            {
                recipients.Add(new EmailAddress { Email = item });
            }


            var bodyMsg = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, message.Subject, message.PlainContent, message.HtmlContent);

            var response = await client.SendEmailAsync(bodyMsg);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }



    }
}



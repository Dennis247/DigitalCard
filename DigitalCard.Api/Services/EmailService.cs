using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Options;
using DIgitalCard.Lib.Models;

using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace DigitalCard.Api.Services
{
    public interface IEmailService
    {

        Task<bool> SendEmailWithSendGrid(Message message);
        void SendEmailWithSMTP(Message message);


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



        //public async void SendEmailWithSMTP(Message message)
        //{

        //    try
        //    {
        //        var fromAddress = new MailAddress("denghazi619@gmail.com");
        //        var toAddress = new MailAddress(message.To[0]);
        //        string fromPassword = "mustang247";
        //        string subject = message.Subject;
        //        string body = message.HtmlContent;

        //        var smtp = new SmtpClient
        //        {
        //            Host = "smtp.gmail.com",
        //            Port = 587,
        //            EnableSsl = true,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false,
        //            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        //        };
        //        using (var msg = new MailMessage(fromAddress, toAddress)
        //        {
        //            Subject = subject,
        //            Body = body
        //        })
        //        {
        //            smtp.Send(msg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.ToString());
        //    }


            //}

            public void SendEmailWithSMTP(Message message)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("tyler74@ethereal.email"));
                email.To.Add(MailboxAddress.Parse(message.To[0]));
                email.Subject = message.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = message.HtmlContent };

                // send email
                using var smtp = new SmtpClient();
                
                smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("tyler74@ethereal.emai", "fETvHqAxXD5gBv9xzt");
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
             
            }
            // create message
    
        }

    }
}



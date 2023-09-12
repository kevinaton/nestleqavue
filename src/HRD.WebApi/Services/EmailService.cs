using HRD.WebApi.ViewModels;
using HRD.WebApi.ViewModels.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HRD.WebApi.Services
{
    public class EmailService : IEmailService
    {
        public readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string EmailBodyMessage(EnumEmailNotificationType type)
        {
            var bodyMessage = string.Empty;
            switch(type)
            {
                case EnumEmailNotificationType.QAScrap:
                    bodyMessage = "This is a Notification for QA Scrap Approval";
                    break;
                case EnumEmailNotificationType.PlantManager:
                    bodyMessage = "This is a Notification for Plant Manager Approval";
                    break;
                case EnumEmailNotificationType.ControllerScrap:
                    bodyMessage = "This is a Notification for ControllerScrap Approval";
                    break;
                case EnumEmailNotificationType.ScrapDestroyed:
                    bodyMessage = "This is a Notification for Scrap Destroyed";
                    break;
            }

            return bodyMessage;
        }

        public string EmailSubject(EnumEmailNotificationType type)
        {
            var emailSubject = string.Empty;
            switch (type)
            {
                case EnumEmailNotificationType.QAScrap:
                    emailSubject = "Notification for QA Scrap Approval";
                    break;
                case EnumEmailNotificationType.PlantManager:
                    emailSubject = "Notification for Plant Manager Approval";
                    break;
                case EnumEmailNotificationType.ControllerScrap:
                    emailSubject = "Notification for ControllerScrap Approval";
                    break;
                case EnumEmailNotificationType.ScrapDestroyed:
                    emailSubject = "Notification for Scrap Destroyed";
                    break;
            }

            return emailSubject;
        }

        public void SendEmail(List<string> toAddresses, string emailSubject, string emailBody)
        {
            try
            {
                var testKey = _configuration["SMTP"];
                var testHost = _configuration["SMTP:Host"];
                SmtpClient client = new SmtpClient()
                {
                    Host = _configuration["SMTP:Host"],
                    Port = Convert.ToInt32(_configuration["SMTP:Port"]),
                    EnableSsl = Convert.ToBoolean(_configuration["SMTP:EnableSsl"])
                };
                var config = _configuration["SMTP:From"];
                client.Credentials = new NetworkCredential()
                {
                    UserName = _configuration["SMTP:CredentialUserName"],
                    Password = _configuration["SMTP:CredentialPassword"]
                };

                //From
                var fromEmail = _configuration["SMTP:From"];
                MailAddress from = new MailAddress(fromEmail);

                // Specify the message content.
                MailMessage message = new MailMessage();

                // Set destinations for the email message.
                foreach(var address in toAddresses)
                {
                    message.To.Add(address);
                }

                //Set From Address
                message.From = from;

                // Include some non-ASCII characters in body and subject.
                //string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                message.Body = emailBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;

                message.Subject = emailSubject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                // Set the method that is called back when the send operation ends.
                //client.SendCompleted += new
                //SendCompletedEventHandler(SendCompletedCallback);

                // The userState can be any object that allows your callback
                // method to identify this send operation.
                // For this example, the userToken is a string constant.
                //string userState = "test message1";
                //client.SendAsync(message, userState);
                //client.SendMailAsync(message);
                client.Send(message);
                
            }
            catch (Exception)
            {
                //return false;
            }
        }
    }
}

using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
          public EmailSettings e_EmailSettings { get; }

        public ILogger<EmailService> e_Logger { get; }
        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            e_EmailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            e_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Email email)
        {
             var client = new SendGridClient(e_EmailSettings.ApiKey);
             var subject = email.Subject;
             var to = new EmailAddress(email.To);
             var emailBody  = email.Body;
              var from = new EmailAddress
              {
                   Email = e_EmailSettings.FromAddress,
                   Name = e_EmailSettings.FromName
              };

           // var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(msg);

            e_Logger.LogInformation("Email Sent. ");

            if(response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
                return true;

             e_Logger.LogInformation("Email Sending failed ");    

             return false;
        }
    }
}
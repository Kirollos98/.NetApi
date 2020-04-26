using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProject.Services
{
    public static class SendGridAPI
    {
        public static async Task<bool> Execute(string UserEmail, string UserName, string plainTextContent, string htmlContent, string subject)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("sekkastaff@gmail.com", "Sekka Support Team");
            var to = new EmailAddress(UserEmail, UserName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return await Task.FromResult(true);
        }
    }
}

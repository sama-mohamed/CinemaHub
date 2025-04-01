using System.Net.Mail;
using System.Net;

namespace CinemaWebSite.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("samamohamed18134@gmail.com", "mkzc ugas ftio xret")
            };

            return client.SendMailAsync(
                new MailMessage(from: "samamohamed18134@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true

                }
                );
        }
    }
}

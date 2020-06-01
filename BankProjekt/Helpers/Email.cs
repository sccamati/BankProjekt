using System.Configuration;
using System.Net;

namespace BankProjekt.Helpers
{
    public static class Email
    {
        public static void SendMail(string to, string subject, string body)
        {
            var message = new System.Net.Mail.MailMessage(ConfigurationManager.AppSettings["sender"], to)
            {
                Subject = subject,
                Body = body
            };
            var smtpClient = new System.Net.Mail.SmtpClient
            {
                Host = ConfigurationManager.AppSettings["smtpHost"],
                Credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["sender"],
                    ConfigurationManager.AppSettings["password"]),
                EnableSsl = true,
                Port = 587
            };
            smtpClient.Send(message);
        }
    }
}
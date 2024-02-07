using Demo.DAL.Models;
using Demo.PL.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Demo.PL.Helpers
{
    public class EmailSettings : IEmailSettings
    {
        private readonly MailSettings _options;


        //public static void SendEmail(Email email)
        //{
        //    var client = new SmtpClient("smtp.gmail.com", 587);
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential("mohammed.gamal.a25@gmail.com", "ljdgnzabgnwyvolh");
        //    client.Send("mohammed.gamal.a25@gmail.com", email.To,email.Subject,email.Body);
        //}



        public EmailSettings(IOptions<MailSettings> options)
        {
            _options = options.Value;
        }
        public  void SendEmail(Email email)
        {
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject
            };

            mail.To.Add(MailboxAddress.Parse(email.To));
            var builder = new BodyBuilder
            {
                HtmlBody = email.Body
            };
            
            mail.From.Add(new MailboxAddress(_options.DisplayName,_options.Email));

            using var smtp = new SmtpClient();
            smtp.Connect(_options.Host,_options.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email,_options.Password);
            smtp.Send(mail);
            smtp.Disconnect(true);

        }
    }
}

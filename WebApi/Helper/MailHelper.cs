using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WebApi.Helper
{
    public class MailHelper
    {
        public static async Task Send(string toEmail, string subject, string messageText)
        {
            /* configuration */
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("No Reply", "Irsyadmuhammad001@gmail.com"));
            message.To.Add(new MailboxAddress("Irsyad", toEmail));
            message.Subject = subject;
            /* configuration */

            /* masukin message text */
            message.Body = new TextPart("plain")
            {
                Text = messageText
            };
            /* masukin message text */

            /* kirim email nya */
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("Irsyadmuhammad001@gmail.com", "bxxn rmbt vtmc mdcd");
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
            }
            /* kirim email nya */
        }
    }
}

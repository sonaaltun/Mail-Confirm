using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace MailRegisteration.Presentation.MailServices
{
    public class MailService : IMailService
    {
        public async Task SendMailAsync(string email, string subject, string message)
        {

            try
            {
                var newmail = new MimeMessage();
                newmail.From.Add(MailboxAddress.Parse("mail adresi"));
                newmail.To.Add(MailboxAddress.Parse(email));
                newmail.Subject = subject;
                var builder = new BodyBuilder();
                builder.HtmlBody = message;
                newmail.Body = builder.ToMessageBody();
                var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("mail adresi", "uygulama anahtarı");
                await smtp.SendAsync(newmail);
                await smtp.DisconnectAsync(true);


            }
            catch (Exception ex)
            {

                throw new InvalidOperationException($"E-posta gönderilirker hata oluştu: {ex.Message}");
            }
        }
    }
}

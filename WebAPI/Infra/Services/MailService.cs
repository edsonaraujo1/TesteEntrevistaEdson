using System.Net;
using System.Net.Mail;
using System.Text;

namespace WebAPI.Infra.Services
{
    public class MailService : IMailService
    {
        private string smtpAddress => "smtp.office365.com";               // "smtp.gmail.com"
        private int portNumber => 587;                                    // 587
        private string emailFromAddress => "edsonaraujo1@outlook.com";    // "edsondearaujooficial@gmail.com"
        private string password => "dx44l09xj";                           // "#HO35M@%DE!Ka"

        public void AddEmailToMessage(MailMessage mailMessage, string[] emails)
        {
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }
        }
        public void SendMail(string[] emails, string subject, string body, bool isHtml = false)
        {
            using(MailMessage mailMessage = new MailMessage())
            {
                string data = @"Curriculo_Edson_Atual.pdf";
                mailMessage.From = new MailAddress(emailFromAddress, "Edson de Araujo");
                MailAddress copy = new MailAddress("edsonaraujo1@outlook.com");
                mailMessage.Bcc.Add(copy);
                //Attachment file = new Attachment(data, MediaTypeNames.Application.Octet);
                //mailMessage.Attachments.Add(file);
                AddEmailToMessage(mailMessage, emails);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isHtml;
                mailMessage.BodyEncoding = Encoding.ASCII;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.EnableSsl = true;
                    smtp.Timeout = 50000;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.Send(mailMessage);
                    smtp.Dispose();
                }
            }
        }
    }
}

using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace Import_Zamowien_Producenckich
{
    internal class MailSender
    {
        public string temat = "Zamówienia producenckie. MAG dystrybucja.";
        public string tresc;
        public string recip;

        public MailSender()
        {
        }

        public MailSender(string body, string to, string subject)
        {
            this.tresc = body;
            this.recip = to;
            this.temat = subject;
        }

        public int send()
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "mag-ol.home.pl";
                smtpClient.Timeout = 10000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("raporty@mag.olsztyn.pl", "3LhjPZnaU");
                smtpClient.EnableSsl = true;
                mailMessage.From = new MailAddress("raporty@mag.olsztyn.pl");
                string[] array = this.recip.Split(new char[]
                {
                    ','
                });
                for (int i = 0; i < array.Count<string>(); i++)
                {
                    mailMessage.To.Add(new MailAddress(array[i]));
                }
                mailMessage.Subject = this.temat;
                mailMessage.Body = this.tresc;
                mailMessage.IsBodyHtml = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //LOGI.Wstaw("Wysyłanie Maila: " + ex.ToString());
            }
            return 1;
        }
    }
}
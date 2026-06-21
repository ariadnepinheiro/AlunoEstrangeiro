using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace SRV.Common.Mail
{
    public class Mail
    {
        public static void SendMail(string to, string subject, string body)
        {
            MailMessage message = new MailMessage();
            message = new MailMessage();
            message.To.Add(new MailAddress(to));
            message.Subject = subject;
            message.Body = body;

            SmtpClient client = new SmtpClient();
            client.Send(message);
        }
    }
}
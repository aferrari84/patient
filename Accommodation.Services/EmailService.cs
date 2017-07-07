using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.Net.Mail;
using Accommodation.Interfaces.Services;

namespace Alexandra.Core.Business
{
    public class EmailService : IEmailService
    {
        SmtpClient _client;
        public EmailService()
        {
            _client = new SmtpClient();
        }

        public bool Send(string To, string Subject, string Body, bool isHtml = false)
        {
            try
            {
                MailMessage email = GetEmailMessage(Subject, Body, To, isHtml);
                _client.Send(email);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private MailMessage GetEmailMessage(string subject, string body, string To, bool isHtml)
        {
            string displayName = "Turismo Alojamiento";

            MailMessage msg = new MailMessage();
            msg.Subject = subject;
            msg.To.Add(new MailAddress(To));
            msg.From = new MailAddress(msg.From.Address, displayName);
            body = body.Replace("\n", Environment.NewLine);


            AlternateView html_view = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");

            msg.AlternateViews.Add(html_view);
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = false;
            return msg;
        }
    }
}

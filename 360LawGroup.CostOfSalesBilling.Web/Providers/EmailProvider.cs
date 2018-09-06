using Elmah;
using System;
using System.Configuration;
using System.Net.Mail;


namespace _360LawGroup.CostOfSalesBilling.Web.Providers
{
    public static class EmailProvider
    {
        private static void SendMail(MailMessage mail)
        {
            try
            {
                var domain = ConfigurationManager.AppSettings["SMTP_HOST"];
                var username = ConfigurationManager.AppSettings["SMTP_USERNAME"];
                var password = ConfigurationManager.AppSettings["SMTP_PASSWORD"];
                var port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP_PORT"]);
                var isUseSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SMTP_SSL"]);
                mail.From = new MailAddress(username);
                var client = new SmtpClient(domain)
                {
                    Port = port,
                    Credentials = new System.Net.NetworkCredential(username, password),
                    EnableSsl = isUseSsl
                };
                mail.IsBodyHtml = true;
                client.Send(mail);
            }
            catch(Exception e)
            {
                var context = System.Web.HttpContext.Current;
                if (context != null)
                    ErrorSignal.FromContext(context).Raise(e);
            }
        }
        
        public static void SendMail(string to, string cc, string bcc, string subject, string body)
        {
            var mail = new MailMessage();

            foreach(var address in to.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(address);
            }

            if(!string.IsNullOrEmpty(cc))
            {
                foreach(var address in cc.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.CC.Add(address);
                }
            }
            if(!string.IsNullOrEmpty(bcc))
            {
                foreach(var address in bcc.Split(new[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    mail.Bcc.Add(address);
                }
            }
            mail.Subject = subject;
            mail.Body = body;
            SendMail(mail);
        }
    }
}
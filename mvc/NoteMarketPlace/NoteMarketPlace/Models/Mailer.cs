using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class Mailer
    {
        public void sendMail(string subject, string Message,List<string> receiver)
        {
            using (MailMessage mm = new MailMessage())
            {
                mm.From = new MailAddress("notesmarketplace0@gmail.com");
                mm.Subject = subject;
                
                foreach (var rec in receiver)
                {
                    mm.CC.Add(new MailAddress(rec));
                }
                    //string body = "Hello " + model.Name + ",";
                    // body += "<br /><br />Please click the following link to activate your account";
                    //body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                    //body += "<br /><br />Thanks";
                    
                mm.Body = Message;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("***********", "****");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }

        }
    }
}

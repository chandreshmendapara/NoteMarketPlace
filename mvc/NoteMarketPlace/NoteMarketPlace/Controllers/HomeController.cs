using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class HomeController : Controller
    {


        private NotesMarketPlaceEntities _Context;
        // GET: Admin

        public HomeController()
        {
            _Context = new NotesMarketPlaceEntities();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ModelState.Clear();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact model)
        {


                if (ModelState.IsValid)
                {

                    using (MailMessage mm = new MailMessage("abc@gmail.com", model.Email))
                    {
                        mm.Subject = model.Subject;
                        string body = "Hello " + model.Name + ",";
                       // body += "<br /><br />Please click the following link to activate your account";
                        //body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Click here to activate your account.</a>";
                        //body += "<br /><br />Thanks";

                        mm.Body = model.Message;
                        mm.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("abc@gmail.com", "***********");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }

                     }
            return View();
        }


        public ActionResult Faq()
        {
            ViewBag.Message = "Your faq page.";

            return View();
        }
        public ActionResult SearchNote()
        {
            ViewBag.Message = "Your note page.";

            List<tblSellerNote> tblSellerNotes = _Context.tblSellerNotes.ToList();
            List<tblCountry> tblCountries = _Context.tblCountries.ToList();

            var multiple = from c in tblSellerNotes
                           join t1 in tblCountries on c.Country equals t1.ID
                           where c.Status == 9
                           select new MultipleData { sellerNote = c, Country=t1};

            ViewBag.Count = (from c in tblSellerNotes
                             join t1 in tblCountries on c.Country equals t1.ID
                             where c.Status == 9 select c).Count();

            return View(multiple);
        }
    }
}
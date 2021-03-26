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

                Mailer mailer = new Mailer();
                mailer.sendMail(model.Subject, model.Message, model.Email);

                    
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
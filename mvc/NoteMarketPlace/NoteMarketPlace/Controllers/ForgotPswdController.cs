using NoteMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ForgotPswdController : Controller
    {
        private NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();
        // GET: ForgotPswd
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string email)
        {

            var isUser = dbobj.tblUsers.Where(m => m.EmailID.Equals(email)).FirstOrDefault();
            if (isUser != null)
            {

                string newpswd = RandomString(8);
                isUser.Password = newpswd;
                string subject = "New Temporary Password has been created for you";                string body = "Hello,  <br/><br/>We have generated a new password for you <br/> Passowrd: "+newpswd;                Models.Mailer mailer = new Models.Mailer();                mailer.sendMail(subject, body, email);
                dbobj.SaveChanges();

            }
            else
                ViewBag.NotValidUser = "User Does not Exists.";



            return RedirectToAction("Login");

        }



        private static Random random = new Random();
        public static string RandomString(int length)
        {
           
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXY@$_&*Z0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
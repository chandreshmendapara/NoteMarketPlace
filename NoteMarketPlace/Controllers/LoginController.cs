using NoteMarketPlace.Models;
using NoteMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{
    public class LoginController : Controller
    {
        private NotesMarketPlaceEntities _Context;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public LoginController()
        {
            _Context = new NotesMarketPlaceEntities();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Login model)
        {
            
            var connectionDB = new NotesMarketPlaceEntities();
            var result = connectionDB.tblUser.Where(m => m.EmailID == model.Email).FirstOrDefault();
            if (result == null)
            {
                ViewBag.NotValidUser = "User Does Not Exists";
            }
            else
            {

                if (model.Password == result.Password)
                {
                    FormsAuthentication.SetAuthCookie(result.EmailID,true);
                    return RedirectToAction("", "user");
                }
                else
                {
                    ViewBag.NotValidPassword = "Passowrd is Incorrect";
                }
            }
                return View("Index");
            
        }
    }
}
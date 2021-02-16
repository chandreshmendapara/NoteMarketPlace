using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class LoginController : Controller
    {
        private NotesMarketPlaceEntities2 _Context;

        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public LoginController()
        {
            _Context = new NotesMarketPlaceEntities2();
        }


        [HttpPost]
        public ActionResult Index(Login model)
        {
            
            var connectionDB = new NotesMarketPlaceEntities2();
            var result = connectionDB.tblUsers.Where(m => m.EmailID == model.Email).FirstOrDefault();
            if (result == null)
            {
                ViewBag.NotValidUser = "User Does Not Exists";
            }
            else
            {

                if (model.Password == result.Password)
                {
                    return RedirectToAction("faq", "home");
                }
                else
                {
                    ViewBag.NotValidPassowrd = "Passowrd is Incorrect";
                }
            }
                return View("Index");
            
        }
    }
}
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
            
            bool isvalid = _Context.tblUsers.Any(m => m.EmailID == model.Email &&  m.Password == model.Password);
           
            if (isvalid)
            {
                var result = _Context.tblUsers.Where(m => m.EmailID == model.Email).FirstOrDefault();
                if (result.RoleID == 101 || result.RoleID == 102)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("", "Admin");


                }

                else if (result.RoleID == 103)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("", "User");


                }
                else
                    ViewBag.NotValidUser = "Something went wrong";
            }
            else
            {
                ViewBag.NotValidUser = "Incorrect Email or Password";

                /*if (model.Password == result.Password)
                {

                  /*  if (User.Identity.IsAuthenticated)
                    {
                        string name = User.Identity.Name;


                    }

                    if()
                    FormsAuthentication.SetAuthCookie(result.EmailID,true);
                    return RedirectToAction("", "user");
                }
                else
                {
                    ViewBag.NotValidPassword = "Passowrd is Incorrect";
                }*/
            }
                return View("Index");
            
        }
    }
}
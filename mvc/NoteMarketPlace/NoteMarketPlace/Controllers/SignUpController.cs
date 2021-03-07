using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NoteMarketPlace.Controllers
{
    public class SignUpController : Controller
    {
        NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();
        

        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }
        

        [HttpPost]
        public ActionResult Index(tblUser model)
        {

            var connectionDB = new NotesMarketPlaceEntities();
            string email = model.EmailID;
            if (IsValidEmail(email))
            {
                if (IsValidPassword(model.Password, model.RePassword))
                {
                    var result = connectionDB.tblUsers.Where(m => m.EmailID == email).FirstOrDefault();
                    if (result == null)
                    {

                        tblUser obj = new tblUser();

                        obj.FirstName = model.FirstName;
                        obj.LastName = model.LastName;
                        obj.EmailID = model.EmailID;
                        obj.Password = model.Password;
                        obj.IsEmailVerified = false;
                        obj.IsActive = true;
                        obj.RePassword = "1223";
                        obj.ModifiedBy = null;
                        obj.ModifiedDate = null;
                        
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedBy = null;
                        obj.RoleID = 103;



                        if (ModelState.IsValid)
                        {
                            dbobj.tblUsers.Add(obj);
                            try
                            {
                                // Your code...
                                // Could also be before try if you know the exception occurs in SaveChanges

                                dbobj.SaveChanges();
                                ModelState.Clear();

                                FormsAuthentication.SetAuthCookie(model.EmailID, true);
                                return RedirectToAction("Profile", "User");

                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName, ve.ErrorMessage);
                                    }
                                }
                            }

                        }


                    }

                    else
                    {
                        ViewBag.NotValidEmail = "This Email is already exists";

                    }

                   

                }

                else
                {
                    ViewBag.NotValidPassword = "Password and Re-enter password must be same";
                }
            }
            else
            {
                ViewBag.NotValidEmail ="Email is not valid";
            }
            return View("Index");

        }
        public static bool IsValidPassword(string pswd, string repswd)
        {
            if (pswd==repswd && pswd != "")
            {
                return true;
            }
            return false;
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
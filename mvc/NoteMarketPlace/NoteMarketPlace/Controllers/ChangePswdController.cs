using NoteMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    [Authorize]
    public class ChangePswdController : Controller
    {

        private NotesMarketPlaceEntities _Context;
        public ChangePswdController()
        {
            _Context = new NotesMarketPlaceEntities();

        }
        // GET: ChangePswd
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string oldPswd, string newPswd, string confirmPswd)
        {
            if(newPswd.Equals(confirmPswd))
            {
                var user = _Context.tblUsers.Where(m => m.EmailID == User.Identity.Name).FirstOrDefault();
                if (user.Password == oldPswd)
                {
                    user.Password = newPswd;
                    try
                    {
                        
                        _Context.SaveChanges();


                    }
                    catch (DbEntityValidationException ex)
                    {
                        string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                        throw new DbEntityValidationException(errorMessages);
                    }
                }
                else
                {
                    ViewBag.Error = "Incorrect Password";
                    return View();
                }

            }




            return RedirectToAction("logout","login");
        }
    }
}
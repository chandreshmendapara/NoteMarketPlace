using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
 
    [Authorize(Roles = "Admin, Super Admin")]
    public class AdminController : Controller
    {

        private NotesMarketPlaceEntities _Context;
        // GET: Admin

        public AdminController()
        {
            _Context = new NotesMarketPlaceEntities();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserQuery()
        {

            List<Contact> contacts = new List<Contact>();
            
            return View(contacts);
        }

        public ActionResult addNoteType()
        {
            return View();

        }


        public ActionResult addNoteCategory()
        {



            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addNoteCategory(tblNoteCategory model)
        {

            if(User.Identity.IsAuthenticated)
            {

                 string name=  User.Identity.Name;
                int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();

                bool isvalid = _Context.tblNoteCategories.Any(m => m.Name == model.Name);

                if (!isvalid)
                {



                    tblNoteCategory obj = new tblNoteCategory();
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                    obj.CreatedDate = DateTime.Now;
                    obj.CreatedBy = u;
                    obj.IsActive = true;




                    if (ModelState.IsValid)
                    {
                        _Context.tblNoteCategories.Add(obj);
                        try
                        {
                            // Your code...
                            // Could also be before try if you know the exception occurs in SaveChanges

                            _Context.SaveChanges();

                            ModelState.Clear();

                            return View();

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
                    ViewBag.Message = "Note Category already exists";
                

            }


            return View();

        }


        public ActionResult addCountry()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addCountry(tblCountry model)
        {

            if (User.Identity.IsAuthenticated)
            {

                string name = User.Identity.Name;
                int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();


                bool isvalid = _Context.tblCountries.Any(m => m.CountryCode == model.CountryCode);

                if (!isvalid)
                {

                    tblCountry obj = new tblCountry();
                    obj.CountryCode = model.CountryCode;
                    obj.Name = model.Name;
                    obj.CreatedDate = DateTime.Now;
                    obj.CreatedBy = u;
                    obj.IsActive = true;
                    if (ModelState.IsValid)
                    {
                        _Context.tblCountries.Add(obj);
                        try
                        {
                            // Your code...
                            // Could also be before try if you know the exception occurs in SaveChanges

                            _Context.SaveChanges();

                            ModelState.Clear();

                            return View();

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
                    ViewBag.Message = "Country already exists in list";
            }


            return View();

        }





        public ActionResult ManageNoteType()
        {
            return View();

        }

        public ActionResult ManageNoteCategory()
        {
            List<tblNoteCategory> tblNoteCategoriesList = _Context.tblNoteCategories.ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multiple = from c in tblNoteCategoriesList
                           join t1 in tblUser on c.CreatedBy equals t1.ID
                           select new MultipleData { NoteCategory = c, User = t1 };


            return View(multiple);

        }

        public ActionResult ManageCountry()
        {
            List<tblCountry> tblCountriesList = _Context.tblCountries.ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multiple = from c in tblCountriesList
                           join t1 in tblUser on c.CreatedBy equals t1.ID
                           select new MultipleData { Country = c, User = t1 };


            return View(multiple);

        }

        public ActionResult ManageAdmin()
        {
            return View();

        }

        public ActionResult addAdmin()
        {
            return View();

        }

        public ActionResult systemConfig()
        {
            return View();

        }

    }
}
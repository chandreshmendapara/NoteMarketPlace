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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addNoteType(tblNoteType model)
        {

            if (User.Identity.IsAuthenticated)
            {
            var connectionDB = new NotesMarketPlaceEntities();

                string name = User.Identity.Name;
                int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();

                bool isvalid = _Context.tblNoteTypes.Any(m => m.Name == model.Name);

                if (!isvalid)
                {



                    tblNoteType obj = new tblNoteType();
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                    obj.CreatedDate = DateTime.Now;
                    obj.CreatedBy = u;
                    obj.IsActive = true;




                    if (ModelState.IsValid)
                    {
                        _Context.tblNoteTypes.Add(obj);
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
                    ViewBag.Message = "Note Type already exists";


            }


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
            List<tblNoteType> tblNoteTypesList = _Context.tblNoteTypes.ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multiple = from c in tblNoteTypesList
                           join t1 in tblUser on c.CreatedBy equals t1.ID
                           select new MultipleData { NoteType = c, User = t1 };


            return View(multiple);

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
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var CountryCode = entities.tblCountries.ToList();
            SelectList list = new SelectList(CountryCode, "CountryCode", "CountryCode");
            ViewBag.CountryCode = list;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Super Admin")]
        public ActionResult addAdmin(UserDetails model)
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var CountryCode = entities.tblCountries.ToList();
            SelectList list = new SelectList(CountryCode, "CountryCode", "CountryCode");
            ViewBag.CountryCode = list;

            string name = User.Identity.Name;
            int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();


            if (User.Identity.IsAuthenticated)
            {

                NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();
                tblUser obj = new tblUser();
                obj.FirstName = model.FirstName;
                obj.LastName = model.LastName;
                obj.EmailID = model.EmailID;
                obj.Password = "Admin@123";
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = u;
                obj.IsActive = true;
                obj.IsEmailVerified = true;
                obj.RoleID = 102;

                    dbobj.tblUsers.Add(obj);
                    dbobj.SaveChanges();
                   

                int id = (from record in dbobj.tblUsers orderby record.ID descending select record.ID).First();

                tblUserProfile userobj = new tblUserProfile();
                userobj.UserID = id;
                userobj.PhoneNumber_CountryCode = model.CountryCode;
                userobj.PhoneNumber = model.PhnNo;
                userobj.AddressLine1 = "addressline1";
                userobj.AddressLine2 = "addressline2";
                userobj.City = "city";
                userobj.State = "State";
                userobj.ZipCode = "123321";
                userobj.Country = "India";
                dbobj.tblUserProfiles.Add(userobj);
                dbobj.SaveChanges();
                ModelState.Clear();
                return RedirectToAction("ManageAdmin", "Admin");


            }
                return View();


        }


        public ActionResult systemConfig()
        {
            return View();

        }

    }
}



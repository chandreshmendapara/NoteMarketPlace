﻿using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoteMarketPlace.Controllers
{
 
    [Authorize(Roles = "Admin, Super Admin")]
    public class AdminController : Controller
    {

        private NotesMarketPlaceEntities _Context;
        private FileStream fs;

        // GET: Admin

        public AdminController()
        {
            _Context = new NotesMarketPlaceEntities();

            ViewBag.daefaultNoteImg = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "defaultNoteImg").Values;
            ViewBag.facebook = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "FacebookURL").Values;
            ViewBag.twitter = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "TwitterURL").Values;
            ViewBag.linkedin = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "LinkedinURL").Values;

        }
        public ActionResult Index(int? p, int? month, string search )
        {

            var current_date = DateTime.Now.Date.AddDays(-7);
            ViewBag.noteForReview = _Context.tblSellerNotes.Where(m => m.Status == 7 || m.Status == 8).Count();
            ViewBag.lastWeekDownload = _Context.tblDownloads.Where(m =>m.IsAttachmentDownloaded==true && m.AttachmentDownloadedDate >=current_date).Count();
            ViewBag.lastWeekNewUser = _Context.tblUsers.Where(m => m.CreatedDate  >= current_date).Count();


            int pageSize = 4;
            if (p != null)
                ViewBag.Count = p * pageSize - pageSize + 1;
            else
                ViewBag.Count = 1;


            var multiple = (from c in _Context.tblSellerNotes
                            join t1 in _Context.tblNoteCategories on c.Category equals t1.ID
                            join t2 in _Context.tblUsers on c.SellerID equals t2.ID
                            join t3 in _Context.tblSellerNotesAttachements on c.ID equals t3.NoteID
                            where c.Status == 9
                            let total = (_Context.tblDownloads.Where(m => m.NoteID == c.ID && m.IsAttachmentDownloaded == true).Count())

                            select new PublishedNoteDetails
                            {
                                id = c.ID,
                                sellingPrice = c.SellingPrice,
                                Title = c.Title,
                                categoryName = t1.Name,
                                IsPaid = c.IsPaid,
                                AttachmentPath = t3.FilePath,
                                sellerName = t2.FirstName + " " + t2.LastName,
                                PublishedDate = (DateTime)c.PublishedDate,
                                totalDownloads = total,

                            }


                          );

            if (search != null && search != "")
                multiple = multiple.Where(m => m.Title.ToLower().Contains(search.ToLower()) || m.categoryName.ToLower().Contains(search.ToLower())
                    
                    || m.sellerName.ToLower().Contains(search.ToLower()));

            if (month != null)
                multiple = multiple.Where(m => m.PublishedDate.Month == month);

            foreach(var data in multiple)
            {
                data.AttachmentSize = FileSize(data.AttachmentPath);
            }





            /* */
            ViewBag.Search = search;
            return View(multiple.ToList().ToPagedList(p ?? 1, pageSize));
        }
        //file size
        public float FileSize(string filePath)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(filePath);
            return (fs.Length);
        }
        public ActionResult UserQuery()
        {

            List<Contact> contacts = new List<Contact>();
            
            return View(contacts);
        }





        [Authorize(Roles = "Super Admin")]
        public ActionResult addNoteType()
        {
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public ActionResult addNoteType(tblNoteType model)
        {
            

            if (User.Identity.IsAuthenticated)
            {
                if (model.ID > 0)
                {
                    var obj = _Context.tblNoteTypes.Where(m => m.ID.Equals(model.ID)).FirstOrDefault();
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                }
                else
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

                        _Context.tblNoteTypes.Add(obj);

                     








                    }

                    else
                    {
                        ViewBag.Message = "Note Type already exists";
                        return View();
                    }
                }
            }

            _Context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("manageNoteType");

        }








        [Authorize(Roles = "Super Admin")]
        public ActionResult addNoteCategory()
        {



            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public ActionResult addNoteCategory(tblNoteCategory model)
        {

            if(User.Identity.IsAuthenticated)
            {

                if (model.ID > 0)
                {
                    var obj = _Context.tblNoteCategories.Where(m => m.ID.Equals(model.ID)).FirstOrDefault();
                    obj.Name = model.Name;
                    obj.Description = model.Description;
                }
                else
                {

                    string name = User.Identity.Name;
                    int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();

                    bool isvalid = _Context.tblNoteCategories.Any(m => m.Name == model.Name);

                    if (!isvalid)
                    {


                        //_Context.tblNoteCategories.Add(model);
                        tblNoteCategory obj = new tblNoteCategory();
                        obj.Name = model.Name;
                        obj.Description = model.Description;
                        obj.CreatedDate = DateTime.Now;
                        obj.CreatedBy = u;
                        obj.IsActive = true;




                        _Context.tblNoteCategories.Add(obj);
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges













                    }

                    else
                    {
                        ViewBag.Message = "Note Category already exists";
                        return View();
                    }
                }


            }
            _Context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("manageNoteCategory");
            

        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult addCountry()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin")]
        public ActionResult addCountry(tblCountry model)
        {

            if (User.Identity.IsAuthenticated)
            {
                if (model.ID > 0)
                {
                    var obj = _Context.tblCountries.Where(m => m.ID.Equals(model.ID)).FirstOrDefault();
                    obj.Name = model.Name;
                    obj.CountryCode = model.CountryCode;
                }
                else
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

                        _Context.tblCountries.Add(obj);

                        ModelState.Clear();






                    }

                    else
                    {
                        ViewBag.Message = "Country already exists in list";
                        return View();
                    }
                }

             
            }
            _Context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("manageCountry");
        }


        [Authorize(Roles = "Super Admin")]
        public ActionResult ManageNoteType(string search, int? page)
        {
            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;
            List<tblNoteType> tblNoteTypesList = _Context.tblNoteTypes.Where(m => m.IsActive == true).ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();

            var multiple = (from c in tblNoteTypesList
                           join t1 in tblUser on c.CreatedBy equals t1.ID
                           select new MultipleData { NoteType = c, User = t1 }).ToList().ToPagedList(page ?? 1, pageSize);
            if (!String.IsNullOrEmpty(search))
            {
              multiple = (from c in tblNoteTypesList
                                join t1 in tblUser on c.CreatedBy equals t1.ID where c.Name.ToLower().Contains(search.ToLower())
                                select new MultipleData { NoteType = c, User = t1 }).ToList().ToPagedList(page ?? 1, pageSize);


            }

            ViewBag.search = search;
                return View(multiple);

        }


        [Authorize(Roles = "Super Admin")]
        public ActionResult ManageNoteCategory(string search, int? page )
        {
            List<tblNoteCategory> tblNoteCategoriesList = _Context.tblNoteCategories.Where(m => m.IsActive == true).ToList(); 
            List<tblUser> tblUser = _Context.tblUsers.ToList(); 
            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize+1;
            else
                ViewBag.Count = 1;

           var multiple = (from c in tblNoteCategoriesList
                        join t1 in tblUser on c.CreatedBy equals t1.ID
                        
                        select new MultipleData { NoteCategory = c, User = t1 }).ToList().ToPagedList(page ?? 1, pageSize);

          if (!String.IsNullOrEmpty(search))
            {
                ViewBag.search = search;
                 multiple = (from c in tblNoteCategoriesList
                                join t1 in tblUser on c.CreatedBy equals t1.ID
                                where c.Name.ToLower().Contains(search.ToLower())
                                select new MultipleData { NoteCategory = c, User = t1 }).ToList().ToPagedList(page ?? 1, pageSize);
                
            }
            else
            {

            }

            ViewBag.search = search;
            return View(multiple);

        }


        [Authorize(Roles = "Super Admin")]
        public ActionResult ManageCountry(string search ,int? page)
        {
            
            List<tblCountry> tblCountriesList = _Context.tblCountries.Where(m => m.IsActive == true).ToList(); //new List<tblNoteCategory>();
            List<tblUser> tblUser = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();
            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;
            var multiple = (from c in tblCountriesList
                           join t1 in tblUser on c.CreatedBy equals t1.ID
                           select new MultipleData { Country = c, User = t1 }).ToList().ToPagedList(page ?? 1, pageSize);
            if (!String.IsNullOrEmpty(search))
            {
                multiple = (from c in tblCountriesList
                            join t1 in tblUser on c.CreatedBy equals t1.ID where c.Name.ToLower().Contains(search.ToLower())
                            || c.CountryCode.Contains(search)
                            select new MultipleData { Country = c, User = t1 }).ToList().ToPagedList(page ?? 1, pageSize);
            }
            ViewBag.search = search;
                return View(multiple);

        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult ManageAdmin(int ? page,  string search)
        {
            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize + 1;
            else
            {
                ViewBag.Count = 1;
            }
            List<tblUser> users = _Context.tblUsers.ToList(); //new List<tblNoteCategory>();
            List<tblUserProfile> userProfiles = _Context.tblUserProfiles.ToList(); //new List<tblNoteCategory>();
           
            var multiple = (from c in users
                            join t1 in userProfiles on c.ID equals t1.UserID
                            where c.RoleID==102 && c.IsActive==true
                         
                            select new MultipleData {  User = c, userProfile=t1 });
            if(search!=null && search!="")
                multiple = multiple.Where(m => m.User.FirstName.ToLower().Contains(search.ToLower()) || m.User.LastName.ToLower().Contains(search.ToLower()) 
                || m.User.EmailID.ToLower().Contains(search.ToLower()) || m.userProfile.PhoneNumber_CountryCode.Contains(search) || 
                m.userProfile.PhoneNumber.Contains(search));
            ViewBag.Search = search;
            return View(multiple.ToList().ToPagedList(page ?? 1, pageSize));

        }



        [Authorize(Roles = "Super Admin")]
        public ActionResult addAdmin()
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var CountryCode = entities.tblCountries.Where(m=>m.IsActive==true).ToList();
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
            var CountryCode = entities.tblCountries.Where(m => m.IsActive == true).ToList();
            SelectList list = new SelectList(CountryCode, "CountryCode", "CountryCode");
            ViewBag.CountryCode = list;

            string name = User.Identity.Name;
            int u = (from user in _Context.tblUsers where user.EmailID == name select user.ID).Single();


            if (User.Identity.IsAuthenticated)
            {
                if (model.Id > 0)
                {
                    var a = _Context.tblUsers.Find(model.Id);
                    a.FirstName = model.FirstName;
                    a.LastName = model.LastName;
                    a.EmailID = model.EmailID;
                    var b = _Context.tblUserProfiles.Where(m => m.UserID == model.Id).FirstOrDefault();
                    b.PhoneNumber = model.PhnNo;
                    b.PhoneNumber_CountryCode = model.CountryCode;
                    _Context.SaveChanges();
                }
                else
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

                }
              
            }
            return RedirectToAction("ManageAdmin", "Admin");


        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult editCategory(int ? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = _Context.tblNoteCategories.Find(id);
            if (category == null)
                return HttpNotFound();
            return View("addNoteCategory", category);
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult deleteCategory(int ? ID)
        {
            if (ID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var category = _Context.tblNoteCategories.Find(ID);
            if(category==null)
                return HttpNotFound();
            category.IsActive = false;
            _Context.SaveChanges();


            return RedirectToAction("manageNoteCategory");
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult editType(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var type = _Context.tblNoteTypes.Find(id);
            if (type == null)
                return HttpNotFound();
            return View("addNoteType", type);
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult deleteType(int? ID)
        {
            if (ID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var type = _Context.tblNoteTypes.Find(ID);
            if (type == null)
                return HttpNotFound();
            type.IsActive = false;
            _Context.SaveChanges();


            return RedirectToAction("manageNoteType");
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult editCountry(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var type = _Context.tblCountries.Find(id);
            if (type == null)
                return HttpNotFound();
            return View("addCountry", type);
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult deleteCountry(int? ID)
        {
            if (ID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var type = _Context.tblCountries.Find(ID);
            if (type == null)
                return HttpNotFound();
            type.IsActive = false;
            _Context.SaveChanges();


            return RedirectToAction("manageCountry");
        }


        [Authorize(Roles = "Super Admin")]
        public ActionResult editAdmin(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var admin = _Context.tblUsers.Find(id);
            if (admin == null)
                return HttpNotFound();
            var CountryCode = _Context.tblCountries.Where(m => m.IsActive == true).ToList();
            SelectList list = new SelectList(CountryCode, "CountryCode", "CountryCode");
            ViewBag.CountryCode = list;
            List<tblUser> users = _Context.tblUsers.ToList();
            List<tblUserProfile> userProfiles = _Context.tblUserProfiles.ToList();
            var a = from c in users
                    join t1 in userProfiles on c.ID equals t1.UserID
                    where c.ID==id
                    select new UserDetails
                    {
                        Id=c.ID,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        EmailID = c.EmailID,
                        CountryCode = t1.PhoneNumber_CountryCode,
                        PhnNo = t1.PhoneNumber
                    };
          

            return View(a.FirstOrDefault());
        }

        [Authorize(Roles = "Super Admin")]
        public ActionResult deleteAdmin(int? ID)
        {
            if (ID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var admin = _Context.tblUsers.Find(ID);
            if (admin == null)
                return HttpNotFound();
            admin.IsActive = false;
            _Context.SaveChanges();


            return RedirectToAction("manageAdmin");
        }







        public ActionResult noteUnderReview(int ? page, string userName, string Search )
        {


            var User = _Context.tblUsers.ToList().Where(m=>m.RoleID==103);
            SelectList UserList = new SelectList(User, "ID", "FirstName");
            ViewBag.UserList = UserList;



            int pageSize = 5;
          
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;

            List<tblSellerNote> tblSellerNotesList = _Context.tblSellerNotes.ToList(); 
            List<tblUser> tblUserList = _Context.tblUsers.ToList();
            List<tblNoteCategory> tblNoteCategoriesList = _Context.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDataList = _Context.tblReferenceDatas.ToList();

            var multiple = (from c in tblSellerNotesList
                            join t1 in tblUserList on c.SellerID equals t1.ID

                            join t2 in tblReferenceDataList on c.Status equals t2.ID
                            join t3 in tblNoteCategoriesList on c.Category equals t3.ID
                            where c.Status == 7 || c.Status == 8
                            select new MultipleData { sellerNote = c, User = t1, referenceData = t2, NoteCategory = t3 });



            if (userName != null)
                multiple = multiple.Where(m => m.sellerNote.SellerID.ToString().Equals(userName));

            if (Search != null && Search!="")
                multiple = multiple.Where(m => m.sellerNote.Title.ToLower().Contains(Search.ToLower()));
            ViewBag.Search = Search;
            return View(multiple.ToList().ToPagedList(page ?? 1, pageSize));
        }


        [HttpPost]
        public ActionResult Rejected(int  noteId, string rejectRemark)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();
            
           
            try
            {
                    var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                    int id = admin_id.ID;
                    obj.Status = 10;
                    obj.AdminRemarks = rejectRemark;
                    obj.ActionBy = id;
                    a.SaveChanges();
                

            }
              catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            return RedirectToAction("noteUnderReview","Admin");


        }



        [HttpPost]
        public ActionResult Approved(int noteId)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();
            int book_current_status=10;

            try
            {
                var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                int id = admin_id.ID;
                book_current_status = obj.Status;
                obj.Status = 9;
                obj.ActionBy = id;
                obj.PublishedDate = DateTime.Now;
                
                a.SaveChanges();


            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            if(book_current_status==10)
                return RedirectToAction("rejectedNotes", "Admin");
            return RedirectToAction("noteUnderReview", "Admin");

        }




        [HttpPost]
        public ActionResult InReview(int noteId)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();


            try
            {
                var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                int id = admin_id.ID;
                obj.Status = 8;
                obj.ActionBy = id;
                a.SaveChanges();


            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            return RedirectToAction("noteUnderReview", "Admin");


        }




        [HttpPost]
        public ActionResult Unpublish(int noteId, string noteIssues)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblSellerNotes.Where(m => m.ID.Equals(noteId)).FirstOrDefault();
            var seller = a.tblUsers.Where(m => m.ID == obj.SellerID).FirstOrDefault();

            try
            {
                var admin_id = a.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
                int id = admin_id.ID;
                obj.Status = 11;
                obj.AdminRemarks = noteIssues;
                obj.ActionBy = id;
                a.SaveChanges();
                List<string> receiver = new List<string>();
                receiver.Add(seller.EmailID);

                string subject = "Sorry! We need to remove your notes from our portal.";
                string body = "Hello "+ seller.FirstName +" " + seller.LastName+",<br/><br/> We want to inform you that, " +
                    "your note <b>"+obj.Title +"</b> has been removed from the portal. " +
                    "Please find our remarks as below -<br/><br/>" +
                    noteIssues +
                    "<br/><br/> Regards,<br/>Notes Marketplace";
                Mailer mailer = new Mailer();
                mailer.sendMail(subject,body,receiver);


            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.PropertyName + ": " + x.ErrorMessage));
                throw new DbEntityValidationException(errorMessages);
            }
            return RedirectToAction("", "Admin");


        }






        public ActionResult AdminDownload(int id)
                         
        {

            var tblSeller = _Context.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();

            var user_id = _Context.tblUsers.Where(m => m.EmailID == User.Identity.Name && m.RoleID != 103).FirstOrDefault();
            if(user_id!=null)
            {
                string path = (from sa in _Context.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                

                string filename = (from sa in _Context.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
                filename += ".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

            }
            return HttpNotFound();
        }




        public ActionResult Member(int? page, string search)
        {
            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;

            var SellerList = _Context.tblUsers.ToList();
            SelectList list = new SelectList(SellerList, "Id", "FirstName");
            ViewBag.SellerList = list;
            

            List<tblUser> users = _Context.tblUsers.ToList();
            List<tblUserProfile> userProfiles = _Context.tblUserProfiles.ToList();
            List<tblSellerNote> sellerNotes = _Context.tblSellerNotes.ToList();
            List<tblDownload> downloads = _Context.tblDownloads.ToList();

            var multiple = (from c in users
                            join t1 in userProfiles on c.ID equals t1.UserID
                            let underReview = (sellerNotes.Where(m => m.SellerID == c.ID &&( m.Status == 7 || m.Status == 8)).Count())
                            let published = (sellerNotes.Where(m => m.SellerID == c.ID && m.Status == 9).Count())
                            let downloaded = (downloads.Where(m => m.Downloader == c.ID && m.IsAttachmentDownloaded == true).Count())
                            let dt= (DateTime)c.CreatedDate
                            let totalExpense=( from d in downloads where d.Downloader==c.ID select d.PurchasedPrice).Sum()
                            let totalEarnings = (from d in downloads where d.Seller == c.ID select d.PurchasedPrice).Sum()
                            where c.IsActive==true
                            select new Profile {
                                Id=c.ID,
                                FirstName=c.FirstName,
                                LastName=c.LastName,
                                EmailID=c.EmailID,
                                joinDate= dt.ToString("dd/MM/yyyy"),
                                userBooksUnderReview=underReview,
                                userPublishedBook=published,
                                userDownloadedBook=downloaded,
                                userTotalExpense= Convert.ToInt32(totalExpense),
                                userTotalEarning= Convert.ToInt32(totalEarnings)



                            });

            if (search != null && search != "")
                multiple = multiple.Where(m => m.FirstName.ToLower().Contains(search.ToLower()) ||
                m.LastName.ToLower().Contains(search.ToLower()) || m.EmailID.ToLower().Contains(search.ToLower())
                );
            ViewBag.Search = search;

            return View(multiple.OrderByDescending(x => x.joinDate).ToList().ToPagedList(page ?? 1, pageSize));
          
        }



        public ActionResult memberDetails( int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<tblUser> users = _Context.tblUsers.ToList();
            List<tblUserProfile> userProfiles = _Context.tblUserProfiles.ToList();

            var multiple = (from c in users 
                            join t1 in userProfiles on c.ID equals t1.UserID
                            where c.ID==id
                            select new MultipleData{ User=c, userProfile=t1 }).ToList();
            return View(multiple);

        }
        public PartialViewResult memberNote(int? page,int id)
        {
            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize + 1;
            else
                ViewBag.Count = 1;
            List<tblSellerNote> sellerNotes = _Context.tblSellerNotes.ToList();
            List<tblNoteCategory> noteCategories = _Context.tblNoteCategories.ToList();
            List<tblDownload> downloads = _Context.tblDownloads.ToList();

            List<tblReferenceData> referenceDatas = _Context.tblReferenceDatas.ToList();
            ViewBag.id = id;
            var multiple = (from c in sellerNotes
                            where c.SellerID == id
                            join t1 in noteCategories on c.Category equals t1.ID
                            join t2 in referenceDatas on c.Status equals t2.ID
                            let bookDownloads = (_Context.tblDownloads.Where(m => m.NoteID == c.ID && m.IsSellerHasAllowedDownload == true).Count())
                            let bookEarning = (from book in downloads where book.Seller== id && book.NoteID ==c.ID select book.PurchasedPrice).Sum()
                            let dt =(DateTime)c.CreatedDate
                            let s = t2.Values
                           
                            
                            select new PublishedNoteDetails
                            {
                                id=c.ID,
                                Title=c.Title,
                                categoryName=t1.Name,
                                status=s,
                                totalDownloads=bookDownloads,
                                totalEarn= Convert.ToInt32(bookEarning),
                                createdDate= dt.ToString("dd/MM/yyyy"),
                                date=c.PublishedDate


                            }
                            );



            return PartialView(multiple.ToList().ToPagedList(page ?? 1, pageSize));
        }


       public ActionResult deleteUser(int? ID)
        {
            if (ID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var user = _Context.tblUsers.Find(ID);
            var user_book = _Context.tblSellerNotes.Where(m => m.SellerID == ID).ToList();
            if (user == null)
                return HttpNotFound();
            user.IsActive = false;
            user_book.ForEach(m => m.IsActive = false);
            user_book.ForEach(m => m.Status = 11);

            _Context.SaveChanges();


            return RedirectToAction("Member");
        }

       [HttpGet]
        public ActionResult systemConfig()
        {
            var obj = _Context.tblSystemConfigurations.ToList();
            ViewBag.UserProfile = obj.FirstOrDefault(model => model.Key == "defaultProfilePic").Values;
            ViewBag.supportEmail= obj.FirstOrDefault(model => model.Key == "supportEmialID").Values;
            ViewBag.supportPhnNo = obj.FirstOrDefault(model => model.Key == "supportPhnNo").Values;
            ViewBag.notificationEmailID = obj.FirstOrDefault(model => model.Key == "notificationEmailID").Values;
            ViewBag.TwitterURL = obj.FirstOrDefault(model => model.Key == "TwitterURL").Values;
            ViewBag.FacebookURL = obj.FirstOrDefault(model => model.Key == "FacebookURL").Values;
            ViewBag.LinkedinURL = obj.FirstOrDefault(model => model.Key == "LinkedinURL").Values;
            ViewBag.defaultNoteImg = obj.FirstOrDefault(model => model.Key == "defaultNoteImg").Values;
            return View();

        }
        [HttpPost]
        public ActionResult systemConfig(systemConfig model)

        {
            ImgtoStr img = new ImgtoStr();
            var defaultNoteImg = _Context.tblSystemConfigurations.Where(m => m.Key.Equals("defaultNoteImg")).FirstOrDefault();
            string note = defaultNoteImg.Values;
            var defaultProfilePic = _Context.tblSystemConfigurations.Where(m => m.Key.Equals("defaultProfilePic")).FirstOrDefault();
            string user = defaultProfilePic.Values;
            string defaultpath = Server.MapPath(string.Format("~/Content/Files/Default"));
            if (!Directory.Exists(defaultpath))
            {
                Directory.CreateDirectory(defaultpath);
            }

            if (model.defaultNoteImg != null)
            {
                string notePic = Path.GetFileName(model.defaultNoteImg.FileName);
                string notePic2 = Path.Combine(defaultpath, notePic);
                model.defaultNoteImg.SaveAs(notePic2);
                note=img.convert(notePic2);
            }

            if (model.defaultProfilePic != null)
            {
                string UserPic = Path.GetFileName(model.defaultProfilePic.FileName);
                string UserPic2 = Path.Combine(defaultpath, UserPic);
                model.defaultProfilePic.SaveAs(UserPic2);
                user = img.convert(UserPic2);
            }
            
            string[] modelkey = { "supportEmialID", "supportPhnNo", "notificationEmailID", "TwitterURL", "facebookURL", "LinkedinURL", "defaultNoteImg", "defaultProfilePic" };
            string[] modelvalue = { model.supportEmialID, model.supportPhnNo, model.notificationEmailID, model.FacebookURL, model.TwitterURL, model.LinkedinURL,note , user };


            for (int i = 0; i < modelkey.Length; i++)
            {
                var temp = modelkey[i];
                var obj = _Context.tblSystemConfigurations.Where(m => m.Key.Equals(temp)).FirstOrDefault();
                obj.Values = modelvalue[i];





            }
            _Context.SaveChanges();
            ModelState.Clear();
            
            return RedirectToAction("","Admin");

        }

        public ActionResult spam(int? page, string search)
        {

            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize + 1;
            else
                ViewBag.Count = 1;
            List<tblSellerNotesReportedIssue> tblSellerNotesReporteds = _Context.tblSellerNotesReportedIssues.ToList(); 
            List<tblUser> tblUser = _Context.tblUsers.ToList();
            List<tblSellerNote> sellerNotes = _Context.tblSellerNotes.ToList();
            List<tblNoteCategory> noteCategories = _Context.tblNoteCategories.ToList();

            var multiple = (from c in tblSellerNotesReporteds
                            join t1 in tblUser on c.ReportedByID equals t1.ID
                            join t2 in sellerNotes on c.NoteID equals t2.ID
                            join t3 in noteCategories on t2.Category equals t3.ID
                            select new MultipleData { reportedIssue = c, User = t1, sellerNote = t2, NoteCategory = t3 });
           if(search!=null && search != "")
            {
                multiple = multiple.Where(m => m.User.FirstName.ToLower().Contains(search.ToLower()) ||
                  m.User.LastName.ToLower().Contains(search.ToLower()) ||
                  m.sellerNote.Title.ToLower().Contains(search.ToLower()) ||
                  m.NoteCategory.Name.ToLower().Contains(search.ToLower())
                 
                );
                ViewBag.search = search;
            }


            return View(multiple.ToList().ToPagedList(page ?? 1, pageSize));
        }


        public ActionResult deleteSpamReport(int? ID)
        {
            if (ID == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var spam = _Context.tblSellerNotesReportedIssues.Find(ID);
            if (spam == null)
                return HttpNotFound();
            _Context.tblSellerNotesReportedIssues.Remove(spam);
            _Context.SaveChanges();


            return RedirectToAction("spam");
        }


        public ActionResult rejectedNotes(int ? page, string Search, string Seller)
        {
            var Usertbl = _Context.tblUsers.ToList().Where(m => m.RoleID == 103);
            SelectList UserList = new SelectList(Usertbl, "ID", "FirstName");
            ViewBag.UserList = UserList;

            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize + 1;
            else
                ViewBag.Count = 1;
            List<tblUser> tblUsersList = _Context.tblUsers.ToList();
            List<tblSellerNote> tblSellerNotes = _Context.tblSellerNotes.ToList();
            List<tblNoteCategory> tblNoteCategories = _Context.tblNoteCategories.ToList();

            int user_id = (from user in _Context.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            


                var multiple = (from i in tblSellerNotes
                                join t1 in tblUsersList on i.SellerID equals t1.ID
                                join t2 in tblNoteCategories on i.Category equals t2.ID
                                join t3 in tblUsersList on i.ActionBy equals t3.ID
                                where i.Status == 10

                                select new MultipleData { sellerNote = i, actionBy = t3, User = t1, NoteCategory = t2 }) ;

            if (Seller != null)
                multiple = multiple.Where(m => m.sellerNote.SellerID.ToString().Equals(Seller));

            if (Search != null && Search !="")
                multiple = multiple.Where(m => m.sellerNote.Title.ToLower().Contains(Search.ToLower()));
            ViewBag.Search = Search;
            return View(multiple.ToList().ToPagedList(page ?? 1, pageSize));
         


          
        }
        public ActionResult downloadedNote(int? page, string Note, string Seller, string Buyer, string Search)
        {
            var UserList = _Context.tblUsers.ToList().Where(m => m.RoleID == 103); ;
            SelectList list = new SelectList(UserList, "Id", "FirstName");
            ViewBag.UserList = list;

            var NoteList = _Context.tblSellerNotes.ToList().Where(m=>m.Status==9);
            SelectList noteList = new SelectList(NoteList, "Id", "Title");
            ViewBag.NoteList = noteList;



            int pageSize = 5;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize + 1;
            else
                ViewBag.Count = 1;
            List<tblUser> users = _Context.tblUsers.ToList();
            List<tblDownload> downloads = _Context.tblDownloads.ToList();

            var data = (from notes in downloads
                        join seller in users on notes.Seller equals seller.ID
                        join buyer in users on notes.Downloader equals buyer.ID 
                        where notes.IsAttachmentDownloaded == true
                       select new MultipleData { download = notes, User=seller, buyer=buyer });


            if (Note != null && Note!="")
            {
                data = data.Where(m => m.download.NoteID.ToString().Equals(Note));
            }
            if (Seller != null && Seller != "")
            {
                data = data.Where(m => m.download.Seller.ToString().Equals(Seller));
            }
            if (Buyer != null && Buyer != "")
            {
                data = data.Where(m => m.download.Downloader.ToString().Equals(Buyer));
            }
            if (Search != null && Search != "")
            {
                data = data.Where(m => m.download.NoteTitle.ToLower().Contains(Search.ToLower()));
                ViewBag.Search = Search;
            }

            return View(data.ToList().ToPagedList(page ?? 1, pageSize));

        }



        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                using (var _Context = new NotesMarketPlaceEntities())
                {
                    // get current user profile image
                    var currentUserImg = (from details in _Context.tblUserProfiles
                                          join users in _Context.tblUsers on details.UserID equals users.ID
                                          where users.EmailID == requestContext.HttpContext.User.Identity.Name
                                          select details.ProfilePicture).FirstOrDefault();

                    if (currentUserImg == null)
                    {
                        // get deafult image
                        var defaultImg = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "defaultProfilePic");
                        ViewBag.UserProfile = defaultImg.Values;
                    }
                    else
                    {
                        ViewBag.UserProfile = currentUserImg;
                    }
                }
            }
        }
    }
}



using NoteMarketPlace.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{

    [Authorize(Roles ="Member")]
    public class UserController : Controller

    {


        private NotesMarketPlaceEntities dbobj = new NotesMarketPlaceEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public new ActionResult Profile()
        {
            return View();
        }
        public ActionResult addnote()
        {
            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var NoteCategory= entities.tblNoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;
            

            var NoteType = entities.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = entities.tblCountries.ToList();
            SelectList CountryList = new SelectList(CountryName, "ID", "Name");
            ViewBag.Country = CountryList;
            


            return View();



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addnote(tblSellerNote model)
        {


            NotesMarketPlaceEntities entities = new NotesMarketPlaceEntities();
            var NoteCategory = entities.tblNoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteType = entities.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = entities.tblCountries.ToList();
            SelectList CountryList = new SelectList(CountryName, "ID", "Name");
            ViewBag.Country = CountryList;







            string name = User.Identity.Name;
            int u = (from user in dbobj.tblUsers where user.EmailID == name select user.ID).Single();
            string book_title = model.Title;
            string notename_fullpath = null;
            string picname_fullpath = null;
            string previewname_fullpath = null;

            tblSellerNote obj = new tblSellerNote();





              if (model.uploadNote != null)
               {
                   string notepath = Server.MapPath("~/App_Data/Files");
                   string notename = Path.GetFileName(model.uploadNote.FileName);
                   notename_fullpath = Path.Combine(notepath, notename);
                   model.uploadNote.SaveAs(notename_fullpath);




               }
            else
            {
                ViewBag.Message = "Please Upload You File";
                return View();
            }

            if (model.displayPic != null)
            {
                string picpath = Server.MapPath("~/App_Data/Files");
                string picname = Path.GetFileName(model.displayPic.FileName);
                picname_fullpath = Path.Combine(picpath, picname);
                model.displayPic.SaveAs(picname_fullpath);
                obj.DisplayPicture = picname_fullpath;


            }
            if (model.notePreview != null)
            {
                string previewpath = Server.MapPath("~/App_Data/Files");
                string previewname = Path.GetFileName(model.notePreview.FileName);
                previewname_fullpath = Path.Combine(previewpath, previewname);
                model.notePreview.SaveAs(previewname_fullpath);
                obj.NotesPreview = previewname_fullpath;

            }





            try
            {
                
                    
                    obj.SellerID = u;
                    obj.Title = model.Title;
                    obj.Category = model.Category;
                    obj.NoteType = model.NoteType;
                    obj.NumberofPages = model.NumberofPages;
                    obj.Description = model.Description;
                    obj.UniversityName = model.UniversityName;
                    obj.Country = model.Country;
                    obj.Course = model.Course;
                    obj.CourseCode = model.CourseCode;
                    obj.Professor = model.Professor;
                    obj.Status = 7;
               
                    obj.IsActive = true;
                    obj.SellingPrice = model.SellingPrice;
                    obj.IsPaid = model.IsPaid;

                    dbobj.tblSellerNotes.Add(obj);
                    dbobj.SaveChanges();

                
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                
            }


            int book_id = (from record in dbobj.tblSellerNotes where record.SellerID == u && record.Title ==book_title select record.ID).First();

            try
            {
                tblSellerNotesAttachement sellerattachment = new tblSellerNotesAttachement();
                sellerattachment.NoteID = book_id;
                sellerattachment.FilePath = notename_fullpath;
                sellerattachment.FileName = book_title;
                sellerattachment.CreatedBy = u;
                sellerattachment.CreatedDate = DateTime.Now;
                sellerattachment.IsActive = true;
                dbobj.tblSellerNotesAttachements.Add(sellerattachment);
                dbobj.SaveChanges();
                
            }
            catch
            {

            }
            return View();
        }

    }
}
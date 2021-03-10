using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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



            int lastid = (from row in dbobj.tblSellerNotes orderby row.ID descending select row.ID).FirstOrDefault();
            lastid += 1;
            string defaultpath = Server.MapPath(string.Format("~/Content/Files/{0}", lastid));
            if(!Directory.Exists(defaultpath))
            {
                Directory.CreateDirectory(defaultpath);
            }

            if (model.uploadNote != null)
               {
                   
                   string notename = Path.GetFileName(model.uploadNote.FileName);
                   notename_fullpath = Path.Combine(defaultpath, notename);
                   model.uploadNote.SaveAs(notename_fullpath);




               }
            else
            {
                ViewBag.Message = "Please Upload You File";
                return View();
            }

            if (model.displayPic != null)
            {
                string picname = Path.GetFileName(model.displayPic.FileName);
                picname_fullpath = Path.Combine(defaultpath, picname);
                model.displayPic.SaveAs(picname_fullpath);
                obj.DisplayPicture = picname_fullpath;


            }
            if (model.notePreview != null)
            {
                string previewname = Path.GetFileName(model.notePreview.FileName);
                previewname_fullpath = Path.Combine(defaultpath, previewname);
                model.notePreview.SaveAs(previewname_fullpath);
                obj.NotesPreview = previewname_fullpath;

            }

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
                obj.CreatedDate = DateTime.Now;
                obj.IsActive = true;
                
                obj.IsPaid = model.IsPaid;
                if (obj.IsPaid)
                {
                    obj.SellingPrice = model.SellingPrice;
                }
                else
                    obj.SellingPrice = 0;



                dbobj.tblSellerNotes.Add(obj);
                dbobj.SaveChanges();


                int book_id = (from record in dbobj.tblSellerNotes where record.SellerID == u && record.Title == book_title orderby record.ID descending select record.ID).First();

                    tblSellerNotesAttachement sellerattachment = new tblSellerNotesAttachement();
                    sellerattachment.NoteID = book_id;
                    sellerattachment.FilePath = notename_fullpath;
                    sellerattachment.FileName = book_title;
                    sellerattachment.CreatedBy = u;
                    sellerattachment.CreatedDate = DateTime.Now;
                    sellerattachment.IsActive = true;
                    dbobj.tblSellerNotesAttachements.Add(sellerattachment);
                    dbobj.SaveChanges();

                    ModelState.Clear();


            return View();


            
           
        }



        public ActionResult showBook(int? id)
        {
            if(id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id);
            if (tblSeller == null)
                return HttpNotFound();

            List<tblSellerNote> tblSellerNotes = dbobj.tblSellerNotes.ToList();
            List<tblCountry> tblCountries = dbobj.tblCountries.ToList();
            List<tblNoteCategory> tblNoteCategories = dbobj.tblNoteCategories.ToList();

            var multiple = from c in tblSellerNotes
                           join t1 in tblCountries on c.Country equals t1.ID
                           join t2 in tblNoteCategories on c.Category equals t2.ID
                           where c.ID == id
                           select new MultipleData { sellerNote = c, Country = t1, NoteCategory=t2 };

            return View(multiple);
        }

    }
}
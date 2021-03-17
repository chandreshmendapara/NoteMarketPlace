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

    [Authorize(Roles = "Member")]
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
            var NoteCategory = entities.tblNoteCategories.ToList();
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
            if (!Directory.Exists(defaultpath))
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



        [AllowAnonymous]
        public ActionResult showBook(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
            var user_id = dbobj.tblUsers.Where(m => m.EmailID == User.Identity.Name && m.RoleID != 103).FirstOrDefault();
            if (user_id != null)
                goto eligible;
            var tblSeller = dbobj.tblSellerNotes.Where(m => m.ID == id && m.Status == 9).FirstOrDefault();
            if (tblSeller == null)
                return HttpNotFound();
            eligible:
            List<tblSellerNote> tblSellerNotes = dbobj.tblSellerNotes.ToList();
            List<tblCountry> tblCountries = dbobj.tblCountries.ToList();
            List<tblNoteCategory> tblNoteCategories = dbobj.tblNoteCategories.ToList();

            var multiple = from c in tblSellerNotes
                           join t1 in tblCountries on c.Country equals t1.ID
                           join t2 in tblNoteCategories on c.Category equals t2.ID
                           where c.ID == id
                           select new MultipleData { sellerNote = c, Country = t1, NoteCategory = t2 };

            return View(multiple);
        }





        public ActionResult FreeDownLoad(int? id)
        {


            var user_email = dbobj.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblSeller = dbobj.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();
            var user_id = user_email.ID;

            /* if (id == null)
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             else*/
            if (!tblSeller.IsPaid)
            {
                if (tblSeller == null || tblSeller.Status != 9)
                    return HttpNotFound();

                else if (tblSeller != null && tblSeller.Status == 9)
                {

                    string path = (from sa in dbobj.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                    string category = (from c in dbobj.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                    tblDownload obj = new tblDownload();
                    obj.NoteID = tblSeller.ID;
                    obj.Seller = tblSeller.SellerID;
                    obj.Downloader = user_id;
                    obj.IsSellerHasAllowedDownload = true;
                    obj.AttachmentPath = path;
                    obj.IsAttachmentDownloaded = true;
                    obj.IsPaid = false;
                    obj.PurchasedPrice = tblSeller.SellingPrice;
                    obj.NoteTitle = tblSeller.Title;
                    obj.NoteCategory = category;

                    obj.CreatedDate = DateTime.Now;
                    dbobj.tblDownloads.Add(obj);
                    dbobj.SaveChanges();


                    //#codebyChandreshMendapara

                    string filename = (from sa in dbobj.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();
                    filename += ".pdf";
                    byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

                }
            }
            return HttpNotFound();

        }


        public ActionResult AskforDownload(int id)
        {
            var user_email = dbobj.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblSeller = dbobj.tblSellerNotes.Where(m => m.ID == id).FirstOrDefault();
            var user_id = user_email.ID;

            //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
            if (tblSeller == null || tblSeller.Status != 9)
                return HttpNotFound();

            else if (tblSeller != null && tblSeller.Status == 9)
            {


                string path = (from sa in dbobj.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                string category = (from c in dbobj.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                string seller_name = (from c in dbobj.tblUsers where c.ID == tblSeller.SellerID select c.FirstName).First().ToString();
                string seller_lname = (from c in dbobj.tblUsers where c.ID == tblSeller.SellerID select c.LastName).First().ToString();
                seller_name += " " + seller_lname;
                tblDownload obj = new tblDownload();
                obj.NoteID = tblSeller.ID;
                obj.Seller = tblSeller.SellerID;
                obj.Downloader = user_id;
                obj.IsSellerHasAllowedDownload = false;
                obj.AttachmentPath = path;
                obj.IsAttachmentDownloaded = false;
                obj.IsPaid = true;
                obj.PurchasedPrice = tblSeller.SellingPrice;
                obj.NoteTitle = tblSeller.Title;
                obj.NoteCategory = category;
                obj.CreatedDate = DateTime.Now;

                dbobj.tblDownloads.Add(obj);
                dbobj.SaveChanges();
                ViewBag.Msg = "Request Added";

                return Json(new { success = true, responseText = seller_name }, JsonRequestBehavior.AllowGet);
                //#codebyChandreshMendapara


            }




            return Json(new { success = false, responseText = "Not Completed." }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BuyerRequest()
        {


            List<tblUser> tblUsersList = dbobj.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobj.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobj.tblUserProfiles.ToList();

            int user_id = (from user in dbobj.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = from d in tblDownloadList
                           join t1 in tblUsersList on d.Downloader equals t1.ID
                           join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                           where d.Seller == user_id && d.IsSellerHasAllowedDownload == false
                           select new MultipleData { download = d, User = t1, userProfile = t2 };

            return View(multiple);
        }




        //when buyer allowed for download #CodebyChandreshMendapara
        public ActionResult BuyerAllowed(int id)
        {


            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblDownloads.Where(m => m.ID.Equals(id)).FirstOrDefault();


            if (obj != null)
            {
                // int id = admin_id.ID;
                obj.IsSellerHasAllowedDownload = true;
               
                a.SaveChanges();
            }

                return RedirectToAction("BuyerRequest","User");
        }



        public ActionResult downloads()
        {




            List<tblUser> tblUsersList = dbobj.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobj.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobj.tblUserProfiles.ToList();

            int user_id = (from user in dbobj.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = from d in tblDownloadList
                           join t1 in tblUsersList on d.Downloader equals t1.ID
                           join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                           where d.Downloader == user_id && d.IsSellerHasAllowedDownload == true
                           select new MultipleData { download = d, User = t1, userProfile = t2 };

            return View(multiple);

        }
    }
}
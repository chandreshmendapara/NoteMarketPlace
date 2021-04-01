using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
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
            var tbldownload = dbobj.tblDownloads;
            var tblseller = dbobj.tblSellerNotes;
            var user_id = dbobj.tblUsers.Where(m => m.EmailID == User.Identity.Name).FirstOrDefault();
            
           
            ViewBag.NoofSoldNote = tbldownload.Where(m => m.IsSellerHasAllowedDownload == true && m.Seller == user_id.ID).Count();
            ViewBag.NoofDownload = tbldownload.Where(m => m.IsSellerHasAllowedDownload == true && m.Downloader == user_id.ID).Count();
            ViewBag.Earnings = tbldownload.Where(m => m.IsSellerHasAllowedDownload == true && m.Seller == user_id.ID).Sum(m => m.PurchasedPrice);
            ViewBag.Rejectednote = tblseller.Count(m => m.SellerID == user_id.ID && m.Status == 10);
            ViewBag.BuyerReq = tbldownload.Where(m => m.IsSellerHasAllowedDownload == false && m.Seller == user_id.ID).Count();
            return View();
        }

        public new ActionResult Profile()
        {
            string user_email = User.Identity.Name;
            var user = dbobj.tblUsers.Where(m => m.EmailID == user_email).FirstOrDefault();
            var user_profile = dbobj.tblUserProfiles.Where(m => m.UserID == user.ID).FirstOrDefault();
            ViewBag.fname = user.FirstName;
            ViewBag.lnmae = user.LastName;
            ViewBag.Email = user_email;
            ViewBag.Dob = user_profile.DOB;
            ViewBag.user_gender = user_profile.Gender;
            ViewBag.User_code = user_profile.PhoneNumber_CountryCode;
            ViewBag.User_phn = user_profile.PhoneNumber;
            ViewBag.ad1 = user_profile.AddressLine1;
            ViewBag.ad2 = user_profile.AddressLine2;
            ViewBag.city = user_profile.City;
            ViewBag.zip = user_profile.ZipCode;
            ViewBag.user_country = user_profile.Country;
            ViewBag.state=user_profile.State;
            ViewBag.Uni = user_profile.University;
            ViewBag.clg = user_profile.College;
            //#CodebyChandreshMendapara


            var CountryName = dbobj.tblCountries.ToList();
            List<SelectListItem> CountryList = new SelectList(CountryName, "Name", "Name").ToList(); 
            CountryList.RemoveAll(i => i.Value == ViewBag.user_country);
            CountryList.Insert(0, (new SelectListItem { Text = ViewBag.user_country, Value = ViewBag.user_country }));
            ViewBag.Country =  CountryList;
           

            var Gender = dbobj.tblReferenceDatas.Where(m => m.RefCategory == "Gender").ToList();
            List<SelectListItem> GenderList = new SelectList(Gender, "ID", "Values").ToList();
            GenderList.RemoveAll(i => i.Text.Equals(ViewBag.user_gender));
            var id = Gender.Where(m => m.Values.Equals("Female")).FirstOrDefault();

            int mm = ViewBag.user_gender; //dbobj.tblReferenceDatas.Where(m => m.Values == ViewBag.user_gender).FirstOrDefault();  
           // GenderList.Insert(0, (new SelectListItem { Text = mm , Value = id.Values }));

            ViewBag.Gender = GenderList;

            var Countrycode = dbobj.tblCountries.ToList();
            SelectList CcodeList = new SelectList(Countrycode, "CountryCode", "CountryCode");
            ViewBag.Ccode = CcodeList;
            return View();
        }
        [HttpPost]
        public new ActionResult Profile(Profile model)
        {
           
            var CountryName = dbobj.tblCountries.ToList();
            SelectList CountryList = new SelectList(CountryName, "Name", "Name");
            ViewBag.Country = CountryList;


            var Gender = dbobj.tblReferenceDatas.ToList().Where(m => m.RefCategory == "Gender");

            SelectList GenderList = new SelectList(Gender, "Values", "Values");
            ViewBag.Gender = GenderList;

            var Countrycode = dbobj.tblCountries.ToList();
            SelectList CcodeList = new SelectList(Countrycode, "CountryCode", "CountryCode");
            
            dbobj.SaveChanges();

            return RedirectToAction("", "User");
        }


        
        public ActionResult addnote()
        {
            var NoteCategory = dbobj.tblNoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteType = dbobj.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = dbobj.tblCountries.ToList();
            SelectList CountryList = new SelectList(CountryName, "ID", "Name");
            ViewBag.Country = CountryList;



            return View();



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addnote(tblSellerNote model)
        {


            var NoteCategory = dbobj.tblNoteCategories.ToList();
            SelectList list = new SelectList(NoteCategory, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteType = dbobj.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteType, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryName = dbobj.tblCountries.ToList();
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
               //  = picname_fullpath;
                ImgtoStr imgto = new ImgtoStr();
                obj.DisplayPicture= imgto.convert(picname_fullpath);


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
            var check_download = dbobj.tblDownloads.Where(m => m.NoteID == id && m.Downloader == user_id).FirstOrDefault();

            if(check_download!=null)
                return Json(new { success = true, alertMsg = "you downloaded this book already." }, JsonRequestBehavior.AllowGet);



            //tblSellerNote tblSeller = dbobj.tblSellerNotes.Find(id).;
            if (tblSeller == null || tblSeller.Status != 9)
                return HttpNotFound();

            else if (tblSeller != null && tblSeller.Status == 9)
            {


                var seller= dbobj.tblUsers.Where(m => m.ID==tblSeller.SellerID).FirstOrDefault();
                string path = (from sa in dbobj.tblSellerNotesAttachements where sa.NoteID == tblSeller.ID select sa.FilePath).First().ToString();
                string category = (from c in dbobj.tblNoteCategories where c.ID == tblSeller.Category select c.Name).First().ToString();
                string seller_name = seller.FirstName;
                seller_name += " " + seller.LastName;
                string buyer_name = user_email.FirstName;
                buyer_name += " " + user_email.LastName;
                List<string> buyer_email = new List<string>();
                buyer_email.Add(seller.EmailID);

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



      

                string subject = buyer_name + " wants to purchase your notes";
                string body = "Hello"+" " + seller_name+ ",<br/><br/>We would like to inform you that, " + buyer_name+" wants to purchase your notes." +
                    " Please see Buyer Requests tab and allow download access to Buyer if you have received " +
                    "the payment from him.<br/><br/>Regards,<br/>Notes Marketplace";
                Mailer mailer = new Mailer();
                mailer.sendMail(subject,body,buyer_email);
                dbobj.SaveChanges();
                ViewBag.Msg = "Request Added";

                return Json(new { success = true, responseText = seller_name }, JsonRequestBehavior.AllowGet);
                //#codebyChandreshMendapara


            }




            return Json(new { success = false, responseText = "Not Completed." }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BuyerRequest(int? page)
        {
            int pageSize = 10;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;


            List<tblUser> tblUsersList = dbobj.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobj.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobj.tblUserProfiles.ToList();

            int user_id = (from user in dbobj.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = (from d in tblDownloadList
                           join t1 in tblUsersList on d.Downloader equals t1.ID
                           join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                           where d.Seller == user_id && d.IsSellerHasAllowedDownload == false
                           select new MultipleData { download = d, User = t1, userProfile = t2 }).ToList().ToPagedList(page ?? 1, pageSize); ; ;

            return View(multiple);
        }




        //when buyer allowed for download #CodebyChandreshMendapara
        public ActionResult BuyerAllowed(int id)
        {


            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var obj = a.tblDownloads.Where(m => m.ID.Equals(id)).FirstOrDefault();
            int buyer_id = obj.Downloader;
            int seller_id = obj.Seller;
            var buyer = a.tblUsers.Where(m => m.ID.Equals(buyer_id)).FirstOrDefault();
            var seller = a.tblUsers.Where(m => m.ID.Equals(seller_id)).FirstOrDefault();
            if (obj != null)
            {
                // int id = admin_id.ID;
                obj.IsSellerHasAllowedDownload = true;

                List<string> buyer_email = new List<string>();
                buyer_email.Add(buyer.EmailID);
                string buyer_name = buyer.FirstName;
                buyer_name += " " + buyer.LastName;

                string seller_name = seller.FirstName;
                seller_name += " " + seller.LastName;
                string subject = seller_name + " Allows you to download a note";
                Mailer mailer = new Mailer();
                string body= "Hello " +buyer_name +
                    ",<br/><br/>We would like to inform you that,"+seller_name+" Allows you to download a note." +
                    "Please login and see My Download tabs to download particular note." +
                    "<br/><br/>Regards,<br/>Notes Marketplace";
                mailer.sendMail(subject,body,buyer_email);


                a.SaveChanges();
            }

            return RedirectToAction("BuyerRequest", "User");
        }



        public ActionResult downloads(int ? page)
        {
            int pageSize = 10;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;


            List<tblUser> tblUsersList = dbobj.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobj.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobj.tblUserProfiles.ToList();

            int user_id = (from user in dbobj.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = (from d in tblDownloadList
                            join t1 in tblUsersList on d.Downloader equals t1.ID
                            join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                            where d.Downloader == user_id && d.IsSellerHasAllowedDownload == true


                            select new MultipleData { download = d, User = t1, userProfile = t2 }).ToList().ToPagedList(page ?? 1, pageSize); ;
          
            return View(multiple);

        }



        //after buyer allowd download user can download from here
        public ActionResult downloadBook(int id)
        {
            NotesMarketPlaceEntities a = new NotesMarketPlaceEntities();
            var user_id = dbobj.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
            var download = dbobj.tblDownloads.Where(m => m.ID == id &&  m.Downloader.Equals(user_id.ID)).FirstOrDefault();
           
            
            if (download == null || download.IsSellerHasAllowedDownload!= true)
                    return HttpNotFound();
            else if (download != null)
            {
                string path = download.AttachmentPath;
                string filename = download.NoteTitle;
                filename += ".pdf";


                download.IsAttachmentDownloaded = true;

                download.AttachmentDownloadedDate = DateTime.Now;
                
                dbobj.SaveChanges();





                //#codebyChandreshMendapara

                //string filename = (from sa in dbobj.tblSellerNotesAttachements where sa.NoteID == id select sa.FileName).First().ToString();

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

            }



            return HttpNotFound();


        }

        public ActionResult soldNote(int?page)
        {
            int pageSize = 10;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;



            List<tblUser> tblUsersList = dbobj.tblUsers.ToList();
            List<tblDownload> tblDownloadList = dbobj.tblDownloads.ToList();
            List<tblUserProfile> tblUserProfilesList = dbobj.tblUserProfiles.ToList();

            int user_id = (from user in dbobj.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();

            var multiple = (from d in tblDownloadList
                            join t1 in tblUsersList on d.Downloader equals t1.ID
                            join t2 in tblUserProfilesList on d.Downloader equals t2.UserID
                            where d.Seller == user_id && d.IsSellerHasAllowedDownload == true


                            select new MultipleData { download = d, User = t1, userProfile = t2 }).ToList().ToPagedList(page ?? 1, pageSize); ;

            return View(multiple);

        }


        public ActionResult rejecteddownload(int id)
        {

            var user_id = dbobj.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();
            var download = dbobj.tblDownloads.Where(m => m.NoteID.Equals(id) && m.Seller==user_id.ID).FirstOrDefault();

            var attachments = dbobj.tblSellerNotesAttachements.Where(m => m.NoteID == id).FirstOrDefault();
            var seller = dbobj.tblSellerNotes.Where(m => m.ID == id && m.SellerID == user_id.ID && m.Status==10).FirstOrDefault();
            if (seller != null || download !=null)
            {
                string path = attachments.FilePath;
                string filename = attachments.FileName +".pdf";
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
            }
            else
                return HttpNotFound();


        }


        public ActionResult rejectedNotes(int ? page)
        {
            int pageSize = 10;

            List<tblUser> tblUsersList = dbobj.tblUsers.ToList();
            List <tblSellerNote> tblSellerNotes = dbobj.tblSellerNotes.ToList();
            List<tblNoteCategory> tblNoteCategories = dbobj.tblNoteCategories.ToList();

            int user_id = (from user in dbobj.tblUsers where user.EmailID == User.Identity.Name select user.ID).FirstOrDefault();
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize +1;
            else
                ViewBag.Count = 1;
            var multiple = (from d in tblSellerNotes
                            join t1 in tblUsersList on d.SellerID equals t1.ID
                            join t2 in tblNoteCategories on d.Category equals t2.ID
                            where d.SellerID == user_id && d.Status == 10


                            select new MultipleData { sellerNote = d, User = t1, NoteCategory = t2 }).ToList().ToPagedList(page ?? 1, pageSize);

            return View(multiple);
        }


        public ActionResult reportSpam(int id)
        {

            
            var user_email = dbobj.tblUsers.Where(m => m.EmailID.Equals(User.Identity.Name)).FirstOrDefault();

            var tblreport_check = dbobj.tblSellerNotesReportedIssues.Where(m => m.ReportedByID == user_email.ID && m.NoteID == id).FirstOrDefault();
            
            var downloader = dbobj.tblDownloads.Where(m => m.NoteID == id && m.Downloader == user_email.ID).FirstOrDefault();
            if (tblreport_check!= null)
                return Json(new { success = true, alertMsg = "you reported this book already." }, JsonRequestBehavior.AllowGet);

            else if (downloader == null )

                return Json(new { success = true, alertMsg = "you can't report this book first download." }, JsonRequestBehavior.AllowGet);


            else
            {
                string downloader_name = user_email.FirstName + " " + user_email.LastName;
                int seller_id = downloader.Seller;
                var seller = dbobj.tblUsers.Where(m => m.ID.Equals(seller_id)).FirstOrDefault();
                string seller_name = seller.FirstName + " " + seller.LastName;
                tblSellerNotesReportedIssue reportedIssue = new tblSellerNotesReportedIssue();
                reportedIssue.NoteID = id;
                reportedIssue.ReportedByID = user_email.ID;
                reportedIssue.AgainstDownloadID = downloader.Seller;
                reportedIssue.Remarks = "there is issue in book";
                reportedIssue.CreatedDate = DateTime.Now;

                dbobj.tblSellerNotesReportedIssues.Add(reportedIssue);

                List<string> admin_list = (from a in dbobj.tblUsers where a.RoleID!=103 select a.EmailID).ToList();

                string subject = downloader_name + "Reported an issue for " + downloader.NoteTitle;
                string body = "Hello Admins, <br/><br/>" + "We want to inform you that, "+ downloader_name +
                    " Reported an issue for "+ seller_name+"’s Note with title "+downloader.NoteTitle+". " +
                    "Please look at the notes and take required actions. <br/><br/>Regards,<br/>Notes Marketplace";
                  Mailer mailer = new Mailer();
                  mailer.sendMail(subject, body, admin_list);
                  dbobj.SaveChanges();
                  ViewBag.Msg = "Request Added";
                

                return Json(new { success = true, responseText = seller_name }, JsonRequestBehavior.AllowGet);
                  //#codebyChandreshMendapara
                   

            }




            return Json(new { success = false, responseText = "Not Completed." }, JsonRequestBehavior.AllowGet);

      } 
            }
        }
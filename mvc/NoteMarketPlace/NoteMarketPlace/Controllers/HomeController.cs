using NoteMarketPlace.Context;
using NoteMarketPlace.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NoteMarketPlace.Controllers
{
    public class HomeController : Controller
    {


        private NotesMarketPlaceEntities _Context;
        // GET: Admin

        public HomeController()
        {
            _Context = new NotesMarketPlaceEntities();

            ViewBag.daefaultNoteImg = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "defaultNoteImg").Values;
            ViewBag.facebook = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "FacebookURL").Values;
            ViewBag.twitter = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "TwitterURL").Values;
            ViewBag.linkedin = _Context.tblSystemConfigurations.FirstOrDefault(model => model.Key == "LinkedinURL").Values;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ModelState.Clear();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(Contact model)
        {


                if (ModelState.IsValid)
                {

                Mailer mailer = new Mailer();
                List<string> admin_list = (from a in _Context.tblUsers where a.RoleID != 103 select a.EmailID).ToList();

                mailer.sendMail(model.Subject, model.Message, admin_list);

                    
                     }
            return View();
        }


        public ActionResult Faq()
        {
            ViewBag.Message = "Your faq page.";

            return View();
        }
        public ActionResult SearchNote(int? page, int? NoteType, int? NoteCategory, string  University,string Country, int? Rating , string Course, string Search)
        {
            int pageSize = 9;
            if (page != null)
                ViewBag.Count = page * pageSize - pageSize + 1;
            else
                ViewBag.Count = 1;

            var NoteCategoryList = _Context.tblNoteCategories.ToList();
            SelectList list = new SelectList(NoteCategoryList, "ID", "Name");
            ViewBag.NoteCategory = list;


            var NoteTypeList = _Context.tblNoteTypes.ToList();
            SelectList NoteTypelist = new SelectList(NoteTypeList, "ID", "Name");
            ViewBag.NoteType = NoteTypelist;


            var CountryNameList = _Context.tblCountries.ToList();
            SelectList CountryList = new SelectList(CountryNameList, "ID", "Name");
            ViewBag.Country = CountryList;


            var sellerNotes = _Context.tblSellerNotes.ToList();

            var Uni = sellerNotes.GroupBy(test => test.UniversityName)
                   .Select(grp => grp.First())
                   .ToList();

            var CourseNameList = sellerNotes.GroupBy(test => test.Course)
                 .Select(grp => grp.First())
                 .ToList();

            SelectList UniList = new SelectList(Uni, "UniversityName", "UniversityName");
            ViewBag.University = UniList;

            SelectList CourseList = new SelectList(CourseNameList, "Course", "Course");
            ViewBag.Course = CourseList;

            List<SelectListItem> RatingList = new List<SelectListItem>();
            RatingList.Add(new SelectListItem() { Text = "1", Value = "1" });
            RatingList.Add(new SelectListItem() { Text = "2", Value = "2" });
            RatingList.Add(new SelectListItem() { Text = "3", Value = "3" });
            RatingList.Add(new SelectListItem() { Text = "4", Value = "4" });
            RatingList.Add(new SelectListItem() { Text = "5", Value = "5" });

            ViewBag.Rating = new SelectList(RatingList, "Value", "Text");






            ViewBag.Message = "Your note page.";

            List<tblSellerNote> tblSellerNotes = _Context.tblSellerNotes.ToList();
            List<tblCountry> tblCountries = _Context.tblCountries.ToList();

            var multiple = from c in tblSellerNotes
                           join t1 in tblCountries on c.Country equals t1.ID
                           where c.Status == 9
                           select new MultipleData { sellerNote = c, Country=t1};

            if(NoteType!=null)
                multiple = multiple.Where(m => m.sellerNote.NoteType.Equals(NoteType));

            if (NoteCategory != null)
                multiple = multiple.Where(m => m.sellerNote.Category.Equals(NoteCategory));

            if (University != null && University != "")
                multiple = multiple.Where(m => m.sellerNote.UniversityName.Equals(University));


            if (Country != null && Country!="")
                multiple = multiple.Where(m => m.sellerNote.Country.ToString().Equals(Country));


            if (Course != null && Course != "")
                multiple = multiple.Where(m => m.sellerNote.Course.Equals(Course));



            if (Search != null && Search != "")
            {
                multiple = multiple.Where(m => m.sellerNote.Title.ToLower().Contains(Search.ToLower()));
                ViewBag.SearchInput = Search;
            }


            ViewBag.Count = multiple.Count();
            return PartialView(multiple.ToPagedList(page ?? 1, pageSize));
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
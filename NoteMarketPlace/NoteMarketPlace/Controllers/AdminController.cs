using NoteMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
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
        public ActionResult addCountry()
        {
            return View();

        }
        public ActionResult ManageNoteType()
        {
            return View();

        }
        public ActionResult ManageNoteCategory()
        {
            return View();

        }
        public ActionResult ManageCountry()
        {
            return View();

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
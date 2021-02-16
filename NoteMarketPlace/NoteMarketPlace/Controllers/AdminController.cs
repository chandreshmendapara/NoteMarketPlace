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
        
    }
}
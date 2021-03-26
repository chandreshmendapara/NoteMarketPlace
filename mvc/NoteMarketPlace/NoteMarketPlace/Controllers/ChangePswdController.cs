using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoteMarketPlace.Controllers
{
    public class ChangePswdController : Controller
    {
        // GET: ChangePswd
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string oldPswd, string newPswd, string confirmPswd)
        {




            return View();
        }
    }
}
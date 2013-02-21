using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gu_s.Models;
using gu_s.ViewModel;

namespace gu_s.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /*public ActionResult Status()
        {
            return View(new StatusViewModel(_db.Countries.Count()));
        }*/
    }
}

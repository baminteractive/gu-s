using System.Web.Mvc;
using gu_s.Services;
using gu_s.ViewModel;

namespace gu_s.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckIp()
        {
            var statusViewModel = new StatusViewModel();
            var clientIp = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = Request.ServerVariables["REMOTE_ADDR"];
            }

            statusViewModel.Ip = clientIp;

            var ipLookup = new IpLookup();
            var result = ipLookup.LookupIp(clientIp);

            if (result.Matched)
            {
                statusViewModel.Country = result.Country.CountryName;
            }

            return View(statusViewModel);
        }

        /*public ActionResult Status()
        {
            return View(new StatusViewModel(_db.Countries.Count()));
        }*/
    }
}

using System;
using System.Web.Mvc;
using gu_s.Services;
using gu_s.ViewModel;

namespace gu_s.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult CheckIp()
        {
            var statusViewModel = new StatusViewModel();
            var clientIp = Request.ServerVariables["REMOTE_ADDR"];
            

            statusViewModel.Ip = clientIp;

            var result = new IpLookupResult();

            try
            {
                var ipLookup = new IpLookup();
                result = ipLookup.LookupIp(clientIp);
            }
            catch (Exception exception)
            {
                result.Matched = false;
                result.Message = exception.Message;
            }

            if (result.Matched)
            {
                statusViewModel.Country = result.Country.CountryName;
            }

            statusViewModel.Message = result.Message != null ? result.Message : "";

            return View(statusViewModel);
        }

        /*public ActionResult Status()
        {
            return View(new StatusViewModel(_db.Countries.Count()));
        }*/
    }
}

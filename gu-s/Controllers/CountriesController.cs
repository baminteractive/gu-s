using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using gu_s.Services;

namespace gu_s.Controllers
{
    public class CountriesController : ApiController
    {
        // GET api/countries/5
        public HttpResponseMessage Get(string ip)
        {
            var ipLookup = new IpLookup();
            var result = ipLookup.LookupIp(ip);

            Trace.WriteLine(string.Format("Ip: {0} Matched: {1} Message: {2} Country: {3}",result.Ip,result.Matched.ToString(),result.Message,result.Country));

            return result.Matched ? Request.CreateResponse(HttpStatusCode.OK, result.Country) : Request.CreateResponse(HttpStatusCode.BadRequest, result.Message);
        }

    }
}

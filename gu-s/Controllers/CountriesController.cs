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

            return result.Matched ? Request.CreateResponse(HttpStatusCode.OK, result.Country) : Request.CreateResponse(HttpStatusCode.BadRequest, result.Message);
        }

    }
}

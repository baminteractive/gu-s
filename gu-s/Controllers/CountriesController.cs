using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using gu_s.Models;
using gu_s.Utilities;

namespace gu_s.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly GusContext _db = new GusContext();

        // GET api/countries/5
        public HttpResponseMessage Get(string ip)
        {
            var regex = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");

            var match = regex.Match(ip);

            if (match.Success)
            {
                var ipStringParts = ip.Split('.').ToList();
                var ipParts = ipStringParts.Select(i => Convert.ToInt32(i)).ToArray();
                var firstOctet = ipParts[0];
                var secondOctet = ipParts[1];
                var thirdOctet = ipParts[2];


                var search = (from country in _db.Countries
                             where country.StartFirstOctet <= firstOctet
                                   && country.EndFirstOctet >= firstOctet
                             select country);

                foreach (var country in search)
                {
                    var ipRange = new IpAddressRange(country.StartAddress, country.EndAddress);
                    if (ipRange.IsInRange(ip))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, country);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        // POST api/test
        [HttpPost]
        public HttpResponseMessage PostCountry(Country country)
        {
            if (ModelState.IsValid)
            {
                _db.Countries.Add(country);
                _db.SaveChanges();

                var response = Request.CreateResponse(HttpStatusCode.Created, country);

                return response;
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using gu_s.Models;

namespace gu_s.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly GusContext _db = new GusContext();

        // GET api/countries/5
        public string Get(string ip)
        {
            var regex = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");

            var match = regex.Match(ip);

            return match.Success.ToString();
        }

        // POST api/test
        public HttpResponseMessage Post([FromBody]Country value)
        {
            if (ModelState.IsValid)
            {
                _db.Countries.Add(value);
                _db.SaveChanges();

                var response = Request.CreateResponse(HttpStatusCode.Created, value);

                return response;
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

    }
}

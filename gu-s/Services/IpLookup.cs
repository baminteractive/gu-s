using System;
using System.Linq;
using System.Text.RegularExpressions;
using gu_s.Models;
using gu_s.Utilities;

namespace gu_s.Services
{
    public class IpLookup
    {
        private readonly MongoRepository _db = new MongoRepository();

        public IpLookupResult LookupIp(string ip)
        {
            var regex = new Regex(@"[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}");

            var match = regex.Match(ip);

            if (!match.Success)
            {
                return new IpLookupResult(ip,false, "Bad Ip");
            }

            var ipStringParts = ip.Split('.').ToList();
            var ipParts = ipStringParts.Select(i => Convert.ToInt32(i)).ToArray();
            var firstOctet = ipParts[0];

            var search = _db.Search(c => c.StartFirstOctet <= firstOctet && c.EndFirstOctet >= firstOctet);

            foreach (var country in search)
            {
                var ipRange = new IpAddressRange(country.StartAddress, country.EndAddress);
                if (ipRange.IsInRange(ip))
                {
                    return new IpLookupResult(ip, true, "", country);
                }
            }

            return new IpLookupResult(ip, false, "No Match");
        }
    }

    public class IpLookupResult
    {
        public bool Matched { get; set; }
        public string Message { get; set; }
        public Country Country { get; set; }
        public string Ip { get; set; }

        public IpLookupResult()
        {
            
        }

        public IpLookupResult(string ip, bool matched, string message, Country country = null)
        {
            Ip = ip;
            Matched = matched;
            Message = message;
            Country = country;
        }
    }
}
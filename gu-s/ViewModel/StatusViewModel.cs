using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gu_s.ViewModel
{
    public class StatusViewModel
    {
        public int CountryCount { get; set; }

        public StatusViewModel(int countryCount)
        {
            CountryCount = countryCount;
        }
    }
}
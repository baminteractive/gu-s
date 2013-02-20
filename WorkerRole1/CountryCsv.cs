using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;

namespace WorkerRole1
{
    public class CountryCsv
    {
        [CsvField(Index = 0)]
        public string StartIp { get; set; }

        [CsvField(Index = 1)]
        public string EndIp { get; set; }

        [CsvField(Index = 2)]
        public string StartIpv6 { get; set; }

        [CsvField(Index = 3)]
        public string EndIpv6 { get; set; }

        [CsvField(Index = 4)]
        public string CountryAbbr { get; set; }

        [CsvField(Index = 5)]
        public string CountryName { get; set; }
    }
}

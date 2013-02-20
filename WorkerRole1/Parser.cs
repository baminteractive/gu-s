using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Net;
using CsvHelper;
using Newtonsoft.Json;

namespace WorkerRole1
{
    public class Parser
    {
        public static void ParseCsvFile(string fileName)
        {
            var csv = new CsvReader(new StreamReader(fileName));

            var countryList = csv.GetRecords<CountryCsv>();

            foreach (var countryCsv in countryList)
            {
                var countryDictionary = new Dictionary<string, string>();
                
                //Break up Start address
                var startAddressSplit = countryCsv.StartIp.Split('.');
                var endAddressSplit = countryCsv.EndIp.Split('.');

                countryDictionary.Add("StartAddress",countryCsv.StartIp);
                countryDictionary.Add("EndAddress", countryCsv.EndIp);
                countryDictionary.Add("StartFirstOctet",startAddressSplit[0]);
                countryDictionary.Add("StartSecondOctet",startAddressSplit[1]);
                countryDictionary.Add("StartThirdOctet",startAddressSplit[2]);
                countryDictionary.Add("EndFirstOctet",endAddressSplit[0]);
                countryDictionary.Add("EndSecondOctet",endAddressSplit[1]);
                countryDictionary.Add("EndThirdOctet",endAddressSplit[2]);
                countryDictionary.Add("CountryAbbr",countryCsv.CountryAbbr);
                countryDictionary.Add("CountryName",countryCsv.CountryName);

                PostCountry(countryDictionary);
            }
        }

        private static void PostCountry(Dictionary<string,string> country)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://url");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(country);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }

    }
}

using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("WorkerRole1 entry point called", "Information");

            while (true)
            {
                var fileUrl = "http://geolite.maxmind.com/download/geoip/database/GeoIPCountryCSV.zip";
                var fileName = "GeoIPCountryCSV.zip";

                // Download the file
                Downloader.DownloadFile(fileUrl, fileName);

                // Extract the file
                Unarchive.ExtractZipFile(fileName,"");

                // Parse the file

                Thread.Sleep(new TimeSpan(0,1,0,0));
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}

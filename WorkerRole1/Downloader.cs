using System.Net;

namespace WorkerRole1
{
    public class Downloader
    {
        public static void DownloadFile(string fileUrl, string filename)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(fileUrl, filename);
            }
        }
    }
}

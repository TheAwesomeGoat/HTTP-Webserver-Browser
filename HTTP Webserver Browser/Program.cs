using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace HTTP_Webserver_Browser
{
    class Program
    {
        static SearchWebserver a = new SearchWebserver();
        static string BaseUrl = "http://172.24.30.1:6969";
        static void Main(string[] args)
        {
            var Folders = a.SearchAllFolders($"{BaseUrl}/");
            var Files = a.SearchFiles($"{BaseUrl}/", ".mp3");

            Console.ReadKey();
        }
    }
}

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
        static string baseurl = "http://172.24.30.1:6969";
        static void Main(string[] args)
        {
            var Folders = a.SearchFolders($"{baseurl}/");
            foreach (var Folder in Folders)
                LoadSubDirs(Folder);

            var Files = a.SearchFiles($"{baseurl}/", ".mp3");

            Console.ReadKey();
        }

        static void LoadSubDirs(string dir)
        {
            Console.WriteLine(dir);
            var Folders = a.SearchFolders($"{baseurl}{dir}");
            foreach (var Folder in Folders)
                LoadSubDirs(Folder);
        }
    }
}

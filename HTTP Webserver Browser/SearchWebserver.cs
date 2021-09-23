using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Webserver_Browser
{
    class SearchWebserver
    {
        private string sub(string str, int begin, int end)
        {
            return str.Substring(begin, end - begin);
        }

        private string Connect(string url)
        {
            using (WebClient client = new WebClient())
                return client.DownloadString(url);
        }

        public List<string> SearchFolders(string url)
        {
            int HrefBegin = 0;
            var Folders = new List<string>();
            string data = Connect(url);
            if (data != null)
            {
                while (true)
                {
                    HrefBegin = data.IndexOf("<a href=\"", HrefBegin + 1);
                    int HrefEnd = data.IndexOf("</a>", HrefBegin + 1) + 4;
                    int HrefDirEnd = data.IndexOf("\">", HrefBegin + 1);

                    if (HrefBegin == -1)
                        break;

                    string FullHref = sub(data, HrefBegin, HrefEnd);
                    string HrefDir = sub(data, HrefBegin + 9, HrefDirEnd);
                    string HrefText = sub(data, HrefDirEnd + 2, HrefEnd - 4);

                    if (!HrefDir.Contains("../") & HrefDir.EndsWith("/"))
                        Folders.Add(HrefDir);
                }
            }
            return Folders;
        }
        public List<string> SearchFiles(string url, string extension)
        {
            int HrefBegin = 0;
            var Files = new List<string>();
            string data = Connect(url);
            if (data != null)
            {
                while (true)
                {
                    HrefBegin = data.IndexOf("<a href=\"", HrefBegin + 1);
                    int HrefEnd = data.IndexOf("</a>", HrefBegin + 1) + 4;
                    int HrefDirEnd = data.IndexOf("\">", HrefBegin + 1);

                    if (HrefBegin == -1)
                        break;

                    string FullHref = sub(data, HrefBegin, HrefEnd);
                    string HrefDir = sub(data, HrefBegin + 9, HrefDirEnd);
                    string HrefText = sub(data, HrefDirEnd + 2, HrefEnd - 4);

                    if (!HrefDir.Contains("../") & (HrefDir.EndsWith(extension) | extension == "*"))
                        Files.Add(HrefDir);
                }
            }
            return Files;
        }
    }
}

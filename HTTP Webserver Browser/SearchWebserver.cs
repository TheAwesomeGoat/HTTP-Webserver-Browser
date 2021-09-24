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
        Dictionary<string, string> dataDict = new Dictionary<string, string>();

        private string sub(string str, int begin, int end)
        {
            return str.Substring(begin, end - begin);
        }

        private string Connect(string url)
        {
            using (WebClient client = new WebClient())
                return client.DownloadString(url);
        }
        struct Href
        {
            public string FullHref;
            public string HrefDir;
            public string HrefText;
        }
        private List<Href> FindHrefs(string data)
        {
            int HrefBegin = 0;
            var Hrefs = new List<Href>();
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

                Hrefs.Add(new Href
                {
                    FullHref = FullHref,
                    HrefDir = HrefDir,
                    HrefText = HrefText
                });
            }
            return Hrefs;
        }

        private string ContainsData(string url)
        {
            string data = "";
            if (!dataDict.ContainsKey(url))
            {
                data = Connect(url);
                dataDict.Add(url, data);
            }
            else
                data = dataDict[url];
            return data;
        }

        public List<string> SearchFolders(string url)
        {
            var Folders = new List<string>();
            string data = ContainsData(url);
            if (data != null)
            {
                var Hrefs = FindHrefs(data);
                foreach (var href in Hrefs)
                    if (!href.HrefDir.Contains("../") & href.HrefDir.EndsWith("/"))
                        Folders.Add(href.HrefDir);
            }
            return Folders;
        }
        public List<string> SearchFiles(string url, string extension)
        {
            var Files = new List<string>();
            string data = ContainsData(url);
            if (data != null)
            {
                var Hrefs = FindHrefs(data);
                foreach (var href in Hrefs)
                    if (!href.HrefDir.Contains("../") & (href.HrefDir.EndsWith(extension) | extension == "*"))
                        Files.Add(href.HrefDir);
            }
            return Files;
        }


        List<string> SearchAllFolders_List = new List<string>();
        public List<string> SearchAllFolders(string url)
        {
            SearchAllFolders_List.Clear();
            var Folders = SearchFolders(url);
            foreach (var Folder in Folders)
                LoadSubDirs(url,Folder);
            return SearchAllFolders_List;
        }
        public void LoadSubDirs(string url, string dir)
        {
            SearchAllFolders_List.Add(dir);
            var Folders = SearchFolders($"{url}{dir}");
            foreach (var Folder in Folders)
                LoadSubDirs(url, Folder);
        }
    }
}

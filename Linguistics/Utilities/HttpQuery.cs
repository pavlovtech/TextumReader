using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Linguistics.Utilities
{
    public static class HttpQuery
    {
        public async static Task<string> Make(string query, CookieContainer cookies = null, string method = "GET")
        {
            string Out = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(query);
            req.Method = method;
            req.CookieContainer = cookies;

            var resp = await req.GetResponseAsync();

            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            sr = new StreamReader(stream);
            Out = sr.ReadToEnd();
            sr.Close();

            return Out;
        }
    }
}
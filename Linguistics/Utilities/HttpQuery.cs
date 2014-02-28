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
        public static async Task<string> Get(string query)
        {
            string Out = "";
            WebRequest req = WebRequest.Create(query);

            WebResponse resp = await req.GetResponseAsync();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            Out = sr.ReadToEnd();
            sr.Close();

            return Out;
        }
    }
}
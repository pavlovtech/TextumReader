using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using HtmlAgilityPack;
using Linguistics.Dictionary;
using Newtonsoft.Json.Linq;
using Form = System.Windows.Forms.Form;

namespace LingvoDictApp
{
    public static class HttpQuery
    {
        public static string post(string query, CookieContainer cookies, string method)
        {
            string Out = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(query);
            req.Method = method;
            req.CookieContainer = cookies;

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            sr = new StreamReader(stream);
            Out = sr.ReadToEnd();
            sr.Close();

            return Out;
        }

        public static string Post(string query)
        {
            var cookies = new CookieContainer();

            string result = post(query, cookies, "POST");

            result = post("https://ankiweb.net/edit/", cookies, "POST");

            string startPattern = "editor.models = ";
            string endPattern = ";\neditor.decks = ";
            var jsonModels = findJSON(result, startPattern, endPattern);

            JArray models = JArray.Parse(jsonModels);
            var cards = models.Select(x => new
            {
                Name = x.Value<string>("name"),
                Id = x.Value<string>("id"),
                FieldCount = x["flds"].Count()
            });

            startPattern = "editor.decks = ";
            endPattern = ";\neditor.curModelID = ";
            var jsonDecks = findJSON(result, startPattern, endPattern);

            JObject decks = JObject.Parse(jsonDecks);
            var deckNames = decks.Properties()
                .Select(x => x.Value)
                .Select(i => i.Value<string>("name"));

            string json = "[[\"test\",\"test\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"],\"\"]";
            string encodedData = HttpUtility.UrlEncode(json);
            string url = "https://ankiweb.net/edit/save?data=" + encodedData + "&mid=1367109759778&deck=Current";

            result = post(url, cookies, "POST");

            return result;
        }

        private static string findJSON(string htmlPage, string startPattern, string endPattern)
        {
            int startIndex = htmlPage.IndexOf(startPattern) + startPattern.Length;
            int endIndex = htmlPage.IndexOf(endPattern);

            string jsonModels = htmlPage.Substring(startIndex, endIndex - startIndex);
            return jsonModels;
        }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text =
                HttpQuery.Post(
                    "https://ankiweb.net/account/login?submitted=1&username=astralonavt%40gmail.com&password=3864eb");
        }
    }
}
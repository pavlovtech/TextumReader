using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Linguistics.Utilities;
using System.Web;
using Newtonsoft.Json.Linq;

namespace Linguistics.Anki
{
    public class AnkiWeb
    {
        private CookieContainer cookie = new CookieContainer();
        private bool isAutorized = false;

        public AnkiWeb(string login, string password)
        {
            Login = login;
            Password = password;

            Autorize();

            isAutorized = true;
        }

        public string Login { get; set; }
        public string Password { get; set; }

        public async Task<IEnumerable<Card>> GetCards()
        {
            AutorizationCheck();

            string htmlPage = await HttpQuery.Make("https://ankiweb.net/edit/", cookie, "POST");

            string startPattern = "editor.models = ";
            string endPattern = ";\neditor.decks = ";
            var jsonModels = findJSON(htmlPage, startPattern, endPattern);

            JArray models = JArray.Parse(jsonModels);
            IEnumerable<Card> cards = models.Select(x => new Card()
            {
                Name = x.Value<string>("name"),
                Id = x.Value<string>("id"),
                FieldCount = x["flds"].Count()
            });

            return cards;
        }

        public async Task<IEnumerable<string>> GetDecks()
        {
            AutorizationCheck();

            string htmlPage = await HttpQuery.Make("https://ankiweb.net/edit/", cookie, "POST");

            string startPattern = "editor.decks = ";
            string endPattern = ";\neditor.curModelID = ";
            var jsonDecks = findJSON(htmlPage, startPattern, endPattern);

            JObject decks = JObject.Parse(jsonDecks);
            IEnumerable<string> deckNames = decks.Properties()
                .Select(x => x.Value)
                .Select(i => i.Value<string>("name"));

            return deckNames;
        }

        public  async void AddWord(string word, string translation, string deckName, string cardId)
        {
            AutorizationCheck();

            Card card = (await GetCards()).SingleOrDefault(x => x.Id == cardId);
            if(card == null)
                throw new Exception("The card with the id of " + cardId + " doesn't exist");

            string fields = String.Format("[\"{0}\",\"{1}\",", word, translation);
            for (int i = 0; i < card.FieldCount-2; i++)
            {
                fields += "\"\"";
            }
            fields += "]";

            string json = String.Format("[{0},\"\"]", fields);
            string encodedData = HttpUtility.UrlEncode(json);

            string url = String.Format("https://ankiweb.net/edit/save?data{0}=&mid={1}&deck={2}", encodedData, card.Id, deckName);

            string result = await HttpQuery.Make(url, cookie, "POST");
        }

        private void AutorizationCheck()
        {
            if (isAutorized != true)
                throw new Exception("You are not autorized");
        }

        public void Autorize()
        {
            string url = String.Format("https://ankiweb.net/account/login?submitted=1&username={0}&password={1}",
                HttpUtility.UrlEncode(Login), Password);

            HttpQuery.Make(url, cookie, "POST");

            isAutorized = true;
        }

        private string findJSON(string htmlPage, string startPattern, string endPattern)
        {
            int startIndex = htmlPage.IndexOf(startPattern) + startPattern.Length;
            int endIndex = htmlPage.IndexOf(endPattern);

            string jsonModels = htmlPage.Substring(startIndex, endIndex - startIndex);
            return jsonModels;
        }
    }
}
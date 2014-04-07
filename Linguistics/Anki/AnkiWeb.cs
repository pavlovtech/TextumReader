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
        public bool IsAutorized { get; private set; }

        public AnkiWeb()
        {
            IsAutorized = false;
        }

        public AnkiWeb(string login, string password)
        {
            IsAutorized = false;

            Login = login;
            Password = password;

            Autorize();

            IsAutorized = true;
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

        public async Task<bool> AddWord(string word, string translation, string deckName, string cardId)
        {
            AutorizationCheck();

            var cards = await GetCards();

            Card card = cards.SingleOrDefault(x => x.Id == cardId);
            if(card == null)
                throw new Exception("The card with the id of " + cardId + " doesn't exist");

            string fields = String.Format("[\"{0}\",\"{1}\",", word, translation);
            for (int i = 0; i < card.FieldCount-2; i++)
            {
                if (i == card.FieldCount - 3)
                    fields += "\"\"";
                else
                    fields += "\"\",";
            }
            fields += "]";

            string json = String.Format("[{0},\"\"]", fields);
            string encodedData = HttpUtility.UrlEncode(json);

            string url = String.Format("https://ankiweb.net/edit/save?data={0}&mid={1}&deck={2}", encodedData, card.Id, deckName);

            string result = await HttpQuery.Make(url, cookie, "POST");

            if(result != "2")
                return false;
            
            return true;
        }

        private void AutorizationCheck()
        {
            if (IsAutorized != true)
                throw new Exception("You are not autorized");
        }

        public async Task<bool> Autorize()
        {
            string url = String.Format("https://ankiweb.net/account/login?submitted=1&username={0}&password={1}",
                HttpUtility.UrlEncode(Login), Password);

            string result = await HttpQuery.Make(url, cookie, "POST");
            if (result.Contains("Incorrect username/email or password"))
            {
                return false;;
            }

            IsAutorized = true;
            return true;
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
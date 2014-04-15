using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LemmaSharp;
using Linguistics.Dictionary;
using Linguistics.Models;
using TextumReader.DataLayer.Abstract;
using TextumReader.DataLayer.Concrete;
using TextumReader.ProblemDomain;
using TextumReader.WebUI.Extensions;
using TextumReader.WebUI.Models;

namespace TextumReader.WebUI.Controllers
{
    [Authorize]
    public class DictionaryAPIController : ApiController
    {
        private readonly DictionaryManager _dictionaryManager = new DictionaryManager();
        
        private readonly IGenericRepository _repository;

        public DictionaryAPIController(IGenericRepository repository)
        {
            _repository = repository;
        }

        // localhost:4766/api/DictionaryAPI?word=community&dictionaryId=24&inputLang=english&outputLang=russian
        public async Task<AggregateWordTranslation> GetTranslations(string word, int dictionaryId, string inputLang, string outputLang)
        {
            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);;

            string lemma = _dictionaryManager.GetLemmatization(word, inputLang.ToEnum<Language>());

            var translations = await _dictionaryManager.GetTranslations(lemma,
                    inputLang.ToEnum<Language>(),
                    outputLang.ToEnum<Language>());
            
            if (!translations.Any())
            {
                translations = await _dictionaryManager.GetTranslations(word,
                    inputLang.ToEnum<Language>(),
                    outputLang.ToEnum<Language>());
            }
            else
            {
                word = lemma;
            }
            
            Word foundWord = dict.Words.FirstOrDefault(w => w.WordName == word);

            var savedTranslations = GetSavedTranslationsFromDB(foundWord);

            int wordFrequencyIndex = GetWordFrequencyIndexFromDB(word, inputLang.ToEnum<Language>());
            string shortendFreqView = GetShortendFrequencyView(wordFrequencyIndex);

            string audio = await Make(string.Format("http://apifree.forvo.com/action/word-pronunciations/format/json/word/{0}/language/en/order/date-desc/limit/1/key/444c6f26cd5e1d526f27ff00a7775f3e/", word));

            var translation = new AggregateWordTranslation()
            {
                SavedTranslations = savedTranslations,
                Translations = translations.Except(savedTranslations),
                Word = word,
                WordFrequency = shortendFreqView
            };

            return translation;
        }

        public void AddWord(string word, string translation, string lang, int dictionaryId)
        {
            word = _dictionaryManager.GetLemmatization(word, lang.ToEnum<Language>());

            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);
            Word foundWords = dict.Words.FirstOrDefault(w => w.WordName == word);

            if (foundWords != null)
            {
                if (foundWords.Translations.Any(t => t.Value == translation))
                    return;

                foundWords.Translations.Add(new Translation() { WordId = foundWords.WordId, Value = translation });
            }
            else
            {
                Word newWord = new Word() { WordName = word, DictionaryId = dictionaryId };
                newWord.AddDate = DateTime.Now;

                _repository.Add<Word>(newWord);

                newWord.Translations.Add(new Translation() { Value = translation });
            }
            _repository.SaveChanges();
        }

        private static IEnumerable<string> GetSavedTranslationsFromDB(Word foundWord)
        {
            IEnumerable<string> savedTranslations = Enumerable.Empty<string>();

            if (foundWord != null)
                savedTranslations = foundWord.Translations.Select(t => t.Value);
            return savedTranslations;
        }

        private static string GetShortendFrequencyView(int wordFrequencyIndex)
        {
            string shortendFreqView;
            if (wordFrequencyIndex != 0)
                shortendFreqView = ((int) Math.Ceiling(wordFrequencyIndex/1000.0)).ToString() + "k";
            else
                shortendFreqView = "";
            return shortendFreqView;
        }

        private int GetWordFrequencyIndexFromDB(string word, Language lang)
        {
            int wordFrequencyIndex = 0;
            if (lang == Language.English)
            {
                var wordFreq = _repository.GetSingle<WordFrequency>(x => x.Word == word);
                if (wordFreq != null)
                    wordFrequencyIndex = wordFreq.Position;
            }
            else
            {
                wordFrequencyIndex = 0;
            }
            
            return wordFrequencyIndex;
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using LemmaSharp;
using Linguistics.Dictionary;
using Linguistics.Models;
using TextumReader.DataLayer.Abstract;
using TextumReader.ProblemDomain;
using TextumReader.WebUI.Extensions;
using TextumReader.WebUI.Models;

namespace TextumReader.WebUI.Controllers
{
    public class DictionaryAPIController : ApiController
    {
        readonly IWebDictionary _webDictionary;
        private readonly IGenericRepository _repository;

        private LanguagePrebuilt currentLanguage = LanguagePrebuilt.English;
        private ILemmatizer lmtz;

        public DictionaryAPIController(IGenericRepository repository, IWebDictionary webDictionary)
        {
            _repository = repository;
            _webDictionary = webDictionary;
        }

        // GET: api/DictionaryAPI/hello/english/russian
        public async Task<AggregateWordTranslation> Get(string word, int dictionaryId, string inputLanguage, string outputLanguage)
        {
            if (currentLanguage != inputLanguage.ToEnum<LanguagePrebuilt>())
            {
                lmtz = new LemmatizerPrebuiltCompact(inputLanguage.ToEnum<LanguagePrebuilt>());
            }

            var normalizedWord = lmtz.Lemmatize(word);

            Dictionary dict = _repository.GetSingle<Dictionary>(d => d.DictionaryId == dictionaryId);
            Word foundWord = dict.Words.FirstOrDefault(w => w.WordName == word);

            var translations = await _webDictionary.GetTranslations(word,
                    inputLanguage.ToEnum<Language>(),
                    outputLanguage.ToEnum<Language>());

            var translation = new AggregateWordTranslation()
            {
                SavedTranslations = foundWord.Translations.Select(t => t.Value).ToArray(),
                Translations = translations.Translations,
                Word = word,
                NormalisedWord = normalizedWord
            };

            return translation;
        }

        // GET: api/DictionaryAPI/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/DictionaryAPI
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/DictionaryAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/DictionaryAPI/5
        public void Delete(int id)
        {
        }
    }
}

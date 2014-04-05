using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linguistics.Models;
using Linguistics.Utilities;
using Newtonsoft.Json.Linq;
using LemmaSharp;

namespace Linguistics.Dictionary
{
    public class GoogleTranslate: IWebDictionary
    {
        private ILemmatizer lmtz;

        private string getLangCode(Language lang)
        {
            var type = typeof(Language);
            var info = type.GetMember(lang.ToString());
            var attributes = info[0].GetCustomAttributes(typeof(LanguageСodeAttribute), false);
            string languageCode = ((LanguageСodeAttribute)attributes[0]).Language.ToString();

            return languageCode;
        }


        public async Task<WordTranslations> GetTranslations(string word, Language inputLang, Language outputLang)
        {
            LanguagePrebuilt derivedLanguage;
            LemmaSharp.LanguagePrebuilt.TryParse<LanguagePrebuilt>(inputLang.ToString(), true, out derivedLanguage);
            lmtz = new LemmatizerPrebuiltCompact(derivedLanguage);

            string lemma = lmtz.Lemmatize(word.Trim().ToLower());

            string url = String.Format("http://translate.google.com/translate_a/t?client=t&sl={0}&tl={1}&hl={0}&sc=2&ie=UTF-8&oe=UTF-8&ssel=0&tsel=0&q={2}",
                getLangCode(inputLang), getLangCode(outputLang), lemma);

            string jsonData = "";
            try
            {
                jsonData = await HttpQuery.Make(url);
            }
            catch (Exception ex)
            {
                throw new Exception("Http connection problem", ex);
            }

            var translations = parseTranslations(lemma, jsonData);

            return new WordTranslations() { Translations = translations, WordName = lemma };
        }

        private string[] parseTranslations(string word, string jsonData)
        {
            JArray jsonArray = JArray.Parse(jsonData);
            var translationsSource = jsonArray[1].Select(x => x[1]);

            var translations = new List<string>();
            if (!translationsSource.Any())
            {
                string translation = jsonArray[0].Select(x => x[0]).FirstOrDefault().ToString();
                translations.Add(translation);
                return translations.ToArray();
            }

            foreach (var t in translationsSource)
            {
                translations.AddRange(t.Take(2).Select(x => x.ToString()));
            }

            return translations.ToArray();
        }
    }
}

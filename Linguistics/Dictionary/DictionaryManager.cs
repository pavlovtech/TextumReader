using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LemmaSharp;
using Linguistics.Models;

namespace Linguistics.Dictionary
{
    public class DictionaryManager
    {
        // TODO: use ninject for binding
        private IWebDictionary webDictionary = new GoogleTranslate();
        private LanguagePrebuilt currentLanguage = LanguagePrebuilt.English;
        private ILemmatizer lmtz;

        public DictionaryManager()
        {
            lmtz = new LemmatizerPrebuiltCompact(currentLanguage);
        }

        public async Task<IEnumerable<string>> GetTranslations(string word, Language inputLang, Language outputLang)
        {
            IEnumerable<string> translations = await webDictionary.GetTranslations(word, inputLang, outputLang);

            return translations;
        }

        public string GetLemmatization(string word, Language lang)
        {
            LanguagePrebuilt derivedLanguage;
            Enum.TryParse<LanguagePrebuilt>(lang.ToString(), true, out derivedLanguage);

            if (currentLanguage != derivedLanguage)
                lmtz = new LemmatizerPrebuiltCompact(derivedLanguage);

            string lemma = lmtz.Lemmatize(word.Trim().ToLower());

            // German specific
            if (lang == Language.German)
                lemma = SaveFirstLetterUppercaseIfNeeded(word, lemma);

            return lemma;
        }

        private string SaveFirstLetterUppercaseIfNeeded(string word, string lemma)
        {
            if (char.IsUpper(word[0]))
            {
                lemma = lemma.Remove(0, 1);
                lemma = lemma.Insert(0, word[0].ToString().ToUpper());
            }
            return lemma;
        }

    }
}

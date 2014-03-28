using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linguistics.Utilities;
using Newtonsoft.Json.Linq;
using LemmaSharp;

namespace Linguistics.Dictionary
{
    public class GoogleTranslate: IWebDictionary
    {
        ILemmatizer lmtz = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);

        public async Task<WordTranslation> GetTranslation(string word, Lang inputLang, Lang outputLang)
        {
            string lemma = lmtz.Lemmatize(word.Trim().ToLower());

            string url = String.Format("http://translate.google.com/translate_a/t?client=t&sl={0}&tl={1}&hl={0}&sc=2&ie=UTF-8&oe=UTF-8&ssel=0&tsel=0&q={2}", inputLang, outputLang, lemma);

            string result = "";
            try
            {
                result = HttpQuery.Make(url);
                //JArray a = JArray.Parse(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Http connection problem", ex);
            }

            return getTranslations(lemma, result);
        }

        private WordTranslation getTranslations(string word, string data)
        {
            var list = data.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            string[] allWords;

            if(word != "true" && word != "false")
                allWords = data.Split(new string[] { "[", "]", ",", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "\"", "\\", ".", "true", "false", "!" }, StringSplitOptions.RemoveEmptyEntries);
            else
                allWords = data.Split(new string[] { "[", "]", ",", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "\"", "\\", ".", "!" }, StringSplitOptions.RemoveEmptyEntries);

            int amountToTake = 2;

            string[] result = new string[] { allWords[0] };

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].Contains("verb"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("noun"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("particle"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("adjective"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("adverb"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
                if (list[i].Contains("preposition"))
                {
                    result = result.Concat(SplitIntoWords(list[++i], amountToTake)).ToArray();
                }
            }

            return new WordTranslation() { Translations = result.Distinct().ToArray(), WordName = word };
        }

        private static string[] SplitIntoWords(string data, int amountToTake)
        {
            return data.Split(new char[] { ',', '"', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(amountToTake).ToArray();
        }
    }
}

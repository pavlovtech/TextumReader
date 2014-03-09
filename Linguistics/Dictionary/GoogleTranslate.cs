using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linguistics.Utilities;

namespace Linguistics.Dictionary
{
    public class GoogleTranslate: IDictionary
    {
        public async Task<WordTranslation> GetTranslation(string word, string translationDirection)
        {
            string url = "http://translate.google.com/translate_a/t?client=t&sl=en&tl=ru&hl=en&sc=2&ie=UTF-8&oe=UTF-8&ssel=0&tsel=0&q=" +
                         word;

            string result = "";
            try
            {
                result = await HttpQuery.Get(url);
            }
            catch (Exception ex)
            {
                throw new Exception("Http connection problem", ex);
            }

            return getTranslations(result);
        }

        private WordTranslation getTranslations(string data)
        {
            var list = data.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            var allWords = data.Split(new string[] { "[", "]", ",", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "\"", "\\", "true", "false" }, StringSplitOptions.RemoveEmptyEntries);

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

            string normalFormOfWord = allWords[allWords.Length - 1];

            if (normalFormOfWord.Length == 2)
            {
                normalFormOfWord = allWords[1];
            }

            return new WordTranslation() { Translations = result.Distinct().ToArray(), WordName = normalFormOfWord };
        }

        private static string[] SplitIntoWords(string data, int amountToTake)
        {
            return data.Split(new char[] { ',', '"', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .Take(amountToTake).ToArray();
        }
    }
}

using System.Threading.Tasks;
using Linguistics.Models;

namespace Linguistics.Dictionary
{
    public interface IWebDictionary
    {
        Task<WordTranslations> GetTranslations(string word, Language inputLang, Language outputLang, bool lemmatization = true);
    }
}

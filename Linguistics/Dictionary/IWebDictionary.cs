using System.Threading.Tasks;

namespace Linguistics.Dictionary
{
    public interface IWebDictionary
    {
        Task<WordTranslation> GetTranslation(string word, Lang inputLang, Lang outputLang);
    }
}

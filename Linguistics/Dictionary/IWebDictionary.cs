using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Linguistics.Models;

namespace Linguistics.Dictionary
{
    public interface IWebDictionary
    {
        Task<IEnumerable<string>> GetTranslations(string word, Language inputLang, Language outputLang);
    }
}

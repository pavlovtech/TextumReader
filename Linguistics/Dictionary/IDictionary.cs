using System.Threading.Tasks;

namespace Linguistics.Dictionary
{
    public interface IDictionary
    {
        Task<WordTranslation> GetTranslation(string word, string translationDirection);
    }
}

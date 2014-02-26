namespace Linguistics.Dictionary
{
    public interface IDictionary
    {
        WordTranslation GetTranslation(string word, string translationDirection);
    }
}

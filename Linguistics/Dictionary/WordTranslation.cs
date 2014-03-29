using System.Collections.Generic;

namespace Linguistics.Dictionary
{
    public class WordTranslation
    {
        public string WordName { get; set; }
        public int WordFrequencyIndex { get; set; }
        public string[] Translations { get; set; }
    }
}

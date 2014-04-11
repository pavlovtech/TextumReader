using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextumReader.WebUI.Models
{
    public class AggregateWordTranslation
    {
        public string Word { get; set; }
        public string NormalisedWord { get; set; }
        public ICollection<string> SavedTranslations { get; set; }
        public ICollection<string> Translations { get; set; }

        public int WordFrequencyIndex { get; set; }
    }
}
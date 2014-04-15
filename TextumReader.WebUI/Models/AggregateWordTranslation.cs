using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextumReader.WebUI.Models
{
    public class AggregateWordTranslation
    {
        public string Word { get; set; }
        public string AudioUrl { get; set; }
        public IEnumerable<string> SavedTranslations { get; set; }
        public IEnumerable<string> Translations { get; set; }
        public string WordFrequency { get; set; }
    }
}
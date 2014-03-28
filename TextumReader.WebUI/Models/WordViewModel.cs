using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextumReader.WebUI.Models
{
    public class WordViewModel
    {
        public int WordId { get; set; }
        public int DictionaryId { get; set; }

        public string WordName { get; set; }

        public ICollection<string> Translations { get; set; }
        public bool IsSelected { get; set; }
    }
}
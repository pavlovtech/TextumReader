using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Models
{
    public class WordViewModel
    {
        public int WordId { get; set; }
        public int DictionaryId { get; set; }

        public string WordName { get; set; }

        public DateTime AddDate { get; set; }

        public ICollection<Translation> Translations { get; set; }
        public bool IsSelected { get; set; }
    }
}
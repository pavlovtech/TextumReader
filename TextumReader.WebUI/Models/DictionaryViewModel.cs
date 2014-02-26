using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Models
{
    public class DictionaryViewModel
    {
        public int MaterialId { get; set; }
        public Dictionary CurrentDictionary { get; set; }
        public IEnumerable<Dictionary> Dictionaries { get; set; }
    }
}
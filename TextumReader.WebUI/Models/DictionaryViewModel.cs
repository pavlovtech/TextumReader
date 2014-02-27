using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Models
{
    public class DictionaryViewModel
    {
        public int MaterialId { get; set; }
        public Dictionary CurrentDictionary { get; set; }
        public IEnumerable<SelectListItem> Dictionaries { get; set; }
    }
}
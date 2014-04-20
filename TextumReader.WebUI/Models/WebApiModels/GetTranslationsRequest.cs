using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TextumReader.WebUI.Models.WebApiModels
{
    public class GetTranslationsRequest
    {
        public string word { get; set; }
        public string lang { get; set; }
        public int dictionaryId { get; set; }
    }
}
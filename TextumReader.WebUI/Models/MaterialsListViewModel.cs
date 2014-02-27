using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TextumReader.ProblemDomain;

namespace TextumReader.WebUI.Models
{
    public class MaterialsListViewModel
    {
        public IEnumerable<MaterialViewModel> Materials { get; set; }
        public string CurrentCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TextumReader.ProblemDomain
{
    public class Material
    {
        public int MaterialId { get; set; }
        public int CategoryId { get; set; }
        public int? DictionaryId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string ForeignText { get; set; }
        public string NativeText { get; set; }

        public virtual Category Category { get; set; }
        public virtual Dictionary Dictionary { get; set; }
    }
}

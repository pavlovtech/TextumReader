using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linguistics.Models;

namespace TextumReader.ProblemDomain
{
    public class SharedMaterial
    {
        public int SharedMaterialId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public Language InputLanguage { get; set; }
        public Language OutputLanguage { get; set; }
    }
}

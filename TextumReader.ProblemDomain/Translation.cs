using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace TextumReader.ProblemDomain
{
    public class Translation
    {
        public int TranslationId { get; set; }

        public int WordId { get; set; }
        public string Value { get; set; }

        public virtual Word Word { get; set; }
    }
}

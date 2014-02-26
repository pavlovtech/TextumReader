using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextumReader.ProblemDomain
{
    public class Word
    {
        public Word()
        {
            Translations = new List<Translation>();
        }

        public int WordId { get; set; }
        public int DictionaryId { get; set; }

        public string WordName { get; set; }

        public virtual Dictionary Dictionary { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }
}

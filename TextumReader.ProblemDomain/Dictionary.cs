using System.Collections.Generic;

namespace TextumReader.ProblemDomain
{
    public class Dictionary
    {
        public int DictionaryId { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Word> Words { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}

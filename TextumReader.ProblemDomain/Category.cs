using System.Collections.Generic;

namespace TextumReader.ProblemDomain
{ 
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Material> Materials { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TextumReader.ProblemDomain
{
    public class User
    {
        public User()
        {
            Materials = new List<Material>();
        }

        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Material> Materials { get; set; }
    }
}

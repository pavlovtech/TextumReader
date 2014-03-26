using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextumReader.ProblemDomain
{
    public class AnkiUser
    {
        public int AnkiUserId { get; set; }
        public string UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string DeckName { get; set; }
        public string CardId { get; set; }
    }
}

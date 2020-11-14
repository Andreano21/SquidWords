using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class PersonalDictionary
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Dictionary Dictionary { get; set; }
        public List<PersonalWord> PersonalWords { get; set; }
    }
}

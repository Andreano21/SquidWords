using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class PersonalDictionary
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Dictionary Dictionary { get; set; }
        public List<PersonalWord> PersonalWords { get; set; }

        public PersonalDictionary()
        {
        }

        public PersonalDictionary(Dictionary dictionary)
        {
            Dictionary = dictionary;
            PersonalWords = new List<PersonalWord>();

            var words = Dictionary.Words.OrderBy(w => w.Position);

            foreach (Word w in words)
            {
                PersonalWords.Add(new PersonalWord(w, UserId));
            }
        }
    }
}

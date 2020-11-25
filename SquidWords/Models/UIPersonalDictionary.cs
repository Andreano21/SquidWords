using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class UIPersonalDictionary
    {
        public int PersonalDictionaryId { get; set; }
        public string Name { get; set; }
        public List<PersonalWord> PersonalWords { get; set; }

        public UIPersonalDictionary(int personalDictionaryId, string name, List<PersonalWord> personalWords)
        {
            PersonalDictionaryId = personalDictionaryId;
            Name = name;
            PersonalWords = personalWords;
        }
    }
}

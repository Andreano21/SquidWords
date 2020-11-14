using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class PersonalWord
    {
        public int Id { get; set; }
        public Word Word { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public DateTime LastUse { get; set; }
        public DateTime NextUse { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class PersonalWord
    {
        [NotMapped]
        private static int[] TimeStep = new int[] { 1, 3, 7, 30 };

        public int Id { get; set; }
        public int UserId { get; set; }
        public Word Word { get; set; }
        public DateTime? LastUse { get; set; }
        public DateTime? NextUse { get; set; }

        private int score;
        public int Score 
        { 
            get { return score; }
            set 
            {
                if(value == 0)
                {
                    LastUse = null;
                    NextUse = null;
                    score = value;
                }
                if(value >= 5)
                {
                    NextUse = null;
                    score = value;
                }
                if(value > score)
                {
                    LastUse = DateTime.UtcNow;
                    NextUse = DateTime.UtcNow.AddDays(TimeStep[score]);
                    score = value;
                }
            } 
        }

        public PersonalWord()
        {
        }

        public PersonalWord(Word word, int UserId)
        {
            Word = word;
            Score = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class PersonalTimetableInfo
    {
        public int ForTodayCount { get; set; }
        public int StartedCount { get; set; }
        public int PlannedCount { get; set; }

        public PersonalTimetableInfo()
        {
            ForTodayCount = 0;
            StartedCount = 0;
            PlannedCount = 0;
        }
    }
}

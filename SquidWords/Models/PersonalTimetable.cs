using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class PersonalTimetable
    {
        //Получение частичного персонального словаря содержащим слова для изучения сегодня.
        public List<UIPersonalDictionary> ForToday { get; set; }

        //Получение частичного персонального словаря содержащим слова к изучению которых уже пристпули.
        public List<UIPersonalDictionary> Started { get; set; }

        //Получение частичного персонального словаря содержащим слова к изучению которых еще не пристпули.
        public List<UIPersonalDictionary> Planned { get; set; }

        public PersonalTimetable()
        {
            ForToday = new List<UIPersonalDictionary>();
            Started = new List<UIPersonalDictionary>();
            Planned = new List<UIPersonalDictionary>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class Dictionary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public int AuthorId { get; set; }
        public bool IsPublic { get; set; }
        public Language SourceLanguage { get; set; }
        public Language TargetLanguage { get; set; }
        public List<Word> Words { get; set; }
        public int Rating { get; set; }
        public int UsersCount { get; set; }
        public string ImgUrl { get; set; }
    }
}

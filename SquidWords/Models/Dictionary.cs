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
        public Language SourceLanguage { get; set; }
        public Language TargetLanguage { get; set; }
        public List<Word> Words { get; set; }
        public string AuthorId { get; set; }
        public string ImgUrl { get; set; }
    }
}

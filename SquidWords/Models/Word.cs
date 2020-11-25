using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SquidWords.Models
{
    public class Word
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public int DictionaryId { get; set; }
        public string WordOrigin { get; set; }
        public string WordTranscription { get; set; }
        public string WordTranslate { get; set; }
        public string VoiceUrl { get; set; }
        public string ImgUrl { get; set; }
    }
}

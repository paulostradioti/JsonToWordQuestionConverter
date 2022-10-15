using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtratorTexto.Models
{
    internal class ChoicesQuestionModel : BaseQuestionModel
    {
        public string SKU { get; set; }
        public int Interpreter { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public string[] TagsOrConcepts { get; set; }
        public string[] Text { get; set; }
        public Alternative[] Alternatives { get; set; }

        public class Alternative
        {
            public string Text { get; set; }
            public string Feedback { get; set; }
            public bool Correct { get; set; }
        }

    }
}

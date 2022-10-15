namespace ExtratorTexto.Models
{
    public class GapsQuestionModel : BaseQuestionModel
    {
        public string Sku { get; set; }
        public int Interpreter { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public object[] TagsOrConcepts { get; set; }
        public string[] Text { get; set; }
        public Alternative[] Alternatives { get; set; }
        public Gap[] Gaps { get; set; }
    }

    public class Alternative
    {
        public string Value { get; set; }
        public string Hint { get; set; }
    }

    public class Gap
    {
        public string Expected { get; set; }
        public Feedback[] Feedbacks { get; set; }
    }

    public class Feedback
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

}

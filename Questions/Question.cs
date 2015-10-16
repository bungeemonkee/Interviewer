
namespace Interviewer.Questions
{
    public class Question
    {
        public string Category { get; protected set; }
        public string QuestionText { get; protected set; }
        public string Hint { get; protected set; }
        public string AnswerText { get; protected set; }

        public Question(string category, string text, string hint, string answer)
        {
            Category = category;
            QuestionText = text;
            Hint = hint;
            AnswerText = answer;
        }

        public override bool Equals(object obj)
        {
            var q = obj as Question;
            if (q == null) return false;
            return QuestionText == q.QuestionText;
        }

        public override int GetHashCode()
        {
            return QuestionText.GetHashCode();
        }
    }
}

using System.Collections.Generic;

namespace Interviewer.Questions
{
    public interface IQuestionSource
    {
        IEnumerable<string> AllCategories { get; }
        IEnumerable<Question> AllQuestions { get; }
        IReadOnlyDictionary<string, IReadOnlyList<Question>> AllQuestionsByCategory { get; }

        IEnumerable<Question> GetRandomQuestionsByCategory(string category, int max);
    }
}

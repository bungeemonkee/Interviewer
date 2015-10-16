using System;
using System.Collections.Generic;
using System.Linq;

namespace Interviewer.Questions
{
    public abstract class BaseQuestionSource : IQuestionSource
    {
        private readonly Random _random;

        public IEnumerable<string> AllCategories { get { return AllQuestionsByCategory.Keys; } }
        public IEnumerable<Question> AllQuestions { get { return AllQuestionsByCategory.Values.SelectMany(x => x); } }
        public IReadOnlyDictionary<string, IReadOnlyList<Question>> AllQuestionsByCategory { get; private set; }

        public IEnumerable<Question> GetRandomQuestionsByCategory(string category, int max)
        {
            var questions = (IList<Question>)AllQuestionsByCategory[category];
            if (max >= questions.Count)
            {
                return questions;
            }

            var indecies = new HashSet<int>();
            var result = new List<Question>(max);

            var index = _random.Next(questions.Count);
            while (result.Count != max)
            {
                // get a new unique random question index
                while (indecies.Contains(index))
                {
                    index = _random.Next(questions.Count);
                }

                // add the next random question
                indecies.Add(index);
                result.Add(questions[index]);
            }

            return result;
        }

        protected BaseQuestionSource()
        {
            _random = new Random();
            AllQuestionsByCategory = new Dictionary<string, IReadOnlyList<Question>>();
        }

        protected void AddQuestion(string category, string text, string hint, string answer)
        {
            if (!AllQuestionsByCategory.ContainsKey(category))
            {
                ((IDictionary<string, IReadOnlyList<Question>>)AllQuestionsByCategory).Add(category, new List<Question>());
            }

            ((IList<Question>)AllQuestionsByCategory[category]).Add(new Question(category, text, hint, answer));
        }
    }
}

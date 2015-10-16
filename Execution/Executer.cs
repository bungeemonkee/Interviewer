using Interviewer.Output;
using Interviewer.Questions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interviewer.Execution
{
    public class Executer : IExecuter
    {
        private readonly IQuestionSource _questions;
        private readonly CateogryToInclude[] _categories;

        public bool IncludeHints { get; set; }
        public bool IncludeAnswers { get; set; }
        public IEnumerable<string> CategoriesOrdered { get { return _categories.Select(c => c.Category); } }

        public Executer(IQuestionSource questions)
        {
            _questions = questions;
            _categories = _questions.AllCategories
                .OrderBy(c => c, StringComparer.InvariantCultureIgnoreCase)
                .Select(c => new CateogryToInclude(c))
                .ToArray();

            IncludeAnswers = true;
            IncludeHints = true;
        }

        public void SetCategoryQuestionCount(int index, int count)
        {
            _categories[index].QuestionCount = count;
        }

        public void OutputQuestionsFromSelectedCategories(IOutput output)
        {
            output.WriteDocumentHeader("Interview Questions");
            foreach (var category in _categories)
            {
                output.WriteSectionHeader(category.Category);
                var questions = _questions.GetRandomQuestionsByCategory(category.Category, category.QuestionCount);
                foreach (var question in questions)
                {
                    output.WriteQuestion(question, IncludeAnswers, IncludeHints);
                }
            }
        }

        public void OutputEveryQuestionFromEachCategory(IOutput output)
        {
            output.WriteDocumentHeader("Interview Questions");
            foreach (var category in CategoriesOrdered)
            {
                output.WriteSectionHeader(category);
                foreach (var question in _questions.AllQuestionsByCategory[category])
                {
                    output.WriteQuestion(question, IncludeAnswers, IncludeHints);
                }
            }
        }
    }
}

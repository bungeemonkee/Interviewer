
using System.Collections.Generic;
using Interviewer.Output;

namespace Interviewer.Execution
{
    public interface IExecuter
    {
        bool IncludeHints { get; set; }
        bool IncludeAnswers { get; set; }
        IEnumerable<string> CategoriesOrdered { get; }
        void SetCategoryQuestionCount(int index, int count);
        void OutputQuestionsFromSelectedCategories(IOutput output);
        void OutputEveryQuestionFromEachCategory(IOutput output);
    }
}

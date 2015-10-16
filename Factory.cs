using Interviewer.Execution;
using Interviewer.Output;
using Interviewer.Questions;

namespace Interviewer
{
    public class Factory
    {
        public IQuestionSource GetQuestionSource(string sourceFile)
        {
            return new XmlQuestionSource(sourceFile);
        }

        public IOutput GetOutput(string outputFile)
        {
            return new TextFileOutput(outputFile);
        }

        public IExecuter GetExecuter(IQuestionSource questions)
        {
            return new Executer(questions);
        }
    }
}

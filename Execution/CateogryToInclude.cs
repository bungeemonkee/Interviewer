
namespace Interviewer.Execution
{
    public class CateogryToInclude
    {
        public string Category { get; private set; }
        public int QuestionCount { get; set; }

        public CateogryToInclude(string category)
        {
            Category = category;
        }
    }
}

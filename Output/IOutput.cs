using Interviewer.Questions;
using System;

namespace Interviewer.Output
{
    public interface IOutput : IDisposable
    {
        void WriteDocumentHeader(string title);
        void WriteSectionHeader(string category);
        void WriteQuestion(Question question, bool includeAnswer, bool includeHint);
    }
}

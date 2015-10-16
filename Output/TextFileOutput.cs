using System;
using System.Collections.Generic;
using Interviewer.Questions;
using System.IO;

namespace Interviewer.Output
{
    public class TextFileOutput : IOutput
    {
        private const int Width = 80;

        private readonly FileStream _stream;
        private readonly TextWriter _writer;

        public TextFileOutput(string output)
        {
            _stream = new FileStream(output, FileMode.Create, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(_stream);
        }

        public void Dispose()
        {
            _writer.Dispose();
            _stream.Dispose();
        }

        public void WriteDocumentHeader(string title)
        {
            _writer.WriteLine(title);
            _writer.WriteLine();
            _writer.WriteLine("Candidate: ________________________________________");
            _writer.WriteLine();
            _writer.WriteLine();
        }

        public void WriteSectionHeader(string category)
        {
            _writer.WriteLine();
            _writer.WriteLine("Category: " + category);
            _writer.WriteLine("--------------------------------------------------------------------------------");
            _writer.WriteLine();
        }

        public void WriteQuestion(Question question, bool includeAnswer, bool includeHint)
        {
            var q = ForceWrapWithIndentAfterFirstLine(question.QuestionText, Width - 4, "    ");
            _writer.WriteLine("Q:  " + q);

            if (includeHint && !string.IsNullOrWhiteSpace(question.Hint))
            {
                var h = ForceWrapWithIndentAfterFirstLine(question.Hint, Width - 4, "    ");
                _writer.WriteLine("H:  " + h);
            }

            if (includeAnswer)
            {
                var a = ForceWrapWithIndentAfterFirstLine(question.AnswerText, Width - 4, "    ");
                _writer.WriteLine("A:  " + a);
            }

            _writer.WriteLine();
            _writer.WriteLine("Response:");

            _writer.WriteLine();
            _writer.WriteLine();
            _writer.WriteLine();
            _writer.WriteLine();
        }

        private static string ForceWrapWithIndentAfterFirstLine(string text, int width, string indent)
        {
            IList<string> lines = new List<string>();

            for (var i = 0; i < text.Length; ++i)
            {
                var lineWidth = width;
                if (lines.Count > 0)
                {
                    lineWidth -= indent.Length;
                }
                var line = GetNextLine(text, i, lineWidth, out i);
                if (lines.Count > 0)
                {
                    line = indent + line;
                }
                lines.Add(line);
            }

            return string.Join(Environment.NewLine, lines);
        }

        private static string GetNextLine(string text, int start, int width, out int lastNonWhitespace)
        {
            var firstNonWhitespace = -1;
            lastNonWhitespace = start + width;
            for (var i = start; i < start + width && i < text.Length; ++i)
            {
                if (firstNonWhitespace == -1 && !Char.IsWhiteSpace(text[i]))
                {
                    firstNonWhitespace = i;
                }

                if (i > start && Char.IsWhiteSpace(text[i]) && !Char.IsWhiteSpace(text[i -1]))
                {
                    lastNonWhitespace = i - 1;
                }
                
                if (i == text.Length - 1 && !Char.IsWhiteSpace(text[i - 1]))
                {
                    lastNonWhitespace = i;
                }
            }

            return text.Substring(firstNonWhitespace, (lastNonWhitespace - firstNonWhitespace) + 1);
        }
    }
}

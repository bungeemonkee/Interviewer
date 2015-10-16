
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Interviewer.Questions
{
    /// <summary>
    /// Pulls questions from an xml file.
    /// </summary>
    /// <remarks>
    /// The xml file needs to look like this:
    /// <![CDATA[
    /// <?xml version="1.0"?>
    /// <categories>
    ///     <category name="Category 1">
    ///         <question>
    ///             <text>
    ///                 What is the color blue?
    ///             </text>
    ///             <hint>
    ///                 Nonsense
    ///             </hint>
    ///             <answer>
    ///                 Polkadots!
    ///             </answer>
    ///         </question>
    ///     </category>
    /// </categories>
    /// ]]>
    /// </remarks>
    public class XmlQuestionSource : BaseQuestionSource
    {
        private static readonly Regex Whitespace = new Regex(@"\s+");

        public XmlQuestionSource(string sourceFile)
        {
            var doc = XDocument.Load(sourceFile);
            foreach (var category in doc.Descendants("category"))
            {
                var name = category.Attribute("name").Value;
                foreach (var question in category.Descendants("question"))
                {
                    var text = question.Descendants("text").First().Value;
                    text = CollapseString(text);

                    var hintNode = question.Descendants("hint").FirstOrDefault();
                    var hint = hintNode != null
                        ? hintNode.Value
                        : null;
                    hint = CollapseString(hint);

                    var answer = question.Descendants("answer").First().Value;
                    answer = CollapseString(answer);

                    AddQuestion(name, text, hint, answer);
                }
            }
        }

        private static string CollapseString(string text)
        {
            return text == null
                ? string.Empty
                : Whitespace.Replace(text, " ").Trim();
        }
    }
}

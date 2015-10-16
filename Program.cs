using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Interviewer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            // find the source and destination files
            var folder = Path.GetDirectoryName(typeof (Program).Assembly.Location);
            var source = Path.Combine(folder, "Questions.xml");
            var destination = Path.Combine(folder, "Questions.txt");

            // tell the user what's going to happen
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Reading: {0}", source);
            Console.WriteLine("Writing: {0}", destination);

            // setup the list
            var factory = new Factory();
            var questions = factory.GetQuestionSource(source);
            var executer = factory.GetExecuter(questions);

            // generate the questions
            var all = false;
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Type g or generate to generate the question list.");
                Console.WriteLine("Type e or exit to exit without generating the list.");
                Console.WriteLine("Type i or include to toggle answers in the output on or off.");
                Console.WriteLine("Type a or all to generate a list with all available questions.");
                Console.WriteLine("Select a category (by number) to choose how many questions:");
                foreach (var c in executer.CategoriesOrdered.Select((c, i) => string.Format("  {0:00}: {1}", i, c)))
                {
                    // write out each category with it's index
                    Console.WriteLine(c);
                }

                // get the user's input
                var input = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();

                // exit
                if (input == "e" || input == "exit") return;

                // done setting up - generate the output
                if (input == "g" || input == "generate") break;

                // toggle answers in the output
                if (input == "i" || input == "include")
                {
                    executer.IncludeAnswers = !executer.IncludeAnswers;
                    executer.IncludeHints = executer.IncludeAnswers;

                    Console.WriteLine(executer.IncludeAnswers
                        ? "Answers will be included in the output."
                        : "Answers will not be included in the output.");

                    continue;
                }

                // output everything
                if (input == "a" || input == "all")
                {
                    all = true;
                    break;
                }

                // parse the input into a category number
                int category;
                if (!int.TryParse(input, out category))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("PEBKAC: Not a number.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                // get the category from the number
                if (category < 0 || category >= questions.AllQuestionsByCategory.Count)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("PEBKAC: Invalid category number.");
                    Console.ForegroundColor = ConsoleColor.Green;
                    continue;
                }

                // get the number of questions to include for this category
                Console.WriteLine("Enter the number of questions from this category:");
                input = Console.ReadLine() ?? string.Empty;
                int count;
                if (int.TryParse(input, out count))
                {
                    executer.SetCategoryQuestionCount(category, count);
                    Console.WriteLine("Using {1} questions from category {0:00}.", category, count);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("PEBKAC: Not a number.");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }

            // generate the output
            using (var output = factory.GetOutput(destination))
            {
                if (all)
                {
                    executer.OutputEveryQuestionFromEachCategory(output);
                }
                else
                {
                    executer.OutputQuestionsFromSelectedCategories(output);
                }
            }

            // open the results file for editing if desired
            Process.Start(destination);
        }
    }
}

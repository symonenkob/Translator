using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Translator_1.RPN
{
    internal class PriorityRow
    {
        public string[] Lexems { get; set; }
        public int StackPriority { get; set; }
        public int ComparisonPriority { get; set; }
    }

    static class PriorityStorage
    {
        public static Dictionary<int, List<string>> Priorities { get; set; }
        public static List<PriorityRow> PriorityRows { get; set; }

        static PriorityStorage()
        {
            Priorities = new Dictionary<int, List<string>>();
            PriorityRows = ParseFile();
        }

        private static List<PriorityRow> ParseFile()
        {
            List<PriorityRow> priorityRows = new List<PriorityRow>();

            using (StreamReader file = new StreamReader(Environment.CurrentDirectory+@"\Priorities.txt"))
            {
                string line;
                bool result = true;

                while ((line = file.ReadLine()) != null)
                {
                    string[] splittedLine = line.Split(':');
                    if (splittedLine.Count() != 3 && !string.IsNullOrWhiteSpace(line))
                    {
                        result = false;
                        break;
                    }

                    string[] lexems = splittedLine[0].Split(',');
                    if (lexems.Count() == 0)
                    {
                        result = false;
                        break;
                    }

                    int stackPriority, comparisonPriority;
                    result = int.TryParse(splittedLine[1], out stackPriority);
                    result = int.TryParse(splittedLine[2], out comparisonPriority);

                    if (!result)
                        throw new FileFormatException("Failed to parse file");

                    PriorityRow priorityRow = new PriorityRow()
                    {
                        Lexems = lexems,
                        StackPriority = stackPriority,
                        ComparisonPriority = comparisonPriority
                    };
                    priorityRows.Add(priorityRow);
                }
                if (!result)
                    throw new FileFormatException("Failed to parse file");
            }
            return priorityRows;
        }

        public static PriorityRow GetLexemPriority(string lexem)
        {
            return PriorityRows.FirstOrDefault(pr => pr.Lexems.Contains(lexem));
        }
    }
}

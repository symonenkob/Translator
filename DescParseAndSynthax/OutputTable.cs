using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1
{
    public class OutputTable
    {
        public String OutputText { get; set; }
        public List<OutputRow> OutputRows = new List<OutputRow>();
        public Dictionary<int, string> Variables = new Dictionary<int, string>();
        public Dictionary<int, int> Constants = new Dictionary<int, int>();

        public void AddToOutputTable(int row, string lexem, int lexemeCode, int? IdConIndex = null)
        {
            OutputRows.Add(new OutputRow
            {
                Row = row,
                SubString = lexem,
                LexemeCode = lexemeCode,
                IdConTableIndex = IdConIndex
            });
        }

        public String GetOutputText()
        {
            int prevRow = 1;
            String outputText = null;
            foreach (OutputRow outputRow in OutputRows)
            {
                if (outputRow.Row == prevRow)
                {
                    outputText += outputRow.LexemeCode + " ";
                }
                else
                {
                    outputText += "\n" + outputRow.LexemeCode + " ";
                    prevRow += 1;
                }
            }
            return outputText;
        }

        public List<string> GetLexemsOnly(bool withIdConNames = false)
        {
            List<string> result = new List<string>();
            foreach (var outputRow in OutputRows)
            {
                if (!withIdConNames && outputRow.LexemeCode == 14)
                {
                    result.Add("id");
                }
                else if (!withIdConNames && outputRow.LexemeCode == 15)
                {
                    result.Add("con");
                }
                else
                {
                    result.Add(outputRow.SubString);
                }
            }
            return result;
        }

        public List<string> GetIds()
        {
            List<string> result = new List<string>();
            foreach (var outputRow in OutputRows)
            {
                if (outputRow.LexemeCode == 14)
                {
                    if (result.FirstOrDefault(r => r == outputRow.SubString) == null)
                        result.Add(outputRow.SubString);
                }
            }
            return result;
        }

        public OutputRow NextRow(OutputRow curRow)
        {
            return OutputRows[OutputRows.IndexOf(curRow) + 1];
        }
    }
}

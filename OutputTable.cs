using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1
{
    class OutputTable
    {
        public String OutputText { get; set; }
        public List<OutputRow> OutputRows = new List<OutputRow>();
        public Dictionary<int, string> Variables = new Dictionary<int, string>();
        public Dictionary<int, int> Constants = new Dictionary<int, int>();

        public void AddToOutputTable(int row, string lexem, int lexemeCode, int? IdConIndex=null)
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
            String outputText=null;
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

        public OutputRow NextRow(OutputRow curRow)
        {
            return OutputRows[OutputRows.IndexOf(curRow) + 1];
        }
    }
}

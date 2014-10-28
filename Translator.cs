using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Translator_1
{
    static class Translator
    {
        public static List<char> Characters = new List<char>();
        public static List<char> Digits = new List<char>();
        public static List<char> SingleSeparators = new List<char>();
        public static char EqualsSymbol = '=';
        public static char LessSymbol = '<';
        public static char GreaterSymbol = '>';
        public static char ExclamationSymbol = '!';

        public static Dictionary<int, string> Lexems = new Dictionary<int, string>();

        static Translator()
        {
            for (char letter = 'a'; letter < 'z'; letter++)
                Characters.Add(letter);

            for (int i = 0; i < 10; i++)
                Digits.Add(i.ToString()[0]);

            SingleSeparators.Add('{');
            SingleSeparators.Add('}');
            SingleSeparators.Add('(');
            SingleSeparators.Add(')');
            SingleSeparators.Add(',');
            SingleSeparators.Add(';');
            SingleSeparators.Add('*');
            SingleSeparators.Add('/');
            SingleSeparators.Add('+');
            SingleSeparators.Add('-');
            SingleSeparators.Add('^');

            Lexems.Add(1, "pr");
            Lexems.Add(2, "{");
            Lexems.Add(3, "}");
            Lexems.Add(4, "int");
            Lexems.Add(5, "read");
            Lexems.Add(6, "write");
            Lexems.Add(7, "do");
            Lexems.Add(8, "to");
            Lexems.Add(9, "by");
            Lexems.Add(10, "while");
            Lexems.Add(11, "if");
            Lexems.Add(12, "then");
            Lexems.Add(13, "end");
            Lexems.Add(14, "id");
            Lexems.Add(15, "con");
            Lexems.Add(16, ";");
            Lexems.Add(17, ",");
            Lexems.Add(18, "+");
            Lexems.Add(19, "-");
            Lexems.Add(20, "*");
            Lexems.Add(21, "/");
            Lexems.Add(22, "(");
            Lexems.Add(23, ")");
            Lexems.Add(24, ">");
            Lexems.Add(25, ">=");
            Lexems.Add(26, "<");
            Lexems.Add(27, "<=");
            Lexems.Add(28, "==");
            Lexems.Add(29, "!=");
            Lexems.Add(30, "=");
            Lexems.Add(31, "^");
        }

        public static string DoLexTranslate(String filteredText, ref OutputTable outputTable)
        {
            int curRow = 1, curCol = 0;

            outputTable = new OutputTable();
            string outputText = null;

            int charsToRow;
            ErrorCode resultCode;
            int curCharIndex = 0;
            while (curCharIndex < filteredText.Length)
            {
                charsToRow = CountCharsToRow(filteredText, curRow);
                curCharIndex = charsToRow + curCol;

                if (filteredText[curCharIndex] == '\n')
                {
                    curRow++;
                    curCol = 0;
                    outputText += Environment.NewLine;

                    charsToRow = CountCharsToRow(filteredText, curRow);
                    curCharIndex = charsToRow + curCol;
                    if (curCharIndex >= filteredText.Length) break;
                }
                if (filteredText[curCharIndex] != '\n' && filteredText[curCharIndex] != '\r' && filteredText[curCharIndex] != ' ')
                {
                    resultCode = GetNextLexem(filteredText.Substring(curCharIndex, filteredText.Length - charsToRow - curCol), outputTable, curRow);

                    if (resultCode != ErrorCode.Ok)
                    {
                        return ErrorHandler.GetErrorDescription(resultCode, outputTable.OutputRows.Last().SubString,
                            curRow, curCol + 1);
                    }

                    /*if (Lexems.ContainsValue(nextLexem))
                        outputText += Lexems.FirstOrDefault(l => l.Value == nextLexem).Key.ToString() + ' ';
                    else if (outputTable.Variables.ContainsValue(nextLexem))
                    {
                        outputText += Lexems.FirstOrDefault(l => l.Value == "id").Key.ToString() + ' ';
                    }
                    else
                    {
                        int constantCode;
                        bool isFirstDigit = false;
                        isFirstDigit = int.TryParse(nextLexem, out constantCode);
                        if (outputTable.Constants.ContainsValue(constantCode) && isFirstDigit)
                        {
                            outputText += Lexems.FirstOrDefault(l => l.Value == "con").Key.ToString() + ' ';
                        }
                        else
                        {
                            if(nextLexem.Length>=1 && char.IsLetter(nextLexem[0]))
                                return ErrorHandler.GetErrorDescription(ErrorCode.LexUnknownIdentifierFound, nextLexem, curRow, curCol+1);
                            else if (nextLexem.Length == 1 && !char.IsDigit(nextLexem[0]))
                            {
                                return ErrorHandler.GetErrorDescription(ErrorCode.LexUnknownLexemFound, nextLexem, curRow,
                                    curCol+1);
                            }
                            else
                            {
                                return ErrorHandler.GetErrorDescription(ErrorCode.UnknownError, nextLexem, curRow,
                                    curCol+1);
                            }
                        }
                    }*/
                    /*if (Lexems.ContainsValue(nextLexem))//state 2
                    {
                        outputTable.OutputRows.Add(new OutputRow
                        {
                            Row = curRow,
                            SubString = nextLexem,
                            LexemeCode = Lexems.FirstOrDefault(l => l.Value == nextLexem).Key
                        });
                    }
                    else
                    {
                        int charsToThisRow = CountCharsToRow(filteredText, curRow);
                        string firstLexemInRow = GetNextLexem(filteredText.Substring(charsToThisRow,
                            filteredText.Length - charsToRow - curCol), outputTable, curRow);
                        if (firstLexemInRow.ToLower() == "int")
                        {
                            if (outputTable.Variables != null)
                                outputTable.Variables.Add(outputTable.Variables.Count + 1, nextLexem);
                            else
                                outputTable.Variables.Add(1, nextLexem);
                        }
                        else
                        {
                            return ErrorHandler.GetErrorDescription(ErrorCode.LexUnknownLexemFound, nextLexem, curRow,
                           curCol + 1);
                        }
                    }*/

                    curCol += outputTable.OutputRows.Last().SubString.Length;
                }
                else curCol++;

                charsToRow = CountCharsToRow(filteredText, curRow);
                curCharIndex = charsToRow + curCol;
            }

            outputText = outputTable.GetOutputText();

            return outputText;
        }

        public static ErrorCode GetNextLexem(String text, OutputTable outputTable, int curRow)
        {
            string lexem = null;
            int i = 0;

            if (char.IsLetter(text[0]))
            {
                while (i < text.Length && char.IsLetterOrDigit(text[i]))
                {
                    lexem += text[i];
                    i++;
                }
                if (Lexems.ContainsValue(lexem))
                {
                    outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
                    if (lexem == "int" && curRow != 2) return ErrorCode.LexInvalidVariableDeclaration;
                }
                else
                {
                    if (!outputTable.Variables.ContainsValue(lexem))
                    {
                        outputTable.Variables.Add(outputTable.Variables.Count + 1, lexem);
                        outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == "id").Key,
                            outputTable.Variables.Count);
                        if (curRow != 2) return ErrorCode.LexInvalidVariableDeclaration;
                    }
                    else
                    {
                        outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == "id").Key,
                           outputTable.Variables.FirstOrDefault(v => v.Value == lexem).Key);
                    }
                }
            }
            else if (char.IsDigit(text[0]))
            {
                while (i < text.Length && char.IsLetterOrDigit(text[i]))
                {
                    lexem += text[i];
                    if (char.IsLetter(text[i]))
                    {
                        outputTable.AddToOutputTable(curRow, lexem, 0);
                        return ErrorCode.LexInvalidIdentificator;
                    }
                    i++;
                }

                int value;
                int.TryParse(lexem, out value);
                if (outputTable.Constants.ContainsValue(value))
                {
                    outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == "con").Key, outputTable.Constants.FirstOrDefault(c => c.Value == value).Key);
                }
                else
                {
                    outputTable.Constants.Add(outputTable.Constants.Count + 1, value);

                    outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == "con").Key, outputTable.Constants.Count);
                }
            }
            else if (SingleSeparators.Contains(text[0]))
            {
                lexem = text[0].ToString();
                outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
            }
            else if (text[0] == '=')
            {
                lexem += text[0];
                if (text[1] == '=')
                    lexem += '=';
                outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
            }
            else if (text[0] == '<')
            {
                lexem += text[0];
                if (text[1] == '=')
                    lexem += '=';
                outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
            }
            else if (text[0] == '>')
            {
                lexem += text[0];
                if (text[1] == '=')
                    lexem += '=';
                outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
            }
            else if (text[0] == '!')
            {
                lexem += text[0];
                if (text[1] == '=')
                    lexem += '=';
                outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
            }
            else if (text[0] == '^')
            {
                lexem += text[0];
                outputTable.AddToOutputTable(curRow, lexem, Lexems.FirstOrDefault(l => l.Value == lexem).Key);
            }
            else
            {
                outputTable.AddToOutputTable(curRow, text[0].ToString(), 0);
                return ErrorCode.LexUnknownLexemFound;
            }
            return ErrorCode.Ok;
        }

        public static int CountCharsToRow(String text, int row)
        {
            int chars = 0;
            int curRow = 1;
            foreach (char c in text)
            {
                if (curRow == row)
                    break;
                if (c == '\n')
                {
                    curRow++;
                }
                chars++;
            }
            return chars;
        }

        public static String DoSynthaxTranslate(OutputTable outputTable)
        {
            String outputText = null;

            // try
            {

                if (outputTable == null) return ErrorHandler.GetErrorDescription(ErrorCode.UnknownError, "", 0, 0);
                int curLexemIndex = 0;
                OutputRow curLexem = outputTable.OutputRows[curLexemIndex++];
                if (curLexem.LexemeCode == Lexems.First(l => l.Value == "pr").Key)
                {
                    curLexem = outputTable.OutputRows[curLexemIndex++];
                    if (curLexem.LexemeCode == Lexems.First(l => l.Value == "{").Key)
                    {
                        curLexem = outputTable.OutputRows[curLexemIndex];
                        if (VariableDeclarationCheck(outputTable, ref curLexemIndex))
                        {
                            curLexem = outputTable.OutputRows[curLexemIndex];
                            if (OperatorsListCheck(outputTable, ref curLexemIndex))
                            {
                                curLexem = outputTable.OutputRows[curLexemIndex];
                                if (curLexem.LexemeCode == Lexems.First(l => l.Value == "}").Key)
                                {

                                }
                                else outputText = ErrorHandler.GetErrorDescription(ErrorCode.SynUnexpectedEnd, curLexem.SubString, curLexem.Row);
                            }
                            else
                                outputText = ErrorHandler.GetErrorDescription(ErrorCode.SynUnexpectedEnd, curLexem.SubString, curLexem.Row);
                        }
                        else
                            outputText = ErrorHandler.GetErrorDescription(ErrorCode.SynInvalidDeclaration, null, curLexem.Row);

                    }
                }

            }
            //catch (ArgumentOutOfRangeException e)
            {
                // outputText = "Program doesn't suits a template";
            }
            return outputText;
        }

        public static bool VariableDeclarationCheck(OutputTable outputTable, ref int curLexemIndex)
        {
            bool result = false;
            bool finished = false;
            try
            {
                OutputRow curLexem = outputTable.OutputRows[curLexemIndex++];
                if (curLexem.LexemeCode == Lexems.First(l => l.Value == "int").Key)
                {
                    curLexem = outputTable.OutputRows[curLexemIndex++];
                    result = true;

                    while (curLexem.LexemeCode == Lexems.First(l => l.Value == "id").Key && !finished && result)
                    {
                        curLexem = outputTable.OutputRows[curLexemIndex++];
                        if (curLexem.SubString == ",")
                        {
                            curLexem = outputTable.OutputRows[curLexemIndex++];
                        }
                        else if (curLexem.SubString == ";")
                        {
                            curLexem = outputTable.OutputRows[curLexemIndex];
                            finished = true;
                        }
                        else
                            result = false;
                    }
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                result = false;
            }

            if (finished && result)
                return true;
            else return false;
        }

        public static bool OperatorsListCheck(OutputTable outputTable, ref int curLexemIndex)
        {
            bool result = false;
            try
            {
                OutputRow curLexem = outputTable.OutputRows[curLexemIndex++];
                if (curLexem.LexemeCode == Lexems.First(l => l.Value == "read" || l.Value == "write").Key)
                {
                    curLexem = outputTable.OutputRows[curLexemIndex++];
                    if (curLexem.SubString == "(")
                    {
                        if (IdentifiersListCheck(outputTable,ref curLexemIndex))
                        {
                            curLexem = outputTable.OutputRows[curLexemIndex++];
                            if (curLexem.SubString == ")")
                            {
                                curLexem = outputTable.OutputRows[curLexemIndex++];
                                if (curLexem.SubString == ";")
                                {
                                    curLexemIndex++;
                                    result = true;
                                }
                            }

                        }
                    }
                    
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                result = false;
            }
            return result;
        }

        public static bool IdentifiersListCheck(OutputTable outputTable, ref int curLexemIndex)
        {
            bool result = false;
            try
            {
                OutputRow curLexem = outputTable.OutputRows[curLexemIndex++];
                if (curLexem.LexemeCode == Lexems.First(l => l.Value == "id").Key)
                {
                    curLexem = outputTable.OutputRows[curLexemIndex++];
                    result = true;

                    while (curLexem.SubString == "," && result)
                    {
                        curLexem = outputTable.OutputRows[curLexemIndex++];
                        if (curLexem.LexemeCode == Lexems.First(l => l.Value == "id").Key)
                        {
                            curLexem = outputTable.OutputRows[curLexemIndex];
                        }
                        else result = false;
                    }
                }
                return result;
            }
            catch (ArgumentOutOfRangeException e)
            {
                result = false;
            }
            return result;
        }
    }
}

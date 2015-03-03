using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator_1
{
    public class Automate
    {
        public List<AutomateRow> Main { get; set; }
        public Stack<int> Stack { get; set; }

        public Automate()
        {
            Stack = new Stack<int>();
            Main = new List<AutomateRow>
            {
                new AutomateRow()
                {
                    Alpha = 0,
                    Label = "pr",
                    Beta = 1,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 1,
                    Label = "{",
                    Beta = 2,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 2,
                    Label = "int",
                    Beta = 3,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 3,
                    Label = "id",
                    Beta = 4,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 4,
                    Label = ";",
                    Beta = 100,
                    StackInput = 5
                },
                new AutomateRow()
                {
                    Alpha = 4,
                    Label = ",",
                    Beta = 3,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 5,
                    Label = ";",
                    Beta = 100,
                    StackInput = 5
                },
                new AutomateRow()
                {
                    Alpha = 5,
                    Label = "}",
                    Beta = -1,
                    StackInput = null
                },

                //operator
            
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "id",
                    Beta = 101,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "read",
                    Beta = 110,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "write",
                    Beta = 110,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "do",
                    Beta = 120,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 100,
                    Label = "if",
                    Beta = 300,
                    StackInput = 141
                },
                new AutomateRow()
                {
                    Alpha = 101,
                    Label = "=",
                    Beta = 200,
                    StackInput = 102
                },
                new AutomateRow()
                {
                    Alpha = 102,
                    Label = "exit",
                    Beta = null,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 110,
                    Label = "(",
                    Beta = 111,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 111,
                    Label = "id",
                    Beta = 112,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 112,
                    Label = ")",
                    Beta = null,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 112,
                    Label = ",",
                    Beta = 111,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 120,
                    Label = "id",
                    Beta = 121,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 121,
                    Label = "=",
                    Beta = 200,
                    StackInput = 122
                },new AutomateRow()
                {
                    Alpha = 122,
                    Label = "to",
                    Beta = 200,
                    StackInput = 123
                },
                new AutomateRow()
                {
                    Alpha = 123,
                    Label = "by",
                    Beta = 200,
                    StackInput = 124
                },
                new AutomateRow()
                {
                    Alpha = 124,
                    Label = "while",
                    Beta = 300,
                    StackInput = 125
                },new AutomateRow()
                {
                    Alpha = 125,
                    Label = "{",
                    Beta = 100,
                    StackInput = 126
                },
                new AutomateRow()
                {
                    Alpha = 126,
                    Label = ";",
                    Beta = 100,
                    StackInput = 126
                },new AutomateRow()
                {
                    Alpha = 126,
                    Label = "}",
                    Beta = null,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 140,
                    Label = "exit",
                    Beta = 300,
                    StackInput = 141
                },
                new AutomateRow()
                {
                    Alpha = 141,
                    Label = "then",
                    Beta = 100,
                    StackInput = 142
                },new AutomateRow()
                {
                    Alpha = 142,
                    Label = "exit",
                    Beta = null,
                    StackInput = null
                },
                //expression

                new AutomateRow()
                {
                    Alpha = 200,
                    Label = "id",
                    Beta = 201,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 200,
                    Label = "con",
                    Beta = 201,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 200,
                    Label = "(",
                    Beta = 200,
                    StackInput = 202
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "+",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "-",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "*",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "/",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "^",
                    Beta = 200,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 201,
                    Label = "exit",
                    Beta = null,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 202,
                    Label = ")",
                    Beta = 201,
                    StackInput = null
                },
                //log expression
                new AutomateRow()
                {
                    Alpha = 300,
                    Label = "[",
                    Beta = 300,
                    StackInput = 303
                },
                new AutomateRow()
                {
                    Alpha = 300,
                    Label = "!",
                    Beta = 300,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 300,
                    Label = "elseChar",//c/
                    Beta = 200,
                    StackInput = 301
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "==",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "!=",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = ">",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = ">=",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "<",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 301,
                    Label = "<=",//c/
                    Beta = 200,
                    StackInput = 302
                },
                new AutomateRow()
                {
                    Alpha = 302,
                    Label = "and",
                    Beta = 300,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 302,
                    Label = "or",
                    Beta = 300,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 302,
                    Label = "exit",
                    Beta = null,
                    StackInput = null
                },
                new AutomateRow()
                {
                    Alpha = 303,
                    Label = "]",
                    Beta = 302,
                    StackInput = null
                },
            };
        }

        public List<AutomateRow> getAutomateRowsByAlpha(int alpha)
        {
            return Main.Where(r => r.Alpha == alpha).ToList();
        }

        public List<AutomateRow> DoAutomateTranslate(List<OutputRow> outputRows)
        {
            List<AutomateRow> curAutomateRows = getAutomateRowsByAlpha(0);

            OutputRow curOutputRow = outputRows[0];
            int curOutputRowIndex = 0;
            int nextBeta;
            int iteration = 0;
            bool endFound = false;

            List<AutomateRow> resultingRows = new List<AutomateRow>();

            while (!endFound && iteration < 2 && curOutputRowIndex < outputRows.Count)
            {
                iteration++;
                
                foreach (var curAutomateRow in curAutomateRows)
                {
                    if (curAutomateRow.Label == Translator.Lexems.First(l => l.Key == curOutputRow.LexemeCode).Value || curAutomateRow.Label == "exit" || curAutomateRow.Label == "elseChar")
                    {
                        if (curOutputRowIndex + 1 == outputRows.Count)
                        {
                            if (curAutomateRow.Beta == -1)
                            {
                                resultingRows.Add(curAutomateRow);
                                return resultingRows;
                            }
                            else return resultingRows;
                        }
                        if (curAutomateRow.Label == "exit")
                        {
                            nextBeta = Stack.Pop();
                        }
                        else
                        {
                            if (curAutomateRow.Beta == null)
                                nextBeta = Stack.Pop();
                            else
                                nextBeta = (int) curAutomateRow.Beta;

                            if (curAutomateRow.StackInput != null)
                                Stack.Push((int) curAutomateRow.StackInput);
                        }

                        curAutomateRows = getAutomateRowsByAlpha(nextBeta);
                        if (curAutomateRow.Beta == -1)
                        {
                            endFound = true;
                            resultingRows.Add(curAutomateRow);
                        }
                        if(curAutomateRow.Label != "elseChar" && curAutomateRow.Label!="exit")
                            curOutputRow = outputRows[++curOutputRowIndex];
                        iteration = 0;
                        curAutomateRow.CurStack = null;

                        foreach (var i in Stack)
                        {
                            curAutomateRow.CurStack += i.ToString() + ' ';
                        }
                        resultingRows.Add(curAutomateRow);
                        break;
                    }
                }
            }
            return resultingRows;
        }

    }
}

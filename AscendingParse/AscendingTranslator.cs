using System.Collections.Generic;
using System.Linq;

namespace Translator_1.AscendingParse
{
    static class AscendingTranslator
    {
        public static Stack<string> Rpn = new Stack<string>();

        public static List<AscOutputRow> Translate(List<string> inputChain, List<string> inputChainWithIdNames = null, bool rpnRequired = false)
        {
            Rpn.Clear();
            bool checker = true;
            bool finished = false;
            List<AscOutputRow> outputRows = new List<AscOutputRow>();

            Stack<string> stack = new Stack<string>();
            int step = 0;

            stack.Push("#");

            do
            {
                string curRelation;
                curRelation = TableConstructor.GetRelation(stack.Peek(), inputChain[0]);

                if (rpnRequired)
                {
                    outputRows.Add(new AscOutputRow()
                    {
                        Step = step,
                        InputChain = string.Join(" ", inputChainWithIdNames),
                        Relation = curRelation,
                        Stack = string.Join(" ", stack.Reverse()),
                        Rpn = string.Join(" ", Rpn.Reverse())
                    });
                }
                else
                {
                    outputRows.Add(new AscOutputRow()
                    {
                        Step = step,
                        InputChain = string.Join(" ", inputChain),
                        Relation = curRelation,
                        Stack = string.Join(" ", stack.Reverse()),
                    });
                }

                switch (curRelation)
                {
                    case ">":
                        {
                            GetMore(ref stack, ref checker, rpnRequired);
                        }
                        break;
                    case "<":
                    case "=":
                        {
                            string nextLexem = inputChain.ElementAt(0);
                            if (rpnRequired)
                            {
                                if (nextLexem == "id" || nextLexem == "con")
                                {
                                    string nextRpnLexem = inputChainWithIdNames[0];
                                    Rpn.Push(nextRpnLexem);
                                }
                                inputChainWithIdNames.RemoveAt(0);
                            }
                            stack.Push(nextLexem);
                            inputChain.RemoveAt(0);
                        }
                        break;
                    default:
                        checker = false;
                        break;
                }

                step++;

                if (inputChain.Count == 0)
                {
                    GetMore(ref stack, ref checker, rpnRequired);
                    outputRows.Add(new AscOutputRow()
                    {
                        Step = step,
                        InputChain = string.Join(" ", inputChain),
                        Relation = curRelation,
                        Stack = string.Join(" ", stack.Reverse())
                    });
                }
            } while ((inputChain.Count > 0) && stack.Peek() != "<пр>" && checker);

            return outputRows;
        }

        private static void GetMore(ref Stack<string> stack, ref bool checker, bool rpnRequired = false)
        {
            string curRelation;
            string topStack;
            string secondStack;

            List<string> newLexems = new List<string>();

            do
            {
                topStack = stack.Pop();
                newLexems.Add(topStack);
                secondStack = stack.Peek();

                curRelation = TableConstructor.GetRelation(secondStack, topStack);
            } while ((curRelation != "<" || (topStack == "<оп>" && secondStack == ";" && curRelation == "<" && stack.ElementAt(1) == "<сп.оп>")) && checker);

            newLexems.Reverse();

            if (rpnRequired)
            {
                if (newLexems.Contains("*"))
                    Rpn.Push("*");
                if (newLexems.Contains("/"))
                    Rpn.Push("/");
                if (newLexems.Contains("+"))
                    Rpn.Push("+");
                if (newLexems.Contains("-"))
                    Rpn.Push("-");
                if (newLexems.Contains("^"))
                    Rpn.Push("^");
            }

            stack.Push(TableConstructor.SearchRule(newLexems, ref checker));
        }


    }
}

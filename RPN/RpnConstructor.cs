using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Translator_1.RPN
{
    class Marks
    {
        public List<Mark> MarksList { get; set; }

        public Mark Add()
        {
            if (MarksList == null)
                MarksList = new List<Mark>();
            Mark mark = new Mark()
            {
                Number = MarksList.Count != 0 ? MarksList.Max(m => m.Number) + 1 : 1,
                Finished = false
            };
            MarksList.Add(mark);

            return mark;
        }

        public Mark Finish(Mark mark)
        {
            MarksList.Find(m => m.Number == mark.Number).Finish();
            return mark;
        }

        public string Finish(string mark)
        {
            string markNumberStr = mark.Substring(1, mark.Length - 1);
            int markNumber;
            bool res = int.TryParse(markNumberStr, out markNumber);
            if (!res)
                throw new ArgumentException("Failde to parse mark: " + mark);
            MarksList.Find(m => m.Number == markNumber).Finish();
            return mark + ":";
        }
    }

    class Mark
    {
        public int Number { get; set; }
        public bool Finished { get; set; }

        public string GetStringValue()
        {
            return Finished ? "m" + Number + ":" : "m" + Number;
        }

        public void Finish()
        {
            Finished = true;
        }
    }

    public static class ExtensionMethods
    {
        public static Stack<T> Clone<T>(this Stack<T> stack)
        {
            Contract.Requires(stack != null);
            return new Stack<T>(new Stack<T>(stack));
        }
    }

    //Dijkstra algorithm
    class RpnConstructor
    {
        private static void SkipMarks(ref Stack<string> stack)
        {
            string topStackElement;
            string markRegExp = "m\\d{1,3}";

            topStackElement = stack.Peek();

            while (Regex.IsMatch(topStackElement, markRegExp)) // check if working
            {
                topStackElement = stack.Pop();
            }
        }

        private static string GetTopStackElement(Stack<string> stack)
        {
            Stack<string> stackCopy = stack.Clone();
            string topStackElement;
            string markRegExp = "m\\d{1,3}";

            topStackElement = stackCopy.Peek();

            while (Regex.IsMatch(topStackElement, markRegExp)) // check if working
            {
                topStackElement = stackCopy.Pop();
            }
            return topStackElement;
        }

        private static List<RpnRow> ConstructIfExpression(List<OutputRow> inputChain)
        {
            int step = 1;
            List<RpnRow> outputRows = new List<RpnRow>();
            Stack<string> stack = new Stack<string>();
            Stack<string> rpnList = new Stack<string>();
            Marks marks = new Marks();

            bool finished = false;
            OutputRow curLexem;
            PriorityRow curLexemPriorityRow;
            string markRegExp = "m\\d{1,3}";

            while (!finished)
            {
                curLexem = inputChain[0];

                if (curLexem.SubString == "if")
                {
                    stack.Push(curLexem.SubString);
                }
                else
                {

                    if (curLexem.SubString == ";" || curLexem.SubString == "}")
                    {
                        marks.MarksList.ForEach(m => m.Finish()); // redo later
                        while (stack.Count > 1)
                        {
                            string topStack = stack.Pop();
                            if (Regex.IsMatch(topStack, markRegExp))
                            {
                                topStack = marks.Finish(topStack);
                            }
                            rpnList.Push(topStack);
                        }
                        stack.Pop();
                        finished = true;
                    }
                    else
                    {

                        if (curLexem.IdConTableIndex != null)
                            rpnList.Push(curLexem.SubString);
                        else
                        {
                            curLexemPriorityRow = PriorityStorage.GetLexemPriority(curLexem.SubString);

                            if (curLexemPriorityRow.StackPriority <=
                                PriorityStorage.GetLexemPriority(GetTopStackElement(stack)).StackPriority &&
                                curLexem.SubString != "(")
                            {
                                string topStackElement;
                                PriorityRow topStackElPriorityRow;

                                topStackElement = GetTopStackElement(stack);
                                topStackElPriorityRow = PriorityStorage.GetLexemPriority(topStackElement);

                                while (curLexemPriorityRow.StackPriority <= topStackElPriorityRow.StackPriority)
                                {
                                    stack.Pop();
                                    if (topStackElement != "(" && topStackElement != ")")
                                        rpnList.Push(topStackElement);
                                    topStackElement = stack.Peek();
                                    topStackElPriorityRow = PriorityStorage.GetLexemPriority(topStackElement);

                                    if (topStackElement == "(" && curLexem.SubString == ")")
                                    {
                                        stack.Pop();
                                        topStackElement = GetTopStackElement(stack);
                                        topStackElPriorityRow = PriorityStorage.GetLexemPriority(topStackElement);
                                    }
                                }
                                if (curLexem.SubString != ")")
                                    stack.Push(curLexem.SubString);
                            }
                            else
                            {
                                if (curLexem.SubString == "then")
                                {
                                    Mark mark = marks.Add();
                                    rpnList.Push(mark.GetStringValue());
                                    rpnList.Push("УПЛ");
                                    stack.Push(mark.GetStringValue());
                                }
                                else
                                {
                                    stack.Push(curLexem.SubString);
                                }
                            }
                        }
                    }
                }
                outputRows.Add(new RpnRow()
                {
                    Step = step,
                    InputChain = string.Join(" ", inputChain.Select(i => i.SubString)),
                    Stack = string.Join(" ", stack.Reverse()),
                    Rpn = string.Join(" ", rpnList.Reverse())
                });
                step++;
                inputChain.RemoveAt(0);
            }
            return outputRows;
        }

        private List<RpnRow> ConstructLoopExpression(List<OutputRow> inputChain)
        {
            return null;
        }

        public static List<RpnRow> Construct(List<OutputRow> inputChain)
        {
            return ConstructIfExpression(inputChain);
        }
    }
}
